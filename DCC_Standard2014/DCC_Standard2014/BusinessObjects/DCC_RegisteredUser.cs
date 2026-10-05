using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Web;

namespace DCC_Standard
{

	#region DCC_RegisteredUser

	[Serializable]
	public class DCC_RegisteredUser
	{


        // Database Fields

        int       k_RegisteredUser = -1;
        string    emailAddress;        // 100 wide
        string    firstName;           // 100 wide
        string    lastName;            // 100 wide
        string    password;            // 100 wide
        string    memorableWord;       // 100 wide
		int       securityQuestion1;
		string    securityAnswer1;     // 100 wide
		int       securityQuestion2;
		string    securityAnswer2;     // 100 wide
		DateTime? registeredDateTime;
		DateTime? lastLoginDateTime;
		DateTime? lockedOutDateTime;
		bool      isLockedOut;
		int       noFailedLogins;



        // Get/Set Properties

        public int       K_RegisteredUser       { get { return k_RegisteredUser  ; } set { k_RegisteredUser  = value; } }
		[Display(Name = "Email Address")]
		[Required(ErrorMessage = "You must enter a valid Email Address")]
		[EmailAddress(ErrorMessage = "Invalid Email Address")]
		public string    EmailAddress           { get { return emailAddress       ; } set { emailAddress       = value; } }
        public string    FirstName              { get { return firstName          ; } set { firstName          = value; } }
        public string    LastName               { get { return lastName           ; } set { lastName           = value; } }
        public string    Password               { get { return password           ; } set { password           = value; } }
        public string    MemorableWord          { get { return memorableWord      ; } set { memorableWord      = value; } }
        public int       SecurityQuestion1      { get { return securityQuestion1  ; } set { securityQuestion1  = value; } }
        public string    SecurityAnswer1        { get { return securityAnswer1    ; } set { securityAnswer1    = value; } }
        public int       SecurityQuestion2      { get { return securityQuestion2  ; } set { securityQuestion2  = value; } }
        public string    SecurityAnswer2        { get { return securityAnswer2    ; } set { securityAnswer2    = value; } }
		public DateTime? RegisteredDateTime     { get { return registeredDateTime ; } set { registeredDateTime = value; } }
		public DateTime? LastLoginDateTime      { get { return lastLoginDateTime  ; } set { lastLoginDateTime  = value; } }
		public DateTime? LockedOutDateTime      { get { return lockedOutDateTime  ; } set { lockedOutDateTime  = value; } }
		public bool      IsLockedOut            { get { return isLockedOut        ; } set { isLockedOut        = value; } }
        public int       NoFailedLogins         { get { return noFailedLogins     ; } set { noFailedLogins     = value; } }
        public string    EnteredPassword        { get; set; }
        public string    EnteredSecurityAnswer1 { get; set; }
        public string    EnteredSecurityAnswer2 { get; set; }
        public string    NewPassword1           { get; set; }
        public string    NewPassword2           { get; set; }
        public string    SoftwareIdentity       { get; set; }
        public bool      AllowUserRegistration  { get; set; }
		public bool      IsAuthenticated        { get; set; }
        public bool      IsAuthorised           { get; set; }
		public bool      IsRegisteredForDCC     { get; set; }
		public bool      IsRegisteredForSystem  { get; set; }
		public bool      IsRevoked              { get; set; }
		public bool      IsFinished             { get; set; }
        public string    ErrorMessage           { get; set; }
		public string    SecurityQuestion1Text  { get; set; }
		public string    SecurityQuestion2Text  { get; set; }
		public int       RandomNumber1          { get; set; }
		public int       RandomNumber2          { get; set; }
		


		// Display Properties

		public string FullNameText           { get { return FirstName + " " + LastName; } }
		public string FullNameSort           { get { return LastName + " " + FirstName; } }
		public string RegisteredDateTimeText { get { return DCC_Formatting.NullDateTxt(registeredDateTime, "dd MMM yyyy"); } }
		public string RegisteredDateTimeSort { get { return DCC_Formatting.NullDateTxt(registeredDateTime, "yyyyMMddHHmm"); } }
		public string LastLoginDateTimeText  { get { return DCC_Formatting.NullDateTxt(lastLoginDateTime, "dd MMM yyyy"); } }
		public string LastLoginDateTimeSort  { get { return DCC_Formatting.NullDateTxt(lastLoginDateTime, "yyyyMMddHHmm"); } }
		public string LockedOutDateTimeText  { get { return DCC_Formatting.NullDateTxt(lockedOutDateTime, "dd MMM yyyy"); } }
		public string LockedOutDateTimeSort  { get { return DCC_Formatting.NullDateTxt(lockedOutDateTime, "yyyyMMddHHmm"); } }



