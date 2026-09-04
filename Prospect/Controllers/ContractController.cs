using DCC_Standard;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ProspectBusinessLayer;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using A = DocumentFormat.OpenXml.Drawing;
using GridColumn = DCC_Standard.GridColumn;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;

namespace Prospect.Controllers
{
    public class ContractController : DCC_Controller
    {

        #region ContractGrid

        [Authorize]
        public ActionResult ContractGrid()
        {
            TenderGrid model = new TenderGrid(DCC_Session.AutoDatabaseConnection, awardedOnly: true);
            model.Initialise(
                Request,
                id: "ContractGrid",
                itemNameSingular: "Contract",
                column1: new GridColumn(GridColumnType.Text, "ContractReference"         , "Contract reference",  90, alignRight: true, boundSortField: "ContractReferenceSort", defaultSortColumn: true, defaultSortDescending: true),
                column2: new GridColumn(GridColumnType.Text, "TenderReference"           , "Tender reference"  ,  90, alignRight: true, boundSortField: "TenderReferenceSort"),
                column3: new GridColumn(GridColumnType.Text, "ContractorName"            , "Operator"          , 250),
                column4: new GridColumn(GridColumnType.Text, "ContractorLicenceNo"       , "Licence number"    ,  90),
                column5: new GridColumn(GridColumnType.Text, "SpecificationServiceNoText", "Service number"    , 180),
                column6: new GridColumn(GridColumnType.Text, "RouteText"                 , "Route"             , 290),
                column7: new GridColumn(GridColumnType.Text, "IsContractCancelled"       , "Is cancelled?"     , 100, trueFalse: "Yes|No"),
                addActionResult: "/Contract/Contract",
                editActionResult: "/Contract/Contract",
                noOfRowsInGrid: 16,
                showEditButton: false,
                showSelectorColumn: false,
                fullRowSelect: true,
                dcc_Session: DCC_Session
                );

            ViewBag.PageTitle = "Contracts";
            return View(model);
        }

        #endregion


        #region Contract ActionResult (Get)

        [Authorize]
        [HttpGet]
        public ActionResult Contract(bool editVariation = false)
        {
            ViewBag.EditMode = false;
            #region Determine which Contract has been selected. First looks in DCC_Session, then the URL.
            int k_Tender = -1;
            string queryKey = Request["primaryKey"] ?? "-1";
            if (int.TryParse(queryKey, out k_Tender) && (k_Tender == -999))
            {
                this.DCC_DisplayMessage(DCC_MessageType.Error, "You must select a Tender before clicking Edit", true);
                return RedirectToAction("Index");
            }
            #endregion

            Tender model = new Tender(DCC_Session.AutoDatabaseConnection, k_Tender) ;

            InitialiseContractVariationGrid(model);

            ContractVariation latestVariation = null;

            if (model.ContractVariationGrid != null && model.ContractVariationGrid.GridRows != null)
            {
                var latestRow = model.ContractVariationGrid.GridRows
                    .OfType<ContractVariation>()
                    .OrderByDescending(x => x.ContractVariationNo)
                    .FirstOrDefault();

                if (latestRow != null)
                {
                    var db = DCC_Session.AutoDatabaseConnection;
                    latestVariation = new ContractVariation(db, pK_ContractVariation: latestRow.K_ContractVariation);

                    // Always use latest saved variation as the base data for the form
                    MapTenderFromVariation(model, latestVariation);

                    model.AgreementDescription = latestVariation.AgreementDescription;
                }
            }

            if (!editVariation)
            {
                model.ContractVariationVariationDateStart = null;
                model.ContractVariationVariationDateEnd = null;
            }

            if (editVariation && latestVariation != null)
            {
                ViewBag.EditMode = true;
                ViewBag.EditingVariationId = latestVariation.K_ContractVariation;
            }

            ViewBag.NextVariationStart = GetNextVariationStartDate(model);
            ViewBag.ReturnTenderId = model.K_Tender;
            ViewBag.PageTitle = "Contract";
            return View(model);
        }

        #endregion


