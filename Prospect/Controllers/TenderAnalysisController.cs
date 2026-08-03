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
        [Authorize]
        [HttpGet]
        public ActionResult Index(int specStart = 1, int specEnd = 999999)
        {
            var db = DCC_Session.AutoDatabaseConnection;

            TenderAnalysisGrid model = new TenderAnalysisGrid(
                DCC_Session.AutoDatabaseConnection,
                specStart,
                specEnd
            );

            model.Initialise(
                Request,
                id: "TenderAnalysisGrid",
                itemNameSingular: null,
                itemNamePlural: "Tenders",
                column1: new GridColumn(GridColumnType.Text, "TenderReference", "Tender Reference", 120),
                column2: new GridColumn(GridColumnType.Text, "SpecRef", "Specification Reference", 120),
                column3: new GridColumn(GridColumnType.Text, "ContractorName", "Operator", 200),
                column4: new GridColumn(GridColumnType.Text, "IsConforming", "Conforming", 120, trueFalse: "Yes|No"),
                column5: new GridColumn(GridColumnType.Text, "IsPackage", "Is Package", 120, trueFalse: "Yes|No"),
                column6: new GridColumn(GridColumnType.Text, "IsPartOfAnotherPackage", "Part of Another Package", 160, trueFalse: "Yes|No"),
                column7: new GridColumn(GridColumnType.Text, "TenderPricePerDayFormatted", "Price Per Day", 120, alignRight: true),
                column8: new GridColumn(GridColumnType.Text, "AcceptTenderButton", "Accept Tender", 140),

                addActionResult: null,
                editActionResult: null,
              
                noOfRowsInGrid: 16,
                showEditButton: false,
                showSelectorColumn: false,
                fullRowSelect: false,
                dcc_Session: DCC_Session
            );

            ViewBag.SpecStart = specStart;
            ViewBag.SpecEnd = specEnd;
            ViewBag.PageTitle = "Tender Analysis";

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Print(int specStart = 1, int specEnd = 999999)
        {
            var db = DCC_Session.AutoDatabaseConnection;
            var list = new TenderAnalysisList(db, specStart, specEnd);
            var model = new TenderAnalysisPrintModel
            {
                SpecStart = specStart,
                SpecEnd = specEnd,
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
        public ActionResult AcceptTender(int primaryKey)
        {
            try
            {
                var db = DCC_Session.AutoDatabaseConnection;
                Tender tender = new Tender(db, primaryKey);
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sAcceptedTenderExistsForSpecification");
                dbf.AddParameter("Q_Specification", tender.Q_Specification);
                dbf.AddParameter("K_Tender", tender.K_Tender);
                dbf.AddTable("");
                db.DataLayer.SelectData(dbf);
                if (dbf.hasTables && dbf.TableZeroRows.Count > 0)
                {
                    this.DCC_DisplayMessage(DCC_MessageType.Error, "A tender has already been accepted for this specification.", true);
                    return RedirectToAction("Index");
                }
                tender.IsTenderAccepted = true;
                tender.Save(db, "Tender");
                this.DCC_DisplayMessage(DCC_MessageType.Success, "Tender accepted successfully.", true);
            }
            catch (Exception ex)
            {
                this.DCC_DisplayMessage(DCC_MessageType.Error, "Failed to accept tender: " + ex.Message, true);
            }
            return RedirectToAction("Index");
        }
    }
}