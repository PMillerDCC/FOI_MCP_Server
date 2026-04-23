using DCC_Standard;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using System.Globalization;
using System.Windows.Forms;

namespace ProspectBusinessLayer
{

    #region Specification

    public class Specification
    {
        // Properties

        public int       K_Specification                        { get; set; }

        public int       ContractReference                      { get; set; }
        public string    ContractReferenceText                  { get { return ContractReference.ToString(); } }
        public string    ContractReferenceSort                  { get { return ContractReference.ToString().PadLeft(5, '0'); } }
        public bool      IsBlank                                { get; set; }
        public string    IsBlankText                            { get { return (IsBlank ? "Yes" : " "); } }
        public bool      IsPackage                              { get; set; }
        public string    IsPackageText                          { get { return (IsPackage ? "Yes" : " "); } }
        public int?      Child01_Q_Specification                { get; set; }
        public int?      Child02_Q_Specification                { get; set; }
        public int?      Child03_Q_Specification                { get; set; }
        public int?      Child04_Q_Specification                { get; set; }
        public int?      Child05_Q_Specification                { get; set; }
        public int?      Child06_Q_Specification                { get; set; }
        public int?      Child07_Q_Specification                { get; set; }
        public int?      Child08_Q_Specification                { get; set; }
        public int?      Child09_Q_Specification                { get; set; }
        public int?      Child10_Q_Specification                { get; set; }

        public int?      Q_ContractType                         { get; set; } // changed q_ values to int? to stop them being mandatory
        public DateTime? DateStart                              { get; set; }
        public string    DateStartText                          { get { return DCC_Formatting.NullDateTxt(DateStart, "dd MMM yyyy"); } }
        public string    DateStartSort                          { get { return DCC_Formatting.NullDateTxt(DateStart, "yyyyMMddHHmm"); } }
        public string    DateStartFullText                      { get { return DCC_Formatting.NullDateTxt(DateStart, "dd MMMM yyyy"); } }
        public DateTime? DateEnd                                { get; set; }
        public string    DateEndText                            { get { return DCC_Formatting.NullDateTxt(DateEnd, "dd MMM yyyy"); } }
        public string    DateEndSort                            { get { return DCC_Formatting.NullDateTxt(DateEnd, "yyyyMMddHHmm"); } }
        public string    DateEndFullText                        { get { return DCC_Formatting.NullDateTxt(DateEnd, "dd MMMM yyyy"); } }
        public string    DatesText                              { get { return (string.IsNullOrEmpty(DateStartText) ? "- not set -" : DateStartText + " to " + DateEndText); } }
        public string    SpecificationServiceNo                 { get; set; }
        public string    SpecificationServiceNoText             { get { return (IsBlank ? "BLANK" : (IsPackage ? "Various" : SpecificationServiceNo)); } }
        public string    RouteStart                             { get; set; }
        public string    RouteEnd                               { get; set; }
        public string    RouteText                              { get { return (IsPackage ? "Various" : (string.IsNullOrEmpty(RouteStart) ? "- not set -" : RouteStart + " to " + RouteEnd) ); } }
        public string    ExceptionalConditions                  { get; set; }

        public string    VehicleCapacity                        { get; set; }
        public string    VehicleElderlyAndDisabled              { get; set; }
        public string    VehicleDimensions                      { get; set; }

        public int?      Q_Agreement                            { get; set; }
        public bool      IsFuelTerm                             { get; set; }
        public string    IsFuelTermText                         { get { return (IsFuelTerm ? "Yes" : "No"); } }
        public DateTime? TenderClosingDate                      { get; set; }
        public string    TenderClosingDateText                  { get { return DCC_Formatting.NullDateTxt(TenderClosingDate, "dd MMM yyyy"); } }
        public string    JointResponsibility1                   { get; set; }
        public string    ServiceDetails1                        { get; set; }

        public bool      OperatesOnMon                          { get; set; }
        public bool      OperatesOnTue                          { get; set; }
        public bool      OperatesOnWed                          { get; set; }
        public bool      OperatesOnThu                          { get; set; }
        public bool      OperatesOnFri                          { get; set; }
        public bool      OperatesOnSat                          { get; set; }
        public bool      OperatesOnSun                          { get; set; }
        public bool      OperatesOnSchoolDays                   { get; set; }
        public bool      OperatesOnNonSchoolDays                { get; set; }
        public bool      OperatesOnBankHolidays                 { get; set; }
        public bool      OperatesOnChristmasAndNewYear          { get; set; }
        public int       CalculatedNoOfDaysOfOperationFirstYear { get; set; }
        public int       CalculatedNoOfDaysOfOperationFullTerm  { get; set; }

        public decimal   AverageMilesPerDay                     { get; set; }
        public decimal   AverageHoursPerDay                     { get; set; }
        public int       NoOfVehicles                           { get; set; }
        public decimal   AverageWeeklyRevenueEstimate           { get; set; }

        public string    Notes                                  { get; set; }