        // Utilities

        public int             PrimaryKey { get { return K_RegisteredUser; } set { K_RegisteredUser = value; } }
        public string          Key        { get { return PrimaryKey.ToString(); } }
        public bool            IsNew      { get { return PrimaryKey == -1; } }
        public override string ToString() { return FullNameText + ", " + PrimaryKey.ToString().PadLeft(3); }



		// Constructors

		public DCC_RegisteredUser()
		{
			IsAuthenticated = false;
			IsAuthorised = false;
			AllowUserRegistration = true;
		}

		public DCC_RegisteredUser(int pK_RegisteredUser, string pSoftwareIdentity = "")
		{
			if (!string.IsNullOrEmpty(pSoftwareIdentity)) SoftwareIdentity = pSoftwareIdentity;

			IsAuthenticated = false;
			IsAuthorised = false;
			AllowUserRegistration = true;

			DCC_Database db = new DCC_Database("SQLSERVER", "DCC_Registration", SoftwareIdentity);
			DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sRegisteredUserByPrimaryKey");
			dbf.AddTable("RegisteredUser");
			dbf.AddParameter("k_RegisteredUser", pK_RegisteredUser);
			db.DataLayer.SelectData(dbf);
			if (dbf.hasTables && dbf.TableZeroRows.Count == 1) GetColumns(dbf.RowZero);
		}

		public DCC_RegisteredUser(string pEmailAddress = "", string pSoftwareIdentity = "")
        {
			IsAuthenticated = false;
			IsAuthorised = false;
			AllowUserRegistration = true;

			EmailAddress = pEmailAddress;
			SoftwareIdentity = pSoftwareIdentity;

			if (EmailAddress == "") return;

			if ( (!string.IsNullOrEmpty(EmailAddress)) && (!string.IsNullOrEmpty(SoftwareIdentity)) )
			{
				DCC_Database db = new DCC_Database("SQLSERVER", "DCC_Registration", SoftwareIdentity);
				DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sRegisteredUserByEmailAddress");
				dbf.AddTable("RegisteredUser");
				dbf.AddParameter("EmailAddress", pEmailAddress, 100);
				db.DataLayer.SelectData(dbf);
				if (dbf.hasTables && dbf.TableZeroRows.Count == 1) GetColumns(dbf.RowZero);
			}
        }



		// Methods

        public bool GetColumns(DataRow dataRow)
        {
            if (dataRow == null) return false;

			K_RegisteredUser   = DCC_GetColumn.Integer32 (dataRow, "K_RegisteredUser");
			EmailAddress       = DCC_GetColumn.String    (dataRow, "EmailAddress");
			FirstName          = DCC_GetColumn.String    (dataRow, "FirstName");
			LastName           = DCC_GetColumn.String    (dataRow, "LastName");
			Password           = DCC_GetColumn.String    (dataRow, "Password");
//			MemorableWord      = DCC_GetColumn.String    (dataRow, "MemorableWord");
			SecurityQuestion1  = DCC_GetColumn.Integer32 (dataRow, "SecurityQuestion1");
			SecurityAnswer1    = DCC_GetColumn.String    (dataRow, "SecurityAnswer1");
			SecurityQuestion2  = DCC_GetColumn.Integer32 (dataRow, "SecurityQuestion2");
			SecurityAnswer2    = DCC_GetColumn.String    (dataRow, "SecurityAnswer2");
			RegisteredDateTime = DCC_GetColumn.DateTimeQ (dataRow, "RegisteredDateTime");
			LastLoginDateTime  = DCC_GetColumn.DateTimeQ (dataRow, "LastLoginDateTime");
			LockedOutDateTime  = DCC_GetColumn.DateTimeQ (dataRow, "LockedOutDateTime");
			IsLockedOut        = DCC_GetColumn.Bit       (dataRow, "IsLockedOut");
			NoFailedLogins     = DCC_GetColumn.Integer32 (dataRow, "NoFailedLogins");

            return true;
        }

