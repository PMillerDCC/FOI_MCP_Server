using DCC_Standard;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace ProspectBusinessLayer
{

    #region Tender

    public class Tender
    {

        // Properties

        public int       K_Tender                                       { get; set; }
        public int       Q_ContractVariation                            { get; set; }
        public int       Q_Specification                                { get; set; }
        public int       Q_Contractor                                   { get; set; }
        public int       Q_Agreement                                    { get; set; }
        public int       TenderReference                                { get; set; }
        public decimal   TenderPricePerDay                              { get; set; }
        public decimal   FuelPriceElement                               { get; set; }
        public DateTime? TenderClosingDate                              { get; set; }
        public decimal   EstimatedAverageRevenuePerDay                  { get; set; }
        public decimal   AverageCostPerDay                              { get; set; }
        public decimal   TenderPricePerYear                             { get; set; }
        public decimal   EstimatedAverageRevenuePerYear                 { get; set; }
        public decimal   AverageCostPerYear                             { get; set; }
        public bool      AnyTenderQualifications                        { get; set; }
        public string    TenderQualificationNotes                       { get; set; }
        public string    TenderNotes                                    { get; set; }
        public bool      IsConforming                                   { get; set; }
        public string    TenderVehicleDetails                           { get; set; }
        public bool      IncludeInTenderAnalysis                        { get; set; }
        public bool      IsTenderAccepted                               { get; set; }
        public bool      IsContractCancelled                            { get; set; }
        public int       ContractReference                              { get; set; }
        public bool      IsPackage                                      { get; set; }
        public string    ServiceNo                                      { get; set; }
        public string    SpecificationServiceNo                         { get; set; }
        public string    RouteStart                                     { get; set; }
        public string    RouteEnd                                       { get; set; }
        public DateTime? DateStart                                      { get; set; }
        public DateTime? DateEnd                                        { get; set; }
        public string    ContractTypeDescription                        { get; set; }
        public string    ContractorName                                 { get; set; }
        public int       CalculatedNoOfDaysInOperation                  { get; set; }
        public string    ContractorLicenceNo                            { get; set; }
        public string    TenderContractorLicenceNo                      { get; set; }
        public int       Agreement                                      { get; set; }
        public string    AgreementDescription                           { get; set; }    
        public decimal   ContractedMiles                                { get; set; }
        public string    ExceptionalConditions                          { get; set; }
        public string    VehicleCapacity                                { get; set; }
        public string    VehicleElderlyAndDisabled                      { get; set; }
        public string    VehicleDimensions                              { get; set; }
        public string    ServiceDetails1                                { get; set; }
        public string    ServiceDetails2                                { get; set; }
        public string    ServiceDetails3                                { get; set; }
        public bool      OperatesOnMon                                  { get; set; }
        public bool      OperatesOnTue                                  { get; set; }
        public bool      OperatesOnWed                                  { get; set; }
        public bool      OperatesOnThu                                  { get; set; }
        public bool      OperatesOnFri                                  { get; set; }
        public bool      OperatesOnSat                                  { get; set; }
        public bool      OperatesOnSun                                  { get; set; }
        public bool      OperatesOnSchoolDays                           { get; set; }
        public bool      OperatesOnNonSchoolDays                        { get; set; }
        public bool      OperatesOnBankHolidays                         { get; set; }
        public bool      OperatesOnChristmasAndNewYear                  { get; set; }
        public string    RouteNotes                                     { get; set; }
        public decimal   AverageMilesPerDay                             { get; set; }
        public string    TicketNotes                                    { get; set; }
        public string    VehicleNotes                                   { get; set; }
        public bool      Cancelled                                      { get; set; }
        public bool      Extended                                       { get; set; }
        public int       Saved_K_Specification                          { get; set; }
        public string    ContractReferenceSort                          { get { return ContractReference.ToString().PadLeft(5, '0'); } }
        public string    TenderReferenceSort                            { get { return TenderReference.ToString().PadLeft(5, '0'); } }
        public string    DateStartText                                  { get { return DCC_Formatting.NullDateTxt(DateStart, "dd MMM yyyy"); } }
        public string    DateStartSort                                  { get { return DCC_Formatting.NullDateTxt(DateStart, "yyyyMMddHHmm"); } }
        public string    DateEndText                                    { get { return DCC_Formatting.NullDateTxt(DateEnd, "dd MMM yyyy"); } }
        public string    DateEndSort                                    { get { return DCC_Formatting.NullDateTxt(DateEnd, "yyyyMMddHHmm"); } }
        public string    RouteText                                      { get { return (string.IsNullOrEmpty(RouteStart) ? "- not set -" : RouteStart + " to " + RouteEnd); } }
        public string    DatesText                                      { get { return ((DateStart == null) ? "- not set -" : DateStartText + " to " + DateEndText); } }
        public string    TitleText                                      { get { return (IsTenderAccepted ? "Contract" : "Tender"); } }
        public string    ServiceNoText                                  { get { return (IsPackage ? "PACKAGE" : SpecificationServiceNo); } }
        public bool      FuelTerm                                       { get; set; }
        public string    TenderClosingDateText                          { get { return DCC_Formatting.NullDateTxt(TenderClosingDate, "dd MMM yyyy"); } }
        public DateTime? ContractVariationClosingDate                   { get; set; }
        public int       ContractVariation_Q_Contractor                 { get; set; }
        public int       ContractVariation_Q_Agreement                  { get; set; }
        public string    ContractVariationServiceNo                     { get; set; }
        public DateTime? ContractVariationDateStart                     { get; set; }
        public string    ContractVariationRouteStart                    { get; set; }
        public string    ContractVariationRouteEnd                      { get; set; }
        public string    ContractVariationVehicleDimensions             { get; set; }
        public string    ContractVariationVehicleCapacity               { get; set; }
        public string    ContractVariationVehicleElderlyAndDisabled     { get; set; }
        public bool      ContractVariationOperatesOnMon                 { get; set; }
        public bool      ContractVariationOperatesOnTue                 { get; set; }
        public bool      ContractVariationOperatesOnWed                 { get; set; }
        public bool      ContractVariationOperatesOnThu                 { get; set; }
        public bool      ContractVariationOperatesOnFri                 { get; set; }
        public bool      ContractVariationOperatesOnSat                 { get; set; }
        public bool      ContractVariationOperatesOnSun                 { get; set; }
        public bool      ContractVariationOperatesOnSchoolDays          { get; set; }
        public bool      ContractVariationOperatesOnNonSchoolDays       { get; set; }
        public bool      ContractVariationOperatesOnBankHolidays        { get; set; }
        public bool      ContractVariationOperatesOnChristmasAndNewYear { get; set; }
        public decimal   ContractVariationFuelPriceElement              { get; set; }
        public int       ContractVariationCalculatedNoOfDaysInOperation { get; set; }
        public decimal   ContractVariationAverageMilesPerDay            { get; set; }
        public decimal   ContractVariationRatePerMile                   { get; set; }
        public decimal   ContractVariationAverageRevenuePerDay          { get; set; }
        public decimal   ContractVariationAverageCostPerDay             { get; set; }
        public decimal   ContractVariationAveragePricePerDay            { get; set; }
        public decimal   ContractVariationAverageRevenuePerYear         { get; set; }
        public decimal   ContractVariationAverageCostPerYear            { get; set; }
        public decimal   ContractVariationAveragePricePerYear           { get; set; }
        public string    ContractVariationTicketNotes                   { get; set; }
        public string    ContractVariationRouteTimetableNotes           { get; set; }
        public DateTime? ContractVariationDateEnd                       { get; set; }
        public string    ContractVariationExceptionalConditions         { get; set; }
        public string    ContractVariationServiceDetails                { get; set; }
        public string    ContractVariationNotes                         { get; set; }
        public DateTime? ContractVariationDateEntered                   { get; set; }
        public bool      ContractVariationIsCancelled                   { get; set; }
        public bool      ContractVariationIsExtended                    { get; set; }
        public int       ContractVariationNo                            { get; set; }





        // Utilities

        public int PrimaryKey { get { return K_Tender; } set { K_Tender = value; } }
        public string Key { get { return PrimaryKey.ToString(); } }
        public bool IsNew { get { return PrimaryKey == -1; } }

        public override string ToString() { return PrimaryKey.ToString().PadLeft(5) + "; specref: " + ContractReference + "; " + ContractorName + "; " + (IsTenderAccepted ? "Successful" : "Not successful"); }



        // Drop Down Lists

        public class ddlItem { public int Id { get; set; } public string Name { get; set; } }

        public List<ddlItem> ContractorItems;
        public IEnumerable<SelectListItem> ddlContractor { get { return new SelectList(ContractorItems, "Id", "Name"); } }

        public List<ddlItem> AgreementItems;
        public IEnumerable<SelectListItem> ddlAgreement { get { return new SelectList(AgreementItems, "Id", "Name"); } }



        // Data Grids

        public TenderSpecificationGrid TenderSpecificationGrid;
        public ContractVariationGrid ContractVariationGrid;



        // Constructors

        public Tender()
        {
        }

        public Tender(DataRow dataRow)
        {
            GetColumns(dataRow);
        }

        public Tender(DCC_Database db, int pK_Tender = -999, int saved_K_Specification = -1)
        {
            Saved_K_Specification = saved_K_Specification;

            if (pK_Tender == -1) // create a new instance of Tender and set defaults
            {
                K_Tender = -1;

                if (saved_K_Specification > 0)
                {
                    Specification specification = new Specification(db, saved_K_Specification);
                    Q_Specification = specification.K_Specification;
                    ContractReference = specification.ContractReference;
                }
            }

            else if (pK_Tender > 0) // read a record from the database for the given K_Tender
            {
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sTender");
                dbf.AddTable("Tender");
                dbf.AddParameter("K_Tender", pK_Tender);
                db.DataLayer.SelectData(dbf);
                if (dbf.hasTables && dbf.TableZeroRows.Count == 1) GetColumns(dbf.RowZero);
            }

            PopulateDropDownLists(db, pK_Tender);
        }


        // Methods

        public bool GetColumns(DataRow dataRow)
        {
            if (dataRow == null) return false;

            K_Tender                                        = DCC_GetColumn.Integer32(dataRow,          "K_Tender");
            TenderReference                                 = DCC_GetColumn.Integer32(dataRow,          "TenderReference");
            Q_Specification                                 = DCC_GetColumn.Integer32(dataRow,          "Q_Specification");
            Q_Contractor                                    = DCC_GetColumn.Integer32(dataRow,          "Q_Contractor");
            Q_ContractVariation                             = DCC_GetColumn.Integer32(dataRow,          "Q_ContractVariation");
            TenderContractorLicenceNo                       = DCC_GetColumn.String(dataRow,             "TenderContractorLicenceNo");
            FuelPriceElement                                = Math.Round(DCC_GetColumn.Decimal(dataRow, "FuelPriceElement"), 2);
            TenderClosingDate                               = DCC_GetColumn.DateTimeQ(dataRow,          "TenderClosingDate");
            EstimatedAverageRevenuePerDay                   = Math.Round(DCC_GetColumn.Decimal(dataRow, "EstimatedAverageRevenuePerDay"), 2);
            AverageCostPerDay                               = Math.Round(DCC_GetColumn.Decimal(dataRow, "AverageCostPerDay"), 2);
            TenderPricePerYear                              = Math.Round(DCC_GetColumn.Decimal(dataRow, "TenderPricePerYear"), 2);
            EstimatedAverageRevenuePerYear                  = Math.Round(DCC_GetColumn.Decimal(dataRow, "EstimatedAverageRevenuePerYear"), 2);
            AverageCostPerYear                              = Math.Round(DCC_GetColumn.Decimal(dataRow, "AverageCostPerYear"), 2);
            AnyTenderQualifications                         = DCC_GetColumn.Bit(dataRow,                "AnyTenderQualifications");
            TenderQualificationNotes                        = DCC_GetColumn.String(dataRow,             "TenderQualificationNotes");
            TenderVehicleDetails                            = DCC_GetColumn.String(dataRow,             "TenderVehicleDetails");
            TenderNotes                                     = DCC_GetColumn.String(dataRow,             "TenderNotes");
            IsConforming                                    = DCC_GetColumn.Bit(dataRow,                "IsConforming");
            IncludeInTenderAnalysis                         = DCC_GetColumn.Bit(dataRow,                "IncludeInTenderAnalysis");
            IsTenderAccepted                                = DCC_GetColumn.Bit(dataRow,                "IsTenderAccepted");
            TenderPricePerDay                               = DCC_GetColumn.Decimal(dataRow,            "TenderPricePerDay");
            ServiceDetails1                                 = DCC_GetColumn.String(dataRow,             "ServiceDetails1");
            ContractVariationNo                             = DCC_GetColumn.Integer32(dataRow,          "ContractVariationNo");
            ContractVariationServiceNo                      = DCC_GetColumn.String(dataRow,             "ContractVariationServiceNo");
            ContractVariationDateStart                      = DCC_GetColumn.DateTimeQ(dataRow,          "ContractVariationDateStart");
            ContractVariationRouteStart                     = DCC_GetColumn.String(dataRow,             "ContractVariationRouteStart");
            ContractVariationRouteEnd                       = DCC_GetColumn.String(dataRow,             "ContractVariationRouteEnd");
            ContractVariationExceptionalConditions          = DCC_GetColumn.String(dataRow,             "ContractVariationExceptionalConditions");
            ContractVariationVehicleDimensions              = DCC_GetColumn.String(dataRow,             "ContractVariationVehicleDimensions");
            ContractVariationVehicleCapacity                = DCC_GetColumn.String(dataRow,             "ContractVariationVehicleCapacity");
            ContractVariationVehicleElderlyAndDisabled      = DCC_GetColumn.String(dataRow,             "ContractVariationVehicleElderlyAndDisabled");
            ContractVariation_Q_Agreement                   = DCC_GetColumn.Integer32(dataRow,          "ContractVariation_Q_Agreement");
            ContractVariationClosingDate                    = DCC_GetColumn.DateTimeQ(dataRow,          "ContractVariationClosingDate");
            ContractVariationOperatesOnMon                  = DCC_GetColumn.Bit(dataRow,                "ContractVariationOperatesOnMon");
            ContractVariationOperatesOnTue                  = DCC_GetColumn.Bit(dataRow,                "ContractVariationOperatesOnTue");
            ContractVariationOperatesOnWed                  = DCC_GetColumn.Bit(dataRow,                "ContractVariationOperatesOnWed");
            ContractVariationOperatesOnThu                  = DCC_GetColumn.Bit(dataRow,                "ContractVariationOperatesOnThu");
            ContractVariationOperatesOnFri                  = DCC_GetColumn.Bit(dataRow,                "ContractVariationOperatesOnFri");
            ContractVariationOperatesOnSat                  = DCC_GetColumn.Bit(dataRow,                "ContractVariationOperatesOnSat");
            ContractVariationOperatesOnSun                  = DCC_GetColumn.Bit(dataRow,                "ContractVariationOperatesOnSun");
            ContractVariationOperatesOnSchoolDays           = DCC_GetColumn.Bit(dataRow,                "ContractVariationOperatesOnSchoolDays");
            ContractVariationOperatesOnNonSchoolDays        = DCC_GetColumn.Bit(dataRow,                "ContractVariationOperatesOnNonSchoolDays");
            ContractVariationOperatesOnBankHolidays         = DCC_GetColumn.Bit(dataRow,                "ContractVariationOperatesOnBankHolidays");
            ContractVariationOperatesOnChristmasAndNewYear  = DCC_GetColumn.Bit(dataRow,                "ContractVariationOperatesOnChristmasAndNewYear");
            ContractVariationCalculatedNoOfDaysInOperation  = DCC_GetColumn.Integer32(dataRow,          "ContractVariationCalculatedNoOfDaysInOperation");
            ContractVariationAverageMilesPerDay             = DCC_GetColumn.Decimal(dataRow,            "ContractVariationAverageMilesPerDay");
            ContractReference                               = DCC_GetColumn.Integer32(dataRow,          "ContractReference");

            if (DCC_GetColumn.Exists(dataRow, "Q_Agreement"))                   Q_Agreement = DCC_GetColumn.Integer32(dataRow,                   "Q_Agreement");
            if (DCC_GetColumn.Exists(dataRow, "AgreementDescription"))          AgreementDescription = DCC_GetColumn.String(dataRow,             "AgreementDescription");
            if (DCC_GetColumn.Exists(dataRow, "IsPackage"))                     IsPackage = DCC_GetColumn.Bit(dataRow,                           "IsPackage");
            if (DCC_GetColumn.Exists(dataRow, "SpecificationServiceNo"))        SpecificationServiceNo = DCC_GetColumn.String(dataRow,           "SpecificationServiceNo");
            if (DCC_GetColumn.Exists(dataRow, "RouteStart"))                    RouteStart = DCC_GetColumn.String(dataRow,                       "RouteStart");
            if (DCC_GetColumn.Exists(dataRow, "RouteEnd"))                      RouteEnd = DCC_GetColumn.String(dataRow,                         "RouteEnd");
            if (DCC_GetColumn.Exists(dataRow, "DateStart"))                     DateStart = DCC_GetColumn.DateTimeQ(dataRow,                     "DateStart");
            if (DCC_GetColumn.Exists(dataRow, "DateEnd"))                       DateEnd = DCC_GetColumn.DateTimeQ(dataRow,                       "DateEnd");
            if (DCC_GetColumn.Exists(dataRow, "ContractTypeDescription"))       ContractTypeDescription = DCC_GetColumn.String(dataRow,          "ContractTypeDescription");
            if (DCC_GetColumn.Exists(dataRow, "ContractorName"))                ContractorName = DCC_GetColumn.String(dataRow,                   "ContractorName");
            if (DCC_GetColumn.Exists(dataRow, "ContractorLicenceNo"))           ContractorLicenceNo = DCC_GetColumn.String(dataRow,              "ContractorLicenceNo");
            if (DCC_GetColumn.Exists(dataRow, "CalculatedNoOfDaysInOperation")) CalculatedNoOfDaysInOperation = DCC_GetColumn.Integer32(dataRow, "CalculatedNoOfDaysInOperation");
            if (DCC_GetColumn.Exists(dataRow, "FuelTerm"))                      FuelTerm = DCC_GetColumn.Bit(dataRow,                            "FuelTerm");
            if (DCC_GetColumn.Exists(dataRow, "AverageMilesPerDay"))            AverageMilesPerDay = DCC_GetColumn.Decimal(dataRow,              "AverageMilesPerDay");
            if (DCC_GetColumn.Exists(dataRow, "ExceptionalConditions"))         ExceptionalConditions = DCC_GetColumn.String(dataRow,            "ExceptionalConditions");
            if (DCC_GetColumn.Exists(dataRow, "VehicleCapacity"))               VehicleCapacity = DCC_GetColumn.String(dataRow,                  "VehicleCapacity");
            if (DCC_GetColumn.Exists(dataRow, "VehicleDimensions"))             VehicleDimensions = DCC_GetColumn.String(dataRow,                "VehicleDimensions");
            if (DCC_GetColumn.Exists(dataRow, "VehicleElderlyAndDisabled"))     VehicleElderlyAndDisabled = DCC_GetColumn.String(dataRow,        "VehicleElderlyAndDisabled");
            if (DCC_GetColumn.Exists(dataRow, "OperatesOnMon"))                 OperatesOnMon = DCC_GetColumn.Bit(dataRow,                       "OperatesOnMon");
            if (DCC_GetColumn.Exists(dataRow, "OperatesOnTue"))                 OperatesOnTue = DCC_GetColumn.Bit(dataRow,                       "OperatesOnTue");
            if (DCC_GetColumn.Exists(dataRow, "OperatesOnWed"))                 OperatesOnWed = DCC_GetColumn.Bit(dataRow,                       "OperatesOnWed");
            if (DCC_GetColumn.Exists(dataRow, "OperatesOnThu"))                 OperatesOnThu = DCC_GetColumn.Bit(dataRow,                       "OperatesOnThu");
            if (DCC_GetColumn.Exists(dataRow, "OperatesOnFri"))                 OperatesOnFri = DCC_GetColumn.Bit(dataRow,                       "OperatesOnFri");
            if (DCC_GetColumn.Exists(dataRow, "OperatesOnSat"))                 OperatesOnSat = DCC_GetColumn.Bit(dataRow,                       "OperatesOnSat");
            if (DCC_GetColumn.Exists(dataRow, "OperatesOnSun"))                 OperatesOnSun = DCC_GetColumn.Bit(dataRow,                       "OperatesOnSun");
            if (DCC_GetColumn.Exists(dataRow, "OperatesOnSchoolDays"))          OperatesOnSchoolDays = DCC_GetColumn.Bit(dataRow,                "OperatesOnSchoolDays");
            if (DCC_GetColumn.Exists(dataRow, "OperatesOnNonSchoolDays"))       OperatesOnNonSchoolDays = DCC_GetColumn.Bit(dataRow,             "OperatesOnNonSchoolDays");
            if (DCC_GetColumn.Exists(dataRow, "OperatesOnBankHolidays"))        OperatesOnBankHolidays = DCC_GetColumn.Bit(dataRow,              "OperatesOnBankHolidays");
            if (DCC_GetColumn.Exists(dataRow, "OperatesOnChristmasAndNewYear")) OperatesOnChristmasAndNewYear = DCC_GetColumn.Bit(dataRow,       "OperatesOnChristmasAndNewYear");
            if (DCC_GetColumn.Exists(dataRow, "ContractReference"))             ContractReference = DCC_GetColumn.Integer32(dataRow,             "ContractReference");
            if (DCC_GetColumn.Exists(dataRow, "ContractTypeDescription"))       ContractTypeDescription = DCC_GetColumn.String(dataRow,          "ContractTypeDescription");


            return true;
        }

        public void PopulateDropDownLists(DCC_Database db, int pK_Tender)
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

            ContractorItems = new List<ddlItem>();
            ContractorList contractorList = new ContractorList(db);
            foreach (Contractor contractor in contractorList.Selectable)
            {
                ddlItem ddlItem = new ddlItem();
                ddlItem.Id = contractor.K_Contractor;
                ddlItem.Name = contractor.NameAndLicenceNo;
                ContractorItems.Add(ddlItem);
            }

            TenderSpecificationGrid = new TenderSpecificationGrid(db, pK_Tender);
            if (IsTenderAccepted)
            {
                ContractVariationGrid = new ContractVariationGrid(db, pK_Tender);
            }
        }

        public int Save(DCC_Database db, bool saveVariation = false)
        {
            int result = (IsNew ? Insert(db) : Update(db, saveVariation));

            return result;
        }

        int Insert(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("iTender");
            dbf.AddTable("Tender");
            dbf.AddParameter("Q_Specification", Q_Specification);
            dbf.AddParameter("Q_Contractor", Q_Contractor);
            dbf.AddParameter("_PRIMARYKEY", DCC_DatabaseFrame.OUTINTEGER);
            int newKey = db.DataLayer.NonQueryPK(dbf);
            if (newKey > -1) K_Tender = newKey;
            return newKey;
        }

        int Update(DCC_Database db, bool saveVariation = false)
        {
            // After insert or update, check if the tender is accepted
            if (IsTenderAccepted)
            {
                //ContractVariation contractVariation = new ContractVariation(db, pK_Tender: K_Tender);
                //if (contractVariation.K_ContractVariation < 1)
                if (ContractVariationGrid.Count == 0)
                {
                    //also populate cv properties on tender class
                    ContractVariationNo = ContractVariationNo;
                    ContractVariationIsCancelled = Cancelled;
                    ContractVariationIsExtended = Extended;
                    ContractVariationNotes = TenderNotes;
                    ContractVariation_Q_Agreement = Q_Agreement;
                    ContractVariation_Q_Contractor = Q_Contractor;
                    ContractVariationServiceNo = SpecificationServiceNo;
                    ContractVariationRouteStart = RouteStart;
                    ContractVariationRouteEnd = RouteEnd;
                    ContractVariationExceptionalConditions = ExceptionalConditions;
                    ContractVariationVehicleCapacity = VehicleCapacity;
                    ContractVariationVehicleElderlyAndDisabled = VehicleElderlyAndDisabled;
                    ContractVariationVehicleDimensions = VehicleDimensions;
                    ContractVariationServiceDetails = ServiceDetails1;
                    ContractVariationOperatesOnMon = OperatesOnMon;
                    ContractVariationOperatesOnTue = OperatesOnTue;
                    ContractVariationOperatesOnWed = OperatesOnWed;
                    ContractVariationOperatesOnThu = OperatesOnThu;
                    ContractVariationOperatesOnFri = OperatesOnFri;
                    ContractVariationOperatesOnSat = OperatesOnSat;
                    ContractVariationOperatesOnSun = OperatesOnSun;
                    ContractVariationOperatesOnSchoolDays = OperatesOnSchoolDays;
                    ContractVariationOperatesOnNonSchoolDays = OperatesOnNonSchoolDays;
                    ContractVariationOperatesOnBankHolidays = OperatesOnBankHolidays;
                    ContractVariationOperatesOnChristmasAndNewYear = OperatesOnChristmasAndNewYear;
                    ContractVariationCalculatedNoOfDaysInOperation = CalculatedNoOfDaysInOperation;
                    ContractVariationAverageMilesPerDay = AverageMilesPerDay;
                    ContractVariationAverageRevenuePerDay = EstimatedAverageRevenuePerDay;
                    ContractVariationAverageCostPerDay = AverageCostPerDay;
                    ContractVariationAveragePricePerDay = TenderPricePerDay;
                    ContractVariationAverageRevenuePerYear = EstimatedAverageRevenuePerYear;
                    ContractVariationAverageCostPerYear = AverageCostPerYear;
                    ContractVariationAveragePricePerYear = TenderPricePerYear;
                    ContractVariationTicketNotes = TicketNotes;
                    ContractVariationFuelPriceElement = FuelPriceElement;
                    K_Tender = SaveTenderRecord(db);
                    SaveContractVariation(db, 0);
                }
                else
                {
                    K_Tender = SaveTenderRecord(db);
                    if (saveVariation)
                    {
                        SaveContractVariation(db, 1);
                    }
                }
            }

            else
            {
                K_Tender = SaveTenderRecord(db);
            }

            return K_Tender;
        }

        void SaveContractVariation(DCC_Database db, int mode)
        {
            ContractVariation newContractVariation = new ContractVariation();
            newContractVariation.ContractVariation_Q_Tender                     = K_Tender;
            newContractVariation.K_ContractVariation                            = -1;
            newContractVariation.ContractVariationNo                            = mode; // Set to 1 if subsequent variation
            newContractVariation.ContractVariationDateEntered                   = System.DateTime.Now;
            newContractVariation.ContractVariationDateStart                     = ContractVariationDateStart;
            newContractVariation.ContractVariationDateEnd                       = ContractVariationDateEnd;
            newContractVariation.ContractVariationIsCancelled                   = ContractVariationIsCancelled;
            newContractVariation.ContractVariationIsExtended                    = ContractVariationIsExtended;
            newContractVariation.ContractVariationNotes                         = ContractVariationNotes;
            newContractVariation.ContractVariation_Q_Agreement                  = ContractVariation_Q_Agreement;
            newContractVariation.ContractVariationServiceNo                     = ContractVariationServiceNo;
            newContractVariation.ContractVariationRouteStart                    = ContractVariationRouteStart;
            newContractVariation.ContractVariationRouteEnd                      = ContractVariationRouteEnd;
            newContractVariation.ContractVariationExceptionalConditions         = ContractVariationExceptionalConditions;
            newContractVariation.ContractVariationVehicleCapacity               = ContractVariationVehicleCapacity;
            newContractVariation.ContractVariationVehicleElderlyAndDisabled     = ContractVariationVehicleElderlyAndDisabled;
            newContractVariation.ContractVariationVehicleDimensions             = ContractVariationVehicleDimensions;
            newContractVariation.ContractVariationServiceDetails                = ContractVariationServiceDetails;
            newContractVariation.ContractVariationOperatesOnMon                 = ContractVariationOperatesOnMon;
            newContractVariation.ContractVariationOperatesOnTue                 = ContractVariationOperatesOnTue;
            newContractVariation.ContractVariationOperatesOnWed                 = ContractVariationOperatesOnWed;
            newContractVariation.ContractVariationOperatesOnThu                 = ContractVariationOperatesOnThu;
            newContractVariation.ContractVariationOperatesOnFri                 = ContractVariationOperatesOnFri;
            newContractVariation.ContractVariationOperatesOnSat                 = ContractVariationOperatesOnSat;
            newContractVariation.ContractVariationOperatesOnSun                 = ContractVariationOperatesOnSun;
            newContractVariation.ContractVariationOperatesOnSchoolDays          = ContractVariationOperatesOnSchoolDays;
            newContractVariation.ContractVariationOperatesOnNonSchoolDays       = ContractVariationOperatesOnNonSchoolDays;
            newContractVariation.ContractVariationOperatesOnBankHolidays        = ContractVariationOperatesOnBankHolidays;
            newContractVariation.ContractVariationOperatesOnChristmasAndNewYear = ContractVariationOperatesOnChristmasAndNewYear;
            newContractVariation.ContractVariationCalculatedNoOfDaysInOperation = ContractVariationCalculatedNoOfDaysInOperation;
            newContractVariation.ContractVariationAverageCostPerDay             = ContractVariationAverageCostPerDay;
            newContractVariation.ContractVariationTicketNotes                   = ContractVariationTicketNotes;
            newContractVariation.ContractVariationClosingDate                   = ContractVariationClosingDate;
            newContractVariation.ContractVariationFuelPriceElement              = ContractVariationFuelPriceElement;
            newContractVariation.Save(db);
        }

        private int SaveTenderRecord(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uTender");
            dbf.AddTable("Tender");
            dbf.AddParameter("K_Tender",                                        K_Tender);
            dbf.AddParameter("Q_Contractor",                                    Q_Contractor);
            dbf.AddParameter("Q_Agreement",                                     Q_Agreement);
            dbf.AddParameter("AverageMilesPerDay",                              AverageMilesPerDay);
            dbf.AddParameter("TenderPricePerDay",                               TenderPricePerDay);
            dbf.AddParameter("FuelPriceElement",                                FuelPriceElement);
            dbf.AddParameter("TenderClosingDate",                               TenderClosingDate);
            dbf.AddParameter("EstimatedAverageRevenuePerDay",                   EstimatedAverageRevenuePerDay);
            dbf.AddParameter("AverageCostPerDay",                               AverageCostPerDay);
            dbf.AddParameter("TenderPricePerYear",                              TenderPricePerYear);
            dbf.AddParameter("EstimatedAverageRevenuePerYear",                  EstimatedAverageRevenuePerYear);
            dbf.AddParameter("TenderVehicleDetails",                            TenderVehicleDetails, 500);
            dbf.AddParameter("ExceptionalConditions",                           ExceptionalConditions, 500);
            dbf.AddParameter("TenderQualificationNotes",                        TenderQualificationNotes, 500);
            dbf.AddParameter("TenderNotes",                                     TenderNotes, 500);
            dbf.AddParameter("AverageCostPerYear",                              AverageCostPerYear);
            dbf.AddParameter("AnyTenderQualifications",                         AnyTenderQualifications);
            dbf.AddParameter("IsConforming",                                    IsConforming);
            dbf.AddParameter("IncludeInTenderAnalysis",                         IncludeInTenderAnalysis);
            dbf.AddParameter("IsTenderAccepted",                                IsTenderAccepted);
            dbf.AddParameter("SpecificationServiceNo",                          SpecificationServiceNo, 500);
            dbf.AddParameter("ContractVariationDateEntered",                    ContractVariationDateEntered);
            dbf.AddParameter("ContractVariationIsExtended",                     ContractVariationIsExtended);
            dbf.AddParameter("ContractVariationIsCancelled",                    ContractVariationIsCancelled);
            dbf.AddParameter("ContractVariationNotes",                          ContractVariationNotes, 500);
            dbf.AddParameter("ContractVariation_Q_Agreement",                   Q_Agreement);
            dbf.AddParameter("ContractVariationServiceNo",                      SpecificationServiceNo, 500);
            dbf.AddParameter("ContractVariationRouteStart",                     RouteStart, 500);
            dbf.AddParameter("ContractVariationRouteEnd",                       RouteEnd, 500);
            dbf.AddParameter("ContractVariationDateStart",                      DateStart);
            dbf.AddParameter("ContractVariationDateEnd",                        DateEnd);
            dbf.AddParameter("ContractVariationExceptionalConditions",          ExceptionalConditions, 500);
            dbf.AddParameter("ContractVariationVehicleCapacity",                VehicleCapacity, 500);
            dbf.AddParameter("ContractVariationVehicleElderlyAndDisabled",      VehicleElderlyAndDisabled, 500);
            dbf.AddParameter("ContractVariationVehicleDimensions",              VehicleDimensions, 500);
            dbf.AddParameter("ContractVariationServiceDetails",                 ServiceDetails1, 500);
            dbf.AddParameter("ContractVariationOperatesOnMon",                  OperatesOnMon);
            dbf.AddParameter("ContractVariationOperatesOnTue",                  OperatesOnTue);
            dbf.AddParameter("ContractVariationOperatesOnWed",                  OperatesOnWed);
            dbf.AddParameter("ContractVariationOperatesOnThu",                  OperatesOnThu);
            dbf.AddParameter("ContractVariationOperatesOnFri",                  OperatesOnFri);
            dbf.AddParameter("ContractVariationOperatesOnSat",                  OperatesOnSat);
            dbf.AddParameter("ContractVariationOperatesOnSun",                  OperatesOnSun);
            dbf.AddParameter("ContractVariationOperatesOnSchoolDays",           OperatesOnSchoolDays);
            dbf.AddParameter("ContractVariationOperatesOnNonSchoolDays",        OperatesOnNonSchoolDays);
            dbf.AddParameter("ContractVariationOperatesOnBankHolidays",         OperatesOnBankHolidays);
            dbf.AddParameter("ContractVariationOperatesOnChristmasAndNewYear",  OperatesOnChristmasAndNewYear);
            dbf.AddParameter("ContractVariationCalculatedNoOfDaysInOperation",  CalculatedNoOfDaysInOperation);
            dbf.AddParameter("ContractVariationAverageMilesPerDay",             AverageMilesPerDay);
            dbf.AddParameter("ContractVariationRatePerMile",                    ContractVariationRatePerMile);
            dbf.AddParameter("ContractVariationAverageRevenuePerDay",           ContractVariationAverageRevenuePerDay);
            dbf.AddParameter("ContractVariationAverageCostPerDay",              ContractVariationAverageCostPerDay);
            dbf.AddParameter("ContractVariationAveragePricePerDay",             ContractVariationAveragePricePerDay);
            dbf.AddParameter("ContractVariationAverageRevenuePerYear",          ContractVariationAverageRevenuePerYear);
            dbf.AddParameter("ContractVariationAverageCostPerYear",             ContractVariationAverageCostPerYear);
            dbf.AddParameter("ContractVariationAveragePricePerYear",            ContractVariationAveragePricePerYear);
            dbf.AddParameter("ContractVariationTicketNotes",                    ContractVariationTicketNotes, 500);
            dbf.AddParameter("ContractVariationRouteTimetableNotes",            ContractVariationRouteTimetableNotes, 500);
            dbf.AddParameter("ContractVariationClosingDate",                    ContractVariationClosingDate);
            dbf.AddParameter("ContractVariationFuelPriceElement",               FuelPriceElement);

            db.DataLayer.NonQuery(dbf);
            return (dbf.OK ? K_Tender : -1);
        }
    }

    #endregion


    #region TenderList

    public class TenderList : IEnumerable<Tender>
    {

        // Properties

        public List<object> All = new List<object>();
        public string PropertiesToIgnore
        {
            get
            {
                return
"K_Tender|" +
"PrimaryKey|Key|IsNew";
            }
        }



        // Utilities

        public int Count { get { return All.Count; } }
        public override string ToString() { return "InvoiceBatchList (" + Count + " items)"; }



        // Constructors

        public TenderList()
        {
        }

        public TenderList(DCC_Database db, int pQ_Specification = -999, bool awardedOnly = false)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame();
            if (pQ_Specification > -999)
            {
                dbf = new DCC_DatabaseFrame("sTenderListBySpecification");
                dbf.AddParameter("Q_Specification ", pQ_Specification);
            }
            else if (awardedOnly)
            {
                dbf = new DCC_DatabaseFrame("sTenderListAwardedOnly");
            }
            else
            {
                dbf = new DCC_DatabaseFrame("sTenderList");
            }
            dbf.AddTable("Tender");
            db.DataLayer.SelectData(dbf);
            if (dbf.hasTables)
            {
                foreach (DataRow dataRow in dbf.TableZeroRows) Add(new Tender(dataRow));
            }
        }



        // Methods

        private void Add(Tender tender)
        {
            All.Add(tender);
        }



        // Enumerator

        public IEnumerator<Tender> GetEnumerator() { foreach (object o in All) yield return (Tender)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }

    }

    #endregion


    #region TenderGrid

    public class TenderGrid : DCC_Grid
    {
        public TenderGrid(DCC_Database db, int pQ_Specification = -999, bool awardedOnly = false)
        {
            TenderList tenderList;
            if (pQ_Specification > 0)
                tenderList = new TenderList(db, pQ_Specification: pQ_Specification);
            else
                tenderList = new TenderList(db, awardedOnly: awardedOnly);
            TotalRows = tenderList.All;
            Count = CountFiltered = TotalRows.Count;
        }
    }

    #endregion

}
