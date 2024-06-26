using DCC_Standard;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace ProspectBusinessLayer
{

    #region FuelCost

    public class FuelCost
    {

        // Properties

        public int       K_FuelCost { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime  EffectiveFrom { get; set; }
        public string    EffectiveFromText { get { return DCC_Formatting.NullDateTxt(EffectiveFrom, "dd MM yyyy");} }
        public string    EffectiveFromSort { get {  return DCC_Formatting.NullDateTxt(EffectiveFrom, "yyyyMMddHHmm");} }
        public decimal   Price { get; set; }
        public string    PriceString { get; set; }
       


        //Utilities

        public int PrimaryKey { get { return K_FuelCost; } set { K_FuelCost = value; } }
        public string Key { get { return PrimaryKey.ToString(); } }
        public bool IsNew { get { return PrimaryKey == -1; } }
        public override string ToString() { return EffectiveFrom.ToString() + " " + Price.ToString() + " " + PrimaryKey.ToString().PadLeft(3); }

   

        //Constructors

        public FuelCost() 
        
        { 
            K_FuelCost = -1;    
        }

        public FuelCost(DataRow dataRow)
        {
            GetColumns(dataRow);
        }

        public FuelCost(DCC_Database db, int pK_FuelCost = -1, DateTime? searchDate = null)
        { 
            if (searchDate.HasValue)
            {
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sFuelCostByDate");
                dbf.AddTable("FuelCost");
                dbf.AddParameter("SearchDate", searchDate.Value);
                db.DataLayer.SelectData(dbf);
                if (dbf.hasTables && dbf.TableZeroRows.Count == 1)
                    GetColumns(dbf.RowZero);
            }

            else if (pK_FuelCost == -1) K_FuelCost = -1;

            else if (pK_FuelCost >0)
            {
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sFuelCost");
                dbf.AddTable("FuelCost");
                dbf.AddParameter("K_FuelCost", pK_FuelCost);
                db.DataLayer.SelectData(dbf);
                if (dbf.hasTables && dbf.TableZeroRows.Count == 1)
                    GetColumns(dbf.RowZero);
            }
        }



        //Methods

        public bool GetColumns(DataRow dataRow)
        {
            if (dataRow == null) return false;

            K_FuelCost = DCC_GetColumn.Integer32 (dataRow, "K_FuelCost");
            EffectiveFrom = DCC_GetColumn.DateTime (dataRow, "EffectiveFrom");
            Price = DCC_GetColumn.Decimal (dataRow, "Price");

            return true;
        }

        public int Save(DCC_Database db)
        {
            return (IsNew ? Insert(db) : Update(db));
        }

        int Insert(DCC_Database db)
        {
            // Initialize a new instance of DCC_DatabaseFrame for the "iFuelCost" stored procedure.
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("iFuelCost");
            // Specify the target table for the stored procedure.
            dbf.AddTable("FuelCost");
            // Add parameters for the stored procedure.
            // Effective date when the fuel price becomes applicable.
            dbf.AddParameter("EffectiveFrom", EffectiveFrom);
            // The price of the fuel effective from the specified date.
            dbf.AddParameter("Price", Price);
            // An output parameter to retrieve the primary key of the newly inserted row.
            dbf.AddParameter("_PRIMARYKEY", DCC_DatabaseFrame.OUTINTEGER);
            // Execute the stored procedure and retrieve the primary key of the inserted record.
            int newKey = db.DataLayer.NonQueryPK(dbf);
            // If the insertion was successful (newKey > -1), update the K_FuelCost property with the new primary key.
            if (newKey > -1) K_FuelCost = newKey;
            // Return the primary key of the inserted row.
            return newKey;
        }

        int Update(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uFuelCost");
            dbf.AddTable("FuelCost");
            dbf.AddParameter("K_FuelCost", K_FuelCost);
            dbf.AddParameter("EffectiveFrom", EffectiveFrom);
            dbf.AddParameter("Price", Price);
            db.DataLayer.NonQuery(dbf);
            return (dbf.OK ? K_FuelCost : -1);
        }

        public int Delete(DCC_Database dcc_Database) { throw new NotImplementedException(); }

    }
    
    #endregion


    #region FuelCostList

    public class FuelCostList : IEnumerable<FuelCost>
    {

        //Properties

        public List<object> All = new List<object>();
        public List<object> Selectable = new List<object>();



        //Utilities

        public int Count { get { return All.Count; } }
        public override string ToString() { return "FuelCostList has " + Count + " items"; }



        // Data Grids

        public FuelCostGrid FuelCostGrid;



        //Constructors

        public FuelCostList() 
        {
            
        }

        public FuelCostList(DCC_Database dCC_Database) 
        { 
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sFuelCostList");
            dbf.AddTable("FuelCost");
            dCC_Database.DataLayer.SelectData(dbf);
            if (dbf.hasTables)
            {
                foreach (DataRow dataRow in dbf.TableZeroRows) Add(dataRow);
            }
        }



        //Methods

        public void Add(DataRow dataRow) 
        {
            FuelCost fuelCost = new FuelCost(dataRow);
            All.Add(fuelCost);
        }



        //Enumerator

        public IEnumerator<FuelCost> GetEnumerator() { foreach (object o in All) yield return (FuelCost) o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach(object o in All) yield return "dummy"; }

    }

    #endregion


    #region FuelCostGrid

    public class FuelCostGrid : DCC_Grid
    {
       public FuelCostGrid(DCC_Database db) 
       { 
            FuelCostList FuelCostList = new FuelCostList(db);
            TotalRows = FuelCostList.All;
            Count = CountFiltered = TotalRows.Count;
       }
    }

    #endregion

}
