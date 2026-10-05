using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace DCC_Standard
{

    /// <summary>
    /// Instrumented, logged, error checked, access to ORACLE databases. 
    /// </summary>
    public class DCC_OracleDataLayer : DCC_DataLayer
    {
        OracleDataAdapter ora_da;
        OracleConnection ora_conn;
        OracleDataReader ora_dr;
        /// <summary>
        /// Constructor for link to ORACLE database.
        /// </summary>
        /// <param name="db">Name of the database.</param>
        public DCC_OracleDataLayer(string db)
        {
            newDCC_DataLayer("ORACLE", db, ConfigurationManager.AppSettings["SoftwareIdentityCode"] ?? "MissingAppID");
        }
        /// <summary>
        /// Constructor for link to ORACLE database.
        /// </summary>
        /// <param name="db">Name of the database.</param>
        /// <param name="applicationName">Name of the application.</param>
        public DCC_OracleDataLayer(string db, string applicationName)
        {
            newDCC_DataLayer("ORACLE", db, applicationName);
        }

        protected override bool ConnectDatabase(string connectionString)
        {
            if (ora_conn != null && ora_conn.State == ConnectionState.Open) return true;

            ora_conn = new OracleConnection(connectionString);

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
                lastError = new ErrorReport("0005", "Database close failed", ConnectionStringHelpers.MaskConnectionString(connectionString), "", ErrorLevel.Information);
            }
        }

        protected override string FetchCulture()
        {
            return "Awaiting developer coding effort";
        }

        protected override DataTable DoSchema(string collectionName)
        {
            return ora_conn.GetSchema(collectionName);
        }

        protected override void DoSelect(string spName, SortedList<string, object> paras)
        {
            ora_da = new OracleDataAdapter();
            ora_da.SelectCommand = new OracleCommand(spName, ora_conn);
            ora_da.SelectCommand.CommandTimeout = 240;// GWW changed from ray's original 10
            ora_da.SelectCommand.CommandType = CommandType.StoredProcedure;
            if (spName.StartsWith(directSQLcommandMarker))
            {
                ora_da.SelectCommand.CommandText = spName.Substring(directSQLcommandMarker.Length);
                ora_da.SelectCommand.CommandType = CommandType.Text;
            }

            convertParas(paras, ora_da.SelectCommand);
            buildOracleParameters(ora_da.SelectCommand);

            int tries_remaining = 5;
            while (tries_remaining > 0)
            {
                try
                {
                    ora_da.Fill(result);
                    break;
                }
                catch (OracleException eOracle)
                {
                    if (IsDebugMode) traceFailure(eOracle);
                    if (eOracle.Message.StartsWith("Timeout"))
                    {
                        tries_remaining--;
                    }
                    else
                    {
                        tries_remaining = 0;
                        throw new DCCDataLayerException("Error 1001", "Database error - DATA NOT READ", "", ora_da.SelectCommand, eOracle);
                    }
                }
                catch (TimeoutException /* eTimeOut*/)
                {
                    if (IsDebugMode) traceTimeout();
                    tries_remaining--;
                }
                catch (Exception eSDR)
                {
                    tries_remaining = 0;
                    throw new DCCDataLayerException("Error 1001", "Database error - DATA NOT READ", "", ora_da.SelectCommand, eSDR);
                }
            }
            if (tries_remaining == 0)
            {
                throw new DCCDataLayerException("Error 1002", "Database error - DATABASE BUSY", "", ora_da.SelectCommand, null);
            }
        }

        protected override void coreFillDR(string spName, SortedList<string, object> paras, bool DirectSQL, SortedList<string, object> results)
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

        protected override DbDataReader coreGetDR(string spName, SortedList<string, object> paras, bool DirectSQL, DbDataReader myDR)
        {
            OracleCommand comm = new OracleCommand();
            comm.CommandTimeout = 10;
            comm.Connection = ora_conn;
            comm.CommandText = spName;
            comm.CommandType = (DirectSQL ? CommandType.Text : CommandType.StoredProcedure);

            convertParas(paras, comm);
            buildOracleParameters(comm);

            ora_dr = null;
            try
            {
                int tries_remaining = 5;
                while (tries_remaining > 0)
                {
                    try
                    {
                        ora_dr = comm.ExecuteReader();
                        myDR = ora_dr;
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
                            throw new DCCDataLayerException("Error 1011", "Database error - DATA NOT READ", "", comm, eOracle);
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
                        throw new DCCDataLayerException("Error 1011", "Database error - DATA NOT READ", "", comm, eSDR);
                    }
                }

                if (tries_remaining == 0)
                {
                    throw new DCCDataLayerException("Error 1012", "Database error - DATABASE BUSY", "", comm, null);
                }

            }

            catch (Exception eSDR)
            {
                throw new DCCDataLayerException("Error 1013", "Database error - DATA NOT READ", "", comm, eSDR);
            }

            return myDR;

        }
        protected override void DoNonQuery(string spName, SortedList<string, object> paras, int expectedRows, ref DCC_DB_NonQuery theResult, ref bool hasPK)
        {
            OracleCommand comm = new OracleCommand();
            comm.CommandTimeout = timeOut;
            comm.CommandType = CommandType.StoredProcedure;
            comm.Connection = ora_conn;
            //comm.CommandText = "BEGIN " + spName + "; END;";
            comm.CommandText = spName;
            if (spName.StartsWith(directSQLcommandMarker))
            {
                comm.CommandType = CommandType.Text;
                comm.CommandText = spName.Substring(directSQLcommandMarker.Length);
            }
            hasPK = convertParas(paras, comm);

            buildOracleParameters(comm);

            try
            {
                int tries_remaining = 5;
                while (tries_remaining > 0)
                {
                    try
                    {
                        theResult.returnENQ = comm.ExecuteNonQuery();
                        theResult.rows = -1;
                        if (comm.Parameters.Count > 0)
                        {
							try
							{
								OracleParameter rc = comm.Parameters["RowCount"];
								if (rc == null)
								{
									int z;
									z = 3 + 3;
								}

								if (rc != null) theResult.rows = (int)(rc.Value);
							}
							catch (Exception eRowCount)
							{
								throw new DCCDataLayerException("Error 1038", "Database error", "Missing output parameter 'RowCount' for a Non-Query", comm, eRowCount);
							}
						}
                        break;  // out of while loop
                    }
                    catch (OracleException eOracle)
                    {
                        if (IsDebugMode) traceFailure(eOracle);
                        if ((eOracle.Message ?? "").StartsWith("Timeout"))
                        {
                            tries_remaining--;
                        }
                        else
                        {
                            tries_remaining = 0;
                            eOracle.Source = "ExecuteNonQuery";
                            throw new DCCDataLayerException("Error 1031", "Database error", "", comm, eOracle);
                        }
                    }
                    catch (TimeoutException /* eTimeOut*/)
                    {
                        if (IsDebugMode) traceTimeout();
                        tries_remaining--;
                    }
                    catch (Exception eSDR)
                    {
                        tries_remaining = 0;
                        eSDR.Source = "ExecuteNonQuery";
                        throw new DCCDataLayerException("Error 1031", "Database error", "", comm, eSDR);
                    }
                }

                if (tries_remaining == 0)
                {
                    throw new DCCDataLayerException("Error 1032", "Database error - DATABASE BUSY", "", comm, null);
                }

                if (theResult.rows == expectedRows || expectedRows == -1)
                {
                    theResult.ok = true;
                    if (hasPK)
                    {
                        foreach (OracleParameter op in comm.Parameters)
                        {
                            if ( (op.ParameterName == "Newprimarykey") || (op.ParameterName == "p_new_pk") ) ///////////////////GWW FIDDLE
                            {
                                if (op.Direction == ParameterDirection.Output)
                                {
                                    if (op.Value is DBNull)
                                    {
                                        throw new DCCDataLayerException("Error 1099", "Database error - DATA NOT SAVED", "Newprimarykey has returned a DBNull", comm, null);
                                    }
                                    theResult.newPK = (int)op.Value;
                                }
                            }
                        }
                    }
                }
                else
                {
                    theResult.ok = false;
                }

            }
            catch (Exception eSDR)
            {
                eSDR.Source = "ExecuteNonQuery";
                throw new DCCDataLayerException("Error 1033", "Database error - DATA NOT SAVED", "", comm, eSDR);
            }

        }

		protected override int coreDirectSQL(string sql, int rows)
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

        private void buildOracleParameters(OracleCommand oraC)
        {
            List<OracleParameter> ops = new List<OracleParameter>();
            foreach (DbParameter p in oraC.Parameters)
            {
                OracleParameter op = (OracleParameter)p;
				if (op.ParameterName.StartsWith("!!"))
                {
                    op = new OracleParameter(p.ParameterName.Replace("!!", ""), p.Value);
                    op.Direction = ParameterDirection.Output;
                }
                else if (op.ParameterName.StartsWith("!B!"))
                {
                    op = new OracleParameter(p.ParameterName.Replace("!B!", ""), p.Value);
                }
                else if (op.ParameterName.StartsWith("!C!"))
                {
                    op = new OracleParameter(p.ParameterName.Replace("!C!", ""), p.Value);
                }
                ops.Add(op);
            }
            oraC.Parameters.Clear();
            foreach (OracleParameter op in ops) oraC.Parameters.Add(op);
            if (IsDebugMode) traceParameters(oraC);
        }

        private static void traceParameters(OracleCommand comm)
        {
            DebugTrace.WriteLine(comm.CommandText);
            foreach (OracleParameter p in comm.Parameters) DebugTrace.WriteLine("  " + DCC_Formatting.DatabaseParameter(p));
        }

        private static void traceFailure(OracleException eSQL)
        {
            DebugTrace.WriteLine("SQL Failed at " + DateTime.Now.ToString("HH:mm:ss") + " with message\r\n" + eSQL.Message);
        }

        private static void traceTimeout()
        {
            DebugTrace.WriteLine("Timed Out at " + DateTime.Now.ToString("HH:mm:ss"));
        }

    }
}
