using DCC_Standard;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace ProspectBusinessLayer
{

    #region TenderSpecifiction

    public class TenderSpecification
    {

        // Properties

        public int       K_TenderSpecification       { get; set; }
        public int       Q_Tender                    { get; set; }
        public int       Q_Specification             { get; set; }
        public bool      IsRouteConforming           { get; set; }
        public string    RouteNotes                  { get; set; }
        public bool      IsTicketConforming          { get; set; }
        public string    TicketNotes                 { get; set; }
        public bool      IsVehicleConforming         { get; set; }
        public string    VehicleNotes                { get; set; }
        public string    Notes                       { get; set; }
                                                     
        public string    AgreementDescription        { get; set; }
        public int       ContractReference           { get; set; }
        public int       TenderReference             { get; set; }
        public int       ParentContractReference     { get; set; }
        public int       ChildContractReference      { get; set; }
        public string    ServiceNo                   { get; set; }
        public string    RouteStart                  { get; set; }
        public string    RouteEnd                    { get; set; }
        public DateTime? DateStart                   { get; set; }
        public DateTime? DateEnd                     { get; set; }
        public string    ContractTypeDescription     { get; set; }
        public string    ContractorName              { get; set; }
        public string    LicenceNo                   { get; set; }

        public string    ContractReferenceSort       { get { return ContractReference.ToString().PadLeft(5, '0'); } }
        public string    TenderReferenceSort         { get { return TenderReference.ToString().PadLeft(5, '0'); } }
        public string    ParentContractReferenceSort { get { return ParentContractReference.ToString().PadLeft(5, '0'); } }
        public string    ChildContractReferenceSort  { get { return ChildContractReference.ToString().PadLeft(5, '0'); } }
        public string    DateStartText               { get { return DCC_Formatting.NullDateTxt(DateStart, "dd MMM yyyy"); } }
        public string    DateStartSort               { get { return DCC_Formatting.NullDateTxt(DateStart, "yyyyMMddHHmm"); } }
        public string    DateEndText                 { get { return DCC_Formatting.NullDateTxt(DateEnd, "dd MMM yyyy"); } }
        public string    DateEndSort                 { get { return DCC_Formatting.NullDateTxt(DateEnd, "yyyyMMddHHmm"); } }
        public string    RouteText                   { get { return (string.IsNullOrEmpty(RouteStart) ? "- not set -" : RouteStart + " to " + RouteEnd); } }
        public string    DatesText                   { get { return (string.IsNullOrEmpty(DateStartText) ? "- not set -" : DateStartText + " to " + DateEndText); } }



        // Drop Down Lists

        public class ddlItem { public int Id { get; set; } public string Name { get; set; } }
        public List<ddlItem> SpecificationItems;
        public IEnumerable<SelectListItem> ddlSpecification { get { return new SelectList(SpecificationItems, "Id", "Name"); } }



        // Utilities

        public int PrimaryKey { get { return K_TenderSpecification; } set { K_TenderSpecification = value; } }
        public string Key { get { return PrimaryKey.ToString(); } }
        public bool IsNew { get { return PrimaryKey == -1; } }
        public override string ToString() { return PrimaryKey.ToString().PadLeft(3) + " - " + K_TenderSpecification.ToString(); }



        // Constructors

        public TenderSpecification()
        {
        }

        public TenderSpecification(DataRow dataRow)
        {
            GetColumns(dataRow);
        }

        public TenderSpecification(DCC_Database db, int pK_TenderSpecifiction = -999, int pQ_Tender = -999)
        {
            if (pK_TenderSpecifiction == -1) // create a new instance of TenderSpecifiction and set defaults
            {
                if (pQ_Tender > 0)
                {
                    K_TenderSpecification = -1;
                    Q_Tender = pQ_Tender;
                }
            }

            else if (pK_TenderSpecifiction > 0) // read a record from the database for the given K_TenderSpecifiction
            {
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sTenderSpecification");
                dbf.AddTable("TenderSpecification");
                dbf.AddParameter("K_TenderSpecification", pK_TenderSpecifiction);
                db.DataLayer.SelectData(dbf);
                if (dbf.hasTables && dbf.TableZeroRows.Count == 1) GetColumns(dbf.RowZero);
            }

            PopulateDropDownLists(db);
        }



        // Methods

        public bool GetColumns(DataRow dataRow)
        {
            if (dataRow == null) return false;

            K_TenderSpecification   = DCC_GetColumn.Integer32   (dataRow, "K_TenderSpecification");
            Q_Tender                = DCC_GetColumn.Integer32   (dataRow, "Q_Tender");
            Q_Specification         = DCC_GetColumn.Integer32   (dataRow, "Q_Specification");
            IsRouteConforming       = DCC_GetColumn.Bit         (dataRow, "IsRouteConforming");
            RouteNotes              = DCC_GetColumn.String      (dataRow, "RouteNotes");
            IsTicketConforming      = DCC_GetColumn.Bit         (dataRow, "IsTicketConforming");
            TicketNotes             = DCC_GetColumn.String      (dataRow, "TicketNotes");
            IsVehicleConforming     = DCC_GetColumn.Bit         (dataRow, "IsVehicleConforming");
            VehicleNotes            = DCC_GetColumn.String      (dataRow, "VehicleNotes");
            Notes                   = DCC_GetColumn.String      (dataRow, "Notes");

            if (DCC_GetColumn.Exists(dataRow, "AgreementDescription"))          AgreementDescription    = DCC_GetColumn.String      (dataRow, "AgreementDescription");
            if (DCC_GetColumn.Exists(dataRow, "ContractReference"))             ContractReference       = DCC_GetColumn.Integer32   (dataRow, "ContractReference");
            if (DCC_GetColumn.Exists(dataRow, "TenderReference"))               TenderReference         = DCC_GetColumn.Integer32   (dataRow, "TenderReference");
            if (DCC_GetColumn.Exists(dataRow, "ParentContractReference"))       ParentContractReference = DCC_GetColumn.Integer32   (dataRow, "ParentContractReference");
            if (DCC_GetColumn.Exists(dataRow, "ChildContractReference"))        ChildContractReference  = DCC_GetColumn.Integer32   (dataRow, "ChildContractReference");
            if (DCC_GetColumn.Exists(dataRow, "ServiceNo"))                     ServiceNo               = DCC_GetColumn.String      (dataRow, "ServiceNo");
            if (DCC_GetColumn.Exists(dataRow, "RouteStart"))                    RouteStart              = DCC_GetColumn.String      (dataRow, "RouteStart");
            if (DCC_GetColumn.Exists(dataRow, "RouteEnd"))                      RouteEnd                = DCC_GetColumn.String      (dataRow, "RouteEnd");
            if (DCC_GetColumn.Exists(dataRow, "DateStart"))                     DateStart               = DCC_GetColumn.DateTimeQ   (dataRow, "DateStart");
            if (DCC_GetColumn.Exists(dataRow, "DateEnd"))                       DateEnd                 = DCC_GetColumn.DateTimeQ   (dataRow, "DateEnd");
            if (DCC_GetColumn.Exists(dataRow, "ChildContractTypeDescription"))  ContractTypeDescription = DCC_GetColumn.String      (dataRow, "ChildContractTypeDescription");
            if (DCC_GetColumn.Exists(dataRow, "ContractorName"))                ContractorName          = DCC_GetColumn.String      (dataRow, "ContractorName");
            if (DCC_GetColumn.Exists(dataRow, "LicenceNo"))                     LicenceNo               = DCC_GetColumn.String      (dataRow, "LicenceNo");

            return true;
        }

        public void PopulateDropDownLists(DCC_Database db)
        {
            SpecificationItems = new List<ddlItem>();
            SpecificationList specificationList = new SpecificationList(db);
            foreach (Specification specification in specificationList.Selectable)
            {
                ddlItem ddlSpecification = new ddlItem();
                ddlSpecification.Id = specification.K_Specification;
                ddlSpecification.Name = specification.ContractReference.ToString() + " " + specification.SpecificationServiceNo;
                SpecificationItems.Add(ddlSpecification);
            }

            Tender tender = new Tender(db, Q_Tender);
            TenderReference = tender.TenderReference;

            Specification specification2 = new Specification(db, tender.Q_Specification);
            ParentContractReference = specification2.ContractReference;

            Specification specification3 = new Specification(db, Q_Specification);
            ChildContractReference = specification3.ContractReference;
            ServiceNo = specification3.SpecificationServiceNo;
            RouteStart = specification3.RouteStart;
            RouteEnd = specification3.RouteEnd;
            ContractTypeDescription = specification3.ContractTypeDescription;
            DateStart = specification3.DateStart;
            DateEnd = specification3.DateEnd;
        }

        public int Save(DCC_Database db)
        {
            return (IsNew ? Insert(db) : Update(db));
        }

        int Insert(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("iTenderSpecification");
            dbf.AddTable("TenderSpecification");
            dbf.AddParameter("Q_Tender", Q_Tender);
            dbf.AddParameter("Q_Specification", Q_Specification);
            dbf.AddParameter("_PRIMARYKEY", DCC_DatabaseFrame.OUTINTEGER);
            SetCommonParameters(dbf);
            int newKey = db.DataLayer.NonQueryPK(dbf);
            if (newKey > -1) K_TenderSpecification = newKey;
            return newKey;
        }

        int Update(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uTenderSpecification");
            dbf.AddTable("TenderSpecification");
            dbf.AddParameter("K_TenderSpecification", K_TenderSpecification);
            SetCommonParameters(dbf);
            db.DataLayer.NonQuery(dbf);
            return (dbf.OK ? K_TenderSpecification : -1);
        }

        void SetCommonParameters(DCC_DatabaseFrame dbf)
        {
            dbf.AddParameter("IsRouteConforming", IsRouteConforming);
            dbf.AddParameter("RouteNotes", RouteNotes, 500);
            dbf.AddParameter("IsTicketConforming", IsTicketConforming);
            dbf.AddParameter("TicketNotes", TicketNotes, 500);
            dbf.AddParameter("IsVehicleConforming", IsVehicleConforming);
            dbf.AddParameter("VehicleNotes", VehicleNotes, 500);
            dbf.AddParameter("Notes", Notes, 500);
        }

    }

    #endregion


    #region TenderSpecificationList

    public class TenderSpecificationList : IEnumerable<TenderSpecification>
    {

        // Properties

        public List<object> All = new List<object>();
        public string PropertiesToIgnore
        {
            get
            {
                return
"K_TenderSpecifiction|" +
"PrimaryKey|Key|IsNew";
            }
        }



        // Utilities

        public int Count { get { return All.Count; } }
        public override string ToString() { return "TenderSpecificationList (" + Count + " items)"; }



        // Constructors

        public TenderSpecificationList(DCC_Database db, int pQ_Tender = -999, DateTime? pStartDate = null, DateTime? pEndDate = null)
        {
            DCC_DatabaseFrame dbf;
            if ((pStartDate != null) && (pEndDate != null))
            {
                dbf = new DCC_DatabaseFrame("sTenderSpecificationListWithinDateRange");
                dbf.AddParameter("StartDate", pStartDate);
                dbf.AddParameter("EndDate", pEndDate);
            }
            else if (pQ_Tender > -999)
            {
                dbf = new DCC_DatabaseFrame("sTenderSpecificationListByTender");
                dbf.AddParameter("Q_Tender", pQ_Tender);
            }
            else
            {
                dbf = new DCC_DatabaseFrame("sTenderSpecificationList");
            }

            dbf.AddTable("TenderSpecification");
            db.DataLayer.SelectData(dbf);
            if (dbf.hasTables)
            {
                foreach (DataRow dataRow in dbf.TableZeroRows) Add(dataRow);
            }
        }



        // Methods

        public void Add(DataRow dataRow)
        {
            TenderSpecification TenderSpecifictionForm = new TenderSpecification(dataRow);
            All.Add(TenderSpecifictionForm);
        }



        // Enumerator

        public IEnumerator<TenderSpecification> GetEnumerator() { foreach (object o in All) yield return (TenderSpecification)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }

    }

    #endregion


    #region TenderSpecificationGrid

    public class TenderSpecificationGrid : DCC_Grid
    {
        public TenderSpecificationGrid(DCC_Database db, int pQ_Tender = -999)
        {
            TenderSpecificationList items = new TenderSpecificationList(db, pQ_Tender);
            TotalRows = items.All;
            Count = CountFiltered = TotalRows.Count;
        }
    }

    #endregion

}
