using DCC_Standard;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace ProspectBusinessLayer
{

    #region ServiceType

    public class ServiceType
    {

        // Properties

        public int    K_ServiceType          { get; set; }
        public string Reference              { get; set; }
        public string ServiceTypeDescription { get; set; }
        public bool   IsArchived             { get; set; }



        // Utilities

        public int PrimaryKey { get { return K_ServiceType; } set { K_ServiceType = value; } }
        public string Key { get { return K_ServiceType.ToString(); } }
        public bool IsNew { get { return K_ServiceType == -1; } }
        public override string ToString() { return ServiceTypeDescription + ", " + K_ServiceType.ToString().PadLeft(3); }



        // Constructors

        public ServiceType()
        {
        }

        public ServiceType(DataRow dataRow)
        {
            GetColumns(dataRow);
        }

        public ServiceType(DCC_Database db, int pK_ServiceType = -999)
        {
            if (pK_ServiceType == -1) // create a new instance of Contractor and set defaults
            {
                K_ServiceType = -1;
            }

            else
            {
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sServiceType");
                dbf.AddTable("ServiceType");
                dbf.AddParameter("K_ServiceType", pK_ServiceType);
                db.DataLayer.SelectData(dbf);
                if (dbf.hasTables && dbf.TableZeroRows.Count == 1) GetColumns(dbf.RowZero);
            }
        }



        // Methods

        public bool GetColumns(DataRow dataRow)
        {
            if (dataRow == null) return false;

            K_ServiceType = DCC_GetColumn.Integer32(dataRow, "K_ServiceType");
            Reference = DCC_GetColumn.String(dataRow, "Reference");
            ServiceTypeDescription = DCC_GetColumn.String(dataRow, "ServiceTypeDescription");
            IsArchived = DCC_GetColumn.Bit(dataRow, "IsArchived");

            return true;
        }

        public int Save(DCC_Database dcc_Database)
        {
            return (IsNew ? Insert(dcc_Database) : Update(dcc_Database));
        }

        int Insert(DCC_Database dcc_Database)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("iServiceType");
            dbf.AddTable("ServiceType");
            dbf.AddParameter("Reference", Reference, 2);
            dbf.AddParameter("ServiceTypeDescription", ServiceTypeDescription, 500);
            dbf.AddParameter("_PRIMARYKEY", DCC_DatabaseFrame.OUTINTEGER);
            int newKey = dcc_Database.DataLayer.NonQueryPK(dbf);
            if (newKey > -1) K_ServiceType = newKey;
            return newKey;
        }

        int Update(DCC_Database dcc_Database)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uServiceType");
            dbf.AddTable("ServiceType");
            dbf.AddParameter("K_ServiceType", PrimaryKey);
            dbf.AddParameter("Reference", Reference, 2);
            dbf.AddParameter("ServiceTypeDescription", ServiceTypeDescription, 500);
            dbf.AddParameter("IsArchived", IsArchived);
            dcc_Database.DataLayer.NonQuery(dbf);
            return (dbf.OK ? K_ServiceType : -1);
        }

    }

    #endregion


    #region ServiceTypeList

    public class ServiceTypeList : IEnumerable<ServiceType>
    {

        // Properties

        public List<object> All = new List<object>();
        public List<object> Selectable = new List<object>();



        // Utilities

        public int Count { get { return All.Count; } }
        public override string ToString() { return "ServiceTypeList (" + Count + " items)"; }



        // Constructors

        public ServiceTypeList(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sServiceTypeList");
            dbf.AddTable("ServiceType");
            db.DataLayer.SelectData(dbf);
            if (dbf.hasTables)
            {
                foreach (DataRow dataRow in dbf.TableZeroRows) Add(dataRow);
            }
        }



        // Methods

        public void Add(DataRow dataRow)
        {
            ServiceType ServiceType = new ServiceType(dataRow);
            All.Add(ServiceType);
            if (!ServiceType.IsArchived) Selectable.Add(ServiceType);
        }



        // Enumerator

        public IEnumerator<ServiceType> GetEnumerator() { foreach (object o in All) yield return (ServiceType)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }

    }

    #endregion


    #region ServiceTypeGrid

    public class ServiceTypeGrid : DCC_Grid
    {
        public ServiceTypeGrid(DCC_Database db)
        {
            ServiceTypeList items = new ServiceTypeList(db);
            TotalRows = items.All;
            Count = CountFiltered = TotalRows.Count;
        }
    }

    #endregion

}
