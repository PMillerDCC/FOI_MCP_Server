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
                        
                        FuelElementApplies = row.Field<bool>("FuelElementApplies"),

                        IsConforming = row.Field<bool?>("IsConforming") ?? false,
                        IsTenderAccepted = row.Field<bool?>("IsTenderAccepted") ?? false
                    };

                    var childSpecificationIds = new int?[]
                    {
                        row.Field<int?>("Child01_SpecRef"),
                        row.Field<int?>("Child02_SpecRef"),
                        row.Field<int?>("Child03_SpecRef"),
                        row.Field<int?>("Child04_SpecRef"),
                        row.Field<int?>("Child05_SpecRef"),
                        row.Field<int?>("Child06_SpecRef"),
                        row.Field<int?>("Child07_SpecRef"),
                        row.Field<int?>("Child08_SpecRef"),
                        row.Field<int?>("Child09_SpecRef"),
                        row.Field<int?>("Child10_SpecRef")
                    };

                   analysisRow.ChildSpecs = childSpecificationIds
                        .Where(x => x.HasValue && x.Value > 0)
                        .Select(contractRef => contractRef.Value)
                        .Distinct()
                        .ToList();

                    analysisRow.IsPackage = analysisRow.ChildSpecs.Any();

                    All.Add(analysisRow);
                }
            }

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

            var acceptedRows = allRows
                .Where(r => r.IsTenderAccepted)
                .ToList();

            var specRefsToRemove = new HashSet<int>();

            foreach (var acceptedRow in acceptedRows)
            {
                specRefsToRemove.Add(acceptedRow.SpecRef);

                foreach (var childSpecRef in acceptedRow.ChildSpecs)
                {
                    specRefsToRemove.Add(childSpecRef);
                }

                var parentPackageRows = allRows
                    .Where(r => r.IsPackage && r.ChildSpecs.Contains(acceptedRow.SpecRef))
                    .ToList();

                foreach (var parentPackageRow in parentPackageRows)
                {
                    specRefsToRemove.Add(parentPackageRow.SpecRef);

                    foreach (var childSpecRef in parentPackageRow.ChildSpecs)
                    {
                        specRefsToRemove.Add(childSpecRef);
                    }
                }
            }

            All = allRows
                .Where(r =>
                    !specRefsToRemove.Contains(r.SpecRef) &&
                    r.SpecRef >= specStart &&
                    r.SpecRef <= specEnd)
                .ToList();

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
