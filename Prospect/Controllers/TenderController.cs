using DCC_Standard;
using Microsoft.Ajax.Utilities;
using ProspectBusinessLayer;
using System;
using System.Web.Mvc;

namespace Prospect.Controllers
{
    public class TenderController : DCC_Controller
    {

        #region TenderGrid

        [Authorize]
        public ActionResult TenderGrid()
        {
            DCC_Session.Add("Saved_K_Specification", -999); // store the primary key of the specification so it's automatically added when adding a tender from this view, set to -999 to reset

            TenderGrid model = new TenderGrid(DCC_Session.AutoDatabaseConnection);
            model.Initialise(
                Request,
                id: "TenderGrid",
                itemNameSingular: "Tender",
                column1: new GridColumn(GridColumnType.Text, "TenderReference"           , "Tender reference"  ,  90, alignRight: true, boundSortField: "TenderReferenceSort", defaultSortColumn: true, defaultSortDescending: true),
                column2: new GridColumn(GridColumnType.Text, "ContractorName"            , "Operator"          , 250),
                column3: new GridColumn(GridColumnType.Text, "ContractorLicenceNo"       , "Licence number"    ,  90),
                column4: new GridColumn(GridColumnType.Text, "ContractReference"         , "Contract reference",  90, alignRight: true, boundSortField: "ContractReferenceSort"),
                column5: new GridColumn(GridColumnType.Text, "SpecificationServiceNoText", "Service number"    , 180),
                column6: new GridColumn(GridColumnType.Text, "RouteText"                 , "Route"             , 290),
                column7: new GridColumn(GridColumnType.Text, "IsPackageText"             , "Is package?"       , 100),
                column8: new GridColumn(GridColumnType.Text, "IsTenderAcceptedText"      , "Is accepted?"      , 100),
                addActionResult: "/Tender/Tender",
                editActionResult: "/Tender/Tender",
                noOfRowsInGrid: 16,
                showEditButton: false,
                showSelectorColumn: false,
                fullRowSelect: true,
                dcc_Session: DCC_Session
                );

            ViewBag.PageTitle = "Tenders";
            return View(model);
        }

        #endregion


        #region Tender ActionResult (Get)

        [Authorize]
        [HttpGet]
        public ActionResult Tender()
        {
            int k_Tender = -1;
            string queryKey = Request["primaryKey"] ?? "-1";
            if (int.TryParse(queryKey, out k_Tender) && (k_Tender == -999))
            {
                this.DCC_DisplayMessage(DCC_MessageType.Error, "You must select a Tender before clicking Edit", true);
                return RedirectToAction("Index");
            }

            int saved_K_Specification;
            try
            {
                saved_K_Specification = (int)DCC_Session.Get("Saved_K_Specification");
            }
            catch
            {
                saved_K_Specification = -999;
            }

            var db = DCC_Session.AutoDatabaseConnection;
            var model = new Tender(db, k_Tender, saved_K_Specification, autoSaveIfNew: false);

            if (model.Q_Specification > 0 && model.TenderAverageMilesPerDay <= 0m)
            {
                var spec = new Specification(db, model.Q_Specification, pBriefMode: true);
                model.TenderAverageMilesPerDay = spec.AverageMilesPerDay;
            }

            model.PopulateDropDownLists(db, model.K_Tender);

            ViewBag.PageTitle = "Tender";
            return View(model);
        }

        #endregion


        #region Tender ActionResult (Post)

        [Authorize]
        [HttpPost]
        public ActionResult Tender(string saveButton, Tender model)
        {
            #region Load parent specification details for form rendering
            if ((model.Q_Specification > 0) && string.IsNullOrEmpty(model.SpecificationServiceNo))
            {
                Specification spec = new Specification(DCC_Session.AutoDatabaseConnection, model.Q_Specification, pBriefMode: true);
                model.IsPackage = spec.IsPackage;
            }
            #endregion

            model.PopulateDropDownLists(DCC_Session.AutoDatabaseConnection, model.K_Tender);

            if (!string.IsNullOrEmpty(saveButton))
            {
                #region Validation
                // if (model.TenderReference.Length != 4) ModelState.AddModelError("Reference", "A Tender reference must be 4 characters.");
                #endregion

                if (ModelState.IsValid)
                {
                    var db = DCC_Session.AutoDatabaseConnection;
                    var mode = "Tender";
                    //var mode = (model.K_Tender > 0) ? "Update" : "Insert";

                    var newKey = model.Save(db, mode);

                    if (newKey <= 0)
                    {
                        this.DCC_DisplayMessage(DCC_MessageType.Error, "A problem occurred while saving");
                    }
                    else
                    {
                        this.DCC_DisplayMessage(DCC_MessageType.Success, "Your changes have been saved", true);
                        return RedirectToAction("Tender", new { primaryKey = newKey });
                    }
                }
                else
                {
                    this.DCC_DisplayMessage(DCC_MessageType.Error, "Some of your changes are invalid");
                }
            }

            #region Contract Variation Grid
            if (model.IsTenderAccepted)
            {
                model.ContractVariationGrid.Initialise(
                    Request,
                    id: "ContractVariationsGrid",
                    itemNameSingular: "Contract variation",
                    column1: new GridColumn(GridColumnType.Text, "VariationNoText", "Variation number", 90, alignRight: true, boundSortField: "VariationNo", defaultSortColumn: true, defaultSortDescending: true),
                    noOfRowsInGrid: 10,
                    showEditButton: false,
                    showSelectorColumn: false,
                    fullRowSelect: true,
                    dcc_Session: DCC_Session
                );
            }
            #endregion
          
            ViewBag.PageTitle = "Tender";
            return View(model);
        }

        #endregion

    }

}
