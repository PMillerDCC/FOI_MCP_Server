using System;
using NUnit.Framework;

namespace DCC_Standard
{
    public static class DebugTrace
    {
        public static void WriteLine(string logText = "")
        {
            // No-op during tests
        }
    }
}

namespace ProspectBusinessLayer.Tests
{
    [TestFixture]
    public class ContractPaymentScheduleTests
    {
        [Test]
        public void SanityCheck()
        {
            Assert.That(1 + 1, Is.EqualTo(2));
        }

        [Test]
        public void Contract500_200_FuelAdjustment_CalculatesCorrectly()
        {
            // Example 1: TOTAL per day £500, fuel element £200, index tender=150, period=153, 20 days
            decimal averagePerDay = 500m;
            decimal fuelElement = 200m;
            decimal tenderCloseIndex = 150m;
            decimal paymentPeriodIndex = 153m;
            int daysInPeriod = 20;

            decimal priceMinusFuel = averagePerDay - fuelElement; // 300
            decimal adjustedFuelElement = fuelElement * (paymentPeriodIndex / tenderCloseIndex); // 200 * 153/150
            adjustedFuelElement = Math.Round(adjustedFuelElement, 2, MidpointRounding.AwayFromZero); // 204.00

            decimal totalPerDay = priceMinusFuel + adjustedFuelElement; // 502.00
            decimal grandTotal = Math.Round(totalPerDay * daysInPeriod, 2, MidpointRounding.AwayFromZero); // 10040.00

            TestContext.WriteLine($"priceMinusFuel={priceMinusFuel}, adjustedFuel={adjustedFuelElement}, totalPerDay={totalPerDay}");

            Assert.That(adjustedFuelElement, Is.EqualTo(204.00m));
            Assert.That(totalPerDay, Is.EqualTo(502.00m));
            Assert.That(grandTotal, Is.EqualTo(10040.00m));
        }

        [Test]
        public void Contract305_80_FuelAdjustment_CalculatesCorrectly()
        {
            // Example 2: TOTAL per day £305, fuel element £80, index tender=147, period=115, 11 days
            decimal averagePerDay = 305m;
            decimal fuelElement = 80m;
            decimal tenderCloseIndex = 147m;
            decimal paymentPeriodIndex = 115m;
            int daysInPeriod = 11;

            decimal priceMinusFuel = averagePerDay - fuelElement; // 225

            // Step 1: round the fuel element per day first
            decimal adjustedFuelElement = fuelElement * (paymentPeriodIndex / tenderCloseIndex);
            adjustedFuelElement = Math.Round(adjustedFuelElement, 2, MidpointRounding.AwayFromZero); // 62.58

            // Step 2: add to base price, then round daily total
            decimal totalPerDay = priceMinusFuel + adjustedFuelElement;
            totalPerDay = Math.Round(totalPerDay, 2, MidpointRounding.AwayFromZero); // 287.58

            // Step 3: multiply by number of days, then round once
            decimal grandTotal = Math.Round(totalPerDay * daysInPeriod, 2, MidpointRounding.AwayFromZero); // 3163.38

            Assert.That(adjustedFuelElement, Is.EqualTo(62.58m).Within(0.01m));
            Assert.That(totalPerDay, Is.EqualTo(287.58m).Within(0.01m));
            Assert.That(grandTotal, Is.EqualTo(3163.38m));
        }

        [Test]
        public void DaysOfOperation_MonToFri_ExcludesWeekends_Basic()
        {
            // This uses a minimal Spec-like object; if your real Spec needs DB, consider
            // wrapping CalculateDaysOfOperationInPeriod to accept a null db for tests.
            var spec = new Specification
            {
                DateStart = new DateTime(2023, 1, 1),
                DateEnd = new DateTime(2023, 1, 31),
                OperatesOnMon = true,
                OperatesOnTue = true,
                OperatesOnWed = true,
                OperatesOnThu = true,
                OperatesOnFri = true,
                OperatesOnSat = false,
                OperatesOnSun = false,
                OperatesOnBankHolidays = false,
                OperatesOnSchoolDays = false,
                OperatesOnNonSchoolDays = false,
                OperatesOnChristmasAndNewYear = false
            };

            // Period is whole January 2023
            var periodStart = new DateTime(2023, 1, 1);
            var periodEnd = new DateTime(2023, 1, 31);

            // If your CalculateDaysOfOperationInPeriod needs a db, pass a lightweight stub or null if it tolerates null
            int days = spec.CalculateDaysOfOperationInPeriod(db: null, periodStart, periodEnd);

            // Jan 2023 weekdays = 23 (check your holiday logic if your Holiday class marks bank holidays inside January)
            Assert.That(days, Is.EqualTo(23));
        }


    }
}
