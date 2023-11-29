using DCC_Standard;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace ProspectBusinessLayer
{

    #region ContractMileage

    public class ContractMileage
    {

        // Properties

        public int       K_ContractMileage { get; set; }
        public string    LicenceNo         { get; set; }
        public string    Contract          { get; set; }
        public DateTime? Date              { get; set; }
        public float     LostMiles         { get; set; }
        public string    ServiceNo         { get; set; }



        // Utilities

        public int PrimaryKey { get { return K_ContractMileage; } set { K_ContractMileage = value; } }
        public string Key { get { return K_ContractMileage.ToString(); } }
        public bool IsNew { get { return K_ContractMileage == -1; } }
        public override string ToString() { return PrimaryKey.ToString().PadLeft(3) + " - " + K_ContractMileage.ToString(); }



        // Constructors

        public ContractMileage()
        {
        }

        public ContractMileage(DataRow dataRow)
        {
            GetColumns(dataRow);
        }

        public ContractMileage(DCC_Database db, int pK_ContractMileage = -999)
        {
            if (pK_ContractMileage == -1) // create a new instance of Contractor and set defaults
            {
                K_ContractMileage = -1;
            }

            else
            {
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sContractMileage");
                dbf.AddTable("ContractMileage");
                dbf.AddParameter("K_ContractMileage", pK_ContractMileage);
                db.DataLayer.SelectData(dbf);
                if (dbf.hasTables && dbf.TableZeroRows.Count == 1) GetColumns(dbf.RowZero);
            }
        }



        // Methods

        public bool GetColumns(DataRow dataRow)
        {
            if (dataRow == null) return false;

            K_ContractMileage = DCC_GetColumn.Integer32(dataRow, "K_ContractMileage");
            LicenceNo         = DCC_GetColumn.String   (dataRow, "LicenceNo");
            Contract          = DCC_GetColumn.String   (dataRow, "Contract");
            Date              = DCC_GetColumn.DateTimeQ(dataRow, "Date");
            LostMiles         = DCC_GetColumn.Float    (dataRow, "LostMiles");
            ServiceNo         = DCC_GetColumn.String   (dataRow, "ServiceNo");

            return true;
        }

        public int Save(DCC_Database dcc_Database)
        {
            return (IsNew ? Insert(dcc_Database) : Update(dcc_Database));
        }

        int Insert(DCC_Database dcc_Database)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("iContractMileage");
            dbf.AddTable("ContractMileage");
            SetCommonParameters(dbf);
            dbf.AddParameter("_PRIMARYKEY", DCC_DatabaseFrame.OUTINTEGER);
            int newKey = dcc_Database.DataLayer.NonQueryPK(dbf);
            if (newKey > -1) K_ContractMileage = newKey;
            return newKey;
        }

        int Update(DCC_Database dcc_Database)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uContractMileage");
            dbf.AddTable("ContractMileage");
            dbf.AddParameter("K_ContractMileage", PrimaryKey);
            SetCommonParameters(dbf);
            dcc_Database.DataLayer.NonQuery(dbf);
            return (dbf.OK ? K_ContractMileage : -1);
        }

        void SetCommonParameters(DCC_DatabaseFrame dbf)
        {
            dbf.AddParameter("LicenceNo", LicenceNo , 500);
            dbf.AddParameter("Contract" , Contract  ,   4);
            dbf.AddParameter("Date"     , Date);
            dbf.AddParameter("LostMiles", LostMiles);
            dbf.AddParameter("ServiceNo", ServiceNo , 500);
        }

    }

    #endregion


    #region ContractMileageList

    public class ContractMileageList : IEnumerable<ContractMileage>
    {

        // Properties

        public List<object> All = new List<object>();



        // Utilities

        public int Count { get { return All.Count; } }
        public override string ToString() { return "ContractMileageList (" + Count + " items)"; }



        // Constructors

        public ContractMileageList(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sContractMileageList");
            dbf.AddTable("ContractMileage");
            db.DataLayer.SelectData(dbf);
            if (dbf.hasTables)
            {
                foreach (DataRow dataRow in dbf.TableZeroRows) Add(dataRow);
            }
        }



        // Methods

        public void Add(DataRow dataRow)
        {
            ContractMileage ContractMileageForm = new ContractMileage(dataRow);
            All.Add(ContractMileageForm);
        }



        // Enumerator

        public IEnumerator<ContractMileage> GetEnumerator() { foreach (object o in All) yield return (ContractMileage)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }

    }

    #endregion


    #region ContractMileageGrid

    public class ContractMileageGrid : DCC_Grid
    {
        public ContractMileageGrid(DCC_Database db)
        {
            ContractMileageList items = new ContractMileageList(db);
            TotalRows = items.All;
            Count = CountFiltered = TotalRows.Count;
        }
    }

    #endregion

}
