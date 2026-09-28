namespace MCP_Project.Models
{
    public class FOIRequest
    {
        public int RequestID { get; set; }
        public DateTime DateReceived { get; set; }
        public string RequesterType { get; set; }
        public string Summary { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