		public void IsRegistered()
		{
			if ((!string.IsNullOrEmpty(EmailAddress)) && (!string.IsNullOrEmpty(SoftwareIdentity)))
			{
				IsAuthenticated = true;

				DCC_Database db = new DCC_Database("SQLSERVER", "DCC_Registration", SoftwareIdentity);
				if (db.DataLayer.IsOpen)
				{
					DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sRegisteredUser");
					dbf.AddTable("RegisteredUser");
					dbf.AddParameter("EmailAddress", EmailAddress, 100);
					db.DataLayer.SelectData(dbf);
					if (dbf.hasTables && dbf.TableZeroRows.Count == 1)
					{
						GetColumns(dbf.RowZero);
						IsRegisteredForDCC = true;

						DCC_SystemAccess systemAccess = new DCC_SystemAccess(SoftwareIdentity, pQ_RegisteredUser: K_RegisteredUser, db: db);
						if (systemAccess.K_SystemAccess > 0)
						{
							if (systemAccess.AccessLevel == null)
							{
								IsRegisteredForSystem = true;
							}
							else if (systemAccess.AccessLevel.Contains("NO ACCESS"))
							{
								IsRegisteredForSystem = true;
								if (systemAccess.RevokedDateTime != null) IsRevoked = true;
							}
							else
							{
								IsRegisteredForSystem = true;
								IsRevoked = false;
							}
						}
					}
				}
			}
		}

        public bool Authenticate()
        {
			IsAuthenticated = false;

			if ((!string.IsNullOrEmpty(EmailAddress)) && (!string.IsNullOrEmpty(SoftwareIdentity)))
			{
				DebugTrace.WriteLine();
				DebugTrace.WriteLine("Login A:  Attempted authentication by " + EmailAddress);
				
				DCC_Database db = new DCC_Database("SQLSERVER", "DCC_Registration", SoftwareIdentity);
				DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sRegisteredUser");
				dbf.AddTable("RegisteredUser");
				dbf.AddParameter("EmailAddress", EmailAddress, 100);
				db.DataLayer.SelectData(dbf);
				if (dbf.hasTables && dbf.TableZeroRows.Count > 0) GetColumns(dbf.RowZero);

				if (K_RegisteredUser > 0)
				{
					if (IsLockedOut)
					{
						DebugTrace.WriteLine();
						DebugTrace.WriteLine("Login A:  User is locked out");
						DebugTrace.WriteLine();
						DebugTrace.WriteLine("GWW1 key            " + K_RegisteredUser);
						DebugTrace.WriteLine("GWW1 email:         " + EmailAddress);
						DebugTrace.WriteLine("GWW1 failed logins: " + NoFailedLogins.ToString());
						DebugTrace.WriteLine("GWW1 islockedout:   " + IsLockedOut.ToString());
						DebugTrace.WriteLine("GWW1 datetime:      " + LockedOutDateTime.ToString());
					}
					else
					{
						if (password == EnteredPassword) // && {memorableWord.substr(?,1) == EnteredLetter1} && {memorableWord.substr(?,1) == EnteredLetter2}
						{
							IsAuthenticated = true;
							UpdateSuccessfulLogin(db);
							DebugTrace.WriteLine();
							DebugTrace.WriteLine("Login A:  User is authenticated!");
						}
						else
						{
							ErrorMessage = "2";
							noFailedLogins++;
							if (noFailedLogins >= 3)
							{
								noFailedLogins = 3;
								isLockedOut = true;
								lockedOutDateTime = DateTime.Now;
							}
							UpdateFailedLogin(db);
							DebugTrace.WriteLine();
							DebugTrace.WriteLine("Login A:  User entered an incorrect password");
							DebugTrace.WriteLine();
							DebugTrace.WriteLine("GWW2 key            " + K_RegisteredUser);
							DebugTrace.WriteLine("GWW2 pw:            " + Password);
							DebugTrace.WriteLine("GWW2 enteredpw:     " + EnteredPassword);
							DebugTrace.WriteLine("GWW2 email:         " + EmailAddress);
							DebugTrace.WriteLine("GWW2 failed logins: " + NoFailedLogins.ToString());
							DebugTrace.WriteLine("GWW2 islockedout:   " + IsLockedOut.ToString());
							DebugTrace.WriteLine("GWW2 datetime:      " + LockedOutDateTime.ToString());
						}
					}
				}
				else
				{
					DebugTrace.WriteLine();
					DebugTrace.WriteLine("Login A:  User is not registered");
					ErrorMessage = "1";
				}
			}

			return IsAuthenticated;
        }

