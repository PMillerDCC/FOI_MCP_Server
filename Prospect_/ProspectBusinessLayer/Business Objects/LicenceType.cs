using DCC_Standard;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace ProspectBusinessLayer
{

    #region LicenceType

    public class LicenceType
    {

        // Properties

        public int    K_LicenceType          { get; set; }
        public string Reference              { get; set; }
        public string LicenceTypeDescription { get; set; }
        public bool   IsArchived             { get; set; }



        // Utilities

        public int PrimaryKey { get { return K_LicenceType; } set { K_LicenceType = value; } }
        public string Key { get { return K_LicenceType.ToString(); } }
        public bool IsNew { get { return K_LicenceType == -1; } }
        public override string ToString() { return LicenceTypeDescription + ", " + K_LicenceType.ToString().PadLeft(3); }



        // Constructors

        public LicenceType()
        {
        }

        public LicenceType(DataRow dataRow)
        {
            GetColumns(dataRow);
        }

        public LicenceType(DCC_Database db, int pK_LicenceType = -999)
        {
            if (pK_LicenceType == -1) // create a new instance of Contractor and set defaults
            {
                K_LicenceType = -1;
            }

            else
            {
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sLicenceType");
                dbf.AddTable("LicenceType");
                dbf.AddParameter("K_LicenceType", pK_LicenceType);
                db.DataLayer.SelectData(dbf);
                if (dbf.hasTables && dbf.TableZeroRows.Count == 1) GetColumns(dbf.RowZero);
            }
        }



        // Methods

        public bool GetColumns(DataRow dataRow)
        {
            if (dataRow == null) return false;

            K_LicenceType          = DCC_GetColumn.Integer32 (dataRow, "K_LicenceType");
            Reference              = DCC_GetColumn.String    (dataRow, "Reference");
            LicenceTypeDescription = DCC_GetColumn.String    (dataRow, "LicenceTypeDescription");
            IsArchived             = DCC_GetColumn.Bit       (dataRow, "IsArchived");

            return true;
        }

        public int Save(DCC_Database dcc_Database)
        {
            return (IsNew ? Insert(dcc_Database) : Update(dcc_Database));
        }

        int Insert(DCC_Database dcc_Database)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("iLicenceType");
            dbf.AddTable("LicenceType");
            dbf.AddParameter("Reference", Reference, 2);
            dbf.AddParameter("LicenceTypeDescription", LicenceTypeDescription, 500);
            dbf.AddParameter("_PRIMARYKEY", DCC_DatabaseFrame.OUTINTEGER);
            int newKey = dcc_Database.DataLayer.NonQueryPK(dbf);
            if (newKey > -1) K_LicenceType = newKey;
            return newKey;
        }

        int Update(DCC_Database dcc_Database)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uLicenceType");
            dbf.AddTable("LicenceType");
            dbf.AddParameter("K_LicenceType", PrimaryKey);
            dbf.AddParameter("Reference", Reference, 2);
            dbf.AddParameter("LicenceTypeDescription", LicenceTypeDescription, 500);
            dbf.AddParameter("IsArchived", IsArchived);
            dcc_Database.DataLayer.NonQuery(dbf);
            return (dbf.OK ? K_LicenceType : -1);
        }

    }

    #endregion


    #region LicenceTypeList

    public class LicenceTypeList : IEnumerable<LicenceType>
    {

        // Properties

        public List<object> All = new List<object>();
        public List<object> Selectable = new List<object>();



        // Utilities

        public int Count { get { return All.Count; } }
        public override string ToString() { return "LicenceTypeList (" + Count + " items)"; }



        // Constructors

        public LicenceTypeList(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sLicenceTypeList");
            dbf.AddTable("LicenceType");
            db.DataLayer.SelectData(dbf);
            if (dbf.hasTables)
            {
                foreach (DataRow dataRow in dbf.TableZeroRows) Add(dataRow);
            }
        }



        // Methods

        public void Add(DataRow dataRow)
        {
            LicenceType LicenceType = new LicenceType(dataRow);
            All.Add(LicenceType);
            if (!LicenceType.IsArchived) Selectable.Add(LicenceType);
        }



        // Enumerator

        public IEnumerator<LicenceType> GetEnumerator() { foreach (object o in All) yield return (LicenceType)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }

    }

    #endregion


    #region LicenceTypeGrid

    public class LicenceTypeGrid : DCC_Grid
    {
        public LicenceTypeGrid(DCC_Database db)
        {
            LicenceTypeList items = new LicenceTypeList(db);
            TotalRows = items.All;
            Count = CountFiltered = TotalRows.Count;
        }
    }

    #endregion

}
