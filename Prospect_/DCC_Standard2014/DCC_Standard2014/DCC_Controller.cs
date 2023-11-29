using System.Web.Mvc;

namespace DCC_Standard
{

    /// <summary>
    /// Custom controller class
    /// </summary>
    public abstract class DCC_Controller : Controller
    {
        /// <summary>
        /// </summary>
        protected readonly DCC_Session DCC_Session;

        /// <summary>
        /// </summary>
        protected DCC_Controller()
        {
            DCC_Session = Initialisation.Start();
			ViewBag.SoftwareIdentity = DCC_Session.SoftwareIdentity;
			ViewBag.ApplicationTitle = DCC_Session.ApplicationTitle;
			ViewBag.UnderMaintenance = DCC_Session.UnderMaintenance;
			ViewBag.Navigation = DCC_Session.NavigationOptions;
			ViewBag.FooterMessage = DCC_Session.LogonInfo;
			ViewBag.LogonID = DCC_Session.LogonID;
		}

        /// <summary>
        /// </summary>
		protected override void OnException(ExceptionContext filterContext)
		{
			filterContext.ExceptionHandled = true;
			DebugTrace.WriteLine("ERROR - " + filterContext.Exception.Message);
			DebugTrace.WriteLine(filterContext.Exception.StackTrace);
            DebugTrace.WriteLine();
            //ViewBag.ExceptionMessage = filterContext.Exception.Message;
			filterContext.Result = RedirectToAction("Index", "Error", new { message = filterContext.Exception.Message });
		}

    }

}
