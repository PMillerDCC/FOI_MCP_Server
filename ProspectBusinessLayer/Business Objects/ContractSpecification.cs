using DCC_Standard;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace ProspectBusinessLayer
{

    #region ContractSpecification

    public class ContractSpecification
    {

        // Properties

        public int       K_ContractSpecification         { get; set; }
        public string    ContractReference               { get; set; }
        public string    SpecificationReference          { get; set; }
        public string    ServiceNumber                   { get; set; }
        public string    RouteStart                      { get; set; }
        public string    RouteEnd                        { get; set; }
        public DateTime? DateStart                       { get; set; }
        public DateTime? DateEnd                         { get; set; }
        public string    VehicleNumberOfSeats            { get; set; }
        public string    Agreement                       { get; set; }
        public int       Q_Agreement                     { get; set; }
        public string    ExceptionalConditions           { get; set; }
        public bool      OperatesMonday                  { get; set; }
        public bool      OperatesTuesday                 { get; set; }
        public bool      OperatesWednesday               { get; set; }
        public bool      OperatesThursday                { get; set; }
        public bool      OperatesFriday                  { get; set; }
        public bool      OperatesSaturday                { get; set; }
        public bool      OperatesSunday                  { get; set; }
        public bool      OperatesSchoolterm              { get; set; }
        public bool      OperatesNonSchoolterm           { get; set; }
        public bool      OperatesBankHolidays            { get; set; }
        public string    VehicleDimensions               { get; set; }
        public string    VehicleFeatures                 { get; set; }
        public int       NumberOfDaysOperating           { get; set; }
        public string    ServiceInformation1             { get; set; }
        public string    ServiceInformation2             { get; set; }
        public string    ServiceInformation3             { get; set; }
        public float     ContractedMiles                 { get; set; }
        public string    LicenceNo                   { get; set; }
        public bool      NumberChanged                   { get; set; }
        public bool      RouteStartChanged               { get; set; }
        public bool      RouteEndChanged                 { get; set; }
        public bool      DateStartChanged                { get; set; }
        public bool      DateEndChanged                  { get; set; }
        public bool      SeatsChanged                    { get; set; }
        public bool      AgreementChanged                { get; set; }
        public bool      ExceptionalConditionsChanged    { get; set; }
        public bool      OperatesMondayChanged           { get; set; }
        public bool      OperatesTuesdayChanged          { get; set; }
        public bool      OperatesWednesdayChanged        { get; set; }
        public bool      OperatesThursdayChanged         { get; set; }
        public bool      OperatesFridayChanged           { get; set; }
        public bool      OperatesSaturdayChanged         { get; set; }
        public bool      OperatesSundayChanged           { get; set; }
        public bool      OperatesSchoolTermChanged       { get; set; }
        public bool      OperatesNonSchoolTermChanged    { get; set; }
        public bool      OperatesBankHolidaysChanged     { get; set; }
        public bool      VehicleDimensionsChanged        { get; set; }
        public bool      VehicleFeaturesChanged          { get; set; }
        public bool      NumberOfDaysOperatingChanged    { get; set; }
        public bool      ServiceInformationChanged       { get; set; }
        public bool      RouteNormal                     { get; set; }
        public string    RouteNotes                      { get; set; }
        public bool      TicketNormal                    { get; set; }
        public string    TicketNotes                     { get; set; }
        public bool      VehicleNormal                   { get; set; }
        public string    VehicleNotes                    { get; set; }
        public bool      Cancelled                       { get; set; }
        public bool      Extended                        { get; set; }
        public bool      OperatesChristmasNewYear        { get; set; }
        public bool      OperatesChristmasNewYearChanged { get; set; }


        // Utilities

        public int PrimaryKey { get { return K_ContractSpecification; } set { K_ContractSpecification = value; } }
        public string Key { get { return PrimaryKey.ToString(); } }
        public bool IsNew { get { return PrimaryKey == -1; } }
        public override string ToString() { return PrimaryKey.ToString().PadLeft(3) + " - " + ContractReference; }



        // Constructors

        public ContractSpecification()
        {
        }

        public ContractSpecification(DataRow dataRow)
        {
            GetColumns(dataRow);
        }

        public ContractSpecification(DCC_Database db, int pK_ContractSpecification = -999, DateTime? pDateStart = null, DateTime? pDateEnd = null)
        {
            if (pK_ContractSpecification == -1) // create a new instance of ContractSpecifications and set defaults
            {
                K_ContractSpecification = -1;
            }

            else if (pK_ContractSpecification > 0) // read a record from the database for the given K_ContractSpecifications
            {
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sContractSpecification");
                dbf.AddTable("ContractSpecification");
                dbf.AddParameter("K_ContractSpecification", pK_ContractSpecification);
                db.DataLayer.SelectData(dbf);
                if (dbf.hasTables && dbf.TableZeroRows.Count == 1) GetColumns(dbf.RowZero);
            }
        }



        // Methods

        public bool GetColumns(DataRow dataRow)
        {
            if (dataRow == null) return false;

            K_ContractSpecification         = DCC_GetColumn.Integer32 (dataRow, "K_ContractSpecification");
            ContractReference               = DCC_GetColumn.String    (dataRow, "ContractReference");
            SpecificationReference          = DCC_GetColumn.String    (dataRow, "SpecificationReference");
            ServiceNumber                   = DCC_GetColumn.String    (dataRow, "ServiceNumber");
            RouteStart                      = DCC_GetColumn.String    (dataRow, "RouteStart");
            RouteEnd                        = DCC_GetColumn.String    (dataRow, "RouteEnd");
            DateStart                       = DCC_GetColumn.DateTimeQ (dataRow, "DateStart");
            DateEnd                         = DCC_GetColumn.DateTimeQ (dataRow, "DateEnd");
            VehicleNumberOfSeats            = DCC_GetColumn.String    (dataRow, "VehicleNumberOfSeats");
            Agreement                       = DCC_GetColumn.String    (dataRow, "Agreement");
            Q_Agreement                     = DCC_GetColumn.Integer32 (dataRow, "Q_Agreement");
            ExceptionalConditions           = DCC_GetColumn.String    (dataRow, "ExceptionalConditions");
            OperatesMonday                  = DCC_GetColumn.Bit       (dataRow, "OperatesMonday");
            OperatesTuesday                 = DCC_GetColumn.Bit       (dataRow, "OperatesTuesday");
            OperatesWednesday               = DCC_GetColumn.Bit       (dataRow, "OperatesWednesday");
            OperatesThursday                = DCC_GetColumn.Bit       (dataRow, "OperatesThursday");
            OperatesFriday                  = DCC_GetColumn.Bit       (dataRow, "OperatesFriday");
            OperatesSaturday                = DCC_GetColumn.Bit       (dataRow, "OperatesSaturday");
            OperatesSunday                  = DCC_GetColumn.Bit       (dataRow, "OperatesSunday");
            OperatesSchoolterm              = DCC_GetColumn.Bit       (dataRow, "OperatesSchoolterm");
            OperatesNonSchoolterm           = DCC_GetColumn.Bit       (dataRow, "OperatesNonSchoolterm");
            OperatesBankHolidays            = DCC_GetColumn.Bit       (dataRow, "OperatesBankHolidays");
            VehicleDimensions               = DCC_GetColumn.String    (dataRow, "VehicleDimensions");
            VehicleFeatures                 = DCC_GetColumn.String    (dataRow, "VehicleFeatures");
            NumberOfDaysOperating           = DCC_GetColumn.Integer32 (dataRow, "NumberOfDaysOperating");
            ServiceInformation1             = DCC_GetColumn.String    (dataRow, "ServiceInformation1");
            ServiceInformation2             = DCC_GetColumn.String    (dataRow, "ServiceInformation2");
            ServiceInformation3             = DCC_GetColumn.String    (dataRow, "ServiceInformation3");
            ContractedMiles                 = DCC_GetColumn.Float     (dataRow, "ContractedMiles");
            LicenceNo                   = DCC_GetColumn.String    (dataRow, "LicenceNo");
            NumberChanged                   = DCC_GetColumn.Bit       (dataRow, "NumberChanged");
            RouteStartChanged               = DCC_GetColumn.Bit       (dataRow, "RouteStartChanged");
            RouteEndChanged                 = DCC_GetColumn.Bit       (dataRow, "RouteEndChanged");
            DateStartChanged                = DCC_GetColumn.Bit       (dataRow, "DateStartChanged");
            DateEndChanged                  = DCC_GetColumn.Bit       (dataRow, "DateEndChanged");
            SeatsChanged                    = DCC_GetColumn.Bit       (dataRow, "SeatsChanged");
            AgreementChanged                = DCC_GetColumn.Bit       (dataRow, "AgreementChanged");
            ExceptionalConditionsChanged    = DCC_GetColumn.Bit       (dataRow, "ExceptionalConditionsChanged");
            OperatesMondayChanged           = DCC_GetColumn.Bit       (dataRow, "OperatesMondayChanged");
            OperatesTuesdayChanged          = DCC_GetColumn.Bit       (dataRow, "OperatesTuesdayChanged");
            OperatesWednesdayChanged        = DCC_GetColumn.Bit       (dataRow, "OperatesWednesdayChanged");
            OperatesThursdayChanged         = DCC_GetColumn.Bit       (dataRow, "OperatesThursdayChanged");
            OperatesFridayChanged           = DCC_GetColumn.Bit       (dataRow, "OperatesFridayChanged");
            OperatesSaturdayChanged         = DCC_GetColumn.Bit       (dataRow, "OperatesSaturdayChanged");
            OperatesSundayChanged           = DCC_GetColumn.Bit       (dataRow, "OperatesSundayChanged");
            OperatesSchoolTermChanged       = DCC_GetColumn.Bit       (dataRow, "OperatesSchoolTermChanged");
            OperatesNonSchoolTermChanged    = DCC_GetColumn.Bit       (dataRow, "OperatesNonSchoolTermChanged");
            OperatesBankHolidaysChanged     = DCC_GetColumn.Bit       (dataRow, "OperatesBankHolidaysChanged");
            VehicleDimensionsChanged        = DCC_GetColumn.Bit       (dataRow, "VehicleDimensionsChanged");
            VehicleFeaturesChanged          = DCC_GetColumn.Bit       (dataRow, "VehicleFeaturesChanged");
            NumberOfDaysOperatingChanged    = DCC_GetColumn.Bit       (dataRow, "NumberOfDaysOperatingChanged");
            ServiceInformationChanged       = DCC_GetColumn.Bit       (dataRow, "ServiceInformationChanged");
            RouteNormal                     = DCC_GetColumn.Bit       (dataRow, "RouteNormal");
            RouteNotes                      = DCC_GetColumn.String    (dataRow, "RouteNotes");
            TicketNormal                    = DCC_GetColumn.Bit       (dataRow, "TicketNormal");
            TicketNotes                     = DCC_GetColumn.String    (dataRow, "TicketNotes");
            VehicleNormal                   = DCC_GetColumn.Bit       (dataRow, "VehicleNormal");
            VehicleNotes                    = DCC_GetColumn.String    (dataRow, "VehicleNotes");
            Cancelled                       = DCC_GetColumn.Bit       (dataRow, "Cancelled");
            Extended                        = DCC_GetColumn.Bit       (dataRow, "Extended");
            OperatesChristmasNewYear        = DCC_GetColumn.Bit       (dataRow, "OperatesChristmasNewYear");
            OperatesChristmasNewYearChanged = DCC_GetColumn.Bit       (dataRow, "OperatesChristmasNewYearChanged");

            return true;
        }

        public int Save(DCC_Database db)
        {
            return (IsNew ? Insert(db) : Update(db));
        }

        int Insert(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("iContractSpecification");
            dbf.AddTable("ContractSpecification");
            dbf.AddParameter("_PRIMARYKEY", DCC_DatabaseFrame.OUTINTEGER);
            int newKey = db.DataLayer.NonQueryPK(dbf);
            if (newKey > -1) K_ContractSpecification = newKey;
            return newKey;
        }

        int Update(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uContractSpecification");
            dbf.AddTable("ContractSpecification");
            dbf.AddParameter("K_ContractSpecification", K_ContractSpecification);
            dbf.AddParameter("SpecificationReference", SpecificationReference);
            dbf.AddParameter("ServiceNumber", ServiceNumber);
            dbf.AddParameter("RouteStart", RouteStart);
            dbf.AddParameter("RouteEnd", RouteEnd);
            dbf.AddParameter("DateStart", DateStart);
            dbf.AddParameter("DateEnd", DateEnd);
            dbf.AddParameter("VehicleNumberOfSeats", VehicleNumberOfSeats);
            dbf.AddParameter("Agreement", Agreement);
            dbf.AddParameter("Q_Agreement", Q_Agreement);
            dbf.AddParameter("ExceptionalConditions", ExceptionalConditions);
            dbf.AddParameter("OperatesMonday", OperatesMonday);
            dbf.AddParameter("OperatesTuesday", OperatesTuesday);
            dbf.AddParameter("OperatesWednesday", OperatesWednesday);
            dbf.AddParameter("OperatesThursday", OperatesThursday);
            dbf.AddParameter("OperatesFriday", OperatesFriday);
            dbf.AddParameter("OperatesSaturday", OperatesSaturday);
            dbf.AddParameter("OperatesSunday", OperatesSunday);
            dbf.AddParameter("OperatesSchoolterm", OperatesSchoolterm);
            dbf.AddParameter("OperatesNonSchoolterm", OperatesNonSchoolterm);
            dbf.AddParameter("OperatesBankHolidays", OperatesBankHolidays);
            dbf.AddParameter("VehicleDimensions", VehicleDimensions);
            dbf.AddParameter("VehicleFeatures", VehicleFeatures);
            dbf.AddParameter("NumberOfDaysOperating", NumberOfDaysOperating);
            dbf.AddParameter("ServiceInformation1", ServiceInformation1);
            dbf.AddParameter("ServiceInformation2", ServiceInformation2);
            dbf.AddParameter("ServiceInformation3", ServiceInformation3);
            dbf.AddParameter("ContractedMiles", ContractedMiles);
            dbf.AddParameter("LicenceNo", LicenceNo);
            dbf.AddParameter("NumberChanged", NumberChanged);
            dbf.AddParameter("RouteStartChanged", RouteStartChanged);
            dbf.AddParameter("RouteEndChanged", RouteEndChanged);
            dbf.AddParameter("DateStartChanged", DateStartChanged);
            dbf.AddParameter("DateEndChanged", DateEndChanged);
            dbf.AddParameter("SeatsChanged", SeatsChanged);
            dbf.AddParameter("AgreementChanged", AgreementChanged);
            dbf.AddParameter("ExceptionalConditionsChanged", ExceptionalConditionsChanged);
            dbf.AddParameter("OperatesMondayChanged", OperatesMondayChanged);
            dbf.AddParameter("OperatesTuesdayChanged", OperatesTuesdayChanged);
            dbf.AddParameter("OperatesWednesdayChanged", OperatesWednesdayChanged);
            dbf.AddParameter("OperatesThursdayChanged", OperatesThursdayChanged);
            dbf.AddParameter("OperatesFridayChanged", OperatesFridayChanged);
            dbf.AddParameter("OperatesSaturdayChanged", OperatesSaturdayChanged);
            dbf.AddParameter("OperatesSundayChanged", OperatesSundayChanged);
            dbf.AddParameter("OperatesSchoolTermChanged", OperatesSchoolTermChanged);
            dbf.AddParameter("OperatesNonSchoolTermChanged", OperatesNonSchoolTermChanged);
            dbf.AddParameter("OperatesBankHolidaysChanged", OperatesBankHolidaysChanged);
            dbf.AddParameter("VehicleDimensionsChanged", VehicleDimensionsChanged);
            dbf.AddParameter("VehicleFeaturesChanged", VehicleFeaturesChanged);
            dbf.AddParameter("NumberOfDaysOperatingChanged", NumberOfDaysOperatingChanged);
            dbf.AddParameter("ServiceInformationChanged", ServiceInformationChanged);
            dbf.AddParameter("RouteNormal", RouteNormal);
            dbf.AddParameter("RouteNotes", RouteNotes);
            dbf.AddParameter("TicketNormal", TicketNormal);
            dbf.AddParameter("TicketNotes", TicketNotes);
            dbf.AddParameter("VehicleNormal", VehicleNormal);
            dbf.AddParameter("VehicleNotes", VehicleNotes);
            dbf.AddParameter("Cancelled", Cancelled);
            dbf.AddParameter("Extended", Extended);
            dbf.AddParameter("OperatesChristmasNewYear", OperatesChristmasNewYear);
            dbf.AddParameter("OperatesChristmasNewYearChanged", OperatesChristmasNewYearChanged);
            db.DataLayer.NonQuery(dbf);
            return (dbf.OK ? K_ContractSpecification : -1);
        }

        void SetCommonParameters(DCC_DatabaseFrame dbf)
        {
            dbf.AddParameter("ContractReference", ContractReference, 7);
            dbf.AddParameter("SpecificationReference", SpecificationReference, 4);
            dbf.AddParameter("ServiceNumber", ServiceNumber, 500);
            dbf.AddParameter("RouteStart", RouteStart, 500);
            dbf.AddParameter("RouteEnd", RouteEnd, 500);
            dbf.AddParameter("DateStart", DateStart);
            dbf.AddParameter("DateEnd", DateEnd);
            dbf.AddParameter("VehicleNumberOfSeats", VehicleNumberOfSeats, 500);
            dbf.AddParameter("Agreement", Agreement, 2);
            dbf.AddParameter("Q_Agreement", Q_Agreement);
            dbf.AddParameter("ExceptionalConditions", ExceptionalConditions, 500);
            dbf.AddParameter("OperatesMonday", OperatesMonday);
            dbf.AddParameter("OperatesTuesday", OperatesTuesday);
            dbf.AddParameter("OperatesWednesday", OperatesWednesday);
            dbf.AddParameter("OperatesThursday", OperatesThursday);
            dbf.AddParameter("OperatesFriday", OperatesFriday);
            dbf.AddParameter("OperatesSaturday", OperatesSaturday);
            dbf.AddParameter("OperatesSunday", OperatesSunday);
            dbf.AddParameter("OperatesSchoolTerm", OperatesSchoolterm);
            dbf.AddParameter("OperatesNonSchoolTerm", OperatesNonSchoolterm);
            dbf.AddParameter("OperatesBankHolidays", OperatesBankHolidays);
            dbf.AddParameter("VehicleDimensions", VehicleDimensions, 500);
            dbf.AddParameter("VehicleFeatures", VehicleFeatures, 500);
            dbf.AddParameter("NumberOfDaysOperating", NumberOfDaysOperating);
            dbf.AddParameter("ServiceInformation1", ServiceInformation1, 500);
            dbf.AddParameter("ServiceInformation2", ServiceInformation2, 500);
            dbf.AddParameter("ServiceInformation3", ServiceInformation3, 500);
            dbf.AddParameter("ContractedMiles", ContractedMiles);
            dbf.AddParameter("LicenceNo", LicenceNo, 9);
            dbf.AddParameter("NumberChanged", NumberChanged);
            dbf.AddParameter("RouteStartChanged", RouteStartChanged);
            dbf.AddParameter("RouteEndChanged", RouteEndChanged);
            dbf.AddParameter("DateStartChanged", DateStartChanged);
            dbf.AddParameter("SeatsChanged", SeatsChanged);
            dbf.AddParameter("AgreementChanged", AgreementChanged);
            dbf.AddParameter("ExceptionalConditionsChanged", ExceptionalConditionsChanged);
            dbf.AddParameter("OperatesMondayChanged", OperatesMondayChanged);
            dbf.AddParameter("OperatesTuesdayChanged", OperatesTuesdayChanged);
            dbf.AddParameter("OperatesWednesdayChanged", OperatesWednesdayChanged);
            dbf.AddParameter("OperatesThursdayChanged", OperatesThursdayChanged);
            dbf.AddParameter("OperatesFridayChanged", OperatesFridayChanged);
            dbf.AddParameter("OperatesSaturdayChanged", OperatesSaturdayChanged);
            dbf.AddParameter("OperatesSundayChanged", OperatesSundayChanged);
            dbf.AddParameter("OperatesSchoolTermChanged", OperatesSchoolTermChanged);
            dbf.AddParameter("OperatesNonSchoolTermChanged", OperatesNonSchoolTermChanged);
            dbf.AddParameter("OperatesBankHolidaysChanged", OperatesBankHolidaysChanged);
            dbf.AddParameter("VehicleDimensionsChanged", VehicleDimensionsChanged);
            dbf.AddParameter("VehicleFeaturesChanged", VehicleFeaturesChanged);
            dbf.AddParameter("NumberOfDaysOperatingChanged", NumberOfDaysOperatingChanged);
            dbf.AddParameter("ServiceInformationChanged", ServiceInformationChanged);
            dbf.AddParameter("RouteNormal", RouteNormal);
            dbf.AddParameter("RouteNotes", RouteNotes, 500);
            dbf.AddParameter("TicketNormal", TicketNormal);
            dbf.AddParameter("TicketNotes", TicketNotes, 500);
            dbf.AddParameter("VehicleNormal", VehicleNormal);
            dbf.AddParameter("VehicleNotes", VehicleNotes, 500);
            dbf.AddParameter("Cancelled", Cancelled);
            dbf.AddParameter("Extended", Extended);
            dbf.AddParameter("OperatesChristmasNewYear", OperatesChristmasNewYear);
            dbf.AddParameter("OperatesChristmasNewYearChanged", OperatesChristmasNewYearChanged);
        }

    }

    #endregion


    #region ContractSpecificationList

    public class ContractSpecificationList : IEnumerable<ContractSpecification>
    {

        // Properties

        public List<object> All = new List<object>();



        // Utilities

        public int Count { get { return All.Count; } }
        public override string ToString() { return "ContractSpecificationList (" + Count + " items)"; }



        // Constructors

        public ContractSpecificationList()
        {
        }

        public ContractSpecificationList(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sContractSpecificationList");
            dbf.AddTable("ContractSpecification");
            db.DataLayer.SelectData(dbf);
            if (dbf.hasTables)
            {
                foreach (DataRow dataRow in dbf.TableZeroRows) Add(dataRow);
            }
        }



        // Methods

        public void Add(DataRow dataRow)
        {
            ContractSpecification contractSpecification = new ContractSpecification(dataRow);
            All.Add(contractSpecification);
        }



        // Enumerator

        public IEnumerator<ContractSpecification> GetEnumerator() { foreach (object o in All) yield return (ContractSpecification)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }

    }

    #endregion


    #region ContractSpecificationGrid

    public class ContractSpecificationGrid : DCC_Grid
    {
        public ContractSpecificationGrid(DCC_Database db)
        {
            ContractSpecificationList items = new ContractSpecificationList(db);
            TotalRows = items.All;
            Count = CountFiltered = TotalRows.Count;
        }
    }

    #endregion

}
