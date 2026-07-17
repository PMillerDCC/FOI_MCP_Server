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
        public ActionResult Index(int specStart = 1, int specEnd = 999999)
        {
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
                column5: new GridColumn(GridColumnType.Text, "TenderPricePerDay", "Price Per Day", 120, alignRight: true),
                column6: new GridColumn(GridColumnType.Text, "AcceptTenderButton", "Accept Tender", 140),

                addActionResult: null,
                editActionResult: null,
              
                noOfRowsInGrid: 16,
                showEditButton: false,
                showSelectorColumn: false,
                fullRowSelect: false,
                dcc_Session: DCC_Session
            );

            return View(model);
        }

        //POST: TenderAnalysis
        [HttpPost]
        public ActionResult Save(FormCollection form, int specStart, int specEnd)
        {
            var model = new TenderAnalysisGrid(DCC_Session.AutoDatabaseConnection, specStart, specEnd);
            model.Initialise(Request);

            // model.Save(form);
            return RedirectToAction("Index", new { specStart, specEnd });
        }




        [Authorize]
        [HttpPost]
        public ActionResult AcceptTender(int primaryKey)
        {
            try
            {
                var db = DCC_Session.AutoDatabaseConnection;
                Tender tender = new Tender(db, primaryKey);
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