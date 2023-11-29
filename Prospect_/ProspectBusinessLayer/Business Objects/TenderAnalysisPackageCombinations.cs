using DCC_Standard;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace ProspectBusinessLayer.Business_Objects
{

    #region TenderAnalysisPackageCombinations

    public class TenderAnalysisPackageCombinations
    {

        // Properties

        public int    K_TenderAnalysisPackageCombinations { get; set; }
        public string Reference                           { get; set; }
        public int    Price                               { get; set; }
        public bool   Rejected                            { get; set; }
        public string Note                                { get; set; }



        // Utilities

        public int PrimaryKey { get { return K_TenderAnalysisPackageCombinations; } set { K_TenderAnalysisPackageCombinations = value; } }
        public string Key { get { return PrimaryKey.ToString(); } }
        public bool IsNew { get { return PrimaryKey == -1; } }
        public override string ToString() { return PrimaryKey.ToString().PadLeft(3) + " - " + Reference; }



        // Constructors

        public TenderAnalysisPackageCombinations()
        {
        }

        public TenderAnalysisPackageCombinations(DataRow dataRow)
        {
            GetColumns(dataRow);
        }

        public TenderAnalysisPackageCombinations(DCC_Database db, int pK_TenderAnalysisPackageCombinations = -999)
        {
            if (pK_TenderAnalysisPackageCombinations == -1) // create a new instance of TenderAnalysisPackageCombinations and set defaults
            {
                K_TenderAnalysisPackageCombinations = -1;
            }

            else if (pK_TenderAnalysisPackageCombinations > 0) // read a record from the database for the given K_TenderAnalysisPackageCombinations
            {
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sTenderAnalysisPackageCombinations");
                dbf.AddTable("TenderAnalysisPackageCombinations");
                dbf.AddParameter("K_TenderAnalysisPackageCombinations", pK_TenderAnalysisPackageCombinations);
                db.DataLayer.SelectData(dbf);
                if (dbf.hasTables && dbf.TableZeroRows.Count == 1) GetColumns(dbf.RowZero);
            }
        }



        // Methods

        public bool GetColumns(DataRow dataRow)
        {
            if (dataRow == null) return false;

            K_TenderAnalysisPackageCombinations = DCC_GetColumn.Integer32 (dataRow, "K_TenderAnalysisPackageCombinations");
            Reference                           = DCC_GetColumn.String    (dataRow, "Reference");
            Price                               = DCC_GetColumn.Integer32 (dataRow, "Price");
            Rejected                            = DCC_GetColumn.Bit       (dataRow, "Rejected");
            Note                                = DCC_GetColumn.String    (dataRow, "Note");

            return true;
        }

        public int Save(DCC_Database db)
        {
            return (IsNew ? Insert(db) : Update(db));
        }

        int Insert(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("iTenderAnalysisPackageCombinations");
            dbf.AddTable("TenderAnalysisPackageCombinations");
            SetCommonParameters(dbf);
            dbf.AddParameter("_PRIMARYKEY", DCC_DatabaseFrame.OUTINTEGER);
            int newKey = db.DataLayer.NonQueryPK(dbf);
            if (newKey > -1) K_TenderAnalysisPackageCombinations = newKey;
            return newKey;
        }

        int Update(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uTenderAnalysisPackageCombinations");
            dbf.AddTable("TenderAnalysisPackageCombinations");
            dbf.AddParameter("K_TenderAnalysisPackageCombinations", K_TenderAnalysisPackageCombinations);
            SetCommonParameters(dbf);
            db.DataLayer.NonQuery(dbf);
            return (dbf.OK ? K_TenderAnalysisPackageCombinations : -1);
        }

        void SetCommonParameters(DCC_DatabaseFrame dbf)
        {
            dbf.AddParameter("Reference", Reference, 6);
            dbf.AddParameter("Price", Price);
            dbf.AddParameter("Rejected", Rejected);
            dbf.AddParameter("Note", Note, 500);
        }

    }

    #endregion


    #region TenderAnalysisPackageCombinationsList

    public class TenderAnalysisPackageCombinationsList : IEnumerable<TenderAnalysisPackageCombinations>
    {

        // Properties

        public List<object> All = new List<object>();
        public string PropertiesToIgnore
        {
            get
            {
                return
"K_TenderAnalysisPackageCombinations|" +
"PrimaryKey|Key|IsNew";
            }
        }



        // Utilities

        public int Count { get { return All.Count; } }
        public override string ToString() { return "TenderAnalysisPackageCombinationsList (" + Count + " items)"; }



        // Constructors

        public TenderAnalysisPackageCombinationsList(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sTenderAnalysisPackageCombinationsList");
            dbf.AddTable("TenderAnalysisPackageCombinations");
            db.DataLayer.SelectData(dbf);
            if (dbf.hasTables)
            {
                foreach (DataRow dataRow in dbf.TableZeroRows) Add(dataRow);
            }
        }



        // Methods

        public void Add(DataRow dataRow)
        {
            TenderAnalysisPackageCombinations TenderAnalysisPackageCombinationsForm = new TenderAnalysisPackageCombinations(dataRow);
            All.Add(TenderAnalysisPackageCombinationsForm);
        }



        // Enumerator

        public IEnumerator<TenderAnalysisPackageCombinations> GetEnumerator() { foreach (object o in All) yield return (TenderAnalysisPackageCombinations)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }

    }

    #endregion


    #region TenderAnalysisPackageCombinationsGrid

    public class TenderAnalysisPackageCombinationsGrid : DCC_Grid
    {
        public TenderAnalysisPackageCombinationsGrid(DCC_Database db)
        {
            TenderAnalysisPackageCombinationsList items = new TenderAnalysisPackageCombinationsList(db);
            TotalRows = items.All;
            Count = CountFiltered = TotalRows.Count;
        }
    }

    #endregion

}
