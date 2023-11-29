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

        public int       K_ContractVariation                  { get; set; }
        public int       Q_Tender                             { get; set; }
        public int       Q_Agreement                          { get; set; }
        public string    SpecificationReference               { get; set; }
        public string    LicenceNumber                        { get; set; }
        public string    Agreement                            { get; set; }
        public bool      AgreementChanged                     { get; set; }
        public decimal   ContractedMiles                      { get; set; }
        public string    ServiceNo                            { get; set; }
        public bool      ServiceNoChanged                     { get; set; }
        public string    RouteStart                           { get; set; }
        public bool      RouteStartChanged                    { get; set; }
        public string    RouteEnd                             { get; set; }
        public bool      RouteEndChanged                      { get; set; }
        public DateTime? DateStart                            { get; set; }
        public bool      DateStartChanged                     { get; set; }
        public DateTime? DateEnd                              { get; set; }
        public bool      DateEndChanged                       { get; set; }
        public string    ExceptionalConditions                { get; set; }
        public bool      ExceptionalConditionsChanged         { get; set; }
        public string    VehicleCapacity                      { get; set; }
        public bool      VehicleCapacityChanged               { get; set; }
        public string    VehicleElderlyAndDisabled            { get; set; }
        public bool      VehicleElderlyAndDisabledChanged     { get; set; }
        public string    VehicleDimensions                    { get; set; }
        public bool      VehicleDimensionsChanged             { get; set; }
        public string    ServiceDetails1                      { get; set; }
        public string    ServiceDetails2                      { get; set; }
        public string    ServiceDetails3                      { get; set; }
        public bool      ServiceDetailsChanged                { get; set; }
        public bool      OperatesOnMon                        { get; set; }
        public bool      OperatesOnMonChanged                 { get; set; }
        public bool      OperatesOnTue                        { get; set; }
        public bool      OperatesOnTueChanged                 { get; set; }
        public bool      OperatesOnWed                        { get; set; }
        public bool      OperatesOnWedChanged                 { get; set; }
        public bool      OperatesOnThu                        { get; set; }
        public bool      OperatesOnThuChanged                 { get; set; }
        public bool      OperatesOnFri                        { get; set; }
        public bool      OperatesOnFriChanged                 { get; set; }
        public bool      OperatesOnSat                        { get; set; }
        public bool      OperatesOnSatChanged                 { get; set; }
        public bool      OperatesOnSun                        { get; set; }
        public bool      OperatesOnSunChanged                 { get; set; }
        public bool      OperatesOnSchoolDays                 { get; set; }
        public bool      OperatesOnSchoolDaysChanged          { get; set; }
        public bool      OperatesOnNonSchoolDays              { get; set; }
        public bool      OperatesOnNonSchoolDaysChanged       { get; set; }
        public bool      OperatesOnBankHolidays               { get; set; }
        public bool      OperatesOnBankHolidaysChanged        { get; set; }
        public bool      OperatesOnChristmasAndNewYear        { get; set; }
        public bool      OperatesOnChristmasAndNewYearChanged { get; set; }
        public int       NoOfDaysOfOperationPerYear           { get; set; }
        public bool      NoOfDaysOfOperationPerYearChanged    { get; set; }
        public string    RouteNotes                           { get; set; }
        public bool      RouteChanged                         { get; set; }
        public string    TicketNotes                          { get; set; }
        public bool      TicketChanged                        { get; set; }
        public string    VehicleNotes                         { get; set; }
        public bool      VehicleChanged                       { get; set; }
        public bool      Extended                             { get; set; }
        public bool      Cancelled                            { get; set; }
        public string    CvContractReference                  { get; set; }
        public decimal   CvAverageRevenuePerDay               { get; set; }
        public decimal   CvAverageCostPerDay                  { get; set; }
        public decimal   CvAveragePricePerDay                 { get; set; }
        public decimal   CvAverageRevenuePerYear              { get; set; }
        public decimal   CvAverageCostPerYear                 { get; set; }
        public decimal   CvAveragePricePerYear                { get; set; }
        public decimal   CvContractedMiles                    { get; set; }
        public decimal   CvRatePerMile                        { get; set; }
        public int       CvXBNDPCENT                          { get; set; }
        public DateTime? CvDateVariationsStarta               { get; set; }
        public DateTime? CvDateVariationsEnda                 { get; set; }
        public int       CvNumberOfDaysOperating              { get; set; }
        public bool      CvCancelled                          { get; set; }
        public bool      CvPriceChanged                       { get; set; }
        public string    CvServiceInformation                 { get; set; }
        public int       VariationNo                          { get; set; }
        public string    CvVariationNumber                    { get; set; }
        public string    AgreementDescription                 { get; set; }
        public int       CalculatedNoOfDaysInOperation        { get; set; }

        public string    CvDateVariationsStartatext           { get { return DCC_Formatting.NullDateTxt(CvDateVariationsStarta, "dd MMM yyyy"); } }
        public string    CvDateVariationsStartaSort           { get { return DCC_Formatting.NullDateTxt(CvDateVariationsStarta, "yyyyMMddHHmm"); } }
        public string    CvDateVariationsEndatext             { get { return DCC_Formatting.NullDateTxt(CvDateVariationsEnda, "dd MMM yyyy"); } }
        public string    CvDateVariationsEndaSort             { get { return DCC_Formatting.NullDateTxt(CvDateVariationsEnda, "yyyyMMddHHmm"); } }
        public string    VariationNoText                      { get { return VariationNo.ToString().PadLeft(2, '0'); } }

       
        
        // Utilities

        public int PrimaryKey { get { return K_ContractVariation; } set { K_ContractVariation = value; } }
        public string Key { get { return PrimaryKey.ToString(); } }
        public bool IsNew { get { return PrimaryKey == -1; } }
        public override string ToString() { return PrimaryKey.ToString().PadLeft(3) + " - " + VariationNoText; }



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
                Q_Tender = pK_Tender;
            }

            else if (pK_ContractVariation > 0) // read a record from the database for the given K_ContractVariation
            {
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sContractVariation");
                dbf.AddTable("ContractVariation");
                dbf.AddParameter("K_ContractVariation", pK_ContractVariation);
                db.DataLayer.SelectData(dbf);
                if (dbf.hasTables && dbf.TableZeroRows.Count == 1) GetColumns(dbf.RowZero);
            }
        }



        // Methods

        public bool GetColumns(DataRow dataRow)
        {
            if (dataRow == null) return false;

            K_ContractVariation                  = DCC_GetColumn.Integer32          (dataRow, "K_ContractVariation");
            Q_Tender                             = DCC_GetColumn.Integer32          (dataRow, "Q_Tender");
            Q_Agreement                          = DCC_GetColumn.Integer32          (dataRow, "Q_Agreement");
            SpecificationReference               = DCC_GetColumn.String             (dataRow, "SpecificationReference");
            LicenceNumber                        = DCC_GetColumn.String             (dataRow, "LicenceNumber");
            Agreement                            = DCC_GetColumn.String             (dataRow, "Agreement");
            AgreementChanged                     = DCC_GetColumn.Bit                (dataRow, "AgreementChanged");
            ContractedMiles                      = DCC_GetColumn.Integer32          (dataRow, "ContractedMiles");
            ServiceNo                            = DCC_GetColumn.String             (dataRow, "ServiceNo");
            ServiceNoChanged                     = DCC_GetColumn.Bit                (dataRow, "ServiceNoChanged");
            RouteStart                           = DCC_GetColumn.String             (dataRow, "RouteStart");
            RouteStartChanged                    = DCC_GetColumn.Bit                (dataRow, "RouteStartChanged");
            RouteEnd                             = DCC_GetColumn.String             (dataRow, "RouteEnd");
            RouteEndChanged                      = DCC_GetColumn.Bit                (dataRow, "RouteEndChanged");
            DateStart                            = DCC_GetColumn.DateTime           (dataRow, "DateStart");
            DateStartChanged                     = DCC_GetColumn.Bit                (dataRow, "DateStartChanged");
            DateEnd                              = DCC_GetColumn.DateTime           (dataRow, "DateEnd");
            DateEndChanged                       = DCC_GetColumn.Bit                (dataRow, "DateEndChanged");
            ExceptionalConditions                = DCC_GetColumn.String             (dataRow, "ExceptionalConditions");
            ExceptionalConditionsChanged         = DCC_GetColumn.Bit                (dataRow, "ExceptionalConditionsChanged");
            VehicleCapacity                      = DCC_GetColumn.String             (dataRow, "VehicleCapacity");
            VehicleCapacityChanged               = DCC_GetColumn.Bit                (dataRow, "VehicleCapacityChanged");
            VehicleElderlyAndDisabled            = DCC_GetColumn.String             (dataRow, "VehicleElderlyAndDisabled");
            VehicleElderlyAndDisabledChanged     = DCC_GetColumn.Bit                (dataRow, "VehicleElderlyAndDisabledChanged");
            VehicleDimensions                    = DCC_GetColumn.String             (dataRow, "VehicleDimensions");
            VehicleDimensionsChanged             = DCC_GetColumn.Bit                (dataRow, "VehicleDimensionsChanged");
            OperatesOnMon                        = DCC_GetColumn.Bit                (dataRow, "OperatesOnMon");
            OperatesOnMonChanged                 = DCC_GetColumn.Bit                (dataRow, "OperatesOnMonChanged");
            OperatesOnTue                        = DCC_GetColumn.Bit                (dataRow, "OperatesOnTue");
            OperatesOnTueChanged                 = DCC_GetColumn.Bit                (dataRow, "OperatesOnTueChanged");
            OperatesOnWed                        = DCC_GetColumn.Bit                (dataRow, "OperatesOnWed");
            OperatesOnWedChanged                 = DCC_GetColumn.Bit                (dataRow, "OperatesOnWedChanged");
            OperatesOnThu                        = DCC_GetColumn.Bit                (dataRow, "OperatesOnThu");
            OperatesOnThuChanged                 = DCC_GetColumn.Bit                (dataRow, "OperatesOnThuChanged");
            OperatesOnFri                        = DCC_GetColumn.Bit                (dataRow, "OperatesOnFri");
            OperatesOnFriChanged                 = DCC_GetColumn.Bit                (dataRow, "OperatesOnFriChanged");
            OperatesOnSat                        = DCC_GetColumn.Bit                (dataRow, "OperatesOnSat");
            OperatesOnSatChanged                 = DCC_GetColumn.Bit                (dataRow, "OperatesOnSatChanged");
            OperatesOnSun                        = DCC_GetColumn.Bit                (dataRow, "OperatesOnSun");
            OperatesOnSunChanged                 = DCC_GetColumn.Bit                (dataRow, "OperatesOnSunChanged");
            OperatesOnSchoolDays                 = DCC_GetColumn.Bit                (dataRow, "OperatesOnSchoolDays");
            OperatesOnSchoolDaysChanged          = DCC_GetColumn.Bit                (dataRow, "OperatesOnSchoolDaysChanged");
            OperatesOnNonSchoolDays              = DCC_GetColumn.Bit                (dataRow, "OperatesOnNonSchoolDays");
            OperatesOnNonSchoolDaysChanged       = DCC_GetColumn.Bit                (dataRow, "OperatesOnNonSchoolDaysChanged");
            OperatesOnBankHolidays               = DCC_GetColumn.Bit                (dataRow, "OperatesOnBankHolidays");
            OperatesOnBankHolidaysChanged        = DCC_GetColumn.Bit                (dataRow, "OperatesOnBankHolidaysChanged");
            OperatesOnChristmasAndNewYear        = DCC_GetColumn.Bit                (dataRow, "OperatesOnChristmasAndNewYear");
            OperatesOnChristmasAndNewYearChanged = DCC_GetColumn.Bit                (dataRow, "OperatesOnChristmasAndNewYearChanged");
            NoOfDaysOfOperationPerYear           = DCC_GetColumn.Integer32          (dataRow, "NoOfDaysOfOperationPerYear");
            NoOfDaysOfOperationPerYearChanged    = DCC_GetColumn.Bit                (dataRow, "NoOfDaysOfOperationPerYearChanged");
            RouteNotes                           = DCC_GetColumn.String             (dataRow, "RouteNotes");
            RouteChanged                         = DCC_GetColumn.Bit                (dataRow, "RouteChanged");
            TicketChanged                        = DCC_GetColumn.Bit                (dataRow, "TicketChanged");
            TicketNotes                          = DCC_GetColumn.String             (dataRow, "TicketNotes");
            VehicleDimensions                    = DCC_GetColumn.String             (dataRow, "VehicleDimensions");
            VehicleChanged                       = DCC_GetColumn.Bit                (dataRow, "VehicleChanged");
            VehicleNotes                         = DCC_GetColumn.String             (dataRow, "VehicleNotes");
            Cancelled                            = DCC_GetColumn.Bit                (dataRow, "Cancelled");
            Extended                             = DCC_GetColumn.Bit                (dataRow, "Extended");
            CvContractReference                  = DCC_GetColumn.String             (dataRow, "CvContractReference");
            CvContractedMiles                    = DCC_GetColumn.Integer32          (dataRow, "CvContractedMiles");
            CvAverageRevenuePerDay               = Math.Round(DCC_GetColumn.Decimal (dataRow, "CvAverageRevenuePerDay"), 2);
            CvAverageRevenuePerYear              = Math.Round(DCC_GetColumn.Decimal (dataRow, "CvAverageRevenuePerYear"), 2);
            CvAverageCostPerDay                  = Math.Round(DCC_GetColumn.Decimal (dataRow, "CvAverageCostPerDay"), 2);
            CvAverageCostPerYear                 = Math.Round(DCC_GetColumn.Decimal (dataRow, "CvAverageCostPerYear"), 2);
            CvAveragePricePerDay                 = Math.Round(DCC_GetColumn.Decimal (dataRow, "CvAveragePricePerDay"), 2);
            CvAveragePricePerYear                = Math.Round(DCC_GetColumn.Decimal (dataRow, "CvAveragePricePerYear"), 2);
            CvContractedMiles                    = Math.Round(DCC_GetColumn.Decimal (dataRow, "CvContractedMiles"), 2);
            CvRatePerMile                        = Math.Round(DCC_GetColumn.Decimal (dataRow, "CvRatePerMile"), 2);
            CvXBNDPCENT                          = DCC_GetColumn.Integer32          (dataRow, "CvXbndpcent");
            CvDateVariationsStarta               = DCC_GetColumn.DateTimeQ          (dataRow, "CvDateVariationsStarta");
            CvDateVariationsEnda                 = DCC_GetColumn.DateTimeQ          (dataRow, "CvDateVariationsEnda");
            CvNumberOfDaysOperating              = DCC_GetColumn.Integer32          (dataRow, "CvNumberOfDaysOperating");
            CvCancelled                          = DCC_GetColumn.Bit                (dataRow, "CvCancelled");
            CvPriceChanged                       = DCC_GetColumn.Bit                (dataRow, "CvPriceChanged");
            CvServiceInformation                 = DCC_GetColumn.String             (dataRow, "CvServiceInformation");
            VariationNo                          = DCC_GetColumn.Integer32          (dataRow, "VariationNo");
            CvVariationNumber                    = DCC_GetColumn.String             (dataRow, "CvVariationNumber");

            if (DCC_GetColumn.Exists(dataRow, "AgreementDescription")) AgreementDescription = DCC_GetColumn.String(dataRow, "AgreementDescription");

            return true;
        }

        public int Save(DCC_Database db)
        {
            return (IsNew ? Insert(db) : Update(db));
        }

        int Insert(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("iContractVariation");
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
            dbf.AddParameter("K_ContractVariation", K_ContractVariation);
            SetCommonParameters(dbf);
            db.DataLayer.NonQuery(dbf);
            return (dbf.OK ? K_ContractVariation : -1);
        }

        void SetCommonParameters(DCC_DatabaseFrame dbf)
        {
            dbf.AddParameter("Q_Tender"               , Q_Tender);
            dbf.AddParameter("CvContractReference"    , CvContractReference       , 5);
            dbf.AddParameter("CvAverageRevenuePerDay" , CvAverageRevenuePerDay);
            dbf.AddParameter("CvAverageCostPerDay"    , CvAverageCostPerDay);
            dbf.AddParameter("CvAveragePricePerDay"   , CvAveragePricePerDay);
            dbf.AddParameter("CvAverageRevenuePerYear", CvAverageRevenuePerYear);
            dbf.AddParameter("CvAverageCostPerYear"   , CvAverageCostPerYear);
            dbf.AddParameter("CvAveragePricePerYear"  , CvAveragePricePerYear);
            dbf.AddParameter("CvContractedMiles"      , CvContractedMiles);
            dbf.AddParameter("CvRatePerMile"          , CvRatePerMile);
            dbf.AddParameter("CvXBNDPCENT"            , CvXBNDPCENT);
            dbf.AddParameter("CvDateVariationsStarta" , CvDateVariationsStarta);
            dbf.AddParameter("CvDateVariationsEnda"   , CvDateVariationsEnda);
            dbf.AddParameter("CvNumberOfDaysOperating", CvNumberOfDaysOperating);
            dbf.AddParameter("CvCancelled"            , CvCancelled);
            dbf.AddParameter("CvPriceChanged"         , CvPriceChanged);
            dbf.AddParameter("CvServiceInformation"   , CvServiceInformation      ,4000);
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

        public ContractVariationList(DCC_Database db, int pQ_Tender = -999)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame();
            if (pQ_Tender > 0)
            {
                dbf = new DCC_DatabaseFrame("sContractVariationListByTender");
                dbf.AddParameter("Q_Tender ", pQ_Tender);
            }

            else
            {
                dbf = new DCC_DatabaseFrame("sContractVariationList");
            }

            dbf.AddTable("ContractVariation");
            db.DataLayer.SelectData(dbf);
            if (dbf.hasTables)
            {
                foreach (DataRow dataRow in dbf.TableZeroRows) Add(new ContractVariation(dataRow));
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
        public ContractVariationGrid(DCC_Database db, int pQ_ContractVariation = -999)
        {
            ContractVariationList items = new ContractVariationList(db, pQ_ContractVariation);
            TotalRows = items.All;
            Count = CountFiltered = TotalRows.Count;
        }
    }

    #endregion

}
