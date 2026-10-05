using System.Collections.Generic;
using System.Linq;
namespace ProspectBusinessLayer.Business_Objects
{
    public class TenderAnalysisPrintModel
    {
        public int SpecStart { get; set; }
        public int SpecEnd { get; set; }
        public List<TenderAnalysisSpecificationGroup> Groups { get; set; } = new List<TenderAnalysisSpecificationGroup>();
    }
    public class TenderAnalysisSpecificationGroup
    {
        public int SpecRef { get; set; }
        public List<TenderAnalysisRow> Rows { get; set; } = new List<TenderAnalysisRow>();
    }
}
