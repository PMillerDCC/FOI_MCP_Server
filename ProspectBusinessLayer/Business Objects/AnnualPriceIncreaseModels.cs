using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ProspectBusinessLayer
{
    /// <summary>
    /// Model for the Annual Price Increase page
    /// </summary>
    public class AnnualPriceIncreaseModel
    {
        [Required]
        [Display(Name = "Specification Start")]
        [Range(1, 999999, ErrorMessage = "Specification Start must be between 1 and 999999")]
        public int? SpecStart { get; set; }

        [Required]
        [Display(Name = "Specification End")]
        [Range(1, 999999, ErrorMessage = "Specification End must be between 1 and 999999")]
        public int? SpecEnd { get; set; }

        [Display(Name = "Price Increase Percentage")]
        [Range(0.01, 100.0, ErrorMessage = "Percentage must be between 0.01 and 100")]
        public decimal? IncreasePercentage { get; set; }

        [Display(Name = "Price Increase Applies From")]
        public DateTime? PriceIncreaseAppliesFrom { get; set; }

        public List<AnnualPriceIncreaseRow> Rows { get; set; } = new List<AnnualPriceIncreaseRow>();

        public List<int> SelectedVariationIds { get; set; } = new List<int>();

        public bool HasSearched { get; set; }
    }

    /// <summary>
    /// Represents one contract row in the annual price increase listing
    /// </summary>
    public class AnnualPriceIncreaseRow
    {
        public int K_ContractVariation { get; set; }
        public int K_Tender { get; set; }
        public int ContractReference { get; set; }
        public string ContractReferenceText => ContractReference.ToString().PadLeft(5, '0');
        public int TenderReference { get; set; }
        public string TenderReferenceText => TenderReference.ToString().PadLeft(5, '0');
        public string ContractorName { get; set; }
        public string ServiceNo { get; set; }
        public string RouteText { get; set; }
        public int ContractVariationNo { get; set; }
        public string ContractVariationNoText => ContractVariationNo.ToString().PadLeft(2, '0');

        public decimal CurrentPricePerDay { get; set; }
        public string CurrentPricePerDayText => CurrentPricePerDay.ToString("N2");

        public int DaysOfOperation { get; set; }

        public decimal CurrentPricePerYear { get; set; }
        public string CurrentPricePerYearText => CurrentPricePerYear.ToString("N2");

        public decimal NewPricePerDay { get; set; }
        public string NewPricePerDayText => NewPricePerDay.ToString("N2");

        public decimal NewPricePerYear { get; set; }
        public string NewPricePerYearText => NewPricePerYear.ToString("N2");

        public bool IsSelected { get; set; }

        /// <summary>
        /// Calculate new prices based on percentage increase
        /// </summary>
        public void CalculateNewPrices(decimal percentageIncrease)
        {
            decimal multiplier = 1 + (percentageIncrease / 100m);
            NewPricePerDay = Math.Round(CurrentPricePerDay * multiplier, 2);
            NewPricePerYear = Math.Round(NewPricePerDay * DaysOfOperation, 2);
        }
    }
}
