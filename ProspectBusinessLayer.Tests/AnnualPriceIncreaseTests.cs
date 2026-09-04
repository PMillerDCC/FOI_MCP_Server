using System;
using NUnit.Framework;
using ProspectBusinessLayer;

namespace ProspectBusinessLayer.Tests
{
    [TestFixture]
    public class AnnualPriceIncreaseTests
    {
        [Test]
        public void CalculateNewPrices_WithValidPercentage_CalculatesCorrectly()
        {
            // Arrange
            var row = new AnnualPriceIncreaseRow
            {
                CurrentPricePerDay = 100.00m,
                DaysOfOperation = 250
            };

            // Act
            row.CalculateNewPrices(5.0m); // 5% increase

            // Assert
            Assert.That(row.NewPricePerDay, Is.EqualTo(105.00m));
            Assert.That(row.NewPricePerYear, Is.EqualTo(26250.00m)); // 105 * 250
        }

        [Test]
        public void CalculateNewPrices_WithDecimalPercentage_RoundsCorrectly()
        {
            // Arrange
            var row = new AnnualPriceIncreaseRow
            {
                CurrentPricePerDay = 123.45m,
                DaysOfOperation = 365
            };

            // Act
            row.CalculateNewPrices(2.5m); // 2.5% increase

            // Assert
            // 123.45 * 1.025 = 126.53625 -> rounds to 126.54
            Assert.That(row.NewPricePerDay, Is.EqualTo(126.54m));
            // 126.54 * 365 = 46,187.10
            Assert.That(row.NewPricePerYear, Is.EqualTo(46187.10m));
        }

        [Test]
        public void CalculateNewPrices_WithZeroPercentage_NoChange()
        {
            // Arrange
            var row = new AnnualPriceIncreaseRow
            {
                CurrentPricePerDay = 200.00m,
                DaysOfOperation = 300
            };

            // Act
            row.CalculateNewPrices(0.0m);

            // Assert
            Assert.That(row.NewPricePerDay, Is.EqualTo(200.00m));
            Assert.That(row.NewPricePerYear, Is.EqualTo(60000.00m));
        }

        [Test]
        public void CalculateNewPrices_WithLargePercentage_CalculatesCorrectly()
        {
            // Arrange
            var row = new AnnualPriceIncreaseRow
            {
                CurrentPricePerDay = 50.00m,
                DaysOfOperation = 200
            };

            // Act
            row.CalculateNewPrices(50.0m); // 50% increase

            // Assert
            Assert.That(row.NewPricePerDay, Is.EqualTo(75.00m));
            Assert.That(row.NewPricePerYear, Is.EqualTo(15000.00m));
        }

        [Test]
        public void CalculateNewPrices_WithSmallPercentage_CalculatesCorrectly()
        {
            // Arrange
            var row = new AnnualPriceIncreaseRow
            {
                CurrentPricePerDay = 1000.00m,
                DaysOfOperation = 365
            };

            // Act
            row.CalculateNewPrices(0.01m); // 0.01% increase

            // Assert
            // 1000.00 * 1.0001 = 1000.10
            Assert.That(row.NewPricePerDay, Is.EqualTo(1000.10m));
            Assert.That(row.NewPricePerYear, Is.EqualTo(365036.50m));
        }

        [Test]
        public void CalculateNewPrices_WithOddDaysOfOperation_CalculatesCorrectly()
        {
            // Arrange
            var row = new AnnualPriceIncreaseRow
            {
                CurrentPricePerDay = 99.99m,
                DaysOfOperation = 123
            };

            // Act
            row.CalculateNewPrices(3.75m); // 3.75% increase

            // Assert
            // 99.99 * 1.0375 = 103.739625 -> rounds to 103.74
            Assert.That(row.NewPricePerDay, Is.EqualTo(103.74m));
            // 103.74 * 123 = 12,760.02
            Assert.That(row.NewPricePerYear, Is.EqualTo(12760.02m));
        }

        [Test]
        public void AnnualPriceIncreaseModel_DefaultValues_AreValid()
        {
            // Arrange & Act
            var model = new AnnualPriceIncreaseModel();

            // Assert
            Assert.That(model.SpecStart, Is.Null);
            Assert.That(model.SpecEnd, Is.Null);
            Assert.That(model.IncreasePercentage, Is.Null);
            Assert.That(model.PriceIncreaseAppliesFrom, Is.Null);
            Assert.That(model.Rows, Is.Not.Null);
            Assert.That(model.Rows.Count, Is.EqualTo(0));
            Assert.That(model.SelectedVariationIds, Is.Not.Null);
            Assert.That(model.SelectedVariationIds.Count, Is.EqualTo(0));
            Assert.That(model.HasSearched, Is.False);
        }

        [Test]
        public void AnnualPriceIncreaseRow_FormattedTextProperties_DisplayCorrectly()
        {
            // Arrange
            var row = new AnnualPriceIncreaseRow
            {
                ContractReference = 123,
                TenderReference = 45,
                ContractVariationNo = 7,
                CurrentPricePerDay = 1234.56m,
                CurrentPricePerYear = 456789.12m
            };

            // Act & Assert
            Assert.That(row.ContractReferenceText, Is.EqualTo("00123"));
            Assert.That(row.TenderReferenceText, Is.EqualTo("00045"));
            Assert.That(row.ContractVariationNoText, Is.EqualTo("07"));
            Assert.That(row.CurrentPricePerDayText, Is.EqualTo("1,234.56"));
            Assert.That(row.CurrentPricePerYearText, Is.EqualTo("456,789.12"));
        }
    }
}
