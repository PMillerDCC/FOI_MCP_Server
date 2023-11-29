using DCC_Standard;
using ProspectBusinessLayer;
using System.Web.Mvc;

namespace Prospect.Controllers
{
    public class ContractSpecificationController : DCC_Controller
    {

        #region Contract Specification ActionResult (Get)

        [Authorize]
        [HttpGet]
        public ActionResult ContractSpecification()
        {
            #region Determine which ContractSpecification has been selected. First looks in DCC_Session, then the URL.
            int k_ContractSpecification = -1;
            object o = DCC_Session.Get("Saved_K_ContractVariation");
            if ((o is int) && ((int)o > 0)) k_ContractSpecification = (int)o;
            else
            {
                string queryKey = Request["PrimaryKey"] ?? "-1";
                if (int.TryParse(queryKey, out k_ContractSpecification) && (k_ContractSpecification == -999))
                {
                    this.DCC_DisplayMessage(DCC_MessageType.Error, "You must select a Contract before clicking Edit", true);
                    return RedirectToAction("Index");
                }
            }
            #endregion

            ContractSpecification model = new ContractSpecification(DCC_Session.AutoDatabaseConnection, k_ContractSpecification);
            ViewBag.PageTitle = "Contract Specification";
            return View(model);
        }

        #endregion

    }
}