using System;
using System.Collections.Generic;

namespace DCC_Standard
{

    /// <summary>
    /// Finds UK Bank (public) Holidays. Adjusted for weekends.
    /// UK Bank Holidays since 1971 Banking and Financial Dealings Act with additions and variations.
    /// See cref="http://www.direct.gov.uk/en/Employment/Employees/Timeoffandholidays/DG_073741"
    /// </summary>
    /// <example>
    /// listBox1.DataSource = UKBankHoliday.BankHolidays(2006);
    /// //fills listbox with 8 dates- 02/01, 14/04, 17/04, 01/05, 29/05, 28/08, 25/12, 26/12
    /// </example>
    public static class UKBankHoliday
    {
        /// <summary>
        /// Version Test.
        /// </summary>
        /// <returns>Information Message</returns>
        public static string RevisionInfo()
        {
            return "Last updated 6/1/12 to support up to 31st Dec 2015 (excepting any Royal Weddings!)";
        }

        /// <summary>
        /// Allows check that year has been programmed
        /// </summary>
        /// <param name="year">given year</param>
        /// <returns>true if given year has been programmed for</returns>
        public static bool IsSupported(int year)
        {
            if (year < 1971) return false;
            if (year > 2012) return false;
            return true;
        }

        /// <summary>
        /// Get a list of dates for all holidays in a year.
        /// </summary>
        /// <param name="year">The year</param>
        /// <returns>List of public holidays</returns>
        [Obsolete("Please use PublicHolidays() instead")]
        public static List<DateTime> BankHolidays(int year)
        {
            List<DateTime> bHols = new List<DateTime>();
            SortedList<DateTime, string> pHols = PublicHolidays(year);
            foreach (DateTime dt in pHols.Keys) bHols.Add(dt);
            return bHols;
        }

        /// <summary>
        /// Get a list of dates and names for all holidays in a year.
        /// </summary>
        /// <param name="year">The year</param>
        /// <returns>List of public holidays and names</returns>
        public static SortedList<DateTime, string> PublicHolidays(int year)
        {
            SortedList<DateTime, string> bankHolidays = new SortedList<DateTime, string>();

            if (year < 1971) return bankHolidays;

            DateTime? dtJan1 = NewYear(year);
            if (dtJan1.HasValue) bankHolidays.Add(dtJan1.Value, "New Year's Day");

            DateTime easter = GetEaster(year);
            bankHolidays.Add(GoodFriday(easter), "Good Friday");
            bankHolidays.Add(EasterMonday(easter), "Easter Monday");

            DateTime? dt = MayDay(year);
            if (dt.HasValue) bankHolidays.Add(dt.Value, "Early May Bank Holiday");

            bankHolidays.Add(Spring(year), "Spring Bank Holiday");

            if (year == 2002) bankHolidays.Add(new DateTime(2002, 6, 3), "Queen Elizabeth II's Golden Jubilee"); 
            if (year == 2011) bankHolidays.Add(new DateTime(2011, 4, 29), "Royal Wedding"); //Mr and Mrs Cambridge's Wedding
            if (year == 2012) bankHolidays.Add(new DateTime(2012, 6, 5), "Queen Elizabeth II's Diamond Jubilee"); 
            if (year == 2022) bankHolidays.Add(new DateTime(2022, 6, 3), "Queen Elizabeth II's Platinum Jubilee");
            if (year == 2022) bankHolidays.Add(new DateTime(2022, 9,19), "Queen Elizabeth II's Funeral");
            if (year == 2023) bankHolidays.Add(new DateTime(2023, 5, 8), "Charles III's Funeral");

            bankHolidays.Add(Summer(year), "Summer Bank Holiday");
            bankHolidays.Add(Christmas(year), "Christmas Day");
            bankHolidays.Add(BoxingDay(year), "Boxing Day");

            if (year == 1999) bankHolidays.Add(new DateTime(1999,12, 31), "Millenium"); 

            return bankHolidays;
        }

