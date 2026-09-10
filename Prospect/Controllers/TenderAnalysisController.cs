using DCC_Standard;
using Org.BouncyCastle.Asn1.Ocsp;
using ProspectBusinessLayer;
using ProspectBusinessLayer.Business_Objects;
using System;
using System.Collections.Generic;
using System.Data.Metadata.Edm;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prospect.Controllers
{
    public class TenderAnalysisController : DCC_Controller
    {
        private const int DefaultSpecStart = 1;
        private const int DefaultSpecEnd = 999999;

        [Authorize]
        [HttpGet]
        public ActionResult Index(int? specStart = null, int? specEnd = null)
        {
            var db = DCC_Session.AutoDatabaseConnection;
            var range = NormaliseSpecificationRange(specStart, specEnd);

            TenderAnalysisGrid model = new TenderAnalysisGrid(
                DCC_Session.AutoDatabaseConnection,
                range.specStart,
                range.specEnd
            );

            model.Initialise(
                Request,
                id: "TenderAnalysisGrid",
                itemNameSingular: null,
                itemNamePlural: "Tenders",
                column1: new GridColumn(GridColumnType.Text, "TenderReference", "Tender Reference", 120, boundLink: "TenderLink") { LinkController = "" },
                column2: new GridColumn(GridColumnType.Text, "SpecRef", "Specification Reference", 120),
                column3: new GridColumn(GridColumnType.Text, "ContractorName", "Operator", 200),
                column4: new GridColumn(GridColumnType.Text, "IsConforming", "Conforming", 120, trueFalse: "Yes|No"),
                column5: new GridColumn(GridColumnType.Text, "IsPackage", "Is Package", 120, trueFalse: "Yes|No"),
                column6: new GridColumn(GridColumnType.Text, "IsPartOfAnotherPackage", "Part of Another Package", 160, trueFalse: "Yes|No"),
                column7: new GridColumn(GridColumnType.Text, "TenderPricePerDayFormatted", "Price Per Day", 120, alignRight: true),
                column8: new GridColumn(GridColumnType.Text, "AcceptTenderButton", "Accept Tender", 140),

                addActionResult: "/Tender/Tender",
                editActionResult: "/Tender/Tender",
              
                noOfRowsInGrid: 16,
                showEditButton: false,
                showSelectorColumn: false,
                fullRowSelect: false,
                dcc_Session: DCC_Session
            );

            ViewBag.SpecStart = range.specStart;
            ViewBag.SpecEnd = range.specEnd;
            ViewBag.PageTitle = "Tender Analysis";

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Print(int? specStart = null, int? specEnd = null)
        {
            var db = DCC_Session.AutoDatabaseConnection;
            var range = NormaliseSpecificationRange(specStart, specEnd);
            var list = new TenderAnalysisList(db, range.specStart, range.specEnd);
            var model = new TenderAnalysisPrintModel
            {
                SpecStart = range.specStart,
                SpecEnd = range.specEnd,
                Groups = list.All
                    .GroupBy(x => x.SpecRef)
                    .OrderBy(g => g.Key)
                    .Select(g => new TenderAnalysisSpecificationGroup
                    {
                        SpecRef = g.Key,
                        Rows = g.OrderBy(x => x.TenderPricePerDay).ToList()
                    })
                    .ToList()
            };

            return View("TenderAnalysisPrint", model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AcceptTender(int primaryKey, int? specStart = null, int? specEnd = null)
        {
            var range = NormaliseSpecificationRange(specStart, specEnd);

            try
            {
                var db = DCC_Session.AutoDatabaseConnection;
                Tender tender = new Tender(db, primaryKey);

                // Prevent duplicate acceptance
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sAcceptedTenderExistsForSpecification");
                dbf.AddParameter("Q_Specification", tender.Q_Specification);
                dbf.AddParameter("K_Tender", tender.K_Tender);
                dbf.AddTable("");
                db.DataLayer.SelectData(dbf);

                if (dbf.hasTables && dbf.TableZeroRows.Count > 0)
                {
                    this.DCC_DisplayMessage(
                        DCC_MessageType.Error,
                        "A tender has already been accepted for this specification.",
                        true
                    );

                    return Index(range.specStart, range.specEnd);
                }

                // Accept tender
                tender.IsTenderAccepted = true;
                tender.Save(db, "Tender");

                this.DCC_DisplayMessage(
                    DCC_MessageType.Success,
                    "Tender accepted successfully.",
                    true
                );
            }
            catch (Exception ex)
            {
                this.DCC_DisplayMessage(
                    DCC_MessageType.Error,
                    "Failed to accept tender: " + ex.Message,
                    true
                );
            }

            var indexResult = Index(range.specStart, range.specEnd) as ViewResult;
            return View("Index", indexResult.Model);
        }

        private (int specStart, int specEnd) NormaliseSpecificationRange(int? specStart, int? specEnd)
        {
            var normalisedSpecStart = specStart.GetValueOrDefault(DefaultSpecStart);
            var normalisedSpecEnd = specEnd.GetValueOrDefault(DefaultSpecEnd);

            if (normalisedSpecStart < 0)
                normalisedSpecStart = DefaultSpecStart;

            if (normalisedSpecEnd < 0)
                normalisedSpecEnd = DefaultSpecEnd;

            if (normalisedSpecStart > normalisedSpecEnd)
            {
                var temp = normalisedSpecStart;
                normalisedSpecStart = normalisedSpecEnd;
                normalisedSpecEnd = temp;
            }

            return (normalisedSpecStart, normalisedSpecEnd);
        }
    }
}