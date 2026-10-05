using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace DCC_Standard
{


    [Serializable]

    public class DCC_PostHierarchy2
    {

        // Properties

        public string    ManagerPersonReference;
        public string    ManagerPostReference;
        public int       ManagerLevelNo;
        public string    EmployeePersonReference;
        public string    EmployeePostReference;

        public string    PersonReference;
        public string    EmployeeNo;
        public string    Surname;
        public string    KnownAsSurname;
        public string    Forename;
        public string    KnownAsForename;
        public string    OtherForenames;
        public string    PostReference;
        public string    PostNo;
        public string    PostTitle;
        public DateTime? PostStartDate;
        public DateTime? PostEndDate;
        public DateTime? ProjectedEndDate;
        public string    MainPost;
        public DateTime? PositionStatusStartDate;
        public DateTime? PositionStatusEndDate;
        public string    PositionStatus;
        public string    HierarchyLevel1;
        public string    HierarchyLevel2;
        public string    HierarchyLevel3;
        public string    HierarchyLevel4;
        public string    HierarchyLevel5;
        public string    HierarchyLevel6;
        public string    HierarchyLevel7;
        public string    HierarchyLevel8;
        public string    HierarchyLevel9;
        public string    HierarchyLevel10;



        // Display Properties

        public string FullName             { get { return (KnownAsForename == "" ? Forename : KnownAsForename) + " " + (KnownAsSurname == "" ? Surname : KnownAsSurname); } }
        public string FullnameAndPostTitle { get { return FullName + " - " + PostTitle; } }
        public string PostNoTitleAndName   { get { return PostNo + " - " + PostTitle + " - " + ((PersonReference == "00000000") ? "VACANT" : FullName); } }



        // Utilities

        public override string ToString() { return FullName; }



        // Constructors

        public DCC_PostHierarchy2()
        {
        }

        public DCC_PostHierarchy2(DataRow dr)
        {
            Load(dr);
        }



        // Methods

        private void Load(DataRow dr)
        {
            ManagerPersonReference  = DCC_GetColumn.String   (dr, "HIER_CB_MAN_PERSON_REF");
            ManagerPostReference    = DCC_GetColumn.String   (dr, "HIER_CB_MAN_POST_REF");
            ManagerLevelNo          = DCC_GetColumn.Integer32(dr, "HIER_CB_LEVEL_NO");
            EmployeePersonReference = DCC_GetColumn.String   (dr, "HIER_CB_EMP_PERSON_REF");
            EmployeePostReference   = DCC_GetColumn.String   (dr, "HIER_CB_EMP_POST_REF");

            PersonReference         = DCC_GetColumn.String   (dr, "PERSON_REF");
            EmployeeNo              = DCC_GetColumn.String   (dr, "EMPLOYEE_NUMBER");
            Surname                 = DCC_GetColumn.String   (dr, "SURNAME");
            KnownAsSurname          = DCC_GetColumn.String   (dr, "KNOWN_AS_SURNAME");
            Forename                = DCC_GetColumn.String   (dr, "FIRST_FORENAME");
            KnownAsForename         = DCC_GetColumn.String   (dr, "KNOWN_AS_FIRST_FORENAME");
            OtherForenames          = DCC_GetColumn.String   (dr, "OTHER_FORENAMES");
            PostReference           = DCC_GetColumn.String   (dr, "POST_REF");
            PostNo                  = DCC_GetColumn.String   (dr, "POST_NUMBER");
            PostTitle               = DCC_GetColumn.String   (dr, "POST_TITLE");
            PostStartDate           = DCC_GetColumn.DateTime (dr, "PH_START_DATE");
            PostEndDate             = DCC_GetColumn.DateTime (dr, "PH_END_DATE");
            ProjectedEndDate        = DCC_GetColumn.DateTime (dr, "PROJECTED_END_DATE");
            MainPost                = DCC_GetColumn.String   (dr, "MAIN_POST");
            PositionStatusStartDate = DCC_GetColumn.DateTime (dr, "POSITION_STATUS_START_DATE");
            PositionStatusEndDate   = DCC_GetColumn.DateTime (dr, "POSITION_STATUS_END_DATE");
            PositionStatus          = DCC_GetColumn.String   (dr, "POSITION_STATUS");
            HierarchyLevel1         = DCC_GetColumn.String   (dr, "HIERARCHY_LEVEL_1");
            HierarchyLevel2         = DCC_GetColumn.String   (dr, "HIERARCHY_LEVEL_2");
            HierarchyLevel3         = DCC_GetColumn.String   (dr, "HIERARCHY_LEVEL_3");
            HierarchyLevel4         = DCC_GetColumn.String   (dr, "HIERARCHY_LEVEL_4");
            HierarchyLevel5         = DCC_GetColumn.String   (dr, "HIERARCHY_LEVEL_5");
            HierarchyLevel6         = DCC_GetColumn.String   (dr, "HIERARCHY_LEVEL_6");
            HierarchyLevel7         = DCC_GetColumn.String   (dr, "HIERARCHY_LEVEL_7");
            HierarchyLevel8         = DCC_GetColumn.String   (dr, "HIERARCHY_LEVEL_8");
            HierarchyLevel9         = DCC_GetColumn.String   (dr, "HIERARCHY_LEVEL_9");
            HierarchyLevel10        = DCC_GetColumn.String   (dr, "HIERARCHY_LEVEL_10");
        }

    }




    [Serializable]

    public class DCC_PostHierarchy2List : IEnumerable<DCC_PostHierarchy2>
    {

        // Properties

        public List<DCC_PostHierarchy2> All = new List<DCC_PostHierarchy2>();



        // Utilities

        public int Count { get { return All.Count; } }
        public override string ToString() { return "DCC_PostHierarchy2List (" + Count + " items)"; }



        // Constructors

        public DCC_PostHierarchy2List()
        {
        }

        public DCC_PostHierarchy2List(DCC_Session session, string pPersonReference = "")
        {
            if (session.ResourceLinkDatabaseConnection != null)
            {
                DCC_Database db = session.ResourceLinkDatabaseConnection;
                if (pPersonReference != "")
                {
                    DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_mv_rrs_d1544m JOIN dcc_v_myworkforce_estab ON dcc_v_myworkforce_estab.PERSON_REF = dcc_mv_rrs_d1544m.hier_cb_man_person_ref WHERE hier_cb_emp_person_ref = :pPersonReference AND hier_cb_type = :pType AND HIER_CB_LEVEL_NO < 6 ORDER BY HIER_CB_LEVEL_NO");
                    dbf.AddTable("DCC_MV_RRS_P2P_OCC");
                    dbf.AddParameter("pPersonReference", pPersonReference, 8);
                    dbf.AddParameter("pType", "POST", 8);
                    db.DataLayer.SelectData(dbf);
                    if (dbf.hasTables) foreach (DataRow dr in dbf.TableZeroRows) All.Add(new DCC_PostHierarchy2(dr));
                }
            }
        }



        // Enumerator

        public IEnumerator<DCC_PostHierarchy2> GetEnumerator() { foreach (object o in All) yield return (DCC_PostHierarchy2)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }

    }

}
