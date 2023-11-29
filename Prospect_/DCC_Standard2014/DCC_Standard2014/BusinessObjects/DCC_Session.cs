using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;

namespace DCC_Standard
{

    #region DCC_Session Class

    /// <summary>
    /// public class containing session & user data
    /// </summary>
    public class DCC_Session
    {


        // Properties

        public string SoftwareIdentity = null;
        public string ApplicationTitle = null;
        public string UnderMaintenance = null;

        public bool IsLocal;
        public string Host = "-not set-";
		public string URL = "-not set-";
        public string IPAddress = "-not set-";
		public string AuditNo = "-not set-";
        public bool HasJavascript;
        public List<string> DatabaseConnections = new List<string>();
        public DCC_Database AutoDatabaseConnection = null;
        public DCC_Database ResourceLinkDatabaseConnection = null;
		public string DebugTrace = "-not set-";

        public string LogonID;
        public string FullName;
        public string GivenName;
        public string LastName;
        public string Initials;
        public string PostTitle;
        public string PersonReference;  // unique key for Resource Link - not stored in AD so not populated by default
        public string PostNo;           // postno for main post in Resource Link - not stored in AD so not populated by default
        public string EmployeeNumber;
        public string EmailAddress;
        public string TelephoneNumber;
        public string Company;
        public string Service;
        public string Division;
        public string Location;
        public string HomeDrive;
        public string HomeDirectory;
        public string UserAccountControl;
        public DateTime LastLogon;
        public List<string> GroupMemberships;
        public string OU;
        public bool IsAuthorised;
        public bool IsDeveloper;
        public bool IsTest;

        public bool IsExternalUser;
		public int K_SystemUser;
		public int K_RegisteredUser;
        public string AccessLevel;

        public DCC_SystemRoleList SystemRoles = new DCC_SystemRoleList();
        public string LogonInfo;
        public string NavigationOptions;
        public DCC_SystemMessageList SystemMessages = new DCC_SystemMessageList();



        // Constructor

        public DCC_Session()
        {
            LogonID =
            FullName =
            GivenName =
            LastName =
            Initials =
            PostTitle =
            PersonReference =
            PostNo =
            EmployeeNumber =
            EmailAddress =
            TelephoneNumber =
            Company =
            Service =
            Division =
            Location =
            HomeDrive =
            HomeDirectory =
            UserAccountControl =
            OU =
            AccessLevel = "-not set-";
            IsAuthorised =
            IsDeveloper =
            IsTest =
            IsExternalUser = false;
        }



        // Methods

        public void Add(string tag, object value)
        {
            if (HttpContext.Current.Session != null) HttpContext.Current.Session[tag] = value;
            return;
        }

        public void Remove(string tag)
        {
            if (HttpContext.Current.Session != null) HttpContext.Current.Session.Remove(tag);
            return;
        }

        public void Save(string tag, object value)
        {
			HttpContext.Current.Session[tag] = value;
            return;
        }

        public object Get(string tag)
        {
            return HttpContext.Current.Session[tag];
        }

        public void AssignRoles()
        {
            string[] mvcRoles = new string[]{};
            if (IsDeveloper) mvcRoles = new string[]{ "Anonymous", "Developer" };
            else             mvcRoles = new string[]{ "Anonymous" };
            mvcRoles = (AccessLevel + ((IsDeveloper) ? "|Developer" : "")).Split('|');

            GenericPrincipal gp = new GenericPrincipal(new GenericIdentity(LogonID), mvcRoles);
            HttpContext.Current.User = gp;
        }

		public bool IsInRole(string role)
        {
            DCC_SystemRole sr = SystemRoles.All.Find(x => x.Role == role);
            if (sr == null) return false;
            return sr.IsChecked;
        }

        public bool SetInRole(string role, bool inRole)
        {
            DCC_SystemRole sr = SystemRoles.All.Find(x => x.Role == role);
            if (sr == null) return false;
            sr.IsChecked = inRole;
            return true;
        }

    }

    #endregion

}
