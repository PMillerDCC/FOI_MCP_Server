using DCC_Standard;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace ProspectBusinessLayer
{
    #region Specification

    public class Specification
    {
        // Properties

        public int       K_Specification                { get; set; }
        public int?      Q_Tender                       { get; set; }
        public int       Q_ContractType                 { get; set; }
        public int       Q_Agreement                    { get; set; }
        public int       ContractReference              { get; set; }
        public bool      IsBlank                        { get; set; }
        public bool      IsPackage                      { get; set; }
        public bool      FuelTerm                       { get; set; }
        public string    SpecificationServiceNo         { get; set; }
        public string    RouteStart                     { get; set; }
        public string    RouteEnd                       { get; set; }
        public DateTime? DateStart                      { get; set; }
        public DateTime? DateEnd                        { get; set; }
        public string    ExceptionalConditions          { get; set; }
        public string    VehicleCapacity                { get; set; }
        public string    VehicleElderlyAndDisabled      { get; set; }
        public string    VehicleDimensions              { get; set; }
        public string    JointResponsibility1           { get; set; }
        public string    ServiceDetails1                { get; set; }
        public bool      OperatesOnMon                  { get; set; }
        public bool      OperatesOnTue                  { get; set; }
        public bool      OperatesOnWed                  { get; set; }
        public bool      OperatesOnThu                  { get; set; }
        public bool      OperatesOnFri                  { get; set; }
        public bool      OperatesOnSat                  { get; set; }
        public bool      OperatesOnSun                  { get; set; }
        public bool      OperatesOnSchoolDays           { get; set; }
        public bool      OperatesOnNonSchoolDays        { get; set; }
        public bool      OperatesOnBankHolidays         { get; set; }
        public bool      OperatesOnChristmasAndNewYear  { get; set; }
        public int       NoOfDaysOfOperationPerYear     { get; set; }
        public decimal   AverageMilesPerDay             { get; set; }
        public decimal   AverageHoursPerDay             { get; set; }
        public int       NoOfVehicles                   { get; set; }
        public decimal   AverageWeeklyRevenueEstimate   { get; set; }
        public string    Notes                          { get; set; }
        public string    AgreementDescription           { get; set; }
        public string    ContractTypeDescription        { get; set; }
        public string    ContractorName                 { get; set; }
        public string    ContractorLicenceNo            { get; set; }
        public int       TenderReference                { get; set; }
        public decimal   TenderPricePerDay              { get; set; }
        public decimal   EstimatedAverageRevenuePerDay  { get; set; }
        public decimal   AverageCostPerDay              { get; set; }
        public decimal   TenderPricePerYear             { get; set; }
        public decimal   EstimatedAverageRevenuePerYear { get; set; }
        public decimal   AverageCostPerYear             { get; set; }
        public int       CalculatedNoOfDaysInOperation  { get; set; }
        public decimal   ContractPricePerDay            { get; set; }
                         
        public string    ContractReferenceSort          { get { return ContractReference.ToString().PadLeft(5, '0'); } }
        public string    TenderReferenceSort            { get { return TenderReference.ToString().PadLeft(5, '0'); } }
        public string    ServiceNoText                  { get { return (IsBlank ? "DUMMY" : (IsPackage ? "PACKAGE" : SpecificationServiceNo)); } }
        public string    DateStartText                  { get { return DCC_Formatting.NullDateTxt(DateStart, "dd MMM yyyy"); } }
        public string    DateStartSort                  { get { return DCC_Formatting.NullDateTxt(DateStart, "yyyyMMddHHmm"); } }
        public string    DateStartFullText              { get { return DCC_Formatting.NullDateTxt(DateStart, "dd MMMM yyyy"); } }
        public string    DateEndText                    { get { return DCC_Formatting.NullDateTxt(DateEnd, "dd MMM yyyy"); } }
        public string    DateEndSort                    { get { return DCC_Formatting.NullDateTxt(DateEnd, "yyyyMMddHHmm"); } }
        public string    DateEndFullText                { get { return DCC_Formatting.NullDateTxt(DateEnd, "dd MMMM yyyy"); } }
        public string    RouteText                      { get { return (string.IsNullOrEmpty(RouteStart) ? "- not set -" : RouteStart + " to " + RouteEnd); } }
        public string    DatesText                      { get { return (string.IsNullOrEmpty(DateStartText) ? "- not set -" : DateStartText + " to " + DateEndText); } }



        // Utilities

        public int PrimaryKey { get { return K_Specification; } set { K_Specification = value; } }
        public string Key { get { return PrimaryKey.ToString(); } }
        public bool IsNew { get { return PrimaryKey == -1; } }
        public override string ToString() { return PrimaryKey.ToString().PadLeft(3) + " - " + SpecificationServiceNo; }



        // Drop Down Lists

        public class ddlItem { public int Id { get; set; } public string Name { get; set; } }

        public List<ddlItem> AgreementItems;
        public IEnumerable<SelectListItem> ddlAgreement { get { return new SelectList(AgreementItems, "Id", "Name"); } }

        public List<ddlItem> ContractTypeItems;
        public IEnumerable<SelectListItem> ddlContractType { get { return new SelectList(ContractTypeItems, "Id", "Name"); } }



        // Data Grids

        public TenderGrid TenderGrid;



        #region Contructors

        // Constructors

        public Specification()
        {
        }

        public Specification(DataRow dataRow) // takes a datarow object as a parameter and calls the GetColumns method with
                                              // the given DataRow object
        {
            GetColumns(dataRow);
        }

        public Specification(DCC_Database db, int pK_Specification = -999, DateTime? pDateStart = null, DateTime? pDateEnd = null)
        {
            if (pK_Specification == -1) // create a new instance of Specification and set defaults. 
            {                           // Checks if the parameter is equal to -1 because that will indicate that a new instance of
                K_Specification = -1;   // the 'Specification' class should be created with default values
            }                           // if pk_Specification is equal to -1, the constructor does not read any data from the
                                        // database and simply creates a new instance of the 'Specification' class. The constructor
                                        // can then set default values for any properties or fields that need them.

            else if (pK_Specification > 0) // read a record from the database for the given K_Specification
            {
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sSpecification");
                dbf.AddTable("Specification");
                dbf.AddParameter("K_Specification", pK_Specification);
                db.DataLayer.SelectData(dbf);
                if (dbf.hasTables && dbf.TableZeroRows.Count == 1) GetColumns(dbf.RowZero);

                CalculateDaysInOperation(db);
            }

            PopulateDropDownLists(db);
        }

        #endregion



        // Methods

        public void CalculateDaysInOperation(DCC_Database db)
        {
            CalculatedNoOfDaysInOperation = 0;
            if ((DateStart == null) || (!DateStart.HasValue) || (DateEnd == null) || (!DateEnd.HasValue)) return;

            // if the contract is for more than a year, use DateStart +364 days for the EndDate

            string dateAndDays =
                DateStart.Value.ToString("dd/MM/yyyy") +
                ((DateEnd.Value > DateStart.Value.AddDays(364)) ? DateStart.Value.AddDays(364) : DateEnd.Value).ToString("dd/MM/yyyy") +
                (OperatesOnMon ? "y" : "f") +
                (OperatesOnTue ? "y" : "f") +
                (OperatesOnWed ? "y" : "f") +
                (OperatesOnThu ? "y" : "f") +
                (OperatesOnFri ? "y" : "f") +
                (OperatesOnSat ? "y" : "f") +
                (OperatesOnSun ? "y" : "f") +
                (OperatesOnSchoolDays ? "y" : "f") +
                (OperatesOnNonSchoolDays ? "y" : "f") +
                (OperatesOnBankHolidays ? "y" : "f") +
                (OperatesOnChristmasAndNewYear ? "y" : "f");

            CalculatedNoOfDaysInOperation = CalculateDaysInOperation(db, dateAndDays);
        }

        public static int CalculateDaysInOperation(DCC_Database db, string DateAndDays)
        {
            int noOfDaysInOperation = 0;
            List<CalendarDay> Calendar = new List<CalendarDay>();
            IEnumerable<DateTime> EachDay(DateTime pStartDate, DateTime pEndDate) { for (var day = pStartDate.Date; day.Date <= pEndDate.Date; day = day.AddDays(1)) yield return day; }

            try
            {
                DateTime startDate, endDate;
                DateTime.TryParse(DateAndDays.Substring(0, 10), out startDate);
                DateTime.TryParse(DateAndDays.Substring(10, 10), out endDate);
                bool operatesOnMon = (DateAndDays.Substring(20, 1) == "y");
                bool operatesOnTue = (DateAndDays.Substring(21, 1) == "y");
                bool operatesOnWed = (DateAndDays.Substring(22, 1) == "y");
                bool operatesOnThu = (DateAndDays.Substring(23, 1) == "y");
                bool operatesOnFri = (DateAndDays.Substring(24, 1) == "y");
                bool operatesOnSat = (DateAndDays.Substring(25, 1) == "y");
                bool operatesOnSun = (DateAndDays.Substring(26, 1) == "y");
                bool operatesOnSchoolDays = (DateAndDays.Substring(27, 1) == "y");
                bool operatesOnNonSchoolDays = (DateAndDays.Substring(28, 1) == "y");
                bool operatesOnBankHolidays = (DateAndDays.Substring(29, 1) == "y");
                bool operatesOnChristmasAndNewYear = (DateAndDays.Substring(30, 1) == "y");

                foreach (DateTime date in EachDay(startDate, endDate))
                {
                    CalendarDay calendarDay = new CalendarDay();
                    calendarDay.Date = date;
                    calendarDay.IsMon = (date.DayOfWeek == DayOfWeek.Monday);
                    calendarDay.IsTue = (date.DayOfWeek == DayOfWeek.Tuesday);
                    calendarDay.IsWed = (date.DayOfWeek == DayOfWeek.Wednesday);
                    calendarDay.IsThu = (date.DayOfWeek == DayOfWeek.Thursday);
                    calendarDay.IsFri = (date.DayOfWeek == DayOfWeek.Friday);
                    calendarDay.IsSat = (date.DayOfWeek == DayOfWeek.Saturday);
                    calendarDay.IsSun = (date.DayOfWeek == DayOfWeek.Sunday);

                    Holiday holiday = new Holiday(db, pYear: date.Year.ToString());

                    calendarDay.IsBankHoliday = (
                        (date == holiday.NewYearsDay) ||
                        (date == holiday.GoodFriday) ||
                        (date == holiday.EasterMonday) ||
                        (date == holiday.MayDay) ||
                        (date == holiday.SpringBankHoliday) ||
                        (date == holiday.SummerBankHoliday) ||
                        (date == holiday.ChristmasDay) ||
                        (date == holiday.BoxingDay) ||
                        ((holiday.Special1 != null) && (date == holiday.Special1)) ||
                        ((holiday.Special2 != null) && (date == holiday.Special2)) ||
                        ((holiday.Special3 != null) && (date == holiday.Special3)) ||
                        ((holiday.Special4 != null) && (date == holiday.Special4)) ||
                        ((holiday.Special5 != null) && (date == holiday.Special5)) ||
                        ((holiday.Special6 != null) && (date == holiday.Special6)) ||
                        ((holiday.Special7 != null) && (date == holiday.Special7)) ||
                        ((holiday.Special8 != null) && (date == holiday.Special8)));

                    calendarDay.IsSchoolDay = (
                        ((date >= holiday.Spring1Start) && (date <= holiday.Spring1End)) ||
                        ((date >= holiday.Spring2Start) && (date <= holiday.Spring2End)) ||
                        ((date >= holiday.Summer1Start) && (date <= holiday.Summer1End)) ||
                        ((date >= holiday.Summer2Start) && (date <= holiday.Summer2End)) ||
                        ((date >= holiday.Autumn1Start) && (date <= holiday.Autumn1End)) ||
                        ((date >= holiday.Autumn2Start) && (date <= holiday.Autumn2End)));

                    calendarDay.IsBetweenChrismasAndNewYear = (
                        ((date > holiday.BoxingDay) && (date < holiday.NewYearsDay.Value.AddYears(1))));

                    if (calendarDay.IsBankHoliday)
                    {
                        if (operatesOnBankHolidays)
                        {
                            calendarDay.IsInOperation = (
                                (calendarDay.IsMon && operatesOnMon) ||
                                (calendarDay.IsTue && operatesOnTue) ||
                                (calendarDay.IsWed && operatesOnWed) ||
                                (calendarDay.IsThu && operatesOnThu) ||
                                (calendarDay.IsFri && operatesOnFri) ||
                                (calendarDay.IsSat && operatesOnSat) ||
                                (calendarDay.IsSun && operatesOnSun));
                        }
                    }

                    else if (calendarDay.IsBetweenChrismasAndNewYear)
                    {
                        if (operatesOnChristmasAndNewYear)
                        {
                            calendarDay.IsInOperation = (
                                (calendarDay.IsMon && operatesOnMon) ||
                                (calendarDay.IsTue && operatesOnTue) ||
                                (calendarDay.IsWed && operatesOnWed) ||
                                (calendarDay.IsThu && operatesOnThu) ||
                                (calendarDay.IsFri && operatesOnFri) ||
                                (calendarDay.IsSat && operatesOnSat) ||
                                (calendarDay.IsSun && operatesOnSun));
                        }
                    }

                    else if (calendarDay.IsSchoolDay)
                    {
                        if (operatesOnSchoolDays)
                        {
                            calendarDay.IsInOperation = (
                                (calendarDay.IsMon && operatesOnMon) ||
                                (calendarDay.IsTue && operatesOnTue) ||
                                (calendarDay.IsWed && operatesOnWed) ||
                                (calendarDay.IsThu && operatesOnThu) ||
                                (calendarDay.IsFri && operatesOnFri) ||
                                (calendarDay.IsSat && operatesOnSat) ||
                                (calendarDay.IsSun && operatesOnSun));
                        }
                    }

                    else if (!calendarDay.IsSchoolDay)
                    {
                        if (operatesOnNonSchoolDays)
                        {
                            calendarDay.IsInOperation = (
                                (calendarDay.IsMon && operatesOnMon) ||
                                (calendarDay.IsTue && operatesOnTue) ||
                                (calendarDay.IsWed && operatesOnWed) ||
                                (calendarDay.IsThu && operatesOnThu) ||
                                (calendarDay.IsFri && operatesOnFri) ||
                                (calendarDay.IsSat && operatesOnSat) ||
                                (calendarDay.IsSun && operatesOnSun));
                        }
                    }

                    if (calendarDay.IsInOperation) noOfDaysInOperation++;

                    Calendar.Add(calendarDay);
                }
            }
            catch (Exception e)
            {
                noOfDaysInOperation = -1;
            }

            DebugTrace.WriteLine();

            foreach (CalendarDay calendarDay in Calendar)
            {
                DebugTrace.WriteLine
                    (calendarDay.Date.ToString("ddd dd/MM/yy") +
                    (calendarDay.IsInOperation ? "  In Op" : "  Not in Op") +
                    (calendarDay.IsSchoolDay ? "  In ST" : "  Not in ST")
                    );
            }
            DebugTrace.WriteLine(noOfDaysInOperation.ToString());
            DebugTrace.WriteLine();

            return noOfDaysInOperation;
        }

        public bool GetColumns(DataRow dataRow)
        {
            if (dataRow == null) return false;

            K_Specification               = DCC_GetColumn.Integer32           (dataRow, "K_Specification");
            ContractReference             = DCC_GetColumn.Integer32           (dataRow, "ContractReference");
            IsBlank                       = DCC_GetColumn.Bit                 (dataRow, "IsBlank");
            IsPackage                     = DCC_GetColumn.Bit                 (dataRow, "IsPackage");
            SpecificationServiceNo        = DCC_GetColumn.String              (dataRow, "SpecificationServiceNo");
            RouteStart                    = DCC_GetColumn.String              (dataRow, "RouteStart");
            RouteEnd                      = DCC_GetColumn.String              (dataRow, "RouteEnd");
            DateStart                     = DCC_GetColumn.DateTimeQ           (dataRow, "DateStart");
            DateEnd                       = DCC_GetColumn.DateTimeQ           (dataRow, "DateEnd");
            Q_ContractType                = DCC_GetColumn.Integer32           (dataRow, "Q_ContractType");
            Q_Agreement                   = DCC_GetColumn.Integer32           (dataRow, "Q_Agreement");

            if (DCC_GetColumn.Exists(dataRow, "AverageMilesPerDay")) AverageMilesPerDay = DCC_GetColumn.Decimal(dataRow, "AverageMilesPerDay");
            if (DCC_GetColumn.Exists(dataRow, "AverageHoursPerDay")) AverageHoursPerDay = DCC_GetColumn.Decimal(dataRow, "AverageHoursPerDay");


            if (DCC_GetColumn.Exists(dataRow, "CalculatedNoOfDaysInOperation"))  CalculatedNoOfDaysInOperation  = DCC_GetColumn.Integer32(dataRow,          "CalculatedNoOfDaysInOperation");
            if (DCC_GetColumn.Exists(dataRow, "NoOfVehicles"))                   NoOfVehicles                   = DCC_GetColumn.Integer32(dataRow,          "NoOfVehicles");
            if (DCC_GetColumn.Exists(dataRow, "FuelTerm"))                       FuelTerm                       = DCC_GetColumn.Bit(dataRow,                "FuelTerm");
            if (DCC_GetColumn.Exists(dataRow, "ExceptionalConditions"))          ExceptionalConditions          = DCC_GetColumn.String(dataRow,             "ExceptionalConditions");
            if (DCC_GetColumn.Exists(dataRow, "VehicleCapacity"))                VehicleCapacity                = DCC_GetColumn.String(dataRow,             "VehicleCapacity");
            if (DCC_GetColumn.Exists(dataRow, "VehicleElderlyAndDisabled"))      VehicleElderlyAndDisabled      = DCC_GetColumn.String(dataRow,             "VehicleElderlyAndDisabled");
            if (DCC_GetColumn.Exists(dataRow, "VehicleDimensions"))              VehicleDimensions              = DCC_GetColumn.String(dataRow,             "VehicleDimensions");
            if (DCC_GetColumn.Exists(dataRow, "JointResponsibility1"))           JointResponsibility1           = DCC_GetColumn.String(dataRow,             "JointResponsibility1");
            if (DCC_GetColumn.Exists(dataRow, "ServiceDetails1"))                ServiceDetails1                = DCC_GetColumn.String(dataRow,             "ServiceDetails1");
            if (DCC_GetColumn.Exists(dataRow, "OperatesOnMon"))                  OperatesOnMon                  = DCC_GetColumn.Bit(dataRow,                "OperatesOnMon");
            if (DCC_GetColumn.Exists(dataRow, "OperatesOnTue"))                  OperatesOnTue                  = DCC_GetColumn.Bit(dataRow,                "OperatesOnTue");
            if (DCC_GetColumn.Exists(dataRow, "OperatesOnWed"))                  OperatesOnWed                  = DCC_GetColumn.Bit(dataRow,                "OperatesOnWed");
            if (DCC_GetColumn.Exists(dataRow, "OperatesOnThu"))                  OperatesOnThu                  = DCC_GetColumn.Bit(dataRow,                "OperatesOnThu");
            if (DCC_GetColumn.Exists(dataRow, "OperatesOnFri"))                  OperatesOnFri                  = DCC_GetColumn.Bit(dataRow,                "OperatesOnFri");
            if (DCC_GetColumn.Exists(dataRow, "OperatesOnSat"))                  OperatesOnSat                  = DCC_GetColumn.Bit(dataRow,                "OperatesOnSat");
            if (DCC_GetColumn.Exists(dataRow, "OperatesOnSun"))                  OperatesOnSun                  = DCC_GetColumn.Bit(dataRow,                "OperatesOnSun");
            if (DCC_GetColumn.Exists(dataRow, "OperatesOnSchoolDays"))           OperatesOnSchoolDays           = DCC_GetColumn.Bit(dataRow,                "OperatesOnSchoolDays");
            if (DCC_GetColumn.Exists(dataRow, "OperatesOnNonSchoolDays"))        OperatesOnNonSchoolDays        = DCC_GetColumn.Bit(dataRow,                "OperatesOnNonSchoolDays");
            if (DCC_GetColumn.Exists(dataRow, "OperatesOnBankHolidays"))         OperatesOnBankHolidays         = DCC_GetColumn.Bit(dataRow,                "OperatesOnBankHolidays");
            if (DCC_GetColumn.Exists(dataRow, "OperatesOnChristmasAndNewYear"))  OperatesOnChristmasAndNewYear  = DCC_GetColumn.Bit(dataRow,                "OperatesOnChristmasAndNewYear");
            if (DCC_GetColumn.Exists(dataRow, "NoOfDaysOfOperationPerYear"))     NoOfDaysOfOperationPerYear     = DCC_GetColumn.Integer32(dataRow,          "NoOfDaysOfOperationPerYear");
            if (DCC_GetColumn.Exists(dataRow, "NoOfVehicles"))                   NoOfVehicles                   = DCC_GetColumn.Integer32(dataRow,          "NoOfVehicles");
            if (DCC_GetColumn.Exists(dataRow, "AverageWeeklyRevenueEstimate"))   AverageWeeklyRevenueEstimate   = Math.Round(DCC_GetColumn.Decimal(dataRow, "AverageWeeklyRevenueEstimate"), 2);
            if (DCC_GetColumn.Exists(dataRow, "Notes"))                          Notes                          = DCC_GetColumn.String(dataRow,             "Notes");
            if (DCC_GetColumn.Exists(dataRow, "AgreementDescription"))           AgreementDescription           = DCC_GetColumn.String(dataRow,             "AgreementDescription");
            if (DCC_GetColumn.Exists(dataRow, "ContractTypeDescription"))        ContractTypeDescription        = DCC_GetColumn.String(dataRow,             "ContractTypeDescription");
            if (DCC_GetColumn.Exists(dataRow, "TenderPricePerDay"))              TenderPricePerDay              = Math.Round(DCC_GetColumn.Decimal(dataRow, "TenderPricePerDay"), 2);
            if (DCC_GetColumn.Exists(dataRow, "EstimatedAverageRevenuePerDay"))  EstimatedAverageRevenuePerDay  = Math.Round(DCC_GetColumn.Decimal(dataRow, "EstimatedAverageRevenuePerDay"), 2);
            if (DCC_GetColumn.Exists(dataRow, "AverageCostPerDay"))              AverageCostPerDay              = Math.Round(DCC_GetColumn.Decimal(dataRow, "AverageCostPerDay"), 2);
            if (DCC_GetColumn.Exists(dataRow, "TenderPricePerYear"))             TenderPricePerYear             = Math.Round(DCC_GetColumn.Decimal(dataRow, "TenderPricePerYear"), 2);
            if (DCC_GetColumn.Exists(dataRow, "EstimatedAverageRevenuePerYear")) EstimatedAverageRevenuePerYear = Math.Round(DCC_GetColumn.Decimal(dataRow, "EstimatedAverageRevenuePerYear"), 2);
            if (DCC_GetColumn.Exists(dataRow, "AverageCostPerYear"))             AverageCostPerYear             = Math.Round(DCC_GetColumn.Decimal(dataRow, "AverageCostPerYear"), 2);
            if (DCC_GetColumn.Exists(dataRow, "ContractorName"))                 ContractorName                 = DCC_GetColumn.String(dataRow,             "ContractorName");
            if (DCC_GetColumn.Exists(dataRow, "ContractorLicenceNo"))            ContractorLicenceNo            = DCC_GetColumn.String(dataRow,             "ContractorLicenceNo");
            if (DCC_GetColumn.Exists(dataRow, "Q_Tender"))                       Q_Tender                       = DCC_GetColumn.Integer32(dataRow,          "Q_Tender");

            return true;
        }

        public void PopulateDropDownLists(DCC_Database db)
        {
            AgreementItems = new List<ddlItem>();
            AgreementList agreementList = new AgreementList(db);
            foreach (Agreement agreement in agreementList.Selectable)
            {
                ddlItem ddlAgreement = new ddlItem();
                ddlAgreement.Id = agreement.K_Agreement;
                ddlAgreement.Name = agreement.AgreementDescription;
                AgreementItems.Add(ddlAgreement);
            }

            ContractTypeItems = new List<ddlItem>();
            ContractTypeList contractTypeList = new ContractTypeList(db);
            foreach (ContractType contractType in contractTypeList.Selectable)
            {
                ddlItem ddlItem = new ddlItem();
                ddlItem.Id = contractType.K_ContractType;
                ddlItem.Name = contractType.ContractTypeDescription;
                ContractTypeItems.Add(ddlItem);
            }

            TenderGrid = new TenderGrid(db, K_Specification);
        }

        public int Save(DCC_Database db)
        {
            return (IsNew ? Insert(db) : Update(db));
        }

        int Insert(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("iSpecification");
            dbf.AddTable("Specification");
            dbf.AddParameter("_PRIMARYKEY", DCC_DatabaseFrame.OUTINTEGER);
            int newKey = db.DataLayer.NonQueryPK(dbf);
            if (newKey > -1) K_Specification = newKey;
            return newKey;
        }

        int Update(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uSpecification");
            dbf.AddTable("Specification");
            dbf.AddParameter("K_Specification",                 K_Specification);
            dbf.AddParameter("Q_ContractType",                  Q_ContractType);
            dbf.AddParameter("Q_Agreement",                     Q_Agreement);
            dbf.AddParameter("Q_Tender",                        Q_Tender);
            dbf.AddParameter("IsBlank",                         IsBlank);
            dbf.AddParameter("IsPackage",                       IsPackage);
            dbf.AddParameter("SpecificationServiceNo",          SpecificationServiceNo, 500);
            dbf.AddParameter("RouteStart",                      RouteStart, 500);
            dbf.AddParameter("RouteEnd",                        RouteEnd, 500);
            dbf.AddParameter("DateStart",                       DateStart);
            dbf.AddParameter("DateEnd",                         DateEnd);
            dbf.AddParameter("OperatesOnMon",                   OperatesOnMon);
            dbf.AddParameter("OperatesOnTue",                   OperatesOnTue);
            dbf.AddParameter("OperatesOnWed",                   OperatesOnWed);
            dbf.AddParameter("OperatesOnThu",                   OperatesOnThu);
            dbf.AddParameter("OperatesOnFri",                   OperatesOnFri);
            dbf.AddParameter("OperatesOnSat",                   OperatesOnSat);
            dbf.AddParameter("OperatesOnSun",                   OperatesOnSun);
            dbf.AddParameter("OperatesOnSchoolDays",            OperatesOnSchoolDays);
            dbf.AddParameter("OperatesOnNonSchoolDays",         OperatesOnNonSchoolDays);
            dbf.AddParameter("OperatesOnBankHolidays",          OperatesOnBankHolidays);
            dbf.AddParameter("OperatesOnChristmasAndNewYear",   OperatesOnChristmasAndNewYear);
            dbf.AddParameter("NoOfDaysOfOperationPerYear",      NoOfDaysOfOperationPerYear);
            dbf.AddParameter("CalculatedNoOfDaysInOperation",   CalculatedNoOfDaysInOperation);
            dbf.AddParameter("AverageMilesPerDay",              AverageMilesPerDay);
            dbf.AddParameter("AverageHoursPerDay",              AverageHoursPerDay);
            dbf.AddParameter("AverageWeeklyRevenueEstimate",    AverageWeeklyRevenueEstimate);
            dbf.AddParameter("FuelTerm",                        FuelTerm);
            dbf.AddParameter("NoOfVehicles",                    NoOfVehicles);
            dbf.AddParameter("VehicleCapacity",                 VehicleCapacity, 500);
            dbf.AddParameter("VehicleElderlyAndDisabled",       VehicleElderlyAndDisabled, 500);
            dbf.AddParameter("VehicleDimensions",               VehicleDimensions, 500);
            dbf.AddParameter("JointResponsibility1",            JointResponsibility1, 500);
            dbf.AddParameter("ServiceDetails1",                 ServiceDetails1, 500);
            dbf.AddParameter("ExceptionalConditions",           ExceptionalConditions, 500);
            dbf.AddParameter("Notes",                           Notes, 500);
            
            db.DataLayer.NonQuery(dbf);
            return (dbf.OK ? K_Specification : -1);
        }

    }

    #endregion


    #region SpecificationList

    public class SpecificationList : IEnumerable<Specification>
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
"K_Specification|" +
"PrimaryKey|Key|IsNew";
            }
        }



        // Utilities

        public int Count { get { return All.Count; } }
        public override string ToString() { return "SpecificationList (" + Count + " items)"; }



        // Constructors

        public SpecificationList(DCC_Database db, DateTime? pStartDate = null, DateTime? pEndDate = null)
        {
            DCC_DatabaseFrame dbf;
            if ((pStartDate == null) && (pEndDate == null))
                dbf = new DCC_DatabaseFrame("sSpecificationList");
            else
            {
                dbf = new DCC_DatabaseFrame("sSpecificationListWithinDateRange");
                dbf.AddParameter("StartDate", pStartDate);
                dbf.AddParameter("EndDate", pEndDate);
            }
            dbf.AddTable("Specification");
            db.DataLayer.SelectData(dbf);
            if (dbf.hasTables)
            {
                foreach (DataRow dataRow in dbf.TableZeroRows) Add(dataRow);
            }
        }



        // Methods

        public void Add(DataRow dataRow)
        {
            Specification specification = new Specification(dataRow);
            All.Add(specification);
            if ((specification.DateStart.HasValue) && (specification.DateStart > DateTime.Now.AddYears(-1))) Selectable.Add(specification);

            #region Calculate Combined No of Days in Operation
            //if its the first specification to be added to the list, if (Count == 1), CombinedCalendar = specification.Calendar;
            //
            //then all subsequent specifications, we'll need to go through the next calendar day by day and if it IsInOperation then set the day in the CombinedCalendar
            //(if it hasn't been checked already)
            #endregion

        }



        // Enumerator

        public IEnumerator<Specification> GetEnumerator() { foreach (object o in All) yield return (Specification)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }

    }

    #endregion


    #region SpecificationGrid

    public class SpecificationGrid : DCC_Grid
    {
        public SpecificationGrid(DCC_Database db)
        {
            SpecificationList items = new SpecificationList(db);
            TotalRows = items.All;
            Count = CountFiltered = TotalRows.Count;
        }
    }

    #endregion


    #region CalendarDay

    public class CalendarDay
    {

        // Properties

        public DateTime Date;
        public bool IsMon;
        public bool IsTue;
        public bool IsWed;
        public bool IsThu;
        public bool IsFri;
        public bool IsSat;
        public bool IsSun;
        public bool IsBankHoliday;
        public bool IsSchoolDay;
        public bool IsBetweenChrismasAndNewYear;
        public bool IsInOperation;

        public string Text { get { return DCC_Formatting.NullDateTxt(Date, "yyyy MMM dd ddd" + (IsInOperation ? " YES" : "")); } }



        // Utilities

        public override string ToString() { return Text; }



        // Constructor

        public CalendarDay()
        {
        }

    }

    #endregion

}