        #region Contract ActionResult (Post)

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contract(
            string saveButton,
            string createButton,
            string recycleButton,
            string editButton,
            Tender model,
            int? editingVariationId)
        {
            var db = DCC_Session.AutoDatabaseConnection;

            // Always repopulate anything the view needs if we have to return it
            model.PopulateDropDownLists(db, model.K_Tender);

            InitialiseContractVariationGrid(model);

            // EDIT EXISTING VARIATION
            if (!string.IsNullOrEmpty(editButton))
            {
                if (!editingVariationId.HasValue || editingVariationId.Value <= 0)
                {
                    ModelState.AddModelError("", "No variation was selected for editing.");
                }

                if (ModelState.IsValid)
                {
                    var cv = new ContractVariation(db, pK_ContractVariation: editingVariationId.Value);
                    MapVariationFieldsFromTender(cv, model);
                    cv.Save(db);

                    this.DCC_DisplayMessage(DCC_MessageType.Success, "Variation updated");
                    return RedirectToAction("Contract", new { primaryKey = model.K_Tender });
                }

                ViewBag.EditMode = true;
                ViewBag.EditingVariationId = editingVariationId ?? -1;
                ViewBag.ReturnTenderId = model.K_Tender;
                ViewBag.NextVariationStart = GetNextVariationStartDate(model);
                ViewBag.PageTitle = "Contract";
                return View(model);
            }

            // CREATE NEW VARIATION
            if (!string.IsNullOrEmpty(createButton))
            {
                if (!model.ContractVariationVariationDateStart.HasValue)
                {
                    ModelState.AddModelError("ContractVariationVariationDateStart", "Start date is required.");
                }

                if (ModelState.IsValid)
                {
                    var tenderFromDb = new Tender(db, model.K_Tender);
                    InitialiseContractVariationGrid(tenderFromDb);

                    var previousVariationRow = tenderFromDb.ContractVariationGrid.GridRows
                        .OfType<ContractVariation>()
                        .OrderByDescending(x => x.ContractVariationNo)
                        .FirstOrDefault();

                    if (previousVariationRow == null)
                    {
                        ModelState.AddModelError("", "No previous variation was found to base the new variation on.");
                    }
                    else
                    {
                        var previousVariation = new ContractVariation(
                            db,
                            pK_ContractVariation: previousVariationRow.K_ContractVariation
                        );

                        var nextVariationNo = previousVariation.ContractVariationNo + 1;

                        // Create the new variation record
                        ContractVariation.CreateNewVariation(
                            db,
                            previousVariation,
                            model.ContractVariationVariationDateStart.Value,
                            nextVariationNo
                        );

                        // Reload the tender and find the newly created variation
                        var refreshedTender = new Tender(db, model.K_Tender);
                        InitialiseContractVariationGrid(refreshedTender);

                        var newVariation = refreshedTender.ContractVariationGrid.GridRows
                            .OfType<ContractVariation>()
                            .OrderByDescending(x => x.ContractVariationNo)
                            .FirstOrDefault(x => x.ContractVariationNo == nextVariationNo);

                        if (newVariation == null)
                        {
                            ModelState.AddModelError("", "The new variation was created but could not be reloaded.");
                        }
                        else
                        {
                            var cv = new ContractVariation(db, pK_ContractVariation: newVariation.K_ContractVariation);

                            MapVariationFieldsFromTender(cv, model);
                            cv.Save(db);

                            this.DCC_DisplayMessage(DCC_MessageType.Success, "New variation created");
                            return RedirectToAction("Contract", new { primaryKey = model.K_Tender });
                        }
                    }
                }

                ViewBag.EditMode = false;
                ViewBag.EditingVariationId = -1;
                ViewBag.ReturnTenderId = model.K_Tender;
                ViewBag.NextVariationStart = GetNextVariationStartDate(model);
                ViewBag.PageTitle = "Contract";
                return View(model);
            }

            // DEFAULT RETURN
            ViewBag.EditMode = false;
            ViewBag.EditingVariationId = editingVariationId ?? -1;
            ViewBag.ReturnTenderId = model.K_Tender;
            ViewBag.NextVariationStart = GetNextVariationStartDate(model);
            ViewBag.PageTitle = "Contract";
            return View(model);
        }

