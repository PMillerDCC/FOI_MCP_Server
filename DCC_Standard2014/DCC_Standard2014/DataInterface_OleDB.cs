using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;

namespace DCC_Standard
{

    public class OleDB_DCC_DataLayer : DCC_DataLayer
    {
        OleDbDataAdapter ole_da;
        OleDbConnection ole_conn;
        OleDbDataReader ole_dr;

        public OleDB_DCC_DataLayer(string connectID)
        {
            newDCC_DataLayer("OleDB", connectID, System.Configuration.ConfigurationManager.AppSettings["SoftwareIdentityCode"]??"MissingAppID");
        }

        public OleDB_DCC_DataLayer(string connectID,string callingSoftwareID)
        {
            newDCC_DataLayer("OleDB", connectID,callingSoftwareID);
        }

        protected override bool ConnectDatabase(string connectionString)
        {
            if (ole_conn != null && ole_conn.State == ConnectionState.Open) return true;

            ole_conn = new OleDbConnection(connectionString);

            return false;
        }
        protected override bool OpenDatabase()
        {
            if (ole_conn.State == ConnectionState.Open) { return true; }
            ole_conn.Open();
            if (ole_conn.State == ConnectionState.Open) { return true; }
            return false;
        }
        protected override void CloseDatabase()
        {
            if (ole_conn == null) return;
            if (ole_conn.State == ConnectionState.Closed) return;
            try
            {
                ole_conn.Close();
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
            return ole_conn.GetSchema(collectionName);
        }

        protected override void DoSelect(string spName, SortedList<string, object> paras)
        {
            ole_da = new OleDbDataAdapter();
            ole_da.SelectCommand = new OleDbCommand(spName, ole_conn);
            ole_da.SelectCommand.CommandTimeout = 10;
            ole_da.SelectCommand.CommandType = CommandType.StoredProcedure;
            if (spName.StartsWith(directSQLcommandMarker))
            {
                ole_da.SelectCommand.CommandText = spName.Substring(directSQLcommandMarker.Length);
                ole_da.SelectCommand.CommandType = CommandType.Text;
            }

            convertParas(paras, ole_da.SelectCommand);
            if (IsDebugMode) traceParameters(ole_da.SelectCommand);
            int tries_remaining = 5;
            while (tries_remaining > 0)
            {
                try
                {
                    ole_da.Fill(result);
                    break;
                }
                catch (OleDbException eOLE)
                {
                    traceFailure(eOLE);
                    if (eOLE.Message.StartsWith("Timeout"))
                    {
                        tries_remaining--;
                    }
                    else
                    {
                        tries_remaining = 0;
                        throw new DCCDataLayerException("Error 1001", "Database error - DATA NOT READ", "", ole_da.SelectCommand, eOLE);
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
                    throw new DCCDataLayerException("Error 1001", "Database error - DATA NOT READ", "", ole_da.SelectCommand, eSDR);
                }
            }
            if (tries_remaining == 0)
            {
                throw new DCCDataLayerException("Error 1002", "Database error - DATABASE BUSY", "", ole_da.SelectCommand, null);
            }
        }

        protected override void DoNonQuery(string spName, SortedList<string, object> paras, int expectedRows, ref DCC_DB_NonQuery theResult, ref bool hasPK)
        {
            OleDbCommand comm = new OleDbCommand();
            comm.CommandTimeout = 10;
            comm.CommandType = CommandType.StoredProcedure;
            comm.Connection = ole_conn;
            comm.CommandText = spName;

            if (spName.StartsWith(directSQLcommandMarker))
            {
                comm.CommandType = CommandType.Text;
                comm.CommandText = spName.Substring(directSQLcommandMarker.Length);
            }
            hasPK = convertParas(paras, comm);

            if (IsDebugMode) traceParameters(comm);

            try
            {
                int tries_remaining = 5;
                while (tries_remaining > 0)
                {
                    try
                    {
                        theResult.rows = comm.ExecuteNonQuery();
                        break;
                    }
                    catch (OleDbException eole)
                    {
                        traceFailure(eole);
                        if (eole.Message.StartsWith("Timeout"))
                        {
                            tries_remaining--;
                        }
                        else
                        {
                            tries_remaining = 0;
                            throw new DCCDataLayerException("Error 1031", "Database error", "", comm, eole);
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
                        throw new DCCDataLayerException("Error 1031", "Database error", "", comm, eSDR);
                    }
                }
                if (tries_remaining == 0)
                {
                    throw new DCCDataLayerException("Error 1032", "Database error - DATABASE BUSY", "", comm, null);
                }

                if (theResult.rows == expectedRows || expectedRows==-1)
                {
                    theResult.ok = true;
                    if (hasPK) theResult.newPK = (int)comm.Parameters["@_PRIMARYKEY"].Value;
                }
                else
                {
                    throw new Exception("Error 1034 Rows returned (" + theResult.rows.ToString() + ") was not as expected (" + expectedRows.ToString() + ")");
                }

            }
            catch (Exception eSDR)
            {
                throw new DCCDataLayerException("Error 1033", "Database error - DATA NOT SAVED", "", comm, eSDR);
            }
        }

        protected override DbDataReader coreGetDR(string spName, SortedList<string, object> paras, bool DirectSQL, DbDataReader myDR)
        {
            OleDbCommand comm = new OleDbCommand();
            comm.CommandTimeout = 10;                 
            comm.Connection = ole_conn;
            comm.CommandText = spName;
            comm.CommandType = (DirectSQL?CommandType.Text:CommandType.StoredProcedure);
            convertParas(paras, comm);

            ole_dr = null;

            if (IsDebugMode) traceParameters(comm);

            try
            {
                int tries_remaining = 5;
                while (tries_remaining > 0)
                {
                    try
                    {
                        ole_dr = comm.ExecuteReader();
                        myDR = ole_dr;
                        break;
                    }
                    catch (OleDbException eSQL)
                    {
                        if (IsDebugMode) traceFailure(eSQL);
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
        
            return myDR;
        }

        protected override int coreDirectSQL(string sql, int rows)
        {
            throw new NotImplementedException();
        }

        private static void traceParameters(OleDbCommand comm)
        {
            DebugTrace.WriteLine(comm.CommandText);
            foreach (OleDbParameter p in comm.Parameters) DebugTrace.WriteLine("  " + DCC_Formatting.DatabaseParameter(p));
        }

        private static void traceFailure(OleDbException eSQL)
        {
            DebugTrace.WriteLine("SQL Failed at " + DateTime.Now.ToString("HH:mm:ss") + " with message\r\n" + eSQL.Message);
        }

        private static void traceTimeout()
        {
            DebugTrace.WriteLine("Timed Out at "+DateTime.Now.ToString("HH:mm:ss"));
        }

    }
}
