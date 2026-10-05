using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Oracle.DataAccess.Client;

namespace DCC_Standard
{

    /// <summary>
    /// Instrumented, logged, error checked, access to ORACLE databases using Oracle's own ODAC. 
    /// </summary>
    public class DCC_ODAC_DataLayer : DCC_DataLayer
    {
            OracleDataAdapter ora_da;
            OracleConnection ora_conn;
            OracleDataReader ora_dr;  
            /// <summary>
            /// Constructor for link to ORACLE database.
            /// </summary>
            /// <param name="pDatabaseName">Name of the database.</param>
            public DCC_ODAC_DataLayer(string pDatabaseName)
            {
                newDCC_DataLayer("ORACLE", pDatabaseName, System.Configuration.ConfigurationManager.AppSettings["SoftwareIdentityCode"]??"MissingAppID");
            }
            /// <summary>
            /// Constructor for link to ORACLE database.
            /// </summary>
            /// <param name="pDatabaseName">Name of the database.</param>
            /// <param name="pApplicationName">Name of the application.</param>
            public DCC_ODAC_DataLayer(string pDatabaseName, string pApplicationName)
            {
                newDCC_DataLayer("ORACLE", pDatabaseName, pApplicationName);
            }

            protected override bool ConnectDatabase(string pConnectionString)
            {
                if (ora_conn != null && ora_conn.State == ConnectionState.Open) return true;

                ora_conn = new OracleConnection(pConnectionString);

                return false;
            }

            protected override bool OpenDatabase()
            {
                if (ora_conn.State == ConnectionState.Open) { return true; }
                ora_conn.Open();
                if (ora_conn.State == ConnectionState.Open) { return true; }
                return false;
            }

            protected override void CloseDatabase()
            {
                if (ora_conn == null) return;
                if (ora_conn.State == ConnectionState.Closed) return;
                try
                {
                    ora_conn.Close();
                }
                catch
                {
                    lastError = new ErrorReport("0005", "Database close failed", connectionString, "", ErrorLevel.Information);
                }
            }

            protected override string FetchCulture()
            {
                return "Awaiting developer coding effort";
            }

            protected override DataTable DoSchema(string pCollectionName)
            {
                return (ora_conn as OracleConnection).GetSchema(pCollectionName);
            }

            protected override void DoSelect(string pStoredProcedure, SortedList<string, object> pParameters)
            {
                bool quiet = false;
                if (pStoredProcedure.StartsWith(quietMarker))
                {
                    quiet = true;
                    pStoredProcedure = pStoredProcedure.Substring(quietMarker.Length);
                }
                ora_da = new OracleDataAdapter();
                ora_da.SelectCommand = new OracleCommand(pStoredProcedure, ora_conn);
                ora_da.SelectCommand.CommandTimeout=10;
                ora_da.SelectCommand.CommandType = CommandType.StoredProcedure;
                if (pStoredProcedure.StartsWith(directSQLcommandMarker))
                {
                    ora_da.SelectCommand.CommandText = pStoredProcedure.Substring(directSQLcommandMarker.Length);
                    ora_da.SelectCommand.CommandType = CommandType.Text;
                }

                convertParas(pParameters, ora_da.SelectCommand);
                buildOracleParameters(ora_da.SelectCommand,quiet);

                int tries_remaining=5;
                while (tries_remaining > 0)
                {
                    try
                    {
                        ora_da.Fill(result);
                        break;
                    }
                    catch (OracleException eOracle)
                    {
                        traceFailure(eOracle);
                        if (eOracle.Message.StartsWith("Timeout"))
                        {
                            tries_remaining--;
                        }
                        else
                        {
                            tries_remaining = 0;
                            throw new DCCDataLayerException("Error #DB6.1001", "Database error - DATA NOT READ", "", ora_da.SelectCommand, eOracle);
                        }
                    }
                    catch (TimeoutException /* eTimeOut*/)
                    {
                        traceTimeout();
                        tries_remaining--;
                    }
                    catch (Exception eSDR)
                    {
                        tries_remaining = 0;
                        throw new DCCDataLayerException("Error #DB6.1001", "Database error - DATA NOT READ", "", ora_da.SelectCommand, eSDR);
                    }
                }
                if (tries_remaining == 0)
                {
                    throw new DCCDataLayerException("Error #DB6.1002", "Database error - DATABASE BUSY", "", ora_da.SelectCommand, null);
                }
            }