        #endregion

        private void MapVariationFieldsFromTender(ContractVariation cv, Tender model)
        {
            cv.ContractVariationIsCancelled = model.ContractVariationIsCancelled;
            cv.ContractVariationIsExtended = model.ContractVariationIsExtended;
            cv.ContractVariationIsOther = model.ContractVariationIsOther;
            cv.ContractVariationOtherReason = model.ContractVariationOtherReason;

            cv.ContractVariationVariationDateStart = model.ContractVariationVariationDateStart;
            cv.ContractVariationVariationDateEnd = model.ContractVariationVariationDateEnd;

            cv.ContractVariationServiceNo = model.ContractVariationServiceNo;
            cv.ContractVariationRouteStart = model.ContractVariationRouteStart;
            cv.ContractVariationRouteEnd = model.ContractVariationRouteEnd;

            cv.ContractVariationExceptionalConditions = model.ContractVariationExceptionalConditions;
            cv.ContractVariationVehicleCapacity = model.ContractVariationVehicleCapacity;
            cv.ContractVariationVehicleElderlyAndDisabled = model.ContractVariationVehicleElderlyAndDisabled;
            cv.ContractVariationVehicleDimensions = model.ContractVariationVehicleDimensions;

            cv.ContractVariation_Q_Agreement = model.ContractVariation_Q_Agreement;
            
            cv.ContractVariationServiceDetails = model.ContractVariationServiceDetails;
            cv.ContractVariationTicketNotes = model.ContractVariationTicketNotes;
            cv.ContractVariationRouteTimetableNotes = model.ContractVariationRouteTimetableNotes;

            cv.ContractVariationOperatesOnMon = model.ContractVariationOperatesOnMon;
            cv.ContractVariationOperatesOnTue = model.ContractVariationOperatesOnTue;
            cv.ContractVariationOperatesOnWed = model.ContractVariationOperatesOnWed;
            cv.ContractVariationOperatesOnThu = model.ContractVariationOperatesOnThu;
            cv.ContractVariationOperatesOnFri = model.ContractVariationOperatesOnFri;
            cv.ContractVariationOperatesOnSat = model.ContractVariationOperatesOnSat;
            cv.ContractVariationOperatesOnSun = model.ContractVariationOperatesOnSun;

            cv.ContractVariationOperatesOnSchoolDays = model.ContractVariationOperatesOnSchoolDays;
            cv.ContractVariationOperatesOnNonSchoolDays = model.ContractVariationOperatesOnNonSchoolDays;
            cv.ContractVariationOperatesOnBankHolidays = model.ContractVariationOperatesOnBankHolidays;
            cv.ContractVariationOperatesOnChristmasAndNewYear = model.ContractVariationOperatesOnChristmasAndNewYear;

            cv.ContractVariationCalculatedNoOfDaysOfOperationFirstYear = model.ContractVariationCalculatedNoOfDaysOfOperationFirstYear;
            cv.ContractVariationCalculatedNoOfDaysOfOperationFullTerm = model.ContractVariationCalculatedNoOfDaysOfOperationFullTerm;

            cv.ContractVariationAverageMilesPerDay = model.ContractVariationAverageMilesPerDay;
            cv.ContractVariationRatePerMile = model.ContractVariationRatePerMile;
            cv.ContractVariationFuelPriceElement = model.ContractVariationFuelPriceElement;

            cv.ContractVariationAveragePricePerDay = model.ContractVariationAveragePricePerDay;
            cv.ContractVariationAveragePricePerYear = model.ContractVariationAveragePricePerYear;
            cv.ContractVariationAverageRevenuePerDay = model.ContractVariationAverageRevenuePerDay;
            cv.ContractVariationAverageRevenuePerYear = model.ContractVariationAverageRevenuePerYear;
            cv.ContractVariationAverageCostPerDay = model.ContractVariationAverageCostPerDay;
            cv.ContractVariationAverageCostPerYear = model.ContractVariationAverageCostPerYear;

            cv.ContractVariationNotes = model.ContractVariationNotes;

            cv.ContractVariationReasonForEdit = model.ContractVariationReasonForEdit;
        }