        /// <summary>
        /// Returns the next working day (Mon-Fri, not bank holiday)
        /// after the specified date (or the same date)
        /// </summary>
        /// <param name="dt">The date you wish to check</param>
        /// <returns>A date that is a working day</returns>
        public static DateTime NextWorkingDay(DateTime dt)
        {
            while (true)
            {
                if (dt.DayOfWeek == DayOfWeek.Saturday) { dt = dt.AddDays(2); continue; }
                if (dt.DayOfWeek != DayOfWeek.Sunday && !IsBankHoliday(dt)) break;
                dt = dt.AddDays(1);
            }        
            return dt;
        }

        /// <summary>
        /// Check if a specific date is a bank holiday.
        /// Obviously the BankHoliday list is more efficient for repeated checks
        /// </summary>
        /// <param name="dt">The date you wish to check</param>
        /// <returns>True if date is a bank holiday (excluding weekends)</returns>
        public static bool IsBankHoliday(DateTime dt)
        {
            SortedList<DateTime, string> bankHolidays = PublicHolidays(dt.Year);
            return (bankHolidays.ContainsKey(dt));
        }

        /// <summary>
        /// Gets the description of the bank holiday
        /// </summary>
        /// <param name="dt">The date you wish to check</param>
        /// <returns>The Bank Holiday description if the given date is a bank holiday or an empty string if it isn't</returns>
        public static string BankHolidayDescription(DateTime dt)
        {
            SortedList<DateTime, string> bankHolidays = PublicHolidays(dt.Year);
            int index = bankHolidays.IndexOfKey(dt);
            return ((index == -1) ? "" : bankHolidays.Values[index]);
        }

        /// <summary>
        /// Work out the date for Easter Sunday for specified year
        /// </summary>
        /// <param name="year">The year as an integer</param>
        /// <returns>Returns a datetime of Easter Sunday.</returns>
        private static DateTime GetEaster(int year)
        {
            int g, c, h, i, j, p;
            int easterMonth;
            int easterDay;
            int century;

            //should be
            //Easter Monday  28 Mar 2005  17 Apr 2006  9 Apr 2007  24 Mar 2008

            //Oudin's Algorithm - http://www.smart.net/~mmontes/oudin.html
            century = (int)year / 100;
            g = year % 19;
            c = year / 100;
            h = (c - c / 4 - (8 * c + 13) / 25 + 19 * g + 15) % 30;
            i = h - (h / 28) * (1 - (h / 28) * (29 / (h + 1)) * ((21 - g) / 11));
            j = (year + year / 4 + i + 2 - c + c / 4) % 7;
            p = i - j;
            easterDay = (int)1 + (p + 27 + (p + 6) / 40) % 31;
            easterMonth = (int)3 + (p + 26) / 30;

            return new DateTime(year, easterMonth, easterDay);
        }


        #region Holiday Adjustments

        private static DateTime FixWeekend(DateTime dt)
        {
            if (dt.DayOfWeek == DayOfWeek.Sunday) return dt.AddDays(1);
            if (dt.DayOfWeek == DayOfWeek.Saturday) return dt.AddDays(2);
            return dt;
        }

        private static DateTime FindMonday(DateTime dt, bool onOrAfter)
        {
            return dt.AddDays(onOrAfter 
                ?    ((7+(int)DayOfWeek.Monday -(int)(dt.DayOfWeek)) % 7)  
                :  ( ((7+(int)(dt.DayOfWeek)-(int)DayOfWeek.Monday ) % 7) *-1 ) );
        }

        #endregion


        #region Individual Holidays

        /// <summary>
        /// Christmas day, or Neil's birthday
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static DateTime Christmas(int year)
        {
            return FixWeekend(new DateTime(year, 12, 25));
        }

        /// <summary>
        /// Boxing Day
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static DateTime BoxingDay(int year)
        {
            DateTime dt = Christmas(year);
            //if Xmas=Fri, then move to Mon
            if (dt.DayOfWeek == DayOfWeek.Friday) return dt.AddDays(3);
            return dt.AddDays(1);
        }

