using DCC_Standard;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace ProspectBusinessLayer
{

    #region TenderAnalysisResult

    public class TenderAnalysisResult
    {

        // Properties

        public int    K_TenderAnalysisResult { get; set; }
        public string SpecificationReference { get; set; }
        public string TenderReference        { get; set; }
        public string LicenceNumber          { get; set; }
        public int    Price                  { get; set; }
        public bool   Package                { get; set; }
        public string SingleTender           { get; set; }
        public int    SinglePrice            { get; set; }
        public bool   SingleOnly             { get; set; }
        public bool   NoSingle               { get; set; }
        public string PriceSpecification     { get; set; }
        public bool   NoPackage              { get; set; }
        public string PackageSpecification   { get; set; }



        // Utilities

        public int PrimaryKey { get { return K_TenderAnalysisResult; } set { K_TenderAnalysisResult = value; } }
        public string Key { get { return K_TenderAnalysisResult.ToString(); } }
        public bool IsNew { get { return K_TenderAnalysisResult == -1; } }
        public override string ToString() { return SpecificationReference + ", " + K_TenderAnalysisResult.ToString().PadLeft(3); }



        // Constructors

        public TenderAnalysisResult()
        {
        }

        public TenderAnalysisResult(DataRow dataRow)
        {
            GetColumns(dataRow);
        }

        public TenderAnalysisResult(DCC_Database db, int pK_TenderAnalysisResult = -999)
        {
            if (pK_TenderAnalysisResult < 1)
                K_TenderAnalysisResult = -1;

            else
            {
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sTenderAnalysisResult");
                dbf.AddTable("TenderAnalysisResult");
                dbf.AddParameter("K_TenderAnalysisResult", pK_TenderAnalysisResult);
                db.DataLayer.SelectData(dbf);
                if (dbf.hasTables && dbf.TableZeroRows.Count == 1) GetColumns(dbf.RowZero);
            }
        }



        // Methods

        public bool GetColumns(DataRow dataRow)
        {
            if (dataRow == null) return false;

            K_TenderAnalysisResult = DCC_GetColumn.Integer32 (dataRow, "K_TenderAnalysisResult");
            SpecificationReference = DCC_GetColumn.String    (dataRow, "SpecificationReference");
            TenderReference        = DCC_GetColumn.String    (dataRow, "TenderReference");
            LicenceNumber          = DCC_GetColumn.String    (dataRow, "LicenceNumber");
            Price                  = DCC_GetColumn.Integer32 (dataRow, "Price");
            Package                = DCC_GetColumn.Bit       (dataRow, "Package");
            SingleTender           = DCC_GetColumn.String    (dataRow, "SingleTender");
            SinglePrice            = DCC_GetColumn.Integer32 (dataRow, "SinglePrice");
            SingleOnly             = DCC_GetColumn.Bit       (dataRow, "SingleOnly");
            NoSingle               = DCC_GetColumn.Bit       (dataRow, "NoSingle");
            PriceSpecification     = DCC_GetColumn.String    (dataRow, "PriceSpecification");
            NoPackage              = DCC_GetColumn.Bit       (dataRow, "NoPackage");
            PackageSpecification   = DCC_GetColumn.String    (dataRow, "PackageSpecification");

            return true;
        }

        public int Save(DCC_Database dcc_Database)
        {
            return (IsNew ? Insert(dcc_Database) : Update(dcc_Database));
        }

        int Insert(DCC_Database dcc_Database)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("iTenderAnalysisResult");
            dbf.AddTable("TenderAnalysisResult");
            dbf.AddParameter("SpecificationReference", SpecificationReference, 4);
            dbf.AddParameter("TenderReference", TenderReference, 5);
            dbf.AddParameter("LicenceNumber", LicenceNumber, 9);
            dbf.AddParameter("Price", Price);
            dbf.AddParameter("Package", Package);
            dbf.AddParameter("SingleTender", SingleTender, 5);
            dbf.AddParameter("SinglePrice", SinglePrice);
            dbf.AddParameter("SingleOnly", SingleOnly);
            dbf.AddParameter("NoSingle", NoSingle);
            dbf.AddParameter("PriceSpecification", PriceSpecification, 4);
            dbf.AddParameter("NoPackage", NoPackage);
            dbf.AddParameter("PackageSpecification", PackageSpecification, 4);
            dbf.AddParameter("_PRIMARYKEY", DCC_DatabaseFrame.OUTINTEGER);
            int newKey = dcc_Database.DataLayer.NonQueryPK(dbf);
            if (newKey > -1) K_TenderAnalysisResult = newKey;
            return newKey;
        }

        int Update(DCC_Database dcc_Database)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uTenderAnalysisResult");
            dbf.AddTable("TenderAnalysisResult");
            dbf.AddParameter("K_TenderAnalysisResult", PrimaryKey);
            dbf.AddParameter("SpecificationReference", SpecificationReference, 4);
            dbf.AddParameter("TenderReference", TenderReference, 5);
            dbf.AddParameter("LicenceNumber", LicenceNumber, 9);
            dbf.AddParameter("Price", Price);
            dbf.AddParameter("Package", Package);
            dbf.AddParameter("SingleTender", SingleTender, 5);
            dbf.AddParameter("SinglePrice", SinglePrice);
            dbf.AddParameter("SingleOnly", SingleOnly);
            dbf.AddParameter("NoSingle", NoSingle);
            dbf.AddParameter("PriceSpecification", PriceSpecification, 4);
            dbf.AddParameter("NoPackage", NoPackage);
            dbf.AddParameter("PackageSpecification", PackageSpecification, 4);
            dcc_Database.DataLayer.NonQuery(dbf);
            return (dbf.OK ? K_TenderAnalysisResult : -1);
        }

    }

    #endregion


    #region TenderAnalysisResultList

    public class TenderAnalysisResultList : IEnumerable<TenderAnalysisResult>
    {

        // Properties

        public List<object> All = new List<object>();
        //public List<object> Selectable = new List<object>();



        // Utilities

        public int Count { get { return All.Count; } }
        public override string ToString() { return "TenderAnalysisResultList (" + Count + " items)"; }



        // Constructors

        public TenderAnalysisResultList(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sTenderAnalysisResultList");
            dbf.AddTable("TenderAnalysisResult");
            db.DataLayer.SelectData(dbf);
            if (dbf.hasTables)
            {
                foreach (DataRow dataRow in dbf.TableZeroRows) Add(dataRow);
            }
        }



        // Methods

        public void Add(DataRow dataRow)
        {
            TenderAnalysisResult TenderAnalysisResult = new TenderAnalysisResult(dataRow);
            All.Add(TenderAnalysisResult);
            //if (!TenderAnalysisResult.TenderAnalysisResultIsArchived) Selectable.Add(TenderAnalysisResult);
        }



        // Enumerator

        public IEnumerator<TenderAnalysisResult> GetEnumerator() { foreach (object o in All) yield return (TenderAnalysisResult)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }

    }

    #endregion


    #region TenderAnalysisResultGrid

    public class TenderAnalysisResultGrid : DCC_Grid
    {
        public TenderAnalysisResultGrid(DCC_Database db)
        {
            TenderAnalysisResultList items = new TenderAnalysisResultList(db);
            TotalRows = items.All;
            Count = CountFiltered = TotalRows.Count;
        }
    }

    #endregion

}
