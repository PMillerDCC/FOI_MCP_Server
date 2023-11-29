using DCC_Standard;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace ProspectBusinessLayer
{

    #region LocalAuthority

    public class LocalAuthority
    {

        // Properties

        public int    K_LocalAuthority          { get; set; }
        public string Reference                 { get; set; }
        public string LocalAuthorityDescription { get; set; }
        public bool   IsArchived                { get; set; }



        // Utilities

        public int PrimaryKey { get { return K_LocalAuthority; } set { K_LocalAuthority = value; } }
        public string Key { get { return K_LocalAuthority.ToString(); } }
        public bool IsNew { get { return K_LocalAuthority == -1; } }
        public override string ToString() { return LocalAuthorityDescription + ", " + K_LocalAuthority.ToString().PadLeft(3); }



        // Constructors

        public LocalAuthority()
        {
        }

        public LocalAuthority(DataRow dataRow)
        {
            GetColumns(dataRow);
        }

        public LocalAuthority(DCC_Database db, int pK_LocalAuthority = -999)
        {
            if (pK_LocalAuthority == -1) // create a new instance of Contractor and set defaults
            {
                K_LocalAuthority = -1;
            }

            else
            {
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sLocalAuthority");
                dbf.AddTable("LocalAuthority");
                dbf.AddParameter("K_LocalAuthority", pK_LocalAuthority);
                db.DataLayer.SelectData(dbf);
                if (dbf.hasTables && dbf.TableZeroRows.Count == 1) GetColumns(dbf.RowZero);
            }
        }



        // Methods

        public bool GetColumns(DataRow dataRow)
        {
            if (dataRow == null) return false;
             
            K_LocalAuthority          = DCC_GetColumn.Integer32 (dataRow, "K_LocalAuthority");
            Reference                 = DCC_GetColumn.String    (dataRow, "Reference");
            LocalAuthorityDescription = DCC_GetColumn.String    (dataRow, "LocalAuthorityDescription");
            IsArchived                = DCC_GetColumn.Bit       (dataRow, "IsArchived");

            return true;
        }

        public int Save(DCC_Database dcc_Database)
        {
            return (IsNew ? Insert(dcc_Database) : Update(dcc_Database));
        }

        int Insert(DCC_Database dcc_Database)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("iLocalAuthority");
            dbf.AddTable("LocalAuthority");
            dbf.AddParameter("Reference", Reference, 2);
            dbf.AddParameter("LocalAuthorityDescription", LocalAuthorityDescription, 500);
            dbf.AddParameter("_PRIMARYKEY", DCC_DatabaseFrame.OUTINTEGER);
            int newKey = dcc_Database.DataLayer.NonQueryPK(dbf);
            if (newKey > -1) K_LocalAuthority = newKey;
            return newKey;
        }

        int Update(DCC_Database dcc_Database)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uLocalAuthority");
            dbf.AddTable("LocalAuthority");
            dbf.AddParameter("K_LocalAuthority", PrimaryKey);
            dbf.AddParameter("Reference", Reference, 2);
            dbf.AddParameter("LocalAuthorityDescription", LocalAuthorityDescription, 500);
            dbf.AddParameter("IsArchived", IsArchived);
            dcc_Database.DataLayer.NonQuery(dbf);
            return (dbf.OK ? K_LocalAuthority : -1);
        }

    }

    #endregion


    #region LocalAuthorityList

    public class LocalAuthorityList : IEnumerable<LocalAuthority>
    {

        // Properties

        public List<object> All = new List<object>();
        public List<object> Selectable = new List<object>();



        // Utilities

        public int Count { get { return All.Count; } }
        public override string ToString() { return "LocalAuthorityList (" + Count + " items)"; }



        // Constructors

        public LocalAuthorityList(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sLocalAuthorityList");
            dbf.AddTable("LocalAuthority");
            db.DataLayer.SelectData(dbf);
            if (dbf.hasTables)
            {
                foreach (DataRow dataRow in dbf.TableZeroRows) Add(dataRow);
            }
        }



        // Methods

        public void Add(DataRow dataRow)
        {
            LocalAuthority LocalAuthority = new LocalAuthority(dataRow);
            All.Add(LocalAuthority);
            if (!LocalAuthority.IsArchived) Selectable.Add(LocalAuthority);
        }



        // Enumerator

        public IEnumerator<LocalAuthority> GetEnumerator() { foreach (object o in All) yield return (LocalAuthority)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }

    }

    #endregion


    #region LocalAuthorityGrid

    public class LocalAuthorityGrid : DCC_Grid
    {
        public LocalAuthorityGrid(DCC_Database db)
        {
            LocalAuthorityList items = new LocalAuthorityList(db);
            TotalRows = items.All;
            Count = CountFiltered = TotalRows.Count;
        }
    }

    #endregion

}
