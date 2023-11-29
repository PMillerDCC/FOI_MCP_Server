using DCC_Standard;
using ProspectBusinessLayer;
using System.Web.Mvc;

namespace Prospect.Controllers
{
    public class TenderController : DCC_Controller
    {

        #region TenderGrid

        [Authorize]
        public ActionResult TenderGrid()
        {
            DCC_Session.Add("Saved_K_Specification", -100); // mechanism to tell the tender view where to return to; put -100 for tendergrid; -200 for contractgrid; or K_Specification from the specification view; null to reset
            DCC_Session.Add("Saved_K_Tender", null);

            TenderGrid model = new TenderGrid(DCC_Session.AutoDatabaseConnection);
            model.Initialise(
                Request,
                id: "TenderGrid",
                itemNameSingular: "Tender",
                column1: new GridColumn(GridColumnType.Text, "ContractReference"        , "Contract reference", 150, alignRight: false, boundSortField: "ContractReferenceSort", defaultSortColumn: true, defaultSortDescending: true),
                column2: new GridColumn(GridColumnType.Text, "ContractorLicenceNo"      , "Licence number"    , 120),
                column3: new GridColumn(GridColumnType.Text, "IsTenderAccepted"         , "Is accepted"       , 100, trueFalse: "Yes|No"),
                column4: new GridColumn(GridColumnType.Text, "IsPackage"                , "Is package"        , 100, trueFalse: "Yes|No"),
                column5: new GridColumn(GridColumnType.Text, "TenderReference"          , "Tender reference"  , 150, alignRight: false, boundSortField: "TenderReferenceSort"),
                column6: new GridColumn(GridColumnType.Text, "SpecificationServiceNo"   , "Service number"    , 150),
                column7: new GridColumn(GridColumnType.Text, "RouteText"                , "Route"             , 290),
                //addActionResult: "/Tender/Tender",
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
            #region Determine which Tender has been selected. First looks in DCC_Session, then the URL.
            int k_Tender = -1;
            object o = DCC_Session.Get("Saved_K_Tender");
            if ((o is int) && ((int)o > 0)) k_Tender = (int)o;
            else
            {
                string queryKey = Request["primaryKey"] ?? "-1";
                if (int.TryParse(queryKey, out k_Tender) && (k_Tender == -999))
                {
                    this.DCC_DisplayMessage(DCC_MessageType.Error, "You must select a Tender before clicking Edit", true);
                    return RedirectToAction("Index");
                }
            }

            int saved_K_Specification = (int)DCC_Session.Get("Saved_K_Specification");
            DCC_Session.Add("Saved_K_Tender", k_Tender);
            #endregion

            Tender model = new Tender(DCC_Session.AutoDatabaseConnection, k_Tender, saved_K_Specification);

            if (model.IsNew)
            {
                model.Save(DCC_Session.AutoDatabaseConnection);
                model = new Tender(DCC_Session.AutoDatabaseConnection, model.K_Tender);
            }

            #region Initialise TenderSpecification Grid

            model.TenderSpecificationGrid.Initialise(
                Request,
                id: "TenderSpecificationGrid",
                itemNameSingular: "Child specification",
                column1: new GridColumn(GridColumnType.Text, "ChildContractReference", "Contract reference", 100, alignRight: true, boundSortField: "ChildContractReferenceSort", defaultSortColumn: true, defaultSortDescending: false),
                column2: new GridColumn(GridColumnType.Text, "SpecificationServiceNo", "Service number", 180),
                column3: new GridColumn(GridColumnType.Text, "RouteText", "Route", 290),
                column4: new GridColumn(GridColumnType.Text, "DatesText", "Dates", 200),
                column5: new GridColumn(GridColumnType.Text, "ContractTypeDescription", "Contract type", 90),
                addActionResult: "/TenderSpecification/TenderSpecification",
                editActionResult: "/TenderSpecification/TenderSpecification",
                noOfRowsInGrid: 10,
                showEditButton: false,
                showSelectorColumn: false,
                fullRowSelect: true,
                dcc_Session: DCC_Session
                );

            #endregion

            #region Initialise Contract Variations Grid

            if (model.IsTenderAccepted)
            {
                model.ContractVariationGrid.Initialise(
                    Request,
                    id: "ContractVariationsGrid",
                    itemNameSingular: "Contract variation",
                    column1: new GridColumn(GridColumnType.Text, "VariationNoText", "Variation Number", 90, alignRight: true, boundSortField: "VariationNo", defaultSortColumn: true, defaultSortDescending: true),
                    column2: new GridColumn(GridColumnType.Text, "CvDateVariationsStartatext", "Variation Start Date", 160),
                    column3: new GridColumn(GridColumnType.Text, "CvPriceChanged", "Price Changed?", 160),
                    column4: new GridColumn(GridColumnType.Text, "AgreementChanged", "Fares Agreement Changed?", 200),
                    column5: new GridColumn(GridColumnType.Text, "NoOfDaysOfOperationPerYearChanged", "Days in Operation Changed?", 200),
                    addButtonText: "Add New Contract Variation",
                    addActionResult: "/ContractVariation/ContractVariation",
                    editActionResult: "/ContractVariation/ContractVariation",
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


        #region Tender ActionResult (Post)

        [Authorize]
        [HttpPost]
        public ActionResult Tender(string saveButton, Tender model)
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

            model.PopulateDropDownLists(DCC_Session.AutoDatabaseConnection, model.K_Tender);

            #region Initialise TenderSpecification Grid

            model.TenderSpecificationGrid.Initialise(
                Request,
                id: "TenderSpecificationGrid",
                itemNameSingular: "Child specification",
                column1: new GridColumn(GridColumnType.Text, "ChildContractReference", "Contract reference", 100, alignRight: true, boundSortField: "ChildContractReferenceSort", defaultSortColumn: true, defaultSortDescending: false),
                column2: new GridColumn(GridColumnType.Text, "SpecificationServiceNo", "Service number", 180),
                column3: new GridColumn(GridColumnType.Text, "RouteText", "Route", 290),
                column4: new GridColumn(GridColumnType.Text, "DatesText", "Dates", 200),
                column5: new GridColumn(GridColumnType.Text, "ContractTypeDescription", "Contract type", 80),
                addActionResult: "/TenderSpecification/TenderSpecification",
                editActionResult: "/TenderSpecification/TenderSpecification",
                noOfRowsInGrid: 10,
                showEditButton: false,
                showSelectorColumn: false,
                fullRowSelect: true,
                dcc_Session: DCC_Session
            );

            #endregion

            #region Initialise ContractVariation Grid

            if (model.IsTenderAccepted)
            {
                model.ContractVariationGrid.Initialise(
                    Request,
                    id: "ContractVariationGrid",
                    itemNameSingular: "Contract variation",
                    column1: new GridColumn(GridColumnType.Text, "CvVariationNumber", "Variation Number", 90, alignRight: true, boundSortField: "CvariationNumber", defaultSortColumn: true, defaultSortDescending: true),
                    column2: new GridColumn(GridColumnType.Text, "CvDateVariationsStarta", "Variation Start Date", 100),
                    column3: new GridColumn(GridColumnType.Text, "CvDateVariationsEnda", "Variation End Date", 100),
                    addActionResult: "/ContractVariation/ContractVariation",
                    editActionResult: "/ContractVariation/ContractVariation",
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
