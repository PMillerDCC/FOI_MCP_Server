using DCC_Standard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ProspectBusinessLayer
{
    /// <summary>
    /// Service class for Annual Price Increase functionality
    /// </summary>
    public class AnnualPriceIncreaseService
    {
        /// <summary>
        /// Load accepted contracts within the specification range with their current/latest variation
        /// </summary>
        public static List<AnnualPriceIncreaseRow> LoadAcceptedContractsInRange(DCC_Database db, int specStart, int specEnd)
        {
            var rows = new List<AnnualPriceIncreaseRow>();

            // First, get all tenders and filter to accepted contracts in the specification range
            var dbf = new DCC_DatabaseFrame("sTenderList");
            dbf.AddTable("Tender");
            db.DataLayer.SelectData(dbf);

            if (!dbf.hasTables)
                return rows;

            // Filter to specification range
            var acceptedTenders = new List<Tender>();
            foreach (DataRow row in dbf.TableZeroRows)
            {
                var tender = new Tender(row);
                if (tender.IsTenderAccepted &&
                    tender.ContractReference >= specStart &&
                    tender.ContractReference <= specEnd)
                {
                    acceptedTenders.Add(tender);
                }
            }

            // For each accepted tender, get the latest variation
            foreach (var tender in acceptedTenders)
            {
                var variationList = new ContractVariationList(db, pQ_Tender: tender.K_Tender);

                if (variationList.Count == 0)
                    continue;

                // Get the variation with the highest ContractVariationNo
                var latestVariation = variationList
                    .OrderByDescending(v => v.ContractVariationNo)
                    .FirstOrDefault();

                if (latestVariation == null)
                    continue;

                // Create the row
                var row = new AnnualPriceIncreaseRow
                {
                    K_ContractVariation = latestVariation.K_ContractVariation,
                    K_Tender = tender.K_Tender,
                    ContractReference = tender.ContractReference,
                    TenderReference = tender.TenderReference,
                    ContractorName = tender.ContractorName,
                    ServiceNo = latestVariation.ContractVariationServiceNo ?? tender.SpecificationServiceNo,
                    RouteText = latestVariation.ContractVariationRouteText,
                    ContractVariationNo = latestVariation.ContractVariationNo,
                    FuelElementApplies = tender.IsFuelTerm,
                    CurrentPricePerDay = latestVariation.ContractVariationAveragePricePerDay,
                    DaysOfOperation = latestVariation.ContractVariationCalculatedNoOfDaysOfOperationFullTerm,
                    CurrentPricePerYear = latestVariation.ContractVariationAveragePricePerYear,
                    NewPricePerDay = latestVariation.ContractVariationAveragePricePerDay,
                    NewPricePerYear = latestVariation.ContractVariationAveragePricePerYear
                };

                rows.Add(row);
            }

            return rows.OrderBy(r => r.ContractReference).ThenBy(r => r.TenderReference).ToList();
        }

        /// <summary>
        /// Apply price increase to selected variations
        /// </summary>
        public static int ApplyPriceIncrease(
            DCC_Database db, 
            List<int> selectedVariationIds, 
            decimal percentageIncrease,
            int specStart,
            int specEnd,
            DateTime priceIncreaseAppliesFrom,
            string reasonForVariation)
        {
            int updatedCount = 0;

            foreach (var variationId in selectedVariationIds)
            {
                try
                {
                    // Reload the variation to ensure it's still valid
                    var variation = new ContractVariation(db, pK_ContractVariation: variationId);

                    if (variation.K_ContractVariation <= 0)
                        continue;

                    // Verify the variation still belongs to an accepted contract in range
                    var tender = new Tender(db, variation.ContractVariation_Q_Tender);

                    if (!tender.IsTenderAccepted)
                        continue;

                    if (tender.ContractReference < specStart || tender.ContractReference > specEnd)
                        continue;

                    var nextVariationNo = variation.ContractVariationNo + 1;

                    var newVariation = ContractVariation.CreateNewVariation(
                        db,
                        variation,
                        priceIncreaseAppliesFrom,
                        nextVariationNo);

                    CopyVariationDetails(variation, newVariation);

                    newVariation.ContractVariationVariationDateStart = priceIncreaseAppliesFrom;
                    newVariation.ContractVariationDateStart = priceIncreaseAppliesFrom;

                    // Calculate new prices
                    decimal multiplier = 1 + (percentageIncrease / 100m);
                    var newPricePerDay = Math.Round(newVariation.ContractVariationAveragePricePerDay * multiplier, 2);
                    var newPricePerYear = Math.Round(
                        newPricePerDay * newVariation.ContractVariationCalculatedNoOfDaysOfOperationFullTerm, 
                        2);

                    // Update the new variation
                    newVariation.ContractVariationAveragePricePerDay = newPricePerDay;
                    newVariation.ContractVariationAveragePricePerYear = newPricePerYear;

                    // Add variation reason
                    if (!string.IsNullOrWhiteSpace(reasonForVariation))
                    {
                        newVariation.ContractVariationIsOther = true;
                        newVariation.ContractVariationOtherReason = reasonForVariation;
                    }

                    // Save
                    newVariation.Save(db);
                    updatedCount++;
                }
                catch (Exception)
                {
                    // Continue with other records if one fails
                    continue;
                }
            }

            return updatedCount;
        }

        private static void CopyVariationDetails(ContractVariation source, ContractVariation target)
        {
            target.ContractVariationIsCancelled = source.ContractVariationIsCancelled;
            target.ContractVariationIsExtended = source.ContractVariationIsExtended;
            target.ContractVariationIsOther = source.ContractVariationIsOther;
            target.ContractVariationOtherReason = source.ContractVariationOtherReason;

            target.ContractVariationServiceNo = source.ContractVariationServiceNo;
            target.ContractVariationRouteStart = source.ContractVariationRouteStart;
            target.ContractVariationRouteEnd = source.ContractVariationRouteEnd;

            target.ContractVariationExceptionalConditions = source.ContractVariationExceptionalConditions;
            target.ContractVariationVehicleCapacity = source.ContractVariationVehicleCapacity;
            target.ContractVariationVehicleElderlyAndDisabled = source.ContractVariationVehicleElderlyAndDisabled;
            target.ContractVariationVehicleDimensions = source.ContractVariationVehicleDimensions;

            target.ContractVariation_Q_Agreement = source.ContractVariation_Q_Agreement;
            target.ContractVariationServiceDetails = source.ContractVariationServiceDetails;
            target.ContractVariationTicketNotes = source.ContractVariationTicketNotes;
            target.ContractVariationRouteTimetableNotes = source.ContractVariationRouteTimetableNotes;

            target.ContractVariationOperatesOnMon = source.ContractVariationOperatesOnMon;
            target.ContractVariationOperatesOnTue = source.ContractVariationOperatesOnTue;
            target.ContractVariationOperatesOnWed = source.ContractVariationOperatesOnWed;
            target.ContractVariationOperatesOnThu = source.ContractVariationOperatesOnThu;
            target.ContractVariationOperatesOnFri = source.ContractVariationOperatesOnFri;
            target.ContractVariationOperatesOnSat = source.ContractVariationOperatesOnSat;
            target.ContractVariationOperatesOnSun = source.ContractVariationOperatesOnSun;
            target.ContractVariationOperatesOnSchoolDays = source.ContractVariationOperatesOnSchoolDays;
            target.ContractVariationOperatesOnNonSchoolDays = source.ContractVariationOperatesOnNonSchoolDays;
            target.ContractVariationOperatesOnBankHolidays = source.ContractVariationOperatesOnBankHolidays;
            target.ContractVariationOperatesOnChristmasAndNewYear = source.ContractVariationOperatesOnChristmasAndNewYear;

            target.ContractVariationCalculatedNoOfDaysOfOperationFirstYear = source.ContractVariationCalculatedNoOfDaysOfOperationFirstYear;
            target.ContractVariationCalculatedNoOfDaysOfOperationFullTerm = source.ContractVariationCalculatedNoOfDaysOfOperationFullTerm;

            target.ContractVariationAverageMilesPerDay = source.ContractVariationAverageMilesPerDay;
            target.ContractVariationRatePerMile = source.ContractVariationRatePerMile;
            target.ContractVariationFuelPriceElement = source.ContractVariationFuelPriceElement;
            target.ContractVariationAveragePricePerDay = source.ContractVariationAveragePricePerDay;
            target.ContractVariationAveragePricePerYear = source.ContractVariationAveragePricePerYear;
            target.ContractVariationAverageRevenuePerDay = source.ContractVariationAverageRevenuePerDay;
            target.ContractVariationAverageRevenuePerYear = source.ContractVariationAverageRevenuePerYear;
            target.ContractVariationAverageCostPerDay = source.ContractVariationAverageCostPerDay;
            target.ContractVariationAverageCostPerYear = source.ContractVariationAverageCostPerYear;

            target.ContractVariationNotes = source.ContractVariationNotes;
            target.ContractVariationReasonForEdit = source.ContractVariationReasonForEdit;
        }
    }
}
