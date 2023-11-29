using DCC_Standard;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace ProspectBusinessLayer
{

    #region Contractor

    public class Contractor
    {

        // Properties

        public int       K_Contractor                      { get; set; }
        public string    LicenceNo                         { get; set; }
        public int       NoOfVehicles                      { get; set; }
        public string    ContractorName                    { get; set; }
        public string    ContractorTradeName               { get; set; }
        public string    ContractorCode                    { get; set; }
        public string    Address1                          { get; set; }
        public string    Address2                          { get; set; }
        public string    Address3                          { get; set; }
        public string    Address4                          { get; set; }
        public string    Address5                          { get; set; }
        public string    Postcode                          { get; set; }
        public string    TransportManager                  { get; set; }
        public string    TelephoneNo                       { get; set; }
        public string    EmailAddress                      { get; set; }
        public int       Q_LicenceStatus                   { get; set; }
        public int       Q_LicenceType                     { get; set; }
        public DateTime? DateLicenceExpires                { get; set; }
        public bool      RequestsForTendersBusServices     { get; set; }
        public bool      RequestsForTendersTaxiBusServices { get; set; }
        public int       Q_Restriction                     { get; set; }
        public string    PublicEnquiries                   { get; set; }
        public string    OperatingCentre                   { get; set; }
        public string    GeneralNotes                      { get; set; }

        public string    LicenceStatusDescription          { get; set; }
        public string    LicenceTypeDescription            { get; set; }
        public string    RestrictionDescription            { get; set; }

        public string    DateLicenceExpiresText            { get { return DCC_Formatting.NullDateTxt(DateLicenceExpires, "dd MMM yyyy"); } }
        public string    DateLicenceExpiresSort            { get { return DCC_Formatting.NullDateTxt(DateLicenceExpires, "yyyyMMddHHmm"); } }
        public string    NameAndLicenceNo                  { get { return ContractorName + " - " + LicenceNo; } }



        // Utilities

        public int PrimaryKey { get { return K_Contractor; } set { K_Contractor = value; } }
        public string Key { get { return PrimaryKey.ToString(); } }
        public bool IsNew { get { return PrimaryKey == -1; } }
        public override string ToString() { return PrimaryKey.ToString().PadLeft(3) + " - " + ContractorName; }



        // Drop Down Lists

        public class ddlItem { public int Id { get; set; } public string Name { get; set; } }

        public List<ddlItem> LicenceTypeItems;
        public IEnumerable<SelectListItem> ddlLicenceType { get { return new SelectList(LicenceTypeItems, "Id", "Name"); } }

        public List<ddlItem> LicenceStatusItems;
        public IEnumerable<SelectListItem> ddlLicenceStatus { get { return new SelectList(LicenceStatusItems, "Id", "Name"); } }



        // Constructors

        public Contractor()
        {
        }

        public Contractor(DataRow dataRow)
        {
            GetColumns(dataRow);
        }

        public Contractor(DCC_Database db, int pK_Contractor = -999)
        {
            if (pK_Contractor == -1) // create a new instance of Contractor and set defaults
            {
                K_Contractor = -1;
            }

            else if (pK_Contractor > 0) // read a record from the database for the given K_Contractor
            {
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sContractor");
                dbf.AddTable("Contractor");
                dbf.AddParameter("K_Contractor", pK_Contractor);
                db.DataLayer.SelectData(dbf);
                if (dbf.hasTables && dbf.TableZeroRows.Count == 1) GetColumns(dbf.RowZero);
            }

            PopulateDropDownLists(db);
        }



        // Methods

        public bool GetColumns(DataRow dataRow)
        {
            if (dataRow == null) return false;

            K_Contractor                      = DCC_GetColumn.Integer32 (dataRow, "K_Contractor");
            LicenceNo                         = DCC_GetColumn.String    (dataRow, "LicenceNo");
            NoOfVehicles                      = DCC_GetColumn.Integer32 (dataRow, "NoOfVehicles");
            ContractorName                    = DCC_GetColumn.String    (dataRow, "ContractorName");
            ContractorTradeName               = DCC_GetColumn.String    (dataRow, "ContractorTradeName");
            ContractorCode                    = DCC_GetColumn.String    (dataRow, "ContractorCode");
            Address1                          = DCC_GetColumn.String    (dataRow, "Address1");
            Address2                          = DCC_GetColumn.String    (dataRow, "Address2");
            Address3                          = DCC_GetColumn.String    (dataRow, "Address3");
            Address4                          = DCC_GetColumn.String    (dataRow, "Address4");
            Address5                          = DCC_GetColumn.String    (dataRow, "Address5");
            Postcode                          = DCC_GetColumn.String    (dataRow, "Postcode");
            TransportManager                  = DCC_GetColumn.String    (dataRow, "TransportManager");
            TelephoneNo                       = DCC_GetColumn.String    (dataRow, "TelephoneNo");
            EmailAddress                      = DCC_GetColumn.String    (dataRow, "EmailAddress");
            Q_LicenceStatus                   = DCC_GetColumn.Integer32 (dataRow, "Q_LicenceStatus");
            Q_LicenceType                     = DCC_GetColumn.Integer32 (dataRow, "Q_LicenceType");
            DateLicenceExpires                = DCC_GetColumn.DateTimeQ (dataRow, "DateLicenceExpires");
            RequestsForTendersBusServices     = DCC_GetColumn.Bit       (dataRow, "RequestsForTendersBusServices");
            RequestsForTendersTaxiBusServices = DCC_GetColumn.Bit       (dataRow, "RequestsForTendersTaxiBusServices");
            Q_Restriction                     = DCC_GetColumn.Integer32 (dataRow, "Q_Restriction");
            PublicEnquiries                   = DCC_GetColumn.String    (dataRow, "PublicEnquiries");
            OperatingCentre                   = DCC_GetColumn.String    (dataRow, "OperatingCentre");
            GeneralNotes                      = DCC_GetColumn.String    (dataRow, "GeneralNotes");

            if (DCC_GetColumn.Exists(dataRow, "LicenceStatusDescription")) LicenceStatusDescription = DCC_GetColumn.String(dataRow, "LicenceStatusDescription");
            if (DCC_GetColumn.Exists(dataRow, "LicenceTypeDescription"))   LicenceTypeDescription   = DCC_GetColumn.String(dataRow, "LicenceTypeDescription");
            if (DCC_GetColumn.Exists(dataRow, "RestrictionDescription"))   RestrictionDescription   = DCC_GetColumn.String(dataRow, "RestrictionDescription");

            return true;
        }

        public void PopulateDropDownLists(DCC_Database db)
        {
            LicenceTypeItems = new List<ddlItem>();
            LicenceTypeList typeList = new LicenceTypeList(db);
            foreach (LicenceType licenceType in typeList.Selectable)
            {
                ddlItem ddlItem = new ddlItem();
                ddlItem.Id = licenceType.K_LicenceType;
                ddlItem.Name = licenceType.LicenceTypeDescription;
                LicenceTypeItems.Add(ddlItem);
            }

            LicenceStatusItems = new List<ddlItem>();
            LicenceStatusList statusList = new LicenceStatusList(db);
            foreach (LicenceStatus licenceStatus in statusList.Selectable)
            {
                ddlItem ddlItem = new ddlItem();
                ddlItem.Id = licenceStatus.K_LicenceStatus;
                ddlItem.Name = licenceStatus.LicenceStatusDescription;
                LicenceStatusItems.Add(ddlItem);
            }
        }

        public int Save(DCC_Database db)
        {
            return (IsNew ? Insert(db) : Update(db));
        }

        int Insert(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("iContractor");
            dbf.AddTable("Contractor");
            SetCommonParameters(dbf);
            dbf.AddParameter("_PRIMARYKEY", DCC_DatabaseFrame.OUTINTEGER);
            int newKey = db.DataLayer.NonQueryPK(dbf);
            if (newKey > -1) K_Contractor = newKey;
            return newKey;
        }

        int Update(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uContractor");
            dbf.AddTable("Contractor");
            dbf.AddParameter("K_Contractor", K_Contractor);
            SetCommonParameters(dbf);
            db.DataLayer.NonQuery(dbf);
            return (dbf.OK ? K_Contractor : -1);
        }

        void SetCommonParameters(DCC_DatabaseFrame dbf)
        {
            dbf.AddParameter("LicenceNo", LicenceNo, 9);
            dbf.AddParameter("NoOfVehicles", NoOfVehicles);
            dbf.AddParameter("ContractorName", ContractorName, 500);
            dbf.AddParameter("ContractorTradeName", ContractorTradeName, 500);
            dbf.AddParameter("ContractorCode", ContractorCode, 8);
            dbf.AddParameter("Address1", Address1, 500);
            dbf.AddParameter("Address2", Address2, 500);
            dbf.AddParameter("Address3", Address3, 500);
            dbf.AddParameter("Address4", Address4, 500);
            dbf.AddParameter("Address5", Address5, 500);
            dbf.AddParameter("Postcode", Postcode, 500);
            dbf.AddParameter("TransportManager", TransportManager, 500);
            dbf.AddParameter("TelephoneNo", TelephoneNo, 500);
            dbf.AddParameter("EmailAddress", EmailAddress, 500);
            dbf.AddParameter("Q_LicenceStatus", Q_LicenceStatus);
            dbf.AddParameter("Q_LicenceType", Q_LicenceType);
            dbf.AddParameter("DateLicenceExpires", DateLicenceExpires);
            dbf.AddParameter("RequestsForTendersBusServices", RequestsForTendersBusServices);
            dbf.AddParameter("RequestsForTendersTaxiBusServices", RequestsForTendersTaxiBusServices);
            dbf.AddParameter("Q_Restriction", Q_Restriction);
            dbf.AddParameter("PublicEnquiries", PublicEnquiries, 500);
            dbf.AddParameter("OperatingCentre", OperatingCentre, 500);
            dbf.AddParameter("GeneralNotes", GeneralNotes, 500);
        }

    }

    #endregion


    #region ContractorList

    public class ContractorList : IEnumerable<Contractor>
    {

        // Properties

        public List<object> All = new List<object>();
        public List<object> Selectable = new List<object>();
        public string PropertiesToIgnore
        {
            get
            {
                return
"K_Contractor|" +
"PrimaryKey|Key|IsNew";
            }
        }



        // Utilities

        public int Count { get { return All.Count; } }
        public override string ToString() { return "ContractorList (" + Count + " items)"; }



        // Constructors

        public ContractorList(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sContractorList");
            dbf.AddTable("Contractor");
            db.DataLayer.SelectData(dbf);
            if (dbf.hasTables)
            {
                foreach (DataRow dataRow in dbf.TableZeroRows) Add(dataRow);
            }
        }



        // Methods

        public void Add(DataRow dataRow)
        {
            Contractor contractor = new Contractor(dataRow);
            All.Add(contractor);
            if (contractor.Q_LicenceStatus == 2) Selectable.Add(contractor);
        }



        // Enumerator

        public IEnumerator<Contractor> GetEnumerator() { foreach (object o in All) yield return (Contractor)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }

    }

    #endregion


    #region ContractorGrid

    public class ContractorGrid : DCC_Grid
    {
        public ContractorGrid(DCC_Database db)
        {
            ContractorList items = new ContractorList(db);
            TotalRows = items.All;
            Count = CountFiltered = TotalRows.Count;
        }
    }

    #endregion

}
