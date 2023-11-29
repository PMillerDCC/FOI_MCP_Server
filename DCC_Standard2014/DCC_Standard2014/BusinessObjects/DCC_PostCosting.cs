using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace DCC_Standard
{


    [Serializable]

    public class DCC_PostCosting
    {

        // Properties

        public string    PostReference;
        public DateTime? PostCostingStartDate;
		public string    PostCostingStartDateText { get { return DCC_Formatting.NullDateTxt(PostCostingStartDate, "dd MMM yyyy"); } }
		public string    PostCostingStartDateSort { get { return DCC_Formatting.NullDateTxt(PostCostingStartDate, "yyyyMMddHHmm"); } }
        public DateTime? PostCostingEndDate;
		public string    PostCostingEndDateText   { get { return DCC_Formatting.NullDateTxt(PostCostingEndDate, "dd MMM yyyy"); } }
		public string    PostCostingEndDateSort   { get { return DCC_Formatting.NullDateTxt(PostCostingEndDate, "yyyyMMddHHmm"); } }
        public string    PostCostingCode;
        public string    PostCostingDescription;
        public string    PostCostCentre;
        public string    PostAccountCode;
        public Single    Post_EES_Percentage;
        public Single    Post_ERS_Percentage;



        // Utilities

        public override string ToString() { return PostReference; }



        // Constructors

        public DCC_PostCosting()
        {
        }

        public DCC_PostCosting(DataRow dr)
        {
            Load(dr);
        }



        // Methods

        private void Load(DataRow dr)
        {
            PostReference          = DCC_GetColumn.String    (dr, "POST_REF");
            PostCostingStartDate   = DCC_GetColumn.DateTimeQ (dr, "POST_COSTING_START_DATE");
            PostCostingEndDate     = DCC_GetColumn.DateTimeQ (dr, "POST_COSTING_END_DATE");
            PostCostingCode        = DCC_GetColumn.String    (dr, "POST_COSTING_CODE");
            PostCostingDescription = DCC_GetColumn.String    (dr, "POST_COSTING_DESCRIPTION");
            PostCostCentre         = DCC_GetColumn.String    (dr, "POST_COST_CENTRE");
            PostAccountCode        = DCC_GetColumn.String    (dr, "POST_ACCOUNT_CODE");
            Post_EES_Percentage    = DCC_GetColumn.Float     (dr, "POST_EES_PERC");
            Post_ERS_Percentage    = DCC_GetColumn.Float     (dr, "POST_ERS_PERC");
        }

    }


    [Serializable]

    public class DCC_PostCostingList : IEnumerable<DCC_PostCosting>
    {

        // Properties

        public List<DCC_PostCosting> All = new List<DCC_PostCosting>();



        // Get/set properties

        public string CostCentres { get; set; }



        // Utilities

        public override string ToString() { return "DCC_CostingList (" + Count + " items)"; }
        public int Count { get { return All.Count; } }



        // Constructors

        public DCC_PostCostingList()
        {
        }

        public DCC_PostCostingList(DCC_Session session, string pPostReference = null)
        {
            if (session.ResourceLinkDatabaseConnection != null)
            {
                if (!string.IsNullOrEmpty(pPostReference))
                {
                    DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_costing WHERE post_ref=:pPostReference ORDER BY post_cost_centre");
                    dbf.AddTable("DCC_POST_HIERARCHY_TABLE");
                    dbf.AddParameter("pPostReference", pPostReference, 8);
                    session.ResourceLinkDatabaseConnection.DataLayer.SelectData(dbf);
                    if ((dbf.hasTables) && (dbf.TableZeroRows.Count > 0))
                    {
                        foreach (DataRow dr in dbf.TableZeroRows)
                        {
                            DCC_PostCosting pc = new DCC_PostCosting(dr);
    						CostCentres += pc.PostCostCentre + "-" + pc.PostAccountCode + " (" + pc.Post_EES_Percentage.ToString() + "%), ";
                            All.Add(pc);
                        }
    		            if (CostCentres.Substring(CostCentres.Length -2) == ", ") CostCentres = CostCentres.Substring(0, CostCentres.Length -2);
                    }
                }
            }
        }



        // Enumerator

        public IEnumerator<DCC_PostCosting> GetEnumerator() { foreach (object o in All) yield return (DCC_PostCosting)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }

    }

}
