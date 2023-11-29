using DCC_Standard;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace ProspectBusinessLayer
{

    #region ContractPayment

    public class ContractPayment
    {

        // Properties

        public int       K_ContractPayment        { get; set; }
        public string    MasterSpecification      { get; set; }
        public string    Specification            { get; set; }
        public string    ServiceNumber            { get; set; }
        public string    RouteStart               { get; set; }
        public string    RouteEnd                 { get; set; }
        public int       PricePerDay              { get; set; }
        public int       NumberOfDays             { get; set; }
        public int       Amount                   { get; set; }
        public string    LicenceNo            { get; set; }
        public DateTime? DateStart                { get; set; }
        public DateTime? DateEnd                  { get; set; }
        public bool      OperatesMonday           { get; set; }
        public bool      OperatesTuesday          { get; set; }
        public bool      OperatesWednesday        { get; set; }
        public bool      OperatesThursday         { get; set; }
        public bool      OperatesFriday           { get; set; }
        public bool      OperatesSaturday         { get; set; }
        public bool      OperatesSunday           { get; set; }
        public bool      OperatesBankHolidays     { get; set; }
        public bool      OperatesSchoolTerm       { get; set; }
        public bool      OperatesNonSchoolTerm    { get; set; }
        public bool      OperatesChristmasNewYear { get; set; }
        public string    Agreement                { get; set; }
        public int       Q_Agreement              { get; set; }
        public int       Variation                { get; set; }
        public int       NetAmount                { get; set; }
        public string    Notes                    { get; set; }
        public bool      FirstStop                { get; set; }
        public string    DispSpecification        { get; set; }
        public string    VariationNumber          { get; set; }
        public string    SortName                 { get; set; }



        // Utilities

        public int      PrimaryKey { get { return K_ContractPayment; } set { K_ContractPayment = value; } }
        public string   Key { get { return PrimaryKey.ToString(); } }
        public bool     IsNew { get { return PrimaryKey == -1; } }
        public override string ToString() { return PrimaryKey.ToString().PadLeft(3) + " - " + MasterSpecification; }



        // Constructors

        public ContractPayment()
        {
        }

        public ContractPayment(DataRow dataRow)
        {
            GetColumns(dataRow);
        }

        public ContractPayment(DCC_Database db, int pK_ContractPayment = -999)
        {
            if (pK_ContractPayment == -1) // create a new instance of ContractPayment and set defaults
            {
                K_ContractPayment = -1;
            }

            else if (pK_ContractPayment > 0) // read a record from the database for the given K_ContractPayment
            {
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sContractPayment");
                dbf.AddTable("ContractPayment");
                dbf.AddParameter("K_ContractPayment", pK_ContractPayment);
                db.DataLayer.SelectData(dbf);
                if (dbf.hasTables && dbf.TableZeroRows.Count == 1) GetColumns(dbf.RowZero);
            }
        }



        // Methods

        public bool GetColumns(DataRow dataRow)
        {
            if (dataRow == null) return false;

            K_ContractPayment        = DCC_GetColumn.Integer32 (dataRow, "K_ContractPayment");
            MasterSpecification      = DCC_GetColumn.String    (dataRow, "MasterSpecification");
            Specification            = DCC_GetColumn.String    (dataRow, "Specification");
            ServiceNumber            = DCC_GetColumn.String    (dataRow, "ServiceNumber");
            RouteStart               = DCC_GetColumn.String    (dataRow, "RouteStart");
            RouteEnd                 = DCC_GetColumn.String    (dataRow, "RouteEnd");
            PricePerDay              = DCC_GetColumn.Integer32 (dataRow, "PricePerDay");
            NumberOfDays             = DCC_GetColumn.Integer32 (dataRow, "NumberOfDays");
            Amount                   = DCC_GetColumn.Integer32 (dataRow, "Amount");
            LicenceNo                = DCC_GetColumn.String    (dataRow, "LicenceNo");
            DateStart                = DCC_GetColumn.DateTimeQ (dataRow, "DateStart");
            DateEnd                  = DCC_GetColumn.DateTimeQ (dataRow, "DateEnd");
            OperatesMonday           = DCC_GetColumn.Bit       (dataRow, "OperatesMonday");
            OperatesTuesday          = DCC_GetColumn.Bit       (dataRow, "OperatesTuesday");
            OperatesWednesday        = DCC_GetColumn.Bit       (dataRow, "OperatesWednesday");
            OperatesThursday         = DCC_GetColumn.Bit       (dataRow, "OperatesThursday");
            OperatesFriday           = DCC_GetColumn.Bit       (dataRow, "OperatesFriday");
            OperatesSaturday         = DCC_GetColumn.Bit       (dataRow, "OperatesSaturday");
            OperatesSunday           = DCC_GetColumn.Bit       (dataRow, "OperatesSunday");
            OperatesBankHolidays     = DCC_GetColumn.Bit       (dataRow, "OperatesBankHolidays");
            OperatesSchoolTerm       = DCC_GetColumn.Bit       (dataRow, "OperatesSchoolTerm");
            OperatesNonSchoolTerm    = DCC_GetColumn.Bit       (dataRow, "OperatesNonSchoolTerm");
            OperatesChristmasNewYear = DCC_GetColumn.Bit       (dataRow, "OperatesChristmasNewYear");
            Agreement                = DCC_GetColumn.String    (dataRow, "Agreement");
            Q_Agreement              = DCC_GetColumn.Integer32 (dataRow, "Q_Agreement");
            Variation                = DCC_GetColumn.Integer32 (dataRow, "Variation");
            NetAmount                = DCC_GetColumn.Integer32 (dataRow, "NetAmount");
            Notes                    = DCC_GetColumn.String    (dataRow, "Notes");
            FirstStop                = DCC_GetColumn.Bit       (dataRow, "FirstStop");
            DispSpecification        = DCC_GetColumn.String    (dataRow, "DispSpecification");
            VariationNumber          = DCC_GetColumn.String    (dataRow, "VariationNumber");
            SortName                 = DCC_GetColumn.String    (dataRow, "SortName");

            return true;
        }

        public int Save(DCC_Database db)
        {
            return (IsNew ? Insert(db) : Update(db));
        }

        int Insert(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("iContractPayment");
            dbf.AddTable("ContractPayment");
            SetCommonParameters(dbf);
            dbf.AddParameter("_PRIMARYKEY", DCC_DatabaseFrame.OUTINTEGER);
            int newKey = db.DataLayer.NonQueryPK(dbf);
            if (newKey > -1) K_ContractPayment = newKey;
            return newKey;
        }

        int Update(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uContractPayment");
            dbf.AddTable("ContractPayment");
            dbf.AddParameter("K_ContractPayment", K_ContractPayment);
            SetCommonParameters(dbf);
            db.DataLayer.NonQuery(dbf);
            return (dbf.OK ? K_ContractPayment : -1);
        }

        void SetCommonParameters(DCC_DatabaseFrame dbf)
        {
            dbf.AddParameter("MasterSpecification", MasterSpecification, 4);
            dbf.AddParameter("Specification", Specification, 4);
            dbf.AddParameter("ServiceNumber", ServiceNumber, 500);
            dbf.AddParameter("RouteStart", RouteStart, 500);
            dbf.AddParameter("RouteEnd", RouteEnd, 500);
            dbf.AddParameter("PricePerDay", PricePerDay);
            dbf.AddParameter("NumberOfDays", NumberOfDays);
            dbf.AddParameter("Amount", Amount);
            dbf.AddParameter("LicenceNo", LicenceNo, 500);
            dbf.AddParameter("DateStart", DateStart);
            dbf.AddParameter("DateEnd", DateEnd);
            dbf.AddParameter("OperatesMonday", OperatesMonday);
            dbf.AddParameter("OperatesTuesday", OperatesTuesday);
            dbf.AddParameter("OperatesWednesday", OperatesWednesday);
            dbf.AddParameter("OperatesThursday", OperatesThursday);
            dbf.AddParameter("OperatesFriday", OperatesFriday);
            dbf.AddParameter("OperatesSaturday", OperatesSaturday);
            dbf.AddParameter("OperatesSunday", OperatesSunday);
            dbf.AddParameter("OperatesBankHolidays", OperatesBankHolidays);
            dbf.AddParameter("OperatesSchoolTerm", OperatesSchoolTerm);
            dbf.AddParameter("OperatesNonSchoolTerm", OperatesNonSchoolTerm);
            dbf.AddParameter("OperatesChristmasNewYear", OperatesChristmasNewYear);
            dbf.AddParameter("Agreement", Agreement, 2);
            dbf.AddParameter("Q_Agreement", Q_Agreement);
            dbf.AddParameter("Variation", Variation);
            dbf.AddParameter("NetAmount", NetAmount);
            dbf.AddParameter("Notes", Notes, 500);
            dbf.AddParameter("FirstStop", FirstStop);
            dbf.AddParameter("DispSpecification", DispSpecification, 4);
            dbf.AddParameter("VariationNumber", VariationNumber, 2);
            dbf.AddParameter("SortName", SortName, 500);
        }

    }

    #endregion


    #region ContractPaymentList

    public class ContractPaymentList : IEnumerable<ContractPayment>
    {

        // Properties

        public List<object> All = new List<object>();



        // Utilities

        public int Count { get { return All.Count; } }
        public override string ToString() { return "ContractPaymentList (" + Count + " items)"; }



        // Constructors

        public ContractPaymentList(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sContractPaymentList");
            dbf.AddTable("ContractPayment");
            db.DataLayer.SelectData(dbf);
            if (dbf.hasTables)
            {
                foreach (DataRow dataRow in dbf.TableZeroRows) Add(dataRow);
            }
        }



        // Methods

        public void Add(DataRow dataRow)
        {
            Specification ContractPaymentForm = new Specification(dataRow);
            All.Add(ContractPaymentForm);
        }



        // Enumerator

        public IEnumerator<ContractPayment> GetEnumerator() { foreach (object o in All) yield return (ContractPayment)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }

    }

    #endregion


    #region ContractPaymentGrid

    public class ContractPaymentGrid : DCC_Grid
    {
        public ContractPaymentGrid(DCC_Database db)
        {
            ContractPaymentList items = new ContractPaymentList(db);
            TotalRows = items.All;
            Count = CountFiltered = TotalRows.Count;
        }
    }

    #endregion

}
