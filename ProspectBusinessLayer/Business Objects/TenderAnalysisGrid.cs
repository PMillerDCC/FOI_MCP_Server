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

            var rows = list.All.Cast<TenderAnalysisRow>().ToList();

            var sorted = rows
                .GroupBy(r => r.SpecRef)
                .SelectMany(g => g.OrderBy(r => r.TenderPricePerDay))
                .ToList();

            var filtered = new List<TenderAnalysisRow>();

            foreach (var group in sorted.GroupBy(r => r.SpecRef))
            {
                var accepted = group.FirstOrDefault(r => r.IsTenderAccepted);

                if (accepted != null)
                {
                    filtered.Add(accepted);
                }
                else
                {
                    filtered.AddRange(group);
                }
            }

            TotalRows = filtered.Cast<object>().ToList();
            Count = CountFiltered = TotalRows.Count;
        }
    }
}
