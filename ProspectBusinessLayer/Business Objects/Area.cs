using DCC_Standard;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace ProspectBusinessLayer
{

    #region Area

    public class Area
    {

        // Properties

        public int    K_Area          { get; set; }
        public string Reference       { get; set; }
        public string AreaDescription { get; set; }
        public bool   IsArchived      { get; set; }



        // Utilities

        public int PrimaryKey { get { return K_Area; } set { K_Area = value; } }
        public string Key { get { return K_Area.ToString(); } }
        public bool IsNew { get { return K_Area == -1; } }
        public override string ToString() { return AreaDescription + ", " + K_Area.ToString().PadLeft(3); }



        // Constructors

        public Area()
        {
        }

        public Area(DataRow dataRow)
        {
            GetColumns(dataRow);
        }

        public Area(DCC_Database db, int pK_Area = -999)
        {
            if (pK_Area == -1) // create a new instance of Contractor and set defaults
            {
                K_Area = -1;
            }

            else
            {
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sArea");
                dbf.AddTable("Area");
                dbf.AddParameter("K_Area", pK_Area);
                db.DataLayer.SelectData(dbf);
                if (dbf.hasTables && dbf.TableZeroRows.Count == 1) GetColumns(dbf.RowZero);
            }
        }



        // Methods

        public bool GetColumns(DataRow dataRow)
        {
            if (dataRow == null) return false;

            K_Area          = DCC_GetColumn.Integer32(dataRow, "K_Area");
            Reference       = DCC_GetColumn.String   (dataRow, "Reference");
            AreaDescription = DCC_GetColumn.String   (dataRow, "AreaDescription");
            IsArchived      = DCC_GetColumn.Bit      (dataRow, "IsArchived");

            return true;
        }

        public int Save(DCC_Database dcc_Database)
        {
            return (IsNew ? Insert(dcc_Database) : Update(dcc_Database));
        }

        int Insert(DCC_Database dcc_Database)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("iArea");
            dbf.AddTable("Area");
            dbf.AddParameter("Reference", Reference, 2);
            dbf.AddParameter("AreaDescription", AreaDescription, 500);
            dbf.AddParameter("_PRIMARYKEY", DCC_DatabaseFrame.OUTINTEGER);
            int newKey = dcc_Database.DataLayer.NonQueryPK(dbf);
            if (newKey > -1) K_Area = newKey;
            return newKey;
        }

        int Update(DCC_Database dcc_Database)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uArea");
            dbf.AddTable("Area");
            dbf.AddParameter("K_Area", PrimaryKey);
            dbf.AddParameter("Reference", Reference, 2);
            dbf.AddParameter("AreaDescription", AreaDescription, 500);
            dbf.AddParameter("IsArchived", IsArchived);
            dcc_Database.DataLayer.NonQuery(dbf);
            return (dbf.OK ? K_Area : -1);
        }

    }

    #endregion


    #region AreaList

    public class AreaList : IEnumerable<Area>
    {

        // Properties

        public List<object> All = new List<object>();
        public List<object> Selectable = new List<object>();



        // Utilities

        public int Count { get { return All.Count; } }
        public override string ToString() { return "AreaList (" + Count + " items)"; }



        // Constructors

        public AreaList(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sAreaList");
            dbf.AddTable("Area");
            db.DataLayer.SelectData(dbf);
            if (dbf.hasTables)
            {
                foreach (DataRow dataRow in dbf.TableZeroRows) Add(dataRow);
            }
        }



        // Methods

        public void Add(DataRow dataRow)
        {
            Area area = new Area(dataRow);
            All.Add(area);
            if (!area.IsArchived) Selectable.Add(area);
        }



        // Enumerator

        public IEnumerator<Area> GetEnumerator() { foreach (object o in All) yield return (Area)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }

    }

    #endregion


    #region AreaGrid

    public class AreaGrid : DCC_Grid
    {

        DCC_Database database;

        public AreaGrid(DCC_Database db)
        {
            database = db;
            InlineValidate = Validate;
            InlineAdd = AddRow;
            InlineEdit = EditRow;
            AreaList siteList = new AreaList(database);
            TotalRows = siteList.All;
            Count = CountFiltered = TotalRows.Count;
        }

        private bool Validate(SortedList<string, string> AddedRow, SortedList<string, string> Errors, int rowKey)
        {
            foreach (string key in AddedRow.Keys)
            {
                string value = AddedRow[key];
                switch (key)
                {
                    case "A1":
                        {
                            if (value.Length < 3) { Errors.Add(key, "Needs at least 3 characters"); break; }
                            if (value.Length > 50) { Errors.Add(key, "Cannot be more than 50 characters"); break; }
                            break;
                        }
                }
            }
            return (Errors.Count == 0);
        }

        private string AddRow(SortedList<string, string> AddedRow)
        {
            Area item = new Area();
            foreach (string key in AddedRow.Keys)
            {
                string value = AddedRow[key];
                switch (key)
                {
                    case "A1":
                        {
                            item.AreaDescription = value;
                            break;
                        }
                }
            }
            item.IsArchived = AddedRow.ContainsKey("A2");

            if (item.Save(database) == -1) return "Failed to add new " + ItemNameSingular;

            TotalRows.Add(item);
            Count++;

            return "";
        }

        private string EditRow(SortedList<string, string> EditedRow, int primaryKey)
        {
            Area item = new Area(database, primaryKey);
            foreach (string key in EditedRow.Keys)
            {
                string value = EditedRow[key];
                switch (key)
                {
                    case "A1":
                        {
                            item.AreaDescription = value;
                            break;
                        }
                }
            }
            item.IsArchived = EditedRow.ContainsKey("A2");
            if (item.Save(database) < 1) return "Failed to save " + ItemNameSingular;

            AreaList items = new AreaList(database);
            Count = CountFiltered = items.Count;
            TotalRows = items.All;

            return "";
        }

    }

    #endregion

}
