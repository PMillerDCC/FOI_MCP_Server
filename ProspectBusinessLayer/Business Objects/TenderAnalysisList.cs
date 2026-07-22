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
                    All.Add(new TenderAnalysisRow
                    {
                        K_Tender = row.Field<int>("K_Tender"),
                        TenderReference = row.Field<int>("TenderReference"),
                        Q_Specification = row.Field<int>("Q_Specification"),
                        SpecRef = row.Field<int>("SpecRef"),
                        ContractorName = row.Field<string>("ContractorName"),
                        TenderPricePerDay = row.Field<decimal?>("TenderPricePerDay") ?? 0,
                        IsConforming = row.Field<bool?>("IsConforming") ?? false,
                        IsTenderAccepted = row.Field<bool?>("IsTenderAccepted") ?? false
                    });
                }
            }
        }

        public IEnumerator<TenderAnalysisRow> GetEnumerator() => All.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => All.GetEnumerator();
    }
}
