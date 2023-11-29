using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace DCC_Standard
{


    public class DCC_Exception : Exception
    {
        public DCC_Exception() : base() { }
        public DCC_Exception(string message) : base(message) { }
        public DCC_Exception(string format, params object[] args) : base(string.Format(format, args)) { }
        public DCC_Exception(string message, Exception innerException) : base(message, innerException) { }
        public DCC_Exception(string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException) { }
    }



    /// <summary>
    /// Provide Debug Tracing into text files
    /// </summary>
    public class DebugTrace
    {
        static StreamWriter streamWriter;
        static DateTime streamWriterStart;
        static DateTime? stackLogStart = null;
        static bool isWebApp;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="startClock"></param>
        public static void stackLog(string text, bool startClock)
        {
            if (startClock) stackLogStart = DateTime.Now;
            string currentMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;
            StackTrace trace = new StackTrace();
            StackFrame frame = trace.GetFrame(1);
            string callerName = frame.GetMethod().Name;
            string message = "";
            if (stackLogStart.HasValue)
            {
                TimeSpan taken = DateTime.Now - stackLogStart.Value;
                if (taken.TotalMilliseconds > 0) message = "taken " + taken.TotalMilliseconds.ToString("######0.00") + "ms";
            }
            WriteLine(DateTime.Now.ToString("HH:mm:ss") + " " + callerName + " " + text + " " + message);
        }



        ///// <summary>
        ///// Writes a blank line into the debug trace log file
        ///// </summary>
        //public static void WriteLine()
        //{
        //    WriteLine("");
        //}


        /// <summary>
        /// Writes the given text into the debug trace log file
        /// A new file is created every 15 minutes or after 50MB
        /// </summary>
        /// <param name="logText">given text</param>
        public static void WriteLine(string logText = "")
        {
            string logFile = "";

			#region Determine if we need to create a new file
			
			if (streamWriter != null)
            {
                bool needToClose = false;
                try
                {
                    needToClose = (streamWriter.BaseStream.Length > (50 * 1024 * 1024));  // && (streamWriterStart.AddSeconds(900) > DateTime.Now));
                }
                catch (Exception e)
				{
					needToClose = true;
				}
                if (needToClose) EndCurrentLog();
            }
			
			#endregion

			#region Create a new Log File

			if (streamWriter == null)
            {
				streamWriterStart = DateTime.Now;
                isWebApp = (HttpContext.Current != null);
                string path = ConfigurationManager.AppSettings["LogPath"] ?? "";
                if (string.IsNullOrEmpty(path))
                {
                    if (isWebApp) path = HttpContext.Current.Request.PhysicalApplicationPath.ToString() + "App_Data\\Logs\\";
                    else MessageBox.Show(@"No [LogPath] specified in App.Config appSettings\r\n\r\nMessage was:\r\n" + logText, "Logging Problem");
                }

			    string filename = "SystemLog " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss ddd") + ".txt";
                logFile = Path.Combine(path, filename);
				try
				{
					streamWriter = new StreamWriter(logFile, true);
				}
				catch
				{
					streamWriter = null;
					if (isWebApp)
					{
						HttpContext.Current.Response.Write("Failed to create Log File [" + logFile + "]");
						HttpContext.Current.Response.End();
					}
					MessageBox.Show("Failed to create Log File [" + logFile + "]" + @")\r\nMessage was:\r\n" + logText);
				}
			}
			
			#endregion

			#region Write to file

			try
			{
                streamWriter.Write(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "  " + logText + Environment.NewLine);
                streamWriter.Flush();
            }
            catch
            {
                streamWriter = null;
                if (isWebApp)
                {
	                HttpContext.Current.Response.Write("Failed to write to Log File [" + logFile + "]");
                    HttpContext.Current.Response.End();
                }
	            MessageBox.Show("Failed to write to Log File [" + logFile + "]" + @")\r\nMessage was:\r\n" + logText);
            }

            #endregion

        }


        /// <summary>
        /// Writes a dump from the given DataRow into the debug trace log file and adds a CRLF
        /// </summary>
        /// <param name="logDataRow">given text</param>
        public static void WriteLine(DataRow logDataRow)
        {
            if ( (logDataRow == null) || (logDataRow.Table.Columns.Count == 0) ) return;

            StringBuilder sb = new StringBuilder();
            int maxCol = 0;
            int maxType = 0;
            foreach (DataColumn dc in logDataRow.Table.Columns)
            {
                maxCol = Math.Max(maxCol, dc.ColumnName.Length);
                maxType = Math.Max(maxType, dc.DataType.ToString().Replace("System.", "").Length);
            }

            foreach (DataColumn dataColumn in logDataRow.Table.Columns)
            {
                sb.Append(dataColumn.DataType.ToString().Replace("System.", "").PadRight(maxType + 2));
                sb.Append(dataColumn.ColumnName.PadRight(maxCol + 2) + " = ");
                switch (dataColumn.DataType.ToString())
                {
                    case "System.String":   { sb.Append("\"" + DCC_GetColumn.String(logDataRow, dataColumn.ColumnName) + "\""); break; }
                    case "System.Boolean":  { sb.Append(DCC_GetColumn.Bit      (logDataRow, dataColumn.ColumnName).ToString()); break; }
                    case "System.Byte":     { sb.Append(DCC_GetColumn.Integer08(logDataRow, dataColumn.ColumnName).ToString()); break; }
                    case "System.Int16":    { sb.Append(DCC_GetColumn.Integer16(logDataRow, dataColumn.ColumnName).ToString()); break; }
                    case "System.Int32":    { sb.Append(DCC_GetColumn.Integer32(logDataRow, dataColumn.ColumnName).ToString()); break; }
                    case "System.Int64":    { sb.Append(DCC_GetColumn.Integer64(logDataRow, dataColumn.ColumnName).ToString()); break; }
                    case "System.Decimal":  { sb.Append(DCC_GetColumn.Decimal  (logDataRow, dataColumn.ColumnName).ToString()); break; }
                    case "System.Float":    { sb.Append(DCC_GetColumn.Float    (logDataRow, dataColumn.ColumnName).ToString()); break; }
                    case "System.DateTime": { sb.Append(DCC_GetColumn.DateTime (logDataRow, dataColumn.ColumnName).ToString("yyyy MMM dd HH:mm:ss").Replace(" 00:00:00", "")); break; }
                    default: { sb.Append("(unhandled type) "); break; }
                }
                sb.AppendLine("");
            }
            sb.AppendLine("------------------------");

            WriteLine(sb.ToString());
            WriteLine("\r\n");
        }


		/// <summary>
		/// Close the current log file
		/// </summary> 
		public static void EndCurrentLog()
		{
			if (streamWriter != null)
            {
			    streamWriter.Close();
			    streamWriter.Dispose();
			    streamWriter = null;
            }
		}

    }




	//-------------------------------------------------------------------------------------------------------------------------------------------------




    /// <summary>
    /// Indication of how important the incident is
    /// </summary>
    public enum ErrorLevel
    {
        /// <summary>
        /// used for tracing and tracking
        /// </summary>
        Information,
        /// <summary>
        /// potential problems that have been recovered from
        /// </summary>
        Warning,
        /// <summary>
        /// problem that cannot be recovered
        /// </summary>
        Error,
        /// <summary>
        /// problem causing application to terminate
        /// </summary>
        SevereError
    };

    /// <summary>
    /// static class to provide a permanent record of errors
    /// </summary>
    public class ErrorFile
    {
        static string logFile = "";

        /// <summary>
        /// write a string to the error file
        /// </summary>
        /// <param name="logText">string to be written</param>
        public static void WriteLine(string logText)
        {
            if ((logFile ?? "").Length == 0)
			{
				bool isWebApp = (HttpContext.Current != null);
				string path = ConfigurationManager.AppSettings["LogPath"] ?? "";
				if (string.IsNullOrEmpty(path))
				{
					if (isWebApp) path = HttpContext.Current.Request.PhysicalApplicationPath.ToString() + "App_Data\\Logs\\";
					else MessageBox.Show(@"No [LogPath] specified in App.Config appSettings\r\n\r\nMessage was:\r\n" + logText, "Logging Problem");
				}
				string filename = "ErrorLog " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss ddd") + ".txt";
				logFile = Path.Combine(path, filename);
			}
			
			if ((logFile ?? "").Length > 0)
            {
                try
                {
                    StreamWriter sw = new StreamWriter(logFile, true);
                    sw.Write(logText);
                    sw.Flush();
                }
                catch
				{
					// give up
				}
            }
        }
    }

    public class ErrorReport
    {
        string lastErrorTxt;
        string lastPrivateTxt;
        string lastErrorCode;
        string lastErrorDump;
        ErrorLevel lastErrorLevel;

        /// <summary>
        /// Returns formatted error level / code / text adjusted to have CR LF instead of HTML line breaks
        /// </summary>
        public string ErrorTxt
        {
            get { return (lastErrorLevel.ToString() + "  " + lastErrorCode + ": " + lastErrorTxt).Replace("<br />", "\r\n"); }
        }

        /// <summary>
        /// Returns formatted error level / code / text adjusted to have HTML line breaks
        /// </summary>
        public string ErrorTxtHTML
        {
            get { return ErrorTxt.Replace("\r\n", "<br />"); }
        }

        /// <summary>
        /// Generate an Error - Reported to server event log, designated error log file, and sent by Email, and displayed
        /// </summary>
        /// <param name="code">A short code e.g. 50010 - there are no rules defined, but there should be</param>
        /// <param name="text">Text an endUser can see</param>
        /// <param name="forSupport">Text for Support</param>
        /// <param name="dump">Text for the developer</param>
        /// <param name="level">Error Level (info/warn ... severe)</param>
        public ErrorReport(string code, string text, string forSupport, string dump, ErrorLevel level)
        {
            lastErrorTxt = text;
            lastErrorCode = code;
            lastErrorLevel = level;
            lastErrorDump = dump;
            lastPrivateTxt = forSupport;

            bool isWebApp = false;
            bool isLocal = false;
            if (HttpContext.Current != null)
            {
                isWebApp = true;
                if (HttpContext.Current.Request != null)
                {
                    isLocal = HttpContext.Current.Request.IsLocal;
                }
            }

            string applicationIdentifier = ConfigurationManager.AppSettings["SoftwareIdentityCode"] ?? "MissingAppID";

            WindowsIdentity ident = WindowsIdentity.GetCurrent();
            WindowsPrincipal user = new WindowsPrincipal(ident);
            StringBuilder sb = new StringBuilder();

            AddTabEndLine(sb, DateTime.Now.ToString("yyyy MMM dd, ddd, HH:mm:ss"));
            AddTabEndLine(sb, "Application: " + applicationIdentifier);
            AddTabEndLine(sb, ErrorTxt);

            if (isWebApp)
            {
                string who = "not available";
                if (HttpContext.Current.User != null)
                {
                    if (HttpContext.Current.User.Identity != null) who = HttpContext.Current.User.Identity.Name ?? "not available";
                }
                AddTabEndLine(sb, "Logon User: " + who);
                AddTabEndLine(sb, "Requesting User: " + HttpContext.Current.Request.ServerVariables["LOGON_USER"]);
                AddTabEndLine(sb, "Identity: " + HttpContext.Current.Request.LogonUserIdentity.Name);
            }
            else
            {
                AddTabEndLine(sb, "Logon User: " + ident.Name);
            }
            AddTabEndLine(sb, "Server: " + Environment.MachineName);
            if (isWebApp)
            {

				try
				{
					if (HttpContext.Current.Session != null)
					{
						AddTabEndLine(sb, "");
						foreach (string k in HttpContext.Current.Session.Keys)
						{
							object obj = HttpContext.Current.Session[k];
							string objTxt = obj.ToString();
							//if (obj is CurrentUser)                 YIKES
							//{
							//	CurrentUser cu = (CurrentUser)obj;
							//	objTxt = cu.FullName + "  mailto://" + cu.EmailAddress + "   " + cu.TelephoneNumber;
							//}
							AddTabEndLine(sb, "Session[" + k + "]: " + objTxt);
						}
					}
				}

				catch
				{
				}

				finally
				{
				}

            }
            AddTabEndLine(sb, "\r\nSupportInfo:\r\n" + lastPrivateTxt.Replace("<br />", "\r\n"));
            AddTabEndLine(sb, "\r\nError Dump:\r\n" + lastErrorDump.Replace("<br />", "\r\n"));
            try
            {
                System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(true);
                AddTabEndLine(sb, "\r\nExternal StackTrace:\r\n" + st.ToString().Replace("<br />", "\r\n"));
            }
            catch
            {
                AddTabEndLine(sb, "\r\nExternal StackTrace:\r\n (failed to retrieve stack)".Replace("<br />", "\r\n"));
            }
            AddTabEndLine(sb, "");
            AddTabEndLine(sb, "");
            if (isWebApp)
            {
                HttpRequest currReq = HttpContext.Current.Request;
                AddTabEndLine(sb, "Session: " + (HttpContext.Current.Session == null ? "(null)" : HttpContext.Current.Session.SessionID));
                AddTabEndLine(sb, "Calling URI: " + currReq.RawUrl);
                AddTabEndLine(sb, "Relative Execution File Path: " + currReq.AppRelativeCurrentExecutionFilePath);
                AddTabEndLine(sb, "Physical Application Path: " + currReq.PhysicalApplicationPath);
                AddTabEndLine(sb, "Request File Path: " + currReq.FilePath);
                AddTabEndLine(sb, "Server MapPath: " + HttpContext.Current.Server.MapPath("./"));
                AddTabEndLine(sb, "Server IIS Version: " + (currReq.IsLocal ? "(local development environment)" : currReq.ServerVariables["SERVER_SOFTWARE"] ?? "-not found-"));
                AddTabEndLine(sb, "Browser: " + currReq.Browser.Browser + " " + currReq.Browser.Version);

                AddTabEndLine(sb, "");
                int n = 0;
                AddTabEndLine(sb, "Request.QueryString has " + currReq.QueryString.Count.ToString() + " keys");
                foreach (string k in currReq.QueryString.Keys)
                {
                    AddTabEndLine(sb, "Query #" + n++.ToString().PadLeft(2) + " [Key:" + (k + "]").PadRight(30) + "[Value: " + currReq.QueryString[k] + "]");
                }
                AddTabEndLine(sb, "");
                n = 0;
                AddTabEndLine(sb, "Request.Cookies has " + currReq.Cookies.Count.ToString() + " keys");
                foreach (string k in currReq.Cookies.Keys)
                {
                    HttpCookie c = currReq.Cookies[k];
                    string expiry = c.Expires.ToString("yyyyMMdd HHmmss");
                    AddTabEndLine(sb, "Cookie #" + n++.ToString().PadLeft(2) + " [Key:" + (k + "]").PadRight(25) + (expiry == "00010101 000000" ? "" : "[Expires: " + expiry + "]") + "[Value: " + c.Value + "]" + (c.Domain == null ? "" : "[Domain: " + c.Domain + "]"));
                }
                AddTabEndLine(sb, "");
                n = 0;
                AddTabEndLine(sb, "Request.Form has " + currReq.Form.Count.ToString() + " keys");
                foreach (string k in currReq.Form.Keys)
                {
                    if (k == "__VIEWSTATE" || k == "__EVENTVALIDATION")
                    {
                        AddTabEndLine(sb, "Form #" + n++.ToString().PadLeft(2) + " [Key:" + (k + "]").PadRight(25) + "(Base64 value)");
                    }
                    else
                    {
                        string v = currReq.Form[k];
                        AddTabEndLine(sb, "Form #" + n++.ToString().PadLeft(2) + " [Key:" + (k + "]").PadRight(25) + (v.Length > 0 ? "[Value: " + v + "]" : "(empty)"));
                    }
                }
                AddTabEndLine(sb, "");
                n = 0;
                AddTabEndLine(sb, "Request.Server has " + currReq.ServerVariables.Count.ToString() + " keys");
                foreach (string k in currReq.ServerVariables.Keys)
                {
                    if (k == "ALL_HTTP" || k == "ALL_RAW")
                    {
                        AddTabEndLine(sb, "Server #" + n++.ToString().PadLeft(2) + " [Key:" + (k + "]").PadRight(25) + "(individual values below)");
                    }
                    else
                    {
                        string v = currReq.ServerVariables[k];
                        AddTabEndLine(sb, "Server #" + n++.ToString().PadLeft(2) + " [Key:" + (k + "]").PadRight(25) + (v.Length > 0 ? "[Value: " + v + "]" : "(empty)"));
                    }
                }

            }
            string errorBody = sb.ToString();

            ReportByEventLog(applicationIdentifier, errorBody);

            ReportToApp_Data("\r\n" + new string('=', 80) + "\r\n\r\n" + errorBody);

            if (level != ErrorLevel.Information)
            {
                ReportByEmail(applicationIdentifier, errorBody, ErrorTxt);
            }
        }

        private static void AddTabEndLine(StringBuilder stringBuilder, string text)
        {
            stringBuilder.AppendLine(text + "\t");
        }

        private static void ReportByEventLog(string applicationID, string errorBody)
        {
            try
            {
                EventLog.WriteEntry(applicationID, errorBody, EventLogEntryType.Error, 50000, 0);
            }
            catch { /* give up ! - we probably don't have rights to write to EventLog */ }
        }

        private static void ReportToApp_Data(string errorBody)
        {
            try
            {
                ErrorFile.WriteLine(errorBody);
            }
            catch { /* give up ! - we probably don't have rights to write to the error file, or it is locked */ }
        }

        private static void ReportByEmail(string applicationID, string errorBody, string errorText)
        {
            try
            {
                string emailTo = "";
                string subjectPrefix = "Live ";

                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Request.IsLocal)
                    {
                        string lu = HttpContext.Current.Request.ServerVariables["LOGON_USER"] ?? "";
                        if (lu == "") lu = HttpContext.Current.Request.LogonUserIdentity.Name;

                        string[] userBits = lu.Split('\\');
                        if (userBits.Length > 1)
                        {
                            emailTo = userBits[1] + "@durham.gov.uk";
                        }
                        subjectPrefix = "Toolkit Testing - ";
                    }
                }
                if (emailTo.Length == 0)
                    emailTo = ConfigurationManager.AppSettings["EmailErrorsTo"];

                if ((emailTo ?? "").Length > 0)
                {
                    string sender = applicationID + "_Error_Report@durham.gov.uk";
                    string subject = subjectPrefix + "Error Report from " + applicationID + " - " + errorText;
                    List<string> toList = emailTo.Split(new char[] { ',', ';' }).ToList();
                    List<string> bccList = new List<string>(0);
                    //if (!emailTo.ToLower().Contains("graham.waters")) bccList.Add("graham.waters@durham.gov.uk");
                    DCC_Email.Send(sender, subject, errorBody, toList, null, bccList, null);  // ignore failures - nothing more we can try !
                }
            }
            catch { }
        }

        public void ReportErrorsToUser(bool isWebApp)
        {
            if (lastErrorLevel != ErrorLevel.Information)
            {
                if (isWebApp)
                {
                    try
                    {
                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.Redirect("~//Error//Index");
                    }
                    catch
                    {
                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.Write("Unable to find Error page.");
                        HttpContext.Current.Response.End();
                    }
                    finally
                    {
                    }
                }
                else
                {
                    // do not try Application.Exit()  - this prevents MessageBox.Show displaying, and loses all Forms, so nothing is displayed
                    // instead must pass failure status back up the stack to be handled elsewhere
                }
            }
        }

    }

}
