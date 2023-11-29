using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace DCC_Standard
{


    [Serializable]
    public class DCC_Employee
    {

        // Properties

        public string    Emp_No;
        public DateTime? Emp_Start_Date;
        public DateTime? Emp_End_Date;
        public string    Surname;
        public string    Forename;
        public string    Other_Forenames;
        public string    Known_As_Surname;
        public string    Known_As_Forename;
        public DateTime  Date_Of_Birth;
        public string    NI_No;
        public string    Address_Line_1;
        public string    Address_Line_2;
        public string    Address_Line_3;
        public string    Address_Line_4;
        public string    Address_Post_Code;
        public string    Home_Tel_No;
        public string    Gender;
        public string    Work_Tel_No;
        public string    Work_Mobile_No;
        public string    Work_Email_Address;
        public string    Work_Fax_No;
        public string    CRB_Category_Description;
        public string    CRB_Reference;

        public DCC_PostList PostList = new DCC_PostList();



        // Display Properties

        public string Emp_Start_DateAsString { get { return Emp_Start_Date.ToString(); } }
        public string Emp_End_DateAsString   { get { return Emp_End_Date.ToString(); } }
        public string FullName               { get { return (Known_As_Forename == "" ? Forename : Known_As_Forename) + " " + (Known_As_Surname == "" ? Surname : Known_As_Surname); } }



        // Utilities

        public override string ToString() { return Emp_No + " " + FullName; }



        // Constructors

        public DCC_Employee()
        {
        }

        public DCC_Employee(DataRow dr)
        {
            Load(dr);
        }

        public DCC_Employee(DCC_Session session, string employeeNumber, bool includePosts = false)
        {
            if (session.ResourceLinkDatabaseConnection != null)
            {
                DCC_Database db = session.ResourceLinkDatabaseConnection;
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**select * from dcc_employee_table where emp_no=:pEmp_No");
                dbf.AddParameter("pEmp_No", employeeNumber, 10);
                db.DataLayer.SelectData(dbf);
                if (dbf.hasTables)
                {
                    foreach (DataRow dr in dbf.TableZeroRows)
                    {
                        Load(dr);
                        if (includePosts) PostList.ReadCurrentPostsFromResourceLink(session, Emp_No);
                    }
                }
            }
        }



        // Methods

        private void Load(DataRow dr)
        {
            Emp_No                   = DCC_GetColumn.String   (dr, "EMP_NO");// * NEEDED *
            Emp_Start_Date           = DCC_GetColumn.DateTime (dr, "EMP_START_DATE");
            Emp_End_Date             = DCC_GetColumn.DateTime (dr, "EMP_END_DATE");
            Surname                  = DCC_GetColumn.String   (dr, "SURNAME");// * NEEDED *
            Forename                 = DCC_GetColumn.String   (dr, "FORENAME");// * NEEDED *
            Other_Forenames          = DCC_GetColumn.String   (dr, "OTHER_FORENAMES");// * NEEDED *
            Known_As_Surname         = DCC_GetColumn.String   (dr, "KNOWN_AS_SURNAME");// * NEEDED *
            Known_As_Forename        = DCC_GetColumn.String   (dr, "KNOWN_AS_FORENAME");// * NEEDED *
            Date_Of_Birth            = DCC_GetColumn.DateTime (dr, "DATE_OF_BIRTH");
            NI_No                    = DCC_GetColumn.String   (dr, "NI_NO");
            Address_Line_1           = DCC_GetColumn.String   (dr, "ADDRESS_LINE_1");
            Address_Line_2           = DCC_GetColumn.String   (dr, "ADDRESS_LINE_2");
            Address_Line_3           = DCC_GetColumn.String   (dr, "ADDRESS_LINE_3");
            Address_Line_4           = DCC_GetColumn.String   (dr, "ADDRESS_LINE_4");
            Address_Post_Code        = DCC_GetColumn.String   (dr, "ADDRESS_POST_CODE");
            Home_Tel_No              = DCC_GetColumn.String   (dr, "HOME_TEL_NO");
            Gender                   = DCC_GetColumn.String   (dr, "GENDER");
            Work_Tel_No              = DCC_GetColumn.String   (dr, "WORK_TEL_NO");
            Work_Mobile_No           = DCC_GetColumn.String   (dr, "WORK_MOBILE_NO");
            Work_Email_Address       = DCC_GetColumn.String   (dr, "WORK_EMAIL_ADDRESS");// * NEEDED *
            Work_Fax_No              = DCC_GetColumn.String   (dr, "WORK_FAX_NO");
            CRB_Category_Description = DCC_GetColumn.String   (dr, "CRB_CATEGORY_DESCRIPTION");
            CRB_Reference            = DCC_GetColumn.String   (dr, "CRB_REFERENCE");

            PostList = new DCC_PostList();
        }

        public bool IsEmployeeNoInResourceLink(DCC_Session session, string pEmployeeNo)
        {
            DCC_Database db = new DCC_Database("ORACLE", "SALP", session.ApplicationTitle);
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**select * from dcc_employee_table where emp_no=:pEmployeeNo");
            dbf.AddParameter("pEmployeeNo", pEmployeeNo, 10);
            db.DataLayer.SelectData(dbf);
            if (dbf.hasTables)
            {
                foreach (DataRow dr in dbf.TableZeroRows) Load(dr);
            }
            return (Emp_No == pEmployeeNo);
        }

    }



    [Serializable]
    public class DCC_EmployeeList : IEnumerable<DCC_Employee>
    {

        // Properties

        public List<DCC_Employee> All = new List<DCC_Employee>();



        // Utilities

        public int Count { get { return All.Count; } }
        public override string ToString() { return "EmployeeList (" + Count + " items)"; }



        // Constructors

        public DCC_EmployeeList()
        {
        }

        public DCC_EmployeeList(DCC_Session session, bool pCurrentOnly = true)
        {
            if (session.ResourceLinkDatabaseConnection != null)
            {
                DCC_Database db = session.ResourceLinkDatabaseConnection;
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**select e.emp_no, e.emp_start_date, e.emp_end_date, e.surname, e.forename, e.other_forenames, e.known_as_forename, e.known_as_surname, e.date_of_birth, e.ni_no, e.address_line_1, e.address_line_2, e.address_line_3, e.address_line_4, e.address_post_code, e.home_tel_no, e.gender, e.work_email_address, e.work_tel_no, e.work_ext_no, e.work_mobile_no, e.work_fax_no, e.crb_reference, e.crb_category_description from DCC_EMPLOYEE_TABLE e where e.emp_end_date is null order by e.emp_no");
                dbf.AddTable("DCC_EMPLOYEE_TABLE");
                db.DataLayer.SelectData(dbf);
                if (dbf.DataSelected != null) if (dbf.hasTables) foreach (DataRow dr in dbf.TableZeroRows) All.Add(new DCC_Employee(dr));
            }
        }



        // Enumerator

        public IEnumerator<DCC_Employee> GetEnumerator() { foreach (object o in All) yield return (DCC_Employee)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }

    }


}