        public string    AgreementDescription                   { get; set; }
        public string    ContractTypeDescription                { get; set; }
        public string    ContractorName                         { get; set; }
        public string    ContractorLicenceNo                    { get; set; }



        // Utilities

        public int PrimaryKey { get { return K_Specification; } set { K_Specification = value; } }
        public string Key { get { return PrimaryKey.ToString(); } }
        public bool IsNew { get { return PrimaryKey == -1; } }
        public override string ToString() { return PrimaryKey.ToString().PadLeft(3) + " - " + SpecificationServiceNo; }



        // Drop Down Lists

        public class ddlItem { public int Id { get; set; } public string Name { get; set; } }

        public List<ddlItem> SpecificationItems;
        public IEnumerable<SelectListItem> ddlSpecification { get { return new SelectList(SpecificationItems, "Id", "Name"); } }

        public List<ddlItem> AgreementItems;
        public IEnumerable<SelectListItem> ddlAgreement { get { return new SelectList(AgreementItems, "Id", "Name"); } }

        public List<ddlItem> ContractTypeItems;
        public IEnumerable<SelectListItem> ddlContractType { get { return new SelectList(ContractTypeItems, "Id", "Name"); } }



        // Data Grids

        public TenderGrid TenderGrid;



        // Constructors

        public Specification()
        {
        }

        public Specification(DataRow dataRow) // takes a datarow object as a parameter and calls the GetColumns method with the given DataRow object
        {
            GetColumns(dataRow);
        }

        public Specification(DCC_Database db, int pK_Specification = -999, bool pBriefMode = false)
        {
            if (pK_Specification == -1)      // passing -1 creates a new instance of the 'Specification' class.
            {                                // it automatically adds a new record, with the auto-generated specification reference
                K_Specification = -1;        // set any default values here
                pK_Specification = Save(db);
            }

            if (pK_Specification > 0)        // read an existing record from the database for the provided K_Specification
            {
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sSpecification");
                dbf.AddTable("Specification");
                dbf.AddParameter("K_Specification", pK_Specification);
                db.DataLayer.SelectData(dbf);
                if (dbf.hasTables && dbf.TableZeroRows.Count == 1) GetColumns(dbf.RowZero);

                if (!pBriefMode) PopulateDropDownLists(db);
            }
        }



        // Methods

