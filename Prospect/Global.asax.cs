using DCC_Standard;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SqlServerTypes;

namespace Prospect
{

    public class MvcApplication : HttpApplication
    {

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModelBinders.Binders.Add(typeof(TimeSpan), new DCC_TimeBinder());
            SqlServerTypes.Utilities.LoadNativeAssemblies(Server.MapPath("~/bin"));
            ModelBinders.Binders.Add(typeof(decimal), new CommaSafeDecimalModelBinder());
            ModelBinders.Binders.Add(typeof(decimal?), new CommaSafeDecimalModelBinder());
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            #region HOW TO RETRIEVE A COOKIE - not currently used - see how the cookie is created in the DCC_Standard->Initialisation Assign Roles region
            //HttpCookie cookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            //if (cookie != null)
            //{
            //    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
            //    string[] roles = ticket.UserData.Split(new Char[] { ',' });
            //}
            #endregion
        }

    }

}