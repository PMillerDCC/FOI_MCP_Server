using DCC_Standard;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ProspectBusinessLayer;
using System.IO;
using System.Web.Mvc;

namespace Prospect.Controllers
{
    public class SpecificationController : DCC_Controller
    {

        #region SpecificationGrid

        [Authorize]
        public ActionResult SpecificationGrid()
        {
            DCC_Session.Add("Saved_K_Specification", null); // mechanism to tell the tender view where to return to; put -100 for tendergrid; -200 for contractgrid; or K_Specification from the specification view; null to reset

            SpecificationGrid model = new SpecificationGrid(DCC_Session.AutoDatabaseConnection);
            model.Initialise(
                Request,
                id: "SpecificationGrid",
                itemNameSingular: "Specification",
                column1: new DCC_Standard.GridColumn(GridColumnType.Text, "ContractReference", "Specification reference", 100, alignRight: true, boundSortField: "ContractReferenceSort", defaultSortColumn: true, defaultSortDescending: true),
                column2: new DCC_Standard.GridColumn(GridColumnType.Text, "ServiceNoText"    , "Service number"         , 150),
                column3: new DCC_Standard.GridColumn(GridColumnType.Text, "RouteText"        , "Route"                  , 250),
                column4: new DCC_Standard.GridColumn(GridColumnType.Text, "DateStartText"    , "Start date"             , 100, alignRight: true, boundSortField: "DateStartSort"),
                column5: new DCC_Standard.GridColumn(GridColumnType.Text, "DateEndText"      , "End date"               , 100, alignRight: true, boundSortField: "DateEndSort"),
                column6: new DCC_Standard.GridColumn(GridColumnType.Text, "IsBlank"          , "Is dummy"               , 100, trueFalse: "Yes|No"),
                column7: new DCC_Standard.GridColumn(GridColumnType.Text, "IsPackage"        , "Is package"             , 100, trueFalse: "Yes|No"),
                addActionResult: "/Specification/Specification",
                editActionResult: "/Specification/Specification",
                noOfRowsInGrid: 16,
                showEditButton: false,
                showSelectorColumn: false,
                fullRowSelect: true,
                dcc_Session: DCC_Session
                );

            ViewBag.PageTitle = "Specifications";
            return View(model);
        }

        #endregion


        #region Specification ActionResult (Get)

        [Authorize]
        [HttpGet]
        public ActionResult Specification()
        {
            #region Determine which Specification has been selected. First looks in DCC_Session, then the URL.
            int k_Specification = -1;
            object o = DCC_Session.Get("Saved_K_Specification");
            if ((o is int) && ((int)o > 0)) k_Specification = (int)o;
            else
            {
                string queryKey = Request["primaryKey"] ?? "-1";
                if ((int.TryParse(queryKey, out k_Specification)) && (k_Specification == -999))
                {
                    this.DCC_DisplayMessage(DCC_MessageType.Error, "You must select a Specification before clicking Edit", true);
                    return RedirectToAction("Index");
                }
            }
            DCC_Session.Add("Saved_K_Specification", k_Specification); // mechanism to tell the tender view where to return to; put -100 for tendergrid; -200 for contractgrid; or K_Specification from the specification view; null to reset
            DCC_Session.Add("Saved_K_Tender", null);
            #endregion

            Specification model = new Specification(DCC_Session.AutoDatabaseConnection, k_Specification);

            if (model.IsNew)
            {
                model.Save(DCC_Session.AutoDatabaseConnection);
                model = new Specification(DCC_Session.AutoDatabaseConnection, model.K_Specification);
            }

            #region Initialise Tender Grid

            model.TenderGrid.Initialise(
                Request,
                id: "TenderGrid",
                itemNameSingular: "Tender",
                column1: new DCC_Standard.GridColumn(GridColumnType.Text, "TenderReference", "Tender reference", 130, alignRight: true, boundSortField: "TenderReferenceSort"),
                column2: new DCC_Standard.GridColumn(GridColumnType.Text, "ContractorName", "Operator", 200),
                column3: new DCC_Standard.GridColumn(GridColumnType.Text, "ContractorLicenceNo", "Licence number", 130),
                column4: new DCC_Standard.GridColumn(GridColumnType.Text, "IsPackage", "Is part of package", 130, trueFalse: "Yes|No"),
                column5: new DCC_Standard.GridColumn(GridColumnType.Text, "IsTenderAccepted", "Is accepted", 130, trueFalse: "Yes|No"),
                addActionResult: "/Tender/Tender",
                editActionResult: "/Tender/Tender",
                noOfRowsInGrid: 10,
                showEditButton: false,
                showSelectorColumn: false,
                fullRowSelect: true,
                dcc_Session: DCC_Session
                );

            #endregion

            ViewBag.PageTitle = "Specification";
            return View(model);
        }

        #endregion


        #region Specification ActionResult (Post)

