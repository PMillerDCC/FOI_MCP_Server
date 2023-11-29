using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.IO;

namespace DCC_Standard
{

	public class DCC_CSV
	{
		public string exception = "";
		public int createdRows = 0;

		public DCC_CSV(List<object> list, string filename = "New.csv", string propertiesToIgnore = "")
		{
			string temporaryFile = Path.GetTempFileName();
			propertiesToIgnore = "|" + propertiesToIgnore +"|";

			if (!filename.Trim().ToLower().EndsWith(".csv")) filename += ".csv";

			if (list.Count == 0)
			{
				exception = "Failed to create " + filename + ":  No rows in List";
				return;
			}

			try
			{
				HttpContext.Current.Response.Clear();
				HttpContext.Current.Response.ContentType = "application/ms-excel";
				HttpContext.Current.Response.AddHeader("Content-Disposition", "inline; filename=" + filename);

				StreamWriter sw = new StreamWriter(temporaryFile);

				#region Write Header Row
                string delimiter = "";
				Type type = list[0].GetType();
				PropertyInfo[] properties = type.GetProperties();
				foreach (PropertyInfo property in properties)
				{
					if (!propertiesToIgnore.Contains("|" + property.Name + "|"))
					{
						sw.Write(delimiter + "\"" + property.Name.Replace("\"", "\"\"") + "\"");
						delimiter = ",";
					}
				}
                sw.WriteLine();
                sw.WriteLine();
				#endregion

				#region Write Body
				foreach (object o in list)
				{
                    delimiter = "";
					type = o.GetType();
					properties = type.GetProperties();
					foreach (PropertyInfo property in properties)
					{
						if (!propertiesToIgnore.Contains("|" + property.Name + "|"))
						{
							var value = property.GetValue(o);
							string line = ((value == null) ? "" : value.ToString());
							sw.Write(delimiter + "\"" + line.Replace("\"", "\"\"") + "\"");
							delimiter = ",";
						}
					}
                    sw.WriteLine();
					createdRows++;
				}
				#endregion

				sw.Flush();
				sw.Close();

				HttpContext.Current.Response.WriteFile(temporaryFile);
				HttpContext.Current.Response.End();
			}

			catch (Exception e)
			{
				exception = "Failed to create " + filename + ":  " + e.Message;
			}
			
		}

	}

}
