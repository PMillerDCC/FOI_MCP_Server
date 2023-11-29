using DCC_Standard;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace ProspectBusinessLayer
{

    #region Restriction

    public class Restriction
    {

        // Properties

        public int    K_Restriction          { get; set; }
        public string Reference              { get; set; }
        public string RestrictionDescription { get; set; }
        public bool   IsArchived             { get; set; }



        // Utilities

        public int PrimaryKey { get { return K_Restriction; } set { K_Restriction = value; } }
        public string Key { get { return K_Restriction.ToString(); } }
        public bool IsNew { get { return K_Restriction == -1; } }
        public override string ToString() { return RestrictionDescription + ", " + K_Restriction.ToString().PadLeft(3); }



        // Constructors

        public Restriction()
        {
        }

        public Restriction(DataRow dataRow)
        {
            GetColumns(dataRow);
        }

        public Restriction(DCC_Database db, int pK_Restriction = -999)
        {
            if (pK_Restriction == -1) // create a new instance of Contractor and set defaults
            {
                K_Restriction = -1;
            }

            else
            {
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sRestriction");
                dbf.AddTable("Restriction");
                dbf.AddParameter("K_Restriction", pK_Restriction);
                db.DataLayer.SelectData(dbf);
                if (dbf.hasTables && dbf.TableZeroRows.Count == 1) GetColumns(dbf.RowZero);
            }
        }



        // Methods

        public bool GetColumns(DataRow dataRow)
        {
            if (dataRow == null) return false;

            K_Restriction          = DCC_GetColumn.Integer32 (dataRow, "K_Restriction");
            Reference              = DCC_GetColumn.String    (dataRow, "Reference");
            RestrictionDescription = DCC_GetColumn.String    (dataRow, "RestrictionDescription");
            IsArchived             = DCC_GetColumn.Bit       (dataRow, "IsArchived");

            return true;
        }

        public int Save(DCC_Database dcc_Database)
        {
            return (IsNew ? Insert(dcc_Database) : Update(dcc_Database));
        }

        int Insert(DCC_Database dcc_Database)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("iRestriction");
            dbf.AddTable("Restriction");
            dbf.AddParameter("Reference", Reference, 2);
            dbf.AddParameter("RestrictionDescription", RestrictionDescription, 500);
            dbf.AddParameter("_PRIMARYKEY", DCC_DatabaseFrame.OUTINTEGER);
            int newKey = dcc_Database.DataLayer.NonQueryPK(dbf);
            if (newKey > -1) K_Restriction = newKey;
            return newKey;
        }

        int Update(DCC_Database dcc_Database)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uRestriction");
            dbf.AddTable("Restriction");
            dbf.AddParameter("K_Restriction", PrimaryKey);
            dbf.AddParameter("Reference", Reference, 2);
            dbf.AddParameter("RestrictionDescription", RestrictionDescription, 500);
            dbf.AddParameter("IsArchived", IsArchived);
            dcc_Database.DataLayer.NonQuery(dbf);
            return (dbf.OK ? K_Restriction : -1);
        }

    }

    #endregion


    #region RestrictionList

    public class RestrictionList : IEnumerable<Restriction>
    {

        // Properties

        public List<object> All = new List<object>();



        // Utilities

        public int Count { get { return All.Count; } }
        public override string ToString() { return "RestrictionList (" + Count + " items)"; }



        // Constructors

        public RestrictionList(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sRestrictionList");
            dbf.AddTable("Restriction");
            db.DataLayer.SelectData(dbf);
            if (dbf.hasTables)
            {
                foreach (DataRow dataRow in dbf.TableZeroRows) Add(dataRow);
            }
        }



        // Methods

        public void Add(DataRow dataRow)
        {
            Restriction Restriction = new Restriction(dataRow);
            All.Add(Restriction);
        }



        // Enumerator

        public IEnumerator<Restriction> GetEnumerator() { foreach (object o in All) yield return (Restriction)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }

    }

    #endregion


    #region RestrictionGrid

    public class RestrictionGrid : DCC_Grid
    {
        public RestrictionGrid(DCC_Database db)
        {
            RestrictionList items = new RestrictionList(db);
            TotalRows = items.All;
            Count = CountFiltered = TotalRows.Count;
        }
    }

    #endregion

}
