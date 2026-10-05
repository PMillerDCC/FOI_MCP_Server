using Microsoft.Reporting.WebForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace DCC_Standard
{
    [Serializable]
    public struct NamedParameter
    {
        public string name;
        public object value;
        public string type;
        public string defaultValue;
        public string prompt;
        public string[] opts;
        public string[] vals;
        public string ddlClass;
        public bool noUserInput;
        public string controlID;

        public NamedParameter(string pName, object pValue)
        {
            name = pName;
            value = pValue;
            type = "";
            defaultValue = "";
            prompt = "";
            controlID = "";
            noUserInput = false;
            opts = null;
            vals = null;
            ddlClass = "";
        }
        public NamedParameter(string pName, string pType, string pDefault,string pPrompt,string pNoUserInput,string pOpts,string pVals,string pDDL_Class)
        {
            name = pName??"Unnamed";
            value = null;
            type = pType??"Text";
            defaultValue = pDefault??"";
            prompt = pPrompt??"";
            noUserInput = ((pNoUserInput??"").ToUpper() == "YES");
            controlID = "";
            if (prompt.Length == 0) prompt = name;
            opts = null;
            vals = null;
            if (pOpts.Length > 0) opts = pOpts.Split('|');
            if (pVals.Length > 0) vals = pVals.Split('|');
            ddlClass = pDDL_Class;
        }
        public override string ToString()
        {
            return name+(type!=null?"  type:"+type:"")+(defaultValue!=null?"  default:"+defaultValue:"")+(value!=null?"  value:"+value.ToString():"");
        }
    }
    [Serializable]
    public class NamedParameterList : IEnumerable<NamedParameter>
    {

		// Properties

        Dictionary<string, NamedParameter> plist = new Dictionary<string, NamedParameter>();
        


		// Utilities

		public int Count { get { return plist.Count; } }



		// Methods

        public bool ContainsKey(string key) { return plist.ContainsKey(key); }
        public void Add(NamedParameter pValue) { plist.Add(pValue.name, pValue); }

        public NamedParameter this[string key]
        {
            get
            {
                if (!plist.ContainsKey(key)) throw new Exception("Cannot find NamedParameter key ["+key+"]");
                return plist[key];
            }
            set
            {
                if (!plist.ContainsKey(key)) throw new Exception("Cannot find NamedParameter key ["+key+"]");
                plist[key]=value;
            }
        }

        public NamedParameter this[int seqNo]
        {
            get
            {
                if (seqNo < 0 || seqNo >= Count) throw new Exception("Cannot find NamedParameter index [" + seqNo + "]");
                return plist.ElementAt(seqNo).Value;
            }
        }



		// Enumerator

        public IEnumerator<NamedParameter> GetEnumerator() { foreach (object o in plist.Values) yield return (NamedParameter)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in plist.Values) yield return "dummy"; }        

    }

    [Serializable]
    public class DCC_Report
    {
        public string ID;
        public string fileStub;
        public string buttonText;
        public string description;
        public string systemHead;
        public string reportHead;
        public string saveAs;
        public string assembly;
        public string reportClass;
        public string objectClass;
        public string orientation;
        public string heading;
        public string footing;
        public string paramSet;
        public string menuSeq;
        public string[] permittedUsers;
        public NamedParameterList queryParameters = new NamedParameterList();
        public string[] pageLayout;


        public void parse(DataSet ds)
        {
            DataTable dt = ds.Tables["DCC_ReportParams"];
            DataRow dr = dt.Rows[0];

            try
            {
                systemHead = fetchElement(dr, "SystemHead");
                buttonText = fetchElement(dr, "ReportButton");
                description = fetchElement(dr, "ReportDescription");
                systemHead = fetchElement(dr, "SystemHead");
                reportHead = fetchElement(dr, "ReportHead");
                saveAs = fetchElement(dr, "SaveAs");
                assembly = fetchElement(dr, "Assembly");
                reportClass = fetchElement(dr, "ReportClass");
                objectClass = fetchElement(dr, "BusinessObjects");
                orientation = fetchElement(dr, "Orientation");
                heading = fetchElement(dr, "Heading");
                footing = fetchElement(dr, "Footing");
                paramSet = fetchElement(dr, "ParameterSet");
                menuSeq = fetchElement(dr, "MenuSeq");

                permittedUsers = fetchElement(dr, "PermittedUsers").Split(new char[] { ',', ';', '|' });
            }
            catch (Exception innerEx)
            {
                throw new ArgumentException("Format error in DCC_RDLC report definition file. The XML file is probably missing an element.", innerEx);
            }
            if (description.Length == 0) description = reportHead;
            if (saveAs.Length == 0) saveAs = fileStub;
            if (orientation.Length == 0) orientation = "Portrait";
            if (paramSet.Length == 0) paramSet = "Std_Parameters";
            if (heading.Length == 0) heading = "Std_PageHeader";
            if (footing.Length == 0) footing = "Std_PageFooter";

            dt = ds.Tables["QueryParam"];
            if (dt != null)
            {
                foreach (DataRow drQ in dt.Rows)
                {
                    string pName = (string)fetchOptionalColumn(drQ, "pName");
                    string pType = (string)fetchOptionalColumn(drQ, "pType");
                    string pDefault = (string)fetchOptionalColumn(drQ, "pDefault");
                    string pPrompt = (string)fetchOptionalColumn(drQ, "pPrompt");
                    string pStatic = (string)fetchOptionalColumn(drQ, "pStatic");
                    string pOpts = (string)fetchOptionalColumn(drQ, "pOpts");
                    string pVals = (string)fetchOptionalColumn(drQ, "pVals");
                    string pFillDDLSource = (string)fetchOptionalColumn(drQ, "pDDLclass");

                    queryParameters.Add(new NamedParameter(pName, pType, pDefault, pPrompt, pStatic, pOpts, pVals, pFillDDLSource));
                }
            }


        }

        private string fetchElement(DataRow dr, string elementName)
        {
            if (!dr.Table.Columns.Contains(elementName)) return "";
            object a = dr[elementName];
            if (a == null) return "";
            return (string)a;
        }

        private object fetchOptionalColumn(DataRow dr, string pColName)
        {
            if (!dr.Table.Columns.Contains(pColName)) return "";
            object r=dr[pColName];
            if (r is DBNull) return "";
            return r;
        }

        public Dictionary<string, List<object>> getRows(DCC_Database di, NamedParameterList optParams)
        {
            Dictionary<string, List<object>> result = new Dictionary<string, List<object>>();

            if (assembly == "*NODATA*") return null;

            if (di == null) return null;

            try
            {
                MemberInfo[] fy = null;
                try
                {
                    fy = Type.GetType(reportClass + "," + assembly).GetMembers();
                }
                catch
                {
                    throw new Exception("Unable to locate report class [" + reportClass + "] in assembly [" + assembly + "]");
                }

                foreach (MemberInfo mi in fy)
                {
                    if (mi.Name == ".ctor")
                    {
                        ConstructorInfo ctor = (ConstructorInfo)mi;
                        object x = ctor.Invoke(new object[] { di, optParams });

                        foreach (MemberInfo mj in fy)
                        {
                            if (mj.Name.StartsWith("Rows"))
                            {
                                System.Reflection.FieldInfo i = (System.Reflection.FieldInfo)mj;
                                object y = i.GetValue(x);
                                result.Add(mj.Name, (List<object>)y);
                            }
                        }
                        if (result.Count > 0) return result;
                        throw new Exception("Unable to locate field [Rows] for report class [" + reportClass + "]");
                    }
                }
                throw new Exception("Unable to locate constructor for report class [" + reportClass + "]");
            }
            catch (Exception innerEx)
            {
                throw new Exception("Unable to get data rows for report class [" + reportClass + "] in assembly [" + assembly + "].", innerEx);
            }
        }

        public int Prepare(Microsoft.Reporting.WebForms.ReportViewer targetReportControl, string[] queryParams, DCC_Standard.DCC_Database di)
        {
            List<string> reportRows = new List<string>();
            reportRows.Add(objectClass + "," + assembly);

            pageLayout = reportHelpers.prepareReport(
                targetReportControl,
                HttpContext.Current.Server.MapPath(@".\Reporting\"),
                fileStub,
                systemHead,
                reportHead,
                reportRows,
                getRows(di, queryParameters),
                queryParams,
                assembly + "." + reportClass + ", " + assembly + ", Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
                heading,
                footing,
                paramSet
                );

            int nRows = 0;
            foreach (ReportDataSource rds in targetReportControl.LocalReport.DataSources)
            {
                if (rds != null)
                {
                    object rdsData = rds.Value;
                    if (rdsData != null)
                    {
                        if (rdsData is DataTable) { nRows += (rdsData as DataTable).Rows.Count; }
                        if (rdsData is List<object>) { nRows += (rdsData as List<object>).Count; }
                    }
                }
            }
            return nRows;
        }
    }

    public static class reportHelpers
    {
        public static string[] prepareReport(ReportViewer rv,string folder,string reportName, string systemHeading, string reportHeading, List<string> rowTypeNames, Dictionary<string,List<object>> reportRows, string[] queryReplies, string reportObject)
        {   
            return prepareReport(rv,folder,reportName, systemHeading, reportHeading, rowTypeNames, reportRows,queryReplies,reportObject,"Std_PageHeader","Std_PageFooter","Std_Parameters");
        }

        /// <summary>
        /// Prepare Report from these parameters
        /// </summary>
        /// <param name="rv">ReportViewer control to attach report to</param>
        /// <param name="folder">Folder on server containing report definitions</param>
        /// <param name="reportName">filename of the .RDLC file</param>
        /// <param name="systemHeading">Main Heading - usually the name of the software system</param>
        /// <param name="reportHeading">Specific Report Heading</param>
        /// <param name="rowTypeNames">Class names for one or more row types used in the report</param>
        /// <param name="reportData">List of the fields available</param>
        /// <param name="queryReplies">Replies to the optional parameters</param>
        /// <param name="reportObject">-unused-</param>
        /// <param name="headerStyle">Name of HeaderStyle</param>
        /// <param name="footerStyle">Name of FooterStyle</param>
        /// <param name="parameterStyle">Name of ParameterStyle</param>
        /// <returns></returns>
        public static string[] prepareReport(ReportViewer rv,string folder,string reportName, string systemHeading, string reportHeading, 
                                             List<string> rowTypeNames, Dictionary<string,List<object>> reportData, string[] queryReplies,
                                             string reportObject,string headerStyle,string footerStyle,string parameterStyle)
        {
            string dataSource = "DataSource1";

            bool needsRewrite = false;

            string rpt = File.ReadAllText(folder + reportName+ ".rdlc");


            #region populate standard parameters, header and footer
            needsRewrite |= replaceXMLsection("<ReportParameters>", folder,parameterStyle, ref rpt);
            needsRewrite |= replaceXMLsection("<PageHeader>", folder,headerStyle, ref rpt);
            needsRewrite |= replaceXMLsection("<PageFooter>", folder,footerStyle, ref rpt);
            #endregion

            Dictionary<string, DataTable> reportDataSources = new Dictionary<string, DataTable>();

            #region populate current fields schema

            if (reportData != null)
            {
                string dataSets = subString(rpt, "<DataSets>", false);
                //            string fields = subString(dataSets, "<Fields>", false);

                StringBuilder newDataSets = new StringBuilder();

                int dataN = -1;
                foreach (string rowTypeName in rowTypeNames)
                {
                    dataN++;
                    string dataSetName = "reportData" + (dataN == 0 ? "" : dataN.ToString());

                    DataTable target = new DataTable(dataSetName);
                    reportDataSources.Add(dataSetName, target);
                    List<string> field = new List<string>();
                    List<System.Reflection.MemberInfo> members = new List<System.Reflection.MemberInfo>();

                    MemberInfo[] fx = null;
                    try
                    {
                        fx = Type.GetType(rowTypeName).GetMembers();
                    }
                    catch
                    {
                        throw new Exception("Unable to locate businessObject [" + rowTypeName + "]");
                    }

                    foreach (System.Reflection.MemberInfo fi in fx)
                    {
                        string DataName = "";
                        Type DataType = null;
                        switch (fi.MemberType.ToString())
                        {
                            case "Property":
                                {
                                    System.Reflection.PropertyInfo i = (System.Reflection.PropertyInfo)fi;
                                    DataType = i.PropertyType;
                                    DataName = i.Name;
                                    break;
                                }
                            case "Field":
                                {
                                    System.Reflection.FieldInfo i = (System.Reflection.FieldInfo)fi;
                                    DataType = i.FieldType;
                                    DataName = i.Name;
                                    break;
                                }
                        }

                        if (DataName.Length == 0) continue;


                        switch (DataType.ToString())
                        {
                            case "System.Boolean":
                            case "System.Char":
                            case "System.String":
                            case "System.Byte":
                            case "System.Short":
                            case "System.Int32":
                            case "System.Long":
                            case "System.Float":
                            case "System.Single":
                            case "System.Decimal":
                            case "System.Double":
                            case "System.DateTime":
                                {
                                    target.Columns.Add(DataName, DataType);
                                    members.Add(fi);
                                    field.Add(@"        <Field Name=""" + DataName + @""">");
                                    field.Add(@"          <DataField>" + DataName + @"</DataField>");
                                    field.Add(@"          <rd:TypeName>" + DataType.ToString() + "</rd:TypeName>");
                                    field.Add(@"        </Field>");
                                    break;
                                }
                            case "System.Nullable`1[System.Char]":
                            case "System.Nullable`1[System.Byte]":
                            case "System.Nullable`1[System.Short]":
                            case "System.Nullable`1[System.Long]":
                            case "System.Nullable`1[System.Double]":
                            case "System.Nullable`1[System.Float]":
                            case "System.Nullable`1[System.Int32]":
                            case "System.Nullable`1[System.Boolean]":
                            case "System.Nullable`1[System.DateTime]":
                            case "System.Nullable`1[System.Decimal]":
                            default:
                                {
                                    // not expecting ushort, uint or ulong
                                    // everything else is not a simple type, so ignore
                                    break;
                                }
                        }
                    }

                    string key = reportData.Keys.ElementAt(dataN);
                    foreach (object rr in reportData[key])
                    {
                        object[] cols = new object[target.Columns.Count];
                        for (int colN = 0; colN < target.Columns.Count; colN++)
                        {
                            if (members[colN] is FieldInfo) cols[colN] = (members[colN] as FieldInfo).GetValue(rr);
                            if (members[colN] is PropertyInfo) cols[colN] = (members[colN] as PropertyInfo).GetValue(rr, null);
                        }
                        target.Rows.Add(cols);
                    }

                    newDataSets.AppendLine(@"
    <DataSet Name=""" + dataSetName + @""">
      <Fields>");
                    foreach (string s in field) newDataSets.AppendLine(s);
                    newDataSets.Append(@"      </Fields>
      <Query>
        <DataSourceName>" + dataSource + @"</DataSourceName>
        <rd:UseGenericDesigner>true</rd:UseGenericDesigner>
        <CommandText>/* Local Query */</CommandText>
        </Query>
      <rd:DataSetInfo>
        <rd:DataSetName>" + dataSetName + @"</rd:DataSetName>
        <rd:TableName>reportData</rd:TableName>
        <rd:TableAdapterFillMethod />
        <rd:TableAdapterGetDataMethod />
        <rd:TableAdapterName />
      </rd:DataSetInfo>
    </DataSet>
   ");
                }

                if (dataSets.Length > 0)
                {
                    needsRewrite |= (dataSets != newDataSets.ToString());
                    rpt = rpt.Replace(dataSets, newDataSets.ToString());
                }
            }
            #endregion

            if (needsRewrite)
            {
                string fileOnDisk = folder + reportName + ".rdlc";
                FileAttributes fa= File.GetAttributes(fileOnDisk);
                bool isReadOnly=((fa & FileAttributes.ReadOnly)==FileAttributes.ReadOnly);
                if (isReadOnly)
                {
                    fa = fa & ~FileAttributes.ReadOnly;
                    File.SetAttributes(fileOnDisk, fa);
                }
                File.WriteAllText(fileOnDisk, rpt);
                if (isReadOnly)
                {
                    fa = fa | FileAttributes.ReadOnly | FileAttributes.Archive;
                    File.SetAttributes(fileOnDisk, fa);
                }

            }

            string PageWidth    =subString(rpt, "<PageWidth>",    false);
            string PageHeight   =subString(rpt, "<PageHeight>",   false);
            string TopMargin    =subString(rpt, "<TopMargin>",    false);
            string LeftMargin   =subString(rpt, "<LeftMargin>",   false); 
            string RightMargin  =subString(rpt, "<RightMargin>",  false);
            string BottomMargin =subString(rpt, "<BottomMargin>", false);

            string[] pageLayout=new string[] {PageWidth,PageHeight,TopMargin,LeftMargin,RightMargin,BottomMargin};

            rv.LocalReport.LoadReportDefinition(new StringReader(rpt));

            List<ReportParameter> rptParams = new List<ReportParameter>(8);

            ReportParameterInfoCollection rqrdParams = rv.LocalReport.GetParameters();
            foreach (ReportParameterInfo rpi in rqrdParams)
            {
                if (rpi.Name == "Report_Parameter_0")          rptParams.Add(new ReportParameter("Report_Parameter_0", queryReplies[0]));
                if (rpi.Name == "Report_Parameter_1")          rptParams.Add(new ReportParameter("Report_Parameter_1", queryReplies[1]));
                if (rpi.Name == "Report_Parameter_2")          rptParams.Add(new ReportParameter("Report_Parameter_2", queryReplies[2]));
                if (rpi.Name == "Report_Parameter_3")          rptParams.Add(new ReportParameter("Report_Parameter_3", queryReplies[3]));
                if (rpi.Name == "Report_Parameter_4")          rptParams.Add(new ReportParameter("Report_Parameter_4", queryReplies[4]));
                if (rpi.Name == "Report_Parameter_SystemHead") rptParams.Add(new ReportParameter("Report_Parameter_SystemHead", systemHeading));
                if (rpi.Name == "Report_Parameter_ReportHead") rptParams.Add(new ReportParameter("Report_Parameter_ReportHead", reportHeading));
                if (rpi.Name == "Report_Parameter_SubHead")    rptParams.Add(new ReportParameter("Report_Parameter_SubHead", ""));
            }

            if (rptParams.Count>0) rv.LocalReport.SetParameters(rptParams);

            rv.LocalReport.DataSources.Clear();
            for (int t = 0; t < reportDataSources.Count; t++)
            {
                string k=reportDataSources.Keys.ElementAt(t);
                Microsoft.Reporting.WebForms.ReportDataSource rds=new Microsoft.Reporting.WebForms.ReportDataSource(k,reportDataSources[k]);
                rv.LocalReport.DataSources.Add(rds);
            }

            return pageLayout;
        }

        private static bool replaceXMLsection(string xmlToken, string folder,string styleFile, ref string rptText)
        {
            if (styleFile == "*LEAVE*") return false; // explicit instruction from DCC_RDLC to leave alone
            if (styleFile.Length == 0) return false;

            string styleFileName = folder + styleFile + ".TXT";
            try
            {
                string xml = subString(rptText, xmlToken, true).Trim();
                if (xml.Length == 0) return false; // nothing to replace

                string newxml = File.ReadAllText(styleFileName).Trim();
                if (xml == newxml) return false; // no change needed
                rptText = rptText.Replace(xml, newxml);
            }
            catch 
            {   throw new Exception("Unable to locate file containing "+xmlToken+" Filename given was ["+styleFileName+"].");
            }

            return true;
        }

        static string subString(string input, string startXMLToken, bool includeStartEnd)
        {
            string endXMLToken = "</" + startXMLToken.Substring(1);
            return subString(input, startXMLToken, endXMLToken, includeStartEnd);
        }

        static string subString(string input, string starts, string ends, bool includeStartEnd)
        {
            int nAt0 = input.IndexOf(starts);
            if (nAt0 == -1) return "";
            int nAt9 = input.IndexOf(ends,nAt0);
            if (nAt9 == -1) return "";
            if (includeStartEnd) return input.Substring(nAt0, nAt9 - nAt0+ends.Length);
            return input.Substring(nAt0+starts.Length, nAt9 - nAt0-starts.Length);
        }



        public static string renderReport(System.Web.HttpResponse pageResponse, ReportViewer rv, string[] pageLayout,string format,string filename,String write2folder="-")
        {
            string[] streams;
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo =
              "<DeviceInfo>" +
              "  <OutputFormat>"+ format        + "</OutputFormat>" +
              "  <PageWidth>"   + pageLayout[0] + "</PageWidth>" +
              "  <PageHeight>"  + pageLayout[1] + "</PageHeight>" +
              "  <MarginTop>"   + pageLayout[2] + "</MarginTop>" +
              "  <MarginLeft>"  + pageLayout[3] + "</MarginLeft>" +
              "  <MarginRight>" + pageLayout[4] + "</MarginRight>" +
              "  <MarginBottom>"+ pageLayout[5] + "</MarginBottom>" +
              "</DeviceInfo>";
            Warning[] warnings;
            byte[] renderedBytes;

            renderedBytes = rv.LocalReport.Render(format, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

            string doubleQuote = @"""";
            string taggedFilename = " (" + DateTime.Now.ToString("d MMM yyyy  HHmmss") + ")." + fileNameExtension;
            string fullFilename = doubleQuote + filename + taggedFilename + doubleQuote;

            if (write2folder != "-")
            {
                string target=write2folder + "\\" + rv.ID+taggedFilename;
                File.WriteAllBytes(target,renderedBytes);
                return target;
            }
            else
            {
                pageResponse.Clear();
                pageResponse.ContentType = mimeType;
                pageResponse.AddHeader("content-disposition", "attachment; filename=" + fullFilename);
                pageResponse.BinaryWrite(renderedBytes);
                pageResponse.End();
                return "by HTTP";
            }
        }

    }
}
