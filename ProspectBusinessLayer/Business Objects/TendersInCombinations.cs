using DCC_Standard;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace ProspectBusinessLayer
{

    #region TendersInCombinations

    public class TendersInCombinations
    {

        // Database Fields

        int k_TendersInCombinations = -1;
        string combinationReference;
        string tenderReference;
        int price;
        bool package;

        // Get/Set Properties

        public int K_TendersInCombinations { get { return k_TendersInCombinations; } set { k_TendersInCombinations = value; } }
        public string CombinationReference { get { return combinationReference; } set { combinationReference = value; } }
        public string TenderReference { get { return tenderReference; } set { tenderReference = value; } }
        public int Price { get { return price; } set { price = value; } }
        public bool Package { get { return package; } set { package = value; } }

        // Utilities

        public int PrimaryKey { get { return K_TendersInCombinations; } set { K_TendersInCombinations = value; } }
        public string Key { get { return K_TendersInCombinations.ToString(); } }
        public bool IsNew { get { return K_TendersInCombinations == -1; } }
        public override string ToString() { return CombinationReference + ", " + K_TendersInCombinations.ToString().PadLeft(3); }

        // Constructors

        public TendersInCombinations()
        {
        }

        public TendersInCombinations(DataRow dataRow)
        {
            GetColumns(dataRow);
        }

        public TendersInCombinations(DCC_Database db, int pK_TendersInCombinations = -999)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sTendersInCombinations");
            dbf.AddTable("TendersInCombinations");
            dbf.AddParameter("K_TendersInCombinations", pK_TendersInCombinations);
            db.DataLayer.SelectData(dbf);
            if (dbf.hasTables && dbf.TableZeroRows.Count == 1) GetColumns(dbf.RowZero);
        }

        // Methods

        public bool GetColumns(DataRow dataRow)
        {
            if (dataRow == null) return false;

            K_TendersInCombinations = DCC_GetColumn.Integer32(dataRow, "K_TendersInCombinations");
            CombinationReference = DCC_GetColumn.String(dataRow, "CombinationReference");
            TenderReference = DCC_GetColumn.String(dataRow, "TenderReference");
            Price = DCC_GetColumn.Integer32(dataRow, "Price");
            Package = DCC_GetColumn.Bit(dataRow, "Package");

            return true;
        }

        public int Save(DCC_Database dcc_Database)
        {
            return (IsNew ? Insert(dcc_Database) : Update(dcc_Database));
        }

        int Insert(DCC_Database dcc_Database)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("iTendersInCombinations");
            dbf.AddTable("TendersInCombinations");
            dbf.AddParameter("CombinationReference", CombinationReference, 6);
            dbf.AddParameter("TenderReference", TenderReference, 5);
            dbf.AddParameter("Price", Price);
            dbf.AddParameter("Package", Package);

            dbf.AddParameter("_PRIMARYKEY", DCC_DatabaseFrame.OUTINTEGER);
            int newKey = dcc_Database.DataLayer.NonQueryPK(dbf);
            if (newKey > -1) K_TendersInCombinations = newKey;
            return newKey;
        }

        int Update(DCC_Database dcc_Database)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uTendersInCombinations");
            dbf.AddTable("TendersInCombinations");
            dbf.AddParameter("K_TendersInCombinations", PrimaryKey);
            dbf.AddParameter("CombinationReference", CombinationReference, 6);
            dbf.AddParameter("TenderReference", TenderReference, 5);
            dbf.AddParameter("Price", Price);
            dbf.AddParameter("Package", Package);
            dcc_Database.DataLayer.NonQuery(dbf);
            return (dbf.OK ? K_TendersInCombinations : -1);
        }

    }

    #endregion


    #region TendersInCombinationsList

    public class TendersInCombinationsList : IEnumerable<TendersInCombinations>
    {

        // Properties

        public List<object> All = new List<object>();
        //public List<object> Selectable = new List<object>();



        // Utilities

        public int Count { get { return All.Count; } }
        public override string ToString() { return "TendersInCombinationsList (" + Count + " items)"; }



        // Constructors

        public TendersInCombinationsList(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sTendersInCombinationsList");
            dbf.AddTable("TendersInCombinations");
            db.DataLayer.SelectData(dbf);
            if (dbf.hasTables)
            {
                foreach (DataRow dataRow in dbf.TableZeroRows) Add(dataRow);
            }
        }



        // Methods

        public void Add(DataRow dataRow)
        {
            TendersInCombinations TendersInCombinations = new TendersInCombinations(dataRow);
            All.Add(TendersInCombinations);
            //if (!TendersInCombinations.TendersInCombinationsIsArchived) Selectable.Add(TendersInCombinations);
        }



        // Enumerator

        public IEnumerator<TendersInCombinations> GetEnumerator() { foreach (object o in All) yield return (TendersInCombinations)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }

    }

    #endregion


    #region TendersInCombinationsGrid

    public class TendersInCombinationsGrid : DCC_Grid
    {
        public TendersInCombinationsGrid(DCC_Database db)
        {
            TendersInCombinationsList items = new TendersInCombinationsList(db);
            TotalRows = items.All;
            Count = CountFiltered = TotalRows.Count;
        }
    }

    #endregion

}
