using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace DCC_Standard
{

    /// <summary>
    /// This class contains the following:
    ///
    /// CLASSES
    /// ActiveDirectoryAccount - A class containing the LDAP properties of an AD account.
    /// ActiveDirectoryAccountList - A class containing a List of ActiveDirectoryAccount objects.
    /// LDAP_Properties - A class containing the LDAP properties used within ActiveDirectoryUtilities.  Enables each LDAP property to be referenced by a plain English expression.
    ///
    /// VALIDATE METHODS
    /// DoesAccountExist – Returns true if an AD account exists.
    /// IsAccountLocked – Returns true if an AD account is locked.
    /// IsAccountDisabled – Returns true if an AD account is disabled.
    /// IsAccountExpired – Returns true if an AD account has expired.
    /// ValidatePassword – Returns true if the password of an AD account is valid.
    ///
    /// AD ACCOUNT METHODS
    /// GetAccount - Searches for an AD account using a given LDAP property & value.  Returns the ActiveDirectoryAccount object - which is blank if not found.
    /// GetAccounts – Searches for all AD accounts that match the given query.  Returns an ActiveDirectoryAccountList object - which is empty if none are found.
    /// UnlockAccount – Unlocks an locked AD account.
    /// DisableAccount – Disables an AD account.
    /// EnableAccount – Enables an disabled AD account.
    /// ExpirePassword – Expires an AD account's Password.
    /// ResetPassword – Resets an AD account's password.
    /// CreateAccount – Creates a new AD account.  Returns true if successful.
    /// DeleteAccount – Deletes an AD account!  Returns true if successful.
    /// SetProperty - Sets an LDAP property of an AD account.  Returns true if successful, false if the account doesn't exist.
    ///
    /// AD GROUP METHODS
    /// GetGroups - Returns a List<string> object containing a list of all AD groups.
    /// GetGroupMembers - Returns a ActiveDirectoryAccountList object containing all the AD accounts in an AD group.
    /// GetGroupMemberships – Returns a List<string> object containing a list of all AD groups that an AD account is a member of.
    /// IsGroupMember – Returns true if an AD account is a member of an AD group.
    /// AddToGroup – Adds an AD account to an AD group.  Returns true if successful.
    /// RemoveFromGroup – Removes an AD account from an AD group.  Returns true if successful.
    /// CreateGroup – Creates a new AD group.
    ///
    /// OU METHODS
    /// Get_OU - Returns a string containing the full OU path of a AD account.  Returns "" if the account doesn't exist.
    /// Get_OUs - Returns a List<string> object containing a list of all AD OU paths.
    /// Get_OU_Members - Returns a list<string> object containing all AD accounts in an OU.
    /// MoveAccount – Moves an AD account to a different OU.  Returns true if successful, false if the account doesn't exist.
    ///
    /// </summary>

    public class ActiveDirectory
    {


        #region Variables
        public static string domain = "durham.gov.uk";
        //public static string ou = "OU=users,DC=durham,DC=gov,DC=uk";
        public static string ou = "DC=durham,DC=gov,DC=uk";
        public static string id = "durham.gov.uk\\ResourcelinkLDAP@durham.gov.uk";
        public static string password = "Br0adbandPr0vider";
        public static PrincipalContext principalContext = new PrincipalContext(ContextType.Domain, domain, ou, id, password);
        #endregion



        #region ActiveDirectoryAccount Class

        /// <summary>
        /// A class containing the LDAP properties of an AD account.
        /// </summary>
        public class ActiveDirectoryAccount
        {

            // Properties

            public string LogonID;
            public string GivenName;
            public string LastName;
            public string FullName;
            public string DisplayName;
            public string Initials;
            public string Title;
            public string EmployeeNumber;
            public string EmailAddress;
            public string TelephoneNumber;
            public string OtherTelephoneNumber;
            public string MobileNumber;
            public string Company;
            public string Department;
            public string Division;
            public string HomeDrive;
            public string HomeDirectory;
            public string Manager;
            public string DistinguishedName;
            public string Location;
            public string UserAccountControl;
            public DateTime LastLogon;
            //public string OriginalOU;
            public string Info;
            public bool Disabled;
            public bool Enabled;
            public List<string> GroupMemberships;
            public string OU;

            public string OtherHomePhone;
            public string CountryNotation;
            public string City;
            public string State;
            public string PostalCode;
            public string Pager;
            public string Fax;
            public string HomePhone;
            public string OtherLocation;
            public string Extension;
            public string CostCentre;





            // ToString

            /// <summary>
            /// ToString() method with a meaningful description.
            /// </summary>
            /// <returns></returns>
            public override string ToString() { return LogonID; }


            // Constructors

            /// <summary>
            /// Empty constructor.
            /// </summary>
            public ActiveDirectoryAccount()
            {
            }

            /// <summary>
            /// Constructor from a DirectoryEntry object.
            /// </summary>
            /// <param name="searchResult">The SearchResult object</param>
            public ActiveDirectoryAccount(SearchResult searchResult)
            {
                DirectoryEntry directoryEntry = searchResult.GetDirectoryEntry();

                LogonID = GetProperty(directoryEntry, LDAP_Properties.LogonID);
                GivenName = GetProperty(directoryEntry, LDAP_Properties.GivenName);
                LastName = GetProperty(directoryEntry, LDAP_Properties.LastName);
                FullName = GetProperty(directoryEntry, LDAP_Properties.FullName);
                DisplayName = GetProperty(directoryEntry, LDAP_Properties.DisplayName);
                Initials = GetProperty(directoryEntry, LDAP_Properties.Initials);
                Title = GetProperty(directoryEntry, LDAP_Properties.Title);
                EmployeeNumber = GetProperty(directoryEntry, LDAP_Properties.EmployeeNumber);
                EmailAddress = GetProperty(directoryEntry, LDAP_Properties.EmailAddress);
                TelephoneNumber = GetProperty(directoryEntry, LDAP_Properties.TelephoneNumber);
                OtherTelephoneNumber = GetProperty(directoryEntry, LDAP_Properties.OtherTelephoneNumber);
                MobileNumber = GetProperty(directoryEntry, LDAP_Properties.MobileNumber);
                Company = GetProperty(directoryEntry, LDAP_Properties.Company);
                Department = GetProperty(directoryEntry, LDAP_Properties.Department);
                Division = GetProperty(directoryEntry, LDAP_Properties.Division);
                HomeDrive = GetProperty(directoryEntry, LDAP_Properties.HomeDrive);
                HomeDirectory = GetProperty(directoryEntry, LDAP_Properties.HomeDirectory);
                Location = GetProperty(directoryEntry, LDAP_Properties.Location);
                UserAccountControl = GetProperty(directoryEntry, LDAP_Properties.UserAccountControl);
                //OriginalOU = GetProperty(directoryEntry, LDAP_Properties.OriginalOU);
                Info = GetProperty(directoryEntry, LDAP_Properties.Info);
                OU = Get_OU(LogonID);

                string manager = GetProperty(directoryEntry, LDAP_Properties.Manager);
                if (!String.IsNullOrEmpty(manager))
                {
                    String[] managerArray = manager.Split(',');
                    Manager = managerArray[0].Replace("CN=", "");
                }

                DistinguishedName = GetProperty(directoryEntry, LDAP_Properties.DistinguishedName);
                LastLogon = ((searchResult.Properties.Contains("lastLogon"))
                    ? DateTime.FromFileTime((long)searchResult.Properties["lastLogon"][0])
                    : DateTime.MinValue);

                try
                {
                    int flags = (int)directoryEntry.Properties["userAccountControl"].Value;
                    Enabled = (!Convert.ToBoolean(flags & 0x0002));
                    Disabled = !Enabled;
                }
                catch
                {
                }
                finally
                {
                }



                #region Attributes with little or no information in...
                OtherHomePhone  = GetProperty(directoryEntry, LDAP_Properties.OtherHomePhone );
                CountryNotation = GetProperty(directoryEntry, LDAP_Properties.CountryNotation);
                City            = GetProperty(directoryEntry, LDAP_Properties.City           );
                State           = GetProperty(directoryEntry, LDAP_Properties.State          );
                PostalCode      = GetProperty(directoryEntry, LDAP_Properties.PostalCode     );
                Pager           = GetProperty(directoryEntry, LDAP_Properties.Pager          );
                Fax             = GetProperty(directoryEntry, LDAP_Properties.Fax            );
                HomePhone       = GetProperty(directoryEntry, LDAP_Properties.HomePhone      );
                OtherLocation   = GetProperty(directoryEntry, LDAP_Properties.OtherLocation  );
                Extension       = GetProperty(directoryEntry, LDAP_Properties.Extension      );
                CostCentre      = GetProperty(directoryEntry, LDAP_Properties.CostCentre     );
                #endregion




                GroupMemberships = ActiveDirectory.GetGroupMemberships(LogonID);
            }

            public bool IsInGroup(string pGroup)
            {
                bool rc = false;
                if (GroupMemberships != null)
                {
                    foreach (string group in GroupMemberships)
                    {
                        if (group.ToLower() == pGroup.ToLower())
                        {
                            rc = true;
                            break;
                        }
                    }
                }

                return rc;
            }


        }

        #endregion

        #region ActiveDirectoryAccountList Class

        /// <summary>
        /// A class containing a List of ActiveDirectoryAccount objects.
        /// </summary>
        public class ActiveDirectoryAccountList : IEnumerable<ActiveDirectoryAccount>
        {

            // Properties

            public List<ActiveDirectoryAccount> All = new List<ActiveDirectoryAccount>();
            public int Count { get { return All.Count; } }


            // ToString

            public override string ToString() { return "ActiveDirectoryAccountList (" + Count + " items)"; }


            // Constructors

            /// <summary>
            /// Constructor for the ActiveDirectoryAccountList class.
            /// </summary>
            public ActiveDirectoryAccountList()
            {
            }


            // Enumerator

            public IEnumerator<ActiveDirectoryAccount> GetEnumerator() { foreach (object o in All) yield return (ActiveDirectoryAccount)o; }
            IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }

        }

        #endregion

        #region LDAP_Properties Class

        /// <summary>
        /// A class containing the LDAP properties used within ActiveDirectoryUtilities.  Enables each LDAP property to be referenced by a plain English expression.
        /// </summary>
        public static class LDAP_Properties
        {
            public const string LogonID = "sAMAccountName";
            public const string GivenName = "givenName";
            public const string MiddleName = "middleName";
            public const string LastName = "sn";
            public const string FullName = "cn";
            public const string DisplayName = "displayName";
            public const string Initials = "initials";
            public const string Title = "title";
            public const string EmployeeNumber = "employeeID";
            public const string EmailAddress = "mail";
            public const string TelephoneNumber = "telephoneNumber";
            public const string OtherTelephoneNumber = "otherTelephone";
            public const string MobileNumber = "mobile";
            public const string Company = "company";
            public const string Department = "department";
            public const string Division = "division";
            public const string HomeDrive = "homeDrive";
            public const string HomeDirectory = "homeDirectory";
            public const string Location = "physicalDeliveryOfficeName";
            public const string UserAccountControl = "userAccountControl";
            public const string Manager = "manager";
            public const string DistinguishedName = "distinguishedName";
            public const string LastLogon = "lastLogonTimeStamp";
            //public const string OriginalOU = "url";  // now uses "info" below
            public const string Info = "info";
            public const string CostCentre = "carLicense";

            #region Attributes with little or no information in...
            public const string OtherHomePhone = "otherHomePhone";
            public const string CountryNotation = "c";
            public const string City = "l";
            public const string State = "st";
            public const string PostalCode = "postalCode";
            public const string Pager = "pager";
            public const string Fax = "facsimileTelephoneNumber";
            public const string HomePhone = "homePhone";
            public const string OtherLocation = "location";
            public const string Extension = "ipPhone";
            //public const string LastLogoff = "lastLogoff";   NOT POPULATED BY DEFAULT
            #endregion

            #region Attributes yet to try... or find a reason to...
            //public const string ObjectClass = "objectClass";
            //public const string InstanceType = "instanceType";
            //public const string WhenCreated = "whenCreated";
            //public const string WhenChanged = "whenChanged";
            //public const string USNCreated = "uSNCreated";
            //public const string MemberOf = "memberOf";
            //public const string USNChanged = "uSNChanged";
            //public const string ProxyAddresses = "proxyAddresses";
            //public const string StreetAddress = "streetAddress";
            //public const string DirectReports = "directReports";
            //public const string Name = "name";
            //public const string ObjectGUID = "objectGUID";
            //public const string BadPasswordCount = "badPwdCount";
            //public const string CodePage = "codePage";
            //public const string CountryCode = "countryCode";
            //public const string BadPasswordTime = "badPasswordTime";
            //public const string LastPasswordSet = "pwdLastSet";
            //public const string PrimaryGroupID = "primaryGroupID";
            //public const string ObjectsID = "objectSid";
            //public const string AdminCount = "adminCount";
            //public const string AccountExpires = "accountExpires";
            //public const string LogonCount = "logonCount";
            //public const string samAccountType = "sAMAccountType";
            //public const string ShowInAddressBook = "showInAddressBook";
            //public const string LegacyExchangeDN = "legacyExchangeDN";
            //public const string UserPrincipalName = "userPrincipalName";
            //public const string ServicePrincipalName = "servicePrincipalName";
            //public const string ObjectCategory = "objectCategory";
            //public const string DSCorePropagationData = "dSCorePropagationData";
            //public const string MSExchUserAccountControl = "msExchUserAccountControl";
            //public const string MDBUseDefaults = "mDBUseDefaults";
            //public const string MSExchmailBoxSecurityDescriptor = "msExchMailboxSecurityDescriptor";
            //public const string HomeMDB = "homeMDB";
            //public const string MSExchpoliciesIncluded = "msExchPoliciesIncluded";
            //public const string HomeMTA = "homeMTA";
            //public const string MSExchRecipientTypeDetails = "msExchRecipientTypeDetails";
            //public const string MailNickname = "mailNickname";
            //public const string MSExchHomeServerName = "msExchHomeServerName";
            //public const string MSExchVersion = "msExchVersion";
            //public const string MSExchRecipientDisplayType = "msExchRecipientDisplayType";
            //public const string MSExchMailBoxGUID = "msExchMailboxGuid";
            //public const string NTSecurityDescriptor = "nTSecurityDescriptor";
            #endregion

        }

        /// <summary>
        /// Returns a string containing the value of an LDAP property.
        /// </summary>
        /// <param name="directoryEntry">The DirectoryEntry object to target</param>
        /// <param name="ldap_Property">The LDAP property to get</param>
        /// <returns>A string of the LDAP property's value</returns>
        private static string GetProperty(DirectoryEntry directoryEntry, String ldap_Property)
        {
            return ((directoryEntry.Properties.Contains(ldap_Property))
                ? directoryEntry.Properties[ldap_Property][0].ToString()
                : "");
        }


        #endregion



        #region DoesAccountExist

        /// <summary>
        /// Returns true if an AD account exists.
        /// </summary>
        /// <param name="logonID">The account to check</param>
        /// <returns>Returns true if account Exists</returns>
        public static bool DoesAccountExist(string logonID)
        {
            DirectorySearcher directorySearcher = new DirectorySearcher();
            directorySearcher.Filter = "(&(ObjectCategory=user)" + "(sAMAccountName=" + logonID + "))";
            SearchResult searchResult = directorySearcher.FindOne();
            return (searchResult != null);
        }

        #endregion

        #region IsAccountLocked

        /// <summary>
        /// Returns true if an AD account is locked.  Will change this to return null if not found at some point.
        /// </summary>
        /// <param name="logonID">The account to check</param>
        public static bool IsAccountLocked(string logonID)
        {
            UserPrincipal up = UserPrincipal.FindByIdentity(principalContext, logonID);
            return up.IsAccountLockedOut();
        }

        #endregion

        #region IsAccountEnabled

        /// <summary>
        /// Returns true if an AD account exists and is enabled.
        /// </summary>
        /// <param name="logonID">The AD account to check</param>
        public static bool IsAccountEnabled(string logonID)
        {
            ActiveDirectoryAccount account = GetAccount(logonID);
            return ( (account.LogonID != null) && (account.Enabled) );
        }

        #endregion

        #region IsAccountExpired

        /// <summary>
        /// Returns true if an AD account has expired.
        /// </summary>
        /// <param name="logonID">The AD account to check</param>
        public static bool IsAccountExpired(string logonID)
        {
            UserPrincipal up = UserPrincipal.FindByIdentity(principalContext, logonID);
            return (up.AccountExpirationDate == null);
        }

        #endregion

        #region ValidatePassword

        /// <summary>
        /// Returns true if the specified password of an AD account is valid.
        /// </summary>
        /// <param name="logonID">The AD account to validate</param>
        /// <param name="password">The password of the AD account to validate</param>
        public static bool ValidatePassword(string logonID, string password)
        {
            return principalContext.ValidateCredentials(logonID, password, ContextOptions.Negotiate);
        }

        #endregion



        #region GetAccount

        /// <summary>
        /// Returns the ActiveDirectoryAccount of that matches a specified LDAP property with a specified value.  Returns null if it doesn't exist.
        /// </summary>
        /// <param name="value">The value to search for</param>
        /// <param name="ldap_Property">The LDAP property to search</param>
        public static ActiveDirectoryAccount GetAccount(string value, string ldap_Property)
        {
            DirectorySearcher directorySearcher = new DirectorySearcher();
            directorySearcher.Filter = "(&(objectCategory=person)(objectClass=user)(" + ldap_Property + "=" + value + "))";
            SearchResult searchResult = directorySearcher.FindOne();
            return ( (searchResult == null) ? new ActiveDirectoryAccount() : new ActiveDirectoryAccount(searchResult) );
        }

        /// <summary>
        /// Returns the ActiveDirectoryAccount of that matches a specified LDAP property with a specified value.  Returns null if it doesn't exist.
        /// This override assumes the LDAP property is sAMAccountName if the second parameter is omitted.
        /// </summary>
        /// <param name="value">The value to search for</param>
        public static ActiveDirectoryAccount GetAccount(string value)
        {
            return GetAccount(value, LDAP_Properties.LogonID);
        }

        #endregion

        #region GetAccounts

        /// <summary>
        /// Returns an ActiveDirectoryAccountList containing all AD accounts that match the specified query.
        /// </summary>
        /// <param name="query">A string containing a search filter.  e.g.
        /// "(&amp;" +
        /// "(objectCategory=person)" +   // All user objects...
        /// "(|(sn=smith)(sn=johnson))" + // ...with the surname "smith" or "johnson".
        /// "(!givenName=graham)" +       // ...but not with the given name "graham"
        /// ")";
        /// </param>
        public static ActiveDirectoryAccountList GetAccounts(string query)
        {
            ActiveDirectoryAccountList list = new ActiveDirectoryAccountList();
            DirectoryEntry root = new DirectoryEntry("LDAP://" + domain, id, password, AuthenticationTypes.Secure);
            DirectorySearcher directorySearcher = new DirectorySearcher(root);
            directorySearcher.Filter = ((query == "*") ? "(&(objectCategory=person))" : query);
            directorySearcher.PageSize = 1000;
            foreach (SearchResult searchResult in directorySearcher.FindAll())
            {
                ActiveDirectoryAccount account = new ActiveDirectoryAccount(searchResult);
DebugTrace.WriteLine(account.LogonID + " " + account.Department);
                list.All.Add(account);
            }
            return list;
        }

        /// <summary>
        /// Searches for all AD accounts that match the given query.  Returns an ActiveDirectoryAccountList object - which is empty if none are found.
        /// This override assumes the caller is requesting ALL accounts.  BEWARE!!, this takes approx. 40mins to complete.
        /// </summary>
        /// <returns>Returns an ActiveDirectoryAccountList containing all user accounts</returns>
        public static ActiveDirectoryAccountList GetAccounts()
        {
            return GetAccounts("*");
        }

        #endregion

        #region UnlockAccount

        /// <summary>
        /// Unlocks an AD account.  Returns void.
        /// </summary>
        /// <param name="logonID">The AD account to unlock</param>
        public static void UnlockAccount(string logonID)
        {
            UserPrincipal up = UserPrincipal.FindByIdentity(principalContext, logonID);
            up.UnlockAccount();
            up.Save();
        }

        #endregion

        #region DisableAccount

        /// <summary>
        /// Disables an AD account (by setting UAC to 514).  Returns True if successful.
        /// </summary>
        /// <param name="logonID">The AD account to disable</param>
        public static bool DisableAccount(string logonID)
        {
            bool rc = false;
            try
            {
                DirectoryEntry root = new DirectoryEntry("LDAP://" + domain, id, password, AuthenticationTypes.Secure);
                DirectorySearcher directorySearcher = new DirectorySearcher(root, "(&(ObjectCategory=user)" + "(sAMAccountName=" + logonID + "))");
                SearchResult searchResult = directorySearcher.FindOne();
                if (searchResult != null)
                {
                    DirectoryEntry directoryEntry = searchResult.GetDirectoryEntry();
                    directoryEntry.Properties["userAccountControl"].Value = "514";
                    directoryEntry.CommitChanges();
                    directoryEntry.Close();
                    rc = true;
                }
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                throw new Exception("Processing 'DisableAccount'",e);
            }
            return rc;
        }

        #endregion

        #region EnableAccount

        /// <summary>
        /// Enables an AD account (by setting the UAC to 512).  Returns True if successful.
        /// </summary>
        /// <param name="logonID">The AD account to enable</param>
        public static bool EnableAccount(string logonID)
        {
            bool rc = false;
            try
            {
                DirectoryEntry root = new DirectoryEntry("LDAP://" + domain, id, password, AuthenticationTypes.Secure);
                DirectorySearcher directorySearcher = new DirectorySearcher(root, "(&(ObjectCategory=user)" + "(sAMAccountName=" + logonID + "))");
                SearchResult searchResult = directorySearcher.FindOne();
                if (searchResult != null)
                {
                    DirectoryEntry directoryEntry = searchResult.GetDirectoryEntry();
                    directoryEntry.Properties["userAccountControl"].Value = "512";
                    directoryEntry.CommitChanges();
                    directoryEntry.Close();
                    rc = true;
                }
            }
            catch (DirectoryServicesCOMException e)
            {
                throw new Exception("Processing 'EnableAccount'", e);
            }
            return rc;
        }

        #endregion

        #region ExpirePassword

        /// <summary>
        /// Expires an AD account's password.  Returns void.
        /// </summary>
        /// <param name="logonID">The AD account to expire the password for</param>
        public static void ExpirePassword(string logonID)
        {
            UserPrincipal up = UserPrincipal.FindByIdentity(principalContext, logonID);
            up.ExpirePasswordNow();
            up.Save();
        }

        #endregion

        #region ResetPassword

        /// <summary>
        /// Resets an AD account's password.  Returns True if successful.
        /// </summary>
        /// <param name="logonID">The AD account to reset the password for</param>
        /// <param name="newPassword">The new password to use</param>
        /// <param name="errorMessage">A string containing any exceptions</param>
        public static bool ResetPassword(string logonID, string newPassword, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                UserPrincipal up = UserPrincipal.FindByIdentity(principalContext, logonID);
                if (up == null) { errorMessage = logonID + " does not exist"; return false; }
                else { up.SetPassword(newPassword); return true; }
            }
            catch (Exception e)
            {
                errorMessage = e.Message.ToString();
                return false;
            }
        }

        public static bool ResetPassword(string logonID, string newPassword)
        {
            string errorMessage;
            return ResetPassword(logonID, newPassword, out errorMessage);
        }

        #endregion

        #region CreateAccount

        /// <summary>
        /// Creates an AD account.  Returns true if successful.
        /// </summary>
        /// <param name="ou">The OU location to save the new AD account</param>
        /// <param name="logonID">The name of the new AD account</param>
        /// <param name="newPassword">The password of the new AD account</param>
        /// <param name="givenName">The given name of the new user</param>
        /// <param name="surname">The surname of the new user</param>
        /// <param name="errorMessage">A string containing any exceptions</param>
        public static bool CreateAccount(string ou, string logonID, string newPassword, string givenName, string surname, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (DoesAccountExist(logonID)) { errorMessage = logonID + " already exists"; return false; }
                else
                {
                    UserPrincipal up = new UserPrincipal(principalContext, logonID, newPassword, true /*Enabled or not*/);
                    up.UserPrincipalName = logonID;
                    up.GivenName = givenName;
                    up.Surname = surname;
                    up.Save();
                    return true;
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message.ToString();
                return false;
            }
        }

        public static bool CreateAccount(string ou, string logonID, string newPassword, string givenName, string surname)
        {
            string errorMessage;
            return CreateAccount(ou, logonID, newPassword, givenName, surname, out errorMessage);
        }

        #endregion

        #region DeleteAccount

        /// <summary>
        /// Deletes an AD account!  Returns true if successful.
        /// </summary>
        /// <param name="logonID">The AD account to target</param>
        /// <param name="errorMessage">A string containing any exceptions</param>
        //public static bool DeleteAccount(string logonID, out string errorMessage)
        //{
        //    errorMessage = "";
        //    try
        //    {
        //        UserPrincipal up = UserPrincipal.FindByIdentity(principalContext, logonID);
        //        if (up == null) { errorMessage = logonID + " does not exist"; return false; }
        //        else { up.Delete(); return true; }
        //    }
        //    catch (Exception e)
        //    {
        //        errorMessage = e.Message.ToString();
        //        return false;
        //    }
        //}

        //public static bool DeleteAccount(string logonID)
        //{
        //    string errorMessage;
        //    return DeleteAccount(logonID, out errorMessage);
        //}

        #endregion

        #region SetProperty

        /// <summary>
        /// Sets an LDAP property of an AD account.  Returns true if successful, false if not (or if the AD account doesn't exist).
        /// </summary>
        /// <param name="logonID">The AD account to set the property of</param>
        /// <param name="ldap_Property">The LDAP attribute to set</param>
        /// <param name="value">The new value</param>
        /// <param name="clear">Set as true if the value is to be cleared</param>
        public static bool SetProperty(string logonID, string ldap_Property, string value = "", bool clear = false)
        {
            if ( !clear && ( (logonID == "") || (value == "") ) ) return false;
            if ( (!clear) && (ldap_Property == "manager") & (string.IsNullOrEmpty(value)) ) return false;

            DirectoryEntry root = new DirectoryEntry("LDAP://" + domain, id, password, AuthenticationTypes.Secure);
            DirectorySearcher directorySearcher = new DirectorySearcher(root, "(&(ObjectCategory=user)" + "(sAMAccountName=" + logonID + "))");
            SearchResult searchResult = directorySearcher.FindOne();
            if (searchResult == null)
            {
                return false;
            }
            else
            {
                DirectoryEntry directoryEntry = searchResult.GetDirectoryEntry();

                if (clear)
                    directoryEntry.Properties[ldap_Property].Clear();

                else if (ldap_Property == "manager")
                {
                    try
                    {
                		ActiveDirectoryAccount manager = GetAccount(value);
                        directoryEntry.Properties["manager"].Value = manager.DistinguishedName;
                    }
                    catch (Exception e)
                    {
                    }
                    finally
                    {
                    }
                }

                else
                {
                    try
                    {
                        directoryEntry.Properties[ldap_Property][0] = value;
                    }
                    catch (Exception e)
                    {
                        directoryEntry.Properties[ldap_Property].Add(value);
                    }
                    finally
                    {
                    }
                }

                directoryEntry.CommitChanges();
                directoryEntry.Close();
                return true;
            }
        }

        #endregion



        #region GetGroups

        /// <summary>
        /// Returns a List<string> containing a list of all AD groups.
        /// </summary>
        /// <param name="errorMessage">A string containing any exceptions</param>
        public static List<string> GetGroups(out string errorMessage)
        {
            errorMessage = "";
            List<string> list = new List<string>();
            try
            {
                GroupPrincipal gp = new GroupPrincipal(principalContext);
                PrincipalSearcher ps = new PrincipalSearcher(gp);
                foreach (var a in ps.FindAll()) if (a.StructuralObjectClass == "group") list.Add(a.Name);
                list.Sort();
            }
            catch (Exception e)
            {
                errorMessage = e.Message.ToString();
            }
            return list;
        }

        public static List<string> GetGroups()
        {
            string errorMessage;
            return GetGroups(out errorMessage);
        }

        #endregion

        #region GetGroupMembers

        /// <summary>
        /// Returns a ActiveDirectoryAccountList containing all the AD accounts in an AD group.
        /// </summary>
        /// <param name="groupName">The name of the group to get the memberships for</param>
        /// <param name="errorMessage">A string containing any exceptions</param>
        public static ActiveDirectoryAccountList GetGroupMembers(string groupName, out string errorMessage)
        {
            errorMessage = "";
            ActiveDirectoryAccountList list = new ActiveDirectoryAccountList();
            try
            {
                GroupPrincipal gp = GroupPrincipal.FindByIdentity(principalContext, groupName);
                if (gp == null) errorMessage = groupName + " does not exist";
                else
                {
                    foreach (Principal member in gp.Members)
                    {
                        ActiveDirectoryAccount account = GetAccount(member.SamAccountName);
                        list.All.Add(account);
                    }
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message.ToString();
            }
            return list;
        }

        public static ActiveDirectoryAccountList GetGroupMembers(string groupName)
        {
            string errorMessage;
            return GetGroupMembers(groupName, out errorMessage);
        }

        #endregion

        #region GetGroupMemberships

        /// <summary>
        /// Returns a List<string> containing a list of all AD groups that an AD account is a member of.  Returns an empty list if the account doesn't exist.
        /// </summary>
        /// <param name="logonID">The account to get the group memberships for</param>
        /// <param name="authorisationGroups">Pass True if you want a list of Authorisation groups</param>
        /// <param name="errorMessage">A string containing any exceptions</param>
        public static List<string> GetGroupMemberships(string logonID, bool authorisationGroups, out string errorMessage)
        {
            errorMessage = "";
            List<string> list = new List<string>();
            try
            {
                UserPrincipal up = UserPrincipal.FindByIdentity(principalContext, logonID);
                if (up == null) errorMessage = logonID + " does not exist";
                else
                {
                    PrincipalSearchResult<Principal> psr = ( authorisationGroups ? up.GetAuthorizationGroups() : up.GetGroups() );
                    foreach (Principal p in psr) list.Add(p.Name.ToUpper());
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message.ToString();
            }
            list.Sort();
            return list;
        }

        public static List<string> GetGroupMemberships(string logonID, bool authorisationGroups)
        {
            string errorMessage;
            return GetGroupMemberships(logonID, authorisationGroups, out errorMessage);
        }

        public static List<string> GetGroupMemberships(string logonID)
        {
            string errorMessage;
            return GetGroupMemberships(logonID, false, out errorMessage);
        }

        #endregion

        #region IsGroupMember

        /// <summary>
        /// Returns true if an AD account is a member of an AD group.
        /// </summary>
        /// <param name="logonID">The AD account to look for</param>
        /// <param name="groupName">The AD group to look in</param>
        /// <param name="errorMessage">A string containing any exceptions</param>
        public static bool IsGroupMember(string logonID, string groupName, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                UserPrincipal up = UserPrincipal.FindByIdentity(principalContext, logonID);
                GroupPrincipal gp = GroupPrincipal.FindByIdentity(principalContext, groupName);
                if (up == null) { errorMessage = logonID + " does not exist"; return false; }
                if (gp == null) { errorMessage = groupName + " does not exist"; return false; }
                return gp.Members.Contains(up);
            }
            catch (Exception e)
            {
                errorMessage = e.Message.ToString();
                return false;
            }
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="logonID"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public static bool IsGroupMember(string logonID, string groupName)
        {
            string errorMessage;
            return IsGroupMember(logonID, groupName, out errorMessage);
        }

        #endregion



        #region AddToGroup

        /// <summary>
        /// Adds an AD account to an AD group.  Returns true if successful or the AD account was already a member of the AD group.
        /// </summary>
        /// <param name="logonID">The AD account to look for</param>
        /// <param name="groupName">The AD group to look in</param>
        /// <param name="errorMessage">A string containing any exceptions</param>
        public static bool AddToGroup(string logonID, string groupName, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (IsGroupMember(logonID, groupName)) return true;
                DirectoryEntry root = new DirectoryEntry("LDAP://" + domain, id, password, AuthenticationTypes.Secure);
                DirectorySearcher dsGroup = new DirectorySearcher();
                dsGroup.SearchRoot = root;
                dsGroup.Filter = "(&(objectClass=group) (cn=" + groupName + "))";
                SearchResultCollection results = dsGroup.FindAll();
                if (results.Count > 0)
                {
                    DirectorySearcher dsUser = new DirectorySearcher(root, "(&(ObjectCategory=user)" + "(sAMAccountName=" + logonID + "))");
                    SearchResult searchResult = dsUser.FindOne();
                    DirectoryEntry deUser = new DirectoryEntry();
                    if (searchResult != null) deUser = searchResult.GetDirectoryEntry();

                    DirectoryEntry group = results[0].GetDirectoryEntry();
                    group.Invoke("Add", new object[] { deUser.Path.ToString() });
                    group.CommitChanges();
                    group.Close();
                }
                return true;
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                errorMessage = e.Message.ToString();
                return false;
            }
        }

        public static bool AddToGroup(string logonID, string groupName)
        {
            string errorMessage;
            return AddToGroup(logonID, groupName, out errorMessage);
        }

        #endregion

        #region RemoveFromGroup

        /// <summary>
        /// Removes an AD account from an AD group.  Returns true if successful.
        /// </summary>
        /// <param name="logonID">The account to remove</param>
        /// <param name="groupName">The group to remove it from</param>
        /// <param name="errorMessage">A string containing any exceptions</param>
        public static bool RemoveFromGroup(string logonID, string groupName, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (!IsGroupMember(logonID, groupName)) return false;
                DirectoryEntry root = new DirectoryEntry("LDAP://" + domain, id, password, AuthenticationTypes.Secure);
                DirectorySearcher dsGroup = new DirectorySearcher();
                dsGroup.SearchRoot = root;
                dsGroup.Filter = "(&(objectClass=group) (cn=" + groupName + "))";
                SearchResultCollection results = dsGroup.FindAll();
                if (results.Count > 0)
                {
                    DirectorySearcher dsUser = new DirectorySearcher(root, "(&(ObjectCategory=user)" + "(sAMAccountName=" + logonID + "))");
                    SearchResult searchResult = dsUser.FindOne();
                    DirectoryEntry deUser = new DirectoryEntry();
                    if (searchResult != null) deUser = searchResult.GetDirectoryEntry();

                    DirectoryEntry group = results[0].GetDirectoryEntry();
                    group.Invoke("Remove", new object[] { deUser.Path.ToString() });
                    group.CommitChanges();
                    group.Close();
                }
                return true;
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                errorMessage = e.Message.ToString();
                return false;
            }
        }

        public static bool RemoveFromGroup(string logonID, string groupName)
        {
            string errorMessage;
            return RemoveFromGroup(logonID, groupName, out errorMessage);
        }

        #endregion

        #region CreateGroup

        /// <summary>
        /// Creates a new AD group.  Returns True if successful.
        /// </summary>
        /// <param name="groupName">The name of the new group</param>
        /// <param name="description">A description of the new group</param>
        /// <param name="securityGroup">Set as true if this is to be a security group, false if it's a distribution group</param>
        /// <param name="errorMessage">A string containing any exceptions</param>
        public static bool CreateGroup(string groupName, string description, bool securityGroup, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                GroupPrincipal gp = new GroupPrincipal(principalContext, groupName);
                gp.Description = description;
                gp.GroupScope = GroupScope.Global;
                gp.IsSecurityGroup = securityGroup;
                gp.Save();
                return true;
            }
            catch (Exception e)
            {
                errorMessage = e.Message.ToString();
                return false;
            }
        }

        public static bool CreateGroup(string groupName, string description, bool securityGroup)
        {
            string errorMessage;
            return CreateGroup(groupName, description, securityGroup, out errorMessage);
        }

        #endregion

        #region Get_OU

        /// <summary>
        /// Returns a string containing the full OU path of the specified AD account.  Returns "" if the account doesn't exist.
        /// </summary>
        /// <param name="logonID">The AD account to get the OU path for</param>
        /// <param name="errorMessage">A string containing any exceptions</param>
        public static string Get_OU(string logonID, out string errorMessage)
        {
            errorMessage = "";
            string ouPath = "";
            try
            {
                UserPrincipal up = UserPrincipal.FindByIdentity(principalContext, logonID);
                if (up != null)
                {
                    DirectoryEntry directoryEntry = (up.GetUnderlyingObject() as DirectoryEntry);
                    if (directoryEntry != null)
                    {
                        string[] directoryEntryPath = directoryEntry.Path.Split(',');
                        foreach (var splitedPath in directoryEntryPath)
                        {
                            string[] elements = splitedPath.Split('=');
                            if (elements[0].Trim() == "OU") ouPath += ",OU=" + elements[1].Trim();
                        }
                    }
                }
                if (ouPath.StartsWith(",")) ouPath = ouPath.Substring(1) + ",DC=durham,DC=gov,DC=uk";
            }
            catch (Exception e)
            {
                errorMessage = e.Message.ToString();
            }
            return ouPath;
        }

        public static string Get_OU(string logonID)
        {
            string errorMessage;
            return Get_OU(logonID, out errorMessage);
        }

        #endregion

        #region Get_OUs

        /// <summary>
        /// Returns a List<string> containing all AD OU paths.
        /// </summary>
        /// <param name="errorMessage">A string containing any exceptions</param>
        public static List<string> Get_OUs(out string errorMessage)
        {
            errorMessage = "";
            List<string> list = new List<string>();
            try
            {
                DirectoryEntry root = new DirectoryEntry("LDAP://" + domain, id, password, AuthenticationTypes.Secure);
                DirectorySearcher directorySearcher = new DirectorySearcher(root);
                directorySearcher.Filter = "(objectClass=organizationalUnit)";
                directorySearcher.SearchScope = SearchScope.Subtree;
                directorySearcher.PropertiesToLoad.Add("displayName");
                foreach (SearchResult searchResult in directorySearcher.FindAll()) list.Add(searchResult.Path.Substring(21));
            }
            catch (Exception e)
            {
                errorMessage = e.Message.ToString();
            }
            return list;
        }

        public static List<string> Get_OUs()
        {
            string errorMessage;
            return Get_OUs(out errorMessage);
        }

        #endregion

        #region Get_OU_Members

        /// <summary>
        /// Returns an ActiveDirectoryAccountList containing all the AD accounts in the specified OU.
        /// </summary>
        /// <param name="ouPath">The OU path to look in</param>
        /// <param name="errorMessage">A string containing any exceptions</param>
        public static ActiveDirectoryAccountList Get_OU_Members(string ouPath, out string errorMessage)
        {
            errorMessage = "";
            ActiveDirectoryAccountList list = new ActiveDirectoryAccountList();
            try
            {
                DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://" + ouPath);
                foreach (DirectoryEntry child in directoryEntry.Children)
                {
                    ActiveDirectoryAccount account = GetAccount(child.Properties["sAMAccountName"].Value.ToString());
                    list.All.Add(account);
                    child.Close();
                    child.Dispose();
                }
                directoryEntry.Close();
                directoryEntry.Dispose();
            }
            catch (DirectoryServicesCOMException e)
            {
                errorMessage = e.Message.ToString();
            }
            return list;
        }

        public static ActiveDirectoryAccountList Get_OU_Members(string ouPath)
        {
            string errorMessage;
            return Get_OU_Members(ouPath, out errorMessage);
        }

        #endregion

        #region MoveAccount

        /// <summary>
        /// Moves an AD account to a different OU.  Returns true if successful, false if the account doesn't exist.
        /// </summary>
        /// <param name="logonID">The AD account to move</param>
        /// <param name="newOUPath">The new OU path to move the account</param>
        /// <param name="errorMessage">A string containing any exceptions</param>
        public static bool MoveAccount(string logonID, string newOUPath, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                DirectoryEntry root = new DirectoryEntry("LDAP://" + domain, id, password, AuthenticationTypes.Secure);
                DirectorySearcher directorySearcher = new DirectorySearcher(root, "(&(ObjectCategory=user)" + "(sAMAccountName=" + logonID + "))");
                SearchResult searchResult = directorySearcher.FindOne();
                if (searchResult != null)
                {
                    DirectoryEntry directoryEntry = searchResult.GetDirectoryEntry();
                    DirectoryEntry myNewPath = null;
                    myNewPath = new DirectoryEntry("LDAP://" + domain + "/" + newOUPath, id, password, AuthenticationTypes.Secure);
                    directoryEntry.MoveTo(myNewPath);
                    directoryEntry.CommitChanges();
                    directoryEntry.Close();
                }
                return true;
            }
            catch (Exception e)
            {
                errorMessage = e.Message.ToString();
                return false;
            }
        }

        public static bool MoveAccount(string logonID, string newOUPath)
        {
            string errorMessage;
            return MoveAccount(logonID, newOUPath, out errorMessage);
        }

        #endregion



    }

}


#region Further notes

// List of UserAccountControl codes:

//Normal Day to Day Values:
//===========================
//512 - Enable Account
//514 - Disable account
//544 - Account Enabled - Require user to change password at first logon
//4096 - Workstation/server
//66048 - Enabled, password never expires
//66050 - Disabled, password never expires
//262656 - Smart Card Logon Required
//532480 - Domain controller

//All Other Values:
//===========================
//1 - script
//2 - accountdisable
//8 - homedir_required
//16 - lockout
//32 - passwd_notreqd
//64 - passwd_cant_change
//128 - encrypted_text_pwd_allowed
//256 - temp_duplicate_account
//512 - normal_account
//2048 - interdomain_trust_account
//4096 - workstation_trust_account
//8192 - server_trust_account
//65536 - dont_expire_password
//131072 - mns_logon_account
//262144 - smartcard_required
//524288 - trusted_for_delegation
//1048576 - not_delegated
//2097152 - use_des_key_only
//4194304 - dont_req_preauth
//8388608 - password_expired
//16777216 - trusted_to_auth_for_delegation

// The following codes are used in DCC accounts to signify disabled accounts:  514, 66050, 66080, 66082

#endregion
