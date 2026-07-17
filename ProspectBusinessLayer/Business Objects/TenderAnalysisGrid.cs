using DCC_Standard;
using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProspectBusinessLayer.Business_Objects
{
    public class TenderAnalysisGrid : DCC_Grid
    {
        public TenderAnalysisGrid(DCC_Database db, int specStart, int specEnd)
        {
            TenderAnalysisList list = new TenderAnalysisList(db, specStart, specEnd);
            TotalRows = list.All.Cast<object>().ToList();
            Count = CountFiltered = TotalRows.Count;
        }
    }
}
