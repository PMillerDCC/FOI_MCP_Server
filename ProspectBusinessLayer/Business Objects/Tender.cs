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
   
        // Tender properties

        public int       K_Tender                                                { get; set; }
        public int       TenderReference                                         { get; set; }
        public string    TenderReferenceSort                                     { get { return TenderReference.ToString().PadLeft(5, '0'); } }
        public int       Q_Specification                                         { get; set; }
        public int       Q_Contractor                                            { get; set; }

        public string    TenderVehicleDetails                                    { get; set; }

        public int       CalculatedNoOfDaysOfOperationFirstYear                  { get; set; }
        public int       CalculatedNoOfDaysOfOperationFullTerm                   { get; set; }

        public decimal   TenderAverageMilesPerDay                                { get; set; }
        public decimal   TenderRatePerMile                                       { get; set; }
        public decimal   TenderFuelPriceElement                                  { get; set; }
        public decimal   TenderPricePerDay                                       { get; set; }
        public decimal   TenderPricePerYear                                      { get; set; }
        public decimal   EstimatedAverageRevenuePerDay                           { get; set; }
        public decimal   EstimatedAverageRevenuePerYear                          { get; set; }
        public decimal   AverageCostPerDay                                       { get; set; }
        public decimal   AverageCostPerYear                                      { get; set; }

        public bool      IsConforming                                            { get; set; }
        public bool      IncludeInTenderAnalysis                                 { get; set; }
        public bool      AnyTenderQualifications                                 { get; set; }
        public string    TenderQualificationNotes                                { get; set; }

        public bool      IsTenderAccepted                                        { get; set; }
        public string    IsTenderAcceptedText                                    { get { return (IsTenderAccepted ? "Yes" : " "); } }
        public string    TitleText                                               { get { return (IsTenderAccepted ? "Contract" : "Tender"); } }

        public string    TenderNotes                                             { get; set; }


        // Contract Variation properties

        public bool      ContractVariationIsCancelled                            { get; set; }
        public bool      ContractVariationIsExtended                             { get; set; }
        public DateTime? ContractVariationDateStart                              { get; set; }
        public string    ContractVariationDateStartText                          { get { return DCC_Formatting.NullDateTxt(ContractVariationDateStart, "dd MMM yyyy"); } }
        public string    ContractVariationDateStartSort                          { get { return DCC_Formatting.NullDateTxt(ContractVariationDateStart, "yyyyMMddHHmm"); } }
        public DateTime? ContractVariationDateEnd                                { get; set; }
        public string    ContractVariationServiceNo                              { get; set; }
        public string    ContractVariationRouteStart                             { get; set; }
        public string    ContractVariationRouteEnd                               { get; set; }
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


        // join to parent specification properties

        public int       ContractReference                                       { get; set; }
        public string    ContractReferenceSort                                   { get { return ContractReference.ToString().PadLeft(5, '0'); } }
        public bool      IsPackage                                               { get; set; }
        public string    IsPackageText                                           { get { return (IsPackage ? "Yes" : " "); } }
        public int?      Child01_Q_Specification                                 { get; set; }
        public int?      Child02_Q_Specification                                 { get; set; }
        public int?      Child03_Q_Specification                                 { get; set; }
        public int?      Child04_Q_Specification                                 { get; set; }
        public int?      Child05_Q_Specification                                 { get; set; }
        public int?      Child06_Q_Specification                                 { get; set; }
        public int?      Child07_Q_Specification                                 { get; set; }
        public int?      Child08_Q_Specification                                 { get; set; }
        public int?      Child09_Q_Specification                                 { get; set; }
        public int?      Child10_Q_Specification                                 { get; set; }

        public DateTime? DateStart                                               { get; set; }
        public string    DateStartText                                           { get { return DCC_Formatting.NullDateTxt(DateStart, "dd MMM yyyy"); } }
        public string    DateStartSort                                           { get { return DCC_Formatting.NullDateTxt(DateStart, "yyyyMMddHHmm"); } }
        public DateTime? DateEnd                                                 { get; set; }
        public string    DateEndText                                             { get { return DCC_Formatting.NullDateTxt(DateEnd, "dd MMM yyyy"); } }
        public string    DateEndSort                                             { get { return DCC_Formatting.NullDateTxt(DateEnd, "yyyyMMddHHmm"); } }
        public string    DatesText                                               { get { return ((DateStart == null) ? "- not set -" : DateStartText + " to " + DateEndText); } }
        public string    SpecificationServiceNo                                  { get; set; }
        public string    SpecificationServiceNoText                              { get { return (IsPackage ? "Various" : SpecificationServiceNo); }  }
        public string    RouteStart                                              { get; set; }
        public string    RouteEnd                                                { get; set; }
        public string    RouteText                                               { get { return (IsPackage ? "Various" : (string.IsNullOrEmpty(RouteStart) ? "- not set -" : RouteStart + " to " + RouteEnd) ); } }

        public int?      Q_Agreement                                             { get; set; }
        public bool      IsFuelTerm                                              { get; set; }
        public string    IsFuelTermText                                           { get { return (IsFuelTerm ? "Yes" : "No"); } }
        public DateTime? TenderClosingDate                                       { get; set; }
        public string    TenderClosingDateText                                   { get { return DCC_Formatting.NullDateTxt(TenderClosingDate, "dd MMM yyyy"); } }
        public string    JointResponsibility1                                    { get; set; }
        public string    ServiceDetails1                                         { get; set; }

        public bool      OperatesOnMon                                           { get; set; }
        public bool      OperatesOnTue                                           { get; set; }
        public bool      OperatesOnWed                                           { get; set; }
        public bool      OperatesOnThu                                           { get; set; }
        public bool      OperatesOnFri                                           { get; set; }
        public bool      OperatesOnSat                                           { get; set; }
        public bool      OperatesOnSun                                           { get; set; }
        public bool      OperatesOnSchoolDays                                    { get; set; }
        public bool      OperatesOnNonSchoolDays                                 { get; set; }
        public bool      OperatesOnBankHolidays                                  { get; set; }
        public bool      OperatesOnChristmasAndNewYear                           { get; set; }

        public decimal   AverageMilesPerDay                                      { get; set; }

        public string    ContractTypeDescription                                 { get; set; }
        public string    ContractorName                                          { get; set; }
        public string    ContractorLicenceNo                                     { get; set; }
        public string    SpecificationAgreementDescription                       { get; set; }    
        public string    ContractVariationAgreementDescription                   { get; set; }    
        public string    AgreementDescription                                    { get; set; }


        // join to child specifications in a package

        public string    Child01_ContractReference                               { get; set; }
        public string    Child02_ContractReference                               { get; set; }
        public string    Child03_ContractReference                               { get; set; }
        public string    Child04_ContractReference                               { get; set; }
        public string    Child05_ContractReference                               { get; set; }
        public string    Child06_ContractReference                               { get; set; }
        public string    Child07_ContractReference                               { get; set; }
        public string    Child08_ContractReference                               { get; set; }
        public string    Child09_ContractReference                               { get; set; }
        public string    Child10_ContractReference                               { get; set; }
        public string    Child01_ServiceNo                                       { get; set; }
        public string    Child02_ServiceNo                                       { get; set; }
        public string    Child03_ServiceNo                                       { get; set; }
        public string    Child04_ServiceNo                                       { get; set; }
        public string    Child05_ServiceNo                                       { get; set; }
        public string    Child06_ServiceNo                                       { get; set; }
        public string    Child07_ServiceNo                                       { get; set; }
        public string    Child08_ServiceNo                                       { get; set; }
        public string    Child09_ServiceNo                                       { get; set; }
        public string    Child10_ServiceNo                                       { get; set; }
        public string    Child01_RouteDescription                                { get; set; }
        public string    Child02_RouteDescription                                { get; set; }
        public string    Child03_RouteDescription                                { get; set; }
        public string    Child04_RouteDescription                                { get; set; }
        public string    Child05_RouteDescription                                { get; set; }
        public string    Child06_RouteDescription                                { get; set; }
        public string    Child07_RouteDescription                                { get; set; }
        public string    Child08_RouteDescription                                { get; set; }
        public string    Child09_RouteDescription                                { get; set; }
        public string    Child10_RouteDescription                                { get; set; }
		public string    Parent_HyperLink                                        { get { return "/Specification/Specification?primaryKey=" + Q_Specification.ToString(); }}
		public string    Child01_HyperLink                                       { get { return "/Specification/Specification?primaryKey=" + Child01_Q_Specification.ToString(); }}
		public string    Child02_HyperLink                                       { get { return "/Specification/Specification?primaryKey=" + Child02_Q_Specification.ToString(); }}
		public string    Child03_HyperLink                                       { get { return "/Specification/Specification?primaryKey=" + Child03_Q_Specification.ToString(); }}
		public string    Child04_HyperLink                                       { get { return "/Specification/Specification?primaryKey=" + Child04_Q_Specification.ToString(); }}
		public string    Child05_HyperLink                                       { get { return "/Specification/Specification?primaryKey=" + Child05_Q_Specification.ToString(); }}
		public string    Child06_HyperLink                                       { get { return "/Specification/Specification?primaryKey=" + Child06_Q_Specification.ToString(); }}
		public string    Child07_HyperLink                                       { get { return "/Specification/Specification?primaryKey=" + Child07_Q_Specification.ToString(); }}
		public string    Child08_HyperLink                                       { get { return "/Specification/Specification?primaryKey=" + Child08_Q_Specification.ToString(); }}
		public string    Child09_HyperLink                                       { get { return "/Specification/Specification?primaryKey=" + Child09_Q_Specification.ToString(); }}
		public string    Child10_HyperLink                                       { get { return "/Specification/Specification?primaryKey=" + Child10_Q_Specification.ToString(); }}


        // don't know ???

        public bool      IsContractCancelled                                     { get; set; }
        public string    ServiceNo                                               { get; set; }
        public string    TenderContractorLicenceNo                               { get; set; }
        public decimal   ContractedMiles                                         { get; set; }
        public string    ExceptionalConditions                                   { get; set; }
        public string    VehicleCapacity                                         { get; set; }
        public string    VehicleElderlyAndDisabled                               { get; set; }
        public string    VehicleDimensions                                       { get; set; }
        public string    RouteNotes                                              { get; set; }
        public string    TicketNotes                                             { get; set; }
        public string    VehicleNotes                                            { get; set; }



        // Utilities

        public int PrimaryKey { get { return K_Tender; } set { K_Tender = value; } }
        public string Key { get { return PrimaryKey.ToString(); } }
        public bool IsNew { get { return PrimaryKey == -1; } }
        public override string ToString() { return PrimaryKey.ToString().PadLeft(5) + "; specref: " + ContractReference + "; " + ContractorName + "; " + (IsTenderAccepted ? "Successful" : "Not successful"); }



        // Drop Down Lists

        public class ddlItem { public int Id { get; set; } public string Name { get; set; } }

        public List<ddlItem> SpecificationItems;
        public IEnumerable<SelectListItem> ddlSpecification { get { return new SelectList(SpecificationItems, "Id", "Name"); } }

        public List<ddlItem> ContractorItems;
        public IEnumerable<SelectListItem> ddlContractor { get { return new SelectList(ContractorItems, "Id", "Name"); } }

        public List<ddlItem> AgreementItems;
        public IEnumerable<SelectListItem> ddlAgreement { get { return new SelectList(AgreementItems, "Id", "Name"); } }



        // Data Grids

        public ContractVariationGrid ContractVariationGrid;



        // Constructors

        public Tender()
        {
        }

        public Tender(DataRow dataRow)
        {
            GetColumns(dataRow);
        }

        public Tender(DCC_Database db, int pK_Tender = -999, int pQ_Specification = -999, bool pBriefMode = false)
        {
            if (pK_Tender == -1) // create a new instance of Tender and set defaults
            {
                K_Tender = -1;

                if (pQ_Specification > 0)
                {
                    Q_Specification = pQ_Specification;
                }

                pK_Tender = Save(db, "Tender");
            }

            if (pK_Tender > 0) // read a record from the database for the given K_Tender
            {
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sTender");
                dbf.AddTable("Tender");
                dbf.AddParameter("K_Tender", pK_Tender);
                db.DataLayer.SelectData(dbf);
                if (dbf.hasTables && dbf.TableZeroRows.Count == 1) GetColumns(dbf.RowZero);
                
                if (!pBriefMode) PopulateDropDownLists(db, pK_Tender);
            }
        }



        // Methods

        public bool GetColumns(DataRow dataRow)
        {
            if (dataRow == null) return false;

            if (DCC_GetColumn.Exists(dataRow, "K_Tender"))                                                K_Tender                                                = DCC_GetColumn.Integer32(dataRow,          "K_Tender");
            if (DCC_GetColumn.Exists(dataRow, "TenderReference"))                                         TenderReference                                         = DCC_GetColumn.Integer32(dataRow,          "TenderReference");
            if (DCC_GetColumn.Exists(dataRow, "Q_Specification"))                                         Q_Specification                                         = DCC_GetColumn.Integer32(dataRow,          "Q_Specification");
            if (DCC_GetColumn.Exists(dataRow, "Q_Contractor"))                                            Q_Contractor                                            = DCC_GetColumn.Integer32(dataRow,          "Q_Contractor");

            if (DCC_GetColumn.Exists(dataRow, "CalculatedNoOfDaysOfOperationFirstYear"))                  CalculatedNoOfDaysOfOperationFirstYear                  = DCC_GetColumn.Integer32(dataRow,          "CalculatedNoOfDaysOfOperationFirstYear");
            if (DCC_GetColumn.Exists(dataRow, "CalculatedNoOfDaysOfOperationFullTerm"))                   CalculatedNoOfDaysOfOperationFullTerm                   = DCC_GetColumn.Integer32(dataRow,          "CalculatedNoOfDaysOfOperationFullTerm");

            if (DCC_GetColumn.Exists(dataRow, "TenderPricePerYear"))                                      TenderPricePerYear                                      = Math.Round(DCC_GetColumn.Decimal(dataRow, "TenderPricePerYear"), 2);
            if (DCC_GetColumn.Exists(dataRow, "TenderPricePerDay"))                                       TenderPricePerDay                                       = Math.Round(DCC_GetColumn.Decimal(dataRow, "TenderPricePerDay"), 2);
            if (DCC_GetColumn.Exists(dataRow, "EstimatedAverageRevenuePerDay"))                           EstimatedAverageRevenuePerDay                           = Math.Round(DCC_GetColumn.Decimal(dataRow, "EstimatedAverageRevenuePerDay"), 2);
            if (DCC_GetColumn.Exists(dataRow, "EstimatedAverageRevenuePerYear"))                          EstimatedAverageRevenuePerYear                          = Math.Round(DCC_GetColumn.Decimal(dataRow, "EstimatedAverageRevenuePerYear"), 2);
            if (DCC_GetColumn.Exists(dataRow, "AverageCostPerDay"))                                       AverageCostPerDay                                       = Math.Round(DCC_GetColumn.Decimal(dataRow, "AverageCostPerDay"), 2);
            if (DCC_GetColumn.Exists(dataRow, "AverageCostPerYear"))                                      AverageCostPerYear                                      = Math.Round(DCC_GetColumn.Decimal(dataRow, "AverageCostPerYear"), 2);

            if (DCC_GetColumn.Exists(dataRow, "TenderVehicleDetails"))                                    TenderVehicleDetails                                    = DCC_GetColumn.String(dataRow,             "TenderVehicleDetails");

            if (DCC_GetColumn.Exists(dataRow, "IsConforming"))                                            IsConforming                                            = DCC_GetColumn.Bit(dataRow,                "IsConforming");
            if (DCC_GetColumn.Exists(dataRow, "IncludeInTenderAnalysis"))                                 IncludeInTenderAnalysis                                 = DCC_GetColumn.Bit(dataRow,                "IncludeInTenderAnalysis");
            if (DCC_GetColumn.Exists(dataRow, "AnyTenderQualifications"))                                 AnyTenderQualifications                                 = DCC_GetColumn.Bit(dataRow,                "AnyTenderQualifications");
            if (DCC_GetColumn.Exists(dataRow, "TenderQualificationNotes"))                                TenderQualificationNotes                                = DCC_GetColumn.String(dataRow,             "TenderQualificationNotes");
            if (DCC_GetColumn.Exists(dataRow, "TenderNotes"))                                             TenderNotes                                             = DCC_GetColumn.String(dataRow,             "TenderNotes");

            if (DCC_GetColumn.Exists(dataRow, "IsTenderAccepted"))                                        IsTenderAccepted                                        = DCC_GetColumn.Bit(dataRow,                "IsTenderAccepted");
            if (DCC_GetColumn.Exists(dataRow, "TenderAverageMilesPerDay"))                                TenderAverageMilesPerDay                                = Math.Round(DCC_GetColumn.Decimal(dataRow, "TenderAverageMilesPerDay"), 1);
            if (DCC_GetColumn.Exists(dataRow, "TenderRatePerMile"))                                       TenderRatePerMile                                       = Math.Round(DCC_GetColumn.Decimal(dataRow, "TenderRatePerMile"), 2);
            if (DCC_GetColumn.Exists(dataRow, "TenderFuelPriceElement"))                                  TenderFuelPriceElement                                  = Math.Round(DCC_GetColumn.Decimal(dataRow, "TenderFuelPriceElement"), 2);
                                                                                                          
            if (DCC_GetColumn.Exists(dataRow, "ContractVariationDateStart"))                              ContractVariationDateStart                              = DCC_GetColumn.DateTimeQ(dataRow,          "ContractVariationDateStart");
            if (DCC_GetColumn.Exists(dataRow, "ContractVariationDateEnd"))                                ContractVariationDateEnd                                = DCC_GetColumn.DateTimeQ(dataRow,          "ContractVariationDateEnd");
            if (DCC_GetColumn.Exists(dataRow, "ContractVariationServiceNo"))                              ContractVariationServiceNo                              = DCC_GetColumn.String(dataRow,             "ContractVariationServiceNo");
            if (DCC_GetColumn.Exists(dataRow, "ContractVariationRouteStart"))                             ContractVariationRouteStart                             = DCC_GetColumn.String(dataRow,             "ContractVariationRouteStart");
            if (DCC_GetColumn.Exists(dataRow, "ContractVariationRouteEnd"))                               ContractVariationRouteEnd                               = DCC_GetColumn.String(dataRow,             "ContractVariationRouteEnd");
            if (DCC_GetColumn.Exists(dataRow, "ContractVariationExceptionalConditions"))                  ContractVariationExceptionalConditions                  = DCC_GetColumn.String(dataRow,             "ContractVariationExceptionalConditions");

            if (DCC_GetColumn.Exists(dataRow, "ContractVariationVehicleCapacity"))                        ContractVariationVehicleCapacity                        = DCC_GetColumn.String(dataRow,             "ContractVariationVehicleCapacity");
            if (DCC_GetColumn.Exists(dataRow, "ContractVariationVehicleElderlyAndDisabled"))              ContractVariationVehicleElderlyAndDisabled              = DCC_GetColumn.String(dataRow,             "ContractVariationVehicleElderlyAndDisabled");
            if (DCC_GetColumn.Exists(dataRow, "ContractVariationVehicleDimensions"))                      ContractVariationVehicleDimensions                      = DCC_GetColumn.String(dataRow,             "ContractVariationVehicleDimensions");

            if (DCC_GetColumn.Exists(dataRow, "ContractVariation_Q_Agreement"))                           ContractVariation_Q_Agreement                           = DCC_GetColumn.Integer32(dataRow,          "ContractVariation_Q_Agreement");
            if (DCC_GetColumn.Exists(dataRow, "ContractVariationServiceDetails"))                         ContractVariationServiceDetails                         = DCC_GetColumn.String(dataRow,             "ContractVariationServiceDetails");
            if (DCC_GetColumn.Exists(dataRow, "ContractVariationTicketNotes"))                            ContractVariationTicketNotes                            = DCC_GetColumn.String(dataRow,             "ContractVariationTicketNotes");
            if (DCC_GetColumn.Exists(dataRow, "ContractVariationRouteTimetableNotes"))                    ContractVariationRouteTimetableNotes                    = DCC_GetColumn.String(dataRow,             "ContractVariationRouteTimetableNotes");

            if (DCC_GetColumn.Exists(dataRow, "ContractVariationOperatesOnMon"))                          ContractVariationOperatesOnMon                          = DCC_GetColumn.Bit(dataRow,                "ContractVariationOperatesOnMon");
            if (DCC_GetColumn.Exists(dataRow, "ContractVariationOperatesOnTue"))                          ContractVariationOperatesOnTue                          = DCC_GetColumn.Bit(dataRow,                "ContractVariationOperatesOnTue");
            if (DCC_GetColumn.Exists(dataRow, "ContractVariationOperatesOnWed"))                          ContractVariationOperatesOnWed                          = DCC_GetColumn.Bit(dataRow,                "ContractVariationOperatesOnWed");
            if (DCC_GetColumn.Exists(dataRow, "ContractVariationOperatesOnThu"))                          ContractVariationOperatesOnThu                          = DCC_GetColumn.Bit(dataRow,                "ContractVariationOperatesOnThu");
            if (DCC_GetColumn.Exists(dataRow, "ContractVariationOperatesOnFri"))                          ContractVariationOperatesOnFri                          = DCC_GetColumn.Bit(dataRow,                "ContractVariationOperatesOnFri");
            if (DCC_GetColumn.Exists(dataRow, "ContractVariationOperatesOnSat"))                          ContractVariationOperatesOnSat                          = DCC_GetColumn.Bit(dataRow,                "ContractVariationOperatesOnSat");
            if (DCC_GetColumn.Exists(dataRow, "ContractVariationOperatesOnSun"))                          ContractVariationOperatesOnSun                          = DCC_GetColumn.Bit(dataRow,                "ContractVariationOperatesOnSun");
            if (DCC_GetColumn.Exists(dataRow, "ContractVariationOperatesOnSchoolDays"))                   ContractVariationOperatesOnSchoolDays                   = DCC_GetColumn.Bit(dataRow,                "ContractVariationOperatesOnSchoolDays");
            if (DCC_GetColumn.Exists(dataRow, "ContractVariationOperatesOnNonSchoolDays"))                ContractVariationOperatesOnNonSchoolDays                = DCC_GetColumn.Bit(dataRow,                "ContractVariationOperatesOnNonSchoolDays");
            if (DCC_GetColumn.Exists(dataRow, "ContractVariationOperatesOnBankHolidays"))                 ContractVariationOperatesOnBankHolidays                 = DCC_GetColumn.Bit(dataRow,                "ContractVariationOperatesOnBankHolidays");
            if (DCC_GetColumn.Exists(dataRow, "ContractVariationOperatesOnChristmasAndNewYear"))          ContractVariationOperatesOnChristmasAndNewYear          = DCC_GetColumn.Bit(dataRow,                "ContractVariationOperatesOnChristmasAndNewYear");
            if (DCC_GetColumn.Exists(dataRow, "ContractVariationCalculatedNoOfDaysOfOperationFirstYear")) ContractVariationCalculatedNoOfDaysOfOperationFirstYear = DCC_GetColumn.Integer32(dataRow,          "ContractVariationCalculatedNoOfDaysOfOperationFirstYear");
            if (DCC_GetColumn.Exists(dataRow, "ContractVariationCalculatedNoOfDaysOfOperationFullTerm"))  ContractVariationCalculatedNoOfDaysOfOperationFullTerm  = DCC_GetColumn.Integer32(dataRow,          "ContractVariationCalculatedNoOfDaysOfOperationFullTerm");

            if (DCC_GetColumn.Exists(dataRow, "ContractVariationAverageMilesPerDay"))                     ContractVariationAverageMilesPerDay                     = Math.Round(DCC_GetColumn.Decimal(dataRow, "ContractVariationAverageMilesPerDay"), 1);
            if (DCC_GetColumn.Exists(dataRow, "ContractVariationRatePerMile"))                            ContractVariationRatePerMile                            = Math.Round(DCC_GetColumn.Decimal(dataRow, "ContractVariationRatePerMile"), 2);
            if (DCC_GetColumn.Exists(dataRow, "ContractVariationFuelPriceElement"))                       ContractVariationFuelPriceElement                       = Math.Round(DCC_GetColumn.Decimal(dataRow, "ContractVariationFuelPriceElement"), 2);
            if (DCC_GetColumn.Exists(dataRow, "ContractVariationAveragePricePerDay"))                     ContractVariationAveragePricePerDay                     = Math.Round(DCC_GetColumn.Decimal(dataRow, "ContractVariationAveragePricePerDay"), 2);
            if (DCC_GetColumn.Exists(dataRow, "ContractVariationAverageRevenuePerDay"))                   ContractVariationAverageRevenuePerDay                   = Math.Round(DCC_GetColumn.Decimal(dataRow, "ContractVariationAverageRevenuePerDay"), 2);
            if (DCC_GetColumn.Exists(dataRow, "ContractVariationAverageCostPerDay"))                      ContractVariationAverageCostPerDay                      = Math.Round(DCC_GetColumn.Decimal(dataRow, "ContractVariationAverageCostPerDay"), 2);
            if (DCC_GetColumn.Exists(dataRow, "ContractVariationAveragePricePerYear"))                    ContractVariationAveragePricePerYear                    = Math.Round(DCC_GetColumn.Decimal(dataRow, "ContractVariationAveragePricePerYear"), 2);
            if (DCC_GetColumn.Exists(dataRow, "ContractVariationAverageRevenuePerYear"))                  ContractVariationAverageRevenuePerYear                  = Math.Round(DCC_GetColumn.Decimal(dataRow, "ContractVariationAverageRevenuePerYear"), 2);
            if (DCC_GetColumn.Exists(dataRow, "ContractVariationAverageCostPerYear"))                     ContractVariationAverageCostPerYear                     = Math.Round(DCC_GetColumn.Decimal(dataRow, "ContractVariationAverageCostPerYear"), 2);

            if (DCC_GetColumn.Exists(dataRow, "ContractVariationVariationDateStart"))                     ContractVariationVariationDateStart                     = DCC_GetColumn.DateTimeQ(dataRow,          "ContractVariationVariationDateStart");
            if (DCC_GetColumn.Exists(dataRow, "ContractVariationVariationDateEnd"))                       ContractVariationVariationDateEnd                       = DCC_GetColumn.DateTimeQ(dataRow,          "ContractVariationVariationDateEnd");

            if (DCC_GetColumn.Exists(dataRow, "ContractVariationNotes"))                                  ContractVariationNotes                                  = DCC_GetColumn.String(dataRow,             "ContractVariationNotes");

            if (DCC_GetColumn.Exists(dataRow, "ContractReference"))                                       ContractReference                                       = DCC_GetColumn.Integer32(dataRow,          "ContractReference");
            if (DCC_GetColumn.Exists(dataRow, "IsPackage"))                                               IsPackage                                               = DCC_GetColumn.Bit(dataRow,                "IsPackage");
            if (DCC_GetColumn.Exists(dataRow, "DateStart"))                                               DateStart                                               = DCC_GetColumn.DateTimeQ(dataRow,          "DateStart");
            if (DCC_GetColumn.Exists(dataRow, "DateEnd"))                                                 DateEnd                                                 = DCC_GetColumn.DateTimeQ(dataRow,          "DateEnd");
            if (DCC_GetColumn.Exists(dataRow, "SpecificationServiceNo"))                                  SpecificationServiceNo                                  = DCC_GetColumn.String(dataRow,             "SpecificationServiceNo");
            if (DCC_GetColumn.Exists(dataRow, "RouteStart"))                                              RouteStart                                              = DCC_GetColumn.String(dataRow,             "RouteStart");
            if (DCC_GetColumn.Exists(dataRow, "RouteEnd"))                                                RouteEnd                                                = DCC_GetColumn.String(dataRow,             "RouteEnd");

            if (DCC_GetColumn.Exists(dataRow, "Q_Agreement"))                                             Q_Agreement                                             = DCC_GetColumn.Integer32(dataRow,          "Q_Agreement");
            if (DCC_GetColumn.Exists(dataRow, "IsFuelTerm"))                                              IsFuelTerm                                              = DCC_GetColumn.Bit(dataRow,                "IsFuelTerm");
            if (DCC_GetColumn.Exists(dataRow, "TenderClosingDate"))                                       TenderClosingDate                                       = DCC_GetColumn.DateTimeQ(dataRow,          "TenderClosingDate");
            if (DCC_GetColumn.Exists(dataRow, "JointResponsibility1"))                                    JointResponsibility1                                    = DCC_GetColumn.String(dataRow,             "JointResponsibility1");
            if (DCC_GetColumn.Exists(dataRow, "ServiceDetails1"))                                         ServiceDetails1                                         = DCC_GetColumn.String(dataRow,             "ServiceDetails1");

            if (DCC_GetColumn.Exists(dataRow, "ContractTypeDescription"))                                 ContractTypeDescription                                 = DCC_GetColumn.String(dataRow,             "ContractTypeDescription");
            if (DCC_GetColumn.Exists(dataRow, "AgreementDescription"))                                    AgreementDescription                                    = DCC_GetColumn.String(dataRow,             "AgreementDescription");
            if (DCC_GetColumn.Exists(dataRow, "ContractorName"))                                          ContractorName                                          = DCC_GetColumn.String(dataRow,             "ContractorName");
            if (DCC_GetColumn.Exists(dataRow, "ContractorLicenceNo"))                                     ContractorLicenceNo                                     = DCC_GetColumn.String(dataRow,             "ContractorLicenceNo");
            if (DCC_GetColumn.Exists(dataRow, "SpecificationAgreementDescription"))                       SpecificationAgreementDescription                       = DCC_GetColumn.String(dataRow,             "SpecificationAgreementDescription");
            if (DCC_GetColumn.Exists(dataRow, "ContractVariationAgreementDescription"))                   ContractVariationAgreementDescription                   = DCC_GetColumn.String(dataRow,             "ContractVariationAgreementDescription");
                                                                                                       
            return true;
        }

        public void PopulateDropDownLists(DCC_Database db, int pK_Tender)
        {
            if (Q_Specification == 0)
            {
                SpecificationList specificationList = new SpecificationList(db);
                SpecificationItems = new List<ddlItem>();
                foreach (Specification specification in specificationList.ActiveIncludingPackages)
                {
                    ddlItem ddlSpecification = new ddlItem();
                    ddlSpecification.Id = specification.K_Specification;
                    ddlSpecification.Name = specification.ContractReference.ToString() + " - " + specification.SpecificationServiceNoText;
                    SpecificationItems.Add(ddlSpecification);
                }
            }

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

            #region If it's a package, load the child specifications

            if (IsPackage)
            {
                Specification parent = new Specification(db, Q_Specification, pBriefMode:true);
                Child01_Q_Specification = parent.Child01_Q_Specification;
                Child02_Q_Specification = parent.Child02_Q_Specification;
                Child03_Q_Specification = parent.Child03_Q_Specification;
                Child04_Q_Specification = parent.Child04_Q_Specification;
                Child05_Q_Specification = parent.Child05_Q_Specification;
                Child06_Q_Specification = parent.Child06_Q_Specification;
                Child07_Q_Specification = parent.Child07_Q_Specification;
                Child08_Q_Specification = parent.Child08_Q_Specification;
                Child09_Q_Specification = parent.Child09_Q_Specification;
                Child10_Q_Specification = parent.Child10_Q_Specification;
                ContractReference = parent.ContractReference;

                if (Child01_Q_Specification.HasValue)
                {
                    Specification specification = new Specification(db, Child01_Q_Specification.Value, pBriefMode:true);
                    Child01_ContractReference = specification.ContractReferenceText;
                    Child01_ServiceNo = specification.SpecificationServiceNo;
                    Child01_RouteDescription = specification.RouteText;
                }
                if (Child02_Q_Specification.HasValue)
                {
                    Specification specification = new Specification(db, Child02_Q_Specification.Value, pBriefMode:true);
                    Child02_ContractReference = specification.ContractReferenceText;
                    Child02_ServiceNo = specification.SpecificationServiceNo;
                    Child02_RouteDescription = specification.RouteText;
                }
                if (Child03_Q_Specification.HasValue)
                {
                    Specification specification = new Specification(db, Child03_Q_Specification.Value, pBriefMode:true);
                    Child03_ContractReference = specification.ContractReferenceText;
                    Child03_ServiceNo = specification.SpecificationServiceNo;
                    Child03_RouteDescription = specification.RouteText;
                }
                if (Child04_Q_Specification.HasValue)
                {
                    Specification specification = new Specification(db, Child04_Q_Specification.Value, pBriefMode: true);
                    Child04_ContractReference = specification.ContractReferenceText;
                    Child04_ServiceNo = specification.SpecificationServiceNo;
                    Child04_RouteDescription = specification.RouteText;
                }
                if (Child05_Q_Specification.HasValue)
                {
                    Specification specification = new Specification(db, Child05_Q_Specification.Value, pBriefMode: true);
                    Child05_ContractReference = specification.ContractReferenceText;
                    Child05_ServiceNo = specification.SpecificationServiceNo;
                    Child05_RouteDescription = specification.RouteText;
                }
                if (Child06_Q_Specification.HasValue)
                {
                    Specification specification = new Specification(db, Child06_Q_Specification.Value, pBriefMode:true);
                    Child06_ContractReference = specification.ContractReferenceText;
                    Child06_ServiceNo = specification.SpecificationServiceNo;
                    Child06_RouteDescription = specification.RouteText;
                }
                if (Child07_Q_Specification.HasValue)
                {
                    Specification specification = new Specification(db, Child07_Q_Specification.Value, pBriefMode: true);
                    Child07_ContractReference = specification.ContractReferenceText;
                    Child07_ServiceNo = specification.SpecificationServiceNo;
                    Child07_RouteDescription = specification.RouteText;
                }
                if (Child08_Q_Specification.HasValue)
                {
                    Specification specification = new Specification(db, Child08_Q_Specification.Value, pBriefMode: true);
                    Child08_ContractReference = specification.ContractReferenceText;
                    Child08_ServiceNo = specification.SpecificationServiceNo;
                    Child08_RouteDescription = specification.RouteText;
                }
                if (Child09_Q_Specification.HasValue)
                {
                    Specification specification = new Specification(db, Child09_Q_Specification.Value, pBriefMode:true);
                    Child09_ContractReference = specification.ContractReferenceText;
                    Child09_ServiceNo = specification.SpecificationServiceNo;
                    Child09_RouteDescription = specification.RouteText;
                }
                if (Child10_Q_Specification.HasValue)
                {
                    Specification specification = new Specification(db, Child10_Q_Specification.Value, pBriefMode: true);
                    Child10_ContractReference = specification.ContractReferenceText;
                    Child10_ServiceNo = specification.SpecificationServiceNo;
                    Child10_RouteDescription = specification.RouteText;
                }
            }

            else
            {
                if ( (Q_Specification > 0) && (string.IsNullOrEmpty(RouteStart)) )
                {
                    Specification specification = new Specification(db, Q_Specification, pBriefMode:true);
                    ContractReference       = specification.ContractReference;
                    SpecificationServiceNo  = specification.SpecificationServiceNo;
                    RouteStart              = specification.RouteStart;
                    RouteEnd                = specification.RouteEnd;
                    ContractTypeDescription = specification.ContractTypeDescription;
                    DateStart               = specification.DateStart;
                    DateEnd                 = specification.DateEnd;
                    IsFuelTerm                = specification.IsFuelTerm;
                }
            }

            #endregion

            if (IsTenderAccepted)
            {
                ContractVariationGrid = new ContractVariationGrid(db, pK_Tender);
            }
        }

        public int Save(DCC_Database db, string pMode, bool saveVariation = false)
        {
            int result = (IsNew ? Insert(db) : Update(db, pMode, saveVariation));
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

        int Update(DCC_Database db, string pMode, bool pSaveVariation = false)
        {
            // if saving from the tender view
            if (pMode == "Tender")
            {
                // load the parent specification, we'll need these properties when we populate the variation block
                // easier to reload the parent specification than to add all the necessary properties as hidden fields on the form
                Specification parent = new Specification(db, Q_Specification, pBriefMode:true);

                if (IsPackage)
                {
                    #region set the days of operation for the child specifications

                    OperatesOnMon =
                    OperatesOnTue =
                    OperatesOnWed =
                    OperatesOnThu =
                    OperatesOnFri =
                    OperatesOnSat =
                    OperatesOnSun =
                    OperatesOnSchoolDays =
                    OperatesOnNonSchoolDays =
                    OperatesOnBankHolidays =
                    OperatesOnChristmasAndNewYear = false;

                    if (Child01_Q_Specification.HasValue)
                    {
                        Specification child = new Specification(db, Child01_Q_Specification.Value, pBriefMode:true);
                        DateStart = child.DateStart;
                        DateEnd = child.DateEnd;
                        if (child.OperatesOnMon)                 OperatesOnMon = true;
                        if (child.OperatesOnTue)                 OperatesOnTue = true;
                        if (child.OperatesOnWed)                 OperatesOnWed = true;
                        if (child.OperatesOnThu)                 OperatesOnThu = true;
                        if (child.OperatesOnFri)                 OperatesOnFri = true;
                        if (child.OperatesOnSat)                 OperatesOnSat = true;
                        if (child.OperatesOnSun)                 OperatesOnSun = true;
                        if (child.OperatesOnSchoolDays)          OperatesOnSchoolDays = true;
                        if (child.OperatesOnNonSchoolDays)       OperatesOnNonSchoolDays = true;
                        if (child.OperatesOnBankHolidays)        OperatesOnBankHolidays = true;
                        if (child.OperatesOnChristmasAndNewYear) OperatesOnChristmasAndNewYear = true;
                    }
                    if (Child02_Q_Specification.HasValue)
                    {
                        Specification child = new Specification(db, Child02_Q_Specification.Value, pBriefMode:true);
                        if (child.DateStart.HasValue && child.DateStart < DateStart.Value) DateStart = child.DateStart;
                        if (child.DateEnd  .HasValue && child.DateEnd   > DateEnd  .Value) DateEnd   = child.DateEnd;
                        if (child.OperatesOnMon)                 OperatesOnMon = true;
                        if (child.OperatesOnTue)                 OperatesOnTue = true;
                        if (child.OperatesOnWed)                 OperatesOnWed = true;
                        if (child.OperatesOnThu)                 OperatesOnThu = true;
                        if (child.OperatesOnFri)                 OperatesOnFri = true;
                        if (child.OperatesOnSat)                 OperatesOnSat = true;
                        if (child.OperatesOnSun)                 OperatesOnSun = true;
                        if (child.OperatesOnSchoolDays)          OperatesOnSchoolDays = true;
                        if (child.OperatesOnNonSchoolDays)       OperatesOnNonSchoolDays = true;
                        if (child.OperatesOnBankHolidays)        OperatesOnBankHolidays = true;
                        if (child.OperatesOnChristmasAndNewYear) OperatesOnChristmasAndNewYear = true;
                    }
                    if (Child03_Q_Specification.HasValue)
                    {
                        Specification child = new Specification(db, Child03_Q_Specification.Value, pBriefMode:true);
                        if (child.DateStart.HasValue && child.DateStart < DateStart.Value) DateStart = child.DateStart;
                        if (child.DateEnd  .HasValue && child.DateEnd   > DateEnd  .Value) DateEnd   = child.DateEnd;
                        if (child.OperatesOnMon)                 OperatesOnMon = true;
                        if (child.OperatesOnTue)                 OperatesOnTue = true;
                        if (child.OperatesOnWed)                 OperatesOnWed = true;
                        if (child.OperatesOnThu)                 OperatesOnThu = true;
                        if (child.OperatesOnFri)                 OperatesOnFri = true;
                        if (child.OperatesOnSat)                 OperatesOnSat = true;
                        if (child.OperatesOnSun)                 OperatesOnSun = true;
                        if (child.OperatesOnSchoolDays)          OperatesOnSchoolDays = true;
                        if (child.OperatesOnNonSchoolDays)       OperatesOnNonSchoolDays = true;
                        if (child.OperatesOnBankHolidays)        OperatesOnBankHolidays = true;
                        if (child.OperatesOnChristmasAndNewYear) OperatesOnChristmasAndNewYear = true;
                    }
                    if (Child04_Q_Specification.HasValue)
                    {
                        Specification child = new Specification(db, Child04_Q_Specification.Value, pBriefMode: true);
                        if (child.DateStart.HasValue && child.DateStart < DateStart.Value) DateStart = child.DateStart;
                        if (child.DateEnd  .HasValue && child.DateEnd   > DateEnd  .Value) DateEnd   = child.DateEnd;
                        if (child.OperatesOnMon)                 OperatesOnMon = true;
                        if (child.OperatesOnTue)                 OperatesOnTue = true;
                        if (child.OperatesOnWed)                 OperatesOnWed = true;
                        if (child.OperatesOnThu)                 OperatesOnThu = true;
                        if (child.OperatesOnFri)                 OperatesOnFri = true;
                        if (child.OperatesOnSat)                 OperatesOnSat = true;
                        if (child.OperatesOnSun)                 OperatesOnSun = true;
                        if (child.OperatesOnSchoolDays)          OperatesOnSchoolDays = true;
                        if (child.OperatesOnNonSchoolDays)       OperatesOnNonSchoolDays = true;
                        if (child.OperatesOnBankHolidays)        OperatesOnBankHolidays = true;
                        if (child.OperatesOnChristmasAndNewYear) OperatesOnChristmasAndNewYear = true;
                    }
                    if (Child05_Q_Specification.HasValue)
                    {
                        Specification child = new Specification(db, Child05_Q_Specification.Value, pBriefMode: true);
                        if (child.DateStart.HasValue && child.DateStart < DateStart.Value) DateStart = child.DateStart;
                        if (child.DateEnd  .HasValue && child.DateEnd   > DateEnd  .Value) DateEnd   = child.DateEnd;
                        if (child.OperatesOnMon)                 OperatesOnMon = true;
                        if (child.OperatesOnTue)                 OperatesOnTue = true;
                        if (child.OperatesOnWed)                 OperatesOnWed = true;
                        if (child.OperatesOnThu)                 OperatesOnThu = true;
                        if (child.OperatesOnFri)                 OperatesOnFri = true;
                        if (child.OperatesOnSat)                 OperatesOnSat = true;
                        if (child.OperatesOnSun)                 OperatesOnSun = true;
                        if (child.OperatesOnSchoolDays)          OperatesOnSchoolDays = true;
                        if (child.OperatesOnNonSchoolDays)       OperatesOnNonSchoolDays = true;
                        if (child.OperatesOnBankHolidays)        OperatesOnBankHolidays = true;
                        if (child.OperatesOnChristmasAndNewYear) OperatesOnChristmasAndNewYear = true;
                    }
                    if (Child06_Q_Specification.HasValue)
                    {
                        Specification child = new Specification(db, Child06_Q_Specification.Value, pBriefMode:true);
                        if (child.DateStart.HasValue && child.DateStart < DateStart.Value) DateStart = child.DateStart;
                        if (child.DateEnd  .HasValue && child.DateEnd   > DateEnd  .Value) DateEnd   = child.DateEnd;
                        if (child.OperatesOnMon)                 OperatesOnMon = true;
                        if (child.OperatesOnTue)                 OperatesOnTue = true;
                        if (child.OperatesOnWed)                 OperatesOnWed = true;
                        if (child.OperatesOnThu)                 OperatesOnThu = true;
                        if (child.OperatesOnFri)                 OperatesOnFri = true;
                        if (child.OperatesOnSat)                 OperatesOnSat = true;
                        if (child.OperatesOnSun)                 OperatesOnSun = true;
                        if (child.OperatesOnSchoolDays)          OperatesOnSchoolDays = true;
                        if (child.OperatesOnNonSchoolDays)       OperatesOnNonSchoolDays = true;
                        if (child.OperatesOnBankHolidays)        OperatesOnBankHolidays = true;
                        if (child.OperatesOnChristmasAndNewYear) OperatesOnChristmasAndNewYear = true;
                    }
                    if (Child07_Q_Specification.HasValue)
                    {
                        Specification child = new Specification(db, Child07_Q_Specification.Value, pBriefMode: true);
                        if (child.DateStart.HasValue && child.DateStart < DateStart.Value) DateStart = child.DateStart;
                        if (child.DateEnd  .HasValue && child.DateEnd   > DateEnd  .Value) DateEnd   = child.DateEnd;
                        if (child.OperatesOnMon)                 OperatesOnMon = true;
                        if (child.OperatesOnTue)                 OperatesOnTue = true;
                        if (child.OperatesOnWed)                 OperatesOnWed = true;
                        if (child.OperatesOnThu)                 OperatesOnThu = true;
                        if (child.OperatesOnFri)                 OperatesOnFri = true;
                        if (child.OperatesOnSat)                 OperatesOnSat = true;
                        if (child.OperatesOnSun)                 OperatesOnSun = true;
                        if (child.OperatesOnSchoolDays)          OperatesOnSchoolDays = true;
                        if (child.OperatesOnNonSchoolDays)       OperatesOnNonSchoolDays = true;
                        if (child.OperatesOnBankHolidays)        OperatesOnBankHolidays = true;
                        if (child.OperatesOnChristmasAndNewYear) OperatesOnChristmasAndNewYear = true;
                    }
                    if (Child08_Q_Specification.HasValue)
                    {
                        Specification child = new Specification(db, Child08_Q_Specification.Value, pBriefMode: true);
                        if (child.DateStart.HasValue && child.DateStart < DateStart.Value) DateStart = child.DateStart;
                        if (child.DateEnd  .HasValue && child.DateEnd   > DateEnd  .Value) DateEnd   = child.DateEnd;
                        if (child.OperatesOnMon)                 OperatesOnMon = true;
                        if (child.OperatesOnTue)                 OperatesOnTue = true;
                        if (child.OperatesOnWed)                 OperatesOnWed = true;
                        if (child.OperatesOnThu)                 OperatesOnThu = true;
                        if (child.OperatesOnFri)                 OperatesOnFri = true;
                        if (child.OperatesOnSat)                 OperatesOnSat = true;
                        if (child.OperatesOnSun)                 OperatesOnSun = true;
                        if (child.OperatesOnSchoolDays)          OperatesOnSchoolDays = true;
                        if (child.OperatesOnNonSchoolDays)       OperatesOnNonSchoolDays = true;
                        if (child.OperatesOnBankHolidays)        OperatesOnBankHolidays = true;
                        if (child.OperatesOnChristmasAndNewYear) OperatesOnChristmasAndNewYear = true;
                    }
                    if (Child09_Q_Specification.HasValue)
                    {
                        Specification child = new Specification(db, Child09_Q_Specification.Value, pBriefMode:true);
                        if (child.DateStart.HasValue && child.DateStart < DateStart.Value) DateStart = child.DateStart;
                        if (child.DateEnd  .HasValue && child.DateEnd   > DateEnd  .Value) DateEnd   = child.DateEnd;
                        if (child.OperatesOnMon)                 OperatesOnMon = true;
                        if (child.OperatesOnTue)                 OperatesOnTue = true;
                        if (child.OperatesOnWed)                 OperatesOnWed = true;
                        if (child.OperatesOnThu)                 OperatesOnThu = true;
                        if (child.OperatesOnFri)                 OperatesOnFri = true;
                        if (child.OperatesOnSat)                 OperatesOnSat = true;
                        if (child.OperatesOnSun)                 OperatesOnSun = true;
                        if (child.OperatesOnSchoolDays)          OperatesOnSchoolDays = true;
                        if (child.OperatesOnNonSchoolDays)       OperatesOnNonSchoolDays = true;
                        if (child.OperatesOnBankHolidays)        OperatesOnBankHolidays = true;
                        if (child.OperatesOnChristmasAndNewYear) OperatesOnChristmasAndNewYear = true;
                    }
                    if (Child10_Q_Specification.HasValue)
                    {
                        Specification child = new Specification(db, Child10_Q_Specification.Value, pBriefMode: true);
                        if (child.DateStart.HasValue && child.DateStart < DateStart.Value) DateStart = child.DateStart;
                        if (child.DateEnd  .HasValue && child.DateEnd   > DateEnd  .Value) DateEnd   = child.DateEnd;
                        if (child.OperatesOnMon)                 OperatesOnMon = true;
                        if (child.OperatesOnTue)                 OperatesOnTue = true;
                        if (child.OperatesOnWed)                 OperatesOnWed = true;
                        if (child.OperatesOnThu)                 OperatesOnThu = true;
                        if (child.OperatesOnFri)                 OperatesOnFri = true;
                        if (child.OperatesOnSat)                 OperatesOnSat = true;
                        if (child.OperatesOnSun)                 OperatesOnSun = true;
                        if (child.OperatesOnSchoolDays)          OperatesOnSchoolDays = true;
                        if (child.OperatesOnNonSchoolDays)       OperatesOnNonSchoolDays = true;
                        if (child.OperatesOnBankHolidays)        OperatesOnBankHolidays = true;
                        if (child.OperatesOnChristmasAndNewYear) OperatesOnChristmasAndNewYear = true;
                    }

                    #endregion
                }
                else
                {
                    #region set the days of operation for the parent specification
                    DateStart = parent.DateStart;
                    DateEnd = parent.DateEnd;
                    if (parent.OperatesOnMon)                 OperatesOnMon = true;
                    if (parent.OperatesOnTue)                 OperatesOnTue = true;
                    if (parent.OperatesOnWed)                 OperatesOnWed = true;
                    if (parent.OperatesOnThu)                 OperatesOnThu = true;
                    if (parent.OperatesOnFri)                 OperatesOnFri = true;
                    if (parent.OperatesOnSat)                 OperatesOnSat = true;
                    if (parent.OperatesOnSun)                 OperatesOnSun = true;
                    if (parent.OperatesOnSchoolDays)          OperatesOnSchoolDays = true;
                    if (parent.OperatesOnNonSchoolDays)       OperatesOnNonSchoolDays = true;
                    if (parent.OperatesOnBankHolidays)        OperatesOnBankHolidays = true;
                    if (parent.OperatesOnChristmasAndNewYear) OperatesOnChristmasAndNewYear = true;
                    #endregion
                }
                CalculateDaysInOperation(db);

                // save the tender block
                K_Tender = SaveTenderBlock(db);

                // if the tender has been accepted and there isn't already a contract variation '00'
                if ( (IsTenderAccepted) && (ContractVariationGrid.Count == 0) )
                {
                    if (IsPackage)
                    {
                        #region set variation block properties (package)

                        ContractVariationIsCancelled = false;
                        ContractVariationIsExtended = false;

                        ContractVariationDateStart                              = DateStart;
                        ContractVariationDateEnd                                = DateEnd;
                        ContractVariationServiceNo                              = "Various";
                        ContractVariationRouteStart                             = "Various";
                        ContractVariationRouteEnd                               = "Various";
                        ContractVariationExceptionalConditions                  = null;

                        ContractVariationVehicleCapacity                        = null;
                        ContractVariationVehicleElderlyAndDisabled              = null;
                        ContractVariationVehicleDimensions                      = null;

                        ContractVariation_Q_Agreement                           = parent.Q_Agreement;
                        ContractVariationServiceDetails                         = null;
                        ContractVariationTicketNotes                            = null; // assume this is null at this stage
                        ContractVariationRouteTimetableNotes                    = null; // assume this is null at this stage

                        ContractVariationOperatesOnMon                          = OperatesOnMon;
                        ContractVariationOperatesOnTue                          = OperatesOnTue;
                        ContractVariationOperatesOnWed                          = OperatesOnWed;
                        ContractVariationOperatesOnThu                          = OperatesOnThu;
                        ContractVariationOperatesOnFri                          = OperatesOnFri;
                        ContractVariationOperatesOnSat                          = OperatesOnSat;
                        ContractVariationOperatesOnSun                          = OperatesOnSun;
                        ContractVariationOperatesOnSchoolDays                   = OperatesOnSchoolDays;
                        ContractVariationOperatesOnNonSchoolDays                = OperatesOnNonSchoolDays;
                        ContractVariationOperatesOnBankHolidays                 = OperatesOnBankHolidays;
                        ContractVariationOperatesOnChristmasAndNewYear          = OperatesOnChristmasAndNewYear;
                        ContractVariationCalculatedNoOfDaysOfOperationFirstYear = CalculatedNoOfDaysOfOperationFirstYear;
                        ContractVariationCalculatedNoOfDaysOfOperationFullTerm  = CalculatedNoOfDaysOfOperationFullTerm;

                        ContractVariationAverageMilesPerDay                     = TenderAverageMilesPerDay;
                        ContractVariationRatePerMile                            = TenderRatePerMile;
                        ContractVariationFuelPriceElement                       = TenderFuelPriceElement;
                        ContractVariationAveragePricePerDay                     = TenderPricePerDay;
                        ContractVariationAveragePricePerYear                    = TenderPricePerYear;
                        ContractVariationAverageRevenuePerDay                   = EstimatedAverageRevenuePerDay;
                        ContractVariationAverageRevenuePerYear                  = EstimatedAverageRevenuePerYear;
                        ContractVariationAverageCostPerDay                      = AverageCostPerDay;
                        ContractVariationAverageCostPerYear                     = AverageCostPerYear;

                        ContractVariationVariationDateStart                     = null;
                        ContractVariationVariationDateEnd                       = null;

                        ContractVariationNotes = TenderNotes;

                        #endregion
                    }
                    else
                    {
                        #region set variation block properties (single specification)

                        ContractVariationIsCancelled                            = false;
                        ContractVariationIsExtended                             = false;

                        ContractVariationDateStart                              = parent.DateStart;
                        ContractVariationDateEnd                                = parent.DateEnd;
                        ContractVariationServiceNo                              = parent.SpecificationServiceNo;
                        ContractVariationRouteStart                             = parent.RouteStart;
                        ContractVariationRouteEnd                               = parent.RouteEnd;
                        ContractVariationExceptionalConditions                  = parent.ExceptionalConditions;

                        ContractVariationVehicleCapacity                        = parent.VehicleCapacity;
                        ContractVariationVehicleElderlyAndDisabled              = parent.VehicleElderlyAndDisabled;
                        ContractVariationVehicleDimensions                      = parent.VehicleDimensions;

                        ContractVariation_Q_Agreement                           = parent.Q_Agreement;
                        ContractVariationServiceDetails                         = parent.ServiceDetails1;
                        ContractVariationTicketNotes                            = null;
                        ContractVariationRouteTimetableNotes                    = null;

                        ContractVariationOperatesOnMon                          = parent.OperatesOnMon;
                        ContractVariationOperatesOnTue                          = parent.OperatesOnTue;
                        ContractVariationOperatesOnWed                          = parent.OperatesOnWed;
                        ContractVariationOperatesOnThu                          = parent.OperatesOnThu;
                        ContractVariationOperatesOnFri                          = parent.OperatesOnFri;
                        ContractVariationOperatesOnSat                          = parent.OperatesOnSat;
                        ContractVariationOperatesOnSun                          = parent.OperatesOnSun;
                        ContractVariationOperatesOnSchoolDays                   = parent.OperatesOnSchoolDays;
                        ContractVariationOperatesOnNonSchoolDays                = parent.OperatesOnNonSchoolDays;
                        ContractVariationOperatesOnBankHolidays                 = parent.OperatesOnBankHolidays;
                        ContractVariationOperatesOnChristmasAndNewYear          = parent.OperatesOnChristmasAndNewYear;
                        ContractVariationCalculatedNoOfDaysOfOperationFirstYear = parent.CalculatedNoOfDaysOfOperationFirstYear;
                        ContractVariationCalculatedNoOfDaysOfOperationFullTerm  = parent.CalculatedNoOfDaysOfOperationFullTerm;

                        ContractVariationAverageMilesPerDay                     = TenderAverageMilesPerDay;
                        ContractVariationRatePerMile                            = TenderRatePerMile;
                        ContractVariationFuelPriceElement                       = TenderFuelPriceElement;
                        ContractVariationAveragePricePerDay                     = TenderPricePerDay;
                        ContractVariationAveragePricePerYear                    = TenderPricePerYear;
                        ContractVariationAverageRevenuePerDay                   = EstimatedAverageRevenuePerDay;
                        ContractVariationAverageRevenuePerYear                  = EstimatedAverageRevenuePerYear;
                        ContractVariationAverageCostPerDay                      = AverageCostPerDay;
                        ContractVariationAverageCostPerYear                     = AverageCostPerYear;

                        ContractVariationVariationDateStart                     = null;
                        ContractVariationVariationDateEnd                       = null;

                        ContractVariationNotes = TenderNotes;

                        #endregion
                    }

                    // save the variation block properties then create the first contract variation '00' by passing a 0 parameter
                    SaveVariationBlock(db);
                    ContractVariationVariationDateStart = parent.DateStart; // set datestart to contract datestart on variation 00, leaving it blank in the variation block for the next one
                    SaveContractVariation(db, 0);
                }
            }

            // if saving from the contract view
            else if (pMode == "Contract")
            {
                // save the variation block...
                K_Tender = SaveVariationBlock(db);

                // ...then create a new contract variation by passing a 1 parameter
                if (pSaveVariation) SaveContractVariation(db, 1);
            }

            return K_Tender;
        }

        private int SaveTenderBlock(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uTender2");
            dbf.AddTable("Tender");
            dbf.AddParameter("K_Tender",                                                K_Tender);
            dbf.AddParameter("Q_Specification",                                         Q_Specification);
            dbf.AddParameter("Q_Contractor",                                            Q_Contractor);
            dbf.AddParameter("CalculatedNoOfDaysOfOperationFirstYear",                  CalculatedNoOfDaysOfOperationFirstYear);
            dbf.AddParameter("CalculatedNoOfDaysOfOperationFullTerm",                   CalculatedNoOfDaysOfOperationFullTerm);
            dbf.AddParameter("TenderPricePerDay",                                       TenderPricePerDay);
            dbf.AddParameter("TenderPricePerYear",                                      TenderPricePerYear);
            dbf.AddParameter("AverageCostPerDay",                                       AverageCostPerDay);
            dbf.AddParameter("AverageCostPerYear",                                      AverageCostPerYear);
            dbf.AddParameter("EstimatedAverageRevenuePerDay",                           EstimatedAverageRevenuePerDay);
            dbf.AddParameter("EstimatedAverageRevenuePerYear",                          EstimatedAverageRevenuePerYear);
            dbf.AddParameter("TenderVehicleDetails",                                    TenderVehicleDetails           , 500);
            dbf.AddParameter("IsConforming",                                            IsConforming);
            dbf.AddParameter("IncludeInTenderAnalysis",                                 IncludeInTenderAnalysis);
            dbf.AddParameter("AnyTenderQualifications",                                 AnyTenderQualifications);
            dbf.AddParameter("TenderQualificationNotes",                                TenderQualificationNotes       , 500);
            dbf.AddParameter("TenderNotes",                                             TenderNotes                    , 500);
            dbf.AddParameter("IsTenderAccepted",                                        IsTenderAccepted);
            dbf.AddParameter("TenderAverageMilesPerDay",                                TenderAverageMilesPerDay);
            dbf.AddParameter("TenderRatePerMile",                                       TenderRatePerMile);
            dbf.AddParameter("TenderFuelPriceElement",                                  TenderFuelPriceElement);
            db.DataLayer.NonQuery(dbf);
            return (dbf.OK ? K_Tender : -1);
        }

        private int SaveVariationBlock(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uTenderVariationBlock");
            dbf.AddTable("Tender");
            dbf.AddParameter("K_Tender",                                                K_Tender);

            dbf.AddParameter("ContractVariationIsCancelled",                            ContractVariationIsCancelled);
            dbf.AddParameter("ContractVariationIsExtended",                             ContractVariationIsExtended);
            dbf.AddParameter("ContractVariationDateStart",                              ContractVariationDateStart);
            dbf.AddParameter("ContractVariationDateEnd",                                ContractVariationDateEnd);
            dbf.AddParameter("ContractVariationServiceNo",                              ContractVariationServiceNo                , 500);
            dbf.AddParameter("ContractVariationRouteStart",                             ContractVariationRouteStart               , 500);
            dbf.AddParameter("ContractVariationRouteEnd",                               ContractVariationRouteEnd                 , 500);
            dbf.AddParameter("ContractVariationExceptionalConditions",                  ContractVariationExceptionalConditions    , 500);

            dbf.AddParameter("ContractVariationVehicleCapacity",                        ContractVariationVehicleCapacity          , 500);
            dbf.AddParameter("ContractVariationVehicleElderlyAndDisabled",              ContractVariationVehicleElderlyAndDisabled, 500);
            dbf.AddParameter("ContractVariationVehicleDimensions",                      ContractVariationVehicleDimensions        , 500);

            dbf.AddParameter("ContractVariation_Q_Agreement",                           ContractVariation_Q_Agreement);
            dbf.AddParameter("ContractVariationServiceDetails",                         ContractVariationServiceDetails           , 500);
            dbf.AddParameter("ContractVariationTicketNotes",                            ContractVariationTicketNotes              , 500);
            dbf.AddParameter("ContractVariationRouteTimetableNotes",                    ContractVariationRouteTimetableNotes      , 500);

            dbf.AddParameter("ContractVariationOperatesOnMon",                          ContractVariationOperatesOnMon);
            dbf.AddParameter("ContractVariationOperatesOnTue",                          ContractVariationOperatesOnTue);
            dbf.AddParameter("ContractVariationOperatesOnWed",                          ContractVariationOperatesOnWed);
            dbf.AddParameter("ContractVariationOperatesOnThu",                          ContractVariationOperatesOnThu);
            dbf.AddParameter("ContractVariationOperatesOnFri",                          ContractVariationOperatesOnFri);
            dbf.AddParameter("ContractVariationOperatesOnSat",                          ContractVariationOperatesOnSat);
            dbf.AddParameter("ContractVariationOperatesOnSun",                          ContractVariationOperatesOnSun);
            dbf.AddParameter("ContractVariationOperatesOnSchoolDays",                   ContractVariationOperatesOnSchoolDays);
            dbf.AddParameter("ContractVariationOperatesOnNonSchoolDays",                ContractVariationOperatesOnNonSchoolDays);
            dbf.AddParameter("ContractVariationOperatesOnBankHolidays",                 ContractVariationOperatesOnBankHolidays);
            dbf.AddParameter("ContractVariationOperatesOnChristmasAndNewYear",          ContractVariationOperatesOnChristmasAndNewYear);
            dbf.AddParameter("ContractVariationCalculatedNoOfDaysOfOperationFirstYear", ContractVariationCalculatedNoOfDaysOfOperationFirstYear);
            dbf.AddParameter("ContractVariationCalculatedNoOfDaysOfOperationFullTerm",  ContractVariationCalculatedNoOfDaysOfOperationFullTerm);

            dbf.AddParameter("ContractVariationAverageMilesPerDay",                     ContractVariationAverageMilesPerDay);
            dbf.AddParameter("ContractVariationRatePerMile",                            ContractVariationRatePerMile);
            dbf.AddParameter("ContractVariationFuelPriceElement",                       ContractVariationFuelPriceElement);
            dbf.AddParameter("ContractVariationAveragePricePerDay",                     ContractVariationAveragePricePerDay);
            dbf.AddParameter("ContractVariationAveragePricePerYear",                    ContractVariationAveragePricePerYear);
            dbf.AddParameter("ContractVariationAverageRevenuePerDay",                   ContractVariationAverageRevenuePerDay);
            dbf.AddParameter("ContractVariationAverageRevenuePerYear",                  ContractVariationAverageRevenuePerYear);
            dbf.AddParameter("ContractVariationAverageCostPerDay",                      ContractVariationAverageCostPerDay);
            dbf.AddParameter("ContractVariationAverageCostPerYear",                     ContractVariationAverageCostPerYear);

            dbf.AddParameter("ContractVariationVariationDateStart",                     ContractVariationVariationDateStart);
            dbf.AddParameter("ContractVariationVariationDateEnd",                       ContractVariationVariationDateEnd);

            dbf.AddParameter("ContractVariationNotes",                                  ContractVariationNotes                    , 500);

            db.DataLayer.NonQuery(dbf);
            return (dbf.OK ? K_Tender : -1);
        }

        void SaveContractVariation(DCC_Database db, int mode)
        {
            ContractVariation newContractVariation = new ContractVariation();
            newContractVariation.ContractVariation_Q_Tender                              = K_Tender;
            newContractVariation.K_ContractVariation                                     = -1;
            newContractVariation.ContractVariationNo                                     = mode; // Set to 1 if subsequent variation

            newContractVariation.ContractVariationIsCancelled                            = ContractVariationIsCancelled;
            newContractVariation.ContractVariationIsExtended                             = ContractVariationIsExtended;
            newContractVariation.ContractVariationDateStart                              = ContractVariationDateStart;
            newContractVariation.ContractVariationDateEnd                                = ContractVariationDateEnd;
            newContractVariation.ContractVariationServiceNo                              = ContractVariationServiceNo;
            newContractVariation.ContractVariationRouteStart                             = ContractVariationRouteStart;
            newContractVariation.ContractVariationRouteEnd                               = ContractVariationRouteEnd;

            newContractVariation.ContractVariationExceptionalConditions                  = ContractVariationExceptionalConditions;
            newContractVariation.ContractVariationVehicleCapacity                        = ContractVariationVehicleCapacity;
            newContractVariation.ContractVariationVehicleElderlyAndDisabled              = ContractVariationVehicleElderlyAndDisabled;
            newContractVariation.ContractVariationVehicleDimensions                      = ContractVariationVehicleDimensions;

            newContractVariation.ContractVariation_Q_Agreement                           = ContractVariation_Q_Agreement;
            newContractVariation.ContractVariationServiceDetails                         = ContractVariationServiceDetails;
            newContractVariation.ContractVariationTicketNotes                            = ContractVariationTicketNotes;
            newContractVariation.ContractVariationRouteTimetableNotes                    = ContractVariationRouteTimetableNotes;

            newContractVariation.ContractVariationOperatesOnMon                          = ContractVariationOperatesOnMon;
            newContractVariation.ContractVariationOperatesOnTue                          = ContractVariationOperatesOnTue;
            newContractVariation.ContractVariationOperatesOnWed                          = ContractVariationOperatesOnWed;
            newContractVariation.ContractVariationOperatesOnThu                          = ContractVariationOperatesOnThu;
            newContractVariation.ContractVariationOperatesOnFri                          = ContractVariationOperatesOnFri;
            newContractVariation.ContractVariationOperatesOnSat                          = ContractVariationOperatesOnSat;
            newContractVariation.ContractVariationOperatesOnSun                          = ContractVariationOperatesOnSun;
            newContractVariation.ContractVariationOperatesOnSchoolDays                   = ContractVariationOperatesOnSchoolDays;
            newContractVariation.ContractVariationOperatesOnNonSchoolDays                = ContractVariationOperatesOnNonSchoolDays;
            newContractVariation.ContractVariationOperatesOnBankHolidays                 = ContractVariationOperatesOnBankHolidays;
            newContractVariation.ContractVariationOperatesOnChristmasAndNewYear          = ContractVariationOperatesOnChristmasAndNewYear;
            newContractVariation.ContractVariationCalculatedNoOfDaysOfOperationFirstYear = ContractVariationCalculatedNoOfDaysOfOperationFirstYear;
            newContractVariation.ContractVariationCalculatedNoOfDaysOfOperationFullTerm  = ContractVariationCalculatedNoOfDaysOfOperationFullTerm;

            newContractVariation.ContractVariationAverageMilesPerDay                     = ContractVariationAverageMilesPerDay;
            newContractVariation.ContractVariationRatePerMile                            = ContractVariationRatePerMile;
            newContractVariation.ContractVariationFuelPriceElement                       = ContractVariationFuelPriceElement;
            newContractVariation.ContractVariationAveragePricePerDay                     = ContractVariationAveragePricePerDay;
            newContractVariation.ContractVariationAveragePricePerYear                    = ContractVariationAveragePricePerYear;
            newContractVariation.ContractVariationAverageRevenuePerDay                   = ContractVariationAverageRevenuePerDay;
            newContractVariation.ContractVariationAverageRevenuePerYear                  = ContractVariationAverageRevenuePerYear;
            newContractVariation.ContractVariationAverageCostPerDay                      = ContractVariationAverageCostPerDay;
            newContractVariation.ContractVariationAverageCostPerYear                     = ContractVariationAverageCostPerYear;

            newContractVariation.ContractVariationVariationDateStart                     = ContractVariationVariationDateStart;
            newContractVariation.ContractVariationVariationDateEnd                       = ContractVariationVariationDateEnd;

            newContractVariation.ContractVariationNotes                                  = ContractVariationNotes;

            newContractVariation.Save(db);
        }

        #region CalculateDaysOfOperation - creates a string based on the days of operation then calls the next method to calculate the number of days

        public void CalculateDaysInOperation(DCC_Database db)
        {
            CalculatedNoOfDaysOfOperationFirstYear = 0;
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

            CalculatedNoOfDaysOfOperationFirstYear = Specification.CalculateDaysOfOperation(db, dateAndDays);

            string dateAndDays2 =
               DateStart.Value.ToString("dd/MM/yyyy") +
               DateEnd.Value.ToString("dd/MM/yyyy") +
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

            CalculatedNoOfDaysOfOperationFullTerm = Specification.CalculateDaysOfOperation(db, dateAndDays2);
        }

        #endregion

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
            if (pQ_Specification > 0)
            {
                dbf = new DCC_DatabaseFrame("sTenderListBySpecification");
                dbf.AddParameter("Q_Specification ", pQ_Specification);
            }
            else if (awardedOnly)
            {
                dbf = new DCC_DatabaseFrame("sTenderListAwardedOnly2");
            }
            else
            {
                dbf = new DCC_DatabaseFrame("sTenderList2");
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