        /// <summary>
        /// Date of New Year bank holiday. This is 1974 on only but will return pre 1974 dates.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static DateTime? NewYear(int year)
        {
            if (year < 1974) return null;
            //since 1974 only
            return FixWeekend(new DateTime(year, 1, 1));
        }

        /// <summary>
        /// Returns "Early Spring"/"May Day" holiday (first Monday in May). Created in 1978.
        /// </summary>
        /// <param name="year"></param>
        /// <returns>(Nullable)date for Early May Bank Holiday (null before 1978)</returns>
        public static DateTime? MayDay(int year)
        {
            //warning- should be null for < 1977
            if (year < 1978) return null;
            if (year == 1995) return new DateTime(1995, 5, 8); //1995 moved for 50th anniversary of VE day
            return FindMonday(new DateTime(year, 5, 1),true);
        }

        /// <summary>
        /// The Spring/Last Monday in May holiday (replaced variable Whit Monday in 1971)
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static DateTime Spring(int year)
        {
            if (year == 2002) return new DateTime(2002, 6, 4); //Golden Jubilee of Elizabeth II
            if (year == 2012) return new DateTime(2012, 6, 4); // Moved into June because of Diamond Jubilee 
            return FindMonday(new DateTime(year, 5, 31),false);
        }

        /// <summary>
        /// Summer bank holiday (last Monday in August)
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static DateTime Summer(int year)
        {
            return FindMonday(new DateTime(year, 8, 31),false);
        }

        /// <summary>
        /// Good Friday (Friday before Easter)
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static DateTime GoodFriday(int year)
        {
            return GetEaster(year).AddDays(-2);
        }

        /// <summary>
        /// Easter Monday (Monday after Easter)
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static DateTime EasterMonday(int year)
        {
            return GetEaster(year).AddDays(1);
        }

        /// <summary>
        /// Private overloads of GoodFriday and EasterMonday reusing Easter calculation
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static DateTime GoodFriday(DateTime dt)
        {
            return dt.AddDays(-2);
        }

        private static DateTime EasterMonday(DateTime dt)
        {
            return dt.AddDays(1);
        }

        #endregion

    }



    /// <summary>
    /// Various Date Routines
    /// </summary>
    public static class DateRoutines
    {

        /// <summary>
        /// Calculates number of business days, taking into account:
        ///  - weekends (Saturdays and Sundays)
        ///  - bank holidays in the middle of the week
        /// </summary>
        /// <param name="firstDay">First day in the time interval</param>
        /// <param name="lastDay">Last day in the time interval</param>
        /// <param name="deductNonWorkingDays">Deduct Saturdays, Sundays & Bank Holidays</param>
        /// <returns>Number of business days during the 'span'</returns>
        public static int? CalculateBusinessDays(DateTime firstDay, DateTime lastDay, bool deductNonWorkingDays = false)
        {
            firstDay = firstDay.Date;
            lastDay = lastDay.Date;
            if (firstDay > lastDay) return null;

            TimeSpan span = lastDay - firstDay;
            int totalDays = span.Days +1;

            if (deductNonWorkingDays)
            {
				#region Deduct Saturdays & Sundays
				int fullWeekCount = totalDays /7;
                if (totalDays > fullWeekCount *7) // Are there any weekends in the time period
                {
                    int firstDayOfWeek = firstDay.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)firstDay.DayOfWeek;
                    int lastDayOfWeek = lastDay.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)lastDay.DayOfWeek;
                    if (lastDayOfWeek < firstDayOfWeek) lastDayOfWeek += 7;
                    if (firstDayOfWeek <= 6)
                    {
                        if (lastDayOfWeek >= 7) totalDays -= 2; // Both Saturday and Sunday are in the remaining time interval
                        else if (lastDayOfWeek >= 6) totalDays -= 1; // Only Saturday is in the remaining time interval
                    }
                    else if (firstDayOfWeek <= 7 && lastDayOfWeek >= 7) totalDays -= 1; // Only Sunday is in the remaining time interval
                }
				totalDays -= fullWeekCount + fullWeekCount;
				#endregion

