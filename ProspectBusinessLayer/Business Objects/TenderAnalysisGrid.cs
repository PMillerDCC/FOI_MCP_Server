using DCC_Standard;
using System.Collections.Generic;
using System.Linq;

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

        public override string GetRowCssClass(object rowObject)
        {
            if (rowObject is TenderAnalysisRow r)
                return r.GroupCssClass;

            return base.GetRowCssClass(rowObject);
        }
    }
}
