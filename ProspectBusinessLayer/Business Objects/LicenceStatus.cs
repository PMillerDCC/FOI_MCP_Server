using DCC_Standard;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace ProspectBusinessLayer
{

    #region LicenceStatus

    public class LicenceStatus
    {

        // Properties

        public int    K_LicenceStatus          { get; set; }
        public string Reference                { get; set; }
        public string LicenceStatusDescription { get; set; }
        public bool   IsArchived               { get; set; }



        // Utilities

        public int PrimaryKey { get { return K_LicenceStatus; } set { K_LicenceStatus = value; } }
        public string Key { get { return K_LicenceStatus.ToString(); } }
        public bool IsNew { get { return K_LicenceStatus == -1; } }
        public override string ToString() { return LicenceStatusDescription + ", " + K_LicenceStatus.ToString().PadLeft(3); }



        // Constructors

        public LicenceStatus()
        {
        }

        public LicenceStatus(DataRow dataRow)
        {
            GetColumns(dataRow);
        }

        public LicenceStatus(DCC_Database db, int pK_LicenceStatus = -999)
        {
            if (pK_LicenceStatus == -1) // create a new instance of Contractor and set defaults
            {
                K_LicenceStatus = -1;
            }

            else
            {
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sLicenceStatus");
                dbf.AddTable("LicenceStatus");
                dbf.AddParameter("K_LicenceStatus", pK_LicenceStatus);
                db.DataLayer.SelectData(dbf);
                if (dbf.hasTables && dbf.TableZeroRows.Count == 1) GetColumns(dbf.RowZero);
            }
        }



        // Methods

        public bool GetColumns(DataRow dataRow)
        {
            if (dataRow == null) return false;

            K_LicenceStatus          = DCC_GetColumn.Integer32 (dataRow, "K_LicenceStatus");
            Reference                = DCC_GetColumn.String    (dataRow, "Reference");
            LicenceStatusDescription = DCC_GetColumn.String    (dataRow, "LicenceStatusDescription");
            IsArchived               = DCC_GetColumn.Bit       (dataRow, "IsArchived");

            return true;
        }

        public int Save(DCC_Database dcc_Database)
        {
            return (IsNew ? Insert(dcc_Database) : Update(dcc_Database));
        }

        int Insert(DCC_Database dcc_Database)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("iLicenceStatus");
            dbf.AddTable("LicenceStatus");
            dbf.AddParameter("Reference", Reference, 2);
            dbf.AddParameter("LicenceStatusDescription", LicenceStatusDescription, 500);
            dbf.AddParameter("_PRIMARYKEY", DCC_DatabaseFrame.OUTINTEGER);
            int newKey = dcc_Database.DataLayer.NonQueryPK(dbf);
            if (newKey > -1) K_LicenceStatus = newKey;
            return newKey;
        }

        int Update(DCC_Database dcc_Database)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uLicenceStatus");
            dbf.AddTable("LicenceStatus");
            dbf.AddParameter("K_LicenceStatus", PrimaryKey);
            dbf.AddParameter("Reference", Reference, 2);
            dbf.AddParameter("LicenceStatusDescription", LicenceStatusDescription, 500);
            dbf.AddParameter("IsArchived", IsArchived);
            dcc_Database.DataLayer.NonQuery(dbf);
            return (dbf.OK ? K_LicenceStatus : -1);
        }

    }

    #endregion


    #region LicenceStatusList

    public class LicenceStatusList : IEnumerable<LicenceStatus>
    {

        // Properties

        public List<object> All = new List<object>();
        public List<object> Selectable = new List<object>();



        // Utilities

        public int Count { get { return All.Count; } }
        public override string ToString() { return "LicenceStatusList (" + Count + " items)"; }



        // Constructors

        public LicenceStatusList(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sLicenceStatusList");
            dbf.AddTable("LicenceStatus");
            db.DataLayer.SelectData(dbf);
            if (dbf.hasTables)
            {
                foreach (DataRow dataRow in dbf.TableZeroRows) Add(dataRow);
            }
        }



        // Methods

        public void Add(DataRow dataRow)
        {
            LicenceStatus LicenceStatus = new LicenceStatus(dataRow);
            All.Add(LicenceStatus);
            if (!LicenceStatus.IsArchived) Selectable.Add(LicenceStatus);
        }



        // Enumerator

        public IEnumerator<LicenceStatus> GetEnumerator() { foreach (object o in All) yield return (LicenceStatus)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }

    }

    #endregion


    #region LicenceStatusGrid

    public class LicenceStatusGrid : DCC_Grid
    {
        public LicenceStatusGrid(DCC_Database db)
        {
            LicenceStatusList items = new LicenceStatusList(db);
            TotalRows = items.All;
            Count = CountFiltered = TotalRows.Count;
        }
    }

    #endregion

}