        [Authorize]
        [HttpPost]
        public ActionResult Specification(string saveButton, Specification model)
        {
            if (!string.IsNullOrEmpty(saveButton))
            {
                #region Page validation

                if ((model.DateStart.HasValue && model.DateEnd.HasValue) && (model.DateStart > model.DateEnd)) ModelState.AddModelError("DateStart", "The start date cannot be after the end date.");
                if (model.NoOfDaysOfOperationPerYear > 365) ModelState.AddModelError("NoOfDaysOfOperationPerYear", "The total number of days can not be greater than 365.");
                
                #endregion

                #region On save

                if (ModelState.IsValid)
                {
                    if (model.Save(DCC_Session.AutoDatabaseConnection) < 0) this.DCC_DisplayMessage(DCC_MessageType.Error, "A problem occurred while saving");
                    else this.DCC_DisplayMessage(DCC_MessageType.Success, "Your changes have been saved");
                }
                else this.DCC_DisplayMessage(DCC_MessageType.Error, "Some of your changes are invalid");
                
                #endregion
            }

            model.PopulateDropDownLists(DCC_Session.AutoDatabaseConnection);

            #region Initialise Tender Grid

            model.TenderGrid.Initialise(
                Request,
                id: "TenderGrid",
                itemNameSingular: "Tender",
                column1: new DCC_Standard.GridColumn(GridColumnType.Text, "TenderReference"     , "Tender reference"  , 130, alignRight: true, boundSortField: "TenderReferenceSort"),
                column2: new DCC_Standard.GridColumn(GridColumnType.Text, "ContractorName"      , "Operator"          , 200),
                column3: new DCC_Standard.GridColumn(GridColumnType.Text, "ContractorLicenceNo" , "Licence number"    , 130),
                column4: new DCC_Standard.GridColumn(GridColumnType.Text, "IsPackage"           , "Is part of package", 130, trueFalse: "Yes|No"),
                column5: new DCC_Standard.GridColumn(GridColumnType.Text, "IsTenderAccepted"    , "Is accepted"       , 130, trueFalse: "Yes|No", defaultSortColumn: true, defaultSortDescending: true),
                addActionResult: "/Tender/Tender",
                editActionResult: "/Tender/Tender",
                noOfRowsInGrid: 10,
                showEditButton: false,
                showSelectorColumn: false,
                fullRowSelect: true,
                dcc_Session: DCC_Session
                );

            #endregion

            ViewBag.PageTitle = "Specification";
            return View(model);
        }

        #endregion


        #region Create Document