        public bool GetColumns(DataRow dataRow)
        {
            if (dataRow == null) return false;

            if (DCC_GetColumn.Exists(dataRow, "K_Specification"))                        K_Specification                        =            DCC_GetColumn.Integer32 (dataRow, "K_Specification");
            if (DCC_GetColumn.Exists(dataRow, "ContractReference"))                      ContractReference                      =            DCC_GetColumn.Integer32 (dataRow, "ContractReference");
            if (DCC_GetColumn.Exists(dataRow, "IsBlank"))                                IsBlank                                =            DCC_GetColumn.Bit       (dataRow, "IsBlank");
            if (DCC_GetColumn.Exists(dataRow, "IsPackage"))                              IsPackage                              =            DCC_GetColumn.Bit       (dataRow, "IsPackage");
            if (DCC_GetColumn.Exists(dataRow, "Child01_Q_Specification"))                Child01_Q_Specification                =            DCC_GetColumn.Integer32 (dataRow, "Child01_Q_Specification");
            if (DCC_GetColumn.Exists(dataRow, "Child02_Q_Specification"))                Child02_Q_Specification                =            DCC_GetColumn.Integer32 (dataRow, "Child02_Q_Specification");
            if (DCC_GetColumn.Exists(dataRow, "Child03_Q_Specification"))                Child03_Q_Specification                =            DCC_GetColumn.Integer32 (dataRow, "Child03_Q_Specification");
            if (DCC_GetColumn.Exists(dataRow, "Child04_Q_Specification"))                Child04_Q_Specification                =            DCC_GetColumn.Integer32 (dataRow, "Child04_Q_Specification");
            if (DCC_GetColumn.Exists(dataRow, "Child05_Q_Specification"))                Child05_Q_Specification                =            DCC_GetColumn.Integer32 (dataRow, "Child05_Q_Specification");
            if (DCC_GetColumn.Exists(dataRow, "Child06_Q_Specification"))                Child06_Q_Specification                =            DCC_GetColumn.Integer32 (dataRow, "Child06_Q_Specification");
            if (DCC_GetColumn.Exists(dataRow, "Child07_Q_Specification"))                Child07_Q_Specification                =            DCC_GetColumn.Integer32 (dataRow, "Child07_Q_Specification");
            if (DCC_GetColumn.Exists(dataRow, "Child08_Q_Specification"))                Child08_Q_Specification                =            DCC_GetColumn.Integer32 (dataRow, "Child08_Q_Specification");
            if (DCC_GetColumn.Exists(dataRow, "Child09_Q_Specification"))                Child09_Q_Specification                =            DCC_GetColumn.Integer32 (dataRow, "Child09_Q_Specification");
            if (DCC_GetColumn.Exists(dataRow, "Child10_Q_Specification"))                Child10_Q_Specification                =            DCC_GetColumn.Integer32 (dataRow, "Child10_Q_Specification");

            if (DCC_GetColumn.Exists(dataRow, "Q_ContractType"))                         Q_ContractType                         =            DCC_GetColumn.Integer32 (dataRow, "Q_ContractType");
            if (DCC_GetColumn.Exists(dataRow, "DateStart"))                              DateStart                              =            DCC_GetColumn.DateTimeQ (dataRow, "DateStart");
            if (DCC_GetColumn.Exists(dataRow, "DateEnd"))                                DateEnd                                =            DCC_GetColumn.DateTimeQ (dataRow, "DateEnd");
            if (DCC_GetColumn.Exists(dataRow, "SpecificationServiceNo"))                 SpecificationServiceNo                 =            DCC_GetColumn.String    (dataRow, "SpecificationServiceNo");
            if (DCC_GetColumn.Exists(dataRow, "RouteStart"))                             RouteStart                             =            DCC_GetColumn.String    (dataRow, "RouteStart");
            if (DCC_GetColumn.Exists(dataRow, "RouteEnd"))                               RouteEnd                               =            DCC_GetColumn.String    (dataRow, "RouteEnd");
            if (DCC_GetColumn.Exists(dataRow, "ExceptionalConditions"))                  ExceptionalConditions                  =            DCC_GetColumn.String    (dataRow, "ExceptionalConditions");

            if (DCC_GetColumn.Exists(dataRow, "VehicleCapacity"))                        VehicleCapacity                        =            DCC_GetColumn.String    (dataRow, "VehicleCapacity");
            if (DCC_GetColumn.Exists(dataRow, "VehicleElderlyAndDisabled"))              VehicleElderlyAndDisabled              =            DCC_GetColumn.String    (dataRow, "VehicleElderlyAndDisabled");
            if (DCC_GetColumn.Exists(dataRow, "VehicleDimensions"))                      VehicleDimensions                      =            DCC_GetColumn.String    (dataRow, "VehicleDimensions");
                                                                                                                                             
            if (DCC_GetColumn.Exists(dataRow, "Q_Agreement"))                            Q_Agreement                            =            DCC_GetColumn.Integer32 (dataRow, "Q_Agreement");
            if (DCC_GetColumn.Exists(dataRow, "IsFuelTerm"))                             IsFuelTerm                             =            DCC_GetColumn.Bit       (dataRow, "IsFuelTerm");
            if (DCC_GetColumn.Exists(dataRow, "TenderClosingDate"))                      TenderClosingDate                      =            DCC_GetColumn.DateTimeQ (dataRow, "TenderClosingDate");
            if (DCC_GetColumn.Exists(dataRow, "JointResponsibility1"))                   JointResponsibility1                   =            DCC_GetColumn.String    (dataRow, "JointResponsibility1");
            if (DCC_GetColumn.Exists(dataRow, "ServiceDetails1"))                        ServiceDetails1                        =            DCC_GetColumn.String    (dataRow, "ServiceDetails1");
                                                                                                                                             
            if (DCC_GetColumn.Exists(dataRow, "OperatesOnMon"))                          OperatesOnMon                          =            DCC_GetColumn.Bit       (dataRow, "OperatesOnMon");
            if (DCC_GetColumn.Exists(dataRow, "OperatesOnTue"))                          OperatesOnTue                          =            DCC_GetColumn.Bit       (dataRow, "OperatesOnTue");
            if (DCC_GetColumn.Exists(dataRow, "OperatesOnWed"))                          OperatesOnWed                          =            DCC_GetColumn.Bit       (dataRow, "OperatesOnWed");
            if (DCC_GetColumn.Exists(dataRow, "OperatesOnThu"))                          OperatesOnThu                          =            DCC_GetColumn.Bit       (dataRow, "OperatesOnThu");
            if (DCC_GetColumn.Exists(dataRow, "OperatesOnFri"))                          OperatesOnFri                          =            DCC_GetColumn.Bit       (dataRow, "OperatesOnFri");
            if (DCC_GetColumn.Exists(dataRow, "OperatesOnSat"))                          OperatesOnSat                          =            DCC_GetColumn.Bit       (dataRow, "OperatesOnSat");
            if (DCC_GetColumn.Exists(dataRow, "OperatesOnSun"))                          OperatesOnSun                          =            DCC_GetColumn.Bit       (dataRow, "OperatesOnSun");
            if (DCC_GetColumn.Exists(dataRow, "OperatesOnSchoolDays"))                   OperatesOnSchoolDays                   =            DCC_GetColumn.Bit       (dataRow, "OperatesOnSchoolDays");
            if (DCC_GetColumn.Exists(dataRow, "OperatesOnNonSchoolDays"))                OperatesOnNonSchoolDays                =            DCC_GetColumn.Bit       (dataRow, "OperatesOnNonSchoolDays");
            if (DCC_GetColumn.Exists(dataRow, "OperatesOnBankHolidays"))                 OperatesOnBankHolidays                 =            DCC_GetColumn.Bit       (dataRow, "OperatesOnBankHolidays");
            if (DCC_GetColumn.Exists(dataRow, "OperatesOnChristmasAndNewYear"))          OperatesOnChristmasAndNewYear          =            DCC_GetColumn.Bit       (dataRow, "OperatesOnChristmasAndNewYear");
            if (DCC_GetColumn.Exists(dataRow, "CalculatedNoOfDaysOfOperationFirstYear")) CalculatedNoOfDaysOfOperationFirstYear =            DCC_GetColumn.Integer32 (dataRow, "CalculatedNoOfDaysOfOperationFirstYear");
            if (DCC_GetColumn.Exists(dataRow, "CalculatedNoOfDaysOfOperationFullTerm"))  CalculatedNoOfDaysOfOperationFullTerm  =            DCC_GetColumn.Integer32 (dataRow, "CalculatedNoOfDaysOfOperationFullTerm");
                                                                                                                                             
            if (DCC_GetColumn.Exists(dataRow, "AverageMilesPerDay"))                     AverageMilesPerDay                     =            DCC_GetColumn.Decimal   (dataRow, "AverageMilesPerDay");
            if (DCC_GetColumn.Exists(dataRow, "AverageHoursPerDay"))                     AverageHoursPerDay                     =            DCC_GetColumn.Decimal   (dataRow, "AverageHoursPerDay");
            if (DCC_GetColumn.Exists(dataRow, "NoOfVehicles"))                           NoOfVehicles                           =            DCC_GetColumn.Integer32 (dataRow, "NoOfVehicles");
            if (DCC_GetColumn.Exists(dataRow, "AverageWeeklyRevenueEstimate"))           AverageWeeklyRevenueEstimate           = Math.Round(DCC_GetColumn.Decimal   (dataRow, "AverageWeeklyRevenueEstimate"), 2);

            if (DCC_GetColumn.Exists(dataRow, "Notes"))                                  Notes                                  =            DCC_GetColumn.String    (dataRow, "Notes");

            if (DCC_GetColumn.Exists(dataRow, "AgreementDescription"))                   AgreementDescription                   =            DCC_GetColumn.String    (dataRow, "AgreementDescription");
            if (DCC_GetColumn.Exists(dataRow, "ContractTypeDescription"))                ContractTypeDescription                =            DCC_GetColumn.String    (dataRow, "ContractTypeDescription");
            if (DCC_GetColumn.Exists(dataRow, "ContractorName"))                         ContractorName                         =            DCC_GetColumn.String    (dataRow, "ContractorName");
            if (DCC_GetColumn.Exists(dataRow, "ContractorLicenceNo"))                    ContractorLicenceNo                    =            DCC_GetColumn.String    (dataRow, "ContractorLicenceNo");

            return true;
        }