        public bool Authorise()
        {
			DebugTrace.WriteLine();
			DebugTrace.WriteLine("Login B:  Attempted authorisation by " + EmailAddress);
			IsAuthorised = false;

			if ((K_RegisteredUser > 0) && (!string.IsNullOrEmpty(SoftwareIdentity)))
			{
				DCC_SystemAccess systemAccess = new DCC_SystemAccess(pSoftwareIdentity: SoftwareIdentity, pQ_RegisteredUser: K_RegisteredUser);
				if ( (systemAccess.AccessLevel != null) && (!systemAccess.AccessLevel.Contains("NO ACCESS")) )
				{
					IsAuthorised = true;
					systemAccess.UpdateLastLoginDateTime();
					HttpContext.Current.Session["ExternalUserEmailAddress"] = EmailAddress;
					HttpContext.Current.Session["ExternalUserK_RegisteredUser"] = systemAccess.Q_RegisteredUser;
					HttpContext.Current.Session["ExternalUserFirstName"] = FirstName;
					HttpContext.Current.Session["ExternalUserLastName"] = LastName;
					HttpContext.Current.Session["ExternalUserFullName"] = FirstName + " " + LastName;
					HttpContext.Current.Session["ExternalUserAccessLevel"] = systemAccess.AccessLevel;
					DebugTrace.WriteLine();
					DebugTrace.WriteLine("Login B:  User is authorised!");
				}
				else
				{
					ErrorMessage = "1";
					DebugTrace.WriteLine();
					DebugTrace.WriteLine("Login B:  User either does not have access to this system or has had their access revoked");
				}
			}

			return IsAuthorised;
        }

		public bool ValidatePassword()
		{
			if (string.IsNullOrEmpty(NewPassword1))	return false;

			bool isValidLength       = NewPassword1.Length >= 8 && NewPassword1.Length <= 20;
			bool hasUpperCaseLetter  = false;
			bool hasLowerCaseLetter  = false;
			bool hasDecimalDigit     = false;
            bool hasSpecialCharacter = false;

			if (isValidLength)
			{
				foreach (char c in NewPassword1)
				{
					if      (char.IsUpper(c)) hasUpperCaseLetter = true;
					else if (char.IsLower(c)) hasLowerCaseLetter = true;
					else if (char.IsDigit(c)) hasDecimalDigit    = true;
                    else if (@"!#$%&'()*+,-./:;<=>?@[\]^_`{|}~".Contains(c.ToString())) hasSpecialCharacter = true; // Remember to change the guidance for this on the webpage if you change these special characters
				}
			}

			return (isValidLength && hasUpperCaseLetter && hasLowerCaseLetter && hasDecimalDigit && hasSpecialCharacter);
		}

		public string GetMemorableWordCharacter()
		{
			string character = "";


			return character;
		}

		public bool Insert()
		{
			bool successful = false;

			if (!string.IsNullOrEmpty(SoftwareIdentity))
			{
				DCC_Database db = new DCC_Database("SQLSERVER", "DCC_Registration", SoftwareIdentity);
				DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("iRegisteredUser");
				dbf.AddTable("RegisteredUser");
				dbf.AddParameter("EmailAddress"     , EmailAddress     , 100);
				dbf.AddParameter("FirstName"        , FirstName        , 100);
				dbf.AddParameter("LastName"         , LastName         , 100);
				dbf.AddParameter("Password"         , NewPassword1     , 100);
				//dbf.AddParameter("MemorableWord", MemorableWord, 100);
				dbf.AddParameter("SecurityQuestion1", SecurityQuestion1);
				dbf.AddParameter("SecurityAnswer1"  , SecurityAnswer1  , 100);
				dbf.AddParameter("SecurityQuestion2", SecurityQuestion2);
				dbf.AddParameter("SecurityAnswer2"  , SecurityAnswer2  , 100);
				dbf.AddParameter("_PRIMARYKEY", DCC_DatabaseFrame.OUTINTEGER);
				int newKey = db.DataLayer.NonQueryPK(dbf);
				if (newKey > -1) PrimaryKey = newKey;
				int a = newKey;
				K_RegisteredUser = newKey;

				DCC_SystemAccess systemAccess = new DCC_SystemAccess();
				systemAccess.Q_RegisteredUser = K_RegisteredUser;
				systemAccess.SoftwareIdentity = SoftwareIdentity;
				int b = systemAccess.Insert();

				if ( (a > 0) && (b > 0) ) successful = true;
				else ErrorMessage = "1";
			}

			return successful;
		}

