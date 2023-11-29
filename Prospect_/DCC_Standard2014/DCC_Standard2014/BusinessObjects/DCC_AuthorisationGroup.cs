using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace DCC_Standard
{

	#region DCC_AuthorisationGroup

    [Serializable]
	public class DCC_AuthorisationGroup
    {

		// Get/Set Properties

        public int    K_AuthorisationGroup         { get; set; }
		public string AuthorisationGroupName       { get; set; }
		public bool   AuthorisationGroupIsObsolete { get; set; }



        // Utilities

        public int             PrimaryKey { get { return K_AuthorisationGroup; } set { K_AuthorisationGroup = value; } }
        public string          Key        { get { return PrimaryKey.ToString(); } }
        public bool            IsNew      { get { return PrimaryKey == -1; } }
        public override string ToString() { return AuthorisationGroupName + " " + PrimaryKey.ToString().PadLeft(3); }



        // Constructors

        public DCC_AuthorisationGroup()
        {
            K_AuthorisationGroup = -1;
        }

        public DCC_AuthorisationGroup(DataRow dataRow)
        {
            GetColumns(dataRow);
        }

        public DCC_AuthorisationGroup(DCC_Database db, int pK_AuthorisationGroup)
        {
			DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sAuthorisationGroup");
            dbf.AddTable("AuthorisationGroup");
			dbf.AddParameter("K_AuthorisationGroup", pK_AuthorisationGroup);
			db.DataLayer.SelectData(dbf);
			if (dbf.hasTables && dbf.TableZeroRows.Count == 1) GetColumns(dbf.RowZero);
        }



        // Methods

        public bool GetColumns(DataRow dataRow)
        {
            if (dataRow == null) return false;

			K_AuthorisationGroup         = DCC_GetColumn.Integer32 (dataRow, "K_AuthorisationGroup");
            AuthorisationGroupName       = DCC_GetColumn.String    (dataRow, "AuthorisationGroupName");
            AuthorisationGroupIsObsolete = DCC_GetColumn.Bit       (dataRow, "AuthorisationGroupIsObsolete");

			return true;
        }

        public int Save(DCC_Database db)
		{
			return (IsNew ? Insert(db) : Update(db));
		}

		int Insert(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("iAuthorisationGroup");
            dbf.AddTable("AuthorisationGroup");
            dbf.AddParameter("AuthorisationGroupName", AuthorisationGroupName, 500);
			dbf.AddParameter("_PRIMARYKEY", DCC_DatabaseFrame.OUTINTEGER);
            int newKey = db.DataLayer.NonQueryPK(dbf);
            if (newKey > -1) K_AuthorisationGroup = newKey;
            return newKey;
        }

        int Update(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uAuthorisationGroup");
            dbf.AddTable("AuthorisationGroup");
            dbf.AddParameter("K_AuthorisationGroup"        , K_AuthorisationGroup);
            dbf.AddParameter("AuthorisationGroupName"      , AuthorisationGroupName                 , 500);
            dbf.AddParameter("AuthorisationGroupIsObsolete", AuthorisationGroupIsObsolete);
            db.DataLayer.NonQuery(dbf);
            return (dbf.OK ? PrimaryKey : -1);
        }

		public int Delete(DCC_Database dcc_Database) { throw new NotImplementedException(); }

    }

    #endregion


    #region DCC_AuthorisationGroupList

    [Serializable]
    public class DCC_AuthorisationGroupList : IEnumerable<DCC_AuthorisationGroup>
    {

        // Properties

        public List<object> All = new List<object>();
        public List<object> Selectable = new List<object>();



        // Utilities

        public int Count { get { return All.Count; } }
        public override string ToString() { return "AuthorisationGroupList has " + Count + " items"; }



        // Constructors

		public DCC_AuthorisationGroupList()
		{
		}

        public DCC_AuthorisationGroupList(DCC_Database dcc_Database)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sAuthorisationGroupList");
            dbf.AddTable("AuthorisationGroup");
            dcc_Database.DataLayer.SelectData(dbf);
            if (dbf.hasTables)
            {
                foreach (DataRow dataRow in dbf.TableZeroRows) Add(dataRow);
            }
        }



        // Methods

        public void Add(DataRow dataRow)
        {
            DCC_AuthorisationGroup authorisationGroup = new DCC_AuthorisationGroup(dataRow);
            All.Add(authorisationGroup);
            if (!authorisationGroup.AuthorisationGroupIsObsolete) Selectable.Add(authorisationGroup);
        }



        // Enumerator

        public IEnumerator<DCC_AuthorisationGroup> GetEnumerator() { foreach (object o in All) yield return (DCC_AuthorisationGroup)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }

    }

	#endregion


    #region DCC_AuthorisationGroupGrid

    public class DCC_AuthorisationGroupGrid : DCC_Grid
    {

		DCC_Database database;

        public DCC_AuthorisationGroupGrid(DCC_Database db)
        {
			database = db;
            InlineValidate = Validate;
            InlineAdd = AddRow;
            InlineEdit = EditRow;
            DCC_AuthorisationGroupList authorisationGroupList = new DCC_AuthorisationGroupList(database);
            TotalRows = authorisationGroupList.All;
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
            DCC_AuthorisationGroup item = new DCC_AuthorisationGroup();
            foreach (string key in AddedRow.Keys)
            {
                string value = AddedRow[key];
                switch (key)
                {
                    case "A1":
                    {
                        item.AuthorisationGroupName = value;
                        break;
                    }
                }
            }
            item.AuthorisationGroupIsObsolete = AddedRow.ContainsKey("A2");

            if (item.Save(database) == -1) return "Failed to add new " + ItemNameSingular;

            TotalRows.Add(item);
            Count++;

            return "";
        }

        private string EditRow(SortedList<string, string> EditedRow, int primaryKey)
        {
            DCC_AuthorisationGroup item = new DCC_AuthorisationGroup(database, primaryKey);
            foreach (string key in EditedRow.Keys)
            {
                string value = EditedRow[key];
                switch (key)
                {
                    case "A1":
                    {
                        item.AuthorisationGroupName = value;
                        break;
                    }
                }
            }
            item.AuthorisationGroupIsObsolete = EditedRow.ContainsKey("A2");
            if (item.Save(database) < 1) return "Failed to save " + ItemNameSingular;

            DCC_AuthorisationGroupList items = new DCC_AuthorisationGroupList(database);
            Count = CountFiltered = items.Count;
            TotalRows = items.All;

            return "";
        }

    }

    #endregion

}
