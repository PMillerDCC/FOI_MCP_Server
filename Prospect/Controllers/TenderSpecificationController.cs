using DCC_Standard;
using ProspectBusinessLayer;
using System.Web.Mvc;

namespace Prospect.Controllers
{
    public class TenderSpecificationController : DCC_Controller
    {

        #region TenderSpecification ActionResult (Get)

        [Authorize]
        [HttpGet]
        public ActionResult TenderSpecification()
        {
            #region Determine which TenderSpecification has been selected. First looks in DCC_Session, then the URL.
            int k_TenderSpecification = -1;
            string queryKey = Request["primaryKey"] ?? "-1";
            if (int.TryParse(queryKey, out k_TenderSpecification) && (k_TenderSpecification == -999))
            {
                this.DCC_DisplayMessage(DCC_MessageType.Error, "You must select a Specification before clicking Edit", true);
                return RedirectToAction("Index");
            }
            #endregion

            int saved_K_Tender = (int)DCC_Session.Get("Saved_K_Tender");

            TenderSpecification model = new TenderSpecification(DCC_Session.AutoDatabaseConnection, k_TenderSpecification, saved_K_Tender);

            ViewBag.PageTitle = "Specification";
            return View(model);
        }

        #endregion


        #region TenderSpecification ActionResult (Post)

        [Authorize]
        [HttpPost]
        public ActionResult TenderSpecification(string saveButton, TenderSpecification model)
        {
            if (!string.IsNullOrEmpty(saveButton))
            {
                #region Page validation
                //if (model.TenderReference.Length != 4) ModelState.AddModelError("Reference", "A Tender reference must be 4 characters.");
                #endregion

                #region On save
                if (ModelState.IsValid)
                {
                    if (model.Save(DCC_Session.AutoDatabaseConnection) < 0) this.DCC_DisplayMessage(DCC_MessageType.Error, "A problem occurred while saving");
                    else this.DCC_DisplayMessage(DCC_MessageType.Success, "Your changes have been saved");
                }
                else this.DCC_DisplayMessage(DCC_MessageType.Error, "Some of your changes are invalid");
                #endregion
            }

            model.PopulateDropDownLists(DCC_Session.AutoDatabaseConnection);

            ViewBag.PageTitle = "Specification";
            return View(model);
        }

        #endregion

    }
}