				#region Deduct Bank Holidays
				int noBankHolidays = 0;
                for (int i = firstDay.Year; i <= lastDay.Year; i++)
                {
                    SortedList<DateTime, string> bankHolidays = UKBankHoliday.PublicHolidays(i);
                    foreach(KeyValuePair<DateTime, string> bh in bankHolidays)
                    {
                        if ( (bh.Key >= firstDay) && (bh.Key <= lastDay) ) noBankHolidays++;
                    }
                }
                totalDays -= noBankHolidays;
				#endregion
			}

			return totalDays;
        }

        /// <summary>
        /// Override to handle null or missing parameters
        /// </summary>
        /// <param name="firstDay"></param>
        /// <param name="lastDay"></param>
        /// <param name="deductNonWorkingDays">Deduct Saturdays, Sundays & Bank Holidays</param>
        /// <returns></returns>
        public static int? CalculateBusinessDays(DateTime? firstDay = null, DateTime? lastDay = null, bool deductNonWorkingDays = false)
        {
            int? rc = null;
            if (firstDay.HasValue && lastDay.HasValue)
            {
                rc = CalculateBusinessDays(firstDay.Value, lastDay.Value, deductNonWorkingDays);
            }
            return rc;
        }

        /// <summary>
        /// Calculates time difference between two dates
        /// </summary>
        /// <param name="firstDate"></param>
        /// <param name="secondDate"></param>
        /// <returns>The time in days, hours, minutes & seconds as a string.</returns>
        public static string CalculateTimeBetweenTwoDates(DateTime firstDate, DateTime secondDate)
        {
            TimeSpan diff = DateTime.Now - DateTime.Today;
            string rc = string.Format(
                System.Globalization.CultureInfo.CurrentCulture, 
                "{0} days, {1} hours, {2} minutes, {3} seconds", 
                diff.Days, 
                diff.Hours, 
                diff.Minutes, 
                diff.Seconds);
            return rc;
        }

        /// <summary>
        /// Calculates difference between two dates
        /// </summary>
        /// <param name="dob"></param>
        /// <param name="asOfDate"></param>
        /// <param name="showMonths"></param>
        /// <param name="showMonthsAndDays"></param>
        /// <returns>The difference in years, months and days as a string.</returns>
        public static string CalculateAge(DateTime? dob, DateTime? asOfDate, bool showMonths = false, bool showMonthsAndDays = true)
        {
            int years, months, days;
            string rc = "NA";

            if (!dob.HasValue) return rc;
            if (!asOfDate.HasValue) asOfDate = DateTime.Now;

            if ((asOfDate.Value.Year - dob.Value.Year) > 0 || 
                (((asOfDate.Value.Year - dob.Value.Year) == 0) && ((dob.Value.Month < asOfDate.Value.Month) || 
                    ((dob.Value.Month == asOfDate.Value.Month) && (dob.Value.Day <= asOfDate.Value.Day)))))
            {
                int DaysInBdayMonth = DateTime.DaysInMonth(dob.Value.Year, dob.Value.Month);
                int DaysRemain = asOfDate.Value.Day + (DaysInBdayMonth - dob.Value.Day);
  
                if (asOfDate.Value.Month > dob.Value.Month)
                {
                    years = asOfDate.Value.Year - dob.Value.Year;
                    months = asOfDate.Value.Month - (dob.Value.Month + 1) + Math.Abs(DaysRemain / DaysInBdayMonth);
                    days = (DaysRemain % DaysInBdayMonth + DaysInBdayMonth) % DaysInBdayMonth;
                }

                else if (asOfDate.Value.Month == dob.Value.Month)
                {
                    if (asOfDate.Value.Day >= dob.Value.Day)
                    {
                        years = asOfDate.Value.Year - dob.Value.Year;
                        months = 0;
                        days = asOfDate.Value.Day - dob.Value.Day;
                    }
                    else
                    {
                        years = (asOfDate.Value.Year - 1) - dob.Value.Year;
                        months = 11;
                        days = DateTime.DaysInMonth(dob.Value.Year, dob.Value.Month) - (dob.Value.Day - asOfDate.Value.Day);
                    }
                }

                else
                {
                    years = (asOfDate.Value.Year - 1) - dob.Value.Year;
                    months = asOfDate.Value.Month + (11 - dob.Value.Month) + Math.Abs(DaysRemain / DaysInBdayMonth);
                    days = (DaysRemain % DaysInBdayMonth + DaysInBdayMonth) % DaysInBdayMonth;
                }

                rc = years.ToString() + "y";
                if (showMonthsAndDays) rc += " " + months.ToString() + "m" + " " + days.ToString() + "d";
                else if (showMonths) rc += " " + months.ToString() + "m";
            }

            else rc = "DOB was later than As Of date";

            return rc;
        }
    }


    public class DCC_FinancialYear
    {
        public DateTime CurrentStartDate       { get; set; }
        public DateTime CurrentEndDate         { get; set; }
        public DateTime LastStartDate          { get; set; }
        public DateTime LastEndDate            { get; set; }
        public DateTime NextStartDate          { get; set; }
        public DateTime NextEndDate            { get; set; }
        public DateTime Next2StartDate         { get; set; }
        public DateTime Next2EndDate           { get; set; }
        public string   CurrentAsString        { get { return (CurrentStartDate.ToString("yyyy") + "/" + CurrentStartDate.AddYears(1).ToString("yyyy")); } }
        public string   CurrentAsShortString   { get { return (CurrentStartDate.ToString("yyyy") + "/" + CurrentStartDate.AddYears(1).ToString("yy")); } }
        public string   CurrentAsShorterString { get { return (CurrentStartDate.ToString("yy")   + "/" + CurrentStartDate.AddYears(1).ToString("yy")); } }
        public string   LastAsString           { get { return (LastStartDate   .ToString("yyyy") + "/" + LastStartDate   .AddYears(1).ToString("yyyy")); } }
        public string   LastAsShortString      { get { return (LastStartDate   .ToString("yyyy") + "/" + LastStartDate   .AddYears(1).ToString("yy")); } }
        public string   LastAsShorterString    { get { return (LastStartDate   .ToString("yy")   + "/" + LastStartDate   .AddYears(1).ToString("yy")); } }
        public string   NextAsString           { get { return (NextStartDate   .ToString("yyyy") + "/" + NextStartDate   .AddYears(1).ToString("yyyy")); } }
        public string   NextAsShortString      { get { return (NextStartDate   .ToString("yyyy") + "/" + NextStartDate   .AddYears(1).ToString("yy")); } }
        public string   NextAsShorterString    { get { return (NextStartDate   .ToString("yy")   + "/" + NextStartDate   .AddYears(1).ToString("yy")); } }
        public string   Next2AsString          { get { return (Next2StartDate  .ToString("yyyy") + "/" + Next2StartDate  .AddYears(1).ToString("yyyy")); } }
        public string   Next2AsShortString     { get { return (Next2StartDate  .ToString("yyyy") + "/" + Next2StartDate  .AddYears(1).ToString("yy")); } }
        public string   Next2AsShorterString   { get { return (Next2StartDate  .ToString("yy")   + "/" + Next2StartDate  .AddYears(1).ToString("yy")); } }

        public DCC_FinancialYear(DateTime? pDate = null)
        {
            if (pDate == null) pDate = DateTime.Now;

            if (pDate.Value.Month < 4)
            {
                CurrentStartDate = new DateTime(pDate.Value.Year -1, 4, 1);
                CurrentEndDate   = CurrentStartDate.AddYears(1).AddDays(-1);
            }
            else
            {
                CurrentStartDate = new DateTime(pDate.Value.Year, 4, 1);
                CurrentEndDate   = CurrentStartDate.AddYears(1).AddDays(-1);
            }

            LastStartDate  = CurrentStartDate.AddYears(-1);
            LastEndDate    = CurrentEndDate  .AddYears(-1);
            NextStartDate  = CurrentStartDate.AddYears(1);
            NextEndDate    = CurrentEndDate  .AddYears(1);
            Next2StartDate = CurrentStartDate.AddYears(2);
            Next2EndDate   = CurrentEndDate  .AddYears(2);
        }
    }

}
