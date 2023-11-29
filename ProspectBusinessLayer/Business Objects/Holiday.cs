using DCC_Standard;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace ProspectBusinessLayer
{

    #region Holiday

    public class Holiday
    {

        // Properties

        public int       K_Holiday         { get; set; }
        public string    Year              { get; set; }
        public DateTime? NewYearsDay       { get; set; }
        public DateTime? GoodFriday        { get; set; }
        public DateTime? EasterMonday      { get; set; }
        public DateTime? MayDay            { get; set; }
        public DateTime? SpringBankHoliday { get; set; }
        public DateTime? SummerBankHoliday { get; set; }
        public DateTime? ChristmasDay      { get; set; }
        public DateTime? BoxingDay         { get; set; }
        public DateTime? Special1          { get; set; }
        public DateTime? Special2          { get; set; }
        public DateTime? Special3          { get; set; }
        public DateTime? Special4          { get; set; }
        public DateTime? Special5          { get; set; }
        public DateTime? Special6          { get; set; }
        public DateTime? Special7          { get; set; }
        public DateTime? Special8          { get; set; }
        public DateTime? Spring1Start      { get; set; }
        public DateTime? Spring1End        { get; set; }
        public DateTime? Spring2Start      { get; set; }
        public DateTime? Spring2End        { get; set; }
        public DateTime? Summer1Start      { get; set; }
        public DateTime? Summer1End        { get; set; }
        public DateTime? Summer2Start      { get; set; }
        public DateTime? Summer2End        { get; set; }
        public DateTime? Autumn1Start      { get; set; }
        public DateTime? Autumn1End        { get; set; }
        public DateTime? Autumn2Start      { get; set; }
        public DateTime? Autumn2End        { get; set; }



        // Utilities

        public int PrimaryKey { get { return K_Holiday; } set { K_Holiday = value; } }
        public string Key { get { return K_Holiday.ToString(); } }
        public bool IsNew { get { return K_Holiday == -1; } }
        public override string ToString() { return K_Holiday.ToString().PadLeft(3); }



        // Constructors

        public Holiday()
        {
        }

        public Holiday(DataRow dataRow)
        {
            GetColumns(dataRow);
        }

        public Holiday(DCC_Database db, int pK_Holiday = -999, string pYear = null)
        {
            if (pK_Holiday == -1) // create a new instance of Contractor and set defaults
            {
                K_Holiday = -1;
            }

            else
            {
                DCC_DatabaseFrame dbf;
                if (pYear != null)
                {
                    dbf = new DCC_DatabaseFrame("sHolidayByYear");
                    dbf.AddParameter("Year", pYear, 4);
                }
                else
                {
                    dbf = new DCC_DatabaseFrame("sHoliday");
                    dbf.AddParameter("K_Holiday", pK_Holiday);
                }
                dbf.AddTable("Holiday");
                db.DataLayer.SelectData(dbf);
                if (dbf.hasTables && dbf.TableZeroRows.Count == 1) GetColumns(dbf.RowZero);
            }
        }



        // Methods

        public bool GetColumns(DataRow dataRow)
        {
            if (dataRow == null) return false;

            K_Holiday         = DCC_GetColumn.Integer32 (dataRow, "K_Holiday");
            Year              = DCC_GetColumn.String    (dataRow, "Year");
            NewYearsDay       = DCC_GetColumn.DateTimeQ (dataRow, "NewYearsDay");
            GoodFriday        = DCC_GetColumn.DateTimeQ (dataRow, "GoodFriday");
            EasterMonday      = DCC_GetColumn.DateTimeQ (dataRow, "EasterMonday");
            MayDay            = DCC_GetColumn.DateTimeQ (dataRow, "MayDay");
            SpringBankHoliday = DCC_GetColumn.DateTimeQ (dataRow, "SpringBankHoliday");
            SummerBankHoliday = DCC_GetColumn.DateTimeQ (dataRow, "SummerBankHoliday");
            ChristmasDay      = DCC_GetColumn.DateTimeQ (dataRow, "ChristmasDay");
            BoxingDay         = DCC_GetColumn.DateTimeQ (dataRow, "BoxingDay");
            Special1          = DCC_GetColumn.DateTimeQ (dataRow, "Special1");
            Special2          = DCC_GetColumn.DateTimeQ (dataRow, "Special2");
            Special3          = DCC_GetColumn.DateTimeQ (dataRow, "Special3");
            Special4          = DCC_GetColumn.DateTimeQ (dataRow, "Special4");
            Special5          = DCC_GetColumn.DateTimeQ (dataRow, "Special5");
            Special6          = DCC_GetColumn.DateTimeQ (dataRow, "Special6");
            Special7          = DCC_GetColumn.DateTimeQ (dataRow, "Special7");
            Special8          = DCC_GetColumn.DateTimeQ (dataRow, "Special8");
            Spring1Start      = DCC_GetColumn.DateTimeQ (dataRow, "Spring1Start");
            Spring1End        = DCC_GetColumn.DateTimeQ (dataRow, "Spring1End");
            Spring2Start      = DCC_GetColumn.DateTimeQ (dataRow, "Spring2Start");
            Spring2End        = DCC_GetColumn.DateTimeQ (dataRow, "Spring2End");
            Summer1Start      = DCC_GetColumn.DateTimeQ (dataRow, "Summer1Start");
            Summer1End        = DCC_GetColumn.DateTimeQ (dataRow, "Summer1End");
            Summer2Start      = DCC_GetColumn.DateTimeQ (dataRow, "Summer2Start");
            Summer2End        = DCC_GetColumn.DateTimeQ (dataRow, "Summer2End");
            Autumn1Start      = DCC_GetColumn.DateTimeQ (dataRow, "Autumn1Start");
            Autumn1End        = DCC_GetColumn.DateTimeQ (dataRow, "Autumn1End");
            Autumn2Start      = DCC_GetColumn.DateTimeQ (dataRow, "Autumn2Start");
            Autumn2End        = DCC_GetColumn.DateTimeQ (dataRow, "Autumn2End");

            return true;
        }

        public int Save(DCC_Database dcc_Database)
        {
            return (IsNew ? Insert(dcc_Database) : Update(dcc_Database));
        }

        int Insert(DCC_Database dcc_Database)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("iHoliday");
            dbf.AddTable("Holiday");
            dbf.AddParameter("Year", Year, 4);
            SetCommonParameters(dbf);
            dbf.AddParameter("_PRIMARYKEY", DCC_DatabaseFrame.OUTINTEGER);
            int newKey = dcc_Database.DataLayer.NonQueryPK(dbf);
            if (newKey > -1) K_Holiday = newKey;
            return newKey;
        }

        int Update(DCC_Database dcc_Database)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uHoliday");
            dbf.AddTable("Holiday");
            dbf.AddParameter("K_Holiday", K_Holiday);
            SetCommonParameters(dbf);
            dcc_Database.DataLayer.NonQuery(dbf);
            return (dbf.OK ? K_Holiday : -1);
        }

        void SetCommonParameters(DCC_DatabaseFrame dbf)
        {
            dbf.AddParameter("NewYearsDay", NewYearsDay);
            dbf.AddParameter("GoodFriday", GoodFriday);
            dbf.AddParameter("EasterMonday", EasterMonday);
            dbf.AddParameter("MayDay", MayDay);
            dbf.AddParameter("SpringBankHoliday", SpringBankHoliday);
            dbf.AddParameter("SummerBankHoliday", SummerBankHoliday);
            dbf.AddParameter("ChristmasDay", ChristmasDay);
            dbf.AddParameter("BoxingDay", BoxingDay);
            dbf.AddParameter("Special1", Special1);
            dbf.AddParameter("Special2", Special2);
            dbf.AddParameter("Special3", Special3);
            dbf.AddParameter("Special4", Special4);
            dbf.AddParameter("Special5", Special5);
            dbf.AddParameter("Special6", Special6);
            dbf.AddParameter("Special7", Special7);
            dbf.AddParameter("Special8", Special8);
            dbf.AddParameter("Spring1Start", Spring1Start);
            dbf.AddParameter("Spring1End", Spring1End);
            dbf.AddParameter("Spring2Start", Spring2Start);
            dbf.AddParameter("Spring2End", Spring2End);
            dbf.AddParameter("Summer1Start", Summer1Start);
            dbf.AddParameter("Summer1End", Summer1End);
            dbf.AddParameter("Summer2Start", Summer2Start);
            dbf.AddParameter("Summer2End", Summer2End);
            dbf.AddParameter("Autumn1Start", Autumn1Start);
            dbf.AddParameter("Autumn1End", Autumn1End);
            dbf.AddParameter("Autumn2Start", Autumn2Start);
            dbf.AddParameter("Autumn2End", Autumn2End);
        }

    }

    #endregion


    #region HolidayList

    public class HolidayList : IEnumerable<Holiday>
    {

        // Properties

        public List<object> All = new List<object>();



        // Utilities

        public int Count { get { return All.Count; } }
        public override string ToString() { return "HolidayList (" + Count + " items)"; }



        // Constructors

        public HolidayList(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sHolidayList");
            dbf.AddTable("Holiday");
            db.DataLayer.SelectData(dbf, 5000);
            if (dbf.hasTables)
            {
                foreach (DataRow dataRow in dbf.TableZeroRows) Add(dataRow);
            }
        }



        // Methods

        public void Add(DataRow dataRow)
        {
            Holiday Holiday = new Holiday(dataRow);
            All.Add(Holiday);
        }



        // Enumerator

        public IEnumerator<Holiday> GetEnumerator() { foreach (object o in All) yield return (Holiday)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }

    }

    #endregion


    #region HolidayGrid

    public class HolidayGrid : DCC_Grid
    {
        public HolidayGrid(DCC_Database db)
        {
            HolidayList items = new HolidayList(db);
            TotalRows = items.All;
            Count = CountFiltered = TotalRows.Count;
        }
    }

    #endregion

}
