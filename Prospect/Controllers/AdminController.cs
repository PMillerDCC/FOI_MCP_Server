using DCC_Standard;
using ProspectBusinessLayer;
using ProspectBusinessLayer.Business_Objects;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Prospect.Controllers
{
    public class AdminController : DCC_Controller
    {

        #region SystemUserGrid actions

        [Authorize(Roles = "System Administrator")]
        public ActionResult SystemUserGrid()
        {
            DCC_SystemUserGrid model = new DCC_SystemUserGrid(DCC_Session);
            model.Initialise(
                Request,
                id: "SystemUserGrid",
                itemNameSingular: "System user",
                column1: new GridColumn(GridColumnType.Text, "LogonID", "Logon ID", 250, defaultSortColumn: true),
                column2: new GridColumn(GridColumnType.Text, "SystemUserFullName", "Name", 250),
                column3: new GridColumn(GridColumnType.Text, "AccessLevelText", "Access level", 200),
                addActionResult: "/Admin/SystemUser",
                editActionResult: "/Admin/SystemUser",
                noOfRowsInGrid: 16,
                showEditButton: false,
                showSelectorColumn: false,
                fullRowSelect: true,
                dcc_Session: DCC_Session
                );

            ViewBag.PageTitle = "System users";
            return View(model);
        }

        #endregion


        #region SystemUser Get Method

        [HttpGet]
        public ActionResult SystemUser()
        {
            #region Determine which SystemUser has been selected. First looks in DCC_Session, then the URL.
            int k_SystemUser = -1;
            string queryKey = Request["primaryKey"] ?? "-1";
            if ((int.TryParse(queryKey, out k_SystemUser)) && (k_SystemUser == -999))
            {
                this.DCC_DisplayMessage(DCC_MessageType.Error, "You must select a System User before clicking Edit", true);
                return RedirectToAction("SystemUserGrid");
            }
            #endregion

            DCC_SystemUser model = new DCC_SystemUser(DCC_Session, k_SystemUser);
            ViewBag.PageTitle = "System user";
            return View(model);
        }

        #endregion


        #region SystemUser Post Method

        [HttpPost]
        public ActionResult SystemUser(string saveButton, DCC_SystemUser model)
        {
            #region Page validation
            if (!ActiveDirectory.IsAccountEnabled(model.LogonID)) ModelState.AddModelError("LogonID", "This Logon ID is not enabled in Active Directory");
            DCC_SystemUser user = new DCC_SystemUser(DCC_Session, logonID: model.LogonID);
            if ((user.K_SystemUser != model.K_SystemUser) && (user.LogonID != null)) ModelState.AddModelError("LogonID", "This Logon ID already exists");
            #endregion

            #region On save
            if (!string.IsNullOrEmpty(saveButton))
            {
                if (ModelState.IsValid)
                {
                    string accessLevel = "";
                    foreach (DCC_SystemRole role in DCC_Session.SystemRoles)
                    {
                        string value = (Request.Form["hid" + role.ID] ?? "");
                        if (value.Contains("True")) accessLevel += role.Role + "|";
                    }
                    if (accessLevel.Substring(accessLevel.Length - 1, 1) == "|") model.AccessLevel = accessLevel.Substring(0, accessLevel.Length - 1);

                    if (model.Save(DCC_Session) < 0) this.DCC_DisplayMessage(DCC_MessageType.Error, "A problem occurred while saving");
                    else this.DCC_DisplayMessage(DCC_MessageType.Success, "Your changes have been saved");
                }
                else this.DCC_DisplayMessage(DCC_MessageType.Error, "Some of your changes are invalid");
            }
            #endregion

            model.SystemRoles = new DCC_SystemRoleList();
            foreach (DCC_SystemRole role in DCC_Session.SystemRoles) model.SystemRoles.Add(new DCC_SystemRole(role.Role, model.AccessLevel.Contains(role.Role)));

            ViewBag.PageTitle = "System user";
            return View(model);
        }

        #endregion


        #region ExportContractorToCSV actions

        public ActionResult ExportContractorToCSV()
        {
            ContractorList contractorList = new ContractorList(DCC_Session.AutoDatabaseConnection);
            DCC_CSV a = new DCC_CSV(contractorList.All, filename: "Prospect-OperatorsExport", propertiesToIgnore: contractorList.PropertiesToIgnore);
            return View();
        }

        #endregion


        #region ExportSpecificationsToCSV actions

        public ActionResult ExportSpecificationsToCSV()
        {
            SpecificationList specificationList = new SpecificationList(DCC_Session.AutoDatabaseConnection);
            DCC_CSV a = new DCC_CSV(specificationList.All, filename: "Prospect-SpecificationsExport", propertiesToIgnore: specificationList.PropertiesToIgnore);
            return View();
        }

        #endregion


        #region FuelCostGrid actions
       
        [Authorize(Roles = "System Administrator")]
        public ActionResult FuelCostGrid()
        {
            FuelCostGrid model = new FuelCostGrid(DCC_Session.AutoDatabaseConnection);
            model.Initialise(
                Request,
                id: "FuelCostGrid",
                itemNameSingular: "Fuel costs",
                itemNamePlural: "Fuel costs",
                column1: new GridColumn(GridColumnType.Text, "EffectiveFromText", "Effective From", 100, defaultSortColumn: true, defaultSortDescending: true, alignRight: true, boundSortField: "EffectiveFromSort"),
                column2: new GridColumn(GridColumnType.Text, "Price", "Price", 100),
                addActionResult: "/Admin/FuelCost",
                editActionResult: "/Admin/FuelCost",
                noOfRowsInGrid: 16,
                showEditButton: false,
                showSelectorColumn: false,
                fullRowSelect: true,
                exportTo_CSV: true,
                dcc_Session: DCC_Session
                );

            ViewBag.PageTitle = "Fuel Costs";
            return View(model);
        }
        #endregion

        
        #region FuelCost Get Method
        [Authorize]
        [HttpGet]
        public ActionResult FuelCost()
        {
            #region Determine which Fuel Cost has been selected. First looks in DCC_Session, then the URL.
            int k_FuelCost = -1;
            string queryKey = Request["primaryKey"] ?? "-1";
            if ((int.TryParse(queryKey, out k_FuelCost)) && (k_FuelCost == -999))
            {
                this.DCC_DisplayMessage(DCC_MessageType.Error, "You must select a Fuel Cost before clicking Edit", true);
                return RedirectToAction("FuelCostGrid");
            }
            #endregion

            FuelCost model = new FuelCost(DCC_Session.AutoDatabaseConnection, k_FuelCost);

            // Set EffectiveFrom to today's date if it's not already set
            if (model.EffectiveFrom == default(DateTime)) // assuming EffectiveFrom is of DateTime type
            {
                model.EffectiveFrom = DateTime.Today;
            }

            ViewBag.PageTitle = "Fuel Cost Prices";
            return View(model);
        }

        #endregion


        #region FuelCost Post Method
        [Authorize]
        [HttpPost]
        public ActionResult FuelCost(string saveButton, FuelCost model)
        {
            #region Page validation
            //if ( ( user.K_SystemUser != model.K_SystemUser ) && (user.LogonID != null) ) ModelState.AddModelError("LogonID", "This Logon ID already exists");
            
            decimal d ; 
            if ( decimal.TryParse(model.PriceString, out d) )
                model.Price = d ;
            else   
                ModelState.AddModelError("PriceString", "Invalid Price");
            
            #endregion

            #region On save
            if (!string.IsNullOrEmpty(saveButton))
            {
                if (ModelState.IsValid)
                {
                    if (model.Save(DCC_Session.AutoDatabaseConnection) < 0) this.DCC_DisplayMessage(DCC_MessageType.Error, "A problem occurred while saving");
                    else this.DCC_DisplayMessage(DCC_MessageType.Success, "Your changes have been saved");
                }
                else this.DCC_DisplayMessage(DCC_MessageType.Error, "Some of your changes are invalid");
            }
            #endregion

            ViewBag.PageTitle = "Fuel Costs";
            return View(model);
        }

        #endregion

        #region HolidayGrid actions

        [Authorize(Roles = "System Administrator")]
        public ActionResult HolidayGrid()
        {
            HolidayGrid model = new HolidayGrid(DCC_Session.AutoDatabaseConnection);
            model.Initialise(
                Request,
                id: "HolidayGrid",
                itemNameSingular: "Bank holiday and school term dates",
                itemNamePlural: "Bank holiday and school term dates",
                column1: new GridColumn(GridColumnType.Text, "Year", "Year", 100, defaultSortColumn: true, defaultSortDescending: true),
                addActionResult: "/Admin/Holiday",
                editActionResult: "/Admin/Holiday",
                noOfRowsInGrid: 16,
                showEditButton: false,
                showSelectorColumn: false,
                fullRowSelect: true,
                exportTo_CSV: true,
                dcc_Session: DCC_Session
                );

            ViewBag.PageTitle = "Bank holiday and school term dates";
            return View(model);
        }

        #endregion


        #region Holiday Get Method

        [HttpGet]
        public ActionResult Holiday()
        {
            #region Determine which Holiday has been selected. First looks in DCC_Session, then the URL.
            int k_Holiday = -1;
            string queryKey = Request["primaryKey"] ?? "-1";
            if ((int.TryParse(queryKey, out k_Holiday)) && (k_Holiday == -999))
            {
                this.DCC_DisplayMessage(DCC_MessageType.Error, "You must select a Holiday before clicking Edit", true);
                return RedirectToAction("HolidayGrid");
            }
            #endregion

            Holiday model = new Holiday(DCC_Session.AutoDatabaseConnection, k_Holiday);
            ViewBag.PageTitle = "Bank holiday and school term dates";
            return View(model);
        }

        #endregion


        #region Holiday Post Method

        [HttpPost]
        public ActionResult Holiday(string saveButton, Holiday model)
        {
            #region Page validation
            //if ( ( user.K_SystemUser != model.K_SystemUser ) && (user.LogonID != null) ) ModelState.AddModelError("LogonID", "This Logon ID already exists");
            #endregion

            #region On save
            if (!string.IsNullOrEmpty(saveButton))
            {
                if (ModelState.IsValid)
                {
                    if (model.Save(DCC_Session.AutoDatabaseConnection) < 0) this.DCC_DisplayMessage(DCC_MessageType.Error, "A problem occurred while saving");
                    else this.DCC_DisplayMessage(DCC_MessageType.Success, "Your changes have been saved");
                }
                else this.DCC_DisplayMessage(DCC_MessageType.Error, "Some of your changes are invalid");
            }
            #endregion

            ViewBag.PageTitle = "Bank holiday and school term dates";
            return View(model);
        }

        #endregion


        #region AgreementGrid actions

        [Authorize(Roles = "System Administrator")]
        public ActionResult AgreementGrid()
        {
            AgreementGrid model = new AgreementGrid(DCC_Session.AutoDatabaseConnection);
            model.Initialise(
                Request,
                id: "AgreementGrid",
                itemNameSingular: "Agreement for fares revenue",
                itemNamePlural: "Agreements for fares revenue",
                column1: new GridColumn(GridColumnType.Text, "AgreementDescription", "Description", 300, defaultSortColumn: true),
                column2: new GridColumn(GridColumnType.Toggle, "IsArchived", "Is archived", 100, trueFalse: "Yes|No"),
                addActionResult: "INLINE",
                editActionResult: "INLINE",
                noOfRowsInGrid: 16,
                showEditButton: true,
                showSelectorColumn: true,
                fullRowSelect: false,
                exportTo_CSV: true,
                dcc_Session: DCC_Session
                );

            ViewBag.PageTitle = "Agreements for fares revenue";
            return View(model);
        }

        #endregion


        #region AreaGrid actions

        [Authorize(Roles = "System Administrator")]
        public ActionResult AreaGrid()
        {
            AreaGrid model = new AreaGrid(DCC_Session.AutoDatabaseConnection);
            model.Initialise(
                Request,
                id: "AreaGrid",
                itemNameSingular: "Area",
                column1: new GridColumn(GridColumnType.Text, "AreaDescription", "Description", 300, defaultSortColumn: true),
                column2: new GridColumn(GridColumnType.Toggle, "IsArchived", "Is archived", 100, trueFalse: "Yes|No"),
                addActionResult: "INLINE",
                editActionResult: "INLINE",
                noOfRowsInGrid: 16,
                showEditButton: true,
                showSelectorColumn: true,
                fullRowSelect: false,
                exportTo_CSV: true,
                dcc_Session: DCC_Session
                );

            ViewBag.PageTitle = "Areas";
            return View(model);
        }

        #endregion


        #region ContractTypeGrid actions

        [Authorize(Roles = "System Administrator")]
        public ActionResult ContractTypeGrid()
        {
            ContractTypeGrid model = new ContractTypeGrid(DCC_Session.AutoDatabaseConnection);
            model.Initialise(
                Request,
                id: "ContractTypeGrid",
                itemNameSingular: "Contract type",
                column1: new GridColumn(GridColumnType.Text, "ContractTypeDescription", "Description", 300, defaultSortColumn: true),
                column2: new GridColumn(GridColumnType.Toggle, "IsArchived", "Is archived", 100, trueFalse: "Yes|No"),
                addActionResult: "INLINE",
                editActionResult: "INLINE",
                noOfRowsInGrid: 16,
                showEditButton: true,
                showSelectorColumn: true,
                fullRowSelect: false,
                exportTo_CSV: true,
                dcc_Session: DCC_Session
                );

            ViewBag.PageTitle = "Contract types";
            return View(model);
        }

        #endregion


        #region LicenceTypeGrid actions

        [Authorize(Roles = "System Administrator")]
        public ActionResult LicenceTypeGrid()
        {
            LicenceTypeGrid model = new LicenceTypeGrid(DCC_Session.AutoDatabaseConnection);
            model.Initialise(
                Request,
                id: "LicenceTypeGrid",
                itemNameSingular: "Licence type",
                column1: new GridColumn(GridColumnType.Text, "LicenceTypeDescription", "Description", 300, defaultSortColumn: true),
                column2: new GridColumn(GridColumnType.Toggle, "IsArchived", "Is archived", 100, trueFalse: "Yes|No"),
                addActionResult: "INLINE",
                editActionResult: "INLINE",
                noOfRowsInGrid: 16,
                showEditButton: true,
                showSelectorColumn: true,
                fullRowSelect: false,
                exportTo_CSV: true,
                dcc_Session: DCC_Session
                );

            ViewBag.PageTitle = "Licence types";
            return View(model);
        }

        #endregion


        #region LicenceStatusGrid actions

        [Authorize(Roles = "System Administrator")]
        public ActionResult LicenceStatusGrid()
        {
            LicenceStatusGrid model = new LicenceStatusGrid(DCC_Session.AutoDatabaseConnection);
            model.Initialise(
                Request,
                id: "LicenceStatusGrid",
                itemNameSingular: "Licence status",
                itemNamePlural: "Licence statuses",
                column1: new GridColumn(GridColumnType.Text, "LicenceStatusDescription", "Description", 300, defaultSortColumn: true),
                column2: new GridColumn(GridColumnType.Toggle, "IsArchived", "Is archived", 100, trueFalse: "Yes|No"),
                addActionResult: "INLINE",
                editActionResult: "INLINE",
                noOfRowsInGrid: 16,
                showEditButton: true,
                showSelectorColumn: true,
                fullRowSelect: false,
                exportTo_CSV: true,
                dcc_Session: DCC_Session
                );

            ViewBag.PageTitle = "Licence status";
            return View(model);
        }

        #endregion


        #region LocalAuthorityGrid actions

        [Authorize(Roles = "System Administrator")]
        public ActionResult LocalAuthorityGrid()
        {
            LocalAuthorityGrid model = new LocalAuthorityGrid(DCC_Session.AutoDatabaseConnection);
            model.Initialise(
                Request,
                id: "LocalAuthorityGrid",
                itemNameSingular: "Local authority",
                itemNamePlural: "Local authorities",
                column1: new GridColumn(GridColumnType.Text, "LocalAuthorityDescription", "Description", 300, defaultSortColumn: true),
                column2: new GridColumn(GridColumnType.Toggle, "IsArchived", "Is archived", 100, trueFalse: "Yes|No"),
                addActionResult: "INLINE",
                editActionResult: "INLINE",
                noOfRowsInGrid: 16,
                showEditButton: true,
                showSelectorColumn: true,
                fullRowSelect: false,
                exportTo_CSV: true,
                dcc_Session: DCC_Session
                );

            ViewBag.PageTitle = "Local authority";
            return View(model);
        }

        #endregion


        #region RestrictionGrid actions

        [Authorize(Roles = "System Administrator")]
        public ActionResult RestrictionGrid()
        {
            RestrictionGrid model = new RestrictionGrid(DCC_Session.AutoDatabaseConnection);
            model.Initialise(
                Request,
                id: "RestrictionGrid",
                itemNameSingular: "Local bus service restriction",
                column1: new GridColumn(GridColumnType.Text, "RestrictionDescription", "Description", 300, defaultSortColumn: true),
                column2: new GridColumn(GridColumnType.Toggle, "IsArchived", "Is archived", 100, trueFalse: "Yes|No"),
                addActionResult: "INLINE",
                editActionResult: "INLINE",
                noOfRowsInGrid: 16,
                showEditButton: true,
                showSelectorColumn: true,
                fullRowSelect: false,
                exportTo_CSV: true,
                dcc_Session: DCC_Session
                );

            ViewBag.PageTitle = "Local bus service restrictions";
            return View(model);
        }

        #endregion


        #region ServiceTypeGrid actions

        [Authorize(Roles = "System Administrator")]
        public ActionResult ServiceTypeGrid()
        {
            ServiceTypeGrid model = new ServiceTypeGrid(DCC_Session.AutoDatabaseConnection);
            model.Initialise(
                Request,
                id: "ServiceTypeGrid",
                itemNameSingular: "Service type",
                column1: new GridColumn(GridColumnType.Text, "ServiceTypeDescription", "Description", 300, defaultSortColumn: true),
                column2: new GridColumn(GridColumnType.Toggle, "IsArchived", "Is archived", 100, trueFalse: "Yes|No"),
                addActionResult: "INLINE",
                editActionResult: "INLINE",
                noOfRowsInGrid: 16,
                showEditButton: true,
                showSelectorColumn: true,
                fullRowSelect: false,
                exportTo_CSV: true,
                dcc_Session: DCC_Session
                );

            ViewBag.PageTitle = "Service type";
            return View(model);
        }

        #endregion

    }

}
