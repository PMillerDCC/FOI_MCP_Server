using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DCC_Standard
{

    #region DCC_EmployeeAndPost

    [Serializable]
    public class DCC_EmployeeAndPost
    {

        // Properties

        public string    PH_Type;
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
        public DateTime? PostHoldingStartDate;
        public string    PostHoldingStartDateText    { get { return PostHoldingStartDate.ToString(); } }
        public string    PostHoldingEndDateText      { get { return PostHoldingEndDate.ToString(); } }
        public DateTime? PostHoldingEndDate;
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
		public string    HierarchyText               { get
        {
            string s = "";
            s += (HierarchyLevel2);
            s += ( (string.IsNullOrEmpty(HierarchyLevel3)) ? "" : " / " + HierarchyLevel3);
            s += ( (string.IsNullOrEmpty(HierarchyLevel4)) ? "" : " / " + HierarchyLevel4);
            s += ( (string.IsNullOrEmpty(HierarchyLevel5)) ? "" : " / " + HierarchyLevel5);
            s += ( (string.IsNullOrEmpty(HierarchyLevel6)) ? "" : " / " + HierarchyLevel6);
            s += ( (string.IsNullOrEmpty(HierarchyLevel7)) ? "" : " / " + HierarchyLevel7);
            s += ( (string.IsNullOrEmpty(HierarchyLevel8)) ? "" : " / " + HierarchyLevel8);
            s += ( (string.IsNullOrEmpty(HierarchyLevel9)) ? "" : " / " + HierarchyLevel9);
            return s;
        } }
        public string    PostGrade;
        public string    GradeText                   { get { return PostGrade.ToString(); } }
        public decimal   PostHours;
        public decimal   PostHeadCount;
        public string    HeadCountText               { get { return PostHeadCount.ToString(); } }
        public string    PayGroup;
        public string    LocationOfPost;
        public string    LocationOfEmployee;
        public string    PostOccupancy;
        public decimal   ContractedHours;
        public decimal   Salary;
        public string    SalaryText                  { get { return DCC_Formatting.ReportMoney(Salary);} }
        public string    NationalInsuranceNo;
        public decimal   Post_GDPR_RetentionYears;
        public decimal   Employee_GDPR_RetentionYears;

        public string    PreviousPostHolderPersonReference;
        public string    PreviousPostHolderEmployeeNo;
        public string    PreviousPostHolderSurname;
        public string    PreviousPostHolderKnownAsSurname;
        public string    PreviousPostHolderForename;
        public string    PreviousPostHolderKnownAsForename;
        public string    PreviousPostHolderOtherForenames;
        public string    PreviousPostHolderPostReference;
        public string    PreviousPostHolderPostNo;
        public string    PreviousPostHolderPostTitle;
        public string    PreviousPostHolderFullName     { get
        {
            return
                ( (string.IsNullOrEmpty(PreviousPostHolderEmployeeNo)) ? "" :
                    (string.IsNullOrEmpty(PreviousPostHolderKnownAsForename)
                    ? PreviousPostHolderForename
                    : PreviousPostHolderKnownAsForename) + " " +
                    (string.IsNullOrEmpty(PreviousPostHolderKnownAsSurname)
                    ? PreviousPostHolderSurname
                    : PreviousPostHolderKnownAsSurname)
                );
        } }
        public string    PreviousEmployeeNoAndFullName  { get
        {
            return
                ((string.IsNullOrEmpty(PreviousPostHolderEmployeeNo) ? "" : PreviousPostHolderEmployeeNo + " - " + PreviousPostHolderFullName ));
        } }
        public string    PostNoAndPostTitleAndName  { get
        {
            string s = PostNo + " - " + PostTitle;

            if (PersonReference == "00000000")
            {
                s += " - VACANT" + (
                    (string.IsNullOrEmpty(PreviousPostHolderFullName)
                        ? ""
                        : " (" + PreviousPostHolderFullName + ")" ) );
            }
            else
            {
                s += " - " + FullName;
            }

            return s;
        } }

        public string    ManagerPersonReference;
        public string    ManagerEmployeeNo;
        public string    ManagerSurname;
        public string    ManagerKnownAsSurname;
        public string    ManagerForename;
        public string    ManagerKnownAsForename;
        public string    ManagerOtherForenames;
        public string    ManagerPostReference;
        public string    ManagerPostNo;
        public string    ManagerPostTitle;
        public string    ManagerFullName                { get { return (string.IsNullOrEmpty(ManagerKnownAsForename) ? ManagerForename : ManagerKnownAsForename) + " " + (string.IsNullOrEmpty(ManagerKnownAsSurname) ? ManagerSurname : ManagerKnownAsSurname); } }
        public string    ManagerEmployeeNoAndFullName   { get { return ManagerEmployeeNo + " - " + ManagerFullName; } }
        public string    ManagerPostNoAndFullName       { get { return ManagerPostNo + " - " + ManagerFullName; } }

        public string    LevelNo;
        public string    Level1;
        public string    Level1PostNo;
        public string    Level1Name;
        public string    Level2;
        public string    Level2PostNo;
        public string    Level2Name;
        public string    Level3;
        public string    Level3PostNo;
        public string    Level3Name;
        public string    Level4;
        public string    Level4PostNo;
        public string    Level4Name;
        public string    Level5;
        public string    Level5PostNo;
        public string    Level5Name;
        public string    Level6;
        public string    Level6PostNo;
        public string    Level6Name;
        public string    Level7;
        public string    Level7PostNo;
        public string    Level7Name;
        public string    Level8;
        public string    Level8PostNo;
        public string    Level8Name;
        public string    Level9;
        public string    Level9PostNo;
        public string    Level9Name;

        public string    FullName                    { get { return (string.IsNullOrEmpty(KnownAsForename) ? Forename : KnownAsForename) + " " + (string.IsNullOrEmpty(KnownAsSurname) ? Surname : KnownAsSurname); } }
        public string    EmployeeNoAndFullName       { get { return ( (string.IsNullOrEmpty(EmployeeNo)) ? "" : EmployeeNo + " - " + FullName ); } }

        public string    FullNameCurrentPostHolder   { get { return ( (PH_Type == "C") ? EmployeeNo + " - " + FullName : "VACANT" + (string.IsNullOrWhiteSpace(FullName) ? "" : " (" + FullName + ")")); } }
        public string    FullNamePreviousPostHolder  { get { return ( (PH_Type == "P") ? EmployeeNo + " - " + FullName : "" ); } }
        public string    FullNameText                { get { return (PH_Type == "C" ? FullNameCurrentPostHolder : FullNamePreviousPostHolder); } }
        public string    FullnameAndPostTitle        { get { return FullName + " - " + PostTitle; } }
        public string    PostNoTitleAndName          { get
        {
            string s = "";
            if (PH_Type == "P")                     s = "VACANT" + (string.IsNullOrWhiteSpace(FullName) ? "" : " (" + FullName + ")");
            else if (PersonReference == "00000000") s = "VACANT" + (string.IsNullOrWhiteSpace(FullName) ? "" : " (" + FullName + ")");
            else                                    s = FullName;
            return PostNo + " - " + PostTitle + " - " + s;
        } }
        public string    PostNoAndName               { get
        {
            string s = "";
            if (PH_Type == "P")                     s = "VACANT" + (string.IsNullOrWhiteSpace(FullName) ? "" : " (" + FullName + ")");
            else if (PersonReference == "00000000") s = "VACANT" + (string.IsNullOrWhiteSpace(FullName) ? "" : " (" + FullName + ")");
            else                                    s = FullName;
            return PostNo + " - " + s;
        } }

        public string    ResponsibleToPostNoAndName  { get
        {
            string s = "";
                 if (Level9PostNo == PostNo) s = Level8PostNo + " - " + Level8Name;
            else if (Level8PostNo == PostNo) s = Level7PostNo + " - " + Level7Name;
            else if (Level7PostNo == PostNo) s = Level6PostNo + " - " + Level6Name;
            else if (Level6PostNo == PostNo) s = Level5PostNo + " - " + Level5Name;
            else if (Level5PostNo == PostNo) s = Level4PostNo + " - " + Level4Name;
            else if (Level4PostNo == PostNo) s = Level3PostNo + " - " + Level3Name;
            else if (Level3PostNo == PostNo) s = Level2PostNo + " - " + Level2Name;
            else if (Level2PostNo == PostNo) s = Level1PostNo + " - " + Level1Name;
            return s;
        } }

        public string    EmployeeSearchReturn { get { return EmployeeNo + "|" + KnownAsForename + "|" + KnownAsSurname + "|" + PayGroup + "|" + Employee_GDPR_RetentionYears.ToString(); } }
        public string    PostSearchReturn     { get { return PostTitle + "|" + ManagerPostNoAndFullName; } }  // + "|" + PayGroup + "|" + SalaryText

        public string    SQL                  { get; set; }
        public string    Clean_SQL            { get { return Regex.Replace(SQL.Replace(Environment.NewLine, ""), @"\s+", " "); } }



        // Utilities

        public override string ToString() { return PostNoTitleAndName; }



        // Constructors

        public DCC_EmployeeAndPost()
        {
        }

        public DCC_EmployeeAndPost(DataRow dr, bool managerTable = false)
        {
            Load(dr, managerTable);
        }

        public DCC_EmployeeAndPost
        (
            DCC_Session session,
            int         pCurrentOrPrevious, // 1 = current, 2 = previous post holder, 3 = include leavers
            string      pPersonReference = null,
            string      pPostReference = null,
            string      pPostNo = null,
            string      pEmployeeNo = null,
            bool        pLoadPrevious = false,
            bool        pLoadManager = false
        )
        {
            if (session.ResourceLinkDatabaseConnection != null)
            {
                DCC_Database db = session.ResourceLinkDatabaseConnection;
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame();



                //phasing these out as you should use the list versions instead as a post can have multiple post holders and an employee can have multiple posts

                #region get a current post by person reference

                if ( (pPersonReference != null) && (pCurrentOrPrevious == 1) )
                {
                    DebugTrace.WriteLine("QUERY: EmployeeAndPost001");
                    SQL = @"**
                        SELECT
                            estab.*,
                            hier.hier_cb_man_person_ref
                        FROM
                            dcc_v_myworkforce_estab  estab
                            INNER JOIN dcc_mv_rrs_d1544m hier ON
                                hier.hier_cb_emp_person_ref =  estab.person_ref AND
                                hier.hier_cb_emp_post_ref   =  estab.post_ref AND
                                hier.hier_cb_level_no       =  '1' AND
                                hier.hier_cb_type           =  'POST'
                        WHERE
                            estab.PERSON_REF = :pPersonReference
                        ";
                    dbf.AddParameter("pPersonReference", pPersonReference, 40);
                }

                #endregion

                #region get a current post by post reference

                else if ( (pPostReference != null) && (pCurrentOrPrevious == 1) )
                {
                    DebugTrace.WriteLine("QUERY: EmployeeAndPost002");
                    SQL = @"**
                        SELECT
                            estab.*,
                            hier.hier_cb_man_person_ref
                        FROM
                            dcc_v_myworkforce_estab  estab
                            INNER JOIN dcc_mv_rrs_d1544m hier ON
                                hier.hier_cb_emp_person_ref =  estab.person_ref AND
                                hier.hier_cb_emp_post_ref   =  estab.post_ref AND
                                hier.hier_cb_level_no       =  '1' AND
                                hier.hier_cb_type           =  'POST'
                        WHERE
                            estab.POST_REF = :pPostReference
                        ";
                    dbf.AddParameter("pPostReference", pPostReference, 40);
                }

                #endregion

                #region get a current post by post no

                else if ( (pPostNo != null) && (pCurrentOrPrevious == 1) )
                {
                    DebugTrace.WriteLine("QUERY: EmployeeAndPost003");
                    SQL = @"**
                        SELECT
                            estab.*,
                            hier.hier_cb_man_person_ref
                        FROM
                            dcc_v_myworkforce_estab  estab
                            INNER JOIN dcc_mv_rrs_d1544m hier ON
                                hier.hier_cb_emp_person_ref =  estab.person_ref AND
                                hier.hier_cb_emp_post_ref   =  estab.post_ref AND
                                hier.hier_cb_level_no       =  '1' AND
                                hier.hier_cb_type           =  'POST'
                        WHERE
                            estab.POST_NUMBER = :pPostNo
                        ";
                    dbf.AddParameter("pPostNo", pPostNo, 40);
                }

                else if ((pPostNo != null) && (pCurrentOrPrevious == 9))//////9 is used on return to substantive
                {
                    SQL = @"**
                        SELECT
                            estab.*,
                            hier.hier_cb_man_person_ref
                        FROM
                            dcc_v_myworkforce_estab  estab
                            INNER JOIN dcc_mv_rrs_d1544m hier ON
                                hier.hier_cb_emp_post_ref   =  estab.post_ref AND
                                hier.hier_cb_level_no       =  '1' AND
                                hier.hier_cb_type           =  'POST'
                        WHERE
                            estab.POST_NUMBER = :pPostNo
                        ";
                    dbf.AddParameter("pPostNo", pPostNo, 40);
                }

                #endregion

                #region get a current post by employee no

                else if ( (pEmployeeNo != null) && (pCurrentOrPrevious == 1) )
                {
                    DebugTrace.WriteLine("QUERY: EmployeeAndPost004");
                    SQL = @"**
                        SELECT
                            estab.*,
                            hier.hier_cb_man_person_ref
                        FROM
                            dcc_v_myworkforce_estab  estab
                            INNER JOIN dcc_mv_rrs_d1544m hier ON
                                hier.hier_cb_emp_person_ref =  estab.person_ref AND
                                hier.hier_cb_emp_post_ref   =  estab.post_ref AND
                                hier.hier_cb_level_no       =  '1' AND
                                hier.hier_cb_type           =  'POST'
                        WHERE
                            estab.EMPLOYEE_NUMBER = :pEmployeeNo
                        ";
                    dbf.AddParameter("pEmployeeNo", pEmployeeNo, 40);
                }

                #endregion



                #region get a current or ex exmployee by employee no (employee only, just used to verify employee numbers)

                else if ( (pEmployeeNo != null) && (pCurrentOrPrevious == 3) )
                {
                    DebugTrace.WriteLine("QUERY: EmployeeAndPost005");
                    SQL = @"**
                        SELECT *
                        FROM
                            dcc_v_myworkforce_emps
                        WHERE
                            EMPLOYEE_NUMBER = :pEmployeeNo
                        ";
                    dbf.AddParameter("pEmployeeNo", pEmployeeNo, 40);
                }

                #endregion



                #region get the PREVIOUS post holder employee data for a current post by person reference
                // only works on vacant posts and contains no post data :/

                else if ( (pPersonReference != null) && (pCurrentOrPrevious == 2) )
                {
                    DebugTrace.WriteLine("QUERY: EmployeeAndPost006");
                    SQL = @"**
                        SELECT
                            estab.*,
                            hier.hier_cb_man_person_ref
                        FROM
                            dcc_v_myworkforce_estab  estab
                            INNER JOIN dcc_mv_rrs_d1544m hier ON
                                hier.hier_cb_emp_post_ref   =  estab.post_ref AND
                                hier.hier_cb_level_no       =  '1' AND
                                hier.hier_cb_type           =  'POST'
                        WHERE
                            estab.PERSON_REF = :pPersonReference AND
                            estab.PH_TYPE = 'P'
                        ";
                    dbf.AddParameter("pPersonReference", pPersonReference, 40);
                }

                #endregion

                #region get the PREVIOUS post holder employee data for a current post by post no
                // only works on vacant posts and contains no post data :/

                else if ( (pPostNo != null) && (pCurrentOrPrevious == 2) )
                {
                    DebugTrace.WriteLine("QUERY: EmployeeAndPost007");
                    SQL = @"**
                        SELECT
                            estab.*,
                            hier.hier_cb_man_person_ref
                        FROM
                            dcc_v_myworkforce_estab  estab
                            INNER JOIN dcc_mv_rrs_d1544m hier ON
                                hier.hier_cb_emp_post_ref   =  estab.post_ref AND
                                hier.hier_cb_level_no       =  '1' AND
                                hier.hier_cb_type           =  'POST'
                        WHERE
                            estab.POST_NUMBER = :pPostNo AND
                            estab.PH_TYPE = 'P'
                        ";
                    dbf.AddParameter("pPostNo", pPostNo, 40);
                }

                #endregion

                #region get the PREVIOUS post holder employee data for a current post by employee no
                // only works on vacant posts and contains no post data :/

                else if ( (pEmployeeNo != null) && (pCurrentOrPrevious == 2) )
                {
                    DebugTrace.WriteLine("QUERY: EmployeeAndPost008");
                    SQL = @"**
                        SELECT
                            estab.*,
                            hier.hier_cb_man_person_ref
                        FROM
                            dcc_v_myworkforce_estab  estab
                            INNER JOIN dcc_mv_rrs_d1544m hier ON
                                hier.hier_cb_emp_post_ref   =  estab.post_ref AND
                                hier.hier_cb_level_no       =  '1' AND
                                hier.hier_cb_type           =  'POST'
                        WHERE
                            estab.EMPLOYEE_NUMBER = :pEmployeeNo AND
                            estab.PH_TYPE = 'P'
                        ";
                    dbf.AddParameter("pEmployeeNo", pEmployeeNo, 40);
                }

                #endregion



                dbf.AddTable("dcc_v_myworkforce_estab");
                dbf.CommandText = Clean_SQL;
                db.DataLayer.SelectData(dbf);
                if (dbf.hasTables)
                {
                    foreach (DataRow dr in dbf.TableZeroRows)
                    {
                        Load(dr);
                        LoadExtra(session, pCurrentOrPrevious, pLoadPrevious, pLoadManager);
                    }
                }
            }
        }

        public void LoadExtra(DCC_Session session, int pCurrentOrPrevious, bool pLoadPrevious, bool pLoadManager)
        {
            DCC_EmployeeAndPost post = new DCC_EmployeeAndPost();

            if (pCurrentOrPrevious == 9)  pCurrentOrPrevious = 1;

            if ( (PersonReference == "00000000") && (pLoadPrevious))
            {
                post = new DCC_EmployeeAndPost(session, 2, pPostNo: PostNo);
                PreviousPostHolderPersonReference = post.PersonReference;
                PreviousPostHolderEmployeeNo      = post.EmployeeNo;
                PreviousPostHolderSurname         = post.Surname;
                PreviousPostHolderKnownAsSurname  = post.KnownAsSurname;
                PreviousPostHolderForename        = post.Forename;
                PreviousPostHolderKnownAsForename = post.KnownAsForename;
                PreviousPostHolderPostReference   = post.PostReference;
                PreviousPostHolderPostNo          = post.PostNo;
                PreviousPostHolderPostTitle       = post.PostTitle;
            }

            if ( (pCurrentOrPrevious == 1) && (pLoadManager) )
            {
                post = new DCC_EmployeeAndPost(session, 1, ManagerPersonReference);
                ManagerPersonReference = post.PersonReference;
                ManagerEmployeeNo      = post.EmployeeNo;
                ManagerSurname         = post.Surname;
                ManagerKnownAsSurname  = post.KnownAsSurname;
                ManagerForename        = post.Forename;
                ManagerKnownAsForename = post.KnownAsForename;
                ManagerPostReference   = post.PostReference;
                ManagerPostNo          = post.PostNo;
                ManagerPostTitle       = post.PostTitle;
            }
        }

        #region old stuff

        public DCC_EmployeeAndPost(DCC_Session session, string pEmployeeNumber = null, string pPersonReference = null, string pPostNumber = null, bool getPrevious = false)
        {
            if (session.ResourceLinkDatabaseConnection != null)
            {
                DCC_Database db = session.ResourceLinkDatabaseConnection;
                if (pEmployeeNumber != null)
                {
                    DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_estab JOIN dcc_mv_rrs_p2p_occ ON dcc_mv_rrs_p2p_occ.post_ref = dcc_v_myworkforce_estab.post_ref WHERE dcc_v_myworkforce_estab.employee_number=:pEmployeeNumber");
                    dbf.AddParameter("pEmployeeNumber", pEmployeeNumber, 10);
                    dbf.AddTable("dcc_v_myworkforce_estab");
                    db.DataLayer.SelectData(dbf);
                    if (dbf.hasTables)
                    {
                        foreach (DataRow dr in dbf.TableZeroRows)
                        {
                            Load(dr);
                            break; // first row should be Current row
                        }
                    }
                }
                else if (pPersonReference != null)
                {
                    DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_estab JOIN dcc_mv_rrs_p2p_occ ON dcc_mv_rrs_p2p_occ.post_ref = dcc_v_myworkforce_estab.post_ref WHERE dcc_v_myworkforce_estab.person_ref=:pPersonReference");
                    dbf.AddParameter("pPersonReference", pPersonReference, 8);
                    dbf.AddTable("dcc_v_myworkforce_estab");
                    db.DataLayer.SelectData(dbf);
                    if (dbf.hasTables)
                    {
                        foreach (DataRow dr in dbf.TableZeroRows) Load(dr);
                    }
                }
                else if (pPostNumber != null)
                {
                    if (getPrevious)
                    {
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_estab JOIN dcc_mv_rrs_p2p_occ ON dcc_mv_rrs_p2p_occ.post_ref = dcc_v_myworkforce_estab.post_ref WHERE dcc_v_myworkforce_estab.POST_NUMBER = :pPostNoSearch ORDER BY dcc_v_myworkforce_estab.post_number, dcc_v_myworkforce_estab.ph_type");
                        dbf.AddParameter("pPostNoSearch", pPostNumber.ToUpper(), 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if (dbf.hasTables)
                        {
                            foreach (DataRow dr in dbf.TableZeroRows)
                            {
                                if (PH_Type == "P")
                                {
                                    Load(dr);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_estab JOIN dcc_mv_rrs_p2p_occ ON dcc_mv_rrs_p2p_occ.post_ref = dcc_v_myworkforce_estab.post_ref WHERE dcc_v_myworkforce_estab.POST_NUMBER = :pPostNoSearch ORDER BY dcc_v_myworkforce_estab.post_number, dcc_v_myworkforce_estab.ph_type");
                        dbf.AddParameter("pPostNoSearch", pPostNumber.ToUpper(), 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if (dbf.hasTables)
                        {
                            foreach (DataRow dr in dbf.TableZeroRows)
                            {
                                Load(dr);
                                if ( (PH_Type == "C") && (!string.IsNullOrWhiteSpace(EmployeeNo)) )
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion



        // Methods

        private void Load(DataRow dr, bool managerTable = false)
        {
            if (managerTable)
            {
                PersonReference              = DCC_GetColumn.String   (dr, "MGR_PERSON_REF");
                EmployeeNo                   = DCC_GetColumn.String   (dr, "MGR_EMPLOYEE_NUMBER");
                Surname                      = DCC_GetColumn.String   (dr, "MGR_SURNAME");
                KnownAsSurname               = DCC_GetColumn.String   (dr, "MGR_KNOWN_AS_SURNAME");
                Forename                     = DCC_GetColumn.String   (dr, "MGR_FIRST_FORENAME");
                KnownAsForename              = DCC_GetColumn.String   (dr, "MGR_KNOWN_AS");
                OtherForenames               = DCC_GetColumn.String   (dr, "MGR_OTHER_FORENAMES");
                PostReference                = DCC_GetColumn.String   (dr, "MGR_POST_REF");
                PostNo                       = DCC_GetColumn.String   (dr, "MGR_POST_NUMBER");
                PostTitle                    = DCC_GetColumn.String   (dr, "MGR_POST_TITLE");
                PostHoldingStartDate         = DCC_GetColumn.DateTime (dr, "MGR_PH_START_DATE");
                PostHoldingEndDate           = DCC_GetColumn.DateTime (dr, "MGR_PH_END_DATE");
            }

            else
            {
                if (DCC_GetColumn.Exists(dr, "PH_TYPE"))                       PH_Type                      = DCC_GetColumn.String   (dr, "PH_TYPE");
                if (DCC_GetColumn.Exists(dr, "PERSON_REF"))                    PersonReference              = DCC_GetColumn.String   (dr, "PERSON_REF");
                if (DCC_GetColumn.Exists(dr, "EMPLOYEE_NUMBER"))               EmployeeNo                   = DCC_GetColumn.String   (dr, "EMPLOYEE_NUMBER");
                if (DCC_GetColumn.Exists(dr, "SURNAME"))                       Surname                      = DCC_GetColumn.String   (dr, "SURNAME");
                if (DCC_GetColumn.Exists(dr, "KNOWN_AS_SURNAME"))              KnownAsSurname               = DCC_GetColumn.String   (dr, "KNOWN_AS_SURNAME");
                if (DCC_GetColumn.Exists(dr, "FIRST_FORENAME"))                Forename                     = DCC_GetColumn.String   (dr, "FIRST_FORENAME");
                if (DCC_GetColumn.Exists(dr, "KNOWN_AS_FIRST_FORENAME"))       KnownAsForename              = DCC_GetColumn.String   (dr, "KNOWN_AS_FIRST_FORENAME");
                if (DCC_GetColumn.Exists(dr, "OTHER_FORENAMES"))               OtherForenames               = DCC_GetColumn.String   (dr, "OTHER_FORENAMES");
                if (DCC_GetColumn.Exists(dr, "POST_REF"))                      PostReference                = DCC_GetColumn.String   (dr, "POST_REF");
                if (DCC_GetColumn.Exists(dr, "POST_NUMBER"))                   PostNo                       = DCC_GetColumn.String   (dr, "POST_NUMBER");
                if (DCC_GetColumn.Exists(dr, "POST_TITLE"))                    PostTitle                    = DCC_GetColumn.String   (dr, "POST_TITLE");
                if (DCC_GetColumn.Exists(dr, "PH_START_DATE"))                 PostHoldingStartDate         = DCC_GetColumn.DateTimeQ(dr, "PH_START_DATE");
                if (DCC_GetColumn.Exists(dr, "PH_END_DATE"))                   PostHoldingEndDate           = DCC_GetColumn.DateTimeQ(dr, "PH_END_DATE");
                if (DCC_GetColumn.Exists(dr, "PROJECTED_END_DATE"))            ProjectedEndDate             = DCC_GetColumn.DateTimeQ(dr, "PROJECTED_END_DATE");
                if (DCC_GetColumn.Exists(dr, "MAIN_POST"))                     MainPost                     = DCC_GetColumn.String   (dr, "MAIN_POST");
                if (DCC_GetColumn.Exists(dr, "POSITION_STATUS_START_DATE"))    PositionStatusStartDate      = DCC_GetColumn.DateTimeQ(dr, "POSITION_STATUS_START_DATE");
                if (DCC_GetColumn.Exists(dr, "POSITION_STATUS_END_DATE"))      PositionStatusEndDate        = DCC_GetColumn.DateTimeQ(dr, "POSITION_STATUS_END_DATE");
                if (DCC_GetColumn.Exists(dr, "POSITION_STATUS"))               PositionStatus               = DCC_GetColumn.String   (dr, "POSITION_STATUS");
                if (DCC_GetColumn.Exists(dr, "HIERARCHY_LEVEL_1"))             HierarchyLevel1              = DCC_GetColumn.String   (dr, "HIERARCHY_LEVEL_1");
                if (DCC_GetColumn.Exists(dr, "HIERARCHY_LEVEL_2"))             HierarchyLevel2              = DCC_GetColumn.String   (dr, "HIERARCHY_LEVEL_2");
                if (DCC_GetColumn.Exists(dr, "HIERARCHY_LEVEL_3"))             HierarchyLevel3              = DCC_GetColumn.String   (dr, "HIERARCHY_LEVEL_3");
                if (DCC_GetColumn.Exists(dr, "HIERARCHY_LEVEL_4"))             HierarchyLevel4              = DCC_GetColumn.String   (dr, "HIERARCHY_LEVEL_4");
                if (DCC_GetColumn.Exists(dr, "HIERARCHY_LEVEL_5"))             HierarchyLevel5              = DCC_GetColumn.String   (dr, "HIERARCHY_LEVEL_5");
                if (DCC_GetColumn.Exists(dr, "HIERARCHY_LEVEL_6"))             HierarchyLevel6              = DCC_GetColumn.String   (dr, "HIERARCHY_LEVEL_6");
                if (DCC_GetColumn.Exists(dr, "HIERARCHY_LEVEL_7"))             HierarchyLevel7              = DCC_GetColumn.String   (dr, "HIERARCHY_LEVEL_7");
                if (DCC_GetColumn.Exists(dr, "HIERARCHY_LEVEL_8"))             HierarchyLevel8              = DCC_GetColumn.String   (dr, "HIERARCHY_LEVEL_8");
                if (DCC_GetColumn.Exists(dr, "HIERARCHY_LEVEL_9"))             HierarchyLevel9              = DCC_GetColumn.String   (dr, "HIERARCHY_LEVEL_9");
                if (DCC_GetColumn.Exists(dr, "HIERARCHY_LEVEL_1"))             HierarchyLevel10             = DCC_GetColumn.String   (dr, "HIERARCHY_LEVEL_10");
                if (DCC_GetColumn.Exists(dr, "POST_GRADE"))                    PostGrade                    = DCC_GetColumn.String   (dr, "POST_GRADE");
                if (DCC_GetColumn.Exists(dr, "POST_HOURS"))                    PostHours                    = DCC_GetColumn.Decimal  (dr, "POST_HOURS");
                if (DCC_GetColumn.Exists(dr, "POST_HEADCOUNT"))                PostHeadCount                = DCC_GetColumn.Decimal  (dr, "POST_HEADCOUNT");
                if (DCC_GetColumn.Exists(dr, "PAY_GROUP"))                     PayGroup                     = DCC_GetColumn.String   (dr, "PAY_GROUP");
                if (DCC_GetColumn.Exists(dr, "POST_LOCATION_DESCRIPTION"))     LocationOfPost               = DCC_GetColumn.String   (dr, "POST_LOCATION_DESCRIPTION");
                if (DCC_GetColumn.Exists(dr, "PH_LOCATION_DESCRIPTION"))       LocationOfEmployee           = DCC_GetColumn.String   (dr, "PH_LOCATION_DESCRIPTION");
                if (DCC_GetColumn.Exists(dr, "OCCUPANCY_TYPE"))                PostOccupancy                = DCC_GetColumn.String   (dr, "OCCUPANCY_TYPE");
                if (DCC_GetColumn.Exists(dr, "HOURS"))                         ContractedHours              = DCC_GetColumn.Decimal  (dr, "HOURS");//contractedhours is HOURS; POST_HOURS is something else
                if (DCC_GetColumn.Exists(dr, "SALARY"))                        Salary                       = DCC_GetColumn.Decimal  (dr, "SALARY");
                if (DCC_GetColumn.Exists(dr, "NI_NO"))                         NationalInsuranceNo          = DCC_GetColumn.String   (dr, "NI_NO");
                if (DCC_GetColumn.Exists(dr, "POST_GDPR_RETENTION_YEARS"))     Post_GDPR_RetentionYears     = DCC_GetColumn.Decimal  (dr, "POST_GDPR_RETENTION_YEARS");
                if (DCC_GetColumn.Exists(dr, "EMPLOYEE_GDPR_RETENTION_YEARS")) Employee_GDPR_RetentionYears = DCC_GetColumn.Decimal  (dr, "EMPLOYEE_GDPR_RETENTION_YEARS");
                if (DCC_GetColumn.Exists(dr, "hier_cb_man_person_ref"))        ManagerPersonReference       = DCC_GetColumn.String   (dr, "hier_cb_man_person_ref");
                if (DCC_GetColumn.Exists(dr, "LevelNo"))                       LevelNo                      = DCC_GetColumn.String   (dr, "LEVEL_NO");
                if (DCC_GetColumn.Exists(dr, "Level1"))                        Level1                       = DCC_GetColumn.String   (dr, "LEVEL_1");
                if (DCC_GetColumn.Exists(dr, "Level1PostNo"))                  Level1PostNo                 = DCC_GetColumn.String   (dr, "LEVEL_1_POST_NUMBER");
                if (DCC_GetColumn.Exists(dr, "Level1Name"))                    Level1Name                   = DCC_GetColumn.String   (dr, "P2P_LEVEL_1");
                if (DCC_GetColumn.Exists(dr, "Level2"))                        Level2                       = DCC_GetColumn.String   (dr, "LEVEL_2");
                if (DCC_GetColumn.Exists(dr, "Level2PostNo"))                  Level2PostNo                 = DCC_GetColumn.String   (dr, "LEVEL_2_POST_NUMBER");
                if (DCC_GetColumn.Exists(dr, "Level2Name"))                    Level2Name                   = DCC_GetColumn.String   (dr, "P2P_LEVEL_2");
                if (DCC_GetColumn.Exists(dr, "Level3"))                        Level3                       = DCC_GetColumn.String   (dr, "LEVEL_3");
                if (DCC_GetColumn.Exists(dr, "Level3PostNo"))                  Level3PostNo                 = DCC_GetColumn.String   (dr, "LEVEL_3_POST_NUMBER");
                if (DCC_GetColumn.Exists(dr, "Level3Name"))                    Level3Name                   = DCC_GetColumn.String   (dr, "P2P_LEVEL_3");
                if (DCC_GetColumn.Exists(dr, "Level4"))                        Level4                       = DCC_GetColumn.String   (dr, "LEVEL_4");
                if (DCC_GetColumn.Exists(dr, "Level4PostNo"))                  Level4PostNo                 = DCC_GetColumn.String   (dr, "LEVEL_4_POST_NUMBER");
                if (DCC_GetColumn.Exists(dr, "Level4Name"))                    Level4Name                   = DCC_GetColumn.String   (dr, "P2P_LEVEL_4");
                if (DCC_GetColumn.Exists(dr, "Level5"))                        Level5                       = DCC_GetColumn.String   (dr, "LEVEL_5");
                if (DCC_GetColumn.Exists(dr, "Level5PostNo"))                  Level5PostNo                 = DCC_GetColumn.String   (dr, "LEVEL_5_POST_NUMBER");
                if (DCC_GetColumn.Exists(dr, "Level5Name"))                    Level5Name                   = DCC_GetColumn.String   (dr, "P2P_LEVEL_5");
                if (DCC_GetColumn.Exists(dr, "Level6"))                        Level6                       = DCC_GetColumn.String   (dr, "LEVEL_6");
                if (DCC_GetColumn.Exists(dr, "Level6PostNo"))                  Level6PostNo                 = DCC_GetColumn.String   (dr, "LEVEL_6_POST_NUMBER");
                if (DCC_GetColumn.Exists(dr, "Level6Name"))                    Level6Name                   = DCC_GetColumn.String   (dr, "P2P_LEVEL_6");
                if (DCC_GetColumn.Exists(dr, "Level7"))                        Level7                       = DCC_GetColumn.String   (dr, "LEVEL_7");
                if (DCC_GetColumn.Exists(dr, "Level7PostNo"))                  Level7PostNo                 = DCC_GetColumn.String   (dr, "LEVEL_7_POST_NUMBER");
                if (DCC_GetColumn.Exists(dr, "Level7Name"))                    Level7Name                   = DCC_GetColumn.String   (dr, "P2P_LEVEL_7");
                if (DCC_GetColumn.Exists(dr, "Level8"))                        Level8                       = DCC_GetColumn.String   (dr, "LEVEL_8");
                if (DCC_GetColumn.Exists(dr, "Level8PostNo"))                  Level8PostNo                 = DCC_GetColumn.String   (dr, "LEVEL_8_POST_NUMBER");
                if (DCC_GetColumn.Exists(dr, "Level8Name"))                    Level8Name                   = DCC_GetColumn.String   (dr, "P2P_LEVEL_8");
                if (DCC_GetColumn.Exists(dr, "Level9"))                        Level9                       = DCC_GetColumn.String   (dr, "LEVEL_9");
                if (DCC_GetColumn.Exists(dr, "Level9PostNo"))                  Level9PostNo                 = DCC_GetColumn.String   (dr, "LEVEL_9_POST_NUMBER");
                if (DCC_GetColumn.Exists(dr, "Level9Name"))                    Level9Name                   = DCC_GetColumn.String   (dr, "P2P_LEVEL_9");
            }
        }

    }

    #endregion


    #region DCC_EmployeeAndPostList

    [Serializable]
    public class DCC_EmployeeAndPostList : IEnumerable<DCC_EmployeeAndPost>
    {

        // Properties

        public string SQL       { get; set; }
        public string Clean_SQL { get { return ( (string.IsNullOrEmpty(SQL)) ? "" : Regex.Replace(SQL.Replace(Environment.NewLine, ""), @"\s+", " ") ); } }

        public List<DCC_EmployeeAndPost> All = new List<DCC_EmployeeAndPost>();
        public List<DCC_EmployeeAndPost> AllVacant = new List<DCC_EmployeeAndPost>();



        // Utilities

        public int Count { get { return All.Count; } }
        public override string ToString() { return "EmployeeAndPostList (" + Count + " items)"; }



        // Constructors

        public DCC_EmployeeAndPostList()
        {
        }

        public DCC_EmployeeAndPostList
            (
                DCC_Session session,
                int         pCurrentAndOrVacant,  // 1 = Current and vacant, 2 = Current only, 3 = Vacant only
                int         pReportingLine,       // 1 = Posts below (direct reports), 2 = Posts above (reporting line), 3 specific level
                string      pPostNo = null,
                string      pPostReference = null,
                string      pEmployeeNo = null,
                string      pHierarchyLevel2 = null,
                string      pHierarchyLevel3 = null,
                string      pHierarchyLevel4 = null,
                string      pHierarchyLevel5 = null,
                string      pHierarchyLevel6 = null,
                string      pHierarchyLevel7 = null,
                string      pHierarchyLevel8 = null,
                string      pHierarchyLevel9 = null,
                string      pSurname = null,
                int         pCurrentOrPrevious = 0, // 1 = current, 2 = previous
                bool        pLoadPrevious = false,
                bool        pLoadManager = false,
                bool        pOmitSelf = false,
                bool        pAllowDuplicates = false
            )
        {
            if (session.ResourceLinkDatabaseConnection != null)
            {
                DCC_Database db = session.ResourceLinkDatabaseConnection;
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame();



                #region get current posts that report to a given manager - current posts that are vacant have no employee info
                // vacant posts can be identified using person_ref = '00000000'
                // remove the cb_level_no = '1' to get ALL current posts under a given manager

                if ( (!string.IsNullOrEmpty(pPostReference)) && (pCurrentAndOrVacant == 1) && (pReportingLine == 1) )
                {
                    DebugTrace.WriteLine("QUERY: EmployeeAndPostList001");
                    SQL = @"**
                        SELECT
                            estab.*,
                            hier.hier_cb_man_person_ref
                        FROM
                            dcc_v_myworkforce_estab  estab
                            INNER JOIN dcc_mv_rrs_d1544m hier ON
                                hier.hier_cb_emp_person_ref =  estab.person_ref AND
                                hier.hier_cb_emp_post_ref   =  estab.post_ref AND
                                hier.hier_cb_level_no       =  '1' AND
                                hier.hier_cb_man_post_ref   =  :pPostReference AND
                                hier.hier_cb_type           =  'POST'
                        ";
                    dbf.AddParameter("pPostReference", pPostReference, 40);
                }

                #endregion

                #region get current posts that report to a given manager - vacant posts have the employee data of the PREVIOUS postholder
                // BUT NO POST INFO :/
                // remove the cb_level_no = '1' to get ALL current posts under a given manager

                // used to get direct reports on post structure change

                else if ( (!string.IsNullOrEmpty(pPostReference)) && (pCurrentAndOrVacant == 2) && (pReportingLine == 1) )
                {
                    DebugTrace.WriteLine("QUERY: EmployeeAndPostList002");
                    SQL = @"**
                        SELECT
                            estab.*
                        FROM
                            dcc_v_myworkforce_estab estab
                            INNER JOIN dcc_mv_rrs_d1544m hier ON
                                hier.hier_cb_level_no       =  '1'  AND 
                                hier.hier_cb_man_post_ref   =  :pPostReference AND
                                hier.hier_cb_type           =  'POST'  AND
                                hier.hier_cb_emp_post_ref   =  estab.post_ref  AND
                                (
                                    estab.person_ref        <> '00000000'
                                    OR
                                    (
                                        estab.person_ref    =  '00000000' AND
                                        estab.post_ref NOT IN
                                        (
                                            SELECT s1.post_ref 
                                            FROM   dcc_v_myworkforce_estab s1
                                            WHERE  s1.ph_type = 'P'
                                        )
                                    )
                                )
                        ";
                    dbf.AddParameter("pPostReference", pPostReference, 40);
                }

                #endregion

                #region get vacant posts that report to a given manager - vacant posts obvs have no employee info
                // remove the cb_level_no = '1' to get ALL vacant posts under a given manager

                else if ( (!string.IsNullOrEmpty(pPostReference)) && (pCurrentAndOrVacant == 3) && (pReportingLine == 1) )
                {
                    DebugTrace.WriteLine("QUERY: EmployeeAndPostList003");
                    SQL = @"**
                        SELECT
                            estab.*,
                            hier.hier_cb_man_person_ref
                        FROM
                            dcc_v_myworkforce_estab estab
                            INNER JOIN dcc_mv_rrs_d1544m hier ON
                                hier.hier_cb_emp_person_ref =  '00000000' AND
                                hier.hier_cb_emp_post_ref   =  estab.post_ref AND
                                hier.hier_cb_level_no       =  '1' AND
                                hier.hier_cb_man_post_ref   =  :pPostReference AND
                                hier.hier_cb_type           =  'POST'
                        "
                    ;
                                //estab.person_ref            <> '00000000'
                    dbf.AddParameter("pPostReference", pPostReference, 40);
                }

                #endregion



                // used by post search on fill & change
                else if ( (string.IsNullOrEmpty(pPostNo)) && (string.IsNullOrEmpty(pPostReference)) && (string.IsNullOrEmpty(pSurname)) )
                {

                    #region get current posts in a given hierarchy 3 - vacant posts have the employee data of the PREVIOUS postholder
                    // remove the cb_level_no = '1' to get ALL current posts under a given manager

                    if ( (!string.IsNullOrEmpty(pHierarchyLevel3)) && (pCurrentAndOrVacant == 2) && (pReportingLine == 3) )
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostList004");
                        SQL = @"**
                            SELECT
                                estab.*,
                                hier.hier_cb_man_person_ref
                            FROM
                                dcc_v_myworkforce_estab estab
                                INNER JOIN dcc_mv_rrs_d1544m hier ON
                                    hier.hier_cb_emp_post_ref   =  estab.post_ref AND
                                    hier.hier_cb_type           =  'POST' AND
                                    hier.hier_cb_level_no       =  '1' AND
                                    estab.hierarchy_level_3     =  :pHierarchyLevel3
                            ORDER BY
                                POST_NUMBER
                            ";
                                    //estab.person_ref            <> '00000000' AND
                        dbf.AddParameter("pHierarchyLevel3", pHierarchyLevel3, 40);
                    }

                    #endregion

                    #region get current posts in a given hierarchy 4 - vacant posts have the employee data of the PREVIOUS postholder

                    else if ( (!string.IsNullOrEmpty(pHierarchyLevel4)) && (pCurrentAndOrVacant == 2) && (pReportingLine == 3) )
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostList005");
                        SQL = @"**
                            SELECT
                                estab.*,
                                hier.hier_cb_man_person_ref
                            FROM
                                dcc_v_myworkforce_estab estab
                                INNER JOIN dcc_mv_rrs_d1544m hier ON
                                    hier.hier_cb_emp_post_ref   =  estab.post_ref AND
                                    hier.hier_cb_type           =  'POST' AND
                                    hier.hier_cb_level_no       =  '1' AND
                                    estab.hierarchy_level_4     =  :pHierarchyLevel4
                            ORDER BY
                                POST_NUMBER
                            ";
                                    //estab.person_ref            <> '00000000' AND
                        dbf.AddParameter("pHierarchyLevel4", pHierarchyLevel4, 40);
                    }

                    #endregion

                    #region get current posts in a given hierarchy 5 - vacant posts have the employee data of the PREVIOUS postholder

                    else if ( (!string.IsNullOrEmpty(pHierarchyLevel5)) && (pCurrentAndOrVacant == 2) && (pReportingLine == 3) )
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostList006");
                        SQL = @"**
                            SELECT
                                estab.*,
                                hier.hier_cb_man_person_ref
                            FROM
                                dcc_v_myworkforce_estab estab
                                INNER JOIN dcc_mv_rrs_d1544m hier ON
                                    hier.hier_cb_emp_post_ref   =  estab.post_ref AND
                                    hier.hier_cb_type           =  'POST' AND
                                    hier.hier_cb_level_no       =  '1' AND
                                    estab.hierarchy_level_5     =  :pHierarchyLevel5
                            ORDER BY
                                POST_NUMBER
                            ";
                                    //estab.person_ref            <> '00000000' AND
                        dbf.AddParameter("pHierarchyLevel5", pHierarchyLevel5, 40);
                    }

                    #endregion

                    #region get current posts in a given hierarchy 6 - vacant posts have the employee data of the PREVIOUS postholder

                    else if ( (!string.IsNullOrEmpty(pHierarchyLevel6)) && (pCurrentAndOrVacant == 2) && (pReportingLine == 3) )
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostList007");
                        SQL = @"**
                            SELECT
                                estab.*,
                                hier.hier_cb_man_person_ref
                            FROM
                                dcc_v_myworkforce_estab estab
                                INNER JOIN dcc_mv_rrs_d1544m hier ON
                                    hier.hier_cb_emp_post_ref   =  estab.post_ref AND
                                    hier.hier_cb_type           =  'POST' AND
                                    hier.hier_cb_level_no       =  '1' AND
                                    estab.hierarchy_level_6     =  :pHierarchyLevel6
                            ORDER BY
                                POST_NUMBER
                            ";
                                    //estab.person_ref            <> '00000000' AND
                        dbf.AddParameter("pHierarchyLevel6", pHierarchyLevel6, 40);
                    }

                    #endregion

                    #region get current posts in a given hierarchy 7 - vacant posts have the employee data of the PREVIOUS postholder

                    else if ( (!string.IsNullOrEmpty(pHierarchyLevel7)) && (pCurrentAndOrVacant == 2) && (pReportingLine == 3) )
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostList008");
                        SQL = @"**
                            SELECT
                                estab.*,
                                hier.hier_cb_man_person_ref
                            FROM
                                dcc_v_myworkforce_estab estab
                                INNER JOIN dcc_mv_rrs_d1544m hier ON
                                    hier.hier_cb_emp_post_ref   =  estab.post_ref AND
                                    hier.hier_cb_type           =  'POST' AND
                                    hier.hier_cb_level_no       =  '1' AND
                                    estab.hierarchy_level_7     =  :pHierarchyLevel7
                            ORDER BY
                                POST_NUMBER
                            ";
                                    //estab.person_ref            <> '00000000' AND
                        dbf.AddParameter("pHierarchyLevel7", pHierarchyLevel7, 40);
                    }

                    #endregion

                    #region get current posts in a given hierarchy 8 - vacant posts have the employee data of the PREVIOUS postholder

                    else if ( (!string.IsNullOrEmpty(pHierarchyLevel8)) && (pCurrentAndOrVacant == 2) && (pReportingLine == 3) )
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostList009");
                        SQL = @"**
                            SELECT
                                estab.*,
                                hier.hier_cb_man_person_ref
                            FROM
                                dcc_v_myworkforce_estab estab
                                INNER JOIN dcc_mv_rrs_d1544m hier ON
                                    hier.hier_cb_emp_post_ref   =  estab.post_ref AND
                                    hier.hier_cb_type           =  'POST' AND
                                    hier.hier_cb_level_no       =  '1' AND
                                    estab.hierarchy_level_8     =  :pHierarchyLevel8
                            ORDER BY
                                POST_NUMBER
                            ";
                                    //estab.person_ref            <> '00000000' AND
                        dbf.AddParameter("pHierarchyLevel8", pHierarchyLevel8, 40);
                    }

                    #endregion

                    #region get current posts in a given hierarchy 9 - vacant posts have the employee data of the PREVIOUS postholder

                    else if ( (!string.IsNullOrEmpty(pHierarchyLevel9)) && (pCurrentAndOrVacant == 2) && (pReportingLine == 3) )
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostList010");
                        SQL = @"**
                            SELECT
                                estab.*,
                                hier.hier_cb_man_person_ref
                            FROM
                                dcc_v_myworkforce_estab estab
                                INNER JOIN dcc_mv_rrs_d1544m hier ON
                                    hier.hier_cb_emp_post_ref   =  estab.post_ref AND
                                    hier.hier_cb_type           =  'POST' AND
                                    hier.hier_cb_level_no       =  '1' AND
                                    estab.hierarchy_level_9     =  :pHierarchyLevel9
                            ORDER BY
                                POST_NUMBER
                            ";
                                    //estab.person_ref            <> '00000000' AND
                        dbf.AddParameter("pHierarchyLevel9", pHierarchyLevel9, 40);
                    }

                    #endregion



                    #region get current posts in a given hierarchy 3 - current posts that are vacant obvs have no employee info
                    // vacant posts can be identified using person_ref = '00000000'

                    else if ( (!string.IsNullOrEmpty(pHierarchyLevel3)) && (string.IsNullOrEmpty(pSurname)) && (pCurrentAndOrVacant == 1) && (pReportingLine == 1) )
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostList011");
                        SQL = @"**
                            SELECT
                                estab.*,
                                hier.hier_cb_man_person_ref
                            FROM
                                dcc_v_myworkforce_estab  estab
                                INNER JOIN dcc_mv_rrs_d1544m hier ON
                                    hier.hier_cb_emp_person_ref =  estab.person_ref AND
                                    hier.hier_cb_emp_post_ref   =  estab.post_ref AND
                                    hier.hier_cb_level_no       =  '1' AND
                                    hier.hier_cb_type           =  'POST' AND
                                    estab.hierarchy_level_3     =  :pHierarchyLevel3
                            ORDER BY
                                POST_NUMBER
                            ";
                        dbf.AddParameter("pHierarchyLevel3", pHierarchyLevel3, 40);
                    }

                    #endregion

                    #region get vacant posts in a given hierarchy 3 - vacant posts have no employee info
                    // remove the cb_level_no = '1' to get ALL vacant posts under a given manager

                    else if ( (!string.IsNullOrEmpty(pHierarchyLevel3)) && (pCurrentAndOrVacant == 3) && (pReportingLine == 1) )
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostList012");
                        SQL = @"**
                            SELECT
                                estab.*,
                                hier.hier_cb_man_person_ref
                            FROM
                                dcc_v_myworkforce_estab estab
                                INNER JOIN dcc_mv_rrs_d1544m hier ON
                                    hier.hier_cb_emp_person_ref =  '00000000' AND
                                    hier.hier_cb_emp_post_ref   =  estab.post_ref AND
                                    hier.hier_cb_level_no       =  '1' AND
                                    hier.hier_cb_type           =  'POST' AND
                                    estab.hierarchy_level_3     =  :pHierarchyLevel3
                            ORDER BY
                                POST_NUMBER
                            ";
                        dbf.AddParameter("pHierarchyLevel3", pHierarchyLevel3, 40);
                    }

                    #endregion

                }


                else if ( (string.IsNullOrEmpty(pPostNo)) && (string.IsNullOrEmpty(pPostReference)) && (!string.IsNullOrEmpty(pSurname)) )
                {
                    #region get all current employees that are LIKE a given surname AND are in Hierarchy Level 9

                    if ( (!string.IsNullOrEmpty(pSurname)) && (!string.IsNullOrEmpty(pHierarchyLevel9)) && (pCurrentAndOrVacant == 2) && (pReportingLine == 3) )
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostList013");
                        SQL = @"**
                            SELECT
                                estab.*,
                                hier.hier_cb_man_person_ref
                            FROM
                                dcc_v_myworkforce_estab estab
                                INNER JOIN dcc_mv_rrs_d1544m hier ON
                                    hier.hier_cb_emp_person_ref =  estab.person_ref AND
                                    hier.hier_cb_emp_post_ref   =  estab.post_ref AND
                                    hier.hier_cb_level_no       =  '1' AND
                                    hier.hier_cb_type           =  'POST'
                            WHERE
                                estab.hierarchy_level_9 = :pHierarchyLevel9 AND
                                estab.surname LIKE :pSurname 
                            ORDER BY
                                estab.post_number
                            ";
                        dbf.AddParameter("pHierarchyLevel9", pHierarchyLevel9, 40);
                        dbf.AddParameter("pSurname", "%" + pSurname.ToUpper() + "%", 40);
                    }

                    #endregion

                    #region get all current employees that are LIKE a given surname AND are in Hierarchy Level 8

                    else if ( (!string.IsNullOrEmpty(pSurname)) && (!string.IsNullOrEmpty(pHierarchyLevel8)) && (pCurrentAndOrVacant == 2) && (pReportingLine == 3) )
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostList014");
                        SQL = @"**
                            SELECT
                                estab.*,
                                hier.hier_cb_man_person_ref
                            FROM
                                dcc_v_myworkforce_estab estab
                                INNER JOIN dcc_mv_rrs_d1544m hier ON
                                    hier.hier_cb_emp_person_ref =  estab.person_ref AND
                                    hier.hier_cb_emp_post_ref   =  estab.post_ref AND
                                    hier.hier_cb_level_no       =  '1' AND
                                    hier.hier_cb_type           =  'POST'
                            WHERE estab.hierarchy_level_8 = :pHierarchyLevel8 AND estab.surname LIKE :pSurname 
                            ORDER BY
                                estab.post_number
                            ";
                        dbf.AddParameter("pHierarchyLevel8", pHierarchyLevel8, 40);
                        dbf.AddParameter("pSurname", "%" + pSurname.ToUpper() + "%", 40);
                    }

                    #endregion

                    #region get all current employees that are LIKE a given surname AND are in Hierarchy Level 7

                    else if ( (!string.IsNullOrEmpty(pSurname)) && (!string.IsNullOrEmpty(pHierarchyLevel7)) && (pCurrentAndOrVacant == 2) && (pReportingLine == 3) )
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostList015");
                        SQL = @"**
                            SELECT
                                estab.*,
                                hier.hier_cb_man_person_ref
                            FROM
                                dcc_v_myworkforce_estab estab
                                INNER JOIN dcc_mv_rrs_d1544m hier ON
                                    hier.hier_cb_emp_person_ref =  estab.person_ref AND
                                    hier.hier_cb_emp_post_ref   =  estab.post_ref AND
                                    hier.hier_cb_level_no       =  '1' AND
                                    hier.hier_cb_type           =  'POST'
                            WHERE estab.hierarchy_level_7 = :pHierarchyLevel7 AND estab.surname LIKE :pSurname 
                            ORDER BY
                                estab.post_number
                            ";
                        dbf.AddParameter("pHierarchyLevel7", pHierarchyLevel7, 40);
                        dbf.AddParameter("pSurname", "%" + pSurname.ToUpper() + "%", 40);
                    }

                    #endregion

                    #region get all current employees that are LIKE a given surname AND are in Hierarchy Level 6

                    else if ( (!string.IsNullOrEmpty(pSurname)) && (!string.IsNullOrEmpty(pHierarchyLevel6)) && (pCurrentAndOrVacant == 2) && (pReportingLine == 3) )
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostList016");
                        SQL = @"**
                            SELECT
                                estab.*,
                                hier.hier_cb_man_person_ref
                            FROM
                                dcc_v_myworkforce_estab estab
                                INNER JOIN dcc_mv_rrs_d1544m hier ON
                                    hier.hier_cb_emp_person_ref =  estab.person_ref AND
                                    hier.hier_cb_emp_post_ref   =  estab.post_ref AND
                                    hier.hier_cb_level_no       =  '1' AND
                                    hier.hier_cb_type           =  'POST'
                            WHERE estab.hierarchy_level_6 = :pHierarchyLevel6 AND estab.surname LIKE :pSurname 
                            ORDER BY
                                estab.post_number
                            ";
                        dbf.AddParameter("pHierarchyLevel6", pHierarchyLevel6, 40);
                        dbf.AddParameter("pSurname", "%" + pSurname.ToUpper() + "%", 40);
                    }

                    #endregion

                    #region get all current employees that are LIKE a given surname AND are in Hierarchy Level 5

                    else if ( (!string.IsNullOrEmpty(pSurname)) && (!string.IsNullOrEmpty(pHierarchyLevel5)) && (pCurrentAndOrVacant == 2) && (pReportingLine == 3) )
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostList017");
                        SQL = @"**
                            SELECT
                                estab.*,
                                hier.hier_cb_man_person_ref
                            FROM
                                dcc_v_myworkforce_estab estab
                                INNER JOIN dcc_mv_rrs_d1544m hier ON
                                    hier.hier_cb_emp_person_ref =  estab.person_ref AND
                                    hier.hier_cb_emp_post_ref   =  estab.post_ref AND
                                    hier.hier_cb_level_no       =  '1' AND
                                    hier.hier_cb_type           =  'POST'
                            WHERE estab.hierarchy_level_5 = :pHierarchyLevel5 AND estab.surname LIKE :pSurname 
                            ORDER BY
                                estab.post_number
                            ";
                        dbf.AddParameter("pHierarchyLevel5", pHierarchyLevel5, 40);
                        dbf.AddParameter("pSurname", "%" + pSurname.ToUpper() + "%", 40);
                    }

                    #endregion

                    #region get all current employees that are LIKE a given surname AND are in Hierarchy Level 4

                    else if ( (!string.IsNullOrEmpty(pSurname)) && (!string.IsNullOrEmpty(pHierarchyLevel4)) && (pCurrentAndOrVacant == 2) && (pReportingLine == 3) )
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostList018");
                        SQL = @"**
                            SELECT
                                estab.*,
                                hier.hier_cb_man_person_ref
                            FROM
                                dcc_v_myworkforce_estab estab
                                INNER JOIN dcc_mv_rrs_d1544m hier ON
                                    hier.hier_cb_emp_person_ref =  estab.person_ref AND
                                    hier.hier_cb_emp_post_ref   =  estab.post_ref AND
                                    hier.hier_cb_level_no       =  '1' AND
                                    hier.hier_cb_type           =  'POST'
                            WHERE estab.hierarchy_level_4 = :pHierarchyLevel4 AND estab.surname LIKE :pSurname 
                            ORDER BY
                                estab.post_number
                            ";
                        dbf.AddParameter("pHierarchyLevel4", pHierarchyLevel4, 40);
                        dbf.AddParameter("pSurname", "%" + pSurname.ToUpper() + "%", 40);
                    }

                    #endregion

                    #region get all current employees that are LIKE a given surname AND are in Hierarchy Level 3

                    else if ( (!string.IsNullOrEmpty(pSurname)) && (!string.IsNullOrEmpty(pHierarchyLevel3)) && (pCurrentAndOrVacant == 2) && (pReportingLine == 3) )
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostList019");
                        SQL = @"**
                            SELECT
                                estab.*,
                                hier.hier_cb_man_person_ref
                            FROM
                                dcc_v_myworkforce_estab estab
                                INNER JOIN dcc_mv_rrs_d1544m hier ON
                                    hier.hier_cb_emp_person_ref =  estab.person_ref AND
                                    hier.hier_cb_emp_post_ref   =  estab.post_ref AND
                                    hier.hier_cb_level_no       =  '1' AND
                                    hier.hier_cb_type           =  'POST'
                            WHERE
                                estab.hierarchy_level_3 = :pHierarchyLevel3 AND
                                estab.surname LIKE :pSurname 
                            ORDER BY
                                estab.post_number
                            ";
                        dbf.AddParameter("pHierarchyLevel3", pHierarchyLevel3, 40);
                        dbf.AddParameter("pSurname", "%" + pSurname.ToUpper() + "%", 40);
                    }

                    #endregion

                    #region get all current employees that are LIKE a given surname AND are in Hierarchy Level 2

                    else if ( (!string.IsNullOrEmpty(pSurname)) && (!string.IsNullOrEmpty(pHierarchyLevel2)) && (pCurrentAndOrVacant == 2) && (pReportingLine == 3) )
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostList020");
                        SQL = @"**
                            SELECT
                                estab.*,
                                hier.hier_cb_man_person_ref
                            FROM
                                dcc_v_myworkforce_estab  estab
                                INNER JOIN dcc_mv_rrs_d1544m hier ON
                                    hier.hier_cb_emp_person_ref =  estab.person_ref AND
                                    hier.hier_cb_emp_post_ref   =  estab.post_ref AND
                                    hier.hier_cb_level_no       =  '1' AND
                                    hier.hier_cb_type           =  'POST'
                            WHERE
                                estab.hierarchy_level_2 = :pHierarchyLevel2 AND
                                estab.surname LIKE :pSurname 
                            ORDER BY
                                estab.post_number
                            ";
                        dbf.AddParameter("pHierarchyLevel2", pHierarchyLevel2, 40);
                        dbf.AddParameter("pSurname", "%" + pSurname.ToUpper() + "%", 40);
                    }

                    #endregion

                    #region get all current employees that are LIKE a given surname

                    else if ( (!string.IsNullOrEmpty(pSurname)) && (pReportingLine == 2) && (pCurrentAndOrVacant == 3) )
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostList021");
                        SQL = @"**
                            SELECT
                                estab.*,
                                hier.hier_cb_man_person_ref
                            FROM
                                dcc_v_myworkforce_estab  estab
                                INNER JOIN dcc_mv_rrs_d1544m hier ON
                                    hier.hier_cb_emp_person_ref =  estab.person_ref AND
                                    hier.hier_cb_emp_post_ref   =  estab.post_ref AND
                                    hier.hier_cb_level_no       =  '1' AND
                                    hier.hier_cb_type           =  'POST'
                            WHERE
                                estab.surname LIKE :pSurname
                            ORDER BY
                                estab.post_number
                            ";
                        dbf.AddParameter("pSurname", "%" + pSurname.ToUpper() + "%", 40);
                    }

                    #endregion
                }


                #region get all postsholders of a given post no - vacant posts have the employee data of the PREVIOUS postholder
                // remove the cb_level_no = '1' to get ALL current posts under a given manager

                else if ( (!string.IsNullOrEmpty(pPostNo)) && (pCurrentAndOrVacant == 2) && (pReportingLine == 3) && (!string.IsNullOrEmpty(pHierarchyLevel2)) )
                {
                    DebugTrace.WriteLine("QUERY: EmployeeAndPostList022");//////  GWW 31/3/23 added test for HL2
                    SQL = @"**
                        SELECT
                            estab.*,
                            hier.hier_cb_man_person_ref
                        FROM
                            dcc_v_myworkforce_estab estab
                            INNER JOIN dcc_mv_rrs_d1544m hier ON
                                hier.hier_cb_emp_post_ref   =  estab.post_ref AND
                                hier.hier_cb_type           =  'POST' AND
                                hier.hier_cb_level_no       =  '1'
                        WHERE
                            estab.hierarchy_level_2 = :pHierarchyLevel2 AND
                            estab.POST_NUMBER = :pPostNo
                        ORDER BY
                            estab.POST_NUMBER
                        ";
                    dbf.AddParameter("pHierarchyLevel2", pHierarchyLevel2, 40);
                    dbf.AddParameter("pPostNo", pPostNo.ToUpper(), 40);
                }

                #endregion

                #region get all postsholders of a given post no - vacant posts have the employee data of the PREVIOUS postholder
                // remove the cb_level_no = '1' to get ALL current posts under a given manager

                else if ( (!string.IsNullOrEmpty(pPostNo)) && (pCurrentAndOrVacant == 2) && (pReportingLine == 3) )
                {
                    DebugTrace.WriteLine("QUERY: EmployeeAndPostList023");
                    SQL = @"**
                        SELECT
                            estab.*,
                            hier.hier_cb_man_person_ref
                        FROM
                            dcc_v_myworkforce_estab estab
                            INNER JOIN dcc_mv_rrs_d1544m hier ON
                                hier.hier_cb_emp_post_ref   =  estab.post_ref AND
                                hier.hier_cb_type           =  'POST' AND
                                hier.hier_cb_level_no       =  '1'
                        WHERE
                            estab.POST_NUMBER = :pPostNo
                        ORDER BY
                            estab.POST_NUMBER
                        ";
                    dbf.AddParameter("pPostNo", pPostNo.ToUpper(), 40);
                }

                #endregion



                dbf.CommandText = Clean_SQL;
                dbf.AddTable("dcc_v_myworkforce_estab");
                db.DataLayer.SelectData(dbf);
                if (dbf.DataSelected != null) if (dbf.hasTables) foreach (DataRow dr in dbf.TableZeroRows)
                {
                    bool ok = true;
                    DCC_EmployeeAndPost row = new DCC_EmployeeAndPost(dr);

                    if (pOmitSelf)   // gww 31/5/23 added test for ph_type=="C" to remove kathryn problem where she is trying to find her old vacant post
                    {
                        if ( (row.PH_Type == "C") && (row.EmployeeNo == session.EmployeeNumber) ) ok = false;
                    }

                    if (!pAllowDuplicates)
                    {
                        int x = All.FindIndex(o => o.PostNo == row.PostNo);
                        if (x > -1) ok = false;
                    }

                    // ensure a post/post holder is only added once
                    int x2 = All.FindIndex(o => o.PostNoAndName == row.PostNoAndName);
                    if (x2 > -1) ok = false;

                    if (ok)
                    {
                        row.LoadExtra(session, pCurrentOrPrevious, pLoadPrevious, pLoadManager);
                        All.Add(row);
                    }
                }
            }
        }



        #region old stuff

        public DCC_EmployeeAndPostList(DCC_Session session, string pHierarchyLevel2 = null, string pHierarchyLevel3 = null, string pHierarchyLevel4 = null, string pHierarchyLevel5 = null, string pHierarchyLevel6 = null, string pHierarchyLevel7 = null, string pHierarchyLevel8 = null, string pHierarchyLevel9 = null, string pPostNoSearch = null, string pSurnameSearch = null, bool managersOnly = false, bool currentAndVacant = false, bool pOmitSelf = false)
        {
            if (session.ResourceLinkDatabaseConnection != null)
            {
                DCC_Database db = session.ResourceLinkDatabaseConnection;



                if (managersOnly)
                {
                    if (!string.IsNullOrEmpty(pSurnameSearch))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX01");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT DISTINCT mgr_person_ref, mgr_employee_number, mgr_surname, mgr_known_as_surname, mgr_first_forename, mgr_known_as, mgr_other_forenames, mgr_post_ref, mgr_post_number, mgr_post_title, mgr_ph_start_date, mgr_ph_end_date FROM dcc_mv_rrs_post_manager WHERE mgr_known_as_surname LIKE :pSurnameSearch ORDER BY mgr_known_as_surname, mgr_known_as");
                        dbf.AddParameter("pSurnameSearch", "%" + pSurnameSearch.ToUpper() + "%", 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if (dbf.DataSelected != null) if (dbf.hasTables) foreach (DataRow dr in dbf.TableZeroRows)
                        {
                            DCC_EmployeeAndPost mgr = new DCC_EmployeeAndPost(dr, true);

                            bool ok = true;
                            if (pOmitSelf)
                            {
                                if (mgr.EmployeeNo == session.EmployeeNumber) ok = false;
                            }

                            if (ok) All.Add(mgr);
                        }
                    }
                    else if (!string.IsNullOrEmpty(pHierarchyLevel9))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX02");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT DISTINCT mgr_person_ref, mgr_employee_number, mgr_surname, mgr_known_as_surname, mgr_first_forename, mgr_known_as, mgr_other_forenames, mgr_post_ref, mgr_post_number, mgr_post_title, mgr_ph_start_date, mgr_ph_end_date, hierarchy_level_2, hierarchy_level_3, hierarchy_level_4, hierarchy_level_5 FROM dcc_mv_rrs_post_manager JOIN dcc_v_myworkforce_estab ON mgr_person_ref = dcc_v_myworkforce_estab.person_ref WHERE hierarchy_level_9 = :pHierarchyLevel9 ORDER BY mgr_known_as_surname, mgr_known_as");
                        dbf.AddParameter("pHierarchyLevel9", pHierarchyLevel9, 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if (dbf.DataSelected != null) if (dbf.hasTables) foreach (DataRow dr in dbf.TableZeroRows)
                        {
                            DCC_EmployeeAndPost mgr = new DCC_EmployeeAndPost(dr, true);
                            All.Add(mgr);
                        }
                    }
                    else if (!string.IsNullOrEmpty(pHierarchyLevel8))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX03");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT DISTINCT mgr_person_ref, mgr_employee_number, mgr_surname, mgr_known_as_surname, mgr_first_forename, mgr_known_as, mgr_other_forenames, mgr_post_ref, mgr_post_number, mgr_post_title, mgr_ph_start_date, mgr_ph_end_date, hierarchy_level_2, hierarchy_level_3, hierarchy_level_4, hierarchy_level_5 FROM dcc_mv_rrs_post_manager JOIN dcc_v_myworkforce_estab ON mgr_person_ref = dcc_v_myworkforce_estab.person_ref WHERE hierarchy_level_8 = :pHierarchyLevel8 ORDER BY mgr_known_as_surname, mgr_known_as");
                        dbf.AddParameter("pHierarchyLevel8", pHierarchyLevel8, 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if (dbf.DataSelected != null) if (dbf.hasTables) foreach (DataRow dr in dbf.TableZeroRows)
                        {
                            DCC_EmployeeAndPost mgr = new DCC_EmployeeAndPost(dr, true);
                            All.Add(mgr);
                        }
                    }
                    else if (!string.IsNullOrEmpty(pHierarchyLevel7))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX04");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT DISTINCT mgr_person_ref, mgr_employee_number, mgr_surname, mgr_known_as_surname, mgr_first_forename, mgr_known_as, mgr_other_forenames, mgr_post_ref, mgr_post_number, mgr_post_title, mgr_ph_start_date, mgr_ph_end_date, hierarchy_level_2, hierarchy_level_3, hierarchy_level_4, hierarchy_level_5 FROM dcc_mv_rrs_post_manager JOIN dcc_v_myworkforce_estab ON mgr_person_ref = dcc_v_myworkforce_estab.person_ref WHERE hierarchy_level_7 = :pHierarchyLevel7 ORDER BY mgr_known_as_surname, mgr_known_as");
                        dbf.AddParameter("pHierarchyLevel7", pHierarchyLevel7, 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if (dbf.DataSelected != null) if (dbf.hasTables) foreach (DataRow dr in dbf.TableZeroRows)
                        {
                            DCC_EmployeeAndPost mgr = new DCC_EmployeeAndPost(dr, true);
                            All.Add(mgr);
                        }
                    }
                    else if (!string.IsNullOrEmpty(pHierarchyLevel6))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX05");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT DISTINCT mgr_person_ref, mgr_employee_number, mgr_surname, mgr_known_as_surname, mgr_first_forename, mgr_known_as, mgr_other_forenames, mgr_post_ref, mgr_post_number, mgr_post_title, mgr_ph_start_date, mgr_ph_end_date, hierarchy_level_2, hierarchy_level_3, hierarchy_level_4, hierarchy_level_5 FROM dcc_mv_rrs_post_manager JOIN dcc_v_myworkforce_estab ON mgr_person_ref = dcc_v_myworkforce_estab.person_ref WHERE hierarchy_level_6 = :pHierarchyLevel6 ORDER BY mgr_known_as_surname, mgr_known_as");
                        dbf.AddParameter("pHierarchyLevel6", pHierarchyLevel6, 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if (dbf.DataSelected != null) if (dbf.hasTables) foreach (DataRow dr in dbf.TableZeroRows)
                        {
                            DCC_EmployeeAndPost mgr = new DCC_EmployeeAndPost(dr, true);
                            All.Add(mgr);
                        }
                    }
                    else if (!string.IsNullOrEmpty(pHierarchyLevel5))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX06");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT DISTINCT mgr_person_ref, mgr_employee_number, mgr_surname, mgr_known_as_surname, mgr_first_forename, mgr_known_as, mgr_other_forenames, mgr_post_ref, mgr_post_number, mgr_post_title, mgr_ph_start_date, mgr_ph_end_date, hierarchy_level_2, hierarchy_level_3, hierarchy_level_4, hierarchy_level_5 FROM dcc_mv_rrs_post_manager JOIN dcc_v_myworkforce_estab ON mgr_person_ref = dcc_v_myworkforce_estab.person_ref WHERE hierarchy_level_5 = :pHierarchyLevel5 ORDER BY mgr_known_as_surname, mgr_known_as");
                        dbf.AddParameter("pHierarchyLevel5", pHierarchyLevel5, 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if (dbf.DataSelected != null) if (dbf.hasTables) foreach (DataRow dr in dbf.TableZeroRows)
                        {
                            DCC_EmployeeAndPost mgr = new DCC_EmployeeAndPost(dr, true);
                            All.Add(mgr);
                        }
                    }
                    else if (!string.IsNullOrEmpty(pHierarchyLevel4))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX07");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT DISTINCT mgr_person_ref, mgr_employee_number, mgr_surname, mgr_known_as_surname, mgr_first_forename, mgr_known_as, mgr_other_forenames, mgr_post_ref, mgr_post_number, mgr_post_title, mgr_ph_start_date, mgr_ph_end_date, hierarchy_level_2, hierarchy_level_3, hierarchy_level_4, hierarchy_level_5 FROM dcc_mv_rrs_post_manager JOIN dcc_v_myworkforce_estab ON mgr_person_ref = dcc_v_myworkforce_estab.person_ref WHERE hierarchy_level_4 = :pHierarchyLevel4 ORDER BY mgr_known_as_surname, mgr_known_as");
                        dbf.AddParameter("pHierarchyLevel4", pHierarchyLevel4, 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if (dbf.DataSelected != null) if (dbf.hasTables) foreach (DataRow dr in dbf.TableZeroRows)
                        {
                            DCC_EmployeeAndPost mgr = new DCC_EmployeeAndPost(dr, true);
                            All.Add(mgr);
                        }
                    }
                    else if (!string.IsNullOrEmpty(pHierarchyLevel3))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX08");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT DISTINCT mgr_person_ref, mgr_employee_number, mgr_surname, mgr_known_as_surname, mgr_first_forename, mgr_known_as, mgr_other_forenames, mgr_post_ref, mgr_post_number, mgr_post_title, mgr_ph_start_date, mgr_ph_end_date, hierarchy_level_2, hierarchy_level_3, hierarchy_level_4, hierarchy_level_5 FROM dcc_mv_rrs_post_manager JOIN dcc_v_myworkforce_estab ON mgr_person_ref = dcc_v_myworkforce_estab.person_ref WHERE hierarchy_level_3 = :pHierarchyLevel3 ORDER BY mgr_known_as_surname, mgr_known_as");
                        dbf.AddParameter("pHierarchyLevel3", pHierarchyLevel3, 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if (dbf.DataSelected != null) if (dbf.hasTables) foreach (DataRow dr in dbf.TableZeroRows)
                        {
                            DCC_EmployeeAndPost mgr = new DCC_EmployeeAndPost(dr, true);
                            All.Add(mgr);
                        }
                    }
                    else if (!string.IsNullOrEmpty(pHierarchyLevel2))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX09");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT DISTINCT mgr_person_ref, mgr_employee_number, mgr_surname, mgr_known_as_surname, mgr_first_forename, mgr_known_as, mgr_other_forenames, mgr_post_ref, mgr_post_number, mgr_post_title, mgr_ph_start_date, mgr_ph_end_date, hierarchy_level_2, hierarchy_level_3, hierarchy_level_4, hierarchy_level_5 FROM dcc_mv_rrs_post_manager JOIN dcc_v_myworkforce_estab ON mgr_person_ref = dcc_v_myworkforce_estab.person_ref WHERE hierarchy_level_2 = :pHierarchyLevel2 ORDER BY mgr_known_as_surname, mgr_known_as");
                        dbf.AddParameter("pHierarchyLevel2", pHierarchyLevel2, 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if (dbf.DataSelected != null) if (dbf.hasTables) foreach (DataRow dr in dbf.TableZeroRows)
                        {
                            DCC_EmployeeAndPost mgr = new DCC_EmployeeAndPost(dr, true);
                            All.Add(mgr);
                        }
                    }
                }



                else if (currentAndVacant)
                {
                    if (!string.IsNullOrEmpty(pPostNoSearch))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX10");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_estab JOIN dcc_mv_rrs_p2p_occ ON dcc_mv_rrs_p2p_occ.post_ref = dcc_v_myworkforce_estab.post_ref WHERE dcc_v_myworkforce_estab.POST_NUMBER = :pPostNoSearch ORDER BY dcc_v_myworkforce_estab.post_number");
                        dbf.AddParameter("pPostNoSearch", pPostNoSearch.ToUpper(), 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if (dbf.DataSelected != null) if (dbf.hasTables) foreach (DataRow dr in dbf.TableZeroRows)
                        {
                            DCC_EmployeeAndPost mgr = new DCC_EmployeeAndPost(dr);
                            All.Add(mgr);
                        }
                    }
                    else if ( (!string.IsNullOrEmpty(pSurnameSearch)) && (!string.IsNullOrEmpty(pHierarchyLevel5)))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX11");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_estab JOIN dcc_mv_rrs_p2p_occ ON dcc_mv_rrs_p2p_occ.post_ref = dcc_v_myworkforce_estab.post_ref WHERE dcc_v_myworkforce_estab.hierarchy_level_5 = :pHierarchyLevel5 AND known_as_surname LIKE :pSurnameSearch ORDER BY dcc_v_myworkforce_estab.post_number, dcc_v_myworkforce_estab.ph_type");
                        dbf.AddParameter("pHierarchyLevel5", pHierarchyLevel5, 40);
                        dbf.AddParameter("pSurnameSearch", "%" + pSurnameSearch.ToUpper() + "%", 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if ((dbf.DataSelected != null) && (dbf.hasTables))
                        {
                            foreach (DataRow dr in dbf.TableZeroRows)
                            {
                                DCC_EmployeeAndPost post = new DCC_EmployeeAndPost(dr);
                                if (All.FindIndex(o => o.PostNo == post.PostNo) == -1) All.Add(post);
                            }
                        }
                    }
                    else if ( (!string.IsNullOrEmpty(pSurnameSearch)) && (!string.IsNullOrEmpty(pHierarchyLevel4)))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX12");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_estab JOIN dcc_mv_rrs_p2p_occ ON dcc_mv_rrs_p2p_occ.post_ref = dcc_v_myworkforce_estab.post_ref WHERE dcc_v_myworkforce_estab.hierarchy_level_4 = :pHierarchyLevel4 AND known_as_surname LIKE :pSurnameSearch ORDER BY dcc_v_myworkforce_estab.post_number, dcc_v_myworkforce_estab.ph_type");
                        dbf.AddParameter("pHierarchyLevel4", pHierarchyLevel4, 40);
                        dbf.AddParameter("pSurnameSearch", "%" + pSurnameSearch.ToUpper() + "%", 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if ((dbf.DataSelected != null) && (dbf.hasTables))
                        {
                            foreach (DataRow dr in dbf.TableZeroRows)
                            {
                                DCC_EmployeeAndPost post = new DCC_EmployeeAndPost(dr);
                                if (All.FindIndex(o => o.PostNo == post.PostNo) == -1) All.Add(post);
                            }
                        }
                    }
                    else if ( (!string.IsNullOrEmpty(pSurnameSearch)) && (!string.IsNullOrEmpty(pHierarchyLevel3)))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX13");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_estab JOIN dcc_mv_rrs_p2p_occ ON dcc_mv_rrs_p2p_occ.post_ref = dcc_v_myworkforce_estab.post_ref WHERE dcc_v_myworkforce_estab.hierarchy_level_3 = :pHierarchyLevel3 AND known_as_surname LIKE :pSurnameSearch ORDER BY dcc_v_myworkforce_estab.post_number, dcc_v_myworkforce_estab.ph_type");
                        dbf.AddParameter("pHierarchyLevel3", pHierarchyLevel3, 40);
                        dbf.AddParameter("pSurnameSearch", "%" + pSurnameSearch.ToUpper() + "%", 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if ((dbf.DataSelected != null) && (dbf.hasTables))
                        {
                            foreach (DataRow dr in dbf.TableZeroRows)
                            {
                                DCC_EmployeeAndPost post = new DCC_EmployeeAndPost(dr);
                                if (All.FindIndex(o => o.PostNo == post.PostNo) == -1) All.Add(post);
                            }
                        }
                    }

                    else if ( (!string.IsNullOrEmpty(pSurnameSearch)) && (!string.IsNullOrEmpty(pHierarchyLevel2)))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX14");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_estab JOIN dcc_mv_rrs_p2p_occ ON dcc_mv_rrs_p2p_occ.post_ref = dcc_v_myworkforce_estab.post_ref WHERE dcc_v_myworkforce_estab.hierarchy_level_2 = :pHierarchyLevel2 AND known_as_surname LIKE :pSurnameSearch ORDER BY dcc_v_myworkforce_estab.post_number, dcc_v_myworkforce_estab.ph_type");
                        dbf.AddParameter("pHierarchyLevel2", pHierarchyLevel2, 40);
                        dbf.AddParameter("pSurnameSearch", "%" + pSurnameSearch.ToUpper() + "%", 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if ((dbf.DataSelected != null) && (dbf.hasTables))
                        {
                            foreach (DataRow dr in dbf.TableZeroRows)
                            {
                                DCC_EmployeeAndPost post = new DCC_EmployeeAndPost(dr);
                                if (All.FindIndex(o => o.PostNo == post.PostNo) == -1) All.Add(post);
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(pSurnameSearch))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX15");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_estab JOIN dcc_mv_rrs_p2p_occ ON dcc_mv_rrs_p2p_occ.post_ref = dcc_v_myworkforce_estab.post_ref WHERE known_as_surname LIKE :pSurnameSearch ORDER BY dcc_v_myworkforce_estab.post_number, dcc_v_myworkforce_estab.ph_type");
                        dbf.AddParameter("pSurnameSearch", "%" + pSurnameSearch.ToUpper() + "%", 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if ((dbf.DataSelected != null) && (dbf.hasTables))
                        {
                            foreach (DataRow dr in dbf.TableZeroRows)
                            {
                                DCC_EmployeeAndPost post = new DCC_EmployeeAndPost(dr);
                                if (All.FindIndex(o => o.PostNo == post.PostNo) == -1) All.Add(post);
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(pHierarchyLevel5))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX16");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_estab JOIN dcc_mv_rrs_p2p_occ ON dcc_mv_rrs_p2p_occ.post_ref = dcc_v_myworkforce_estab.post_ref WHERE dcc_v_myworkforce_estab.hierarchy_level_5 = :pHierarchyLevel5 ORDER BY dcc_v_myworkforce_estab.post_number, dcc_v_myworkforce_estab.ph_type");
                        dbf.AddParameter("pHierarchyLevel5", pHierarchyLevel5, 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if ((dbf.DataSelected != null) && (dbf.hasTables))
                        {
                            foreach (DataRow dr in dbf.TableZeroRows)
                            {
                                DCC_EmployeeAndPost post = new DCC_EmployeeAndPost(dr);
                                if ( (post.PH_Type == "C") && (string.IsNullOrWhiteSpace(post.FullName)) )
                                {
                                }
                                else
                                {
                                    if (All.FindIndex(o => o.PostNo == post.PostNo) == -1) All.Add(post);
                                }
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(pHierarchyLevel4))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX17");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_estab JOIN dcc_mv_rrs_p2p_occ ON dcc_mv_rrs_p2p_occ.post_ref = dcc_v_myworkforce_estab.post_ref WHERE dcc_v_myworkforce_estab.hierarchy_level_4 = :pHierarchyLevel4 ORDER BY dcc_v_myworkforce_estab.post_number, dcc_v_myworkforce_estab.ph_type");
                        dbf.AddParameter("pHierarchyLevel4", pHierarchyLevel4, 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if ((dbf.DataSelected != null) && (dbf.hasTables))
                        {
                            foreach (DataRow dr in dbf.TableZeroRows)
                            {
                                DCC_EmployeeAndPost post = new DCC_EmployeeAndPost(dr);
                                if ( (post.PH_Type == "C") && (string.IsNullOrWhiteSpace(post.FullName)) )
                                {
                                }
                                else
                                {
                                    if (All.FindIndex(o => o.PostNo == post.PostNo) == -1) All.Add(post);
                                }
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(pHierarchyLevel3))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX18");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_estab JOIN dcc_mv_rrs_p2p_occ ON dcc_mv_rrs_p2p_occ.post_ref = dcc_v_myworkforce_estab.post_ref WHERE dcc_v_myworkforce_estab.hierarchy_level_3 = :pHierarchyLevel3 ORDER BY dcc_v_myworkforce_estab.post_number, dcc_v_myworkforce_estab.ph_type");
                        dbf.AddParameter("pHierarchyLevel3", pHierarchyLevel3, 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if ((dbf.DataSelected != null) && (dbf.hasTables))
                        {
                            foreach (DataRow dr in dbf.TableZeroRows)
                            {
                                DCC_EmployeeAndPost post = new DCC_EmployeeAndPost(dr);
                                if ( (post.PH_Type == "C") && (string.IsNullOrWhiteSpace(post.FullName)) )
                                {
                                }
                                else
                                {
                                    if (All.FindIndex(o => o.PostNo == post.PostNo) == -1) All.Add(post);
                                }
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(pHierarchyLevel2))///////////// this takes a minute when creating a new form
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX19");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_estab JOIN dcc_mv_rrs_p2p_occ ON dcc_mv_rrs_p2p_occ.post_ref = dcc_v_myworkforce_estab.post_ref WHERE dcc_v_myworkforce_estab.hierarchy_level_2 = :pHierarchyLevel2 ORDER BY dcc_v_myworkforce_estab.post_number, dcc_v_myworkforce_estab.ph_type");
                        dbf.AddParameter("pHierarchyLevel2", pHierarchyLevel2, 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if ((dbf.DataSelected != null) && (dbf.hasTables))
                        {
                            foreach (DataRow dr in dbf.TableZeroRows)
                            {
                                DCC_EmployeeAndPost post = new DCC_EmployeeAndPost(dr);
                                if ( (post.PH_Type == "C") && (string.IsNullOrWhiteSpace(post.FullName)) )
                                {
                                }
                                else
                                {
                                    if (All.FindIndex(o => o.PostNo == post.PostNo) == -1) All.Add(post);
                                }
                            }
                        }
                    }
                    else
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX20");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_estab JOIN dcc_mv_rrs_p2p_occ ON dcc_mv_rrs_p2p_occ.post_ref = dcc_v_myworkforce_estab.post_ref ORDER BY dcc_v_myworkforce_estab.post_number, dcc_v_myworkforce_estab.ph_type");
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if ((dbf.DataSelected != null) && (dbf.hasTables))
                        {
                            foreach (DataRow dr in dbf.TableZeroRows)
                            {
                                DCC_EmployeeAndPost post = new DCC_EmployeeAndPost(dr);
                                if ( (post.PH_Type == "C") && (string.IsNullOrWhiteSpace(post.FullName)) )
                                {
                                }
                                else
                                {
                                    if (All.FindIndex(o => o.PostNo == post.PostNo) == -1) All.Add(post);
                                }
                            }
                        }
                    }
                }



                else // not managersOnly OR currentAndVacant
                {
                    if (!string.IsNullOrEmpty(pPostNoSearch))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX21");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_estab JOIN dcc_mv_rrs_p2p_occ ON dcc_mv_rrs_p2p_occ.post_ref = dcc_v_myworkforce_estab.post_ref WHERE dcc_v_myworkforce_estab.POST_NUMBER = :pPostNoSearch ORDER BY dcc_v_myworkforce_estab.post_number");
                        dbf.AddParameter("pPostNoSearch", pPostNoSearch.ToUpper(), 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if (dbf.DataSelected != null) if (dbf.hasTables) foreach (DataRow dr in dbf.TableZeroRows)
                        {
                            DCC_EmployeeAndPost mgr = new DCC_EmployeeAndPost(dr);
                            All.Add(mgr);
                        }
                    }
                    else if ( (!string.IsNullOrEmpty(pSurnameSearch)) && (!string.IsNullOrEmpty(pHierarchyLevel9)))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX22");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_estab JOIN dcc_mv_rrs_p2p_occ ON dcc_mv_rrs_p2p_occ.post_ref = dcc_v_myworkforce_estab.post_ref WHERE dcc_v_myworkforce_estab.hierarchy_level_9 = :pHierarchyLevel9 AND known_as_surname LIKE :pSurnameSearch ORDER BY dcc_v_myworkforce_estab.post_number");
                        dbf.AddParameter("pHierarchyLevel9", pHierarchyLevel9, 40);
                        dbf.AddParameter("pSurnameSearch", "%" + pSurnameSearch.ToUpper() + "%", 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if ((dbf.DataSelected != null) && (dbf.hasTables))
                        {
                            foreach (DataRow dr in dbf.TableZeroRows)
                            {
                                DCC_EmployeeAndPost post = new DCC_EmployeeAndPost(dr);
                                if (All.FindIndex(o => o.PostNo == post.PostNo) == -1) All.Add(post);
                            }
                        }
                    }
                    else if ( (!string.IsNullOrEmpty(pSurnameSearch)) && (!string.IsNullOrEmpty(pHierarchyLevel8)))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX23");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_estab JOIN dcc_mv_rrs_p2p_occ ON dcc_mv_rrs_p2p_occ.post_ref = dcc_v_myworkforce_estab.post_ref WHERE dcc_v_myworkforce_estab.hierarchy_level_8 = :pHierarchyLevel8 AND known_as_surname LIKE :pSurnameSearch ORDER BY dcc_v_myworkforce_estab.post_number");
                        dbf.AddParameter("pHierarchyLevel8", pHierarchyLevel8, 40);
                        dbf.AddParameter("pSurnameSearch", "%" + pSurnameSearch.ToUpper() + "%", 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if ((dbf.DataSelected != null) && (dbf.hasTables))
                        {
                            foreach (DataRow dr in dbf.TableZeroRows)
                            {
                                DCC_EmployeeAndPost post = new DCC_EmployeeAndPost(dr);
                                if (All.FindIndex(o => o.PostNo == post.PostNo) == -1) All.Add(post);
                            }
                        }
                    }
                    else if ( (!string.IsNullOrEmpty(pSurnameSearch)) && (!string.IsNullOrEmpty(pHierarchyLevel7)))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX24");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_estab JOIN dcc_mv_rrs_p2p_occ ON dcc_mv_rrs_p2p_occ.post_ref = dcc_v_myworkforce_estab.post_ref WHERE dcc_v_myworkforce_estab.hierarchy_level_7 = :pHierarchyLevel7 AND known_as_surname LIKE :pSurnameSearch ORDER BY dcc_v_myworkforce_estab.post_number");
                        dbf.AddParameter("pHierarchyLevel7", pHierarchyLevel7, 40);
                        dbf.AddParameter("pSurnameSearch", "%" + pSurnameSearch.ToUpper() + "%", 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if ((dbf.DataSelected != null) && (dbf.hasTables))
                        {
                            foreach (DataRow dr in dbf.TableZeroRows)
                            {
                                DCC_EmployeeAndPost post = new DCC_EmployeeAndPost(dr);
                                if (All.FindIndex(o => o.PostNo == post.PostNo) == -1) All.Add(post);
                            }
                        }
                    }
                    else if ( (!string.IsNullOrEmpty(pSurnameSearch)) && (!string.IsNullOrEmpty(pHierarchyLevel6)))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX25");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_estab JOIN dcc_mv_rrs_p2p_occ ON dcc_mv_rrs_p2p_occ.post_ref = dcc_v_myworkforce_estab.post_ref WHERE dcc_v_myworkforce_estab.hierarchy_level_6 = :pHierarchyLevel6 AND known_as_surname LIKE :pSurnameSearch ORDER BY dcc_v_myworkforce_estab.post_number");
                        dbf.AddParameter("pHierarchyLevel6", pHierarchyLevel6, 40);
                        dbf.AddParameter("pSurnameSearch", "%" + pSurnameSearch.ToUpper() + "%", 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if ((dbf.DataSelected != null) && (dbf.hasTables))
                        {
                            foreach (DataRow dr in dbf.TableZeroRows)
                            {
                                DCC_EmployeeAndPost post = new DCC_EmployeeAndPost(dr);
                                if (All.FindIndex(o => o.PostNo == post.PostNo) == -1) All.Add(post);
                            }
                        }
                    }
                    else if ( (!string.IsNullOrEmpty(pSurnameSearch)) && (!string.IsNullOrEmpty(pHierarchyLevel5)))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX26");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_estab JOIN dcc_mv_rrs_p2p_occ ON dcc_mv_rrs_p2p_occ.post_ref = dcc_v_myworkforce_estab.post_ref WHERE dcc_v_myworkforce_estab.hierarchy_level_5 = :pHierarchyLevel5 AND known_as_surname LIKE :pSurnameSearch ORDER BY dcc_v_myworkforce_estab.post_number");
                        dbf.AddParameter("pHierarchyLevel5", pHierarchyLevel5, 40);
                        dbf.AddParameter("pSurnameSearch", "%" + pSurnameSearch.ToUpper() + "%", 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if ((dbf.DataSelected != null) && (dbf.hasTables))
                        {
                            foreach (DataRow dr in dbf.TableZeroRows)
                            {
                                DCC_EmployeeAndPost post = new DCC_EmployeeAndPost(dr);
                                if (All.FindIndex(o => o.PostNo == post.PostNo) == -1) All.Add(post);
                            }
                        }
                    }
                    else if ( (!string.IsNullOrEmpty(pSurnameSearch)) && (!string.IsNullOrEmpty(pHierarchyLevel4)))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX27");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_estab JOIN dcc_mv_rrs_p2p_occ ON dcc_mv_rrs_p2p_occ.post_ref = dcc_v_myworkforce_estab.post_ref WHERE dcc_v_myworkforce_estab.hierarchy_level_4 = :pHierarchyLevel4 AND known_as_surname LIKE :pSurnameSearch ORDER BY dcc_v_myworkforce_estab.post_number");
                        dbf.AddParameter("pHierarchyLevel4", pHierarchyLevel4, 40);
                        dbf.AddParameter("pSurnameSearch", "%" + pSurnameSearch.ToUpper() + "%", 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if ((dbf.DataSelected != null) && (dbf.hasTables))
                        {
                            foreach (DataRow dr in dbf.TableZeroRows)
                            {
                                DCC_EmployeeAndPost post = new DCC_EmployeeAndPost(dr);
                                if (All.FindIndex(o => o.PostNo == post.PostNo) == -1) All.Add(post);
                            }
                        }
                    }
                    else if ( (!string.IsNullOrEmpty(pSurnameSearch)) && (!string.IsNullOrEmpty(pHierarchyLevel3)))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX28");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_estab JOIN dcc_mv_rrs_p2p_occ ON dcc_mv_rrs_p2p_occ.post_ref = dcc_v_myworkforce_estab.post_ref WHERE dcc_v_myworkforce_estab.hierarchy_level_3 = :pHierarchyLevel3 AND known_as_surname LIKE :pSurnameSearch ORDER BY dcc_v_myworkforce_estab.post_number");
                        dbf.AddParameter("pHierarchyLevel3", pHierarchyLevel3, 40);
                        dbf.AddParameter("pSurnameSearch", "%" + pSurnameSearch.ToUpper() + "%", 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if ((dbf.DataSelected != null) && (dbf.hasTables))
                        {
                            foreach (DataRow dr in dbf.TableZeroRows)
                            {
                                DCC_EmployeeAndPost post = new DCC_EmployeeAndPost(dr);
                                if (All.FindIndex(o => o.PostNo == post.PostNo) == -1) All.Add(post);
                            }
                        }
                    }
                    else if ( (!string.IsNullOrEmpty(pSurnameSearch)) && (!string.IsNullOrEmpty(pHierarchyLevel2)))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX29");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_estab JOIN dcc_mv_rrs_p2p_occ ON dcc_mv_rrs_p2p_occ.post_ref = dcc_v_myworkforce_estab.post_ref WHERE dcc_v_myworkforce_estab.hierarchy_level_2 = :pHierarchyLevel2 AND known_as_surname LIKE :pSurnameSearch ORDER BY dcc_v_myworkforce_estab.post_number");
                        dbf.AddParameter("pHierarchyLevel2", pHierarchyLevel2, 40);
                        dbf.AddParameter("pSurnameSearch", "%" + pSurnameSearch.ToUpper() + "%", 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if (dbf.DataSelected != null) if (dbf.hasTables) foreach (DataRow dr in dbf.TableZeroRows)
                        {
                            DCC_EmployeeAndPost post = new DCC_EmployeeAndPost(dr);
                            if (All.FindIndex(o => o.PostNo == post.PostNo) == -1) All.Add(post);
                        }
                    }
                    else if (!string.IsNullOrEmpty(pSurnameSearch))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX30");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_estab JOIN dcc_mv_rrs_p2p_occ ON dcc_mv_rrs_p2p_occ.post_ref = dcc_v_myworkforce_estab.post_ref WHERE known_as_surname LIKE :pSurnameSearch ORDER BY dcc_v_myworkforce_estab.post_number");
                        dbf.AddParameter("pSurnameSearch", "%" + pSurnameSearch.ToUpper() + "%", 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if (dbf.DataSelected != null) if (dbf.hasTables) foreach (DataRow dr in dbf.TableZeroRows)
                        {
                            DCC_EmployeeAndPost post = new DCC_EmployeeAndPost(dr);
                            if (All.FindIndex(o => o.PostNo == post.PostNo) == -1) All.Add(post);
                        }
                    }
                    else if (!string.IsNullOrEmpty(pHierarchyLevel9))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX31");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_estab JOIN dcc_mv_rrs_p2p_occ ON dcc_mv_rrs_p2p_occ.post_ref = dcc_v_myworkforce_estab.post_ref WHERE dcc_v_myworkforce_estab.hierarchy_level_9 = :pHierarchyLevel9 ORDER BY dcc_v_myworkforce_estab.post_number");
                        dbf.AddParameter("pHierarchyLevel9", pHierarchyLevel9, 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if ((dbf.DataSelected != null) && (dbf.hasTables))
                        {
                            foreach (DataRow dr in dbf.TableZeroRows)
                            {
                                DCC_EmployeeAndPost post = new DCC_EmployeeAndPost(dr);
                                int i = All.FindIndex(o => o.PostNo == post.PostNo);
                                if (i == -1) All.Add(post);
                                else
                                {
                                    if (post.PH_Type == "P")
                                    {
                                        All[i].Forename = post.Forename;
                                        All[i].Surname = post.Surname;
                                        All[i].KnownAsForename = post.KnownAsForename;
                                        All[i].KnownAsSurname = post.KnownAsSurname;
                                    }
                                }
                                if (post.PH_Type == "P")
                                {
                                    if (AllVacant.FindIndex(o => o.PostNo == post.PostNo) == -1) AllVacant.Add(post);
                                }
                                else if ((post.PH_Type == "C") && (post.PersonReference == "00000000"))
                                {
                                    if (AllVacant.FindIndex(o => o.PostNo == post.PostNo) == -1) AllVacant.Add(post);
                                }
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(pHierarchyLevel8))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX32");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_estab JOIN dcc_mv_rrs_p2p_occ ON dcc_mv_rrs_p2p_occ.post_ref = dcc_v_myworkforce_estab.post_ref WHERE dcc_v_myworkforce_estab.hierarchy_level_8 = :pHierarchyLevel8 ORDER BY dcc_v_myworkforce_estab.post_number");
                        dbf.AddParameter("pHierarchyLevel8", pHierarchyLevel8, 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if ((dbf.DataSelected != null) && (dbf.hasTables))
                        {
                            foreach (DataRow dr in dbf.TableZeroRows)
                            {
                                DCC_EmployeeAndPost post = new DCC_EmployeeAndPost(dr);
                                int i = All.FindIndex(o => o.PostNo == post.PostNo);
                                if (i == -1) All.Add(post);
                                else
                                {
                                    if (post.PH_Type == "P")
                                    {
                                        All[i].Forename = post.Forename;
                                        All[i].Surname = post.Surname;
                                        All[i].KnownAsForename = post.KnownAsForename;
                                        All[i].KnownAsSurname = post.KnownAsSurname;
                                    }
                                }
                                if (post.PH_Type == "P")
                                {
                                    if (AllVacant.FindIndex(o => o.PostNo == post.PostNo) == -1) AllVacant.Add(post);
                                }
                                else if ((post.PH_Type == "C") && (post.PersonReference == "00000000"))
                                {
                                    if (AllVacant.FindIndex(o => o.PostNo == post.PostNo) == -1) AllVacant.Add(post);
                                }
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(pHierarchyLevel7))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX33");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_estab JOIN dcc_mv_rrs_p2p_occ ON dcc_mv_rrs_p2p_occ.post_ref = dcc_v_myworkforce_estab.post_ref WHERE dcc_v_myworkforce_estab.hierarchy_level_7 = :pHierarchyLevel7 ORDER BY dcc_v_myworkforce_estab.post_number");
                        dbf.AddParameter("pHierarchyLevel7", pHierarchyLevel7, 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if ((dbf.DataSelected != null) && (dbf.hasTables))
                        {
                            foreach (DataRow dr in dbf.TableZeroRows)
                            {
                                DCC_EmployeeAndPost post = new DCC_EmployeeAndPost(dr);
                                int i = All.FindIndex(o => o.PostNo == post.PostNo);
                                if (i == -1) All.Add(post);
                                else
                                {
                                    if (post.PH_Type == "P")
                                    {
                                        All[i].Forename = post.Forename;
                                        All[i].Surname = post.Surname;
                                        All[i].KnownAsForename = post.KnownAsForename;
                                        All[i].KnownAsSurname = post.KnownAsSurname;
                                    }
                                }
                                if (post.PH_Type == "P")
                                {
                                    if (AllVacant.FindIndex(o => o.PostNo == post.PostNo) == -1) AllVacant.Add(post);
                                }
                                else if ((post.PH_Type == "C") && (post.PersonReference == "00000000"))
                                {
                                    if (AllVacant.FindIndex(o => o.PostNo == post.PostNo) == -1) AllVacant.Add(post);
                                }
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(pHierarchyLevel6))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX34");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_estab JOIN dcc_mv_rrs_p2p_occ ON dcc_mv_rrs_p2p_occ.post_ref = dcc_v_myworkforce_estab.post_ref WHERE dcc_v_myworkforce_estab.hierarchy_level_6 = :pHierarchyLevel6 ORDER BY dcc_v_myworkforce_estab.post_number");
                        dbf.AddParameter("pHierarchyLevel6", pHierarchyLevel6, 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if ((dbf.DataSelected != null) && (dbf.hasTables))
                        {
                            foreach (DataRow dr in dbf.TableZeroRows)
                            {
                                DCC_EmployeeAndPost post = new DCC_EmployeeAndPost(dr);
                                int i = All.FindIndex(o => o.PostNo == post.PostNo);
                                if (i == -1) All.Add(post);
                                else
                                {
                                    if (post.PH_Type == "P")
                                    {
                                        All[i].Forename = post.Forename;
                                        All[i].Surname = post.Surname;
                                        All[i].KnownAsForename = post.KnownAsForename;
                                        All[i].KnownAsSurname = post.KnownAsSurname;
                                    }
                                }
                                if (post.PH_Type == "P")
                                {
                                    if (AllVacant.FindIndex(o => o.PostNo == post.PostNo) == -1) AllVacant.Add(post);
                                }
                                else if ((post.PH_Type == "C") && (post.PersonReference == "00000000"))
                                {
                                    if (AllVacant.FindIndex(o => o.PostNo == post.PostNo) == -1) AllVacant.Add(post);
                                }
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(pHierarchyLevel5))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX35");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_estab JOIN dcc_mv_rrs_p2p_occ ON dcc_mv_rrs_p2p_occ.post_ref = dcc_v_myworkforce_estab.post_ref WHERE dcc_v_myworkforce_estab.hierarchy_level_5 = :pHierarchyLevel5 ORDER BY dcc_v_myworkforce_estab.post_number");
                        dbf.AddParameter("pHierarchyLevel5", pHierarchyLevel5, 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if ((dbf.DataSelected != null) && (dbf.hasTables))
                        {
                            foreach (DataRow dr in dbf.TableZeroRows)
                            {
                                DCC_EmployeeAndPost post = new DCC_EmployeeAndPost(dr);
                                int i = All.FindIndex(o => o.PostNo == post.PostNo);
                                if (i == -1) All.Add(post);
                                else
                                {
                                    if (post.PH_Type == "P")
                                    {
                                        All[i].Forename = post.Forename;
                                        All[i].Surname = post.Surname;
                                        All[i].KnownAsForename = post.KnownAsForename;
                                        All[i].KnownAsSurname = post.KnownAsSurname;
                                    }
                                }
                                if (post.PH_Type == "P")
                                {
                                    if (AllVacant.FindIndex(o => o.PostNo == post.PostNo) == -1) AllVacant.Add(post);
                                }
                                else if ((post.PH_Type == "C") && (post.PersonReference == "00000000"))
                                {
                                    if (AllVacant.FindIndex(o => o.PostNo == post.PostNo) == -1) AllVacant.Add(post);
                                }
                            }
                        }
                    }












                    /////////////////////////////// used to get posts affected ///////////////////////////

                    else if (!string.IsNullOrEmpty(pHierarchyLevel4))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX36");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_estab JOIN dcc_mv_rrs_p2p_occ ON dcc_mv_rrs_p2p_occ.post_ref = dcc_v_myworkforce_estab.post_ref WHERE dcc_v_myworkforce_estab.hierarchy_level_4 = :pHierarchyLevel4 ORDER BY dcc_v_myworkforce_estab.post_number");
                        dbf.AddParameter("pHierarchyLevel4", pHierarchyLevel4, 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if ((dbf.DataSelected != null) && (dbf.hasTables))
                        {
                            foreach (DataRow dr in dbf.TableZeroRows)
                            {
                                DCC_EmployeeAndPost post = new DCC_EmployeeAndPost(dr);
                                int i = All.FindIndex(o => o.PostNo == post.PostNo);
                                if (i == -1) All.Add(post);
                                else
                                {
                                    if (post.PH_Type == "P")
                                    {
                                        All[i].Forename = post.Forename;
                                        All[i].Surname = post.Surname;
                                        All[i].KnownAsForename = post.KnownAsForename;
                                        All[i].KnownAsSurname = post.KnownAsSurname;
                                    }
                                }
                                if (post.PH_Type == "P")
                                {
                                    if (AllVacant.FindIndex(o => o.PostNo == post.PostNo) == -1) AllVacant.Add(post);
                                }
                                else if ((post.PH_Type == "C") && (post.PersonReference == "00000000"))
                                {
                                    if (AllVacant.FindIndex(o => o.PostNo == post.PostNo) == -1) AllVacant.Add(post);
                                }
                            }
                        }
                    }






                    else if (!string.IsNullOrEmpty(pHierarchyLevel3))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX37");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_estab JOIN dcc_mv_rrs_p2p_occ ON dcc_mv_rrs_p2p_occ.post_ref = dcc_v_myworkforce_estab.post_ref WHERE dcc_v_myworkforce_estab.hierarchy_level_3 = :pHierarchyLevel3 ORDER BY dcc_v_myworkforce_estab.post_number");
                        dbf.AddParameter("pHierarchyLevel3", pHierarchyLevel3, 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if ((dbf.DataSelected != null) && (dbf.hasTables))
                        {
                            foreach (DataRow dr in dbf.TableZeroRows)
                            {
                                DCC_EmployeeAndPost post = new DCC_EmployeeAndPost(dr);
                                int i = All.FindIndex(o => o.PostNo == post.PostNo);
                                if (i == -1) All.Add(post);
                                else
                                {
                                    if (post.PH_Type == "P")
                                    {
                                        All[i].Forename = post.Forename;
                                        All[i].Surname = post.Surname;
                                        All[i].KnownAsForename = post.KnownAsForename;
                                        All[i].KnownAsSurname = post.KnownAsSurname;
                                    }
                                }
                                if (post.PH_Type == "P")
                                {
                                    if (AllVacant.FindIndex(o => o.PostNo == post.PostNo) == -1) AllVacant.Add(post);
                                }
                                else if ((post.PH_Type == "C") && (post.PersonReference == "00000000"))
                                {
                                    if (AllVacant.FindIndex(o => o.PostNo == post.PostNo) == -1) AllVacant.Add(post);
                                }
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(pHierarchyLevel2))
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX38");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_estab JOIN dcc_mv_rrs_p2p_occ ON dcc_mv_rrs_p2p_occ.post_ref = dcc_v_myworkforce_estab.post_ref WHERE dcc_v_myworkforce_estab.hierarchy_level_2 = :pHierarchyLevel2 ORDER BY dcc_v_myworkforce_estab.post_number");
                        dbf.AddParameter("pHierarchyLevel2", pHierarchyLevel2, 40);
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if ((dbf.DataSelected != null) && (dbf.hasTables))
                        {
                            foreach (DataRow dr in dbf.TableZeroRows)
                            {
                                DCC_EmployeeAndPost post = new DCC_EmployeeAndPost(dr);
                                int i = All.FindIndex(o => o.PostNo == post.PostNo);
                                if (i == -1) All.Add(post);
                                else
                                {
                                    if (post.PH_Type == "P")
                                    {
                                        All[i].Forename = post.Forename;
                                        All[i].Surname = post.Surname;
                                        All[i].KnownAsForename = post.KnownAsForename;
                                        All[i].KnownAsSurname = post.KnownAsSurname;
                                    }
                                }
                                if (post.PH_Type == "P")
                                {
                                    if (AllVacant.FindIndex(o => o.PostNo == post.PostNo) == -1) AllVacant.Add(post);
                                }
                                else if ((post.PH_Type == "C") && (post.PersonReference == "00000000"))
                                {
                                    if (AllVacant.FindIndex(o => o.PostNo == post.PostNo) == -1) AllVacant.Add(post);
                                }
                            }
                        }
                    }
                    else
                    {
                        DebugTrace.WriteLine("QUERY: EmployeeAndPostListX39");
                        DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**SELECT * FROM dcc_v_myworkforce_estab JOIN dcc_mv_rrs_p2p_occ ON dcc_mv_rrs_p2p_occ.post_ref = dcc_v_myworkforce_estab.post_ref ORDER BY dcc_v_myworkforce_estab.post_number");
                        dbf.AddTable("dcc_v_myworkforce_estab");
                        db.DataLayer.SelectData(dbf);
                        if ((dbf.DataSelected != null) && (dbf.hasTables))
                        {
                            foreach (DataRow dr in dbf.TableZeroRows)
                            {
                                DCC_EmployeeAndPost post = new DCC_EmployeeAndPost(dr);
                                int i = All.FindIndex(o => o.PostNo == post.PostNo);
                                if (i == -1) All.Add(post);
                                else
                                {
                                    if (post.PH_Type == "P")
                                    {
                                        All[i].Forename = post.Forename;
                                        All[i].Surname = post.Surname;
                                        All[i].KnownAsForename = post.KnownAsForename;
                                        All[i].KnownAsSurname = post.KnownAsSurname;
                                    }
                                }
                                if (post.PH_Type == "P")
                                {
                                    if (AllVacant.FindIndex(o => o.PostNo == post.PostNo) == -1) AllVacant.Add(post);
                                }
                                else if ((post.PH_Type == "C") && (post.PersonReference == "00000000"))
                                {
                                    if (AllVacant.FindIndex(o => o.PostNo == post.PostNo) == -1) AllVacant.Add(post);
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion



        // Enumerator

        public IEnumerator<DCC_EmployeeAndPost> GetEnumerator() { foreach (object o in All) yield return (DCC_EmployeeAndPost)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }

    }

    #endregion

}
