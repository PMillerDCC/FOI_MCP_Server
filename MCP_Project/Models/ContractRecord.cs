namespace MCP_Project.Models
{
    /// <summary>
    /// Denormalized contract record mapped from the contracts spreadsheet.
    /// </summary>
    public class ContractRecord
    {
        /// <summary>
        /// Master reference from the source workbook.
        /// </summary>
        public string MasterRefNo { get; set; } = string.Empty;

        /// <summary>
        /// Contract reference identifier.
        /// </summary>
        public string ContractReference { get; set; } = string.Empty;

        /// <summary>
        /// Classification reference identifier.
        /// </summary>
        public string ClassReferenceNumber { get; set; } = string.Empty;

        /// <summary>
        /// Contract classification label.
        /// </summary>
        public string Classification { get; set; } = string.Empty;

        /// <summary>
        /// Contract manager email contact.
        /// </summary>
        public string ContractManagerEmail { get; set; } = string.Empty;

        /// <summary>
        /// Procurement officer name.
        /// </summary>
        public string ProcurementOfficer { get; set; } = string.Empty;

        /// <summary>
        /// Framework title in the source register.
        /// </summary>
        public string FrameworksTitle { get; set; } = string.Empty;
        
        /// <summary>
        /// Owning service area.
        /// </summary>
        public string ServiceArea { get; set; } = string.Empty;

        /// <summary>
        /// Human-readable contract title.
        /// </summary>
        public string ContractTitle { get; set; } = string.Empty;

        /// <summary>
        /// Supplier name.
        /// </summary>
        public string Supplier { get; set; } = string.Empty;

        /// <summary>
        /// Contract manager name.
        /// </summary>
        public string ContractManager { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether the record is marked as relevant to IT.
        /// </summary>
        public bool RelevantToIT { get; set; }

        /// <summary>
        /// Contract start date when available.
        /// </summary>
        public DateTime? ContractStartDate { get; set; }

        /// <summary>
        /// Contract end date when available.
        /// </summary>
        public DateTime? ContractEndDate { get; set; }

        /// <summary>
        /// Total contract value when provided in source data.
        /// </summary>
        public decimal? TotalContractValue { get; set; }
    }
}
