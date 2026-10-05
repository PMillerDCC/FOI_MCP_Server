using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace DCC_Standard
{

    #region DCC_SystemUser

    [Serializable]
    public class DCC_SystemUser
    {

        // Database Fields

        int    k_SystemUser = -1;
        string logonID;             // 40 wide
        string accessLevel;         // 500 wide - was 60 - WILL NEED TO CHANGE THIS IN THE TABLE THE NEXT TIME YOU PUBLISH
        string systemUserFirstName; // 40 wide given name
        string systemUserLastName;  // 40 wide



        // Get/Set Properties

        public int    K_SystemUser        { get { return k_SystemUser        ; } set { k_SystemUser        = value; } }
        public string LogonID             { get { return logonID             ; } set { logonID             = value; } }
        public string AccessLevel         { get { return accessLevel         ; } set { accessLevel         = value; } }
        public string SystemUserFirstName { get { return systemUserFirstName ; } set { systemUserFirstName = value; } }
        public string SystemUserLastName  { get { return systemUserLastName  ; } set { systemUserLastName  = value; } }



        // Display Properties

        public string AccessLevelText     { get { return accessLevel.Replace("|"," | ");}}
        public string SystemUserFullName  { get { return systemUserFirstName + " " + systemUserLastName;}}



        // Utilities

        public int PrimaryKey             { get { return K_SystemUser; } set { K_SystemUser = value; } }
        public string Key                 { get { return PrimaryKey.ToString(); } }
        public bool IsNew                 { get { return PrimaryKey == -1; } }
        public override string ToString() { return "Key: " + Key + "  " + LogonID + "  " + AccessLevelText; }



        // Dynamic Checkboxes

        public DCC_SystemRoleList SystemRoles;



        // Constructors

        public DCC_SystemUser()
        {
        }

        public DCC_SystemUser(DataRow dataRow)
        {
            GetColumns(dataRow);
        }

        public DCC_SystemUser(DCC_Session session, int k_SystemUser = -999, string logonID = "", DCC_SystemRoleList pSystemRoleList = null)
        {

            if (k_SystemUser == -1) // Ask the constructor to create a new instance of SystemUser
            {
                // Set default values for new records
                AccessLevel = "NO ACCESS";

				Load(session, pSystemRoleList);
            }

            else if (k_SystemUser > 0) // Ask the constructor to read a record from the database for the given K_SystemUser
            {
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sSystemUser");
                dbf.AddTable("SystemUser");
                dbf.AddParameter("K_SystemUser", k_SystemUser);
                session.AutoDatabaseConnection.DataLayer.SelectData(dbf);
                if (dbf.hasTables && dbf.TableZeroRows.Count == 1) GetColumns(dbf.RowZero);

				Load(session, pSystemRoleList);
            }

            else if (logonID != null) // Ask the constructor to read a record from the database for the given LogonID
            {
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sSystemUserByLogonID");
                dbf.AddTable("SystemUser");
                dbf.AddParameter("LogonID", logonID, 40);
                session.AutoDatabaseConnection.DataLayer.SelectData(dbf);
                if (dbf.hasTables && dbf.TableZeroRows.Count == 1) GetColumns(dbf.RowZero);
            }

        }



        // Methods

        public bool GetColumns(DataRow dataRow)
        {
            if (dataRow == null) return false;

            k_SystemUser        = DCC_GetColumn.Integer32(dataRow, "k_SystemUser");
            logonID             = DCC_GetColumn.String   (dataRow, "logonID");
            accessLevel         = DCC_GetColumn.String   (dataRow, "accessLevel");
            systemUserFirstName = DCC_GetColumn.String   (dataRow, "systemUserFirstName");
            systemUserLastName  = DCC_GetColumn.String   (dataRow, "systemUserLastName");

            return true;
        }

        public int Save(DCC_Session dcc_Session)
        {
            return ( IsNew ? Insert(dcc_Session) : Update(dcc_Session) );
        }

        int Insert(DCC_Session dcc_Session)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("iSystemUser");
            dbf.AddTable("SystemUser");
            SetCommonParameters(dbf);
            dbf.AddParameter("_PRIMARYKEY", DCC_DatabaseFrame.OUTINTEGER);
            int newKey = dcc_Session.AutoDatabaseConnection.DataLayer.NonQueryPK(dbf);
            if (newKey > -1) PrimaryKey = newKey;
            return newKey;
        }

        int Update(DCC_Session dcc_Session)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uSystemUser");
            dbf.AddTable("SystemUser");
            dbf.AddParameter("K_SystemUser", PrimaryKey);
            SetCommonParameters(dbf);
            dcc_Session.AutoDatabaseConnection.DataLayer.NonQuery(dbf);
            return (dbf.OK ? PrimaryKey : -1);
        }

        public bool Delete(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("dSystemUser");
            dbf.AddTable("SystemUser");
            dbf.AddParameter("K_SystemUser", PrimaryKey);
            db.DataLayer.NonQuery(dbf);
            return dbf.OK;
        }

        private void SetCommonParameters(DCC_DatabaseFrame dbf)
        {
            dbf.AddParameter("logonID"            , logonID            , 40);
            dbf.AddParameter("accessLevel"        , accessLevel        , 500);
            dbf.AddParameter("systemUserFirstName", systemUserFirstName, 40);
            dbf.AddParameter("systemUserLastName" , systemUserLastName , 40);
        }

		public void Load(DCC_Session session, DCC_SystemRoleList pSystemRoleList = null)
		{
			if (pSystemRoleList == null)
			{
				SystemRoles = new DCC_SystemRoleList();
	            foreach (DCC_SystemRole role in session.SystemRoles) SystemRoles.Add(new DCC_SystemRole(role.Role, AccessLevel.Contains(role.Role)));
			}
			else
			{
				SystemRoles = new DCC_SystemRoleList();
	            foreach (DCC_SystemRole role in pSystemRoleList)
				{
					SystemRoles.Add(new DCC_SystemRole(role.Role, AccessLevel.Contains(role.Role)));
				}
			}
		}

        public bool SendEmail(string sender, string subject, string body, List<string> attachments = null)
        {
            ActiveDirectory.ActiveDirectoryAccount account = ActiveDirectory.GetAccount(LogonID);
            if ( string.IsNullOrEmpty(account.EmailAddress) ||
                 string.IsNullOrEmpty(subject) ||
                 string.IsNullOrEmpty(body) ) return false;
            List<string> cc = new List<string>();
            cc.Add(account.EmailAddress);
            return (DCC_Email.Send(sender, subject, body, cc, null, null, attachments) == "OK");
        }

    }

    #endregion


    #region DCC_SystemUserList

    [Serializable]
    public class DCC_SystemUserList : IEnumerable<DCC_SystemUser>
    {

        // Properties

        public List<object> All = new List<object>();
        public SortedList<string, List<object>> DynamicSortBy = new SortedList<string, List<object>>();



        // Utilities

        public int Count { get { return All.Count; } }
        public override string ToString() { return "SystemUserList has " + Count + " items"; }



        // Constructors

        public DCC_SystemUserList()
        {
        }

        public DCC_SystemUserList(DCC_Database db, string role = "")
        {
            DCC_DatabaseFrame dbf;
            if (role == "")
                dbf = new DCC_DatabaseFrame("sSystemUserList");
            else
            {
                dbf = new DCC_DatabaseFrame("sSystemUserListByRole");
                dbf.AddParameter("role", role, 500);
            }
            dbf.AddTable("SystemUser");
            db.DataLayer.SelectData(dbf);
            if (dbf.hasTables)
            {
                foreach (DataRow dr in dbf.TableZeroRows) Add(new DCC_SystemUser(dr));
            }
        }



        // Methods

        public bool Add(DCC_SystemUser item)
        {
            All.Add(item);
            return true;
        }



        // Enumerator

        public IEnumerator<DCC_SystemUser> GetEnumerator() { foreach (object o in All) yield return (DCC_SystemUser)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }

    }

    #endregion


    #region DCC_SystemUserGrid

    public class DCC_SystemUserGrid : DCC_Grid
    {

        public DCC_SystemUserGrid(DCC_Session session)
        {
            DCC_SystemUserList systemUserList = new DCC_SystemUserList(session.AutoDatabaseConnection);
            TotalRows = systemUserList.All;
            Count = CountFiltered = TotalRows.Count;
        }

    }

    #endregion

}
