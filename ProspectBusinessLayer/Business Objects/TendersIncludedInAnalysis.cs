using DCC_Standard;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace ProspectBusinessLayer
{

    #region TendersIncludedInAnalysis

    public class TendersIncludedInAnalysis
    {

        // Database Fields

        int k_TendersIncludedInAnalysis = -1;
        string tenderReference;
        string licenceNo;
        int price;
        bool package;
        bool rejected;
        string note;
        string specificationReference;

        // Get/Set Properties

        public int K_TendersIncludedInAnalysis { get { return k_TendersIncludedInAnalysis; } set { k_TendersIncludedInAnalysis = value; } }
        public string TenderReference { get { return tenderReference; } set { tenderReference = value; } }
        public string LicenceNo { get { return licenceNo; } set { licenceNo = value; } }
        public int Price { get { return price; } set { price = value; } }
        public bool Package { get { return package; } set { package = value; } }
        public bool Rejected { get { return rejected; } set { rejected = value; } }
        public string Note { get { return note; } set { note = value; } }
        public string SpecificationReference { get { return specificationReference; } set { specificationReference = value; } }

        // Utilities

        public int PrimaryKey { get { return K_TendersIncludedInAnalysis; } set { K_TendersIncludedInAnalysis = value; } }
        public string Key { get { return K_TendersIncludedInAnalysis.ToString(); } }
        public bool IsNew { get { return K_TendersIncludedInAnalysis == -1; } }
        public override string ToString() { return TenderReference + ", " + K_TendersIncludedInAnalysis.ToString().PadLeft(3); }

        // Constructors

        public TendersIncludedInAnalysis()
        {
        }

        public TendersIncludedInAnalysis(DataRow dataRow)
        {
            GetColumns(dataRow);
        }

        public TendersIncludedInAnalysis(DCC_Database db, int pK_TendersIncludedInAnalysis = -999)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sTendersIncludedInAnalysis");
            dbf.AddTable("TendersIncludedInAnalysis");
            dbf.AddParameter("K_TendersIncludedInAnalysis", pK_TendersIncludedInAnalysis);
            db.DataLayer.SelectData(dbf);
            if (dbf.hasTables && dbf.TableZeroRows.Count == 1) GetColumns(dbf.RowZero);
        }

        // Methods

        public bool GetColumns(DataRow dataRow)
        {
            if (dataRow == null) return false;

            K_TendersIncludedInAnalysis = DCC_GetColumn.Integer32(dataRow, "K_TendersIncludedInAnalysis");
            TenderReference = DCC_GetColumn.String(dataRow, "TenderReference");
            LicenceNo = DCC_GetColumn.String(dataRow, "LicenceNo");
            Price = DCC_GetColumn.Integer32(dataRow, "Price");
            Package = DCC_GetColumn.Bit(dataRow, "Package");
            Rejected = DCC_GetColumn.Bit(dataRow, "Rejected");
            Note = DCC_GetColumn.String(dataRow, "Note");
            SpecificationReference = DCC_GetColumn.String(dataRow, "SpecificationReference");

            return true;
        }

        public int Save(DCC_Database dcc_Database)
        {
            return (IsNew ? Insert(dcc_Database) : Update(dcc_Database));
        }

        int Insert(DCC_Database dcc_Database)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("iTendersIncludedInAnalysis");
            dbf.AddTable("TendersIncludedInAnalysis");
            dbf.AddParameter("TenderReference", TenderReference, 5);
            dbf.AddParameter("LicenceNo", LicenceNo, 9);
            dbf.AddParameter("Price", Price);
            dbf.AddParameter("Package", Package);
            dbf.AddParameter("Rejected", Rejected);
            dbf.AddParameter("Note", Note, 500);
            dbf.AddParameter("SpecificationReference", SpecificationReference, 4);
            dbf.AddParameter("_PRIMARYKEY", DCC_DatabaseFrame.OUTINTEGER);
            int newKey = dcc_Database.DataLayer.NonQueryPK(dbf);
            if (newKey > -1) K_TendersIncludedInAnalysis = newKey;
            return newKey;
        }

        int Update(DCC_Database dcc_Database)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uTendersIncludedInAnalysis");
            dbf.AddTable("TendersIncludedInAnalysis");
            dbf.AddParameter("K_TendersIncludedInAnalysis", PrimaryKey);
            dbf.AddParameter("TenderReference", TenderReference, 5);
            dbf.AddParameter("LicenceNo", LicenceNo, 9);
            dbf.AddParameter("Price", Price);
            dbf.AddParameter("Package", Package);
            dbf.AddParameter("Rejected", Rejected);
            dbf.AddParameter("Note", Note, 500);
            dbf.AddParameter("SpecificationReference", SpecificationReference, 4);
            dcc_Database.DataLayer.NonQuery(dbf);
            return (dbf.OK ? K_TendersIncludedInAnalysis : -1);
        }

    }

    #endregion


    #region TendersIncludedInAnalysisList

    public class TendersIncludedInAnalysisList : IEnumerable<TendersIncludedInAnalysis>
    {

        // Properties

        public List<object> All = new List<object>();
        //public List<object> Selectable = new List<object>();



        // Utilities

        public int Count { get { return All.Count; } }
        public override string ToString() { return "TendersIncludedInAnalysisList (" + Count + " items)"; }



        // Constructors

        public TendersIncludedInAnalysisList(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sTendersIncludedInAnalysisList");
            dbf.AddTable("TendersIncludedInAnalysis");
            db.DataLayer.SelectData(dbf);
            if (dbf.hasTables)
            {
                foreach (DataRow dataRow in dbf.TableZeroRows) Add(dataRow);
            }
        }



        // Methods

        public void Add(DataRow dataRow)
        {
            TendersIncludedInAnalysis TendersIncludedInAnalysis = new TendersIncludedInAnalysis(dataRow);
            All.Add(TendersIncludedInAnalysis);
            //if (!TendersIncludedInAnalysis.TendersIncludedInAnalysisIsArchived) Selectable.Add(TendersIncludedInAnalysis);
        }



        // Enumerator

        public IEnumerator<TendersIncludedInAnalysis> GetEnumerator() { foreach (object o in All) yield return (TendersIncludedInAnalysis)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }

    }

    #endregion


    #region TendersIncludedInAnalysisGrid

    public class TendersIncludedInAnalysisGrid : DCC_Grid
    {
        public TendersIncludedInAnalysisGrid(DCC_Database db)
        {
            TendersIncludedInAnalysisList items = new TendersIncludedInAnalysisList(db);
            TotalRows = items.All;
            Count = CountFiltered = TotalRows.Count;
        }
    }

    #endregion

}
