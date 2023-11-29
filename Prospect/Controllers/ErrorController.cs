using DCC_Standard;
using System;
using System.Web.Mvc;

namespace Prospect.Controllers
{

    public class ErrorController : Controller
    {


        public ActionResult Index(string message)
        {
            ViewBag.PageTitle = "Error Page";
            ViewBag.ExceptionMessage = message;
            return View();
        }


        public ActionResult NotFound()
        {
            ViewBag.PageTitle = "Error Page";
            return View();
        }


        public ActionResult Exception()
        {
            throw new ArgumentNullException();
        }


        public ActionResult DCC_Exception()
        {
            throw new DCC_Exception();
        }


    }

}