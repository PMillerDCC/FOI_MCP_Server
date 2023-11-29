using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace DCC_Standard
{


    [Serializable]

    public class DCC_Post
    {

        // Properties

        public string Emp_No;
        public string Post_No;
        public string Post_Description;
        public string Hierarchy_Level_1;
        public string Hierarchy_Level_2;
        public string Hierarchy_Level_3;
        public string Hierarchy_Level_4;
        public string Hierarchy_Level_5;
        public string Post_Location_Description;
        public DateTime Post_Start_Date;
        public DateTime? Post_End_Date;
        public string Post_Main_Flag;
        public Decimal? Contract_Hours;
        public string Position_Status;
        public string Post_Location_Address_1;
        public string Post_Location_Address_2;
        public string Post_Location_Address_3;
        public string Post_Location_Address_4;
        public string Post_Location_Post_Code;
        public string Post_Cost_Centre;
        public string Manager_Name;
        public string Manager_Post;
        public string ManagerEmployeeNo;
        public string ManagerPostTitle;



        // Display Properties

        public string Post_Start_DateAsString { get { return Post_Start_Date.ToString(); } }
        public string Post_End_DateAsString { get { return Post_End_Date.ToString(); } }



        // ToString

        public override string ToString() { return Post_Description + ", " + Emp_No; }



        // Constructors

        public DCC_Post()
        {
        }

        public DCC_Post(DataRow dr)
        {
            Load(dr);
        }



        // Methods

        private void Load(DataRow dr)
        {
            Emp_No = DCC_GetColumn.String(dr, "EMP_NO");
            Post_No = DCC_GetColumn.String(dr, "POST_NO");
            Post_Description = DCC_GetColumn.String(dr, "POST_DESCRIPTION");
            Hierarchy_Level_1 = DCC_GetColumn.String(dr, "HIERARCHY_LEVEL_1");///////////////////
            Hierarchy_Level_2 = DCC_GetColumn.String(dr, "HIERARCHY_LEVEL_2");
            Hierarchy_Level_3 = DCC_GetColumn.String(dr, "HIERARCHY_LEVEL_3");
            Hierarchy_Level_4 = DCC_GetColumn.String(dr, "HIERARCHY_LEVEL_4");
            Hierarchy_Level_5 = DCC_GetColumn.String(dr, "HIERARCHY_LEVEL_5");
            Post_Location_Description = DCC_GetColumn.String(dr, "Post_Location_Description");
            Post_Start_Date = DCC_GetColumn.DateTime(dr, "POST_START_DATE");
            Post_End_Date = DCC_GetColumn.DateTimeQ(dr, "POST_END_DATE");
            Post_Main_Flag = DCC_GetColumn.String(dr, "POST_MAIN_FLAG");
            Contract_Hours = DCC_GetColumn.DecimalQ(dr, "Contract_Hours");
            Position_Status = DCC_GetColumn.String(dr, "Position_Status");
            Post_Location_Address_1 = DCC_GetColumn.String(dr, "Post_Location_Address_1");
            Post_Location_Address_2 = DCC_GetColumn.String(dr, "Post_Location_Address_2");
            Post_Location_Address_3 = DCC_GetColumn.String(dr, "Post_Location_Address_3");
            Post_Location_Address_4 = DCC_GetColumn.String(dr, "Post_Location_Address_4");
            Post_Location_Post_Code = DCC_GetColumn.String(dr, "Post_Location_Post_Code");
            Post_Cost_Centre = DCC_GetColumn.String(dr, "Post_Cost_Centre");
            Post_Description = Regex.Replace(Post_Description, @"[^\u0000-\u007F]", string.Empty); // remove erroneous unicode characters
            Manager_Name = DCC_GetColumn.String(dr, "Manager_Name");
//            Manager_Post = DCC_GetColumn.String(dr, "Manager_Post");
            ManagerEmployeeNo = DCC_GetColumn.String(dr, "ManagerEmployeeNo");
            ManagerPostTitle = DCC_GetColumn.String(dr, "ManagerPostTitle");
        }

        public void ReadFromResourceLink(DCC_Session session, string post_No)
        {
            if (session.ResourceLinkDatabaseConnection != null)
            {
                DCC_Database db = session.ResourceLinkDatabaseConnection;
                DCC_DatabaseFrame di = new DCC_DatabaseFrame("pac_Resource_Link.Get_Post");
                di.AddTable("DCC_POST_HIERARCHY_TABLE");
                di.AddParameter("pPost_No", post_No, 10);
                di.AddParameter("o_cursor", DCC_DatabaseFrame.CURSOR);
                db.DataLayer.SelectData(di);
                if (di.hasTables) foreach (DataRow dr in di.TableZeroRows) Load(dr);
            }
        }

    }




    [Serializable]

    public class DCC_PostList : IEnumerable<DCC_Post>
    {

        // Properties

        public List<DCC_Post> All = new List<DCC_Post>();
        public int Count { get { return All.Count; } }

        public DCC_Post GetCurrentPost
        {
            get
            {
                if (Count == 0) return null;
                return All.Where(p => !p.Post_End_Date.HasValue).OrderByDescending(p => p.Post_End_Date).FirstOrDefault();
            }
        }


        // Indexer

        public DCC_Post this[int ordinalPosition]
        {
            get
            {
                if (Count == 0) return null;
                if (ordinalPosition < 0) return null;
                if (ordinalPosition >= Count) return null;
                return All[ordinalPosition];
            }
        }


        // ToString

        public override string ToString() { return "PostList (" + Count + " items)"; }



        // Constructors

        public DCC_PostList()
        {
        }



        // Enumerator

        public IEnumerator<DCC_Post> GetEnumerator() { foreach (object o in All) yield return (DCC_Post)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }



        // Methods

        public void ReadAllPostsFromResourceLink(DCC_Session session)
        {
            ReadAllPostsFromResourceLink(session, "");
        }

        public void ReadAllPostsFromResourceLink(DCC_Session session, string employeeNumber)
        {
            if (session.ResourceLinkDatabaseConnection != null)
            {
                DCC_Database db = session.ResourceLinkDatabaseConnection;
                DCC_DatabaseFrame dbf;
                if (employeeNumber == "")
                {
                    dbf = new DCC_DatabaseFrame("pac_Resource_Link.Get_Current_Posts");
                    dbf.AddTable("DCC_POST_HIERARCHY_TABLE");
                    dbf.AddParameter("o_cursor", DCC_DatabaseFrame.CURSOR);
                }
                else
                {
                    //dbf = new DCC_DatabaseFrame("pac_Resource_Link.Get_All_Employee_Posts");
                    //dbf.AddTable("DCC_POST_HIERARCHY_TABLE");
                    //dbf.AddParameter("pEmp_No", employeeNumber, 10);
                    //dbf.AddParameter("o_cursor", DCC_DatabaseFrame.CURSOR);
 
				    //ebs_dbf = new DCC_DatabaseFrame("**SELECT * from xxdcc_supplier_sites_v where supplier_site_id=:pSuppNo");
                    dbf = new DCC_DatabaseFrame("**select p.emp_no, p.post_no, p.post_description, p.hierarchy_level_1, p.hierarchy_level_2, p.hierarchy_level_3, p.hierarchy_level_4, p.hierarchy_level_5, p2.post_location_description, p2.post_start_date, p2.post_end_date, p2.post_main_flag, p2.contract_hours, p2.position_status, p2.post_location_address_1, p2.post_location_address_2, p2.post_location_address_3, p2.post_location_address_4, p2.post_location_post_code, p2.post_cost_centre, p3.manager_name, p3.manager_post, p4.emp_no as ManagerEmployeeNo, p4.post_description as ManagerPostTitle from DCC_Post_Hierarchy_Table p join DCC_Post_Holding_Table p2 on (p.emp_no = p2.emp_no) and (p.post_no = p2.post_no) join DCC_ActiveD_Table p3 on (p2.post_no = p3.post_no) join DCC_Post_Hierarchy_Table p4 on (p3.manager_post = p4.POST_NO) where p.emp_no=:pEmp_No order by p.post_start_date desc");
                    dbf.AddTable("DCC_POST_HIERARCHY_TABLE");
                    dbf.AddParameter("pEmp_No", employeeNumber, 10);

                }
                db.DataLayer.SelectData(dbf);
                if ((dbf.hasTables) && (dbf.TableZeroRows.Count > 0))
                {
                    foreach (DataRow dr in dbf.TableZeroRows) All.Add(new DCC_Post(dr));
                }
            }
        }

        public void ReadCurrentPosts(DCC_Session session)
        {
            ReadCurrentPostsFromResourceLink(session, "");
        }

        public void ReadCurrentPostsFromResourceLink(DCC_Session session, string employeeNumber)
        {
            if (session.ResourceLinkDatabaseConnection != null)
            {
                DCC_Database db = session.ResourceLinkDatabaseConnection;
                DCC_DatabaseFrame dbf;
                if (employeeNumber == "")
                {
                    dbf = new DCC_DatabaseFrame("pac_Resource_Link.Get_Current_Posts");
                    dbf.AddTable("DCC_POST_HIERARCHY_TABLE");
                    dbf.AddParameter("o_cursor", DCC_DatabaseFrame.CURSOR);
                }
                else
                {
                    //dbf = new DCC_DatabaseFrame("pac_Resource_Link.Get_Current_Employee_Posts");
                    dbf = new DCC_DatabaseFrame("**select p.emp_no, p.post_no, p.post_description, p.hierarchy_level_1, p.hierarchy_level_2, p.hierarchy_level_3, p.hierarchy_level_4, p.hierarchy_level_5, p2.post_location_description, p2.post_start_date, p2.post_end_date, p2.post_main_flag, p2.contract_hours, p2.position_status, p2.post_location_address_1, p2.post_location_address_2, p2.post_location_address_3, p2.post_location_address_4, p2.post_location_post_code, p2.post_cost_centre, p3.manager_name, p3.manager_post, p4.emp_no as ManagerEmployeeNo, p4.post_description as ManagerPostTitle from DCC_Post_Hierarchy_Table p left join DCC_Post_Holding_Table p2 on (p.EMP_NO = p2.EMP_NO) and (p.POST_NO = p2.POST_NO) left join DCC_ActiveD_Table p3 on (p2.POST_NO = p3.POST_NO) left join DCC_Post_Hierarchy_Table p4 on (p3.MANAGER_POST = p4.POST_NO) and (p4.POST_END_DATE is null) where p.EMP_NO=:pEmp_No and p2.POST_END_DATE is null and sysdate between p.POST_START_DATE and nvl(p.POST_END_DATE, sysdate+1) order by p.POST_START_DATE desc, p4.POST_START_DATE desc");
                    //dbf = new DCC_DatabaseFrame("**select p.emp_no, p.post_no, p.post_description, p.hierarchy_level_1, p.hierarchy_level_2, p.hierarchy_level_3, p.hierarchy_level_4, p.hierarchy_level_5, p2.post_location_description, p2.post_start_date, p2.post_end_date, p2.post_main_flag, p2.contract_hours, p2.position_status, p2.post_location_address_1, p2.post_location_address_2, p2.post_location_address_3, p2.post_location_address_4, p2.post_location_post_code, p2.post_cost_centre, p3.manager_name, p3.manager_post, p4.emp_no as ManagerEmployeeNo, p4.post_description as ManagerPostTitle from DCC_Post_Hierarchy_Table p left join DCC_Post_Holding_Table p2 on (p.EMP_NO = p2.EMP_NO) and (p.POST_NO = p2.POST_NO) left join DCC_ActiveD_Table p3 on (p2.POST_NO = p3.POST_NO) left join DCC_Post_Hierarchy_Table p4 on (p3.MANAGER_POST = p4.POST_NO) and (p4.POST_END_DATE is null) where p.EMP_NO=:pEmp_No and sysdate between p.POST_START_DATE and nvl(p.POST_END_DATE, sysdate+1) order by p.POST_START_DATE desc, p4.POST_START_DATE desc");
                    dbf.AddTable("DCC_POST_HIERARCHY_TABLE");
                    dbf.AddParameter("pEmp_No", employeeNumber, 10);
                }
                db.DataLayer.SelectData(dbf, pCacheLife: 0);
                if ((dbf.hasTables) && (dbf.TableZeroRows.Count > 0))
                {
                    foreach (DataRow dr in dbf.TableZeroRows)
                    {
                        DCC_Post p = new DCC_Post(dr);
                        if (p.Post_Start_Date <= DateTime.Now) All.Add(p);
                    }
                }
            }
        }

    }





    [Serializable]

    public class PostExpanded
    {

        // Properties

        public string Emp_No;
        public DateTime? Emp_Start_Date;
        public DateTime? Emp_End_Date;
        public string Surname;
        public string Forename;
        public string Other_Forenames;
        public string Known_As_Surname;
        public string Known_As_Forename;
        public DateTime Date_Of_Birth;
        public string NI_No;
        public string Address_Line_1;
        public string Address_Line_2;
        public string Address_Line_3;
        public string Address_Line_4;
        public string Address_Post_Code;
        public string Home_Tel_No;
        public string Gender;
        public string Work_Tel_No;
        public string Work_Ext_No;
        public string Work_Mobile_No;
        public string Work_Email_Address;
        public string Work_Fax_No;
        public string CRB_Category_Description;
        public string CRB_Reference;
        public string Post_No;
        public string Post_Main_Flag;
        public string Post_Description;
        public string Hierarchy_Level_1;/////////////////////
        public string Hierarchy_Level_2;
        public string Hierarchy_Level_3;
        public string Hierarchy_Level_4;
        public string Hierarchy_Level_5;
        public string Post_Location_Description;
        public DateTime Post_Start_Date;
        public DateTime Post_End_Date;
        public DCC_PostList Post_List;



        // Display Properties

        public string Emp_Start_DateAsString { get { return Emp_Start_Date.ToString(); } }
        public string Emp_End_DateAsString { get { return Emp_End_Date.ToString(); } }
        public string FullName { get { return (Known_As_Forename == "" ? Forename : Known_As_Forename) + " " + (Known_As_Surname == "" ? Surname : Known_As_Surname); } }



        // ToString

        public override string ToString() { return FullName + ", " + Emp_No; }



        // Constructors

        public PostExpanded()
        {
        }

        public PostExpanded(DataRow dr)
        {
            Emp_No = DCC_GetColumn.String(dr, "EMP_NO");
            Emp_Start_Date = DCC_GetColumn.DateTime(dr, "EMP_START_DATE");
            Emp_End_Date = DCC_GetColumn.DateTime(dr, "EMP_END_DATE");
            Surname = DCC_GetColumn.String(dr, "SURNAME");
            Forename = DCC_GetColumn.String(dr, "FORENAME");
            Other_Forenames = DCC_GetColumn.String(dr, "OTHER_FORENAMES");
            Known_As_Surname = DCC_GetColumn.String(dr, "KNOWN_AS_SURNAME");
            Known_As_Forename = DCC_GetColumn.String(dr, "KNOWN_AS_FORENAME");
            Date_Of_Birth = DCC_GetColumn.DateTime(dr, "DATE_OF_BIRTH");
            NI_No = DCC_GetColumn.String(dr, "NI_NO");
            Address_Line_1 = DCC_GetColumn.String(dr, "ADDRESS_LINE_1");
            Address_Line_2 = DCC_GetColumn.String(dr, "ADDRESS_LINE_2");
            Address_Line_3 = DCC_GetColumn.String(dr, "ADDRESS_LINE_3");
            Address_Line_4 = DCC_GetColumn.String(dr, "ADDRESS_LINE_4");
            Address_Post_Code = DCC_GetColumn.String(dr, "ADDRESS_POST_CODE");
            Home_Tel_No = DCC_GetColumn.String(dr, "HOME_TEL_NO");
            Gender = DCC_GetColumn.String(dr, "GENDER");
            Work_Tel_No = DCC_GetColumn.String(dr, "WORK_TEL_NO");
            Work_Ext_No = DCC_GetColumn.String(dr, "WORK_EXT_NO");
            Work_Mobile_No = DCC_GetColumn.String(dr, "WORK_MOBILE_NO");
            Work_Email_Address = DCC_GetColumn.String(dr, "WORK_EMAIL_ADDRESS");
            Work_Fax_No = DCC_GetColumn.String(dr, "WORK_FAX_NO");
            CRB_Category_Description = DCC_GetColumn.String(dr, "CRB_CATEGORY_DESCRIPTION");
            CRB_Reference = DCC_GetColumn.String(dr, "CRB_REFERENCE");

            try
            {
                Post_No = DCC_GetColumn.String(dr, "POST_NO");
                Post_Main_Flag = DCC_GetColumn.String(dr, "POST_MAIN_FLAG");
                Post_Description = DCC_GetColumn.String(dr, "POST_DESCRIPTION");
                Hierarchy_Level_1 = DCC_GetColumn.String(dr, "HIERARCHY_LEVEL_1");//////////////////////////
                Hierarchy_Level_2 = DCC_GetColumn.String(dr, "HIERARCHY_LEVEL_2");
                Hierarchy_Level_3 = DCC_GetColumn.String(dr, "HIERARCHY_LEVEL_3");
                Hierarchy_Level_4 = DCC_GetColumn.String(dr, "HIERARCHY_LEVEL_4");
                Hierarchy_Level_5 = DCC_GetColumn.String(dr, "HIERARCHY_LEVEL_5");
                Post_Location_Description = DCC_GetColumn.String(dr, "Post_Location_Description");
                Post_Start_Date = DCC_GetColumn.DateTime(dr, "POST_START_DATE");
                Post_End_Date = DCC_GetColumn.DateTime(dr, "POST_END_DATE");
            }
            catch
            {
            }
        }


    }





    [Serializable]

    public class PostExpandedList : IEnumerable<PostExpanded>
    {

        // Properties

        public List<PostExpanded> All = new List<PostExpanded>();
        public int Count { get { return All.Count; } }



        // ToString

        public override string ToString() { return "PostWithEmployeeDetailsList (" + Count + " items)"; }



        // Constructors

        public PostExpandedList()
        {
        }



        // Enumerator

        public IEnumerator<PostExpanded> GetEnumerator() { foreach (object o in All) yield return (PostExpanded)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }



        // Methods

        public void ReadCurrentPostsFromResourceLink(DCC_Session session)
        {
            if (session.ResourceLinkDatabaseConnection != null)
            {
                DCC_Database db = session.ResourceLinkDatabaseConnection;
                DCC_DatabaseFrame di = new DCC_DatabaseFrame("pac_Resource_Link.Get_Current_Posts_FULL");
                di.AddTable("DCC_POST_HIERARCHY_TABLE");
                di.AddTable("DCC_EMPLOYEE_TABLE");
                di.AddParameter("o_cursor", DCC_DatabaseFrame.CURSOR);
                db.DataLayer.SelectData(di);
                if (di.hasTables) foreach (DataRow dr in di.TableZeroRows) All.Add(new PostExpanded(dr));
            }
        }

    }

}