		public bool UpdateSecurityInformation()
		{
			if (!string.IsNullOrEmpty(SoftwareIdentity)) return false;

			bool successful = false;
			DCC_Database db = new DCC_Database("SQLSERVER", "DCC_Registration", SoftwareIdentity);
			DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uRegisteredUserSecurityInformation");
			dbf.AddTable("RegisteredUser");
			dbf.AddParameter("K_RegisteredUser", K_RegisteredUser);
			dbf.AddParameter("FirstName", FirstName, 100);
			dbf.AddParameter("LastName", LastName, 100);
//			dbf.AddParameter("MemorableWord", MemorableWord, 100);
			dbf.AddParameter("SecurityQuestion1", SecurityQuestion1);
			dbf.AddParameter("SecurityAnswer1", SecurityAnswer1, 100);
			dbf.AddParameter("SecurityQuestion2", SecurityQuestion2);
			dbf.AddParameter("SecurityAnswer2", SecurityAnswer2, 100);
	        db.DataLayer.NonQuery(dbf);
			successful = dbf.OK;
			if (!successful) ErrorMessage = "1";
			return successful;
		}

        public bool UpdatePassword()
        {
			if ((k_RegisteredUser <= 0) && (!string.IsNullOrEmpty(SoftwareIdentity))) return false;

			bool successful = false;
	        DCC_Database db = new DCC_Database("SQLSERVER", "DCC_Registration", SoftwareIdentity);
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uRegisteredUserPassword");
            dbf.AddTable("RegisteredUser");
            dbf.AddParameter("K_RegisteredUser", K_RegisteredUser);
            dbf.AddParameter("Password", NewPassword1, 100);
            db.DataLayer.NonQuery(dbf);
			successful = dbf.OK;
			if (!successful) ErrorMessage = "1";
			return successful;
        }

		public int UpdateSuccessfulLogin(DCC_Database db = null)
		{
			if (db == null) db = new DCC_Database("SQLSERVER", "DCC_Registration", SoftwareIdentity);
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uRegisteredUserSuccessfulLogin");
            dbf.AddTable("RegisteredUser");
            dbf.AddParameter("K_RegisteredUser", K_RegisteredUser);
            db.DataLayer.NonQuery(dbf);
            return (dbf.OK ? 1 : -1);
		}

		int UpdateFailedLogin(DCC_Database db = null)
		{
			if (db == null) db = new DCC_Database("SQLSERVER", "DCC_Registration", SoftwareIdentity);
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uRegisteredUserFailedLogin");
            dbf.AddTable("RegisteredUser");
            dbf.AddParameter("K_RegisteredUser", K_RegisteredUser);
            dbf.AddParameter("LockedOutDateTime", LockedOutDateTime);
            dbf.AddParameter("IsLockedOut", IsLockedOut);
            dbf.AddParameter("NoFailedLogins", NoFailedLogins);
            db.DataLayer.NonQuery(dbf);
            return (dbf.OK ? 1 : -1);
		}

		public int UpdateUnlockAccount(DCC_Database db = null)
		{
			if (db == null) db = new DCC_Database("SQLSERVER", "DCC_Registration", SoftwareIdentity);
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uRegisteredUserUnlockAccount");
            dbf.AddTable("RegisteredUser");
            dbf.AddParameter("K_RegisteredUser", K_RegisteredUser);
            db.DataLayer.NonQuery(dbf);
            return (dbf.OK ? 1 : -1);
		}

		public string EmailSystemAdministrators(string url, string overrideEmailAddress = null)
		{
			string body;
			List<string> cc = new List<string>();

			if (overrideEmailAddress == null)
			{
				DCC_SystemAccessList salist = new DCC_SystemAccessList(SoftwareIdentity);
				foreach (DCC_SystemAccess sa in salist.SystemAdministrators) cc.Add(sa.EmailAddress);
			}
			else
				cc.Add(overrideEmailAddress);

			body =
				@"<html>
				<body>
				<h2>New System Access Request</h2>
				{1} ({2}) has requested access to {3}
				<br />
				<br />
				<a href='{4}'>Click here</a> to access the system.
				<br />
				<br />
				PLEASE DO NOT REPLY TO THIS EMAIL.
				</body>
				</html>";
			body = body.Replace("{1}", FullNameText);
			body = body.Replace("{2}", EmailAddress);
			body = body.Replace("{3}", SoftwareIdentity );
			body = body.Replace("{4}", url.Replace("http:","https:"));  // quick shoddy fix - for some reason some links were being sent with http instead of https

			return DCC_Email.Send(null, "New System Access Request for " + SoftwareIdentity, body, cc, null, null, null);
		}

	}

	#endregion

}
