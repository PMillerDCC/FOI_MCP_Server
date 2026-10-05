using System;
using DCC_Standard;
using ProspectBusinessLayer;
using System.Web.Mvc;

namespace Prospect.Controllers
{
    public class ContractVariationController : DCC_Controller
    {
        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id, int? returnTenderId)
        {
            var db = DCC_Session.AutoDatabaseConnection;
            var cv = new ContractVariation(db, pK_ContractVariation: id);

            ViewBag.ReturnTenderId = returnTenderId ?? cv.ContractVariation_Q_Tender;

            return View(cv);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ContractVariation model, int returnTenderId)
        {
            var db = DCC_Session.AutoDatabaseConnection;
            var cv = new ContractVariation(db);

            cv.ContractVariation_Q_Tender = returnTenderId;

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

            cv.ContractVariationOperatesOnBankHolidays = model.ContractVariationOperatesOnBankHolidays;
            cv.ContractVariationOperatesOnSchoolDays = model.ContractVariationOperatesOnSchoolDays;
            cv.ContractVariationOperatesOnNonSchoolDays = model.ContractVariationOperatesOnNonSchoolDays;
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
            cv.Save(db);

            this.DCC_DisplayMessage(DCC_MessageType.Success, "Variation created");
            return RedirectToAction("Contract", "Contract", new { primaryKey = returnTenderId });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ContractVariation model, int returnTenderId)
        {
            var db = DCC_Session.AutoDatabaseConnection;
            var cv = new ContractVariation(db, pK_ContractVariation: model.K_ContractVariation);

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

            cv.ContractVariationOperatesOnBankHolidays = model.ContractVariationOperatesOnBankHolidays;
            cv.ContractVariationOperatesOnSchoolDays = model.ContractVariationOperatesOnSchoolDays;
            cv.ContractVariationOperatesOnNonSchoolDays = model.ContractVariationOperatesOnNonSchoolDays;
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

            cv.Save(db);

            this.DCC_DisplayMessage(DCC_MessageType.Success, "Variation updated");
            return RedirectToAction("Contract", "Contract", new { primaryKey = returnTenderId });
        }

        #region Annual Price Increase

        [Authorize]
        [HttpGet]
        public ActionResult AnnualPriceIncrease()
        {
            var model = new AnnualPriceIncreaseModel();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AnnualPriceIncrease(AnnualPriceIncreaseModel model, string action)
        {
            var db = DCC_Session.AutoDatabaseConnection;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Validate range
            if (model.SpecStart > model.SpecEnd)
            {
                ModelState.AddModelError("SpecStart", "Specification Start must be less than or equal to Specification End");
                return View(model);
            }

            if (action == "Search")
            {
                // Load data
                model.Rows = AnnualPriceIncreaseService.LoadAcceptedContractsInRange(db, model.SpecStart.Value, model.SpecEnd.Value);
                model.HasSearched = true;

                if (model.Rows.Count == 0)
                {
                    this.DCC_DisplayMessage(DCC_MessageType.Information, "No accepted contracts found in the specified range");
                }

                return View(model);
            }
            else if (action == "Calculate")
            {
                // Reload rows
                model.Rows = AnnualPriceIncreaseService.LoadAcceptedContractsInRange(db, model.SpecStart.Value, model.SpecEnd.Value);
                model.HasSearched = true;

                if (!model.IncreasePercentage.HasValue)
                {
                    ModelState.AddModelError("IncreasePercentage", "Please enter a percentage increase");
                    return View(model);
                }

                // Calculate new prices for all rows
                foreach (var row in model.Rows)
                {
                    row.CalculateNewPrices(model.IncreasePercentage.Value);
                }

                return View(model);
            }
            else if (action == "Apply")
            {
                // Reload rows and recalculate
                model.Rows = AnnualPriceIncreaseService.LoadAcceptedContractsInRange(db, model.SpecStart.Value, model.SpecEnd.Value);

                if (!model.IncreasePercentage.HasValue)
                {
                    ModelState.AddModelError("IncreasePercentage", "Please enter a percentage increase");
                    model.HasSearched = true;
                    return View(model);
                }

                if (!model.PriceIncreaseAppliesFrom.HasValue)
                {
                    ModelState.AddModelError("PriceIncreaseAppliesFrom", "Please enter the date the price increase applies from");
                    model.HasSearched = true;
                    return View(model);
                }

                // Recalculate
                foreach (var row in model.Rows)
                {
                    row.CalculateNewPrices(model.IncreasePercentage.Value);
                }

                // Validate selections
                if (model.SelectedVariationIds == null || model.SelectedVariationIds.Count == 0)
                {
                    ModelState.AddModelError("", "Please select at least one contract variation to apply the price increase");
                    model.HasSearched = true;
                    return View(model);
                }

                // Apply increases
                string reasonForVariation = "Annual price increase";
                int updatedCount = AnnualPriceIncreaseService.ApplyPriceIncrease(
                    db, 
                    model.SelectedVariationIds, 
                    model.IncreasePercentage.Value,
                    model.SpecStart.Value,
                    model.SpecEnd.Value,
                    model.PriceIncreaseAppliesFrom.Value,
                    reasonForVariation);

                this.DCC_DisplayMessage(DCC_MessageType.Success, $"Price increase applied to {updatedCount} contract variation(s)");

                // Reset for new search
                model = new AnnualPriceIncreaseModel
                {
                    SpecStart = model.SpecStart,
                    SpecEnd = model.SpecEnd
                };
                return View(model);
            }

            return View(model);
        }

        #endregion
    }
}