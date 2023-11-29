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

        public int       K_Tender                        { get; set; }
        public int       TenderReference                 { get; set; }
        public int       Q_Specification                 { get; set; }
        public int       Q_Contractor                    { get; set; }
        public int       NoOfDaysOfOperationPerYear      { get; set; }
        public decimal   TenderPricePerDay               { get; set; }
        public decimal   FuelPriceElement                { get; set; }
        public DateTime? TenderClosingDate               { get; set; }
        public decimal   EstimatedAverageRevenuePerDay   { get; set; }
        public decimal   AverageCostPerDay               { get; set; }
        public decimal   TenderPricePerYear              { get; set; }
        public decimal   EstimatedAverageRevenuePerYear  { get; set; }
        public decimal   AverageCostPerYear              { get; set; }
        public bool      AnyTenderQualifications         { get; set; }
        public string    TenderQualificationNotes        { get; set; }
        public string    Notes                           { get; set; }
        public bool      IsConforming                    { get; set; }
        public string    VehicleDetails                  { get; set; }
        public bool      IncludeInTenderAnalysis         { get; set; }
        public bool      IsTenderAccepted                { get; set; }
        public bool      IsContractCancelled             { get; set; }
        public int       ContractReference               { get; set; }
        public bool      IsPackage                       { get; set; }

        public string    SpecificationServiceNo          { get; set; }
        public string    RouteStart                      { get; set; }
        public string    RouteEnd                        { get; set; }
        public DateTime? DateStart                       { get; set; }
        public DateTime? DateEnd                         { get; set; }
        public string    ContractTypeDescription         { get; set; }
        public string    ContractorName                  { get; set; }
        public int       VariationNo                     { get; set; }
        public int       CalculatedNoOfDaysInOperation   { get; set; }
        public string    ContractorLicenceNo             { get; set; }
        public string    Agreement                       { get; set; }
        public decimal   ContractedMiles                 { get; set; }
        public string    ExceptionalConditions           { get; set; }
        public string    VehicleCapacity                 { get; set; }
        public string    VehicleElderlyAndDisabled       { get; set; }
        public string    VehicleDimensions               { get; set; }
        public int       Q_Agreement                     { get; set; }
        public string    ServiceDetails1                 { get; set; }
        public string    ServiceDetails2                 { get; set; }
        public string    ServiceDetails3                 { get; set; }
        public bool      OperatesOnMon                   { get; set; }
        public bool      OperatesOnTue                   { get; set; }
        public bool      OperatesOnWed                   { get; set; }
        public bool      OperatesOnThu                   { get; set; }
        public bool      OperatesOnFri                   { get; set; }
        public bool      OperatesOnSat                   { get; set; }
        public bool      OperatesOnSun                   { get; set; }
        public bool      OperatesOnSchoolDays            { get; set; }
        public bool      OperatesOnNonSchoolDays         { get; set; }
        public bool      OperatesOnBankHolidays          { get; set; }
        public bool      OperatesOnChristmasAndNewYear   { get; set; }
        public string    RouteNotes                      { get; set; }
        public string    TicketNotes                     { get; set; }
        public string    VehicleNotes                    { get; set; }
        public bool      Cancelled                       { get; set; }
        public bool      Extended                        { get; set; }
        public string    AgreementDescription            { get; set; }
        public DateTime? CvDateVariationsStartA          { get; set; }
        public DateTime? CvDateVariationsEndA            { get; set; }
        public decimal   CvAverageRevenuePerDay          { get; set; }
        public decimal   CvAverageCostPerDay             { get; set; }
        public decimal   CvAveragePricePerDay            { get; set; }
        public decimal   CvAverageRevenuePerYear         { get; set; }
        public decimal   CvAverageCostPerYear            { get; set; }
        public decimal   CvAveragePricePerYear           { get; set; }
        public decimal   CvContractedMiles               { get; set; }
        public decimal   CvRatePerMile                   { get; set; }
        public decimal   CvXBNDPCENT                     { get; set; }
        public int       CvVariationNumber               { get; set; }
        public int       CvNumberOfDaysOperating         { get; set; }
        public string    CvCancelled                     { get; set; }
        public string    CvPriceChanged                  { get; set; }
        public string    CvServiceInformation            { get; set; }
        public string    ContractReferenceAndVariationNo { get; set; }
                                                         
        public int       Saved_K_Specification           { get; set; }

        public string    ContractReferenceSort           { get { return ContractReference.ToString().PadLeft(5, '0'); } }
        public string    TenderReferenceSort             { get { return TenderReference.ToString().PadLeft(5, '0'); } }
        public string    DateStartText                   { get { return DCC_Formatting.NullDateTxt(DateStart, "dd MMM yyyy"); } }
        public string    DateStartSort                   { get { return DCC_Formatting.NullDateTxt(DateStart, "yyyyMMddHHmm"); } }
        public string    DateEndText                     { get { return DCC_Formatting.NullDateTxt(DateEnd, "dd MMM yyyy"); } }
        public string    DateEndSort                     { get { return DCC_Formatting.NullDateTxt(DateEnd, "yyyyMMddHHmm"); } }
        public string    RouteText                       { get { return (string.IsNullOrEmpty(RouteStart) ? "- not set -" : RouteStart + " to " + RouteEnd); } }
        public string    DatesText                       { get { return ( (DateStart == null) ? "- not set -" : DateStartText + " to " + DateEndText); } }
        public string    TitleText                       { get { return (IsTenderAccepted ? "Contract" : "Tender"); } }
        public string    VariationNoText                 { get { return VariationNo.ToString().PadLeft(2, '0'); } }
        public string    ServiceNoText                   { get { return (IsPackage ? "PACKAGE" : SpecificationServiceNo); } }
        public bool      FuelTerm                        { get; set; }
        public string    TenderClosingDateText           { get { return DCC_Formatting.NullDateTxt(TenderClosingDate, "dd MMM yyyy"); } }
        public DateTime?    ContractVariationClosingDate    { get; set; }




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

            K_Tender                       = DCC_GetColumn.Integer32          (dataRow, "K_Tender");
            TenderReference                = DCC_GetColumn.Integer32          (dataRow, "TenderReference");
            Q_Specification                = DCC_GetColumn.Integer32          (dataRow, "Q_Specification");
            Q_Contractor                   = DCC_GetColumn.Integer32          (dataRow, "Q_Contractor");
            NoOfDaysOfOperationPerYear     = DCC_GetColumn.Integer32          (dataRow, "NoOfDaysOfOperationPerYear");
            TenderPricePerDay              = Math.Round(DCC_GetColumn.Decimal (dataRow, "TenderPricePerDay"), 2);
            FuelPriceElement               = Math.Round(DCC_GetColumn.Decimal (dataRow, "FuelPriceElement"), 2);
            TenderClosingDate              = DCC_GetColumn.DateTimeQ          (dataRow, "TenderClosingDate");
            EstimatedAverageRevenuePerDay  = Math.Round(DCC_GetColumn.Decimal (dataRow, "EstimatedAverageRevenuePerDay"), 2);
            AverageCostPerDay              = Math.Round(DCC_GetColumn.Decimal (dataRow, "AverageCostPerDay"), 2);
            TenderPricePerYear             = Math.Round(DCC_GetColumn.Decimal (dataRow, "TenderPricePerYear"), 2);
            EstimatedAverageRevenuePerYear = Math.Round(DCC_GetColumn.Decimal (dataRow, "EstimatedAverageRevenuePerYear"), 2);
            AverageCostPerYear             = Math.Round(DCC_GetColumn.Decimal (dataRow, "AverageCostPerYear"), 2);
            AnyTenderQualifications        = DCC_GetColumn.Bit                (dataRow, "AnyTenderQualifications");
            TenderQualificationNotes       = DCC_GetColumn.String             (dataRow, "TenderQualificationNotes");
            Notes                          = DCC_GetColumn.String             (dataRow, "Notes");
            IsConforming                   = DCC_GetColumn.Bit                (dataRow, "IsConforming");
            VehicleDetails                 = DCC_GetColumn.String             (dataRow, "VehicleDetails");
            IncludeInTenderAnalysis        = DCC_GetColumn.Bit                (dataRow, "IncludeInTenderAnalysis");
            IsTenderAccepted               = DCC_GetColumn.Bit                (dataRow, "IsTenderAccepted");
            IsContractCancelled            = DCC_GetColumn.Bit                (dataRow, "IsContractCancelled");

            if (DCC_GetColumn.Exists(dataRow, "ContractReference"))             ContractReference             = DCC_GetColumn.Integer32(dataRow, "ContractReference");
            if (DCC_GetColumn.Exists(dataRow, "IsPackage"))                     IsPackage                     = DCC_GetColumn.Bit(dataRow,       "IsPackage");
            if (DCC_GetColumn.Exists(dataRow, "SpecificationServiceNo"))        SpecificationServiceNo        = DCC_GetColumn.String(dataRow,    "SpecificationServiceNo");
            if (DCC_GetColumn.Exists(dataRow, "RouteStart"))                    RouteStart                    = DCC_GetColumn.String(dataRow,    "RouteStart");
            if (DCC_GetColumn.Exists(dataRow, "RouteEnd"))                      RouteEnd                      = DCC_GetColumn.String(dataRow,    "RouteEnd");
            if (DCC_GetColumn.Exists(dataRow, "DateStart"))                     DateStart                     = DCC_GetColumn.DateTimeQ(dataRow, "DateStart");
            if (DCC_GetColumn.Exists(dataRow, "DateEnd"))                       DateEnd                       = DCC_GetColumn.DateTimeQ(dataRow, "DateEnd");
            if (DCC_GetColumn.Exists(dataRow, "ContractTypeDescription"))       ContractTypeDescription       = DCC_GetColumn.String(dataRow,    "ContractTypeDescription");
            if (DCC_GetColumn.Exists(dataRow, "ContractorName"))                ContractorName                = DCC_GetColumn.String(dataRow,    "ContractorName");
            if (DCC_GetColumn.Exists(dataRow, "VariationNo"))                   VariationNo                   = DCC_GetColumn.Integer32(dataRow, "VariationNo");
            if (DCC_GetColumn.Exists(dataRow, "ContractorLicenceNo"))           ContractorLicenceNo           = DCC_GetColumn.String(dataRow,    "ContractorLicenceNo");
            if (DCC_GetColumn.Exists(dataRow, "CalculatedNoOfDaysInOperation")) CalculatedNoOfDaysInOperation = DCC_GetColumn.Integer32(dataRow, "CalculatedNoOfDaysInOperation");
            if (DCC_GetColumn.Exists(dataRow, "FuelTerm"))                      FuelTerm                      = DCC_GetColumn.Bit(dataRow,       "FuelTerm");
            if (DCC_GetColumn.Exists(dataRow, "ContractVariationClosingDate"))  ContractVariationClosingDate  = DCC_GetColumn.DateTimeQ(dataRow, "ContractVariationClosingDate");

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

        //public int Save(DCC_Database db)
        //{
        //    return (IsNew ? Insert(db) : Update(db));
        //}

        public int Save(DCC_Database db)
        {
            int result = (IsNew ? Insert(db) : Update(db));

            // After insert or update, check if the tender is accepted
            if (result > -1 && IsTenderAccepted)
            {
                InsertFirstContractVariation(db, result);
            }

            return result;
        }

        int InsertFirstContractVariation(DCC_Database db, int K_Tender)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("IFirstContractVariation");
            dbf.AddTable("ContractVariation");
            //dbf.AddParameter("K_Tender", K_Tender);
            int newKey = db.DataLayer.NonQueryPK(dbf);
            if (newKey > -1) K_Tender = newKey;
            return newKey;
        }


        int Insert(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("iTender");
            dbf.AddTable("Tender");
            dbf.AddParameter("Q_Specification", Q_Specification);
            dbf.AddParameter("_PRIMARYKEY", DCC_DatabaseFrame.OUTINTEGER);
            int newKey = db.DataLayer.NonQueryPK(dbf);
            if (newKey > -1) K_Tender = newKey;
            return newKey;
        }

        int Update(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uTender");
            dbf.AddTable("Tender");
            dbf.AddParameter("K_Tender"                      , K_Tender);
            dbf.AddParameter("Q_Contractor"                  , Q_Contractor);
            dbf.AddParameter("NoOfDaysOfOperationPerYear"    , NoOfDaysOfOperationPerYear);
            dbf.AddParameter("TenderPricePerDay"             , TenderPricePerDay);
            dbf.AddParameter("FuelPriceElement"              , FuelPriceElement);
            dbf.AddParameter("TenderClosingDate"             , TenderClosingDate);
            dbf.AddParameter("EstimatedAverageRevenuePerDay" , EstimatedAverageRevenuePerDay);
            dbf.AddParameter("AverageCostPerDay"             , AverageCostPerDay);
            dbf.AddParameter("TenderPricePerYear"            , TenderPricePerYear);
            dbf.AddParameter("EstimatedAverageRevenuePerYear", EstimatedAverageRevenuePerYear);
            dbf.AddParameter("AverageCostPerYear"            , AverageCostPerYear);
            dbf.AddParameter("AnyTenderQualifications"       , AnyTenderQualifications);
            dbf.AddParameter("TenderQualificationNotes"      , TenderQualificationNotes, 500);
            dbf.AddParameter("Notes"                         , Notes, 500);
            dbf.AddParameter("IsConforming"                  , IsConforming);
            dbf.AddParameter("VehicleDetails"                , VehicleDetails, 500);
            dbf.AddParameter("IncludeInTenderAnalysis"       , IncludeInTenderAnalysis);
            dbf.AddParameter("IsTenderAccepted"              , IsTenderAccepted);
            dbf.AddParameter("IsContractCancelled"           , IsContractCancelled);
            dbf.AddParameter("ContractorLicenceNo"           , ContractorLicenceNo);

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
