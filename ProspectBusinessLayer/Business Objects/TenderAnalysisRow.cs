using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProspectBusinessLayer.Business_Objects
{
    public class TenderAnalysisRow
    {
        public int PrimaryKey => K_Tender;
        public string TenderLink => "/Tender/Tender?primaryKey=" + PrimaryKey;
        public string GroupCssClass       { get; set; }
        public int K_Tender                { get; set; }
        public int TenderReference         { get; set; }
        public int Q_Specification         { get; set; }
        public int SpecRef                 { get; set; }
        public string ContractorName       { get; set; }
        public decimal TenderPricePerDay   { get; set; }
        public string TenderPricePerDayFormatted => TenderPricePerDay.ToString("F2");
        public bool IsConforming           { get; set; }
        public bool IsTenderAccepted       { get; set; }
        public bool IsPackage              { get; set; }
        public List<int> ChildSpecs        { get; set; } = new List<int>();
        public bool IsPartOfAnotherPackage { get; set; }
        public string AcceptTenderButton
        {
            get
            {
                return
                    $"<button type='submit' name='primaryKey' value='{PrimaryKey}' " +
                    "onclick='return confirm(\"Accept this tender?\");' " +
                    "style='background-color:#198754; color:white; border:none; border-radius:999px; padding:4px 12px; font-size:12px; cursor:pointer; display:block; margin:0 auto;'>" +
                    "Accept</button>";
            }
        }
    }
}
