using System;
using System.Data.Common;
using System.Data.OracleClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace DCC_Standard
{

    /// <summary>
    /// Provides commonly used text formatting functions
    /// </summary>
    public class DCC_Formatting
    {


        /// <summary>
        /// Format a size in bytes to include the most approporate suffix
        /// </summary>
        /// <param name="bytes">
        /// The number of bytes
        /// </param>
        /// <returns>
        /// The size using the most appropriate suffix
        /// </returns>
        public static string FileSize(Int64 bytes)
        {
            string[] suffixes = { "Bytes", "KB", "MB", "GB", "TB", "PB" };
            int counter = 0;
            decimal number = (decimal)bytes;
            while (Math.Round(number / 1024) >= 1)
            {
                number = number / 1024;
                counter++;
            }
            return string.Format("{0:n1}{1}", number, suffixes[counter]);
        }



        /// <summary>
        /// Formats a number of days as years, weeks and days
        /// </summary>
        /// <param name="days">The number of days to format</param>
        /// <returns></returns>
        public static string FormatDaysAsYearsWeeksAndDays(int? days = -1)
        {
            string rc = "";
            if ( (days.HasValue) && (days.Value > 0) )
            {
                if (days > 0)
                {
			        int y = (days.Value / 365);
                    int w = (days.Value % 365) / 7;
                    int d = days.Value - ((y * 365) + (w * 7));
                    rc = (y > 0 ? y + " yr" + ( y == 1 ? "" : "s")+", " : "") + (w > 0 ? w + " wk" + ( w == 1 ? "" : "s") + " and " : "") + d + " day" + ( d == 1 ? "" : "s");
                }
            }
            return rc;
        }



        /// <summary>
        /// Provides standard formatting for dates
        /// </summary>
        /// <param name="when">
        /// DateTime to be formatted
        /// </param>
        /// <param name="showSecs">True for HH:mm:ss, false for HH:mm</param>
        /// <returns>
        /// The value is formatted relative to today
        /// </returns>
        public static string RelativeDateTime(DateTime when, bool showSecs = false)
        {
            string mask = "HH:mm";
            if (showSecs) mask += ":ss";
            string HHmm=" "+when.ToString(mask);

            if (DateTime.Now.Date == when.Date) return "today"+HHmm;

            if (DateTime.Now.Date == when.Date.AddDays(1)) return "yesterday"+HHmm;

            if (DateTime.Now.Date.ToString("MMyyyy") == when.Date.ToString("MMyyyy"))
            {
                string stndrdth = "th";
                switch (when.Day)
                {   case 1:
                    case 21:
                    case 31:
                        stndrdth = "st";
                        break;
                    case 2:
                    case 22:
                        stndrdth = "nd";
                        break;
                    case 3:
                    case 23:
                        stndrdth = "rd";
                        break;
                }
                return when.ToString("ddd, d") + stndrdth + HHmm;
            }

            return when.ToString("ddd, d MMM yyyy") + HHmm;
        }

        /// <summary>
        /// Provides standard formatting for money amounts on reports
        /// </summary>
        /// <param name="amount">
        /// decimal value to be formatted
        /// </param>
        /// <returns>
        /// The value is formatted for display in a report (printed/csv/pdf).
        /// Negative values are placed inside '()'.
        /// </returns>
        public static string ReportMoney(decimal amount)
        {
            if (amount >= 0) return "£" + amount.ToString("###,###,###,##0.00");
            return "(£" + (amount*-1).ToString("###,###,###,##0.00") + ")";
        }

        /// <summary>
        /// Provides standard formatting for money amounts
        /// </summary>
        /// <param name="amount">
        /// decimal value to be formatted
        /// </param>
        /// <returns>
        /// The value is formatted for display on the web.
        /// The HTML entity &amp;pound; is used instead of the £ sign (some older browsers show a square box for £)
        /// Negative values are placed in an HTML span in () and coloured red without the minus sign.
        /// </returns>
        public static string Money(decimal amount)
        {
            return Money(amount, false);
        }

        /// <summary>
        /// Provides standard formatting for money amounts
        /// </summary>
        /// <param name="amount">
        /// decimal value to be formatted
        /// </param>
        /// <param name="withCommas">if true, adds ',' every third digit</param>
        /// <returns>
        /// The value is formatted for display on the web.
        /// The HTML entity &amp;pound; is used instead of the £ sign (some older browsers show a square box for £)
        /// Negative values are placed in an HTML span in () and coloured red without the minus sign.
        /// </returns>
        public static string Money(decimal amount, bool withCommas)
        {
            string format = (withCommas ? "###,###,###,###,##0.00" : "0.00");
            if (amount >= 0) return "£" + amount.ToString(format);
            return "<span class='red'>(" + "£" + (amount * -1).ToString(format) + ")</span>";
        }

        /// <summary>
        /// Provides standard un-formatting for money amounts
        /// </summary>
        /// <param name="text">
        /// money amount formatted by Formatting.Money()
        /// </param>
        /// <returns>
        /// The value is returned to a simple text string.
        /// Negative values are indicated by a leading '-' sign.
        /// </returns>
        public static string PlainMoney(string text)
        {
            string reply = text.ToLower().Replace(",","");
            string credit = "";
            if (reply.StartsWith("<"))
            {
                reply = reply.Replace("<span class='red'>","").Replace("</span>","");
            }
            if (reply.StartsWith("("))
            {   reply = reply.Replace("(","").Replace(")","");
                credit = "-";
            }
            if (reply.StartsWith("&")) reply = reply.Replace("&pound;","");
            return credit + reply;
        }

        /// <summary>
        /// Formats minutes into hours and minutes
        /// </summary>
        /// <param name="minutes">number of minutes</param>
        /// <returns>Formatted string hh:mm (-ve minutes rendered as +ve)</returns>
        public static string HoursFromMins(int minutes)
        {
            return HoursFromMins(minutes, "hh\\:mm");
        }

        /// <summary>
        /// Formats minutes into hours and minutes
        /// </summary>
        /// <param name="minutes">number of minutes</param>
        /// <param name="format">format string (tokens 'h' 'hh' 'm' 'mm' and delimiters escaped by '\' - examples hh\:mm   h\.mm  hh\hmm\m  see TimeSpan format strings)</param>
        /// <returns>Formatted string hh:mm (-ve minutes rendered as +ve)  If over 23h59m then will be preceeded with number of days as 'nnd '</returns>
        public static string HoursFromMins(int minutes, string format)
        {
            int ma = Math.Abs(minutes);
            int hh = ma / 60;
            int mm = ma % 60;
            TimeSpan ts = new TimeSpan(hh,mm,0);
            if (ts.TotalHours >= 24) return (ts.ToString("d\\d\\ " + format));
            return (ts.ToString(format));
        }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static string ReadableTime(double ms = 0, double s = 0, double m = 0, double h = 0, bool includeZeroes = true, bool includeMilliseconds = true, bool inEnglish = false)
		{
			TimeSpan ts = new TimeSpan();
			string returnString = "";

			if      (ms > 0) ts = TimeSpan.FromMilliseconds(ms);
			else if (s  > 0) ts = TimeSpan.FromSeconds(s);
			else if (m  > 0) ts = TimeSpan.FromMinutes(m);
			else if (h  > 0) ts = TimeSpan.FromHours(h);

			if (inEnglish)
			{
				if (includeMilliseconds)
				{
					if (includeZeroes)
					{
						returnString = ts.Hours.ToString() + " hours " + ts.Minutes.ToString() + " minutes " + ts.Seconds.ToString() + " seconds " + ts.Milliseconds + " milliseconds";
					}
					else
					{
						if      (ts.Hours   > 0) returnString = ts.Hours.ToString() + " hours " + ts.Minutes.ToString() + " minutes " + ts.Seconds.ToString() + " seconds " + ts.Milliseconds + " milliseconds";
						else if (ts.Minutes > 0) returnString = ts.Minutes.ToString() + " minutes " + ts.Seconds.ToString() + " seconds " + ts.Milliseconds + " milliseconds";
						else if (ts.Seconds > 0) returnString = ts.Seconds.ToString() + " seconds " + ts.Milliseconds + " milliseconds";
					}
				}
				else
				{
					if (includeZeroes)
					{
						returnString = ts.Hours.ToString() + " hours " + ts.Minutes.ToString() + " minutes " + ts.Seconds.ToString() + " seconds";
					}
					else
					{
						if      (ts.Hours   > 0) returnString = ts.Hours.ToString() + " hours " + ts.Minutes.ToString() + " minutes " + ts.Seconds.ToString() + " seconds";
						else if (ts.Minutes > 0) returnString = ts.Minutes.ToString() + " minutes " + ts.Seconds.ToString() + " seconds";
						else if (ts.Seconds > 0) returnString = ts.Seconds.ToString() + " seconds";
					}
				}
			}

			else
			{
				if (includeMilliseconds)
				{
					if (includeZeroes)
					{
						returnString = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
					}
					else
					{
						if      (ts.Hours   > 0) returnString = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
						else if (ts.Minutes > 0) returnString = string.Format("{0:D2}m:{1:D2}s:{2:D3}ms", ts.Minutes, ts.Seconds, ts.Milliseconds);
						else if (ts.Seconds > 0) returnString = string.Format("{0:D2}s:{1:D3}ms", ts.Seconds, ts.Milliseconds);
					}
				}
				else
				{
					if (includeZeroes)
					{
						returnString = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:", ts.Hours, ts.Minutes, ts.Seconds);
					}
					else
					{
						if      (ts.Hours   > 0) returnString = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:", ts.Hours, ts.Minutes, ts.Seconds);
						else if (ts.Minutes > 0) returnString = string.Format("{0:D2}m:{1:D2}s:", ts.Minutes, ts.Seconds);
						else if (ts.Seconds > 0) returnString = string.Format("{0:D2}s:", ts.Seconds);
					}
				}
			}

			return returnString;
		}

        /// <summary>
        /// Provides standard formatting for eNum names
        /// Uses '_' as token marker
        /// '_open_' becomes '('
        /// '_close_' becomes ')'
        /// '_slash_' becomes '/'
        /// '_pcent_' becomes '%'
        /// '_comma_' becomes ','
        /// '_dot_' becomes '.'
        /// '_' becomes ' '
        /// All tokens must be in lowercase
        /// Also supported is '_hnn_' where nn is hex code for the required character e.g. '_h20_' = space, '_h25_' = '%' .... '_h7d_' = '}'
        /// </summary>
        /// <param name="eNumRawValue">The string name of the enum value</param>
        /// <returns>formatted string including slash, commas, periods and embedded spaces (or any ASCII char)</returns>
        public static string PrettyEnum(string eNumRawValue)
        {
            if (!eNumRawValue.Contains('_')) return eNumRawValue;   // quick return for the bulk of cases
            Regex r = new Regex("_h[0-9a-f]{2,2}_");
            int nAt = -1;
            eNumRawValue += "      "; // ensure always enough trailing
            while (true)
            {   nAt=eNumRawValue.IndexOf("_h");
                if (nAt<0) break;
                string token = eNumRawValue.Substring(nAt, 5);
                if (r.IsMatch(token))
                {
                    eNumRawValue = eNumRawValue.Replace(token, HttpUtility.UrlDecode("%" + token.Substring(2, 2)));
                }
                else
                {
                    eNumRawValue = eNumRawValue.Replace(token, token.Replace("_"," "));  // if not recognised, just remove the "_"
                }
            }
            // care - don't add a literal token that starts with _h - it will get clobbered before it gets here
            return eNumRawValue.Replace("_open_","(").Replace("_close_", ")").Replace("_slash_", "/").Replace("_pcent_", "%").Replace("_comma_", ",").Replace("_dot_", ".").Replace("_", " ").Trim();
        }

        /// <summary>
        /// Formats date with ordinals
        /// placeholder of 'oo' is translated to 'st','nd', 'rd' or 'th'
        /// </summary>
        /// <example>input of 2/12/2011 with format 'doo MMM yyyy' gives '2nd Dec 2011'</example>
        /// <param name="dateTime">input datetime</param>
        /// <param name="format">standard custom format strings with "oo" as placeholder</param>
        /// <returns>formatted datetime string</returns>
        public static string SetOrdinal(DateTime dateTime, string format)
        {
            string thstndrd = "th";
            if (dateTime.Day < 10 || dateTime.Day > 20)
            {
                if (dateTime.Day % 10 == 1) thstndrd = "st";
                if (dateTime.Day % 10 == 2) thstndrd = "nd";
                if (dateTime.Day % 10 == 3) thstndrd = "rd";
            }
            return dateTime.ToString(format).Replace("oo",thstndrd);
        }

        /// <summary>
        /// Provides a sortable string from a DateTime? including the time
        /// </summary>
        /// <param name="dateTime">Input</param>
        /// <returns>Sortable string, with nulls being forced to the beginning</returns>
        public static string NullDateTimeSortTxt(DateTime? dateTime)
        {
            return NullDateTxt(dateTime, "yyyyMMddHHmmss", "17520914000000");
        }

        /// <summary>
        /// Provides a sortable string from a DateTime? excluding the time
        /// </summary>
        /// <param name="dateTime">Input</param>
        /// <returns>Sortable string, with nulls being forced to the beginning</returns>
        public static string NullDateSortTxt(DateTime? dateTime)
        {
            return NullDateTxt(dateTime, "yyyyMMdd", "17520914");
        }

        /// <summary>
        /// Provides standard formatting for nullable DateTimes
        /// </summary>
        /// <param name="dateTime">Input</param>
        /// <returns>Default format of 'd MMM yy' or '-' if null input</returns>
        public static string NullDateTxt(DateTime? dateTime)
        {
            return NullDateTxt(dateTime, "d MMM yy");
        }

        /// <summary>
        /// Provides standard formatting for nullable DateTimes
        /// </summary>
        /// <param name="dateTime">Input</param>
        /// <param name="format">Standard DateTime format string</param>
        /// <returns>Formatted string or '-' if null input</returns>
        public static string NullDateTxt(DateTime? dateTime, string format)
        {
            return NullDateTxt(dateTime, format,"- not set -");
        }

        /// <summary>
        /// Provides standard formatting for nullable DateTimes
        /// </summary>
        /// <param name="dateTime">Input</param>
        /// <param name="format">Standard DateTime format string</param>
        /// <param name="nullText">String to substitute for null input</param>
        /// <returns>Formatted string</returns>
        public static string NullDateTxt(DateTime? dateTime, string format, string nullText)
        {
            return NullDateTxt(dateTime, format, nullText, false);
        }

        /// <summary>
        /// Provides standard formatting for nullable DateTime objects
        /// </summary>
        /// <param name="dateTime">
        /// DateTime? object to be formatted
        /// </param>
        /// <param name="format">
        /// string giving display format (examples "yy MMM dd HH:mm:ss"  "d MMM yyyy HH:mm:ss" )
        /// </param>
        /// <param name="nullText">
        /// string to substitute for null input (example "--- not set ---")
        /// this value is also returned if the year is earlier than 1901 (assumption being that 1900 might be used as a fudge instead of null)
        /// </param>
        /// <param name="hideMidnight">
        /// true to suppress 00:00:00 times where data may have a mix of DateTime?'s with and without a time part</param>
        /// <returns>
        /// string formatted as required
        /// </returns>
        public static string NullDateTxt(DateTime? dateTime, string format, string nullText, bool hideMidnight)
        {
            if (!dateTime.HasValue) return nullText;
            if (dateTime.Value.Year < 1901) return nullText;
            if ((hideMidnight) && (dateTime.Value.ToString("HHmmss") == "000000"))
            {
                format = format.Replace("HH:mm:ss", "").Replace("HHmmss", "").Trim();
            }
            return dateTime.Value.ToString(format);
        }

        /// <summary>
        /// Provides standard formatting for DbParameter objects
        /// </summary>
        /// <param name="parameter">source object</param>
        /// <returns>string formatted as required</returns>
        public static string DatabaseParameter(DbParameter parameter)
        {
            string reply = parameter.ParameterName.PadRight(25);

            if (parameter.DbType.ToString() == "Int32" && parameter.Value.ToString().Length==0) return reply + " contains null";

            string reply2 = parameter.DbType.ToString() + (parameter.Size == 0
                ? ""
                : " " + parameter.Size.ToString().PadLeft(3));

            if (parameter is System.Data.Odbc.OdbcParameter)
            {
                System.Data.Odbc.OdbcParameter p = (System.Data.Odbc.OdbcParameter)    parameter;
                reply2 += (p.Precision == 0 ? "" : "." + p.Precision.ToString());
            }
            if (parameter is System.Data.OleDb.OleDbParameter)
            {
                System.Data.OleDb.OleDbParameter p = (System.Data.OleDb.OleDbParameter)  parameter;
                reply2 += (p.Precision == 0 ? "" : "." + p.Precision.ToString());
            }
            if (parameter is System.Data.SqlClient.SqlParameter)
            {
                System.Data.SqlClient.SqlParameter p = (System.Data.SqlClient.SqlParameter)parameter;
                reply2 += (p.Precision == 0 ? "" : "." + p.Precision.ToString());
            }
            if (parameter is System.Data.OracleClient.OracleParameter)
            {
                System.Data.OracleClient.OracleParameter p = (OracleParameter)parameter;
                reply2 += " Direction: " + p.Direction.ToString() + "  OracleType: " + p.OracleType.ToString();
            }

            //if (param is Oracle.DataAccess.Client.OracleParameter)
            //{   Oracle.DataAccess.Client.OracleParameter p = (Oracle.DataAccess.Client.OracleParameter)param;
            //    reply2+=" Direction: "+p.Direction.ToString()+"  OracleDbType: "+p.OracleDbType.ToString();
            //}

            reply += ("(" + reply2 +  ")").PadRight(12);

            if (parameter.Value == null) return reply + " contains null";

            string v=parameter.Value.ToString();

            if (parameter.DbType.ToString() == "DateTime") return reply +" = " + v.Replace(" 00:00:00","");
            if (parameter.DbType.ToString() != "String") return reply +" = " + v;

            if (v.Length == 0) return reply + " is empty";

            return reply + v.Length.ToString().PadLeft(3) + " chars \"" + v + "\"";
        }

        //public static string Developer(string x)
        //{
        //    if (x == null) return "";
        //    if (HttpContext.Current == null) return x;
        //    object user = HttpContext.Current.Session["currentUser"];
        //    if (user == null) return x;
        //    if (!(user is CurrentUser)) return x;
        //    if (!((user as CurrentUser).IsDeveloper)) return x;
        //    if (x.Length < 4) return x;
        //    return x.Substring(0, 3) + (new string('x', x.Length-3));
        //}

        /// <summary>
        /// Convert names by capitalising first char of each word.
        /// Understands 'and', 'of', 'to' not capitalised.
        /// Keeps d', de, der and van in lower for French, Dutch and German names.
        /// DOES NOT handle MacDonald or McDonald or MacCoy - impossible !
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Proper(string input)
        {
            if (input == null) return null;
            if (input.Length == 0) return "";
            if (input.Length == 1) return input.ToUpper();

            string inputU = input.ToUpper();
            char[] chars = input.ToLower().ToCharArray();
            chars[0] = inputU[0];

            for (int i = 0; i < input.Length-1; i++)
            {
                char a = chars[i];
                if (a == ' ' || a == '-' || a == '\'' || a == '.' || a == ',' || a == '(' || a == ')') chars[i+1] = inputU[i+1];
            }
            string output = new string(chars);

            return output.Replace(" And "," and ").Replace(" Of "," of ").Replace(" To "," to ").Replace(" De "," de ").Replace(" Van "," van ").Replace(" Der "," der ").Replace(" D'"," d'");
        }
    }

    public static class StringExtra
    {

        public static bool ulContains(this string source, string toCheck, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            return ulIndexOf(source,toCheck,comparison) >= 0;
        }

        public static int ulIndexOf(this string source, string toCheck, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            return source.IndexOf(toCheck, comparison);
        }

        public static string ulReplace(this string source, string oldText, string newText, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            int nAT=source.ulIndexOf(oldText, comparison);
            if (nAT > -1)
            {
                string replace = source.Substring(nAT, oldText.Length);
                return source.Replace(replace, newText);
            }
            return source;
        }

    }








	public class DCC_TimeOfDay
    {
        public TimeSpan ts;
        public string tsTxt;
        public bool isValid;

        public DCC_TimeOfDay(string hh_comma_mm)
        {
            tsTxt = "??:??";
            ts = new TimeSpan(1, 0, 0, 0);
            isValid = false;

            if (hh_comma_mm == null || !hh_comma_mm.Contains(',')) return;

            string[] timeTokens = hh_comma_mm.Split(',');
            tsTxt = (timeTokens[0].PadLeft(2, '0') + ":" + timeTokens[1].PadLeft(2, '0')).Replace("-1", "??");
            int h = 0;
            int m = 0;
            if (!int.TryParse(timeTokens[0], out h)) return;
            if (!int.TryParse(timeTokens[1], out m)) return;
            ts = new TimeSpan(h, m, 0);
            isValid = true;
        }
        public DCC_TimeOfDay(TimeSpan hhmm)
        {
            ts = hhmm;
            tsTxt = hhmm.ToString(@"hh\:mm");
            isValid = ts.TotalDays<1;
        }
        public override string ToString()
        {
            if (isValid) return tsTxt;
            return "no time";
        }

    }

    public class DCC_TimeOfDayBinder : IModelBinder
    {

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            string key = bindingContext.ModelName;
            var value = bindingContext.ValueProvider.GetValue(key);
            if (value == null || string.IsNullOrEmpty(value.AttemptedValue)) return null;

            bindingContext.ModelState.SetModelValue(key, value);

            string[] inputTokens = (string[])value.RawValue;

            return new DCC_TimeOfDay(inputTokens[0]+","+inputTokens[1]);
        }

    }


}
