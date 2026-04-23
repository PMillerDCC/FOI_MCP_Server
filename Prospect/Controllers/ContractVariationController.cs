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
    }
}