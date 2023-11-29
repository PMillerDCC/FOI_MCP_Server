using DCC_Standard;
using ProspectBusinessLayer;
using System.Web.Mvc;

namespace Prospect.Controllers
{
    public class ReportController : DCC_Controller
    {

        #region ContractsPaymentsSchedule ActionResult (Get)

        [HttpGet]
        public ActionResult ContractsPaymentsSchedule()
        {
            ContractsPaymentsScheduleParameters model = new ContractsPaymentsScheduleParameters();
            model.StartDate = new System.DateTime(2023, 1, 1);
            model.EndDate = new System.DateTime(2023, 7, 31);
            ViewBag.PageTitle = "Contracts Payments Schedule";
            return View(model);
        }

        #endregion


        #region ContractsPaymentsSchedule ActionResult (Post)

        [HttpPost]
        public ActionResult ContractsPaymentsSchedule(string displayButton, ContractsPaymentsScheduleParameters model)
        {
            model.LoadHTML(DCC_Session.AutoDatabaseConnection);
            ViewBag.PageTitle = "Contracts Payments Schedule";
            return View(model);
        }

        #endregion

    }

}