        public FileContentResult ExportToWord(int k_Specification)
        {
            Specification s = new Specification(DCC_Session.AutoDatabaseConnection, k_Specification);

            // Prepare filepath
            string file = "../Template/SpecTemplate.docx";
            string fullFilePAth = HttpContext.ApplicationInstance.Server.MapPath(file);

            // Copy file content to MemoryStreem via byte array
            MemoryStream stream = new MemoryStream();
            byte[] fileBytesArray = System.IO.File.ReadAllBytes(fullFilePAth);
            
            // Copy file to memorystream.
            stream.Write(fileBytesArray, 0, fileBytesArray.Length);
            stream.Position = 0;

            //Edit word document content
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(stream, true))
            {

                Body body = wordDoc.MainDocumentPart.Document.Body;

                foreach (BookmarkStart bookMarkStart in wordDoc.MainDocumentPart.RootElement.Descendants<BookmarkStart>())
                {

                    if (bookMarkStart.Name == "SPECNO")
                    {
                        OpenXmlElement elem = bookMarkStart.NextSibling();

                        while (elem != null && !(elem is BookmarkEnd))
                        {
                            OpenXmlElement nextElem = elem.NextSibling();
                            elem.Remove();
                            elem = nextElem;
                        }

                        Text text = new Text("" + s.ContractReference);
                        Run run = new Run(new RunProperties(new Bold()));
                        run.Append(text);
                        bookMarkStart.InsertAfterSelf(run);

                    }

                    if (bookMarkStart.Name == "SERVICENO")
                    {
                        OpenXmlElement elem = bookMarkStart.NextSibling();

                        while (elem != null && !(elem is BookmarkEnd))
                        {
                            OpenXmlElement nextElem = elem.NextSibling();
                            elem.Remove();
                            elem = nextElem;
                        }

                        Text text = new Text("" + s.SpecificationServiceNo);
                        Run run = new Run(new RunProperties(new Bold()));
                        run.Append(text);
                        bookMarkStart.InsertAfterSelf(run);

                    }

                    if (bookMarkStart.Name == "ROUTESTART")
                    {
                        OpenXmlElement elem = bookMarkStart.NextSibling();

                        while (elem != null && !(elem is BookmarkEnd))
                        {
                            OpenXmlElement nextElem = elem.NextSibling();
                            elem.Remove();
                            elem = nextElem;
                        }

                        Text text = new Text("" + s.RouteStart);
                        Run run = new Run(new RunProperties(new Bold()));
                        run.Append(text);
                        bookMarkStart.InsertAfterSelf(run);

                    }

                    if (bookMarkStart.Name == "ROUTEFINISH")
                    {
                        OpenXmlElement elem = bookMarkStart.NextSibling();

                        while (elem != null && !(elem is BookmarkEnd))
                        {
                            OpenXmlElement nextElem = elem.NextSibling();
                            elem.Remove();
                            elem = nextElem;
                        }

                        Text text = new Text("" + s.RouteEnd);
                        Run run = new Run(new RunProperties(new Bold()));
                        run.Append(text);
                        bookMarkStart.InsertAfterSelf(run);

                    }

                    if (bookMarkStart.Name == "DATESTART")
                    {
                        OpenXmlElement elem = bookMarkStart.NextSibling();

                        while (elem != null && !(elem is BookmarkEnd))
                        {
                            OpenXmlElement nextElem = elem.NextSibling();
                            elem.Remove();
                            elem = nextElem;
                        }

                        Text text = new Text("" + s.DateStartFullText);
                        Run run = new Run(new RunProperties(new Bold()));
                        run.Append(text);
                        bookMarkStart.InsertAfterSelf(run);

                    }


                    if (bookMarkStart.Name == "DATEEND")
                    {
                        OpenXmlElement elem = bookMarkStart.NextSibling();

                        while (elem != null && !(elem is BookmarkEnd))
                        {
                            OpenXmlElement nextElem = elem.NextSibling();
                            elem.Remove();
                            elem = nextElem;
                        }

                        Text text = new Text("" + s.DateEndFullText);
                        Run run = new Run(new RunProperties(new Bold()));
                        run.Append(text);
                        bookMarkStart.InsertAfterSelf(run);

                    }

                    if (bookMarkStart.Name == "VEHCAP")
                    {
                        OpenXmlElement elem = bookMarkStart.NextSibling();

                        while (elem != null && !(elem is BookmarkEnd))
                        {
                            OpenXmlElement nextElem = elem.NextSibling();
                            elem.Remove();
                            elem = nextElem;
                        }

                        Text text = new Text("" + s.VehicleCapacity);
                        Run run = new Run(new RunProperties(new Bold()));
                        run.Append(text);
                        bookMarkStart.InsertAfterSelf(run);

                    }

                    if (bookMarkStart.Name == "MAXVEHDIM")
                    {
                        OpenXmlElement elem = bookMarkStart.NextSibling();

                        while (elem != null && !(elem is BookmarkEnd))
                        {
                            OpenXmlElement nextElem = elem.NextSibling();
                            elem.Remove();
                            elem = nextElem;
                        }

                        Text text = new Text("" + s.VehicleDimensions);
                        Run run = new Run(new RunProperties(new Bold()));
                        run.Append(text);
                        bookMarkStart.InsertAfterSelf(run);

                    }

                    if (bookMarkStart.Name == "ELDERDISAB")
                    {
                        OpenXmlElement elem = bookMarkStart.NextSibling();

                        while (elem != null && !(elem is BookmarkEnd))
                        {
                            OpenXmlElement nextElem = elem.NextSibling();
                            elem.Remove();
                            elem = nextElem;
                        }

                        Text text = new Text("" + s.VehicleElderlyAndDisabled);
                        Run run = new Run(new RunProperties(new Bold()));
                        run.Append(text);
                        bookMarkStart.InsertAfterSelf(run);

                    }

                    if (bookMarkStart.Name == "FARESREV")
                    {
                        OpenXmlElement elem = bookMarkStart.NextSibling();

                        while (elem != null && !(elem is BookmarkEnd))
                        {
                            OpenXmlElement nextElem = elem.NextSibling();
                            elem.Remove();
                            elem = nextElem;
                        }

                        Text text = new Text("" + s.AgreementDescription);
                        Run run = new Run(new RunProperties(new Bold()));
                        run.Append(text);
                        bookMarkStart.InsertAfterSelf(run);

                    }

                    if (bookMarkStart.Name == "JOINTRESP")
                    {
                        OpenXmlElement elem = bookMarkStart.NextSibling();

                        while (elem != null && !(elem is BookmarkEnd))
                        {
                            OpenXmlElement nextElem = elem.NextSibling();
                            elem.Remove();
                            elem = nextElem;
                        }

                        Text text = new Text("" + s.JointResponsibility1);
                        Run run = new Run(new RunProperties(new Bold()));
                        run.Append(text);
                        bookMarkStart.InsertAfterSelf(run);

                    }

                    if (bookMarkStart.Name == "EXCEPTIONALCOND")
                    {
                        OpenXmlElement elem = bookMarkStart.NextSibling();

                        while (elem != null && !(elem is BookmarkEnd))
                        {
                            OpenXmlElement nextElem = elem.NextSibling();
                            elem.Remove();
                            elem = nextElem;
                        }

                        Text text = new Text("" + s.ExceptionalConditions);
                        Run run = new Run(new RunProperties(new Bold()));
                        run.Append(text);
                        bookMarkStart.InsertAfterSelf(run);

                    }
                }
            }

            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "Specification_" + s.ContractReference + ".docx");
        }

        #endregion


		#region AJAX calls

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult CalculateNoDaysInOperation(string dateAndDays)
        {
            
			var result = ProspectBusinessLayer.Specification.CalculateDaysInOperation(DCC_Session.AutoDatabaseConnection, dateAndDays);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

		#endregion



    }

}