        private void InitialiseContractVariationGrid(Tender model)
        {
            if (!model.IsTenderAccepted) return;

            model.ContractVariationGrid.Initialise(
                Request,
                id: "ContractVariationsGrid",
                itemNameSingular: "Contract variation",
                column1: new GridColumn(GridColumnType.Text, "ContractVariationNo", "Variation Number", 90, alignRight: true, boundSortField: "ContractVariationNo", defaultSortColumn: true, defaultSortDescending: true),
                column2: new GridColumn(GridColumnType.Text, "ContractVariationDateStartText", "Variation Start Date", 160),
                column3: new GridColumn(GridColumnType.Text, "IgnoreIsPriceChange", "Price Changed?", 160),
                column4: new GridColumn(GridColumnType.Text, "IgnoreAgreementChanged", "Fares Agreement Changed?", 200),
                column5: new GridColumn(GridColumnType.Text, "IgnoreNoOfDaysOfOperationPerYearChanged", "Days in Operation Changed?", 200),
                addButtonText: "Add New Contract Variation",
                addActionResult: "/Contract/Contract?primaryKey=" + model.K_Tender,
                editActionResult: "/Contract/Contract",
                noOfRowsInGrid: 16,
                showEditButton: false,
                showSelectorColumn: false,
                fullRowSelect: true,
                dcc_Session: DCC_Session
            );
        }

        private void MapTenderFromVariation(Tender model, ContractVariation cv)
        {
            model.ContractVariationIsCancelled = cv.ContractVariationIsCancelled;
            model.ContractVariationIsExtended = cv.ContractVariationIsExtended;
            model.ContractVariationIsOther = cv.ContractVariationIsOther;
            model.ContractVariationOtherReason = cv.ContractVariationOtherReason;

            model.ContractVariationVariationDateStart = cv.ContractVariationVariationDateStart;
            model.ContractVariationVariationDateEnd = cv.ContractVariationVariationDateEnd;

            model.ContractVariationServiceNo = cv.ContractVariationServiceNo;
            model.ContractVariationRouteStart = cv.ContractVariationRouteStart;
            model.ContractVariationRouteEnd = cv.ContractVariationRouteEnd;

            model.ContractVariationExceptionalConditions = cv.ContractVariationExceptionalConditions;
            model.ContractVariationVehicleCapacity = cv.ContractVariationVehicleCapacity;
            model.ContractVariationVehicleElderlyAndDisabled = cv.ContractVariationVehicleElderlyAndDisabled;
            model.ContractVariationVehicleDimensions = cv.ContractVariationVehicleDimensions;

            model.ContractVariation_Q_Agreement = cv.ContractVariation_Q_Agreement;

            model.ContractVariationServiceDetails = cv.ContractVariationServiceDetails;
            model.ContractVariationTicketNotes = cv.ContractVariationTicketNotes;
            model.ContractVariationRouteTimetableNotes = cv.ContractVariationRouteTimetableNotes;

            model.ContractVariationOperatesOnMon = cv.ContractVariationOperatesOnMon;
            model.ContractVariationOperatesOnTue = cv.ContractVariationOperatesOnTue;
            model.ContractVariationOperatesOnWed = cv.ContractVariationOperatesOnWed;
            model.ContractVariationOperatesOnThu = cv.ContractVariationOperatesOnThu;
            model.ContractVariationOperatesOnFri = cv.ContractVariationOperatesOnFri;
            model.ContractVariationOperatesOnSat = cv.ContractVariationOperatesOnSat;
            model.ContractVariationOperatesOnSun = cv.ContractVariationOperatesOnSun;

            model.ContractVariationOperatesOnSchoolDays = cv.ContractVariationOperatesOnSchoolDays;
            model.ContractVariationOperatesOnNonSchoolDays = cv.ContractVariationOperatesOnNonSchoolDays;
            model.ContractVariationOperatesOnBankHolidays = cv.ContractVariationOperatesOnBankHolidays;
            model.ContractVariationOperatesOnChristmasAndNewYear = cv.ContractVariationOperatesOnChristmasAndNewYear;

            model.ContractVariationCalculatedNoOfDaysOfOperationFirstYear = cv.ContractVariationCalculatedNoOfDaysOfOperationFirstYear;
            model.ContractVariationCalculatedNoOfDaysOfOperationFullTerm = cv.ContractVariationCalculatedNoOfDaysOfOperationFullTerm;

            model.ContractVariationAverageMilesPerDay = cv.ContractVariationAverageMilesPerDay;
            model.ContractVariationRatePerMile = cv.ContractVariationRatePerMile;
            model.ContractVariationFuelPriceElement = cv.ContractVariationFuelPriceElement;

            model.ContractVariationAveragePricePerDay = cv.ContractVariationAveragePricePerDay;
            model.ContractVariationAveragePricePerYear = cv.ContractVariationAveragePricePerYear;
            model.ContractVariationAverageRevenuePerDay = cv.ContractVariationAverageRevenuePerDay;
            model.ContractVariationAverageRevenuePerYear = cv.ContractVariationAverageRevenuePerYear;
            model.ContractVariationAverageCostPerDay = cv.ContractVariationAverageCostPerDay;
            model.ContractVariationAverageCostPerYear = cv.ContractVariationAverageCostPerYear;

            model.ContractVariationNotes = cv.ContractVariationNotes;

            model.ContractVariationReasonForEdit = cv.ContractVariationReasonForEdit;
        }