        public void PopulateDropDownLists(DCC_Database db)
        {
            SpecificationList specificationList = new SpecificationList(db);
            SpecificationItems = new List<ddlItem>();
            foreach (Specification specification in specificationList.ActiveExcludingPackages) // use ExcludingPackages here as you cannot select another package as a child
            {
                ddlItem ddlSpecification = new ddlItem();
                ddlSpecification.Id = specification.K_Specification;
                ddlSpecification.Name = specification.ContractReference.ToString() + " - " + specification.SpecificationServiceNoText;
                SpecificationItems.Add(ddlSpecification);
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
            #region If it's a package, set the earliest start date and latest end date of the child specifications
            if (IsPackage)
            {
                if (Child01_Q_Specification.HasValue)
                {
                    Specification specification = new Specification(db, Child01_Q_Specification.Value, pBriefMode:true);
                    DateStart = specification.DateStart;
                    DateEnd = specification.DateEnd;
                }
                if (Child02_Q_Specification.HasValue)
                {
                    Specification specification = new Specification(db, Child02_Q_Specification.Value, pBriefMode:true);
                    if (specification.DateStart.HasValue && specification.DateStart < DateStart.Value) DateStart = specification.DateStart;
                    if (specification.DateEnd  .HasValue && specification.DateEnd   > DateEnd  .Value) DateEnd   = specification.DateEnd;
                }
                if (Child03_Q_Specification.HasValue)
                {
                    Specification specification = new Specification(db, Child03_Q_Specification.Value, pBriefMode:true);
                    if (specification.DateStart.HasValue && specification.DateStart < DateStart.Value) DateStart = specification.DateStart;
                    if (specification.DateEnd  .HasValue && specification.DateEnd   > DateEnd  .Value) DateEnd   = specification.DateEnd;
                }
                if (Child04_Q_Specification.HasValue)
                {
                    Specification specification = new Specification(db, Child04_Q_Specification.Value, pBriefMode: true);
                    if (specification.DateStart.HasValue && specification.DateStart < DateStart.Value) DateStart = specification.DateStart;
                    if (specification.DateEnd  .HasValue && specification.DateEnd   > DateEnd  .Value) DateEnd   = specification.DateEnd;
                }
                if (Child05_Q_Specification.HasValue)
                {
                    Specification specification = new Specification(db, Child05_Q_Specification.Value, pBriefMode: true);
                    if (specification.DateStart.HasValue && specification.DateStart < DateStart.Value) DateStart = specification.DateStart;
                    if (specification.DateEnd  .HasValue && specification.DateEnd   > DateEnd  .Value) DateEnd   = specification.DateEnd;
                }
                if (Child06_Q_Specification.HasValue)
                {
                    Specification specification = new Specification(db, Child06_Q_Specification.Value, pBriefMode:true);
                    if (specification.DateStart.HasValue && specification.DateStart < DateStart.Value) DateStart = specification.DateStart;
                    if (specification.DateEnd  .HasValue && specification.DateEnd   > DateEnd  .Value) DateEnd   = specification.DateEnd;
                }
                if (Child07_Q_Specification.HasValue)
                {
                    Specification specification = new Specification(db, Child07_Q_Specification.Value, pBriefMode: true);
                    if (specification.DateStart.HasValue && specification.DateStart < DateStart.Value) DateStart = specification.DateStart;
                    if (specification.DateEnd  .HasValue && specification.DateEnd   > DateEnd  .Value) DateEnd   = specification.DateEnd;
                }
                if (Child08_Q_Specification.HasValue)
                {
                    Specification specification = new Specification(db, Child08_Q_Specification.Value, pBriefMode: true);
                    if (specification.DateStart.HasValue && specification.DateStart < DateStart.Value) DateStart = specification.DateStart;
                    if (specification.DateEnd  .HasValue && specification.DateEnd   > DateEnd  .Value) DateEnd   = specification.DateEnd;
                }
                if (Child09_Q_Specification.HasValue)
                {
                    Specification specification = new Specification(db, Child09_Q_Specification.Value, pBriefMode:true);
                    if (specification.DateStart.HasValue && specification.DateStart < DateStart.Value) DateStart = specification.DateStart;
                    if (specification.DateEnd  .HasValue && specification.DateEnd   > DateEnd  .Value) DateEnd   = specification.DateEnd;
                }
                if (Child10_Q_Specification.HasValue)
                {
                    Specification specification = new Specification(db, Child10_Q_Specification.Value, pBriefMode: true);
                    if (specification.DateStart.HasValue && specification.DateStart < DateStart.Value) DateStart = specification.DateStart;
                    if (specification.DateEnd  .HasValue && specification.DateEnd   > DateEnd  .Value) DateEnd   = specification.DateEnd;
                }
            }
            #endregion

            CalculateDaysOfOperation(db);

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
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uSpecification2");
            dbf.AddTable("Specification");
            dbf.AddParameter("K_Specification",                        K_Specification);
            dbf.AddParameter("IsBlank",                                IsBlank);
            dbf.AddParameter("IsPackage",                              IsPackage);
            dbf.AddParameter("Child01_Q_Specification",                Child01_Q_Specification);
            dbf.AddParameter("Child02_Q_Specification",                Child02_Q_Specification);
            dbf.AddParameter("Child03_Q_Specification",                Child03_Q_Specification);
            dbf.AddParameter("Child04_Q_Specification",                Child04_Q_Specification);
            dbf.AddParameter("Child05_Q_Specification",                Child05_Q_Specification);
            dbf.AddParameter("Child06_Q_Specification",                Child06_Q_Specification);
            dbf.AddParameter("Child07_Q_Specification",                Child07_Q_Specification);
            dbf.AddParameter("Child08_Q_Specification",                Child08_Q_Specification);
            dbf.AddParameter("Child09_Q_Specification",                Child09_Q_Specification);
            dbf.AddParameter("Child10_Q_Specification",                Child10_Q_Specification);

            dbf.AddParameter("Q_ContractType",                         Q_ContractType);
            dbf.AddParameter("DateStart",                              DateStart);
            dbf.AddParameter("DateEnd",                                DateEnd);
            dbf.AddParameter("SpecificationServiceNo",                 SpecificationServiceNo   , 500);
            dbf.AddParameter("RouteStart",                             RouteStart               , 500);
            dbf.AddParameter("RouteEnd",                               RouteEnd                 , 500);
            dbf.AddParameter("ExceptionalConditions",                  ExceptionalConditions    , 500);

            dbf.AddParameter("VehicleCapacity",                        VehicleCapacity          , 500);
            dbf.AddParameter("VehicleElderlyAndDisabled",              VehicleElderlyAndDisabled, 500);
            dbf.AddParameter("VehicleDimensions",                      VehicleDimensions        , 500);

            dbf.AddParameter("Q_Agreement",                            Q_Agreement);
            dbf.AddParameter("IsFuelTerm",                             IsFuelTerm);
            dbf.AddParameter("TenderClosingDate",                      TenderClosingDate);
            dbf.AddParameter("JointResponsibility1",                   JointResponsibility1     , 500);
            dbf.AddParameter("ServiceDetails1",                        ServiceDetails1          , 500);

            dbf.AddParameter("OperatesOnMon",                          OperatesOnMon);
            dbf.AddParameter("OperatesOnTue",                          OperatesOnTue);
            dbf.AddParameter("OperatesOnWed",                          OperatesOnWed);
            dbf.AddParameter("OperatesOnThu",                          OperatesOnThu);
            dbf.AddParameter("OperatesOnFri",                          OperatesOnFri);
            dbf.AddParameter("OperatesOnSat",                          OperatesOnSat);
            dbf.AddParameter("OperatesOnSun",                          OperatesOnSun);
            dbf.AddParameter("OperatesOnSchoolDays",                   OperatesOnSchoolDays);
            dbf.AddParameter("OperatesOnNonSchoolDays",                OperatesOnNonSchoolDays);
            dbf.AddParameter("OperatesOnBankHolidays",                 OperatesOnBankHolidays);
            dbf.AddParameter("OperatesOnChristmasAndNewYear",          OperatesOnChristmasAndNewYear);
            dbf.AddParameter("CalculatedNoOfDaysOfOperationFirstYear", CalculatedNoOfDaysOfOperationFirstYear);
            dbf.AddParameter("CalculatedNoOfDaysOfOperationFullTerm",  CalculatedNoOfDaysOfOperationFullTerm);

            dbf.AddParameter("AverageMilesPerDay",                     AverageMilesPerDay);
            dbf.AddParameter("AverageHoursPerDay",                     AverageHoursPerDay);
            dbf.AddParameter("NoOfVehicles",                           NoOfVehicles);
            dbf.AddParameter("AverageWeeklyRevenueEstimate",           AverageWeeklyRevenueEstimate);

            dbf.AddParameter("Notes",                                  Notes                    , 500);

            db.DataLayer.NonQuery(dbf);
            return (dbf.OK ? K_Specification : -1);
        }

        #region CalculateDaysOfOperation - creates a string based on the days of operation then calls the next method to calculate the number of days
        
        public void CalculateDaysOfOperation(DCC_Database db)
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

            var fyStart = DateStart.Value.Date;
            var fyEnd = (DateEnd.Value > DateStart.Value.AddDays(364) ? DateStart.Value.AddDays(364) : DateEnd.Value).Date;
            DebugTrace.WriteLine($"[FY] Range: {fyStart:dd/MM/yyyy}..{fyEnd:dd/MM/yyyy}  Days: {(fyEnd - fyStart).TotalDays + 1}");

            CalculatedNoOfDaysOfOperationFirstYear = CalculateDaysOfOperation(db, dateAndDays);

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

            var ftStart = DateStart.Value.Date;
            var ftEnd = DateEnd.Value.Date;
            DebugTrace.WriteLine($"[FT] Range: {ftStart:dd/MM/yyyy}..{ftEnd:dd/MM/yyyy}  Days: {(ftEnd - ftStart).TotalDays + 1}");

            CalculatedNoOfDaysOfOperationFullTerm = CalculateDaysOfOperation(db, dateAndDays2);
        }

