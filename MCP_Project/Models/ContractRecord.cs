namespace MCP_Project.Models
{
    public class ContractRecord
    {
        public string MasterRefNo { get; set; } = string.Empty;
        public string ServiceArea { get; set; } = string.Empty;
        public string ContractTitle { get; set; } = string.Empty;
        public string Supplier { get; set; } = string.Empty;
        public string ContractManager { get; set; } = string.Empty;
        public bool RelevantToIT { get; set; }

        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }

        public decimal? TotalContractValue { get; set; }
    }
}