        private DateTime? GetNextVariationStartDate(Tender model)
        {
            DateTime? lastEnd = model.ContractVariationDateEnd ?? model.DateEnd;

            if (model.IsTenderAccepted && model.ContractVariationGrid?.GridRows != null)
            {
                DateTime? best = null;
                foreach (var row in model.ContractVariationGrid.GridRows)
                {
                    var cv = row as ProspectBusinessLayer.ContractVariation;
                    if (cv == null) continue;

                    var end = cv.ContractVariationVariationDateEnd ?? cv.ContractVariationDateEnd;
                    if (end.HasValue && (!best.HasValue || end.Value > best.Value))
                        best = end;
                }
                if (best.HasValue) lastEnd = best;
            }

            return lastEnd?.AddDays(1);
        }

        [Authorize]
        public FileContentResult ExportContractVariationToWord(int k_ContractVariation)
        {
            var variation = new ContractVariation(DCC_Session.AutoDatabaseConnection, pK_ContractVariation: k_ContractVariation);
            var tender = new Tender(DCC_Session.AutoDatabaseConnection, variation.ContractVariation_Q_Tender);

            using (var stream = new MemoryStream())
            {
                using (var wordDoc = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document, true))
                {
                    var mainPart = wordDoc.AddMainDocumentPart();
                    mainPart.Document = new Document(new Body());

                    var body = mainPart.Document.Body;

                    AddCouncilLogo(wordDoc, mainPart, body, HttpContext.Server.MapPath("~/Template/SpecTemplate.docx"));
                    AddHeading(body, $"Contract Variation {variation.ContractVariationNoText}", "28");
                    AddField(body, "Contract reference", variation.ContractReferenceText);
                    AddField(body, "Tender reference", variation.TenderReferenceText);
                    AddField(body, "Operator", FirstPopulated(variation.ContractorName, tender.ContractorName));
                    AddField(body, "Licence number", FirstPopulated(variation.ContractorLicenceNo, tender.ContractorLicenceNo));
                    AddField(body, "Contract type", FirstPopulated(variation.ContractTypeDescription, tender.ContractTypeDescription));

                    AddHeading(body, "Service details", "24");
                    AddField(body, "Reason for variation", variation.ContractVariationReasonText);
                    AddField(body, "Variation start date", variation.ContractVariationVariationDateStartText);
                    AddField(body, "Variation end date", variation.ContractVariationVariationDateEndText);
                    AddField(body, "Service number", variation.ContractVariationServiceNo);
                    AddField(body, "Route", variation.ContractVariationRouteText);
                    AddField(body, "Exceptional conditions", variation.ContractVariationExceptionalConditions);
                    AddField(body, "Service details", variation.ContractVariationServiceDetails);
                    AddField(body, "Ticket notes", variation.ContractVariationTicketNotes);
                    AddField(body, "Route timetable notes", variation.ContractVariationRouteTimetableNotes);

                    AddHeading(body, "Quality of service", "24");
                    AddField(body, "Vehicle capacity", variation.ContractVariationVehicleCapacity);
                    AddField(body, "Accessibility", variation.ContractVariationVehicleElderlyAndDisabled);
                    AddField(body, "Vehicle dimensions", variation.ContractVariationVehicleDimensions);

                    AddHeading(body, "Agreement", "24");
                    AddField(body, "Agreement for fares", variation.AgreementDescription);

                    AddHeading(body, "Days of operation", "24");
                    AddField(body, "Operates on Monday", variation.ContractVariationOperatesOnMonText);
                    AddField(body, "Operates on Tuesday", variation.ContractVariationOperatesOnTueText);
                    AddField(body, "Operates on Wednesday", variation.ContractVariationOperatesOnWedText);
                    AddField(body, "Operates on Thursday", variation.ContractVariationOperatesOnThuText);
                    AddField(body, "Operates on Friday", variation.ContractVariationOperatesOnFriText);
                    AddField(body, "Operates on Saturday", variation.ContractVariationOperatesOnSatText);
                    AddField(body, "Operates on Sunday", variation.ContractVariationOperatesOnSunText);
                    AddField(body, "Operates on bank holidays", variation.ContractVariationOperatesOnBankHolidaysText);
                    AddField(body, "Operates on school days", variation.ContractVariationOperatesOnSchoolDaysText);
                    AddField(body, "Operates on non school days", variation.ContractVariationOperatesOnNonSchoolDaysText);
                    AddField(body, "Operates between Christmas and New Year", variation.ContractVariationOperatesOnChristmasAndNewYearText);
                    AddField(body, "Days of operation (first year)", variation.ContractVariationCalculatedNoOfDaysOfOperationFirstYear.ToString());
                    AddField(body, "Days of operation (full term)", variation.ContractVariationCalculatedNoOfDaysOfOperationFullTerm.ToString());

                    AddHeading(body, "Prices", "24");
                    AddField(body, "Average miles per day", variation.ContractVariationAverageMilesPerDay.ToString("N1"));
                    AddField(body, "Rate per mile", variation.ContractVariationRatePerMile.ToString("N2"));
                    AddField(body, "Fuel price element", variation.ContractVariationFuelPriceElement.ToString("N2"));
                    AddField(body, "Contract price per day", variation.ContractVariationAveragePricePerDay.ToString("N2"));
                    AddField(body, "Contract price per year", variation.ContractVariationAveragePricePerYear.ToString("N2"));
                    AddField(body, "Estimate of average revenue per day", variation.ContractVariationAverageRevenuePerDay.ToString("N2"));
                    AddField(body, "Estimate of average revenue per year", variation.ContractVariationAverageRevenuePerYear.ToString("N2"));
                    AddField(body, "Contractor estimate of total income per day", variation.ContractVariationAverageCostPerDay.ToString("N2"));
                    AddField(body, "Contractor estimate of total income per year", variation.ContractVariationAverageCostPerYear.ToString("N2"));

                    AddHeading(body, "Notes", "24");
                    AddField(body, "Notes", variation.ContractVariationNotes);
                    AddField(body, "Reason for edit", variation.ContractVariationReasonForEdit);

                    mainPart.Document.Save();
                }

                var fileName = $"ContractVariation_{variation.ContractReferenceText}_{variation.ContractVariationNoText}.docx";
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
            }
        }

