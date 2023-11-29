using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace DCC_Standard
{

    /// <summary>
    /// Helpers to encrypt and decrypt connection strings
    /// </summary>
    public class ConnectionStringHelpers
    {

        // DO NOT CHANGE THESE WHILST *ANY* APP.CONFIG OR WEB.CONFIG CONTAINS ENCRYPTED DATA //              **** VERY IMPORTANT ****
        static string passPhrase = "DurhamCC";         // can be any string
        static string saltValue = "NaCl";             // can be any string
        static string hashAlgorithm = "SHA1";             // can be "MD5"
        static int passwordIterations = 2;                  // can be any number
        static string initVector = "senrab-b1953-yar"; // must be 16 bytes
        static int keySize = 256;                // can also be 192 or 128
        static string marker = "$";                // marker at start of connection string name to indicate encrypted

        /// <summary>
        /// Gets value true if any of the config file connection strings remain unencrypted
        /// </summary>
        public static bool HasUnencryptedConnections
        {
            get
            {
                Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
                foreach (ConnectionStringSettings css in config.ConnectionStrings.ConnectionStrings)
                {
                    if (css.Name == "LocalSqlServer") continue;
                    if (css.Name == "OraAspNetConString") continue;
                    if (!css.Name.StartsWith(marker)) return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Encrypts all the connections string inside the CONFIG file except specific name 'LocalSqlServer'
        /// </summary>
        public static void EncryptConnectionString()
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            //ConfigurationSection section = config.GetSection("connectionStrings");
            //if (!section.SectionInformation.IsProtected)
            //{
            //    section.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");   // this is the Microsoft standard
            //    config.Save();
            //}
            List<ConnectionStringSettings> clearCS = new List<ConnectionStringSettings>();
            foreach (ConnectionStringSettings css in config.ConnectionStrings.ConnectionStrings)
            {
                if (!css.Name.StartsWith(marker)) clearCS.Add(css);
            }
            bool needSave = false;
            foreach (ConnectionStringSettings css in clearCS)
            {
                string cssName = css.Name;
                if (cssName == "LocalSqlServer") continue;
                if (cssName == "OraAspNetConString") continue;

                string plainText = css.ConnectionString;
                string cipherText = DCC_Encryption.RijndaelSimple.Encrypt(
                    plainText,
                    passPhrase,
                    saltValue,
                    hashAlgorithm,
                    passwordIterations,
                    initVector,
                    keySize);
                string plainText2 = css.ProviderName;
                string cipherText2 = DCC_Encryption.RijndaelSimple.Encrypt(
                    plainText2,
                    passPhrase,
                    saltValue,
                    hashAlgorithm,
                    passwordIterations,
                    initVector,
                    keySize);
                config.ConnectionStrings.ConnectionStrings.Remove(css.Name);
                config.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings(marker + css.Name, cipherText, cipherText2));
                needSave = true;
            }
            if (needSave) config.Save();
        }

        /// <summary>
        /// Decrypts all the connections string inside the CONFIG file 
        /// </summary>
        public static void DecryptConnectionString()
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            List<ConnectionStringSettings> cypherCS = new List<ConnectionStringSettings>();
            foreach (ConnectionStringSettings css in config.ConnectionStrings.ConnectionStrings)
            {
                if (css.Name.StartsWith(marker)) cypherCS.Add(css);
            }
            bool needSave = false;
            foreach (ConnectionStringSettings css in cypherCS)
            {
                config.ConnectionStrings.ConnectionStrings.Remove(css.Name);
                config.ConnectionStrings.ConnectionStrings.Add(Decrypt1ConnectionString(css));
                needSave = true;
            }
            if (needSave) config.Save();
        }

        /// <summary>
        /// Reveal the encrypted connections string inside the CONFIG file 
        /// </summary>
        public static string RevealConnectionStrings(bool writeHTML)
        {
            StringBuilder sb = new StringBuilder();
            if (writeHTML) sb.AppendLine("<table summary='List of Connection Strings'>");
            foreach (ConnectionStringSettings css in ConfigurationManager.ConnectionStrings)
            {
                ConnectionStringSettings cssClear = css;
                if (css.Name.StartsWith(marker)) cssClear = Decrypt1ConnectionString(css);
                if (writeHTML) sb.AppendLine("<tr><td valign='top'><b>");
                sb.Append(cssClear.Name);
                if (writeHTML) sb.AppendLine("</b></td><td>"); else sb.Append('\t');
                sb.AppendLine(cssClear.ConnectionString);
                if (writeHTML) sb.AppendLine("</td><td>"); else sb.Append('\t');
                sb.AppendLine(cssClear.ProviderName);
                if (writeHTML) sb.AppendLine("</td></tr>"); else sb.AppendLine("");
            }
            if (writeHTML) sb.AppendLine("</table>");
            return sb.ToString();
        }

        private static ConnectionStringSettings Decrypt1ConnectionString(ConnectionStringSettings css)
        {

            string cipherText = css.ConnectionString;
            string plainText = DCC_Encryption.RijndaelSimple.Decrypt(
                               cipherText,
                               passPhrase,
                               saltValue,
                               hashAlgorithm,
                               passwordIterations,
                               initVector,
                               keySize);
            string cipherText2 = css.ProviderName;
            string plainText2 = DCC_Encryption.RijndaelSimple.Decrypt(
                                cipherText2,
                                passPhrase,
                                saltValue,
                                hashAlgorithm,
                                passwordIterations,
                                initVector,
                                keySize);
//#if (DEBUG)
            //                System.Diagnostics.Trace.WriteLine(plainText + "  .... " + plainText2);
//#endif
            return new ConnectionStringSettings(css.Name.Substring(marker.Length), plainText, plainText2);
        }

        /// <summary>
        /// Retrieves un-encrypted connection string from CONFIG
        /// </summary>
        /// <param name="csID">Name of the required string</param>
        /// <returns>Clear text connection string</returns>
        public static ConnectionStringSettings FetchConnectionStringSettings(string csID)
        {
            bool isLocalHost = true;
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Request != null)
                {
                    isLocalHost = HttpContext.Current.Request.IsLocal;
                }
            }

            ConnectionStringSettings cs = ConfigurationManager.ConnectionStrings[csID];
            if (cs == null)
            {
                cs = ConfigurationManager.ConnectionStrings[marker + csID];
                if (cs != null) cs = Decrypt1ConnectionString(cs);
            }
            return cs;
        }

        public static string MaskConnectionString(string plainConnectionString)
        {
            string reply = plainConnectionString;
            {
                int nAt = reply.ulIndexOf("password");
                if (nAt > -1)
                {
                    int nAt2 = reply.IndexOf('=', nAt);
                    if (nAt2 > -1)
                    {
                        int nAt3 = reply.IndexOf(';', nAt2);
                        if (nAt3 == -1)
                        {
                            string pass = reply.Substring(nAt);
                            reply = reply.Replace(pass, "password=**********");
                        }
                        else
                        {
                            string pass = reply.Substring(nAt, nAt3 - nAt);
                            reply = reply.Replace(pass, "password=**********");
                        }
                    }
                }
            }
            {
                int nAt = reply.ulIndexOf("pwd");
                if (nAt > -1)
                {
                    int nAt2 = reply.IndexOf('=', nAt);
                    if (nAt2 > -1)
                    {
                        int nAt3 = reply.IndexOf(';', nAt2);
                        if (nAt3 == -1)
                        {
                            string pass = reply.Substring(nAt);
                            reply = reply.Replace(pass, "pwd=**********");
                        }
                        else
                        {
                            string pass = reply.Substring(nAt, nAt3 - nAt);
                            reply = reply.Replace(pass, "pwd=**********");
                        }
                    }
                }
            }
            {
                int nAt = reply.ulIndexOf("user id");
                if (nAt > -1)
                {
                    int nAt2 = reply.IndexOf('=', nAt);
                    if (nAt2 > -1)
                    {
                        int nAt3 = reply.IndexOf(';', nAt2);
                        if (nAt3 == -1)
                        {
                            string pass = reply.Substring(nAt);
                            reply = reply.Replace(pass, "user id=**********");
                        }
                        else
                        {
                            string pass = reply.Substring(nAt, nAt3 - nAt);
                            reply = reply.Replace(pass, "user id=**********");
                        }
                    }
                }
            }
            {
                int nAt = reply.ulIndexOf("uid");
                if (nAt > -1)
                {
                    int nAt2 = reply.IndexOf('=', nAt);
                    if (nAt2 > -1)
                    {
                        int nAt3 = reply.IndexOf(';', nAt2);
                        if (nAt3 == -1)
                        {
                            string pass = reply.Substring(nAt);
                            reply = reply.Replace(pass, "uid=**********");
                        }
                        else
                        {
                            string pass = reply.Substring(nAt, nAt3 - nAt);
                            reply = reply.Replace(pass, "uid=**********");
                        }
                    }
                }
            }
            //username

            return reply;
            /*
All blank characters, except those placed within a value or within quotation marks, are ignored
Blank characters will though affect connection pooling mechanism, pooled connections must have the exact same connection string
If a semicolon (;) is part of a value it must be delimited by quotation marks (")
Use a single-quote (') if the value begins with a double-quote (")
Conversely, use the double quote (") if the value begins with a single quote (')
No escape sequences are supported
The value type is not relevant
Names are case iNsEnSiTiVe
If a KEYWORD=VALUE pair occurs more than once in the connection string, the value associated with the last occurrence is used
However, if the provider keyword occurs multiple times in the string, the first occurrence is used.
If a keyword contains an equal sign (=), it must be preceded by an additional equal sign to indicate that it is part of the keyword.
If a value has preceding or trailing spaces it must be enclosed in single- or double quotes, ie Keyword=" value ", else the spaces are removed.
             */
        }

    }

}
