using DCC_Standard;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace ProspectBusinessLayer
{

    #region ContractVariations

    public class ContractVariation
    {

        // Properties

        public int       K_ContractVariation                            { get; set; }
        public int       ContractVariation_Q_Tender                     { get; set; }
        public int       ContractVariationNo                            { get; set; }
        public string    ContractVariationNoText                        { get { return ContractVariationNo.ToString().PadLeft(2, '0'); } }
        public DateTime? ContractVariationDateEntered                   { get; set; }
        public string    ContractVariationDateEnteredText               { get { return DCC_Formatting.NullDateTxt(ContractVariationDateEntered, "dd MMM yyyy"); } }
        public string    ContractVariationDateEnteredSort               { get { return DCC_Formatting.NullDateTxt(ContractVariationDateEntered, "yyyyMMddHHmm"); } }
        public bool      ContractVariationIsExtended                    { get; set; }
        public bool      ContractVariationIsCancelled                   { get; set; }
        public string    ContractVariationNotes                         { get; set; }
        public int       ContractVariation_Q_Agreement                  { get; set; }
        public string    ContractVariationServiceNo                     { get; set; }
        public string    ContractVariationRouteStart                    { get; set; }
        public string    ContractVariationRouteEnd                      { get; set; }
        public string    ContractVariationRouteText                     { get { return (string.IsNullOrEmpty(ContractVariationRouteStart) ? "- not set -" : ContractVariationRouteStart + " to " + ContractVariationRouteEnd); } }
        public DateTime? ContractVariationDateStart                     { get; set; }
        public string    ContractVariationDateStartText                 { get { return DCC_Formatting.NullDateTxt(ContractVariationDateStart, "dd MMM yyyy"); } }
        public string    ContractVariationDateStartSort                 { get { return DCC_Formatting.NullDateTxt(ContractVariationDateStart, "yyyyMMddHHmm"); } }
        public string    ContractVariationDateStartFullText             { get { return DCC_Formatting.NullDateTxt(ContractVariationDateStart, "dd MMMM yyyy"); } }
        public DateTime? ContractVariationDateEnd                       { get; set; }
        public string    ContractVariationDateEndText                   { get { return DCC_Formatting.NullDateTxt(ContractVariationDateEnd, "dd MMM yyyy"); } }
        public string    ContractVariationDateEndSort                   { get { return DCC_Formatting.NullDateTxt(ContractVariationDateEnd, "yyyyMMddHHmm"); } }
        public string    ContractVariationDateEndFullText               { get { return DCC_Formatting.NullDateTxt(ContractVariationDateEnd, "dd MMMM yyyy"); } }
        public string    ContractVariationDatesText                     { get { return (string.IsNullOrEmpty(ContractVariationDateStartText) ? "- not set -" : ContractVariationDateStartText + " to " + ContractVariationDateEndText); } }
        public string    ContractVariationExceptionalConditions         { get; set; }
        public string    ContractVariationVehicleCapacity               { get; set; }
        public string    ContractVariationVehicleElderlyAndDisabled     { get; set; }
        public string    ContractVariationVehicleDimensions             { get; set; }
        public string    ContractVariationServiceDetails                { get; set; }
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
        public int       ContractVariationCalculatedNoOfDaysInOperation { get; set; }
        public decimal   ContractVariationAverageMilesPerDay            { get; set; }
        public decimal   ContractVariationRatePerMile                   { get; set; }
        public decimal   ContractVariationAverageRevenuePerDay          { get; set; }
        public decimal   ContractVariationAverageCostPerDay             { get; set; }
        public decimal   ContractVariationAveragePricePerDay            { get; set; }
        public decimal   ContractVariationAverageRevenuePerYear         { get; set; }
        public decimal   ContractVariationAverageCostPerYear            { get; set; }
        public decimal   ContractVariationAveragePricePerYear           { get; set; }
        public bool      ContractVariationIsTicketChanged               { get; set; }
        public string    ContractVariationTicketNotes                   { get; set; }
        public bool      ContractVariationIsRouteTimetableChanged       { get; set; }
        public string    ContractVariationRouteTimetableNotes           { get; set; }
        public DateTime? ContractVariationClosingDate                   { get; set; }
        public string    ContractVariationClosingDateText               { get { return DCC_Formatting.NullDateTxt(ContractVariationClosingDate, "dd MMM yyyy"); } }
        public decimal   ContractVariationFuelPriceElement              { get; set; }

        public string    AgreementDescription                           { get; set; }
        public int       ContractReference                              { get; set; }
        public string    ContractReferenceText                          { get { return ContractReference.ToString().PadLeft(5, '0'); } }
        public string    ContractTypeDescription                        { get; set; }
        public string    ContractorName                                 { get; set; }
        public string    ContractorLicenceNo                            { get; set; }
        public int       TenderReference                                { get; set; }
        public string    TenderReferenceText                            { get { return TenderReference.ToString().PadLeft(5, '0'); } }
        public string    ContractReferenceSort                          { get { return ContractReference.ToString().PadLeft(5, '0'); } }
        public string    TenderReferenceSort                            { get { return TenderReference.ToString().PadLeft(5, '0'); } }
        public decimal   EstimatedAverageRevenuePerDay                  { get; set; }
        public decimal   TenderPricePerDay                              { get; set; }

        public string    ExceptionalConditions                          { get; set; }
        public string    VehicleCapacity                                { get; set; }
        public string    VehicleElderlyAndDisabled                      { get; set; }
        public string    VehicleDimensions                              { get; set; }
        public decimal   AverageMilesPerDay                             { get; set; }

        public bool OperatesOnMon                                       { get; set; }
        public bool OperatesOnTue                                       { get; set; }
        public bool OperatesOnWed                                       { get; set; }
        public bool OperatesOnThu                                       { get; set; }
        public bool OperatesOnFri                                       { get; set; }
        public bool OperatesOnSat                                       { get; set; }
        public bool OperatesOnSun                                       { get; set; }
        public bool OperatesOnSchoolDays                                { get; set; }
        public bool OperatesOnNonSchoolDays                             { get; set; }
        public bool OperatesOnBankHolidays                              { get; set; }
        public bool OperatesOnChristmasAndNewYear                       { get; set; }
        


        // Utilities

        public int PrimaryKey { get { return K_ContractVariation; } set { K_ContractVariation = value; } }
        public string Key { get { return PrimaryKey.ToString(); } }
        public bool IsNew { get { return PrimaryKey == -1; } }
        public override string ToString() { return PrimaryKey.ToString().PadLeft(3) + " - " + ContractVariationNoText; }



        // Calendar

        public List<CalendarDay> Calendar = new List<CalendarDay>();
        public IEnumerable<DateTime> EachDay(DateTime pStartDate, DateTime pEndDate) { for (var day = pStartDate.Date; day.Date <= pEndDate.Date; day = day.AddDays(1)) yield return day; }



        // Constructors

        public ContractVariation()
        {
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

            K_ContractVariation                             = DCC_GetColumn.Integer32(dataRow,          "K_ContractVariation");
            ContractVariation_Q_Tender                      = DCC_GetColumn.Integer32(dataRow,          "ContractVariation_Q_Tender");
            ContractVariationNo                             = DCC_GetColumn.Integer32(dataRow,          "ContractVariationNo");
            ContractVariationDateEntered                    = DCC_GetColumn.DateTimeQ(dataRow,          "ContractVariationDateEntered");
            ContractVariationIsExtended                     = DCC_GetColumn.Bit(dataRow,                "ContractVariationIsExtended");
            ContractVariationIsCancelled                    = DCC_GetColumn.Bit(dataRow,                "ContractVariationIsCancelled");
            ContractVariationNotes                          = DCC_GetColumn.String(dataRow,             "ContractVariationNotes");
            ContractVariation_Q_Agreement                   = DCC_GetColumn.Integer32(dataRow,          "ContractVariation_Q_Agreement");
            ContractVariationServiceNo                      = DCC_GetColumn.String(dataRow,             "ContractVariationServiceNo");
            ContractVariationRouteStart                     = DCC_GetColumn.String(dataRow,             "ContractVariationRouteStart");
            ContractVariationRouteEnd                       = DCC_GetColumn.String(dataRow,             "ContractVariationRouteEnd");
            ContractVariationDateStart                      = DCC_GetColumn.DateTime(dataRow,           "ContractVariationDateStart");
            ContractVariationDateEnd                        = DCC_GetColumn.DateTime(dataRow,           "ContractVariationDateEnd");
            ContractVariationExceptionalConditions          = DCC_GetColumn.String(dataRow,             "ContractVariationExceptionalConditions");
            ContractVariationVehicleCapacity                = DCC_GetColumn.String(dataRow,             "ContractVariationVehicleCapacity");
            ContractVariationVehicleElderlyAndDisabled      = DCC_GetColumn.String(dataRow,             "ContractVariationVehicleElderlyAndDisabled");
            ContractVariationVehicleDimensions              = DCC_GetColumn.String(dataRow,             "ContractVariationVehicleDimensions");
            ContractVariationServiceDetails                 = DCC_GetColumn.String(dataRow,             "ContractVariationServiceDetails");
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
            ContractVariationAverageMilesPerDay             = Math.Round(DCC_GetColumn.Decimal(dataRow, "ContractVariationAverageMilesPerDay"), 2);
            ContractVariationRatePerMile                    = Math.Round(DCC_GetColumn.Decimal(dataRow, "ContractVariationRatePerMile"), 2);
            ContractVariationAverageRevenuePerDay           = Math.Round(DCC_GetColumn.Decimal(dataRow, "ContractVariationAverageRevenuePerDay"), 2);
            ContractVariationAverageRevenuePerYear          = Math.Round(DCC_GetColumn.Decimal(dataRow, "ContractVariationAverageRevenuePerYear"), 2);
            ContractVariationAverageCostPerDay              = Math.Round(DCC_GetColumn.Decimal(dataRow, "ContractVariationAverageCostPerDay"), 2);
            ContractVariationAverageCostPerYear             = Math.Round(DCC_GetColumn.Decimal(dataRow, "ContractVariationAverageCostPerYear"), 2);
            ContractVariationAveragePricePerDay             = Math.Round(DCC_GetColumn.Decimal(dataRow, "ContractVariationAveragePricePerDay"), 2);
            ContractVariationAveragePricePerYear            = Math.Round(DCC_GetColumn.Decimal(dataRow, "ContractVariationAveragePricePerYear"), 2);
            ContractVariationIsTicketChanged                = DCC_GetColumn.Bit(dataRow,                "ContractVariationIsTicketChanged");
            ContractVariationTicketNotes                    = DCC_GetColumn.String(dataRow,             "ContractVariationTicketNotes");
            ContractVariationIsRouteTimetableChanged        = DCC_GetColumn.Bit(dataRow,                "ContractVariationIsRouteTimetableChanged");
            ContractVariationRouteTimetableNotes            = DCC_GetColumn.String(dataRow,             "ContractVariationRouteTimetableNotes");
            ContractVariationClosingDate                    = DCC_GetColumn.DateTimeQ(dataRow,          "ContractVariationClosingDate");
            ContractVariationFuelPriceElement               = Math.Round(DCC_GetColumn.Decimal(dataRow, "ContractVariationFuelPriceElement"), 2);

            if (DCC_GetColumn.Exists(dataRow, "AgreementDescription")) AgreementDescription                   = DCC_GetColumn.String(dataRow,    "AgreementDescription");
            if (DCC_GetColumn.Exists(dataRow, "ContractReference")) ContractReference                         = DCC_GetColumn.Integer32(dataRow, "ContractReference");
            if (DCC_GetColumn.Exists(dataRow, "ContractTypeDescription")) ContractTypeDescription             = DCC_GetColumn.String(dataRow,    "ContractTypeDescription");
            if (DCC_GetColumn.Exists(dataRow, "ContractorName")) ContractorName                               = DCC_GetColumn.String(dataRow,    "ContractorName");
            if (DCC_GetColumn.Exists(dataRow, "ContractorLicenceNo")) ContractorLicenceNo                     = DCC_GetColumn.String(dataRow,    "ContractorLicenceNo");
            if (DCC_GetColumn.Exists(dataRow, "TenderReference")) TenderReference                             = DCC_GetColumn.Integer32(dataRow, "TenderReference");
            if (DCC_GetColumn.Exists(dataRow, "EstimatedAverageRevenuePerDay")) EstimatedAverageRevenuePerDay = DCC_GetColumn.Decimal(dataRow,   "EstimatedAverageRevenuePerDay");
            if (DCC_GetColumn.Exists(dataRow, "TenderPricePerDay")) TenderPricePerDay                         = DCC_GetColumn.Decimal(dataRow,   "TenderPricePerDay");

            if (DCC_GetColumn.Exists(dataRow, "ExceptionalConditions")) ExceptionalConditions                 = DCC_GetColumn.String(dataRow,  "ExceptionalConditions");
            if (DCC_GetColumn.Exists(dataRow, "VehicleCapacity")) VehicleCapacity                             = DCC_GetColumn.String(dataRow,  "VehicleCapacity");
            if (DCC_GetColumn.Exists(dataRow, "VehicleElderlyAndDisabled")) VehicleElderlyAndDisabled         = DCC_GetColumn.String(dataRow,  "VehicleElderlyAndDisabled");
            if (DCC_GetColumn.Exists(dataRow, "VehicleDimensions")) VehicleDimensions                         = DCC_GetColumn.String(dataRow,  "VehicleDimensions");
            if (DCC_GetColumn.Exists(dataRow, "AverageMilesPerDay")) AverageMilesPerDay                       = DCC_GetColumn.Decimal(dataRow, "AverageMilesPerDay");
           


            return true;
        }

        public int Save(DCC_Database db)
        {
            return (IsNew ? Insert(db) : Update(db));
        }

        int Insert(DCC_Database db)
        {
            DCC_DatabaseFrame dbf;
            if (ContractVariationNo == 0)
            {
                dbf = new DCC_DatabaseFrame("iFirstContractVariation");
            }
            else
            {
                dbf = new DCC_DatabaseFrame("iContractVariation");
            }


            dbf.AddTable("ContractVariation");
            SetCommonParameters(dbf);
            dbf.AddParameter("_PRIMARYKEY", DCC_DatabaseFrame.OUTINTEGER);
            int newKey = db.DataLayer.NonQueryPK(dbf);
            if (newKey > -1) K_ContractVariation = newKey;
            return newKey;
        }

        int Update(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uContractVariation");
            dbf.AddTable("ContractVariation");
            SetCommonParameters(dbf);
            db.DataLayer.NonQuery(dbf);
            return (dbf.OK ? K_ContractVariation : -1);
        }

        void SetCommonParameters(DCC_DatabaseFrame dbf)
        {
            dbf.AddParameter("ContractVariation_Q_Tender"                    , ContractVariation_Q_Tender);
            dbf.AddParameter("ContractVariationNo"                           , ContractVariationNo);
            dbf.AddParameter("ContractVariationDateEntered"                  , ContractVariationDateEntered);
            dbf.AddParameter("ContractVariationIsCancelled"                  , ContractVariationIsCancelled);
            dbf.AddParameter("ContractVariationIsExtended"                   , ContractVariationIsExtended);
            dbf.AddParameter("ContractVariationNotes"                        , ContractVariationNotes, 500);
            dbf.AddParameter("ContractVariation_Q_Agreement"                 , ContractVariation_Q_Agreement);
            dbf.AddParameter("ContractVariationServiceNo"                    , ContractVariationServiceNo, 500);
            dbf.AddParameter("ContractVariationRouteStart"                   , ContractVariationRouteStart, 500);
            dbf.AddParameter("ContractVariationRouteEnd"                     , ContractVariationRouteEnd, 500);
            dbf.AddParameter("ContractVariationDateStart"                    , ContractVariationDateStart);
            dbf.AddParameter("ContractVariationDateEnd"                      , ContractVariationDateEnd);
            dbf.AddParameter("ContractVariationExceptionalConditions"        , ContractVariationExceptionalConditions, 500);
            dbf.AddParameter("ContractVariationVehicleCapacity"              , ContractVariationVehicleCapacity, 500);
            dbf.AddParameter("ContractVariationVehicleElderlyAndDisabled"    , ContractVariationVehicleElderlyAndDisabled, 500);
            dbf.AddParameter("ContractVariationVehicleDimensions"            , ContractVariationVehicleDimensions, 500);
            dbf.AddParameter("ContractVariationServiceDetails"               , ContractVariationServiceDetails, 500);
            dbf.AddParameter("ContractVariationOperatesOnMon"                , ContractVariationOperatesOnMon);
            dbf.AddParameter("ContractVariationOperatesOnTue"                , ContractVariationOperatesOnTue);
            dbf.AddParameter("ContractVariationOperatesOnWed"                , ContractVariationOperatesOnWed);
            dbf.AddParameter("ContractVariationOperatesOnThu"                , ContractVariationOperatesOnThu);
            dbf.AddParameter("ContractVariationOperatesOnFri"                , ContractVariationOperatesOnFri);
            dbf.AddParameter("ContractVariationOperatesOnSat"                , ContractVariationOperatesOnSat);
            dbf.AddParameter("ContractVariationOperatesOnSun"                , ContractVariationOperatesOnSun);
            dbf.AddParameter("ContractVariationOperatesOnSchoolDays"         , ContractVariationOperatesOnSchoolDays);
            dbf.AddParameter("ContractVariationOperatesOnNonSchoolDays"      , ContractVariationOperatesOnNonSchoolDays);
            dbf.AddParameter("ContractVariationOperatesOnBankHolidays"       , ContractVariationOperatesOnBankHolidays);
            dbf.AddParameter("ContractVariationOperatesOnChristmasAndNewYear", ContractVariationOperatesOnChristmasAndNewYear);
            dbf.AddParameter("ContractVariationCalculatedNoOfDaysInOperation", ContractVariationCalculatedNoOfDaysInOperation);
            dbf.AddParameter("ContractVariationAverageMilesPerDay"           , ContractVariationAverageMilesPerDay);
            dbf.AddParameter("ContractVariationRatePerMile"                  , ContractVariationRatePerMile);
            dbf.AddParameter("ContractVariationAverageRevenuePerDay"         , ContractVariationAverageRevenuePerDay);
            dbf.AddParameter("ContractVariationAverageCostPerDay"            , ContractVariationAverageCostPerDay);
            dbf.AddParameter("ContractVariationAveragePricePerDay"           , ContractVariationAveragePricePerDay);
            dbf.AddParameter("ContractVariationAverageRevenuePerYear"        , ContractVariationAverageRevenuePerYear);
            dbf.AddParameter("ContractVariationAverageCostPerYear"           , ContractVariationAverageCostPerYear);
            dbf.AddParameter("ContractVariationAveragePricePerYear"          , ContractVariationAveragePricePerYear);
            dbf.AddParameter("ContractVariationTicketNotes"                  , ContractVariationTicketNotes, 500);
            dbf.AddParameter("ContractVariationClosingDate"                  , ContractVariationClosingDate);
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
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sContractVariationList");
                dbf.AddParameter("Q_Tender", pQ_Tender);
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
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }

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