        private static void AddCouncilLogo(WordprocessingDocument targetDocument, MainDocumentPart mainPart, Body body, string templatePath)
        {
            if (!System.IO.File.Exists(templatePath)) return;

            using (var templateDocument = WordprocessingDocument.Open(templatePath, false))
            {
                var templateImagePart = templateDocument.MainDocumentPart.ImageParts.FirstOrDefault();
                if (templateImagePart == null) return;

                var imagePart = mainPart.AddImagePart(GetImagePartType(templateImagePart.ContentType));
                using (var imageStream = templateImagePart.GetStream())
                {
                    imagePart.FeedData(imageStream);
                }

                var relationshipId = mainPart.GetIdOfPart(imagePart);
                body.Append(CreateImageParagraph(relationshipId));
            }
        }

        private static ImagePartType GetImagePartType(string contentType)
        {
            switch ((contentType ?? string.Empty).ToLowerInvariant())
            {
                case "image/jpeg":
                case "image/jpg":
                    return ImagePartType.Jpeg;
                case "image/gif":
                    return ImagePartType.Gif;
                case "image/bmp":
                    return ImagePartType.Bmp;
                case "image/tiff":
                    return ImagePartType.Tiff;
                case "image/x-icon":
                    return ImagePartType.Icon;
                default:
                    return ImagePartType.Png;
            }
        }