            protected override void coreFillDR(string pStoredProcedure, SortedList<string, object> pParameters, bool pDirectSQL, SortedList<string, object> pResults)
            {
                throw new NotImplementedException();

             //    OracleCommand comm = new OracleCommand();
             //    comm.Connection = ora_conn;
             //    comm.CommandText = SQL;
             //    if (paras != null)
             //        foreach (string para in paras.Keys) comm.Parameters.Add(new OracleParameter(para, paras[para]));

             //    OracleDataReader sdrX = null;
             //    try
             //    {
             //        sdrX = comm.ExecuteReader(CommandBehavior.SingleRow);
             //        if (sdrX.HasRows)
             //        {
             //            sdrX.Read();
             //            for (int f = 0; f < sdrX.FieldCount; f++)
             //            {
             //                results.Add(sdrX.GetName(f), sdrX[f]);
             //            }
             //        }
             //    }
             //    catch (Exception eSDR)
             //    {
             //        StringBuilder sb = new StringBuilder();
             //        foreach (OracleParameter p in comm.Parameters) sb.Append( "\r\n" + p.ParameterName + " = " + p.Value.ToString());
             //        MessageBox.Show("Database error: \r\nSQL was: " + comm.CommandText + "\r\nParams:" + sb.ToString(), eSDR.Message);
             //    }
             //    finally
             //    {
             //        sdrX.Close();
             //    }
            }