        #endregion

        #region CalculateDaysOfOperation - calculates the number of days based off the provided DateAndDays string and returns it in an int

        public static int CalculateDaysOfOperation(DCC_Database db, string DateAndDays)
        {
            int noOfDaysInOperation = 0;
            List<CalendarDay> Calendar = new List<CalendarDay>();
            IEnumerable<DateTime> EachDay(DateTime pStartDate, DateTime pEndDate) { for (var day = pStartDate.Date; day.Date <= pEndDate.Date; day = day.AddDays(1)) yield return day; }

            try
            {
                var startDate = DateTime.ParseExact(DateAndDays.Substring(0, 10), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var endDate = DateTime.ParseExact(DateAndDays.Substring(10, 10), "dd/MM/yyyy", CultureInfo.InvariantCulture);
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

                    // --- 31/12/2025 forensic probe ---
                    if (date.Year == 2025 && date.Month == 12 && date.Day == 31)
                    {
                        // Recreate each equality used by IsBankHoliday
                        bool bhNY = (date == holiday.NewYearsDay);
                        bool bhGF = (date == holiday.GoodFriday);
                        bool bhEM = (date == holiday.EasterMonday);
                        bool bhMay = (date == holiday.MayDay);
                        bool bhSpr = (date == holiday.SpringBankHoliday);
                        bool bhSum = (date == holiday.SummerBankHoliday);
                        bool bhXmas = (date == holiday.ChristmasDay);
                        bool bhBox = (date == holiday.BoxingDay);
                        bool bhS1 = (holiday.Special1 != null) && (date == holiday.Special1);
                        bool bhS2 = (holiday.Special2 != null) && (date == holiday.Special2);
                        bool bhS3 = (holiday.Special3 != null) && (date == holiday.Special3);
                        bool bhS4 = (holiday.Special4 != null) && (date == holiday.Special4);
                        bool bhS5 = (holiday.Special5 != null) && (date == holiday.Special5);
                        bool bhS6 = (holiday.Special6 != null) && (date == holiday.Special6);
                        bool bhS7 = (holiday.Special7 != null) && (date == holiday.Special7);
                        bool bhS8 = (holiday.Special8 != null) && (date == holiday.Special8);

                        var bhCalc = bhNY || bhGF || bhEM || bhMay || bhSpr || bhSum || bhXmas || bhBox
                                     || bhS1 || bhS2 || bhS3 || bhS4 || bhS5 || bhS6 || bhS7 || bhS8;

                        // Xmas window comparison: data-driven vs hard-coded
                        var windowStartData = (holiday.BoxingDay ?? new DateTime(2025, 12, 26));
                        var windowEndData = (holiday.NewYearsDay?.AddYears(1)) ?? new DateTime(2026, 1, 1);
                        bool xmasWinData = (date >= windowStartData) && (date < windowEndData);

                        var windowStartFixed = new DateTime(2025, 12, 26);
                        var windowEndFixed = new DateTime(2026, 1, 1);
                        bool xmasWinFixed = (date >= windowStartFixed) && (date < windowEndFixed);

                        DebugTrace.WriteLine($"[Probe 31/12/2025] BH? calc:{bhCalc}");
                        DebugTrace.WriteLine($"NY:{bhNY} GF:{bhGF} EM:{bhEM} May:{bhMay} Spr:{bhSpr} Sum:{bhSum}");
                        DebugTrace.WriteLine($"Xmas:{bhXmas} Box:{bhBox} S1:{bhS1} S2:{bhS2} S3:{bhS3} S4:{bhS4} S5:{bhS5} S6:{bhS6} S7:{bhS7} S8:{bhS8}");
                        DebugTrace.WriteLine($"XmasWin data:{xmasWinData} [{windowStartData:dd/MM}..{windowEndData:dd/MM})  fixed:{xmasWinFixed} [{windowStartFixed:dd/MM}..{windowEndFixed:dd/MM})");
                    }

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

                    // 1) First set the Xmas–New Year window (null-safe)
                    var windowStart = holiday.BoxingDay ?? new DateTime(date.Year, 12, 26);
                    var windowEnd = (holiday.NewYearsDay?.AddYears(1)) ?? new DateTime(date.Year + 1, 1, 1);
                    calendarDay.IsBetweenChristmasAndNewYear = (date >= windowStart) && (date < windowEnd);

                    // 2) Now derive weekday flags
                    bool isWeekday = date.DayOfWeek >= DayOfWeek.Monday && date.DayOfWeek <= DayOfWeek.Friday;
                    bool inXmasMonFri = calendarDay.IsBetweenChristmasAndNewYear && isWeekday;

                    if (date.Year == 2025 && date.Month == 1 && date.Day == 1)
                    {
                        DebugTrace.WriteLine(
                            $"[HOL 2025] NYD:{holiday.NewYearsDay:dd/MM/yyyy}  " +
                            $"GF:{holiday.GoodFriday:dd/MM/yyyy}  EM:{holiday.EasterMonday:dd/MM/yyyy}  " +
                            $"May:{holiday.MayDay:dd/MM/yyyy}  Spring:{holiday.SpringBankHoliday:dd/MM/yyyy}  " +
                            $"Summer:{holiday.SummerBankHoliday:dd/MM/yyyy}  " +
                            $"Xmas:{holiday.ChristmasDay:dd/MM/yyyy}  Boxing:{holiday.BoxingDay:dd/MM/yyyy}  " +
                            $"Xmas:{holiday.NewYearsDay:dd/MM/yyyy} " +
                            $"S1:{holiday.Special1:dd/MM/yyyy}  S2:{holiday.Special2:dd/MM/yyyy}  " +
                            $"S3:{holiday.Special3:dd/MM/yyyy}  S4:{holiday.Special4:dd/MM/yyyy}  " +
                            $"S5:{holiday.Special5:dd/MM/yyyy}  S6:{holiday.Special6:dd/MM/yyyy}  " +
                            $"S7:{holiday.Special7:dd/MM/yyyy}  S8:{holiday.Special8:dd/MM/yyyy}"
                        );
                    }

                    // 3) Then school-day calc can safely use isWeekday
                    calendarDay.IsSchoolDay =
                        isWeekday && (
                            (date >= holiday.Spring1Start && date <= holiday.Spring1End) ||
                            (date >= holiday.Spring2Start && date <= holiday.Spring2End) ||
                            (date >= holiday.Summer1Start && date <= holiday.Summer1End) ||
                            (date >= holiday.Summer2Start && date <= holiday.Summer2End) ||
                            (date >= holiday.Autumn1Start && date <= holiday.Autumn1End) ||
                            (date >= holiday.Autumn2Start && date <= holiday.Autumn2End)
                        );

                    // //
                    // ----- Prospect rule: at least one of School/Non-school must be selected -----
                    if (!operatesOnSchoolDays && !operatesOnNonSchoolDays)
                    {
                        calendarDay.IsInOperation = false;
                        Calendar.Add(calendarDay);
                        continue; // skip this date entirely
                    }

                    // Selected DoW
                    bool dowSelected =
                        (calendarDay.IsMon && operatesOnMon) ||
                        (calendarDay.IsTue && operatesOnTue) ||
                        (calendarDay.IsWed && operatesOnWed) ||
                        (calendarDay.IsThu && operatesOnThu) ||
                        (calendarDay.IsFri && operatesOnFri) ||
                        (calendarDay.IsSat && operatesOnSat) ||
                        (calendarDay.IsSun && operatesOnSun);

                    // “never operate” trio
                    bool isNeverService = (date.Month == 1 && date.Day == 1) ||
                                         (date.Month == 12 && date.Day == 25) ||
                                         (date.Month == 12 && date.Day == 26);

                    bool include = false;

                    if (isNeverService) { include = false; }

                    // 0) Hard stop: never run on 1 Jan, 25 Dec, 26 Dec
                    if (isNeverService)
                    {
                        include = false;
                    }
                    else
                    {
                        // 1) Xmas window special handling (Mon–Fri only, never count BHs here)
                        if (inXmasMonFri)
                        {
                            if (calendarDay.IsBankHoliday)
                            {
                                include = operatesOnBankHolidays;
                            }
                            else
                            {
                                // Non-BH weekdays in that window depend on the Xmas checkbox
                                if (!operatesOnChristmasAndNewYear)
                                {
                                    include = false;
                                }
                                else
                                {
                                    // In the Xmas window, Mon-Fri should operate if the Xmas flag is on, regardless of normal weekday selections.
                                    include = true;

                                    // Apply School/Non-school filter on Mon–Fri only
                                    if (isWeekday)
                                    {
                                        if (operatesOnSchoolDays && !operatesOnNonSchoolDays)
                                            include = calendarDay.IsSchoolDay;
                                        else if (!operatesOnSchoolDays && operatesOnNonSchoolDays)
                                            include = !calendarDay.IsSchoolDay;
                                        // both selected => no extra filter
                                    }
                                }
                            }
                        }
                        else if (calendarDay.IsBankHoliday)
                        {
                                include = operatesOnBankHolidays;
                        }
                        else
                        {
                            // 3) Regular non-BH day, outside Xmas suppression
                            if (dowSelected)
                            {
                                include = true;

                                // Apply School/Non-school filter on Mon–Fri only
                                if (isWeekday)
                                {
                                    if (operatesOnSchoolDays && !operatesOnNonSchoolDays)
                                        include = calendarDay.IsSchoolDay;
                                    else if (!operatesOnSchoolDays && operatesOnNonSchoolDays)
                                        include = !calendarDay.IsSchoolDay;
                                    // both selected => no extra filter
                                }
                            }
                            else
                            {
                                include = false;
                            }
                        }
                    }

                    calendarDay.IsInOperation = include;
                    if (include) noOfDaysInOperation++;

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

        #endregion

        public int CalculateDaysOfOperationInPeriod(DCC_Database db, DateTime periodStart, DateTime periodEnd)
        {
            if (!DateStart.HasValue || !DateEnd.HasValue) return 0;

            DateTime actualStart = (periodStart < DateStart.Value) ? DateStart.Value : periodStart;
            DateTime actualEnd = (periodEnd > DateEnd.Value) ? DateEnd.Value : periodEnd;

            if (actualEnd < actualStart) return 0;
            
            string dateAndDays =
                actualStart.ToString("dd/MM/yyyy") +
                actualEnd.ToString("dd/MM/yyyy") +
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

            return CalculateDaysOfOperation(db, dateAndDays);
        }

    }

    #endregion


    #region SpecificationList

    public class SpecificationList : IEnumerable<Specification>
    {

        // Properties

        public List<object> All = new List<object>();
        public List<object> ActiveIncludingPackages = new List<object>();
        public List<object> ActiveExcludingPackages = new List<object>();

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
            if ((specification.DateStart.HasValue) && (specification.DateStart > DateTime.Now.AddYears(-1)))
            {
                ActiveIncludingPackages.Add(specification);
                if (!specification.IsPackage) ActiveExcludingPackages.Add(specification);
            }
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
        public bool     IsMon;
        public bool     IsTue;
        public bool     IsWed;
        public bool     IsThu;
        public bool     IsFri;
        public bool     IsSat;
        public bool     IsSun;
        public bool     IsBankHoliday;
        public bool     IsSchoolDay;
        public bool     IsBetweenChristmasAndNewYear;
        public bool     IsInOperation;
        public string   IsInOperationText { get { return DCC_Formatting.NullDateTxt(Date, "yyyy MMM dd ddd" + (IsInOperation ? " YES" : "")); } }



        // Utilities

        public override string ToString() { return IsInOperationText; }



        // Constructor

        public CalendarDay()
        {
        }

    }

    #endregion

}
