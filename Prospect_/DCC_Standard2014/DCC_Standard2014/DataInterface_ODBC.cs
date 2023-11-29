using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;

namespace DCC_Standard
{

    public class ODBC_DCC_DataLayer : DCC_DataLayer
    {

        OdbcDataAdapter odbc_da;
        OdbcConnection odbc_conn;
        OdbcDataReader odbc_dr;

        public ODBC_DCC_DataLayer(string pConnectID)
        {
            newDCC_DataLayer("ODBC", pConnectID, System.Configuration.ConfigurationManager.AppSettings["SoftwareIdentityCode"] ?? "MissingAppID");
        }

        public ODBC_DCC_DataLayer(string pConnectID, string pCallingSoftwareID)
        {
            newDCC_DataLayer("ODBC", pConnectID, pCallingSoftwareID);
        }

        protected override bool ConnectDatabase(string pConnectionString)
        {
            if (odbc_conn != null && odbc_conn.State == ConnectionState.Open) return true;

            odbc_conn = new OdbcConnection(pConnectionString);
            return false;
        }

        protected override bool OpenDatabase()
        {
            if (odbc_conn.State == ConnectionState.Open) { return true; }
            odbc_conn.Open();
            if (odbc_conn.State == ConnectionState.Open) { return true; }
            return false;
        }

        protected override void CloseDatabase()
        {
            if (odbc_conn == null) return;
            if (odbc_conn.State == ConnectionState.Closed) return;
            try
            {
                odbc_conn.Close();
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

        protected override DataTable DoSchema(string pCollectionName)
        {
            return odbc_conn.GetSchema(pCollectionName);
        }

        protected override void DoSelect(string pStoredProcedure, SortedList<string, object> pParameters)
        {
            odbc_da = new OdbcDataAdapter();
            odbc_da.SelectCommand = new OdbcCommand(pStoredProcedure, odbc_conn);
            odbc_da.SelectCommand.CommandTimeout = 10;
            odbc_da.SelectCommand.CommandType = CommandType.Text;

            if (IsDebugMode) traceParameters(odbc_da.SelectCommand);

            int tries_remaining = 5;
            while (tries_remaining > 0)
            {
                try
                {
                    odbc_da.Fill(result);
                    break;
                }
                catch (OdbcException eODBC)
                {
                    if (IsDebugMode) traceFailure(eODBC);
                    if (eODBC.Message.StartsWith("Timeout"))
                    {
                        tries_remaining--;
                    }
                    else
                    {
                        tries_remaining = 0;
                        throw new DCCDataLayerException("Error 1001", "Database error - DATA NOT READ", "", odbc_da.SelectCommand, eODBC);
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
                    throw new DCCDataLayerException("Error 1001", "Database error - DATA NOT READ", "", odbc_da.SelectCommand, eSDR);
                }
            }
            if (tries_remaining == 0)
            {
                throw new DCCDataLayerException("Error 1002", "Database error - DATABASE BUSY", "", odbc_da.SelectCommand, null);
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
            OdbcCommand comm = new OdbcCommand();
            comm.CommandTimeout = 10;
            comm.Connection = odbc_conn;
            comm.CommandText = pStoredProcedure;
            comm.CommandType = (pDirectSQL ? CommandType.Text : CommandType.StoredProcedure);
            convertParas(pParameters, comm);

            odbc_dr = null;

            traceParameters(comm);

            try
            {
                int tries_remaining = 5;
                while (tries_remaining > 0)
                {
                    try
                    {
                        odbc_dr = comm.ExecuteReader();
                        pMyDR = odbc_dr;
                        break;
                    }
                    catch (OdbcException eSQL)
                    {
                        traceFailure(eSQL);
                        if (eSQL.Message.StartsWith("Timeout"))
                        {
                            tries_remaining--;
                        }
                        else
                        {
                            tries_remaining = 0;
                            throw new DCCDataLayerException("Error 1011", "Database error - DATA NOT READ", "", comm, eSQL);
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
            return pMyDR;
        }

        protected override void DoNonQuery(string pStoredProcedure, SortedList<string, object> pParameters, int pExpectedRows, ref DCC_DB_NonQuery pResult, ref bool pHasPK)
        {
            throw new NotImplementedException();
            //    OracleCommand comm = new OracleCommand();
            //    comm.Connection = ora_conn;
            //    comm.CommandText = SQL;
            //    if (paras != null) 
            //        foreach (string para in paras.Keys) comm.Parameters.Add(new OracleParameter(para, paras[para]));
            //    try
            //    {
            //        rows = comm.ExecuteNonQuery();
            //        if (rows != 1)
            //        {
            //            StringBuilder sb = new StringBuilder();
            //            foreach (OracleParameter p in comm.Parameters) sb.Append("\r\n" + p.ParameterName + " = " + p.Value.ToString());
            //            MessageBox.Show(comm.CommandText + "\r\nParams:" + sb.ToString(), "Rows returned was " + rows.ToString());
            //        }
            //    }
            //    catch (Exception eSDR)
            //    {
            //        StringBuilder sb = new StringBuilder();
            //        foreach (OracleParameter p in comm.Parameters) sb.Append( "\r\n" + p.ParameterName + " = " + p.Value.ToString());
            //        MessageBox.Show("Database error: \r\nSQL was: " + comm.CommandText + "\r\nParams:" + sb.ToString(), eSDR.Message);
            //    }
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

        private static void traceParameters(OdbcCommand pCommand)
        {
            DebugTrace.WriteLine(pCommand.CommandText);
            foreach (OdbcParameter p in pCommand.Parameters) DebugTrace.WriteLine("  " + DCC_Formatting.DatabaseParameter(p));
        }

        private static void traceFailure(OdbcException pException)
        {
            DebugTrace.WriteLine("SQL Failed at " + DateTime.Now.ToString("HH:mm:ss") + " with message\r\n" + pException.Message);
        }

        private static void traceTimeout()
        {
            DebugTrace.WriteLine("Timed Out at " + DateTime.Now.ToString("HH:mm:ss"));
        }
    }
}
