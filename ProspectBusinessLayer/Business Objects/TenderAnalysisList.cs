using DCC_Standard;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProspectBusinessLayer.Business_Objects
{
    public class TenderAnalysisList : IEnumerable<TenderAnalysisRow>
    {
        public List<TenderAnalysisRow> All = new List<TenderAnalysisRow>();

        public TenderAnalysisList(DCC_Database db, int specStart, int specEnd)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sTenderAnalysisBySpecificationRange");
            dbf.AddParameter("SpecStart", specStart);
            dbf.AddParameter("SpecEnd", specEnd);
            dbf.AddTable("");

            db.DataLayer.SelectData(dbf);

            if (dbf.hasTables)
            {
                foreach (DataRow row in dbf.TableZeroRows)
                {
                    var analysisRow = new TenderAnalysisRow
                    {
                        K_Tender = row.Field<int>("K_Tender"),
                        TenderReference = row.Field<int>("TenderReference"),
                        Q_Specification = row.Field<int>("Q_Specification"),
                        SpecRef = row.Field<int>("SpecRef"),
                        ContractorName = row.Field<string>("ContractorName"),
                        TenderPricePerDay = row.Field<decimal?>("TenderPricePerDay") ?? 0,
                        IsConforming = row.Field<bool?>("IsConforming") ?? false,
                        IsTenderAccepted = row.Field<bool?>("IsTenderAccepted") ?? false
                    };

                    analysisRow.ChildSpecs = new List<int>
                    {
                        row.Field<int?>("Child01_SpecRef") ?? 0,
                        row.Field<int?>("Child02_SpecRef") ?? 0,
                        row.Field<int?>("Child03_SpecRef") ?? 0,
                        row.Field<int?>("Child04_SpecRef") ?? 0,
                        row.Field<int?>("Child05_SpecRef") ?? 0,
                        row.Field<int?>("Child06_SpecRef") ?? 0,
                        row.Field<int?>("Child07_SpecRef") ?? 0,
                        row.Field<int?>("Child08_SpecRef") ?? 0,
                        row.Field<int?>("Child09_SpecRef") ?? 0,
                        row.Field<int?>("Child10_SpecRef") ?? 0
                    }
                    .Where(x => x > 0)
                    .ToList();

                    analysisRow.IsPackage = analysisRow.ChildSpecs.Any();

                    All.Add(analysisRow);
                }
            }

            All = All
                .Where(x => x.SpecRef >= specStart && x.SpecRef <= specEnd)
                .ToList();

            var allRows = this.All.Cast<TenderAnalysisRow>().ToList();

            var packageSpecs = allRows
                .Where(r => r.IsPackage)
                .SelectMany(r => r.ChildSpecs)
                .ToHashSet();

            foreach (var row in allRows)
            {
                row.IsPartOfAnotherPackage =
                packageSpecs.Contains(row.SpecRef) &&
                !row.IsPackage;
            }

            All = All
                .OrderByDescending(r => r.SpecRef)
                .ThenBy(r => r.TenderPricePerDay)
                .ToList();

            var groupedBySpecification = All
                .GroupBy(r => r.SpecRef)
                .ToList();

            for (var i = 0; i < groupedBySpecification.Count; i++)
            {
                var groupCssClass = (i % 2 == 0) ? "spec-group-a" : "spec-group-b";

                foreach (var row in groupedBySpecification[i])
                {
                    row.GroupCssClass = groupCssClass;
                }
            }



        }

        public IEnumerator<TenderAnalysisRow> GetEnumerator() => All.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => All.GetEnumerator();
    }
}
