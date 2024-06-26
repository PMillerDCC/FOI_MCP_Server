using DCC_Standard;
using ProspectBusinessLayer;
using System.Web.Mvc;

namespace Prospect.Controllers
{
    public class ContractorController : DCC_Controller
    {

        #region ContractorGrid

        [Authorize]
        public ActionResult ContractorGrid()
        {
            ContractorGrid model = new ContractorGrid(DCC_Session.AutoDatabaseConnection);
            model.Initialise(
                Request,
                id: "ContractorGrid",
                itemNameSingular: "Operator",
                column1: new GridColumn(GridColumnType.Text, "ContractorLicenceNo"     , "Licence number"    , 120, defaultSortColumn: true),
                column2: new GridColumn(GridColumnType.Text, "ContractorName"          , "Name"              , 220),
                column3: new GridColumn(GridColumnType.Text, "ContractorTradeName"     , "Trade name"        , 210),
                column4: new GridColumn(GridColumnType.Text, "TransportManager"        , "Transport manager" , 140),
                column5: new GridColumn(GridColumnType.Text, "LicenceTypeDescription"  , "Licence type"      , 200),
                column6: new GridColumn(GridColumnType.Text, "LicenceStatusDescription", "Is current"        , 230),
                column7: new GridColumn(GridColumnType.Text, "DateLicenceExpiresText"  , "Date expires"      , 110, boundSortField: "DateLicenceExpiresSort"),
                addActionResult: "/Contractor/Contractor",
                editActionResult: "/Contractor/Contractor",
                noOfRowsInGrid: 16,
                showEditButton: false,
                showSelectorColumn: false,
                fullRowSelect: true,
                dcc_Session: DCC_Session
                );

            ViewBag.PageTitle = "Operators";
            return View(model);
        }

        #endregion


        #region Contractor ActionResult (Get)

        [HttpGet]
        public ActionResult Contractor()
        {
            #region Determine which Contractor has been selected. First looks in DCC_Session, then the URL.
            int k_Contractor = -1;
            string queryKey = Request["primaryKey"] ?? "-1";
            if ((int.TryParse(queryKey, out k_Contractor)) && (k_Contractor == -999))
            {
                this.DCC_DisplayMessage(DCC_MessageType.Error, "You must select an Operator before clicking Edit", true);
                return RedirectToAction("Index");
            }
            #endregion

            Contractor model = new Contractor(DCC_Session.AutoDatabaseConnection, k_Contractor);

            ViewBag.PageTitle = "Operator";
            return View(model);
        }

        #endregion


        #region Contractor ActionResult (Post)

        [Authorize]
        [HttpPost]
        public ActionResult Contractor(string saveButton, Contractor model)
        {
            model.PopulateDropDownLists(DCC_Session.AutoDatabaseConnection);

            if (!string.IsNullOrEmpty(saveButton))
            {
                #region Page validation

                //if (model.Reference.Length != 4) ModelState.AddModelError("Reference", "A specification reference must be 4 characters.");

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

            ViewBag.PageTitle = "Operator";
            return View(model);
        }

        #endregion

    }
}
