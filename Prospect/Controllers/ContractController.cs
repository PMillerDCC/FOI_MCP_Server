using DCC_Standard;
using ProspectBusinessLayer;
using System.Web.Mvc;

namespace Prospect.Controllers
{
    public class ContractController : DCC_Controller
    {

        #region ContractGrid

        [Authorize]
        public ActionResult ContractGrid()
        {
            DCC_Session.Add("Saved_K_Specification", -200); // mechanism to tell the tender view where to return to; enter -100 for tendergrid; -200 for contractgrid; or K_Specification from the specification view; null to reset
            DCC_Session.Add("Saved_K_Tender", null);
            DCC_Session.Add("Saved_K_ContractVariation", null);

            TenderGrid model = new TenderGrid(DCC_Session.AutoDatabaseConnection, awardedOnly: true);
            model.Initialise(
                Request,
                id: "ContractGrid",
                itemNameSingular: "Contract",
                column1: new GridColumn(GridColumnType.Text, "ContractReference"   , "Contract reference",  90, alignRight: true, boundSortField: "ContractReferenceSort", defaultSortColumn: true, defaultSortDescending: true),
                column2: new GridColumn(GridColumnType.Text, "ContractorLicenceNo" , "Licence Number"    ,  90, alignRight: true),
                column3: new GridColumn(GridColumnType.Text, "TenderReference"     , "Tender reference"  ,  90, alignRight: true, boundSortField: "TenderReferenceSort"),
                column4: new GridColumn(GridColumnType.Text, "IsContractCancelled" , "Cancelled?"        ,  80, trueFalse: "Yes|No"),
                //column5: new GridColumn(GridColumnType.Text, "Extended"           , "Extended"          , 80, trueFalse: "Yes|No"),
                addActionResult: "/Contract/Contract",
                editActionResult: "/Contract/Contract",
                noOfRowsInGrid: 16,
                showEditButton: false,
                showSelectorColumn: false,
                fullRowSelect: true,
                dcc_Session: DCC_Session
                );

            ViewBag.PageTitle = "Contracts";
            return View(model);
        }

        #endregion


        #region Contract ActionResult (Get)

        [Authorize]
        [HttpGet]
        public ActionResult Contract()
        {
            #region Determine which Contract has been selected. First looks in DCC_Session, then the URL.
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

            #endregion

            Tender model = new Tender(DCC_Session.AutoDatabaseConnection, k_Tender) ;

            #region Initialise Contract Variations Grid

            if (model.IsTenderAccepted)
            {

                model.ContractVariationGrid.Initialise(
                    Request,
                    id: "ContractVariationsGrid",
                    itemNameSingular: "Contract variation",
                    column1: new GridColumn(GridColumnType.Text, "ContractVariationNo", "Variation Number", 90, alignRight: true, boundSortField: "ContractVariationNo", defaultSortColumn: true, defaultSortDescending: true),
                    column2: new GridColumn(GridColumnType.Text, "ContractVariationDateStartText", "Variation Start Date", 160),
                    column3: new GridColumn(GridColumnType.Text, "CvPriceChanged", "Price Changed?", 160),
                    column4: new GridColumn(GridColumnType.Text, "AgreementChanged", "Fares Agreement Changed?", 200),
                    column5: new GridColumn(GridColumnType.Text, "NoOfDaysOfOperationPerYearChanged", "Days in Operation Changed?", 200),
                    addButtonText: "Add New Contract Variation",
                    addActionResult: "/ContractVariation/ContractVariation",
                    editActionResult: "/ContractVariation/ContractVariation",
                    noOfRowsInGrid: 16,
                    showEditButton: false,
                    showSelectorColumn: false,
                    fullRowSelect: true,
                    dcc_Session: DCC_Session
                    );
            }
            #endregion

            ViewBag.PageTitle = "Contract";
            return View(model);
        }

        #endregion


        #region Contract ActionResult (Post)

        [Authorize]
        [HttpPost]
        public ActionResult Contract(string saveButton, string createButton, string recycleButton, Tender model)
        {
            bool saveVariation = (!string.IsNullOrEmpty(createButton));

            model.PopulateDropDownLists(DCC_Session.AutoDatabaseConnection, model.K_Tender);

            if ( (!string.IsNullOrEmpty(saveButton)) || (!string.IsNullOrEmpty(createButton)))
            {
                #region Page validation

                //if (model.TenderReference.Length != 4) ModelState.AddModelError("Reference", "A Tender reference must be 4 characters.");

                #endregion

                #region On save
                if (ModelState.IsValid)
                {
                    if (model.Save(DCC_Session.AutoDatabaseConnection, saveVariation) < 0) this.DCC_DisplayMessage(DCC_MessageType.Error, "A problem occurred while saving");
                    else this.DCC_DisplayMessage(DCC_MessageType.Success, "Your changes have been saved");
                }
                else this.DCC_DisplayMessage(DCC_MessageType.Error, "Some of your changes are invalid");
                #endregion

               
            }

            model.PopulateDropDownLists(DCC_Session.AutoDatabaseConnection, model.K_Tender);

            #region Initialise Contract Variations Grid

            if (model.IsTenderAccepted)
            {

                model.ContractVariationGrid.Initialise(
                Request,
                id: "ContractVariationsGrid",
                itemNameSingular: "Contract variation",
                column1: new GridColumn(GridColumnType.Text, "ContractVariationNo", "Variation Number", 90, alignRight: true, boundSortField: "ContractVariationNo", defaultSortColumn: true, defaultSortDescending: true),
                column2: new GridColumn(GridColumnType.Text, "ContractVariationDateStartText", "Variation Start Date", 160),
                column3: new GridColumn(GridColumnType.Text, "IgnoreIsPriceChange", "Price Changed?", 160),
                column4: new GridColumn(GridColumnType.Text, "IgnoreAgreementChanged", "Fares Agreement Changed?", 200),
                column5: new GridColumn(GridColumnType.Text, "IgnoreNoOfDaysOfOperationPerYearChanged", "Days in Operation Changed?", 200),
                addButtonText: "Add New Contract Variation",
                addActionResult: "/ContractVariation/ContractVariation",
                editActionResult: "/ContractVariation/ContractVariation",
                noOfRowsInGrid: 16,
                showEditButton: false,
                showSelectorColumn: false,
                fullRowSelect: true,
                dcc_Session: DCC_Session
                );
            }
            #endregion



            ViewBag.PageTitle = "Contract";
            return View(model);
        }

        #endregion

    }
}
