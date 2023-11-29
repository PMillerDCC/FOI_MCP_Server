using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace DCC_Standard
{

	#region DCC_SystemAccess

	public class DCC_SystemAccess
	{

        // Database Fields

        int       k_SystemAccess = -1;
        int       q_RegisteredUser;
        string    softwareIdentity;      // 100 wide
        string    accessLevel;           // 100 wide
        string    authorisedBy;          // 100 wide
		DateTime? authorisedDateTime;
        string    revokedBy;             // 100 wide
		DateTime? revokedDateTime;
		DateTime? lastLoginDateTime;
        string    emailAddress;
        string    firstName;
        string    lastName;
		DateTime? registeredDateTime;
		bool      isLockedOut;



        // Get/Set Properties

        public int       K_SystemAccess         { get { return k_SystemAccess     ; } set { k_SystemAccess     = value; } }
        public int       Q_RegisteredUser       { get { return q_RegisteredUser   ; } set { q_RegisteredUser   = value; } }
		public string    SoftwareIdentity       { get { return softwareIdentity   ; } set { softwareIdentity   = value; } }
        public string    AccessLevel            { get { return accessLevel        ; } set { accessLevel        = value; } }
        public string    AuthorisedBy           { get { return authorisedBy       ; } set { authorisedBy       = value; } }
		public DateTime? AuthorisedDateTime     { get { return authorisedDateTime ; } set { authorisedDateTime = value; } }
        public string    RevokedBy              { get { return revokedBy          ; } set { revokedBy          = value; } }
		public DateTime? RevokedDateTime        { get { return revokedDateTime    ; } set { revokedDateTime    = value; } }
		public string    EmailAddress           { get { return emailAddress       ; } set { emailAddress       = value; } }
        public string    FirstName              { get { return firstName          ; } set { firstName          = value; } }
        public string    LastName               { get { return lastName           ; } set { lastName           = value; } }
		public DateTime? LastLoginDateTime      { get { return lastLoginDateTime  ; } set { lastLoginDateTime  = value; } }
		public DateTime? RegisteredDateTime     { get { return registeredDateTime ; } set { registeredDateTime = value; } }
		public bool      IsLockedOut            { get { return isLockedOut        ; } set { isLockedOut        = value; } }
		public string    OriginalAccessLevel    { get; set; }
		public string    URL                    { get; set; }
		public bool      UnlockAccount          { get; set; }



		// Display Properties

        public string    FullNameText           { get { return FirstName + " " + LastName; } }
        public string    FullNameSort           { get { return LastName + " " + FirstName; } }
		public string    AuthorisedDateTimeText { get { return DCC_Formatting.NullDateTxt(AuthorisedDateTime, "dd MMM yyyy"); } }
        public string    AuthorisedDateTimeSort { get { return DCC_Formatting.NullDateTxt(AuthorisedDateTime, "yyyyMMddHHmm");}}
		public string    RevokedDateTimeText    { get { return DCC_Formatting.NullDateTxt(RevokedDateTime, "dd MMM yyyy"); } }
        public string    RevokedDateTimeSort    { get { return DCC_Formatting.NullDateTxt(RevokedDateTime, "yyyyMMddHHmm");}}
		public string    LastLoginDateTimeText  { get { return DCC_Formatting.NullDateTxt(LastLoginDateTime, "dd MMM yyyy"); } }
        public string    LastLoginDateTimeSort  { get { return DCC_Formatting.NullDateTxt(LastLoginDateTime, "yyyyMMddHHmm");}}
        public string    AccessLevelText        { get { return AccessLevel.Replace("|"," | ");}}
		public string    RegisteredDateTimeText { get { return DCC_Formatting.NullDateTxt(RegisteredDateTime, "dd MMM yyyy"); } }
        public string    RegisteredDateTimeSort { get { return DCC_Formatting.NullDateTxt(RegisteredDateTime, "yyyyMMddHHmm");}}
        public string    IsLockedOutText        { get { return (IsLockedOut ? "!" : "");}}



		// Utilities

        public int             PrimaryKey       { get { return K_SystemAccess; } set { K_SystemAccess = value; } }  // Unique key suitable for creating SortedList
        public string          Key              { get { return K_SystemAccess.ToString(); } }
        public bool            IsNew            { get { return K_SystemAccess == -1; } }
        public override string ToString()       { return "PK: " + K_SystemAccess + "  FK: " + Q_RegisteredUser + "  " + SoftwareIdentity; }



        // Dynamic Checkboxes

        public DCC_SystemRoleList SystemRoles;



		// Constructors

        public DCC_SystemAccess()
        {
        }

		public DCC_SystemAccess(string pSoftwareIdentity = "", int pK_SystemAccess = -999, int pQ_RegisteredUser = -999, DCC_SystemRoleList pSystemRoleList = null, DCC_Database db = null)
		{
			k_SystemAccess = pK_SystemAccess;
			q_RegisteredUser = pQ_RegisteredUser;
			softwareIdentity = pSoftwareIdentity;
	
			if (K_SystemAccess > 0)
			{
				if (db == null) db = new DCC_Database("SQLSERVER", "DCC_Registration", softwareIdentity);
				DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sSystemAccess2");
				dbf.AddParameter("K_SystemAccess", K_SystemAccess);
				db.DataLayer.SelectData(dbf);
				if (dbf.hasTables && dbf.TableZeroRows.Count == 1)
				{
					GetColumns(dbf.RowZero);

					if (pSystemRoleList != null)
					{
						SystemRoles = new DCC_SystemRoleList();
						foreach (DCC_SystemRole role in pSystemRoleList) SystemRoles.Add(new DCC_SystemRole(role.Role, AccessLevel.Contains(role.Role)));
					}
				}
			}

			else if (q_RegisteredUser > 0)
			{
				if (!string.IsNullOrEmpty(softwareIdentity))
				{
					if (db == null) db = new DCC_Database("SQLSERVER", "DCC_Registration", softwareIdentity);
					DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sSystemAccessByRegisteredUser");
					dbf.AddParameter("q_RegisteredUser", q_RegisteredUser);
					dbf.AddParameter("softwareIdentity", softwareIdentity, 100);
					db.DataLayer.SelectData(dbf);
					if (dbf.hasTables)
					{
						if (dbf.TableZeroRows.Count > 0)
						{
							GetColumns(dbf.RowZero);
							if (dbf.TableZeroRows.Count > 1)
							{
								#region Email error to Developers
								string body;
								List<string> cc = new List<string>();
								cc.Add("graham.waters@durham.gov.uk");
								body =
									@"<html>
									<body>
									<h2>Error occurred with DCC_Registration!</h2>
									{1} ({2}) is in the SystemAccess table twice for {3}
									<br />
									<br />
									PLEASE DO NOT REPLY TO THIS EMAIL.
									</body>
									</html>";
								body = body.Replace("{1}", FullNameText);
								body = body.Replace("{2}", EmailAddress);
								body = body.Replace("{3}", SoftwareIdentity);
								DCC_Email.Send(null, "New System Access Request for " + SoftwareIdentity, body, cc, null, null, null);
								#endregion
							}
						}
					}
				}
			}

			OriginalAccessLevel = accessLevel;
		}

		public DCC_SystemAccess(DataRow dataRow)
		{
			GetColumns(dataRow);
		}



		// Methods

		public bool GetColumns(DataRow dataRow)
        {
            if (dataRow == null) return false;

            K_SystemAccess     = DCC_GetColumn.Integer32(dataRow, "K_SystemAccess");
            Q_RegisteredUser   = DCC_GetColumn.Integer32(dataRow, "Q_RegisteredUser");
            SoftwareIdentity   = DCC_GetColumn.String   (dataRow, "SoftwareIdentity");
            AccessLevel        = DCC_GetColumn.String   (dataRow, "AccessLevel");
            AuthorisedBy       = DCC_GetColumn.String   (dataRow, "AuthorisedBy");
            AuthorisedDateTime = DCC_GetColumn.DateTimeQ(dataRow, "AuthorisedDateTime");
            RevokedBy          = DCC_GetColumn.String   (dataRow, "RevokedBy");
            RevokedDateTime    = DCC_GetColumn.DateTimeQ(dataRow, "RevokedDateTime");
            LastLoginDateTime  = DCC_GetColumn.DateTimeQ(dataRow, "LastLoginDateTime");

            if (DCC_GetColumn.Exists(dataRow, "EmailAddress"))       EmailAddress       = DCC_GetColumn.String   (dataRow, "EmailAddress");
            if (DCC_GetColumn.Exists(dataRow, "FirstName"))          FirstName          = DCC_GetColumn.String   (dataRow, "FirstName");
            if (DCC_GetColumn.Exists(dataRow, "LastName"))           LastName           = DCC_GetColumn.String   (dataRow, "LastName");
            if (DCC_GetColumn.Exists(dataRow, "RegisteredDateTime")) RegisteredDateTime = DCC_GetColumn.DateTimeQ(dataRow, "RegisteredDateTime");
            if (DCC_GetColumn.Exists(dataRow, "RegisteredDateTime")) RegisteredDateTime = DCC_GetColumn.DateTimeQ(dataRow, "RegisteredDateTime");

            return true;
        }

        public int Insert(DCC_Database db = null)
        {
			if (string.IsNullOrEmpty(softwareIdentity)) return -1;

			if (db == null) db = new DCC_Database("SQLSERVER", "DCC_Registration", SoftwareIdentity);
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("iSystemAccess");
            dbf.AddTable("SystemAccess");
            dbf.AddParameter("Q_RegisteredUser", Q_RegisteredUser);
            dbf.AddParameter("SoftwareIdentity", SoftwareIdentity, 100);
            dbf.AddParameter("_PRIMARYKEY", DCC_DatabaseFrame.OUTINTEGER);
            int newKey = db.DataLayer.NonQueryPK(dbf);
            if (newKey > -1) PrimaryKey = newKey;
			k_SystemAccess = newKey;
            return newKey;
        }

        public int Update(DCC_Session session, string customerURL = "", string customAccessGrantedEmailBody = "", string customAccountUnlockedEmailBody = "")
        {
			if (string.IsNullOrEmpty(SoftwareIdentity)) return -1;

			string body;
			List<string> cc = new List<string>();
			cc.Add(EmailAddress);

			if (UnlockAccount)
			{
				DCC_RegisteredUser ru = new DCC_RegisteredUser(Q_RegisteredUser);
				ru.UpdateUnlockAccount();

				if (string.IsNullOrEmpty(customAccountUnlockedEmailBody))
				{
					body =
						@"<html>
						<body>
						<h2>{1}</h2>
						<br />
						Your account to the above website has been unlocked and your password reset to <strong>Password.19</strong>
						<br />
						<br />
						<a href='{3}'>Click here</a> to access the system.
						<br />
						<br />
						PLEASE DO NOT REPLY TO THIS EMAIL.
						</body>
						</html>";
					body = body.Replace("{1}", customerURL);
					body = body.Replace("{2}", SoftwareIdentity);
					body = body.Replace("{3}", customerURL);
				}
				else
				{
					body = customAccountUnlockedEmailBody;
				}

				DCC_Email.Send(null, "DCC System Access Request - " + SoftwareIdentity, body, cc, null, null, null);
			}
			else
			{
				if ( (!OriginalAccessLevel.Contains("NO ACCESS")) && (AccessLevel.Contains("NO ACCESS")) )
				{
					RevokedBy = session.FullName;
					RevokedDateTime = DateTime.Now;
				}
				else if ( (OriginalAccessLevel.Contains("NO ACCESS")) && (!AccessLevel.Contains("NO ACCESS")) )
				{
					AuthorisedBy = session.FullName;
					AuthorisedDateTime = DateTime.Now;
					RevokedBy = null;
					RevokedDateTime = null;

					if (string.IsNullOrEmpty(customerURL))
					{
						customerURL = session.URL;
					}

					if (string.IsNullOrEmpty(customAccessGrantedEmailBody))
					{
						body =
							@"<html>
							<body>
							<h2>Durham County Council System Access Request</h2>
							<br />
							You have been granted access to {1}
							<br />
							<br />
							<a href='{2}'>Click here</a> to access the system.  You will not need to register again.
							<br />
							<br />
							PLEASE DO NOT REPLY TO THIS EMAIL.
							</body>
							</html>";
						body = body.Replace("{1}", SoftwareIdentity);
						body = body.Replace("{2}", customerURL);
					}
					else
					{
						body = customAccessGrantedEmailBody;
					}

					DCC_Email.Send(null, "DCC System Access Request - " + SoftwareIdentity, body, cc, null, null, null);
				}
			}

			DCC_Database db = new DCC_Database("SQLSERVER", "DCC_Registration", softwareIdentity);
			DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uSystemAccess");
            dbf.AddTable("SystemAccess");
            dbf.AddParameter("K_SystemAccess"    , K_SystemAccess);
            dbf.AddParameter("AccessLevel"       , AccessLevel        , 100);
            dbf.AddParameter("AuthorisedBy"      , AuthorisedBy       , 100);
            dbf.AddParameter("AuthorisedDateTime", AuthorisedDateTime);
            dbf.AddParameter("RevokedBy"         , RevokedBy          , 100);
            dbf.AddParameter("RevokedDateTime"   , RevokedDateTime);
            db.DataLayer.NonQuery(dbf);
            return (dbf.OK ? PrimaryKey : -1);
        }

		public bool Delete(DCC_Database db)
		{
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uSystemAccessRemoveRequest");
            dbf.AddTable("SystemAccess");
            dbf.AddParameter("k_SystemAccess", k_SystemAccess);
            db.DataLayer.NonQuery(dbf);
            return (dbf.OK);
		}

        public int UpdateLastLoginDateTime()
        {
			if (string.IsNullOrEmpty(SoftwareIdentity)) return -1;

			DCC_Database db = new DCC_Database("SQLSERVER", "DCC_Registration", softwareIdentity);
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uSystemAccessLastLoginDateTime");
            dbf.AddTable("SystemAccess");
            dbf.AddParameter("k_SystemAccess", k_SystemAccess);
            db.DataLayer.NonQuery(dbf);
            return (dbf.OK ? PrimaryKey : -1);
        }

	}

	#endregion


    #region DCC_SystemAccessList

    public class DCC_SystemAccessList : IEnumerable<DCC_SystemAccess>
    {


        // Database Fields

        public List<object> All = new List<object>();
        public List<object> SystemAdministrators = new List<object>();
        public SortedList<string, List<object>> DynamicSortBy = new SortedList<string, List<object>>();



        // Utilities

        public int Count { get { return All.Count; } }
        public override string ToString() { return "DCC_SystemAccessList has " + Count + " items"; }



        // Constructors

        public DCC_SystemAccessList(string pSoftwareIdentity, string pAccessLevel = null)
        {
            if (!string.IsNullOrEmpty(pSoftwareIdentity))
            {
				DCC_Database db = new DCC_Database("SQLSERVER", "DCC_Registration", pSoftwareIdentity);
                DCC_DatabaseFrame dbf;
				
				if (pAccessLevel == null)
				{
					dbf = new DCC_DatabaseFrame("sSystemAccessList");
					dbf.AddTable("SystemAccess");
					dbf.AddParameter("SoftwareIdentity", pSoftwareIdentity, 100);
					db.DataLayer.SelectData(dbf);
					if (dbf.hasTables)
					{
						foreach (DataRow dataRow in dbf.TableZeroRows) Add(new DCC_SystemAccess(dataRow));
					}
				}

				else
				{
					dbf = new DCC_DatabaseFrame("sSystemAccessListByAccessLevel");
					dbf.AddTable("SystemAccess");
					dbf.AddParameter("SoftwareIdentity", pSoftwareIdentity, 100);
					dbf.AddParameter("AccessLevel",      pAccessLevel, 100);
					db.DataLayer.SelectData(dbf);
					if (dbf.hasTables)
					{
						foreach (DataRow dataRow in dbf.TableZeroRows) Add(new DCC_SystemAccess(dataRow));
					}
				}

            }
        }



        // Methods

        public bool Add(DCC_SystemAccess item)
        {
            All.Add(item);
			if (item.AccessLevel.Contains("System Administrator")) SystemAdministrators.Add(item);
            return true;
        }



        // Enumerator

        public IEnumerator<DCC_SystemAccess> GetEnumerator() { foreach (object o in All) yield return (DCC_SystemAccess)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }

    }

    #endregion


    #region DCC_SystemAccessGrid

    public class DCC_SystemAccessGrid : DCC_Grid
    {
		string _SoftwareIdentity;

        public DCC_SystemAccessGrid(string pSoftwareIdentity)
        {
			_SoftwareIdentity = pSoftwareIdentity;
			InlineDelete = DeleteRows;

			if (!string.IsNullOrEmpty(pSoftwareIdentity))
			{
				DCC_SystemAccessList items = new DCC_SystemAccessList(pSoftwareIdentity);
				TotalRows = items.All;
				Count = CountFiltered = TotalRows.Count;
			}
        }

        public string DeleteRows(List<int> primeKeys)
        {
            if (primeKeys == null || primeKeys.Count == 0) return "You must select a row to delete.";

            bool ok = true;
            foreach (int primeKey in primeKeys)
            {
                DCC_SystemAccess bp = new DCC_SystemAccess(pK_SystemAccess: primeKey);
				if ( (bp.AccessLevel != "NO ACCESS") && (bp.AuthorisedBy == null) )
					ok = false;
            }
			if (!ok)
			{
				return "You can only delete registration requests that have not been approved.";
			}

            ok = true;
			DCC_Database db = new DCC_Database("SQLSERVER", "DCC_Registration", _SoftwareIdentity);
            db.DataLayer.TransactionBegin();
            foreach (int primeKey in primeKeys)
            {
                DCC_SystemAccess bp = new DCC_SystemAccess(pK_SystemAccess: primeKey);
                ok &= bp.Delete(db);
            }
			db.DataLayer.TransactionEnd(ok);

            if (!ok)
                return "Failed to update database";
            else
            {
				DCC_SystemAccessList items = new DCC_SystemAccessList(_SoftwareIdentity);
				TotalRows = items.All;
				Count = CountFiltered = TotalRows.Count;
                return "";
            }
        }

    }

    #endregion


}