        private static Paragraph CreateImageParagraph(string relationshipId)
        {
            const long widthEmus = 1905000;
            const long heightEmus = 762000;

            var inline = new DW.Inline(
                new DW.Extent { Cx = widthEmus, Cy = heightEmus },
                new DW.EffectExtent { LeftEdge = 0L, TopEdge = 0L, RightEdge = 0L, BottomEdge = 0L },
                new DW.DocProperties { Id = 1U, Name = "Council logo" },
                new DW.NonVisualGraphicFrameDrawingProperties(new A.GraphicFrameLocks { NoChangeAspect = true }),
                new A.Graphic(
                    new A.GraphicData(
                        new PIC.Picture(
                            new PIC.NonVisualPictureProperties(
                                new PIC.NonVisualDrawingProperties { Id = 0U, Name = "Council logo" },
                                new PIC.NonVisualPictureDrawingProperties()),
                            new PIC.BlipFill(
                                new A.Blip { Embed = relationshipId },
                                new A.Stretch(new A.FillRectangle())),
                            new PIC.ShapeProperties(
                                new A.Transform2D(
                                    new A.Offset { X = 0L, Y = 0L },
                                    new A.Extents { Cx = widthEmus, Cy = heightEmus }),
                                new A.PresetGeometry(new A.AdjustValueList()) { Preset = A.ShapeTypeValues.Rectangle })))
                    { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" }))
            { DistanceFromTop = 0U, DistanceFromBottom = 0U, DistanceFromLeft = 0U, DistanceFromRight = 0U };

            var element = new Drawing(inline);

            return new Paragraph(
                new ParagraphProperties(new Justification { Val = JustificationValues.Left }, new SpacingBetweenLines { After = "240" }),
                new Run(element));
        }

        private static void AddHeading(Body body, string text, string fontSize)
        {
            var paragraph = new Paragraph(
                new ParagraphProperties(new SpacingBetweenLines { Before = "240", After = "120" }),
                new Run(
                    new RunProperties(new Bold(), new FontSize { Val = fontSize }),
                    new Text(text ?? string.Empty)));

            body.Append(paragraph);
        }

        private static void AddField(Body body, string label, string value)
        {
            var paragraph = new Paragraph(
                new ParagraphProperties(new SpacingBetweenLines { After = "80" }),
                new Run(
                    new RunProperties(new Bold()),
                    new Text(label + ": ") { Space = SpaceProcessingModeValues.Preserve }),
                new Run(new Text(value ?? string.Empty)));

            body.Append(paragraph);
        }

        private static string FirstPopulated(params string[] values)
        {
            return values.FirstOrDefault(value => !string.IsNullOrWhiteSpace(value)) ?? string.Empty;
        }

        public ActionResult TestUpdateVariation(int id)
        { 
            var db = DCC_Session.AutoDatabaseConnection;

            ContractVariation variation = new ContractVariation(db, pK_ContractVariation: id);

            variation.ContractVariationNotes = "UPDATE TEST " + DateTime.Now.ToString();

            variation.Save(db);

            return Content("Done");
        }

    }

}
