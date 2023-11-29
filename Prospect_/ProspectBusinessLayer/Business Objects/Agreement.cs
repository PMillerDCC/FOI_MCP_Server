using DCC_Standard;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace ProspectBusinessLayer
{

    #region Agreement

    public class Agreement
    {

        // Properties

        public int    K_Agreement          { get; set; }
        public string Reference            { get; set; }
        public string AgreementDescription { get; set; }
        public bool   IsArchived           { get; set; }



        // Utilities

        public int PrimaryKey { get { return K_Agreement; } set { K_Agreement = value; } }
        public string Key { get { return K_Agreement.ToString(); } }
        public bool IsNew { get { return K_Agreement == -1; } }
        public override string ToString() { return AgreementDescription + ", " + K_Agreement.ToString().PadLeft(3); }



        // Constructors

        public Agreement()
        {
        }

        public Agreement(DataRow dataRow)
        {
            GetColumns(dataRow);
        }

        public Agreement(DCC_Database db, int pK_Agreement = -999)
        {
            if (pK_Agreement < 1)
                K_Agreement = -1;

            else
            {
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sAgreement");
                dbf.AddTable("Agreement");
                dbf.AddParameter("K_Agreement", pK_Agreement);
                db.DataLayer.SelectData(dbf);
                if (dbf.hasTables && dbf.TableZeroRows.Count == 1) GetColumns(dbf.RowZero);
            }
        }



        // Methods

        public bool GetColumns(DataRow dataRow)
        {
            if (dataRow == null) return false;

            K_Agreement = DCC_GetColumn.Integer32(dataRow, "K_Agreement");
            Reference = DCC_GetColumn.String(dataRow, "Reference");
            AgreementDescription = DCC_GetColumn.String(dataRow, "AgreementDescription");
            IsArchived = DCC_GetColumn.Bit(dataRow, "IsArchived");

            return true;
        }

        public int Save(DCC_Database dcc_Database)
        {
            return (IsNew ? Insert(dcc_Database) : Update(dcc_Database));
        }

        int Insert(DCC_Database dcc_Database)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("iAgreement");
            dbf.AddTable("Agreement");
            dbf.AddParameter("Reference", Reference, 2);
            dbf.AddParameter("AgreementDescription", AgreementDescription, 500);
            dbf.AddParameter("_PRIMARYKEY", DCC_DatabaseFrame.OUTINTEGER);
            int newKey = dcc_Database.DataLayer.NonQueryPK(dbf);
            if (newKey > -1) K_Agreement = newKey;
            return newKey;
        }

        int Update(DCC_Database dcc_Database)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uAgreement");
            dbf.AddTable("Agreement");
            dbf.AddParameter("K_Agreement", PrimaryKey);
            dbf.AddParameter("Reference", Reference, 2);
            dbf.AddParameter("AgreementDescription", AgreementDescription, 500);
            dbf.AddParameter("IsArchived", IsArchived);
            dcc_Database.DataLayer.NonQuery(dbf);
            return (dbf.OK ? K_Agreement : -1);
        }

    }

    #endregion


    #region AgreementList

    public class AgreementList : IEnumerable<Agreement>
    {

        // Properties

        public List<object> All = new List<object>();
        public List<object> Selectable = new List<object>();



        // Utilities

        public int Count { get { return All.Count; } }
        public override string ToString() { return "AgreementList (" + Count + " items)"; }



        // Constructors

        public AgreementList(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sAgreementList");
            dbf.AddTable("Agreement");
            db.DataLayer.SelectData(dbf);
            if (dbf.hasTables)
            {
                foreach (DataRow dataRow in dbf.TableZeroRows) Add(dataRow);
            }
        }



        // Methods

        public void Add(DataRow dataRow)
        {
            Agreement Agreement = new Agreement(dataRow);
            All.Add(Agreement);
            if (!Agreement.IsArchived) Selectable.Add(Agreement);
        }



        // Enumerator

        public IEnumerator<Agreement> GetEnumerator() { foreach (object o in All) yield return (Agreement)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }

    }

    #endregion


    #region AgreementGrid

    public class AgreementGrid : DCC_Grid
    {
        public AgreementGrid(DCC_Database db)
        {
            AgreementList items = new AgreementList(db);
            TotalRows = items.All;
            Count = CountFiltered = TotalRows.Count;
        }
    }

    #endregion

}
