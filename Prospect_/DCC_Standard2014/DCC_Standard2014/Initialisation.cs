using System;
using System.Configuration;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;

namespace DCC_Standard
{

    /// <summary>
    /// Initialisation class
    /// </summary>
    public class Initialisation
    {

        /// <summary>
        /// Start procedure
		/// Creates a DCC_Session if one doesn't already exist
        /// </summary>
        public static DCC_Session Start(bool isWeb = true, bool isOffline = false)
        {


			bool sendDevelopersToTestDatabase = true;




			string debugTrace = "A";

			DCC_Session session;

			string host = null;
			string softwareIdentity = ConfigurationManager.AppSettings["SoftwareIdentityCode"] ?? "DCC_Application";
			string authorised_AD_Group = ConfigurationManager.AppSettings["Authorised_AD_Group"] ?? "*public internal*";
			string developers_AD_Group = ConfigurationManager.AppSettings["Developers_AD_Group"] ?? "InternalApplicationsGroup";
			string autoDatabaseConnection = ConfigurationManager.AppSettings["AutoDatabaseConnection"] ?? "*none*";
			string getAutoDatabaseConnectionFromURL = ConfigurationManager.AppSettings["GetAutoDatabaseConnectionFromURL"] ?? "false";
			string sessionID = null;

			Regex r = new Regex("^[A-Za-z][A-Za-z0-9_-]{3,49}$");
			if (!r.IsMatch(softwareIdentity)) throw new Exception("Web.config must have 'SoftwareIdentityCode' defined in <AppSettings>. It must be 5-50 chars consisting only of letters, digits, hyphens or underscores and must start with a letter. Value found is [" + softwareIdentity + "]");

			if (isOffline) isWeb = false;

			if (isWeb)
			{
				sessionID = System.Web.HttpContext.Current.Session.SessionID;
				host = HttpContext.Current.Request.Url.Host.ToLower().Replace(".durham.gov.uk","");
				if (!host.ToLower().Contains("localhost")) softwareIdentity = host;
				//session = (DCC_Session)HttpContext.Current.Session[softwareIdentity + "_CurrentUser"];
				session = (DCC_Session)HttpContext.Current.Session[softwareIdentity + "_" + sessionID];

				#region if the session exists, check it is complete
				DebugTrace.WriteLine("Initialisation A:  Host is " + host);
				DebugTrace.WriteLine("Initialisation A:  Auto Database Connection is " + autoDatabaseConnection);
				if (session == null)
				{
					DebugTrace.WriteLine("Initialisation A:  " + softwareIdentity + "_" + sessionID + " does not exist");
				}
				else
				{
					if (authorised_AD_Group.ToLower().Contains("public external"))
					{
						DebugTrace.WriteLine("Initialisation A:  " + softwareIdentity + "_" + sessionID + " exists and authentication method is public external");
					}
					else
					{
						if ( (session.IsExternalUser) && ((session.K_RegisteredUser < 1) || (session.LogonID == "IUSR")) )
						{
							DebugTrace.WriteLine("Initialisation A:  " + softwareIdentity + "_" + sessionID + " exists but K_RegisteredUser < 1 OR the LoginID is IUSR");
							session = null;
						}
						else
						{
							DebugTrace.WriteLine("Initialisation A:  " + softwareIdentity + "_" + sessionID + " exists");
						}
					}
				}
				#endregion
			}
			else
			{
				session = null;
				DebugTrace.WriteLine("Initialisation A:  Non-web system");
			}


            if (session == null)
            {
				try
				{
					debugTrace = "B";

					session = new DCC_Session();
					session.SoftwareIdentity = softwareIdentity;
					session.ApplicationTitle = ConfigurationManager.AppSettings["ApplicationTitle"] ?? "(no Application Title provided in web.config)";
					session.UnderMaintenance = ConfigurationManager.AppSettings["UnderMaintenance"] ?? "";

					// new stuff march 23
					if ( (!string.IsNullOrEmpty(host)) && (host.Substring(host.Length -4) == "test"))
					{
						autoDatabaseConnection += "Test";
						session.IsTest = true;
					}


					#region Web Authentication

					if (isWeb)
					{
						session.HasJavascript = !JavaScriptHelper.TestJavaScript();
						//if (!session.HasJavascript) session.OuterMessage += (JavaScriptHelper.InfoText ?? "");
						if (HttpContext.Current.Request != null)
						{
							session.Host = host;
							session.URL = "https://" + session.SoftwareIdentity + ".durham.gov.uk";
							session.IsLocal = HttpContext.Current.Request.IsLocal;
							session.IPAddress = HttpContext.Current.Request.UserHostAddress;
						}

						#region External Web Authentication

						if (authorised_AD_Group.ToLower().Contains("external"))
						{
							debugTrace = "C";

							session.IsExternalUser = true;
							if (authorised_AD_Group.ToLower().Contains("public"))
							{
								DebugTrace.WriteLine("Initialisation C:  User is External and Public");
								session.IsAuthorised = true;
							}
							else
							{
								DebugTrace.WriteLine("Initialisation C:  User is External; getting user info from session variables from login page");

								session.LogonID =
								session.EmailAddress = ( ((HttpContext.Current.Session["ExternalUserEmailAddress"] == null)) ? "" : HttpContext.Current.Session["ExternalUserEmailAddress"].ToString());
								session.GivenName    = ( ((HttpContext.Current.Session["ExternalUserFirstName"]    == null)) ? "" : HttpContext.Current.Session["ExternalUserFirstName"].ToString());
								session.LastName     = ( ((HttpContext.Current.Session["ExternalUserLastName"]     == null)) ? "" : HttpContext.Current.Session["ExternalUserLastName"].ToString());
								session.FullName     = ( ((HttpContext.Current.Session["ExternalUserFullName"]     == null)) ? "" : HttpContext.Current.Session["ExternalUserFullName"].ToString());
								session.AccessLevel  = ( ((HttpContext.Current.Session["ExternalUserAccessLevel"]  == null)) ? "" : HttpContext.Current.Session["ExternalUserAccessLevel"].ToString());
								int k_RegisteredUserid = -1;
								if (session.LogonID != "")	int.TryParse(HttpContext.Current.Session["ExternalUserK_RegisteredUser"].ToString(), out k_RegisteredUserid);
								session.K_RegisteredUser = k_RegisteredUserid;
								session.IsAuthorised = (session.K_RegisteredUser > 0);

								DebugTrace.WriteLine("Initialisation C:  session.Host is " + session.Host);
								DebugTrace.WriteLine("Initialisation C:  session.URL is " + session.URL);
								DebugTrace.WriteLine("Initialisation C:  session.IPAddress is " + session.IPAddress);
								DebugTrace.WriteLine("Initialisation C:  externalUserK_RegisteredUserid is " + k_RegisteredUserid.ToString());
								DebugTrace.WriteLine("Initialisation C:  externalUserEmailAddress is " + session.EmailAddress);
								DebugTrace.WriteLine("Initialisation C:  externalUserFirstName is " + session.GivenName);
								DebugTrace.WriteLine("Initialisation C:  externalUserLastName is " + session.LastName);
								DebugTrace.WriteLine("Initialisation C:  externalUserFullName is " + session.FullName);
								DebugTrace.WriteLine("Initialisation C:  externalUserAccessLevel is " + session.AccessLevel);
								DebugTrace.WriteLine("Initialisation C:  K_RegisteredUser is " + session.K_RegisteredUser.ToString());
								DebugTrace.WriteLine("Initialisation C:  isAuthorised is " + session.IsAuthorised.ToString());
                            }

							if (session.K_RegisteredUser > 0)
							{
								DebugTrace.WriteLine("Initialisation C:  Authenticated!  Creating session");
								HttpContext.Current.Session[softwareIdentity + "_" + sessionID] = session;
							}
							else
							{
								DebugTrace.WriteLine("Initialisation C:  Failed to authenticate");
	//							throw new Exception("Failure during authentication phase");
							}
                        }
						#endregion

						#region Internal Web Authentication

						else
						{
							debugTrace = "D";
							session.IsExternalUser = false;
							session.LogonID = HttpContext.Current.Request.LogonUserIdentity.Name.Substring(HttpContext.Current.Request.LogonUserIdentity.Name.LastIndexOf(@"\") + 1);
							if (session.LogonID == "IUSR") session.LogonID = "";
							DebugTrace.WriteLine("Initialisation D:  User is Internal and authenticated through AD as " + session.LogonID);

							try
							{
								ActiveDirectory.ActiveDirectoryAccount account = ActiveDirectory.GetAccount(session.LogonID);
								if ((!string.IsNullOrEmpty(account.LogonID)) && (account.Enabled))
								{
									session.FullName = account.FullName;
									session.GivenName = account.GivenName;
									session.LastName = account.LastName;
									session.Initials = account.Initials;
									session.PostTitle = account.Title;
									session.EmployeeNumber = account.EmployeeNumber;
									session.EmailAddress = account.EmailAddress;
									session.TelephoneNumber = account.TelephoneNumber;
									session.Company = account.Company;
									session.Service = account.Department;
									session.Division = account.Division;
									session.HomeDrive = account.HomeDirectory;
									session.HomeDirectory = account.HomeDrive;
									session.Location = account.Location;
									session.UserAccountControl = account.UserAccountControl;
									session.OU = account.OU;
									session.GroupMemberships = account.GroupMemberships;
									session.IsDeveloper = session.GroupMemberships.Contains(developers_AD_Group.ToUpper());
								}

								if (string.IsNullOrEmpty(session.LogonID))
								{
									DebugTrace.WriteLine("Initialisation D:  Failed to authenticate");
								}
								else
								{
									DebugTrace.WriteLine("Initialisation D:  Authenticated!  Creating session");
									HttpContext.Current.Session[softwareIdentity + "_" + sessionID] = session;
								}

								// send me to the test database automatically
								if (sendDevelopersToTestDatabase && session.IsDeveloper && (autoDatabaseConnection == "MyWorkforce"))
								{
									autoDatabaseConnection += "Test";
									session.IsTest = true;
								}
							}
							catch (Exception e)
							{
								debugTrace = "Initialisation D:  " + e.Message;
							}
						}

						#endregion
					}

					#endregion


					#region Windows Authentication

					else
					{
						debugTrace = "E";
						DebugTrace.WriteLine("Initialisation E:  Windows Authentication");

						WindowsPrincipal windowsPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
						string logonID = windowsPrincipal.Identity.Name;
						session.LogonID = logonID.Substring(logonID.LastIndexOf("\\") + 1);

						if (isOffline)
						{
							session.AuditNo = Environment.MachineName;
							DebugTrace.WriteLine("Initialisation E:  System is Offline");
						}

						else
						{
							ActiveDirectory.ActiveDirectoryAccount account = ActiveDirectory.GetAccount(session.LogonID);
							if ((account.LogonID != null) && (account.Enabled))
							{
								session.FullName = account.FullName;
								session.GivenName = account.GivenName;
								session.LastName = account.LastName;
								session.Initials = account.Initials;
								session.PostTitle = account.Title;
								session.EmployeeNumber = account.EmployeeNumber;
								session.EmailAddress = account.EmailAddress;
								session.TelephoneNumber = account.TelephoneNumber;
								session.Company = account.Company;
								session.Service = account.Department;
								session.Division = account.Division;
								session.HomeDrive = account.HomeDirectory;
								session.HomeDirectory = account.HomeDrive;
								session.Location = account.Location;
								session.UserAccountControl = account.UserAccountControl;
								session.OU = account.OU;
								session.GroupMemberships = account.GroupMemberships;
								session.IsDeveloper = session.GroupMemberships.Contains(developers_AD_Group.ToUpper());
								session.AuditNo = Environment.MachineName;
								DebugTrace.WriteLine("Initialisation E:  User is Authenticated!");
							}
							else
							{
								DebugTrace.WriteLine("Initialisation E:  User either does not exist in AD (?) or user is disabled");
							}
						}

					}

					#endregion


					#region Establish Connection Strings

					if (!isOffline)
					{
						debugTrace = "F";
						DebugTrace.WriteLine("Initialisation F:  Establish Connection Strings");

						foreach (ConnectionStringSettings cs in ConfigurationManager.ConnectionStrings)
						{
							if ((!cs.Name.ToUpper().Contains("LOCALSQLSERVER")) && (!cs.Name.ToUpper().Contains("ORAASPNETCONSTRING")))
							{
								string tag = cs.Name.Replace("$", "");
								session.DatabaseConnections.Add(tag);
							}
						}
					}

					#endregion


					#region Establish SystemRoles

					// Note:  The AuthorisationGroup functionality is not compatable with the "Get database from URL" functionality as you need to know the database to get the groups

					debugTrace = "G";

					string systemRoles = ConfigurationManager.AppSettings["SystemRoles"];

					if (authorised_AD_Group.ToLower() == "*database*")
					{
						session.AutoDatabaseConnection = new DCC_Database(ConfigurationManager.AppSettings["DatabaseDriver"], autoDatabaseConnection, session.SoftwareIdentity);
						if ((session.AutoDatabaseConnection != null) && (session.AutoDatabaseConnection.DataLayer.IsOpen) && (DCC_Database.DoesTableExist(session.AutoDatabaseConnection, "AuthorisationGroup")))
						{
							DCC_AuthorisationGroupList authorisationGroupList = new DCC_AuthorisationGroupList(session.AutoDatabaseConnection);
							foreach (DCC_AuthorisationGroup group in authorisationGroupList.Selectable)
							{
								systemRoles += "|" + group.AuthorisationGroupName;
							}
						}
					}

					systemRoles += "|NO ACCESS";
					if (systemRoles == null) session.SystemRoles.All.Add(new DCC_SystemRole("Anonymous", true));

					DebugTrace.WriteLine("Initialisation G:  SystemRoles are " + systemRoles);

					#endregion


					#region External Authorisation

					if (session.IsExternalUser)
					{
						debugTrace = "H";

						if (!string.IsNullOrWhiteSpace(session.AccessLevel))
						{
							DebugTrace.WriteLine("Initialisation H:  AccessLevel is " + session.AccessLevel);
							string[] roles = systemRoles.Split('|');
							foreach (string s in roles) session.SystemRoles.Add(new DCC_SystemRole(s, session.AccessLevel.Contains(s)));
						}

						if (session.IsAuthorised) session.IsAuthorised = (!session.IsInRole("NO ACCESS"));
						DebugTrace.WriteLine("Initialisation H:  isAuthorised is " + session.IsAuthorised.ToString());

						#region Determine which Connection String to use

						if (session.IsAuthorised)
						{
							if (getAutoDatabaseConnectionFromURL == "true")
							{
								if (session.Host.ToLower().Contains("localhost"))  // if developer
								{
									if (autoDatabaseConnection.ToLower() != "*none*")
									{
										session.AutoDatabaseConnection = new DCC_Database(ConfigurationManager.AppSettings["DatabaseDriver"], autoDatabaseConnection, session.SoftwareIdentity);
										DebugTrace.WriteLine("Initialisation H1: Using: " + session.AutoDatabaseConnection);
									}
								}
								else
								{
									foreach (string cs in session.DatabaseConnections)
									{
										if (cs.ToLower() == session.SoftwareIdentity.ToLower())
										{
											session.AutoDatabaseConnection = new DCC_Database(ConfigurationManager.AppSettings["DatabaseDriver"], cs, session.SoftwareIdentity);
											DebugTrace.WriteLine("Initialisation H2: Using: " + session.AutoDatabaseConnection);
											break;
										}
									}
								}
							}
							else
							{
								if (autoDatabaseConnection.ToLower() != "*none*")
								{
									session.AutoDatabaseConnection = new DCC_Database(ConfigurationManager.AppSettings["DatabaseDriver"], autoDatabaseConnection, session.SoftwareIdentity);
									DebugTrace.WriteLine("Initialisation H3: Using: " + session.AutoDatabaseConnection);
								}
							}
						}

						#endregion
					}

					#endregion


					#region Internal User Authorisation

					else
					{
						debugTrace = "I";

						#region If the system is offline, the user is authorised

						if ((session.AutoDatabaseConnection == null) && (!autoDatabaseConnection.Contains("*none*")))
						{
							session.AutoDatabaseConnection = new DCC_Database(ConfigurationManager.AppSettings["DatabaseDriver"], autoDatabaseConnection, session.SoftwareIdentity);
						}

						if (isOffline)
						{
							DebugTrace.WriteLine("Initialisation I:  The system is offline so the user is authorised!");
							session.IsAuthorised = true;
						}

						#endregion

						#region If the system uses database authentication, try to authorise using the systemuser table

						else if (authorised_AD_Group.ToLower().Contains("database"))
						{
							if ((session.AutoDatabaseConnection != null) && (session.AutoDatabaseConnection.DataLayer.IsOpen) && (DCC_Database.DoesTableExist(session.AutoDatabaseConnection, "SystemUser")))
							{
								DCC_SystemUser systemUser = new DCC_SystemUser(session, logonID: session.LogonID);
								if (systemUser.LogonID == null)
								{
									if (authorised_AD_Group.ToLower().Contains("public"))
									{
										string[] roles = systemRoles.Split('|');
										session.AccessLevel = "Basic";
										foreach (string s in roles) session.SystemRoles.Add(new DCC_SystemRole(s, (s == "Basic")));
										DebugTrace.WriteLine("Initialisation I:  User is not in SystemUser table but system is public so is authorised!");
										session.IsAuthorised = true;
									}
									else
									{
										DebugTrace.WriteLine("Initialisation I:  The user is not in the SystemUser table and the system is not public so the user is not authorised");
										session.IsAuthorised = false;
									}
								}
								else
								{
									if (!string.IsNullOrWhiteSpace(systemUser.AccessLevel))
									{
										session.K_SystemUser = systemUser.K_SystemUser;
										session.AccessLevel = systemUser.AccessLevel;
										session.LogonID = systemUser.LogonID;
										session.GivenName = systemUser.SystemUserFirstName;
										session.LastName = systemUser.SystemUserLastName;
										string[] roles = systemRoles.Split('|');
										foreach (string s in roles) session.SystemRoles.Add(new DCC_SystemRole(s, systemUser.AccessLevel.Contains(s)));
									}
									if (DCC_Database.DoesTableExist(session.AutoDatabaseConnection, "SystemMessage"))
									{
										DCC_SystemMessageList messageList = new DCC_SystemMessageList(session.AutoDatabaseConnection, systemUser.K_SystemUser);
										foreach (DCC_SystemMessage message in messageList) session.SystemMessages.Add(message);
									}
									if (!session.IsInRole("NO ACCESS"))
									{
										DebugTrace.WriteLine("Initialisation I:  The user is in the systemuser table so is authorised!");
										session.IsAuthorised = true;
									}
									else
									{
										DebugTrace.WriteLine("Initialisation I:  The user has had their access revoked in the systemuser table so is not authorised");
										session.IsAuthorised = false;
									}
								}
							}
						}

						#endregion

						#region If the system is public and database authentication is not used, the user is authorised

						else if (authorised_AD_Group.ToLower().Contains("public"))
						{
							string[] roles = systemRoles.Split('|');
							session.AccessLevel = "Basic";
							foreach (string s in roles) session.SystemRoles.Add(new DCC_SystemRole(s, (s == "Basic")));
							DebugTrace.WriteLine("Initialisation I:  The system is public so the user is authorised!");
							session.IsAuthorised = true;
						}

						#endregion

						#region Allow developers access - currently disabled

						//else if (session.IsDeveloper)
						//{
						//	DebugTrace.WriteLine("Initialisation I:  The user is in the Developers_AD_Group so is authorised!");
						//	session.IsAuthorised = true;
						//}

						#endregion

						#region If the system uses AD group authentication, try to authorise

						else
						{
							//if ((session.GroupMemberships.Contains(authorised_AD_Group.ToUpper())) || (session.IsDeveloper))
							if (session.GroupMemberships.Contains(authorised_AD_Group.ToUpper()))
							{
								DebugTrace.WriteLine("Initialisation I:  The user is in Authorised_AD_Group so is authorised!");
								session.IsAuthorised = true;
							}
							else
							{
								DebugTrace.WriteLine("Initialisation I:  The user is not in Authorised_AD_Group so is not authorised");
								session.IsAuthorised = false;
							}
						}

						#endregion

						#region If the user isn't authorised, display a minimal message

						if (!session.IsAuthorised)
						{
							DebugTrace.WriteLine("Initialisation I:  Display not authorised message");

							HttpContext.Current.Response.Write("<strong>Sorry, you do not have access to this application.  Please contact the system administrator.</strong>");
							HttpContext.Current.Response.End();
						}

						#endregion
					}

					#endregion


					#region Create database connection to SALP if the connection string is in web.config

					if (session.DatabaseConnections.Contains("SALP"))
					{
						DebugTrace.WriteLine("Initialisation J:  Creating database connection to SALP");
						DebugTrace.WriteLine();
						try
						{
							session.ResourceLinkDatabaseConnection = new DCC_Database("ORACLE", "SALP", session.ApplicationTitle);
						}
						catch (Exception e)
						{
						}
						finally
						{
						}
					}

					#endregion

				}

				#region Handle exceptions

				catch (Exception e)
				{
					session.DebugTrace = debugTrace;
					if (isWeb)
					{
						HttpContext.Current.Response.Write(debugTrace);
						HttpContext.Current.Response.Write("<br>");
						HttpContext.Current.Response.Write(e.Message);
						HttpContext.Current.Response.End();
					}
					else MessageBox.Show("An error has occurred during initialisation.  " + e.Message, "Error");
				}

				finally
				{
					session.DebugTrace = debugTrace;
				}

				#endregion

            }


			#region Assign Roles

			DebugTrace.WriteLine("Initialisation K:  Assign Roles");

			if (session != null)
			{
				if (isWeb)
				{
					session.AssignRoles();
				}

				string s = "";
				if (session.SystemRoles.Count > 0)
				{
					foreach (DCC_SystemRole role in session.SystemRoles)
					{
						if (role.IsChecked) s += role.Role + ", ";
					}
					if (s == "")
						s = "You are logged on as " + session.LogonID;
					else
					{
						s = s.Substring(0, s.Length - 2);
						if (Regex.Matches(s, ", ").Count > 0)
						{
							int pos = s.LastIndexOf(", ");
							s = s.Substring(0, pos) + " & " + s.Substring(pos + 2);
						}
						s = "You are logged on as " + session.LogonID + " with " + s + " access";
					}
				}
				session.LogonInfo = s + ( (autoDatabaseConnection.Contains("Test")) ? " on the TEST database" : "");
			}

			#endregion


			#region Create Navigation HTML

			DebugTrace.WriteLine("Initialisation L:  Create Navigation HTML");

			if (System.IO.File.Exists(System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "/Sitemap.xml"))
			{
				System.IO.StreamReader reader = new System.IO.StreamReader(System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "/Sitemap.xml");
				System.Xml.Serialization.XmlSerializer serializer2 = new System.Xml.Serialization.XmlSerializer(typeof(DCC_Navigation));
				DCC_Navigation navigation = (DCC_Navigation)serializer2.Deserialize(reader);

				#region Idenity any last children
				bool foundOne = false;
				for (int i = navigation.OptionList.Count -1; i >= 0; i--)
				{
					if (navigation.OptionList[i].IsChild.ToLower().Contains("yes"))
					{
						if (!foundOne)
						{
							navigation.OptionList[i].IsLastChild = foundOne = true;
						}
					}
					else foundOne = false;
				}
				#endregion

				bool isParentAllowed = true;
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				sb.AppendLine("<ul class=\"navbar\">");
				foreach (NavigationOption option in navigation.OptionList)
				{
					#region Determine if option is restricted for this user
					bool isOptionAllowed = false;
					if (string.IsNullOrWhiteSpace(option.RestrictedTo))
					{
						isOptionAllowed = true;
					}
					else
					{
						string[] roles = option.RestrictedTo.Split('|');
						foreach (string targetRole in roles)
						{
							if (session.IsInRole(targetRole))
							{
								isOptionAllowed = true;
								break;
							}
						}
					}
					if ( (option.IsChild.ToLower() == "yes") && !isParentAllowed) isOptionAllowed = false;
					#endregion

					if (isOptionAllowed)
					{
						if (option.IsParent.ToLower() == "yes")
						{
							// DEC 22 - removed empty href
							if (string.IsNullOrEmpty(option.Href))
                                sb.AppendLine("<li class=\"dropdown\"><a class=\"dropdown-toggle\" data-bs-toggle=\"dropdown\" aria-expanded=\"false\">" + option.Title + "<b class=\"caret\"></b></a><ul class=\"dropdown-menu\">");
							else
	                            sb.AppendLine("<li class=\"dropdown\"><a class=\"dropdown-toggle\" data-bs-toggle=\"dropdown\" href=\"{1}\" aria-expanded=\"false\">" + option.Title + "<b class=\"caret\"></b></a><ul class=\"dropdown-menu\">");
                            isParentAllowed = true;

                            //sb.AppendLine("<li class=\"dropdown\"><a class=\"dropdown-toggle\" data-bs-toggle=\"dropdown\" href=\"{1}\" aria-expanded=\"false\">" + option.Title + "<b class=\"caret\"></b></a><ul class=\"dropdown-menu\">");
                            //isParentAllowed = true;
						}

						else if (option.IsChild.ToLower() == "yes")
						{
							sb.AppendLine("<li><a href=\"" + option.Href + "\">" + option.Title + "</a></li>");
							if (option.IsLastChild) sb.AppendLine("</ul></li>");
						}

						else
						{
							sb.AppendLine("<li><a href=\"" + option.Href + "\">" + option.Title + "</a></li>");
						}
					}

					else if (option.IsParent.ToLower() == "yes") isParentAllowed = false;

				}
				sb.AppendLine("</ul>");
				sb.Replace("{1}", HttpContext.Current.Request.Url.ToString());

				session.NavigationOptions = sb.ToString();
			}

			#endregion


			#region Finished, return session

			DebugTrace.WriteLine("Initialisation M:  Finished!");
			DebugTrace.WriteLine();

		    return session;

			#endregion

		}


	}

}
