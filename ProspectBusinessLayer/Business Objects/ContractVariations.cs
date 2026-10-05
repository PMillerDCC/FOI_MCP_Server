using DCC_Standard;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ProspectBusinessLayer
{

    #region ContractVariation

    public class ContractVariation
    {

        // Properties

        public int       K_ContractVariation                                     { get; set; }
        public int       ContractVariation_Q_Tender                              { get; set; }
        public int       ContractVariationNo                                     { get; set; }
        public string    ContractVariationNoText                                 { get { return ContractVariationNo.ToString().PadLeft(2, '0'); } }
        public bool      ContractVariationIsCancelled                            { get; set; }
        public bool      ContractVariationIsExtended                             { get; set; }
        public bool      ContractVariationIsOther                                { get; set; }
        public string    ContractVariationOtherReason                            { get; set; }
        public string    ContractVariationReasonText
        {
            get
            {
                var parts = new List<string>();
                if (ContractVariationIsExtended) parts.Add("Extended");
                if (ContractVariationIsOther)
                {
                    if (!string.IsNullOrWhiteSpace(ContractVariationOtherReason))
                        parts.Add($"{ContractVariationOtherReason}");
                    else
                        parts.Add("Other");
                }
                return parts.Count == 0 ? "-" : string.Join(", ", parts);
            }
        }
        public DateTime? ContractVariationDateStart                              { get; set; }
        public string    ContractVariationDateStartText                          { get { return DCC_Formatting.NullDateTxt(ContractVariationDateStart, "dd MMM yyyy"); } }
        public string    ContractVariationDateStartSort                          { get { return DCC_Formatting.NullDateTxt(ContractVariationDateStart, "yyyyMMddHHmm"); } }
        public string    ContractVariationDateStartFullText                      { get { return DCC_Formatting.NullDateTxt(ContractVariationDateStart, "dd MMMM yyyy"); } }
        public DateTime? ContractVariationDateEnd                                { get; set; }
        public string    ContractVariationDateEndText                            { get { return DCC_Formatting.NullDateTxt(ContractVariationDateEnd, "dd MMM yyyy"); } }
        public string    ContractVariationDateEndSort                            { get { return DCC_Formatting.NullDateTxt(ContractVariationDateEnd, "yyyyMMddHHmm"); } }
        public string    ContractVariationDateEndFullText                        { get { return DCC_Formatting.NullDateTxt(ContractVariationDateEnd, "dd MMMM yyyy"); } }
        public string    ContractVariationDatesText                              { get { return (string.IsNullOrEmpty(ContractVariationDateStartText) ? "- not set -" : ContractVariationDateStartText + " to " + ContractVariationDateEndText); } }
        public string    ContractVariationServiceNo                              { get; set; }
        public string    ContractVariationRouteStart                             { get; set; }
        public string    ContractVariationRouteEnd                               { get; set; }
        public string    ContractVariationRouteText                              { get { return (string.IsNullOrEmpty(ContractVariationRouteStart) ? "- not set -" : ContractVariationRouteStart + " to " + ContractVariationRouteEnd); } }
        public string    ContractVariationExceptionalConditions                  { get; set; }

        public string    ContractVariationVehicleCapacity                        { get; set; }
        public string    ContractVariationVehicleElderlyAndDisabled              { get; set; }
        public string    ContractVariationVehicleDimensions                      { get; set; }

        public int?      ContractVariation_Q_Agreement                           { get; set; }
        public string    ContractVariationServiceDetails                         { get; set; }
        public string    ContractVariationTicketNotes                            { get; set; }
        public string    ContractVariationRouteTimetableNotes                    { get; set; }

        public bool      ContractVariationOperatesOnMon                          { get; set; }
        public bool      ContractVariationOperatesOnTue                          { get; set; }
        public bool      ContractVariationOperatesOnWed                          { get; set; }
        public bool      ContractVariationOperatesOnThu                          { get; set; }
        public bool      ContractVariationOperatesOnFri                          { get; set; }
        public bool      ContractVariationOperatesOnSat                          { get; set; }
        public bool      ContractVariationOperatesOnSun                          { get; set; }
        public bool      ContractVariationOperatesOnSchoolDays                   { get; set; }
        public bool      ContractVariationOperatesOnNonSchoolDays                { get; set; }
        public bool      ContractVariationOperatesOnBankHolidays                 { get; set; }
        public bool      ContractVariationOperatesOnChristmasAndNewYear          { get; set; }
        public string    ContractVariationOperatesOnMonText                      { get { return (ContractVariationOperatesOnMon ? "Y" : "N"); } }
        public string    ContractVariationOperatesOnTueText                      { get { return (ContractVariationOperatesOnTue ? "Y" : "N"); } }
        public string    ContractVariationOperatesOnWedText                      { get { return (ContractVariationOperatesOnWed ? "Y" : "N"); } }
        public string    ContractVariationOperatesOnThuText                      { get { return (ContractVariationOperatesOnThu ? "Y" : "N"); } }
        public string    ContractVariationOperatesOnFriText                      { get { return (ContractVariationOperatesOnFri ? "Y" : "N"); } }
        public string    ContractVariationOperatesOnSatText                      { get { return (ContractVariationOperatesOnSat ? "Y" : "N"); } }
        public string    ContractVariationOperatesOnSunText                      { get { return (ContractVariationOperatesOnSun ? "Y" : "N"); } }
        public string    ContractVariationOperatesOnSchoolDaysText               { get { return (ContractVariationOperatesOnSchoolDays ? "Y" : "N"); } }
        public string    ContractVariationOperatesOnNonSchoolDaysText            { get { return (ContractVariationOperatesOnNonSchoolDays ? "Y" : "N"); } }
        public string    ContractVariationOperatesOnBankHolidaysText             { get { return (ContractVariationOperatesOnBankHolidays ? "Y" : "N"); } }
        public string    ContractVariationOperatesOnChristmasAndNewYearText      { get { return (ContractVariationOperatesOnChristmasAndNewYear ? "Y" : "N"); } }
        public int       ContractVariationCalculatedNoOfDaysOfOperationFirstYear { get; set; }
        public int       ContractVariationCalculatedNoOfDaysOfOperationFullTerm  { get; set; }

        public decimal   ContractVariationAverageMilesPerDay                     { get; set; }
        public decimal   ContractVariationRatePerMile                            { get; set; }
        public decimal   ContractVariationFuelPriceElement                       { get; set; }
        public decimal   ContractVariationAveragePricePerDay                     { get; set; }
        public decimal   ContractVariationAveragePricePerYear                    { get; set; }
        public decimal   ContractVariationAverageRevenuePerDay                   { get; set; }
        public decimal   ContractVariationAverageRevenuePerYear                  { get; set; }
        public decimal   ContractVariationAverageCostPerDay                      { get; set; }
        public decimal   ContractVariationAverageCostPerYear                     { get; set; }

        public DateTime? ContractVariationVariationDateStart                     { get; set; }
        public string    ContractVariationVariationDateStartText                 { get { return DCC_Formatting.NullDateTxt(ContractVariationVariationDateStart, "dd MMM yyyy"); } }
        public string    ContractVariationVariationDateStartSort                 { get { return DCC_Formatting.NullDateTxt(ContractVariationVariationDateStart, "yyyyMMddHHmm"); } }
        public DateTime? ContractVariationVariationDateEnd                       { get; set; }
        public string    ContractVariationVariationDateEndText                   { get { return DCC_Formatting.NullDateTxt(ContractVariationVariationDateEnd, "dd MMM yyyy"); } }
        public string    ContractVariationVariationDateEndSort                   { get { return DCC_Formatting.NullDateTxt(ContractVariationVariationDateEnd, "yyyyMMddHHmm"); } }

        public string    ContractVariationNotes                                  { get; set; }

        public int       ContractReference                                       { get; set; }
        public string    ContractReferenceText                                   { get { return ContractReference.ToString().PadLeft(5, '0'); } }
        public string    ContractReferenceSort                                   { get { return ContractReference.ToString().PadLeft(5, '0'); } }
        public string    ContractTypeDescription                                 { get; set; }
        public string    AgreementDescription                                    { get; set; }
        public string    ContractorName                                          { get; set; }
        public string    ContractorLicenceNo                                     { get; set; }
        public int       TenderReference                                         { get; set; }
        public string    TenderReferenceText                                     { get { return TenderReference.ToString().PadLeft(5, '0'); } }
        public string    TenderReferenceSort                                     { get { return TenderReference.ToString().PadLeft(5, '0'); } }
        public string    ContractVariationReasonForEdit                          { get; set; }

        public string    PackageServiceNumbers
        {
            get
            {
                // Load the Tender properly using the correct constructor
                using (var db = new DCC_Database("SQLSERVER", "Prospect", "Prospect"))
                {
                    var tender = new Tender(db, this.ContractVariation_Q_Tender);

                    // If the tender is not a package, return the variation's own service number
                    if (!tender.IsPackage)
                        return this.ContractVariationServiceNo;

                    // If it IS a package, aggregate service numbers from child specifications
                    var numbers = new List<string>();

                    var dbf = new DCC_DatabaseFrame("sChildSpecificationsByTenderReference");
                    dbf.AddTable("Specification");
                    dbf.AddParameter("TenderReference", tender.TenderReference);

                    db.DataLayer.SelectData(dbf);

                    if (dbf.hasTables)
                    {
                        foreach (DataRow row in dbf.TableZeroRows)
                        {
                            var serviceNo = row["SpecificationServiceNo"]?.ToString();

                            if (!string.IsNullOrEmpty(serviceNo) && !numbers.Contains(serviceNo))
                                numbers.Add(serviceNo);
                        }
                    }

                    // If no child service numbers found, fall back to the variation's own service number
                    if (numbers.Count == 0)
                        return this.ContractVariationServiceNo;

                    return string.Join(", ", numbers);
                }
            }
        }

        public string PackageRouteText
        {
            get
            {
                using (var db = new DCC_Database("SQLSERVER", "Prospect", "Prospect"))
                {
                    var tender = new Tender(db, this.ContractVariation_Q_Tender);

                    if (tender.IsPackage)
                        return "Package";

                    return this.ContractVariationRouteText;
                }
            }
        }


        // Utilities

        public int PrimaryKey { get { return K_ContractVariation; } set { K_ContractVariation = value; } }
        public string Key { get { return PrimaryKey.ToString(); } }
        public bool IsNew { get { return PrimaryKey == -1; } }
        public override string ToString() { return PrimaryKey.ToString().PadLeft(3) + " - " + ContractVariationNoText; }

        //public string ContractVariationAgreementDescription => ddlAgreement?.FirstOrDefault(XmlReadMode => XmlReadMode.Value == ContractVariation_Q_Agreement?.ToString())?.Text;

        // Calendar

        public List<CalendarDay> Calendar = new List<CalendarDay>();
        public IEnumerable<DateTime> EachDay(DateTime pStartDate, DateTime pEndDate) { for (var day = pStartDate.Date; day.Date <= pEndDate.Date; day = day.AddDays(1)) yield return day; }



        // Constructors

        public ContractVariation()
        {
            K_ContractVariation = -1;
        }

        public ContractVariation(DataRow dataRow)
        {
            GetColumns(dataRow);
        }

        public ContractVariation(DCC_Database db, int pK_ContractVariation = -999, DateTime? pDateStart = null, DateTime? pDateEnd = null, int pK_Tender = -999)
        {
            if (pK_ContractVariation == -1) // create a new instance of ContractVariation and set defaults
            {
                K_ContractVariation = -1;
                ContractVariation_Q_Tender = pK_Tender;
            }

            else if (pK_ContractVariation > 0) // read a record from the database for the given K_ContractVariation
            {
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sContractVariation");
                dbf.AddTable("ContractVariation");
                dbf.AddParameter("K_ContractVariation", pK_ContractVariation);
                db.DataLayer.SelectData(dbf);
                if (dbf.hasTables && dbf.TableZeroRows.Count == 1) GetColumns(dbf.RowZero);
            }

            else if (pK_Tender > 0) 
            {
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sContractVariationByTender");
                dbf.AddTable("ContractVariation");
                dbf.AddParameter("ContractVariation_Q_Tender", pK_Tender);
                db.DataLayer.SelectData(dbf);
                if (dbf.hasTables && dbf.TableZeroRows.Count == 1) GetColumns(dbf.RowZero);
            }
        }


        // Methods


        public bool GetColumns(DataRow dataRow)
        {
            if (dataRow == null) return false;

            K_ContractVariation                                     = DCC_GetColumn.Integer32(dataRow,          "K_ContractVariation");
            ContractVariation_Q_Tender                              = DCC_GetColumn.Integer32(dataRow,          "ContractVariation_Q_Tender");
            ContractVariationNo                                     = DCC_GetColumn.Integer32(dataRow,          "ContractVariationNo");

            ContractVariationIsExtended                             = DCC_GetColumn.Bit(dataRow,                "ContractVariationIsExtended");
            ContractVariationIsCancelled                            = DCC_GetColumn.Bit(dataRow,                "ContractVariationIsCancelled");
            ContractVariationIsOther                                = DCC_GetColumn.Bit(dataRow,                "ContractVariationIsOther");
            ContractVariationOtherReason                            = DCC_GetColumn.String(dataRow,             "ContractVariationOtherReason");
            ContractVariationDateStart                              = DCC_GetColumn.DateTime(dataRow,           "ContractVariationDateStart");
            ContractVariationDateEnd                                = DCC_GetColumn.DateTime(dataRow,           "ContractVariationDateEnd");
            ContractVariationServiceNo                              = DCC_GetColumn.String(dataRow,             "ContractVariationServiceNo");
            ContractVariationRouteStart                             = DCC_GetColumn.String(dataRow,             "ContractVariationRouteStart");
            ContractVariationRouteEnd                               = DCC_GetColumn.String(dataRow,             "ContractVariationRouteEnd");

            ContractVariationExceptionalConditions                  = DCC_GetColumn.String(dataRow,             "ContractVariationExceptionalConditions");
            ContractVariationVehicleCapacity                        = DCC_GetColumn.String(dataRow,             "ContractVariationVehicleCapacity");
            ContractVariationVehicleElderlyAndDisabled              = DCC_GetColumn.String(dataRow,             "ContractVariationVehicleElderlyAndDisabled");
            ContractVariationVehicleDimensions                      = DCC_GetColumn.String(dataRow,             "ContractVariationVehicleDimensions");

            ContractVariation_Q_Agreement                           = DCC_GetColumn.Integer32(dataRow,          "ContractVariation_Q_Agreement");
            ContractVariationServiceDetails                         = DCC_GetColumn.String(dataRow,             "ContractVariationServiceDetails");
            ContractVariationTicketNotes                            = DCC_GetColumn.String(dataRow,             "ContractVariationTicketNotes");
            ContractVariationRouteTimetableNotes                    = DCC_GetColumn.String(dataRow,             "ContractVariationRouteTimetableNotes");

            ContractVariationOperatesOnMon                          = DCC_GetColumn.Bit(dataRow,                "ContractVariationOperatesOnMon");
            ContractVariationOperatesOnTue                          = DCC_GetColumn.Bit(dataRow,                "ContractVariationOperatesOnTue");
            ContractVariationOperatesOnWed                          = DCC_GetColumn.Bit(dataRow,                "ContractVariationOperatesOnWed");
            ContractVariationOperatesOnThu                          = DCC_GetColumn.Bit(dataRow,                "ContractVariationOperatesOnThu");
            ContractVariationOperatesOnFri                          = DCC_GetColumn.Bit(dataRow,                "ContractVariationOperatesOnFri");
            ContractVariationOperatesOnSat                          = DCC_GetColumn.Bit(dataRow,                "ContractVariationOperatesOnSat");
            ContractVariationOperatesOnSun                          = DCC_GetColumn.Bit(dataRow,                "ContractVariationOperatesOnSun");
            ContractVariationOperatesOnSchoolDays                   = DCC_GetColumn.Bit(dataRow,                "ContractVariationOperatesOnSchoolDays");
            ContractVariationOperatesOnNonSchoolDays                = DCC_GetColumn.Bit(dataRow,                "ContractVariationOperatesOnNonSchoolDays");
            ContractVariationOperatesOnBankHolidays                 = DCC_GetColumn.Bit(dataRow,                "ContractVariationOperatesOnBankHolidays");
            ContractVariationOperatesOnChristmasAndNewYear          = DCC_GetColumn.Bit(dataRow,                "ContractVariationOperatesOnChristmasAndNewYear");
            ContractVariationCalculatedNoOfDaysOfOperationFirstYear = DCC_GetColumn.Integer32(dataRow,          "ContractVariationCalculatedNoOfDaysOfOperationFirstYear");
            ContractVariationCalculatedNoOfDaysOfOperationFullTerm  = DCC_GetColumn.Integer32(dataRow,          "ContractVariationCalculatedNoOfDaysOfOperationFullTerm");

            ContractVariationAverageMilesPerDay                     = Math.Round(DCC_GetColumn.Decimal(dataRow, "ContractVariationAverageMilesPerDay"), 1);
            ContractVariationRatePerMile                            = Math.Round(DCC_GetColumn.Decimal(dataRow, "ContractVariationRatePerMile"), 2);
            ContractVariationFuelPriceElement                       = Math.Round(DCC_GetColumn.Decimal(dataRow, "ContractVariationFuelPriceElement"), 2);
            ContractVariationAveragePricePerDay                     = Math.Round(DCC_GetColumn.Decimal(dataRow, "ContractVariationAveragePricePerDay"), 2);
            ContractVariationAveragePricePerYear                    = Math.Round(DCC_GetColumn.Decimal(dataRow, "ContractVariationAveragePricePerYear"), 2);
            ContractVariationAverageRevenuePerDay                   = Math.Round(DCC_GetColumn.Decimal(dataRow, "ContractVariationAverageRevenuePerDay"), 2);
            ContractVariationAverageRevenuePerYear                  = Math.Round(DCC_GetColumn.Decimal(dataRow, "ContractVariationAverageRevenuePerYear"), 2);
            ContractVariationAverageCostPerDay                      = Math.Round(DCC_GetColumn.Decimal(dataRow, "ContractVariationAverageCostPerDay"), 2);
            ContractVariationAverageCostPerYear                     = Math.Round(DCC_GetColumn.Decimal(dataRow, "ContractVariationAverageCostPerYear"), 2);

            ContractVariationVariationDateStart                     = DCC_GetColumn.DateTimeQ(dataRow,          "ContractVariationVariationDateStart");
            var rawEnd = DCC_GetColumn.DateTimeQ(dataRow, "ContractVariationVariationDateEnd");
            ContractVariationVariationDateEnd = (rawEnd == new DateTime(1900, 1, 1) ? null : rawEnd);

            ContractVariationNotes                                  = DCC_GetColumn.String(dataRow,             "ContractVariationNotes");
            ContractVariationReasonForEdit = DCC_GetColumn.String(dataRow, "ContractVariationReasonForEdit");

            if (DCC_GetColumn.Exists(dataRow, "ContractReference"))                         ContractReference                       = DCC_GetColumn.Integer32(dataRow, "ContractReference");
            if (DCC_GetColumn.Exists(dataRow, "ContractTypeDescription"))                   ContractTypeDescription                 = DCC_GetColumn.String(dataRow,    "ContractTypeDescription");
            if (DCC_GetColumn.Exists(dataRow, "ContractorName"))                            ContractorName                          = DCC_GetColumn.String(dataRow,    "ContractorName");
            if (DCC_GetColumn.Exists(dataRow, "ContractorLicenceNo"))                       ContractorLicenceNo                     = DCC_GetColumn.String(dataRow,    "ContractorLicenceNo");
            if (DCC_GetColumn.Exists(dataRow, "TenderReference"))                           TenderReference                         = DCC_GetColumn.Integer32(dataRow, "TenderReference");
            if (DCC_GetColumn.Exists(dataRow, "AgreementDescription"))                      AgreementDescription                    = DCC_GetColumn.String(dataRow,    "AgreementDescription");

            return true;
        }

        public int Save(DCC_Database db)
        {
            return (IsNew ? Insert(db) : Update(db));
        }

        public static ContractVariation CreateNewVariation(
            DCC_Database db,
            ContractVariation previousVariation,
            DateTime newStartDate,
            int newVariationNo)
        {
            // 1. Create a new variation object
            var newVar = new ContractVariation();
            newVar.ContractVariation_Q_Tender = previousVariation.ContractVariation_Q_Tender;

            // 1b. Set the variation number
            newVar.ContractVariationNo = newVariationNo;

            // 2. Set the start date (variation-level and contract-level)
            newVar.ContractVariationVariationDateStart = newStartDate;
            newVar.ContractVariationDateStart = newStartDate;

            // 3. Default the end date to the contract end date (Variation 00)
            newVar.ContractVariationVariationDateEnd = previousVariation.ContractVariationVariationDateEnd;
            newVar.ContractVariationDateEnd = previousVariation.ContractVariationDateEnd;

            // 4. Shorten variation 00 to end the day before the new variation starts
            previousVariation.ContractVariationVariationDateEnd = newStartDate.AddDays(-1);
            previousVariation.ContractVariationDateEnd = newStartDate.AddDays(-1);

            // 5. Save both
            previousVariation.Save(db);
            newVar.Save(db);

            return newVar;
        }

        int Insert(DCC_Database db)
        {
            bool isFirst = (ContractVariationNo == 0);

            if (isFirst)
            {
                var dbf = new DCC_DatabaseFrame("iFirstContractVariation");
                dbf.AddTable("ContractVariation");

                dbf.AddParameter("ContractVariation_Q_Tender", ContractVariation_Q_Tender);
                dbf.AddParameter("ContractVariationIsCancelled", ContractVariationIsCancelled);
                dbf.AddParameter("ContractVariationIsExtended", ContractVariationIsExtended);
                dbf.AddParameter("ContractVariationIsOther", ContractVariationIsOther);
                dbf.AddParameter("ContractVariationOtherReason", ContractVariationOtherReason, 500);
                dbf.AddParameter("ContractVariationDateStart", ContractVariationDateStart);
                dbf.AddParameter("ContractVariationDateEnd", ContractVariationDateEnd);
                dbf.AddParameter("ContractVariationServiceNo", ContractVariationServiceNo, 500);
                dbf.AddParameter("ContractVariationRouteStart", ContractVariationRouteStart, 500);
                dbf.AddParameter("ContractVariationRouteEnd", ContractVariationRouteEnd, 500);
                dbf.AddParameter("ContractVariationExceptionalConditions", ContractVariationExceptionalConditions, 500);
                dbf.AddParameter("ContractVariationVehicleCapacity", ContractVariationVehicleCapacity, 500);
                dbf.AddParameter("ContractVariationVehicleElderlyAndDisabled", ContractVariationVehicleElderlyAndDisabled, 500);
                dbf.AddParameter("ContractVariationVehicleDimensions", ContractVariationVehicleDimensions, 500);
                dbf.AddParameter("ContractVariation_Q_Agreement", ContractVariation_Q_Agreement);
                dbf.AddParameter("ContractVariationServiceDetails", ContractVariationServiceDetails, 500);
                dbf.AddParameter("ContractVariationTicketNotes", ContractVariationTicketNotes, 500);
                dbf.AddParameter("ContractVariationRouteTimetableNotes", ContractVariationRouteTimetableNotes, 500);

                dbf.AddParameter("ContractVariationOperatesOnMon", ContractVariationOperatesOnMon);
                dbf.AddParameter("ContractVariationOperatesOnTue", ContractVariationOperatesOnTue);
                dbf.AddParameter("ContractVariationOperatesOnWed", ContractVariationOperatesOnWed);
                dbf.AddParameter("ContractVariationOperatesOnThu", ContractVariationOperatesOnThu);
                dbf.AddParameter("ContractVariationOperatesOnFri", ContractVariationOperatesOnFri);
                dbf.AddParameter("ContractVariationOperatesOnSat", ContractVariationOperatesOnSat);
                dbf.AddParameter("ContractVariationOperatesOnSun", ContractVariationOperatesOnSun);
                dbf.AddParameter("ContractVariationOperatesOnSchoolDays", ContractVariationOperatesOnSchoolDays);
                dbf.AddParameter("ContractVariationOperatesOnNonSchoolDays", ContractVariationOperatesOnNonSchoolDays);
                dbf.AddParameter("ContractVariationOperatesOnBankHolidays", ContractVariationOperatesOnBankHolidays);
                dbf.AddParameter("ContractVariationOperatesOnChristmasAndNewYear", ContractVariationOperatesOnChristmasAndNewYear);
                dbf.AddParameter("ContractVariationCalculatedNoOfDaysOfOperationFirstYear", ContractVariationCalculatedNoOfDaysOfOperationFirstYear);
                dbf.AddParameter("ContractVariationCalculatedNoOfDaysOfOperationFullTerm", ContractVariationCalculatedNoOfDaysOfOperationFullTerm);

                dbf.AddParameter("ContractVariationAverageMilesPerDay", ContractVariationAverageMilesPerDay);
                dbf.AddParameter("ContractVariationRatePerMile", ContractVariationRatePerMile);
                dbf.AddParameter("ContractVariationFuelPriceElement", ContractVariationFuelPriceElement);
                dbf.AddParameter("ContractVariationAveragePricePerDay", ContractVariationAveragePricePerDay);
                dbf.AddParameter("ContractVariationAveragePricePerYear", ContractVariationAveragePricePerYear);
                dbf.AddParameter("ContractVariationAverageRevenuePerDay", ContractVariationAverageRevenuePerDay);
                dbf.AddParameter("ContractVariationAverageRevenuePerYear", ContractVariationAverageRevenuePerYear);
                dbf.AddParameter("ContractVariationAverageCostPerDay", ContractVariationAverageCostPerDay);
                dbf.AddParameter("ContractVariationAverageCostPerYear", ContractVariationAverageCostPerYear);

                dbf.AddParameter("ContractVariationVariationDateStart", ContractVariationVariationDateStart);
                dbf.AddParameter("ContractVariationVariationDateEnd", ContractVariationVariationDateEnd);

                dbf.AddParameter("ContractVariationNotes", ContractVariationNotes, 500);

                dbf.AddParameter("ContractVariationReasonForEdit", ContractVariationReasonForEdit);

                dbf.AddParameter("_PRIMARYKEY", DCC_DatabaseFrame.OUTINTEGER);

                int newKey = db.DataLayer.NonQueryPK(dbf);
                if (newKey > -1) K_ContractVariation = newKey;
                return newKey;
            }
            else
            {
                var dbf = new DCC_DatabaseFrame("iContractVariation");
                dbf.AddTable("ContractVariation");
                SetCommonParameters(dbf, includeVariationDates: true);
                dbf.AddParameter("_PRIMARYKEY", DCC_DatabaseFrame.OUTINTEGER);

                int newKey = db.DataLayer.NonQueryPK(dbf);
                if (newKey > -1) K_ContractVariation = newKey;
                return newKey;
            }
        }

        int Update(DCC_Database db)
        {
            
            var dbf = new DCC_DatabaseFrame("uContractVariation");
            dbf.AddTable("ContractVariation");

            dbf.AddParameter("K_ContractVariation", K_ContractVariation);
            SetCommonParameters(dbf);

            db.DataLayer.NonQuery(dbf);
            return (dbf.OK ? K_ContractVariation : -1);
        }

        void SetCommonParameters(DCC_DatabaseFrame dbf, bool includeVariationDates = true)
        {
            dbf.AddParameter("ContractVariation_Q_Tender"                             , ContractVariation_Q_Tender);

            dbf.AddParameter("ContractVariationIsCancelled"                           , ContractVariationIsCancelled);
            dbf.AddParameter("ContractVariationIsExtended"                            , ContractVariationIsExtended);
            dbf.AddParameter("ContractVariationIsOther"                               , ContractVariationIsOther);
            dbf.AddParameter("ContractVariationOtherReason"                           , ContractVariationOtherReason, 500);
            dbf.AddParameter("ContractVariationDateStart"                             , ContractVariationDateStart);
            dbf.AddParameter("ContractVariationDateEnd"                               , ContractVariationDateEnd);
            dbf.AddParameter("ContractVariationServiceNo"                             , ContractVariationServiceNo, 500);
            dbf.AddParameter("ContractVariationRouteStart"                            , ContractVariationRouteStart, 500);
            dbf.AddParameter("ContractVariationRouteEnd"                              , ContractVariationRouteEnd, 500);

            dbf.AddParameter("ContractVariationExceptionalConditions"                 , ContractVariationExceptionalConditions, 500);
            dbf.AddParameter("ContractVariationVehicleCapacity"                       , ContractVariationVehicleCapacity, 500);
            dbf.AddParameter("ContractVariationVehicleElderlyAndDisabled"             , ContractVariationVehicleElderlyAndDisabled, 500);
            dbf.AddParameter("ContractVariationVehicleDimensions"                     , ContractVariationVehicleDimensions, 500);

            dbf.AddParameter("ContractVariation_Q_Agreement"                          , ContractVariation_Q_Agreement);
            dbf.AddParameter("ContractVariationServiceDetails"                        , ContractVariationServiceDetails, 500);
            dbf.AddParameter("ContractVariationTicketNotes"                           , ContractVariationTicketNotes, 500);
            dbf.AddParameter("ContractVariationRouteTimetableNotes"                   , ContractVariationRouteTimetableNotes, 500);
            

            dbf.AddParameter("ContractVariationOperatesOnMon"                         , ContractVariationOperatesOnMon);
            dbf.AddParameter("ContractVariationOperatesOnTue"                         , ContractVariationOperatesOnTue);
            dbf.AddParameter("ContractVariationOperatesOnWed"                         , ContractVariationOperatesOnWed);
            dbf.AddParameter("ContractVariationOperatesOnThu"                         , ContractVariationOperatesOnThu);
            dbf.AddParameter("ContractVariationOperatesOnFri"                         , ContractVariationOperatesOnFri);
            dbf.AddParameter("ContractVariationOperatesOnSat"                         , ContractVariationOperatesOnSat);
            dbf.AddParameter("ContractVariationOperatesOnSun"                         , ContractVariationOperatesOnSun);
            dbf.AddParameter("ContractVariationOperatesOnSchoolDays"                  , ContractVariationOperatesOnSchoolDays);
            dbf.AddParameter("ContractVariationOperatesOnNonSchoolDays"               , ContractVariationOperatesOnNonSchoolDays);
            dbf.AddParameter("ContractVariationOperatesOnBankHolidays"                , ContractVariationOperatesOnBankHolidays);
            dbf.AddParameter("ContractVariationOperatesOnChristmasAndNewYear"         , ContractVariationOperatesOnChristmasAndNewYear);
            dbf.AddParameter("ContractVariationCalculatedNoOfDaysOfOperationFirstYear", ContractVariationCalculatedNoOfDaysOfOperationFirstYear);
            dbf.AddParameter("ContractVariationCalculatedNoOfDaysOfOperationFullTerm" , ContractVariationCalculatedNoOfDaysOfOperationFullTerm);

            dbf.AddParameter("ContractVariationAverageMilesPerDay"                    , ContractVariationAverageMilesPerDay);
            dbf.AddParameter("ContractVariationRatePerMile"                           , ContractVariationRatePerMile);
            dbf.AddParameter("ContractVariationFuelPriceElement"                      , ContractVariationFuelPriceElement);
            dbf.AddParameter("ContractVariationAveragePricePerDay"                    , ContractVariationAveragePricePerDay);
            dbf.AddParameter("ContractVariationAveragePricePerYear"                   , ContractVariationAveragePricePerYear);
            dbf.AddParameter("ContractVariationAverageRevenuePerDay"                  , ContractVariationAverageRevenuePerDay);
            dbf.AddParameter("ContractVariationAverageRevenuePerYear"                 , ContractVariationAverageRevenuePerYear);
            dbf.AddParameter("ContractVariationAverageCostPerDay"                     , ContractVariationAverageCostPerDay);
            dbf.AddParameter("ContractVariationAverageCostPerYear"                    , ContractVariationAverageCostPerYear);

            if (includeVariationDates)
            {
                dbf.AddParameter("ContractVariationVariationDateStart"                , ContractVariationVariationDateStart);
                dbf.AddParameter("ContractVariationVariationDateEnd"                  , ContractVariationVariationDateEnd);

            }

            dbf.AddParameter("ContractVariationNotes"                                 , ContractVariationNotes, 500);

            dbf.AddParameter("ContractVariationReasonForEdit"                         , ContractVariationReasonForEdit, 500);
        }

    }

    #endregion
     

    #region ContractVariationList

    public class ContractVariationList : IEnumerable<ContractVariation>
    {

        // Properties

        public List<object> All = new List<object>();
        public List<object> Selectable = new List<object>();
        public List<CalendarDay> CombinedCalendar = new List<CalendarDay>();
        public int CombinedNoOfDaysInOperation;

        public string PropertiesToIgnore
        {
            get
            {
                return
                    "K_ContractVariation|" + "PrimaryKey|Key|IsNew";
            }
        }



        // Utilities

        public int Count { get { return All.Count; } }
        public override string ToString() { return "ContractVariationList (" + Count + " items)"; }



        // Constructors

        public ContractVariationList()
        {

        }

        public ContractVariationList(DCC_Database db, int pQ_Tender = -999, DateTime? pDateStart = null, DateTime? pDateEnd = null)
        {
            if (pQ_Tender > 0)
            {
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sContractVariationListByTender");
                dbf.AddParameter("ContractVariation_Q_Tender", pQ_Tender);
                dbf.AddTable("ContractVariation");
                db.DataLayer.SelectData(dbf);
                if (dbf.hasTables)
                {
                    foreach (DataRow dataRow in dbf.TableZeroRows) Add(new ContractVariation(dataRow));
                }
            }

            else if ((pDateStart.HasValue) && (pDateEnd.HasValue))
            {
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sContractVariationListWithinRange");
                dbf.AddParameter("DateStart", pDateStart.Value);
                dbf.AddParameter("DateEnd", pDateEnd.Value);
                dbf.AddTable("ContractVariation");
                db.DataLayer.SelectData(dbf);
                if (dbf.hasTables)
                {
                    foreach (DataRow dataRow in dbf.TableZeroRows) Add(new ContractVariation(dataRow));
                }
            }
        }




        // Methods

        public void Add(ContractVariation contractVariation)
        {
            All.Add(contractVariation);
        }



        // Enumerator

        public IEnumerator<ContractVariation> GetEnumerator() { foreach (object o in All) yield return (ContractVariation)o; }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

    }

    #endregion


    #region ContractVariationGrid

    public class ContractVariationGrid : DCC_Grid
    {
        public ContractVariationGrid(DCC_Database db, int pQ_Tender = -999)
        {
            ContractVariationList items = new ContractVariationList(db, pQ_Tender);
            TotalRows = items.All;
            Count = CountFiltered = TotalRows.Count;
        }
    }

    #endregion

}
