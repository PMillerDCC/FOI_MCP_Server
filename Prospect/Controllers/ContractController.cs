using DCC_Standard;
using DocumentFormat.OpenXml.EMMA;
using ProspectBusinessLayer;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Prospect.Controllers
{
    public class ContractController : DCC_Controller
    {

        #region ContractGrid

        [Authorize]
        public ActionResult ContractGrid()
        {
            TenderGrid model = new TenderGrid(DCC_Session.AutoDatabaseConnection, awardedOnly: true);
            model.Initialise(
                Request,
                id: "ContractGrid",
                itemNameSingular: "Contract",
                column1: new GridColumn(GridColumnType.Text, "ContractReference"         , "Contract reference",  90, alignRight: true, boundSortField: "ContractReferenceSort", defaultSortColumn: true, defaultSortDescending: true),
                column2: new GridColumn(GridColumnType.Text, "TenderReference"           , "Tender reference"  ,  90, alignRight: true, boundSortField: "TenderReferenceSort"),
                column3: new GridColumn(GridColumnType.Text, "ContractorName"            , "Operator"          , 250),
                column4: new GridColumn(GridColumnType.Text, "ContractorLicenceNo"       , "Licence number"    ,  90),
                column5: new GridColumn(GridColumnType.Text, "SpecificationServiceNoText", "Service number"    , 180),
                column6: new GridColumn(GridColumnType.Text, "RouteText"                 , "Route"             , 290),
                column7: new GridColumn(GridColumnType.Text, "IsContractCancelled"       , "Is cancelled?"     , 100, trueFalse: "Yes|No"),
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
        public ActionResult Contract(bool editVariation = false)
        {
            ViewBag.EditMode = false;
            #region Determine which Contract has been selected. First looks in DCC_Session, then the URL.
            int k_Tender = -1;
            string queryKey = Request["primaryKey"] ?? "-1";
            if (int.TryParse(queryKey, out k_Tender) && (k_Tender == -999))
            {
                this.DCC_DisplayMessage(DCC_MessageType.Error, "You must select a Tender before clicking Edit", true);
                return RedirectToAction("Index");
            }
            #endregion

            Tender model = new Tender(DCC_Session.AutoDatabaseConnection, k_Tender) ;

            InitialiseContractVariationGrid(model);

            ContractVariation latestVariation = null;

            if (model.ContractVariationGrid != null && model.ContractVariationGrid.GridRows != null)
            {
                var latestRow = model.ContractVariationGrid.GridRows
                    .OfType<ContractVariation>()
                    .OrderByDescending(x => x.ContractVariationNo)
                    .FirstOrDefault();

                if (latestRow != null)
                {
                    var db = DCC_Session.AutoDatabaseConnection;
                    latestVariation = new ContractVariation(db, pK_ContractVariation: latestRow.K_ContractVariation);

                    // Always use latest saved variation as the base data for the form
                    MapTenderFromVariation(model, latestVariation);

                    model.AgreementDescription = latestVariation.AgreementDescription;
                }
            }

            if (!editVariation)
            {
                model.ContractVariationVariationDateStart = null;
                model.ContractVariationVariationDateEnd = null;
            }

            if (editVariation && latestVariation != null)
            {
                ViewBag.EditMode = true;
                ViewBag.EditingVariationId = latestVariation.K_ContractVariation;
            }

            ViewBag.NextVariationStart = GetNextVariationStartDate(model);
            ViewBag.ReturnTenderId = model.K_Tender;
            ViewBag.PageTitle = "Contract";
            return View(model);
        }

        #endregion


        #region Contract ActionResult (Post)

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contract(
            string saveButton,
            string createButton,
            string recycleButton,
            string editButton,
            Tender model,
            int? editingVariationId)
        {
            var db = DCC_Session.AutoDatabaseConnection;

            // Always repopulate anything the view needs if we have to return it
            model.PopulateDropDownLists(db, model.K_Tender);

            InitialiseContractVariationGrid(model);

            // EDIT EXISTING VARIATION
            if (!string.IsNullOrEmpty(editButton))
            {
                if (!editingVariationId.HasValue || editingVariationId.Value <= 0)
                {
                    ModelState.AddModelError("", "No variation was selected for editing.");
                }

                if (ModelState.IsValid)
                {
                    var cv = new ContractVariation(db, pK_ContractVariation: editingVariationId.Value);
                    MapVariationFieldsFromTender(cv, model);
                    cv.Save(db);

                    this.DCC_DisplayMessage(DCC_MessageType.Success, "Variation updated");
                    return RedirectToAction("Contract", new { primaryKey = model.K_Tender });
                }

                ViewBag.EditMode = true;
                ViewBag.EditingVariationId = editingVariationId ?? -1;
                ViewBag.ReturnTenderId = model.K_Tender;
                ViewBag.NextVariationStart = GetNextVariationStartDate(model);
                ViewBag.PageTitle = "Contract";
                return View(model);
            }

            // CREATE NEW VARIATION
            if (!string.IsNullOrEmpty(createButton))
            {
                if (!model.ContractVariationVariationDateStart.HasValue)
                {
                    ModelState.AddModelError("ContractVariationVariationDateStart", "Start date is required.");
                }

                if (ModelState.IsValid)
                {
                    var tenderFromDb = new Tender(db, model.K_Tender);
                    InitialiseContractVariationGrid(tenderFromDb);

                    var previousVariationRow = tenderFromDb.ContractVariationGrid.GridRows
                        .OfType<ContractVariation>()
                        .OrderByDescending(x => x.ContractVariationNo)
                        .FirstOrDefault();

                    if (previousVariationRow == null)
                    {
                        ModelState.AddModelError("", "No previous variation was found to base the new variation on.");
                    }
                    else
                    {
                        var previousVariation = new ContractVariation(
                            db,
                            pK_ContractVariation: previousVariationRow.K_ContractVariation
                        );

                        var nextVariationNo = previousVariation.ContractVariationNo + 1;

                        // Create the new variation record
                        ContractVariation.CreateNewVariation(
                            db,
                            previousVariation,
                            model.ContractVariationVariationDateStart.Value,
                            nextVariationNo
                        );

                        // Reload the tender and find the newly created variation
                        var refreshedTender = new Tender(db, model.K_Tender);
                        InitialiseContractVariationGrid(refreshedTender);

                        var newVariation = refreshedTender.ContractVariationGrid.GridRows
                            .OfType<ContractVariation>()
                            .OrderByDescending(x => x.ContractVariationNo)
                            .FirstOrDefault(x => x.ContractVariationNo == nextVariationNo);

                        if (newVariation == null)
                        {
                            ModelState.AddModelError("", "The new variation was created but could not be reloaded.");
                        }
                        else
                        {
                            var cv = new ContractVariation(db, pK_ContractVariation: newVariation.K_ContractVariation);

                            MapVariationFieldsFromTender(cv, model);
                            cv.Save(db);

                            this.DCC_DisplayMessage(DCC_MessageType.Success, "New variation created");
                            return RedirectToAction("Contract", new { primaryKey = model.K_Tender });
                        }
                    }
                }

                ViewBag.EditMode = false;
                ViewBag.EditingVariationId = -1;
                ViewBag.ReturnTenderId = model.K_Tender;
                ViewBag.NextVariationStart = GetNextVariationStartDate(model);
                ViewBag.PageTitle = "Contract";
                return View(model);
            }

            // DEFAULT RETURN
            ViewBag.EditMode = false;
            ViewBag.EditingVariationId = editingVariationId ?? -1;
            ViewBag.ReturnTenderId = model.K_Tender;
            ViewBag.NextVariationStart = GetNextVariationStartDate(model);
            ViewBag.PageTitle = "Contract";
            return View(model);
        }

        #endregion

        private void MapVariationFieldsFromTender(ContractVariation cv, Tender model)
        {
            cv.ContractVariationIsCancelled = model.ContractVariationIsCancelled;
            cv.ContractVariationIsExtended = model.ContractVariationIsExtended;
            cv.ContractVariationIsOther = model.ContractVariationIsOther;
            cv.ContractVariationOtherReason = model.ContractVariationOtherReason;

            cv.ContractVariationVariationDateStart = model.ContractVariationVariationDateStart;
            cv.ContractVariationVariationDateEnd = model.ContractVariationVariationDateEnd;

            cv.ContractVariationServiceNo = model.ContractVariationServiceNo;
            cv.ContractVariationRouteStart = model.ContractVariationRouteStart;
            cv.ContractVariationRouteEnd = model.ContractVariationRouteEnd;

            cv.ContractVariationExceptionalConditions = model.ContractVariationExceptionalConditions;
            cv.ContractVariationVehicleCapacity = model.ContractVariationVehicleCapacity;
            cv.ContractVariationVehicleElderlyAndDisabled = model.ContractVariationVehicleElderlyAndDisabled;
            cv.ContractVariationVehicleDimensions = model.ContractVariationVehicleDimensions;

            cv.ContractVariation_Q_Agreement = model.ContractVariation_Q_Agreement;
            
            cv.ContractVariationServiceDetails = model.ContractVariationServiceDetails;
            cv.ContractVariationTicketNotes = model.ContractVariationTicketNotes;
            cv.ContractVariationRouteTimetableNotes = model.ContractVariationRouteTimetableNotes;

            cv.ContractVariationOperatesOnMon = model.ContractVariationOperatesOnMon;
            cv.ContractVariationOperatesOnTue = model.ContractVariationOperatesOnTue;
            cv.ContractVariationOperatesOnWed = model.ContractVariationOperatesOnWed;
            cv.ContractVariationOperatesOnThu = model.ContractVariationOperatesOnThu;
            cv.ContractVariationOperatesOnFri = model.ContractVariationOperatesOnFri;
            cv.ContractVariationOperatesOnSat = model.ContractVariationOperatesOnSat;
            cv.ContractVariationOperatesOnSun = model.ContractVariationOperatesOnSun;

            cv.ContractVariationOperatesOnSchoolDays = model.ContractVariationOperatesOnSchoolDays;
            cv.ContractVariationOperatesOnNonSchoolDays = model.ContractVariationOperatesOnNonSchoolDays;
            cv.ContractVariationOperatesOnBankHolidays = model.ContractVariationOperatesOnBankHolidays;
            cv.ContractVariationOperatesOnChristmasAndNewYear = model.ContractVariationOperatesOnChristmasAndNewYear;

            cv.ContractVariationCalculatedNoOfDaysOfOperationFirstYear = model.ContractVariationCalculatedNoOfDaysOfOperationFirstYear;
            cv.ContractVariationCalculatedNoOfDaysOfOperationFullTerm = model.ContractVariationCalculatedNoOfDaysOfOperationFullTerm;

            cv.ContractVariationAverageMilesPerDay = model.ContractVariationAverageMilesPerDay;
            cv.ContractVariationRatePerMile = model.ContractVariationRatePerMile;
            cv.ContractVariationFuelPriceElement = model.ContractVariationFuelPriceElement;

            cv.ContractVariationAveragePricePerDay = model.ContractVariationAveragePricePerDay;
            cv.ContractVariationAveragePricePerYear = model.ContractVariationAveragePricePerYear;
            cv.ContractVariationAverageRevenuePerDay = model.ContractVariationAverageRevenuePerDay;
            cv.ContractVariationAverageRevenuePerYear = model.ContractVariationAverageRevenuePerYear;
            cv.ContractVariationAverageCostPerDay = model.ContractVariationAverageCostPerDay;
            cv.ContractVariationAverageCostPerYear = model.ContractVariationAverageCostPerYear;

            cv.ContractVariationNotes = model.ContractVariationNotes;

            cv.ContractVariationReasonForEdit = model.ContractVariationReasonForEdit;
        }

        private void InitialiseContractVariationGrid(Tender model)
        {
            if (!model.IsTenderAccepted) return;

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
                addActionResult: "/Contract/Contract?primaryKey=" + model.K_Tender,
                editActionResult: "/Contract/Contract",
                noOfRowsInGrid: 16,
                showEditButton: false,
                showSelectorColumn: false,
                fullRowSelect: true,
                dcc_Session: DCC_Session
            );
        }

        private void MapTenderFromVariation(Tender model, ContractVariation cv)
        {
            model.ContractVariationIsCancelled = cv.ContractVariationIsCancelled;
            model.ContractVariationIsExtended = cv.ContractVariationIsExtended;
            model.ContractVariationIsOther = cv.ContractVariationIsOther;
            model.ContractVariationOtherReason = cv.ContractVariationOtherReason;

            model.ContractVariationVariationDateStart = cv.ContractVariationVariationDateStart;
            model.ContractVariationVariationDateEnd = cv.ContractVariationVariationDateEnd;

            model.ContractVariationServiceNo = cv.ContractVariationServiceNo;
            model.ContractVariationRouteStart = cv.ContractVariationRouteStart;
            model.ContractVariationRouteEnd = cv.ContractVariationRouteEnd;

            model.ContractVariationExceptionalConditions = cv.ContractVariationExceptionalConditions;
            model.ContractVariationVehicleCapacity = cv.ContractVariationVehicleCapacity;
            model.ContractVariationVehicleElderlyAndDisabled = cv.ContractVariationVehicleElderlyAndDisabled;
            model.ContractVariationVehicleDimensions = cv.ContractVariationVehicleDimensions;

            model.ContractVariation_Q_Agreement = cv.ContractVariation_Q_Agreement;

            model.ContractVariationServiceDetails = cv.ContractVariationServiceDetails;
            model.ContractVariationTicketNotes = cv.ContractVariationTicketNotes;
            model.ContractVariationRouteTimetableNotes = cv.ContractVariationRouteTimetableNotes;

            model.ContractVariationOperatesOnMon = cv.ContractVariationOperatesOnMon;
            model.ContractVariationOperatesOnTue = cv.ContractVariationOperatesOnTue;
            model.ContractVariationOperatesOnWed = cv.ContractVariationOperatesOnWed;
            model.ContractVariationOperatesOnThu = cv.ContractVariationOperatesOnThu;
            model.ContractVariationOperatesOnFri = cv.ContractVariationOperatesOnFri;
            model.ContractVariationOperatesOnSat = cv.ContractVariationOperatesOnSat;
            model.ContractVariationOperatesOnSun = cv.ContractVariationOperatesOnSun;

            model.ContractVariationOperatesOnSchoolDays = cv.ContractVariationOperatesOnSchoolDays;
            model.ContractVariationOperatesOnNonSchoolDays = cv.ContractVariationOperatesOnNonSchoolDays;
            model.ContractVariationOperatesOnBankHolidays = cv.ContractVariationOperatesOnBankHolidays;
            model.ContractVariationOperatesOnChristmasAndNewYear = cv.ContractVariationOperatesOnChristmasAndNewYear;

            model.ContractVariationCalculatedNoOfDaysOfOperationFirstYear = cv.ContractVariationCalculatedNoOfDaysOfOperationFirstYear;
            model.ContractVariationCalculatedNoOfDaysOfOperationFullTerm = cv.ContractVariationCalculatedNoOfDaysOfOperationFullTerm;

            model.ContractVariationAverageMilesPerDay = cv.ContractVariationAverageMilesPerDay;
            model.ContractVariationRatePerMile = cv.ContractVariationRatePerMile;
            model.ContractVariationFuelPriceElement = cv.ContractVariationFuelPriceElement;

            model.ContractVariationAveragePricePerDay = cv.ContractVariationAveragePricePerDay;
            model.ContractVariationAveragePricePerYear = cv.ContractVariationAveragePricePerYear;
            model.ContractVariationAverageRevenuePerDay = cv.ContractVariationAverageRevenuePerDay;
            model.ContractVariationAverageRevenuePerYear = cv.ContractVariationAverageRevenuePerYear;
            model.ContractVariationAverageCostPerDay = cv.ContractVariationAverageCostPerDay;
            model.ContractVariationAverageCostPerYear = cv.ContractVariationAverageCostPerYear;

            model.ContractVariationNotes = cv.ContractVariationNotes;

            model.ContractVariationReasonForEdit = cv.ContractVariationReasonForEdit;
        }

        private DateTime? GetNextVariationStartDate(Tender model)
        {
            DateTime? lastEnd = model.ContractVariationDateEnd ?? model.DateEnd;

            if (model.IsTenderAccepted && model.ContractVariationGrid?.GridRows != null)
            {
                DateTime? best = null;
                foreach (var row in model.ContractVariationGrid.GridRows)
                {
                    var cv = row as ProspectBusinessLayer.ContractVariation;
                    if (cv == null) continue;

                    var end = cv.ContractVariationVariationDateEnd ?? cv.ContractVariationDateEnd;
                    if (end.HasValue && (!best.HasValue || end.Value > best.Value))
                        best = end;
                }
                if (best.HasValue) lastEnd = best;
            }

            return lastEnd?.AddDays(1);
        }

        public ActionResult TestUpdateVariation(int id)
        { 
            var db = DCC_Session.AutoDatabaseConnection;

            ContractVariation variation = new ContractVariation(db, pK_ContractVariation: id);

            variation.ContractVariationNotes = "UPDATE TEST " + DateTime.Now.ToString();

            variation.Save(db);

            return Content("Done");
        }

    }

}
