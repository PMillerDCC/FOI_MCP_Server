using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace DCC_Standard
{

	public class SQLServer_DCC_DataLayer : DCC_DataLayer
	{
		SqlDataAdapter sql_da;
		IDbConnection sql_conn;
		SqlDataReader sql_dr;

		public SQLServer_DCC_DataLayer(string connectID)
		{
			newDCC_DataLayer("SQLServer", connectID, System.Configuration.ConfigurationManager.AppSettings["SoftwareIdentityCode"] ?? "MissingAppID");
		}

		public SQLServer_DCC_DataLayer(string connectID, string callingSoftwareID)
		{
			newDCC_DataLayer("SQLServer", connectID, callingSoftwareID);
		}

		protected override bool ConnectDatabase(string connectionString)
		{
			if (sql_conn != null && sql_conn.State == ConnectionState.Open) return true;

			// add timeout if none specified
			if (!connectionString.ToLower().Contains("timeout"))
			{
				connectionString += ";Connection Timeout=" + timeOut.ToString() + ";";
				connectionString = connectionString.Replace(";;", ";");
			}

			sql_conn = new SqlConnection(connectionString);

			return false;
		}

		protected override bool OpenDatabase()
		{
			if (sql_conn.State == ConnectionState.Open) { return true; }
			sql_conn.Open();
			return (sql_conn.State == ConnectionState.Open);
		}

		protected override void CloseDatabase()
		{
			if (sql_conn == null) return;
			if (sql_conn.State == ConnectionState.Closed) return;
			try
			{
				sql_conn.Close();
			}
			catch (Exception e)
			{
				lastError = new ErrorReport("0005", "Database close failed", ConnectionStringHelpers.MaskConnectionString(connectionString), e.Message, ErrorLevel.Information);
			}
		}

		protected override string FetchCulture()
		{
			sql_da = new SqlDataAdapter();
			sql_da.SelectCommand = new SqlCommand("EXEC SP_CONFIGURE 'default language';", (SqlConnection)sql_conn);
			sql_da.SelectCommand.CommandTimeout = timeOut;
			sql_da.SelectCommand.CommandType = CommandType.Text;
			result = new DataSet();
			sql_da.Fill(result);
			int lang = -999;
			try
			{
				lang = (int)result.Tables[0].Rows[0]["config_value"];
			}
			catch
			{
			}

			sql_da = new SqlDataAdapter();
			sql_da.SelectCommand = new SqlCommand("EXEC SP_HELPLANGUAGE;", (SqlConnection)sql_conn);
			sql_da.SelectCommand.CommandTimeout = timeOut;
			sql_da.SelectCommand.CommandType = CommandType.Text;
			result = new DataSet();
			sql_da.Fill(result);
			try
			{
				foreach (DataRow dr in result.Tables[0].Rows)
				{
					int langid = (short)dr["langid"];
					if (langid == lang)
					{
						return (string)dr["name"];
					}
				}
			}
			catch
			{
			}

			return "(no language match found)";
		}

		protected override DataTable DoSchema(string collectionName)
		{
			return (sql_conn as SqlConnection).GetSchema(collectionName);
		}

		/// <summary>
		/// Select data
		/// </summary>
		/// <param name="spName">Stored Procedure name</param>
		/// <param name="paras">Stored Procedure parameters</param>
		protected override void DoSelect(string spName, SortedList<string, object> paras)
		{
			int tries_remaining = 5;
			while (tries_remaining > 0)
			{
				sql_da = new SqlDataAdapter();
				sql_da.SelectCommand = new SqlCommand(spName, (SqlConnection)sql_conn);
				sql_da.SelectCommand.CommandTimeout = timeOut;
				sql_da.SelectCommand.CommandType = CommandType.StoredProcedure;
				if (spName.StartsWith(directSQLcommandMarker))
				{
					sql_da.SelectCommand.CommandText = spName.Substring(directSQLcommandMarker.Length);
					sql_da.SelectCommand.CommandType = CommandType.Text;
				}

				convertParas(paras, sql_da.SelectCommand);
				if (tries_remaining == 5) traceParameters(sql_da.SelectCommand);

				try
				{
					result.Clear(); // added this line to fix bug that resulted in duplicate records - c nesbitt dec 2016
					sql_da.Fill(result);
					break;
				}
				catch (SqlException eSQL)
				{
					traceFailure(eSQL);
					if (eSQL.Message.StartsWith("Timeout"))
					{
						tries_remaining--;
					}
					else
					{
						tries_remaining = 0;
						throw new DCCDataLayerException("Error _Standard.1001", "Database error - DATA NOT READ", "", sql_da.SelectCommand, eSQL);
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
					throw new DCCDataLayerException("Error _Standard.1001", "Database error - DATA NOT READ", "", sql_da.SelectCommand, eSDR);
				}
			}
			if (tries_remaining == 0)
			{
				throw new DCCDataLayerException("Error _Standard.1002", "Database error - DATABASE BUSY", "", sql_da.SelectCommand, null);
			}
		}

		protected override void coreFillDR(string spName, SortedList<string, object> paras, bool DirectSQL, SortedList<string, object> results)
		{
			SqlCommand comm = new SqlCommand();
			comm.CommandTimeout = timeOut;
			comm.Connection = (SqlConnection)sql_conn;
			comm.CommandText = spName;
			comm.CommandType = (DirectSQL ? CommandType.Text : CommandType.StoredProcedure);
			convertParas(paras, comm);

			SqlDataReader sdrX = null;
			traceParameters(comm);
			try
			{
				int tries_remaining = 5;
				while (tries_remaining > 0)
				{
					try
					{
						sdrX = comm.ExecuteReader(CommandBehavior.SingleRow);
						break;
					}
					catch (SqlException eSQL)
					{
						traceFailure(eSQL);
						if (eSQL.Message.StartsWith("Timeout"))
						{
							tries_remaining--;
						}
						else
						{
							tries_remaining = 0;
							throw new DCCDataLayerException("Error _Standard.1011", "Database error - DATA NOT READ", "", comm, eSQL);
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
						throw new DCCDataLayerException("Error _Standard.1011", "Database error - DATA NOT READ", "", comm, eSDR);
					}
				}

				if (tries_remaining == 0)
				{
					throw new DCCDataLayerException("Error _Standard.1012", "Database error - DATABASE BUSY", "", comm, null);
				}

				if (sdrX.HasRows)
				{
					sdrX.Read();
					for (int f = 0; f < sdrX.FieldCount; f++)
					{
						results.Add(sdrX.GetName(f), sdrX[f]);
					}
				}
			}

			catch (Exception eSDR)
			{
				throw new DCCDataLayerException("Error _Standard.1013", "Database error - DATA NOT READ", "", comm, eSDR);
			}
			finally
			{
				if (sdrX != null) sdrX.Close();
			}
		}

		protected override DbDataReader coreGetDR(string spName, SortedList<string, object> paras, bool DirectSQL, DbDataReader myDR)
		{
			SqlCommand comm = new SqlCommand();
			comm.CommandTimeout = timeOut;
			comm.Connection = (SqlConnection)sql_conn;
			comm.CommandText = spName;
			comm.CommandType = (DirectSQL ? CommandType.Text : CommandType.StoredProcedure);
			convertParas(paras, comm);

			sql_dr = null;
			if (IsDebugMode) traceParameters(comm);

			try
			{
				int tries_remaining = 5;
				while (tries_remaining > 0)
				{
					try
					{
						sql_dr = comm.ExecuteReader();
						myDR = sql_dr;
						break;
					}
					catch (SqlException eSQL)
					{
						traceFailure(eSQL);
						if (eSQL.Message.StartsWith("Timeout"))
						{
							tries_remaining--;
						}
						else
						{
							tries_remaining = 0;
							throw new DCCDataLayerException("Error _Standard.1011", "Database error - DATA NOT READ", "", comm, eSQL);
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
						throw new DCCDataLayerException("Error _Standard.1011", "Database error - DATA NOT READ", "", comm, eSDR);
					}
				}

				if (tries_remaining == 0)
				{
					throw new DCCDataLayerException("Error _Standard.1012", "Database error - DATABASE BUSY", "", comm, null);
				}

			}

			catch (Exception eSDR)
			{
				throw new DCCDataLayerException("Error _Standard.1013", "Database error - DATA NOT READ", "", comm, eSDR);
			}

			return myDR;
		}

		protected override void DoNonQuery(string spName, SortedList<string, object> paras, int expectedRows, ref DCC_DB_NonQuery theResult, ref bool hasPK)
		{
			SqlCommand comm = new SqlCommand();
			comm.CommandTimeout = timeOut;
			comm.CommandType = CommandType.StoredProcedure;
			comm.Connection = (SqlConnection)sql_conn;
			comm.CommandText = spName;
			if (spName.StartsWith(directSQLcommandMarker))
			{
				comm.CommandType = CommandType.Text;
				comm.CommandText = spName.Substring(directSQLcommandMarker.Length);
			}
			hasPK = convertParas(paras, comm);
			if (IsDebugMode) traceParameters(comm);
			if (isReadOnly) throw new DCCDataLayerException("Error _Standard.1039", "Database error", "Attempt to Write to ReadOnly: " + spName);
			try
			{
				int tries_remaining = 5;
				while (tries_remaining > 0)
				{
					try
					{
						theResult.rows = comm.ExecuteNonQuery();
						break; // out of while loop
					}
					catch (SqlException eSQL)
					{
						traceFailure(eSQL);
						if (eSQL.Message.StartsWith("Timeout"))
						{
							tries_remaining--;
						}
						else
						{
							tries_remaining = 0;
							eSQL.Source = "ExecuteNonQuery";
							throw new DCCDataLayerException("Error _Standard.1031", "Database error", "", comm, eSQL);
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
						eSDR.Source = "ExecuteNonQuery";
						throw new DCCDataLayerException("Error _Standard.1031", "Database error", "", comm, eSDR);
					}
				}

				if (tries_remaining == 0)
				{
					throw new DCCDataLayerException("Error _Standard.1032", "Database error - DATABASE BUSY", "", comm, null);
				}

				if (theResult.rows == expectedRows || expectedRows == -1)
				{
					theResult.ok = true;
					if (hasPK) theResult.newPK = (int)comm.Parameters["@_PRIMARYKEY"].Value;
				}
				else
				{
					throw new Exception("Error _Standard.1034 Rows returned (" + theResult.rows.ToString() + ") was not as expected (" + expectedRows.ToString() + ")");
				}

			}
			catch (Exception eSDR)
			{
				eSDR.Source = "ExecuteNonQuery";
				throw new DCCDataLayerException("Error _Standard.1033", "Database error - DATA NOT SAVED", "", comm, eSDR);
			}
		}

		protected override int coreDirectSQL(string spName, int rows)
		{
			SqlCommand comm = new SqlCommand(spName, (SqlConnection)sql_conn);
			comm.CommandTimeout = timeOut;
			if (IsDebugMode) traceParameters(comm);
			if (isReadOnly) throw new DCCDataLayerException("Error _Standard.1039", "Database error", "Attempt to Write to ReadOnly: " + spName);

			try
			{
				int tries_remaining = 5;
				while (tries_remaining > 0)
				{
					try
					{
						rows = comm.ExecuteNonQuery();
						break;
					}
					catch (Exception eSDR)
					{
						if (eSDR.Message.ToUpper().Contains("TIMEOUT"))
						{
							traceTimeout();
							tries_remaining--;
							continue;
						}
						else
						{
							tries_remaining = 0;
							throw new DCCDataLayerException("Error _Standard.1041", "Database error - DATA NOT READ", "", comm, eSDR);
						}
					}
				}
				if (tries_remaining == 0)
				{
					throw new DCCDataLayerException("Error _Standard.1042", "Database error - DATABASE BUSY", "", comm, null);
				}

			}
			catch (Exception eSDR)
			{
				throw new DCCDataLayerException("Error _Standard.1043", "Database error - DATA NOT SAVED", "", comm, eSDR);
			}
			return rows;
		}

		private static void traceParameters(SqlCommand comm)
		{
			DebugTrace.WriteLine(comm.CommandText);
			foreach (SqlParameter p in comm.Parameters) DebugTrace.WriteLine("  " + DCC_Formatting.DatabaseParameter(p));
		}

		private static void traceFailure(SqlException eSQL)
		{
			DebugTrace.WriteLine("SQL Failed at " + DateTime.Now.ToString("HH:mm:ss") + " with message\r\n" + eSQL.Message);
		}

		private static void traceTimeout()
		{
			DebugTrace.WriteLine("Timed Out at " + DateTime.Now.ToString("HH:mm:ss"));
		}

	}
}