            protected override DbDataReader coreGetDR(string pStoredProcedure, SortedList<string, object> pParameters, bool pDirectSQL, DbDataReader pMyDR)
            {
                bool quiet = false;
                if (pStoredProcedure.StartsWith(quietMarker))
                {
                    quiet = true;
                    pStoredProcedure = pStoredProcedure.Substring(quietMarker.Length);
                }
                OracleCommand comm = new OracleCommand();
                comm.CommandTimeout = 10;
                comm.Connection = ora_conn;
                comm.CommandText = pStoredProcedure;
                comm.CommandType = (pDirectSQL ? CommandType.Text : CommandType.StoredProcedure);

                convertParas(pParameters, comm);
                buildOracleParameters(comm,quiet);

                ora_dr = null;
                try
                {
                    int tries_remaining = 5;
                    while (tries_remaining > 0)
                    {
                        try
                        {
                            ora_dr = comm.ExecuteReader();
                            pMyDR = ora_dr;
                            break;
                        }
                        catch (OracleException eOracle)
                        {
                            traceFailure(eOracle);
                            if (eOracle.Message.StartsWith("Timeout"))
                            {
                                tries_remaining--;
                            }
                            else
                            {
                                tries_remaining = 0;
                                throw new DCCDataLayerException("Error #DB6.1011", "Database error - DATA NOT READ", "", comm, eOracle);
                            }
                        }
                        catch (TimeoutException /* eTimeOut*/)
                        {
                            traceTimeout();
                            tries_remaining--;
                        }
                        catch (Exception eSDR)
                        {
                            tries_remaining = 0;
                            throw new DCCDataLayerException("Error #DB6.1011", "Database error - DATA NOT READ", "", comm, eSDR);
                        }
                    }

                    if (tries_remaining == 0)
                    {
                        throw new DCCDataLayerException("Error #DB6.1012", "Database error - DATABASE BUSY", "", comm, null);
                    }

                }

                catch (Exception eSDR)
                {
                    throw new DCCDataLayerException("Error #DB6.1013", "Database error - DATA NOT READ", "", comm, eSDR);
                }

                return pMyDR;

            }
            protected override void DoNonQuery(string pStoredProcedure, SortedList<string, object> pParameters, int pExpectedRows, ref DCC_DB_NonQuery pResult, ref bool pHasPK)
            {
                bool quiet = false;
                if (pStoredProcedure.StartsWith(quietMarker))
                {
                    quiet = true;
                    pStoredProcedure = pStoredProcedure.Substring(quietMarker.Length);
                }
                OracleCommand comm = new OracleCommand();
                comm.CommandTimeout = timeOut;
                comm.CommandType = CommandType.StoredProcedure;
                comm.Connection = ora_conn;
                comm.CommandText = pStoredProcedure;
                if (pStoredProcedure.StartsWith(directSQLcommandMarker))
                {
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = pStoredProcedure.Substring(directSQLcommandMarker.Length);
                }
                pHasPK = convertParas(pParameters, comm);

                buildOracleParameters(comm,quiet);

                try
                {
                    int tries_remaining = 5;
                    while (tries_remaining > 0)
                    {
                        try
                        {
                            pResult.returnENQ = comm.ExecuteNonQuery();
                            pResult.rows = -1;
                            if (comm.Parameters.Count > 0)
                            {
                                OracleParameter rc = comm.Parameters["RowCount"];
                                if (rc != null) pResult.rows = (int)(rc.Value);
                            }
                            break;
                        }
                        catch (OracleException eOracle)
                        {
                            traceFailure(eOracle);
                            if (eOracle.Message.StartsWith("Timeout"))
                            {
                                tries_remaining--;
                            }
                            else
                            {
                                tries_remaining = 0;
                                throw new DCCDataLayerException("Error #DB6.1031", "Database error", "", comm, eOracle);
                            }
                        }
                        catch (TimeoutException /* eTimeOut*/)
                        {
                            traceTimeout();
                            tries_remaining--;
                        }
                        catch (Exception eSDR)
                        {
                            tries_remaining = 0;
                            throw new DCCDataLayerException("Error #DB6.1031", "Database error", "", comm, eSDR);
                        }
                    }
                    if (tries_remaining == 0)
                    {
                        throw new DCCDataLayerException("Error #DB6.1032", "Database error - DATABASE BUSY", "", comm, null);
                    }

                    if (pResult.rows == pExpectedRows || pExpectedRows==-1)
                    {
                        pResult.ok = true;
                        if (pHasPK)
                        {
                            foreach (OracleParameter op in comm.Parameters)
                            {
                                if (op.ParameterName == "Newprimarykey")
                                {
                                    if (op.Direction == ParameterDirection.Output)
                                    {
                                        pResult.newPK = (int)op.Value;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        pResult.ok = false;
                    }

                }
                catch (Exception eSDR)
                {
                    throw new DCCDataLayerException("Error #DB6.1033", "Database error - DATA NOT SAVED", "", comm, eSDR);
                }

            }
            protected override int coreDirectSQL(string pSQL, int pRows)
            {
                throw new NotImplementedException();

            //    OracleCommand comm = new OracleCommand(sql, ora_conn);
            //    try
            //    {
            //        rows = comm.ExecuteNonQuery();
            //    }
            //    catch (Exception eSDR)
            //    {
            //        StringBuilder sb = new StringBuilder();
            //        foreach (OracleParameter p in comm.Parameters) sb.Append("\r\n" + p.ParameterName + " = " + p.Value.ToString());
            //        MessageBox.Show("Database error: \r\nSQL was: " + comm.CommandText + "\r\nParams:" + sb.ToString(), eSDR.Message);
            //    }

            }

            private void buildOracleParameters(OracleCommand pOracleCommand, bool pQuiet)
            {
                List<OracleParameter> ops = new List<OracleParameter>();
                foreach (DbParameter p in pOracleCommand.Parameters)
                {
                    OracleParameter op = (OracleParameter)p;
                    if (op.ParameterName.StartsWith("!!"))
                    {
                        op = new OracleParameter(p.ParameterName.Replace("!!", ""), Oracle.DataAccess.Client.OracleDbType.RefCursor);
                        op.Direction = ParameterDirection.Output;
                    }
                    if (op.ParameterName.StartsWith("!B!"))
                    {
                        op = new OracleParameter(p.ParameterName.Replace("!B!", ""), Oracle.DataAccess.Client.OracleDbType.Blob);
                        op.Value = p.Value;
                    }
                    if (op.ParameterName.StartsWith("!C!"))
                    {
                        op = new OracleParameter(p.ParameterName.Replace("!C!", ""), Oracle.DataAccess.Client.OracleDbType.Clob);
                        op.Value = p.Value;
                    }
                    ops.Add(op);
                }
                pOracleCommand.Parameters.Clear();
                foreach (OracleParameter op in ops) pOracleCommand.Parameters.Add(op);
                traceParameters(pOracleCommand, pQuiet);
            }

            private static void traceParameters(OracleCommand pOracleCommand, bool pQuiet)
            {
#if  (DEBUG)
                if (!pQuiet)
                {
                    DebugTrace.WriteLine(pOracleCommand.CommandText);
                    foreach (OracleParameter p in pOracleCommand.Parameters) DebugTrace.WriteLine("  " + DCC_Formatting.DatabaseParameter(p));
                }
#endif
            }

            private static void traceFailure(OracleException pOracleException)
            {
#if (DEBUG)
                DebugTrace.WriteLine("SQL Failed at " + DateTime.Now.ToString("HH:mm:ss") + " with message\r\n" + pOracleException.Message);
#endif
            }

            private static void traceTimeout()
            {
#if (DEBUG)
                DebugTrace.WriteLine("Timed Out at "+DateTime.Now.ToString("HH:mm:ss"));
#endif
            }

        }
    }
