using System.Web;

namespace DCC_Standard
{

    /// <summary>
    /// MasterPage helper
    /// </summary>
    public class JavaScriptHelper
    {

        const string NOJS = "NoJavascript";
        const string SESSION_NAME = "NoJavascript";

        /// <summary>
        /// LiteralControl Text giving users notification
        /// </summary>
        public static string InfoText
        {
            get
            {
                return @"
<div>
<div style='border: 2px solid fuchsia; font-weight: bold; color: fuchsia; font-size: 150%; padding: 20px; margin: 50px; text-align: center;'>
Javascript is disabled.
<br />
<br />
To enjoy the full functionality of this application you should enable scripting in your browser, and restart this application.
</div>
</div>";
            }
        }

        /// <summary>
        /// Checks if session contains "ScriptStatus" (as created by special HTM page)
        /// </summary>
        /// <returns>true if javascript is NOT available</returns>
        public static bool TestJavaScript()
        {
            if (HttpContext.Current != null)
            {
                string urlParameter=(string)(HttpContext.Current.Request["ScriptStatus"] ?? "");

                if (urlParameter == "")
                {
                    // if no parameter supplied, then look at session variable

                    object nj = HttpContext.Current.Session[SESSION_NAME];
                    if (nj != null)
                    {
                        if (nj is string)
                        {
                            return (((string)nj) == NOJS);
                        }
                    }
                    // if no session variable either, then just assume we have Javascript
                    //HttpContext.Current.Session.Add(SESSION_NAME,"JavascriptEnabled");
                    return false;
                }

                bool hasNoJS = (urlParameter == "Disabled");
                //HttpContext.Current.Session.Add(SESSION_NAME,(hasNoJS?NOJS:"JavascriptEnabled"));
                return hasNoJS;
            }
            return false;
        }

    }

}
