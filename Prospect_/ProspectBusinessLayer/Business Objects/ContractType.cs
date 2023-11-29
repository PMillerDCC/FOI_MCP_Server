using DCC_Standard;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace ProspectBusinessLayer
{

    #region ContractType

    public class ContractType
    {

        // Properties

        public int    K_ContractType          { get; set; }
        public string Reference               { get; set; }
        public string ContractTypeDescription { get; set; }
        public bool   IsArchived              { get; set; }



        // Utilities

        public int PrimaryKey { get { return K_ContractType; } set { K_ContractType = value; } }
        public string Key { get { return K_ContractType.ToString(); } }
        public bool IsNew { get { return K_ContractType == -1; } }
        public override string ToString() { return ContractTypeDescription + ", " + K_ContractType.ToString().PadLeft(3); }



        // Constructors

        public ContractType()
        {
        }

        public ContractType(DataRow dataRow)
        {
            GetColumns(dataRow);
        }

        public ContractType(DCC_Database db, int pK_ContractType = -999)
        {
            if (pK_ContractType == -1) // create a new instance of Contractor and set defaults
            {
                K_ContractType = -1;
            }

            else
            {
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sContractType");
                dbf.AddTable("ContractType");
                dbf.AddParameter("K_ContractType", pK_ContractType);
                db.DataLayer.SelectData(dbf);
                if (dbf.hasTables && dbf.TableZeroRows.Count == 1) GetColumns(dbf.RowZero);
            }
        }



        // Methods

        public bool GetColumns(DataRow dataRow)
        {
            if (dataRow == null) return false;

            K_ContractType          = DCC_GetColumn.Integer32(dataRow, "K_ContractType");
            Reference               = DCC_GetColumn.String   (dataRow, "Reference");
            ContractTypeDescription = DCC_GetColumn.String   (dataRow, "ContractTypeDescription");
            IsArchived              = DCC_GetColumn.Bit      (dataRow, "IsArchived");

            return true;
        }

        public int Save(DCC_Database dcc_Database)
        {
            return (IsNew ? Insert(dcc_Database) : Update(dcc_Database));
        }

        int Insert(DCC_Database dcc_Database)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("iContractType");
            dbf.AddTable("ContractType");
            dbf.AddParameter("Reference", Reference, 2);
            dbf.AddParameter("ContractTypeDescription", ContractTypeDescription, 500);
            dbf.AddParameter("_PRIMARYKEY", DCC_DatabaseFrame.OUTINTEGER);
            int newKey = dcc_Database.DataLayer.NonQueryPK(dbf);
            if (newKey > -1) K_ContractType = newKey;
            return newKey;
        }

        int Update(DCC_Database dcc_Database)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uContractType");
            dbf.AddTable("ContractType");
            dbf.AddParameter("K_ContractType", PrimaryKey);
            dbf.AddParameter("Reference", Reference, 2);
            dbf.AddParameter("ContractTypeDescription", ContractTypeDescription, 500);
            dbf.AddParameter("IsArchived", IsArchived);
            dcc_Database.DataLayer.NonQuery(dbf);
            return (dbf.OK ? K_ContractType : -1);
        }

    }

    #endregion


    #region ContractTypeList

    public class ContractTypeList : IEnumerable<ContractType>
    {

        // Properties

        public List<object> All = new List<object>();
        public List<object> Selectable = new List<object>();



        // Utilities

        public int Count { get { return All.Count; } }
        public override string ToString() { return "ContractTypeList (" + Count + " items)"; }



        // Constructors

        public ContractTypeList(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sContractTypeList");
            dbf.AddTable("ContractType");
            db.DataLayer.SelectData(dbf);
            if (dbf.hasTables)
            {
                foreach (DataRow dataRow in dbf.TableZeroRows) Add(dataRow);
            }
        }



        // Methods

        public void Add(DataRow dataRow)
        {
            ContractType ContractType = new ContractType(dataRow);
            All.Add(ContractType);
            if (!ContractType.IsArchived) Selectable.Add(ContractType);
        }



        // Enumerator

        public IEnumerator<ContractType> GetEnumerator() { foreach (object o in All) yield return (ContractType)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }

    }

    #endregion


    #region ContractTypeGrid

    public class ContractTypeGrid : DCC_Grid
    {
        DCC_Database database;

        public ContractTypeGrid(DCC_Database db)
        {
            database = db;
            InlineValidate = Validate;
            InlineAdd = AddRow;
            InlineEdit = EditRow;
            ContractTypeList siteList = new ContractTypeList(database);
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
            ContractType item = new ContractType();
            foreach (string key in AddedRow.Keys)
            {
                string value = AddedRow[key];
                switch (key)
                {
                    case "A1":
                        {
                            item.ContractTypeDescription = value;
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
            ContractType item = new ContractType(database, primaryKey);
            foreach (string key in EditedRow.Keys)
            {
                string value = EditedRow[key];
                switch (key)
                {
                    case "A1":
                        {
                            item.ContractTypeDescription = value;
                            break;
                        }
                }
            }
            item.IsArchived = EditedRow.ContainsKey("A2");
            if (item.Save(database) < 1) return "Failed to save " + ItemNameSingular;

            ContractTypeList items = new ContractTypeList(database);
            Count = CountFiltered = items.Count;
            TotalRows = items.All;

            return "";
        }

    }

    #endregion

}
