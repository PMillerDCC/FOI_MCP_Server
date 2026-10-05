using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace DCC_Standard
{


    [Serializable]
    public class DCC_Leaver
    {

        // Database Fields

        public string ITBM_Key;
        public string Employee_Number;
        public DateTime? Date_Made_Leaver;
        public DateTime? Date_Left;
        public string Surname;
        public string First_Forename;
        public string Login_Name;
        public string Email_Address;
        public string Job_Title;
        public string Team_Name;
        public string Location;



        // Properties

		public string Date_Made_LeaverText { get { return DCC_Formatting.NullDateTxt(Date_Made_Leaver, "dd MMM"); } }
		public string Date_LeftText        { get { return DCC_Formatting.NullDateTxt(Date_Left, "dd MMM"); } }
        public string FullName             { get { return First_Forename + " " + Surname; } }



        // ToString

        public override string ToString() { return Employee_Number; }



        // Constructors

        public DCC_Leaver()
        {
        }

        public DCC_Leaver(DataRow dr)
        {
            Load(dr);
        }



        // Methods

        private void Load(DataRow dr)
        {
            ITBM_Key         = DCC_GetColumn.String  (dr, "ITBM_Key");
            Employee_Number  = DCC_GetColumn.String  (dr, "Employee_Number");
            Date_Made_Leaver = DCC_GetColumn.DateTime(dr, "Date_Made_Leaver");
            Date_Left        = DCC_GetColumn.DateTime(dr, "Date_Left");
            Surname          = DCC_GetColumn.String  (dr, "Surname");
            First_Forename   = DCC_GetColumn.String  (dr, "First_Forename");
            Login_Name       = DCC_GetColumn.String  (dr, "Login_Name");
            Email_Address    = DCC_GetColumn.String  (dr, "Email_Address");
            Job_Title        = DCC_GetColumn.String  (dr, "Job_Title");
            Team_Name        = DCC_GetColumn.String  (dr, "Team_Name");
            Location         = DCC_GetColumn.String  (dr, "Location");
        }

    }



    [Serializable]
    public class DCC_LeaverList : IEnumerable<DCC_Leaver>
    {

        // Properties

        public List<DCC_Leaver> All = new List<DCC_Leaver>();
        public int Count { get { return All.Count; } }



        // ToString

        public override string ToString() { return "LeaverList (" + Count + " items)"; }



        // Constructors

        public DCC_LeaverList()
        {
        }

        public DCC_LeaverList(DCC_Session session)
        {
            //if (session.DatabaseConnections.Contains("SALP"))
            //{
            //    DCC_Database db = new DCC_Database("ORACLE", "SALP", session.ApplicationTitle);

            if (session.ResourceLinkDatabaseConnection != null)
            {
                DCC_Database db = session.ResourceLinkDatabaseConnection;
                DCC_DatabaseFrame di = new DCC_DatabaseFrame("**select * from DCC_V_ITBM_LEAVERS");
                di.AddTable("DCC_V_ITBM_LEAVERS");
                di.AddParameter("o_cursor", DCC_DatabaseFrame.CURSOR);
                db.DataLayer.SelectData(di);
                if (di.hasTables) foreach (DataRow dr in di.TableZeroRows) All.Add(new DCC_Leaver(dr));
            }
        }



        // Enumerator

        public IEnumerator<DCC_Leaver> GetEnumerator() { foreach (object o in All) yield return (DCC_Leaver)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }

    }


}
