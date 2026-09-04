using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Reflection;

namespace DCC_Standard
{

    public abstract class DCC_Grid
    {

		// Properties

        public string      ID;
        public string      Legend = "";
        public string      ItemNameSingular = "";
        public string      ItemNamePlural = "";
        public int         PageSize = 20;
        public int         DefaultPageSize = 20;
        public string      AddActionController = "None";
        public string      EditActionController = "None";
        public string      DeleteActionController = "None";
        public string      EditActionControllerTokens = "None";
        public string      AddButtonText = "Add";
        public string      EditButtonText = "Edit";
        public string      DeleteButtonText = "Delete";
        public bool        IsReadOnly = false;
        public bool        FullRowSelect = false;
        public bool        HasSelector = true;
		public bool        HideEditButton = false;
		public bool        HideFilterButton = false;
		public bool        HidePrintButton = true;
		public bool        HideShowAllButton = true;
        public bool        OpenLinksInNewTab = false;
        public bool        DisallowSorting = false;
        public bool        DisallowMultipleDeletes = false;
        public bool        RotateColumnHeaders = false;
        public string      HelpText = "";
        public bool        ExportTo_CSV = false;
        public bool        ExportTo_XML = false;
        public DCC_Session Session;

        public int         Width;
        public int         Count;
        public int         CountFiltered;
        public bool        ShowAll = false;
        public int         PageOffset = 0;
        public int         ScrollOffset = 0;
        public string      SelectorLink = "PrimaryKey";
        public string      Action = "none";
        public bool        IsEdit = false;
        public bool        IsDelete = false;
        public bool        IsAdd = false;
        public bool        IsSave = false;
		public bool        IsFilter = false;
        public bool        IsExportCSV = false;
        public bool        IsExportXML = false;
		public string      SortColumn = "";
		public string      DefaultSortColumn = "";
		public string      FilterValue = "";
        public bool        HasTotalRow = false;
        public string      TimeTaken = "";

  

		// Display Properties

		public string Article { get { return "";/*("aeiou".Contains(ItemNameSingular.Substring(0,1).ToLower()) ? " an " : " a ");*/ } }



		// Utilities

        public List<GridColumn>           Columns  = new List<GridColumn>(10);
        SortedList<string, GridColumn>    ColsByID = new SortedList<string, GridColumn>(10);
        List<GridColumn>                  displayColumns = null;
        List<GridColumn>                  exportColumns = null;
        public GridColumn.BoundField      boundSelectorLink;
        public List<object>               GridRows;
        public List<object>               TotalRows;
        public SortedList<string, string> EnteredRow;
        public SortedList<string, string> EnteredRowErrors;
        public List<int>                  SelectedRowLinkKeys;
		public List<Control>              linqFilteringControls = new List<Control>(5);

        public GridColumn this[string colID]
        {
            get
            {
                if (colID == null) return null;
                if (!ColsByID.ContainsKey(colID)) return null;
                return ColsByID[colID];
            }
        }


		#region Actions

		public struct ActionDefinition
        {
            public string labelTxt;
            public string altTxt;
            bool isInternal;
            string controller;
            string action;
            string actionQuery;
            string externalURI;

            public ActionDefinition(string label,string help,string controllerName,string actionName,string paras="")
            {
                labelTxt = (label??"").Replace("'","&#39;");
                altTxt = (help??"").Replace("'", "&#39;");
                controller = controllerName??"";
                action = actionName??"";
                actionQuery = paras??"";
                isInternal = true;
                externalURI = "";
            }

            public ActionDefinition(string label, string help, string externalHTTP)
            {
                labelTxt = (label ?? "").Replace("'", "&#39;");
                altTxt = (help ?? "").Replace("'", "&#39;");
                controller = "";
                action = "";
                actionQuery = "";
                isInternal = false;
                externalURI = externalHTTP;
                if (externalURI.Contains("://"))
                {
                    if (!externalURI.ToUpper().StartsWith("HTTPS://"))
                    {
                        if (!externalURI.ToUpper().StartsWith("HTTP://")) { externalURI = "http://" + externalURI; }
                    }
                }
                else
                {
                    externalURI = "http://" + externalURI;
                }
            }

            public string uri
            {
                get
                {
                    string path = externalURI;
                    if (isInternal)
                    {
                        path = "/" + controller + "/" + action;
                        if (actionQuery.Length > 0) path += "?" + actionQuery;
                    }
                    return path;
                }
            }
        }

        public List<ActionDefinition> actions = new List<ActionDefinition>();

		#endregion


		#region Delegates

		/// <summary>
		/// Called to save added inline row
		/// </summary>
		/// <param name="addedRow">Dictionary of column ID and entered strings</param>
		/// <returns>Empty string if process OK, otherwise an error message</returns>
		public delegate string InlineAddDelegate(SortedList<string, string> addedRow);
        public InlineAddDelegate InlineAdd;

        /// <summary>
        /// Called to save edited inline row
        /// </summary>
        /// <param name="addedRow">Dictionary of column ID and entered strings</param>
        /// <param name="primaryKey">Primary Key for the edited row</param>
        /// <returns>Empty string if process OK, otherwise an error message</returns>
        public delegate string InlineEditDelegate(SortedList<string, string> addedRow, int primaryKey);
        public InlineEditDelegate InlineEdit;

        /// <summary>
        /// Called to delete inline row(s)
        /// </summary>
        /// <param name="addedRow">Dictionary of column ID and entered strings</param>
        /// <param name="primaryKey">Primary Key for the edited row</param>
        /// <returns>Empty string if process OK, otherwise an error message</returns>
        public delegate string InlineDeleteDelegate(List<int> primaryKeys);
        public InlineDeleteDelegate InlineDelete;

        /// <summary>
        /// Called when save of added or edited inline row
        /// </summary>
        /// <param name="enteredRow">Dictionary of column ID and entered strings</param>
        /// <param name="rowErrors">returned dictionary of any errors by column ID</param>
        /// <param name="primaryKeyBeingEdited">primekey for row being edited (so that validate can exclude edited row when comparing other rows in grid). Set to -1 for 'Add'</param>
        /// <returns>true if valid, false if any error entries</returns>
        public delegate bool InlineValidateDelegate(SortedList<string, string> enteredRow, SortedList<string, string> rowErrors, int primaryKeyBeingEdited);
        public InlineValidateDelegate InlineValidate;

        /// <summary>
        /// Called when inline Add button processed
        /// </summary>
        /// <returns>Dictionary of column ID and default values</returns>
        public delegate SortedList<string, string> InlineDefaultsDelegate();
        public InlineDefaultsDelegate InlineDefaults;

        /// <summary>
        /// Called to save 'live' inline row
        /// </summary>
        /// <param name="liveRow">Dictionary of column ID and entered strings</param>
        /// <param name="primaryKey">Primary Key for the live row</param>
        /// <returns>Empty string if process OK, otherwise an error message</returns>
        public delegate string InlineLiveDelegate(SortedList<string, string> liveRow, int primaryKey);
        public InlineLiveDelegate InlineLive;

		public delegate string RenderTotalDelegate(string columnID);
        public RenderTotalDelegate RenderTotal;

        public delegate List<List<string>> GridHeaderDelegate();
        public GridHeaderDelegate GridRowsHeader;
        public delegate List<List<string>> ChangeHeaderDelegate(string lastGroupingKey, string nextGroupingKey, int groupKounter);
        public ChangeHeaderDelegate ChangeHeader;
        public delegate void EveryRowDelegate(object gridRow);
        public EveryRowDelegate EveryRow;
        public delegate List<List<string>> ChangeFooterDelegate(string lastGroupingKey, string nextGroupingKey, int groupKounter);
        public ChangeFooterDelegate ChangeFooter;
        public delegate List<List<string>> GridFooterDelegate();
        public GridFooterDelegate GridRowsFooter;

        public delegate string ExportToCSVDelegate();
        public ExportToCSVDelegate ExportToCSV;

        public delegate string ExportToXMLDelegate();
        public ExportToXMLDelegate ExportToXML;

        public virtual string GetRowCssClass(object rowObject)
        {
            return "";
        }

		#endregion


		#region Constructors

        public DCC_Grid()
        {
            boundSelectorLink = new GridColumn.BoundField(SelectorLink);
        }

		#endregion


		#region Initialise & GetState Methods

		public void Initialise(
			HttpRequestBase request,
			string          id = "DCC_Grid",
			GridColumn      column1  = null,
			GridColumn      column2  = null,
			GridColumn      column3  = null,
			GridColumn      column4  = null,
			GridColumn      column5  = null,
			GridColumn      column6  = null,
			GridColumn      column7  = null,
			GridColumn      column8  = null,
			GridColumn      column9  = null,
			GridColumn      column10 = null,
			GridColumn      column11 = null,
			GridColumn      column12 = null,
			GridColumn      column13 = null,
			GridColumn      column14 = null,
			GridColumn      column15 = null,
			GridColumn      column16 = null,
			GridColumn      column17 = null,
			GridColumn      column18 = null,
			GridColumn      column19 = null,
			GridColumn      column20 = null,
			GridColumn      column21 = null,
			GridColumn      column22 = null,
			GridColumn      column23 = null,
			GridColumn      column24 = null,
			GridColumn      column25 = null,
			GridColumn      column26 = null,
			GridColumn      column27 = null,
			GridColumn      column28 = null,
			GridColumn      column29 = null,
			GridColumn      column30 = null,
			string          itemNameSingular = "Item",
			string          itemNamePlural = "",
			string          legend = "",
			int             noOfRowsInGrid = 20,
			int             width = 1300,
			string          addActionResult = "None",
			string          editActionResult = "None",
			string          editActionResultTokens = "None",
			string          addButtonText = "Add",
			string          editButtonText = "Edit",
			string          deleteButtonText = "Delete",
			bool            isReadOnly = false,
			bool            fullRowSelect = false,
			bool            showSelectorColumn = true,
			bool            showEditButton = true,
			bool            showFilterButton = true,
			bool            showPrintButton = false,
			bool            showShowAllButton = false,
			bool            openRecordInNewTab = false,
			bool            allowSorting = true,
			bool            allowMultipleDeletes = false,
			bool            rotateColumnHeaders = false,
			bool            defaultFilterOn = false,
			bool            exportTo_CSV = false,
			bool            exportTo_XML = false,
			string          helpText = "",
			DCC_Session     dcc_Session = null)
        {
			if (column1  != null) { column1 .ID = "column1" ; AddGridColumn(column1 ); }
			if (column2  != null) { column2 .ID = "column2" ; AddGridColumn(column2 ); }
			if (column3  != null) { column3 .ID = "column3" ; AddGridColumn(column3 ); }
			if (column4  != null) { column4 .ID = "column4" ; AddGridColumn(column4 ); }
			if (column5  != null) { column5 .ID = "column5" ; AddGridColumn(column5 ); }
			if (column6  != null) { column6 .ID = "column6" ; AddGridColumn(column6 ); }
			if (column7  != null) { column7 .ID = "column7" ; AddGridColumn(column7 ); }
			if (column8  != null) { column8 .ID = "column8" ; AddGridColumn(column8 ); }
			if (column9  != null) { column9 .ID = "column9" ; AddGridColumn(column9 ); }
			if (column10 != null) { column10.ID = "column10"; AddGridColumn(column10); }
			if (column11 != null) { column11.ID = "column11"; AddGridColumn(column11); }
			if (column12 != null) { column12.ID = "column12"; AddGridColumn(column12); }
			if (column13 != null) { column13.ID = "column13"; AddGridColumn(column13); }
			if (column14 != null) { column14.ID = "column14"; AddGridColumn(column14); }
			if (column15 != null) { column15.ID = "column15"; AddGridColumn(column15); }
			if (column16 != null) { column16.ID = "column16"; AddGridColumn(column16); }
			if (column17 != null) { column17.ID = "column17"; AddGridColumn(column17); }
			if (column18 != null) { column18.ID = "column18"; AddGridColumn(column18); }
			if (column19 != null) { column19.ID = "column19"; AddGridColumn(column19); }
			if (column20 != null) { column20.ID = "column20"; AddGridColumn(column20); }
			if (column21 != null) { column21.ID = "column21"; AddGridColumn(column21); }
			if (column22 != null) { column22.ID = "column22"; AddGridColumn(column22); }
			if (column23 != null) { column23.ID = "column23"; AddGridColumn(column23); }
			if (column24 != null) { column24.ID = "column24"; AddGridColumn(column24); }
			if (column25 != null) { column25.ID = "column25"; AddGridColumn(column25); }
			if (column26 != null) { column26.ID = "column26"; AddGridColumn(column26); }
			if (column27 != null) { column27.ID = "column27"; AddGridColumn(column27); }
			if (column28 != null) { column28.ID = "column28"; AddGridColumn(column28); }
			if (column29 != null) { column29.ID = "column29"; AddGridColumn(column29); }
			if (column30 != null) { column30.ID = "column30"; AddGridColumn(column30); }

			ID = id;
			ItemNameSingular = itemNameSingular;
			ItemNamePlural = ( (itemNamePlural == "") ? itemNameSingular + "s" : itemNamePlural);
			Legend =  ( (legend == "") ? ItemNamePlural : legend);
			PageSize = DefaultPageSize = noOfRowsInGrid;
            Width = width;
			AddActionController = addActionResult;
			EditActionController = editActionResult;
            EditActionControllerTokens = editActionResultTokens;
			AddButtonText = addButtonText;
			EditButtonText = editButtonText;
			DeleteButtonText = deleteButtonText;
			IsReadOnly = isReadOnly;
			FullRowSelect = fullRowSelect;
			HasSelector = showSelectorColumn;
			HideEditButton = !showEditButton;
			HideFilterButton = !showFilterButton;
			HidePrintButton = !showPrintButton;
			HideShowAllButton = !showShowAllButton;
			OpenLinksInNewTab = openRecordInNewTab;
			DisallowSorting = !allowSorting;
			DisallowMultipleDeletes = !allowMultipleDeletes;
			RotateColumnHeaders = rotateColumnHeaders;
			HelpText = helpText;
			ExportTo_CSV = exportTo_CSV;
			ExportTo_XML = exportTo_XML;
			Session = dcc_Session;

			GetState(request, defaultFilterOn);
		}

		public void GetState(HttpRequestBase request, bool defaultFilterOn = false)
        {
            IsExportCSV = ((request.Form[ID + "_isExportCSV"] ?? "false") == "true");
            IsExportXML = ((request.Form[ID + "_isExportXML"] ?? "false") == "true");
            IsSave      = ((request.Form[ID + "_isSave"]      ?? "false") == "true");
            IsEdit      = ((request.Form[ID + "_isEdit"]      ?? "false") == "true");
            IsDelete    = ((request.Form[ID + "_isDelete"]    ?? "false") == "true");
            IsAdd       = ((request.Form[ID + "_isAdd"]       ?? "false") == "true");
            SortColumn  =  (request.Form[ID + "_sortColumn"]  ?? "");
            FilterValue =  (request.Form[ID + "_filter"]      ?? "");

			if (SortColumn == "")
			{
				SortColumn = DefaultSortColumn;
			}
			
            PageOffset = 0;
            int.TryParse((request.Form[ID + "_pageSize"]   ?? PageSize.ToString()), out PageSize);
            int.TryParse((request.Form[ID + "_pageOffset"] ?? "0"), out PageOffset);

            SelectedRowLinkKeys = new List<int>(PageSize);
            foreach (string k in request.Form.Keys)
            {
                if (k.StartsWith(ID + "_selector_"))
                {
                    if (request.Form[k] == "on")
                    {
                        string keyTxt = k.Replace(ID + "_selector_", "");
                        int test = 0;
                        if (int.TryParse(keyTxt,out test))  SelectedRowLinkKeys.Add(test);
                    }
                }
            }

            if (IsDelete && InlineDelete != null)
            {
                EnteredRowErrors = new SortedList<string, string>();
                if (SelectedRowLinkKeys.Count == 0)
                {
                    IsDelete = false;
                    EnteredRowErrors.Add("*", "You must select at least one row to delete");
                }

				if ( (SelectedRowLinkKeys.Count > 1) && (DisallowMultipleDeletes) )
				{
                    IsDelete = false;
                    EnteredRowErrors.Add("*", "You cannot select more than one row to delete");
				}

                else
                {
                    if (IsSave)
                    {
                        string addOKmessage = InlineDelete(SelectedRowLinkKeys);
                        if (addOKmessage.Length > 0)
                        {
                            EnteredRowErrors.Add("*", addOKmessage);
                        }
                        else
                        {
                            IsDelete = false;
                        }
                    }
                }
            }

            if (IsEdit && InlineEdit != null)
            {
                EnteredRowErrors = new SortedList<string, string>();
                if (SelectedRowLinkKeys.Count != 1)
                {
                    IsEdit = false;
                    string err = "Must select single row to edit";
                    if (SelectedRowLinkKeys.Count > 1) err = "Sorry, can only edit a single row";
                    EnteredRowErrors.Add("*", err);
                }
                else
                {
                    if (IsSave)
                    {
                        EnteredRow = new SortedList<string, string>();
                        EnteredRowErrors = new SortedList<string, string>();
                        string marker = "Edit_" + ID + "_";
                        foreach (string k in request.Form.Keys)
                        {
                            if (!k.StartsWith(marker)) continue;
                            EnteredRow.Add(k.Replace(marker, ""), request.Form[k]);
                        }
                        if (EnteredRow.Count > 0)
                        {
                            bool addOK = true;
                            if (InlineValidate != null)
                            {
                                addOK = InlineValidate(EnteredRow, EnteredRowErrors, SelectedRowLinkKeys[0]);
                            }
                            if (addOK)
                            {
                                string addOKmessage = InlineEdit(EnteredRow, SelectedRowLinkKeys[0]);
                                if (addOKmessage.Length > 0)
                                {
                                    EnteredRowErrors.Add("*", addOKmessage);
                                }
                                else
                                {
                                    IsEdit = false;
                                }
                            }
                        }
                    }
                }
            }

            if (IsAdd && InlineAdd != null)
            {
                if (IsSave)
                {
                    EnteredRow = new SortedList<string, string>();
                    EnteredRowErrors = new SortedList<string, string>();
                    string marker = "Add_" + ID + "_";
                    foreach (string k in request.Form.Keys)
                    {
                        if (!k.StartsWith(marker)) continue;
                        EnteredRow.Add(k.Replace(marker, ""), request.Form[k]);
                    }
                    if (EnteredRow.Count > 0)
                    {
                        bool addOK = true;
                        if (InlineValidate != null)
                        {
                            addOK = InlineValidate(EnteredRow,EnteredRowErrors,-1);
                        }
                        if (addOK)
                        {
                            string addOKmessage=InlineAdd(EnteredRow);
                            if (addOKmessage.Length > 0)
                            {
                                EnteredRowErrors.Add("*", addOKmessage);
                            }
                            else
                            {
                                IsAdd = false;
                            }
                        }
                    }
                }
                else
                {
                    if (InlineDefaults != null) EnteredRow = InlineDefaults();
                }
            }

            if (InlineLive != null)
            {
                string marker = "Live_";

                SortedList<int, SortedList<string, string>> liveCols = new SortedList<int, SortedList<string, string>>(25);

                foreach (string k in request.Form.Keys)
                {
                    if (!k.StartsWith(marker)) continue;
                    string[] tokens = k.Replace(marker, "").Split(new char[] {'_'},2);
                    if (tokens.Length == 2)
                    {
                        int linkKey = -1;
                        int.TryParse(tokens[0], out linkKey);
                        if (!liveCols.ContainsKey(linkKey)) liveCols.Add(linkKey, new SortedList<string, string>(10));
                        liveCols[linkKey].Add(tokens[1], request.Form[k]);
                    }
                }
                foreach (int k in liveCols.Keys)
                {
                    InlineLive(liveCols[k], k);
                }
            }

            Action = request["action"] ?? "none";
	        
			if ((request.Form[ID + "_TopClick"]      ?? "0") == "1") PageOffset = 0;
	        if ((request.Form[ID + "_PreviousClick"] ?? "0") == "1") PageOffset = Math.Max(0, (PageOffset - PageSize));
	        if ((request.Form[ID + "_NextClick"]     ?? "0") == "1") PageOffset = Math.Min(CountFiltered - PageSize, (PageOffset + PageSize));
	        if ((request.Form[ID + "_BottomClick"]   ?? "0") == "1") PageOffset = CountFiltered - PageSize;

	        if ((request.Form[ID + "_ShowAll"]       ?? "0") == "1") { PageSize = 5000;            PageOffset = 0; }
	        if ((request.Form[ID + "_ShowPageSize"]  ?? "0") == "1") { PageSize = DefaultPageSize; PageOffset = 0; }
            
			     if (Action == ID + "_FiltOn")  IsFilter = true;
			else if (Action == ID + "_FiltOff") IsFilter = false;
			else if (defaultFilterOn)           IsFilter = true;
			ApplyFilter(request.Form);

        }

		public void SetItemName(string singular, string plural = "") // phase out
		{
			ItemNameSingular = singular;
			ItemNamePlural = plural;
			Legend = plural;
			if (plural == "") ItemNamePlural = singular + "s";
			if (Legend == "") Legend = ItemNamePlural;
		}

		#endregion


		#region Add Column Methods

		public void AddGridColumn(GridColumn gc)
		{
			     if (gc.Type == GridColumnType.Toggle) AddToggleColumn(gc.ID, gc.BoundCellName, gc.Heading, gc.BoundSortName, gc.BoundLinkName, /*gc.LinkController,*/ gc.Heading_CSS, gc.Cell_CSS, gc.Width, gc.TrueFalse);
			else if (gc.Type == GridColumnType.Date)   AddDateColumn  (gc.ID, gc.BoundCellName, gc.Heading, gc.BoundSortName, gc.BoundLinkName, /*gc.LinkController,*/ gc.Heading_CSS, gc.Cell_CSS, gc.Width, gc.NullFormat);
			else if (gc.Type == GridColumnType.Digits) AddDigitsColumn(gc.ID, gc.BoundCellName, gc.Heading, gc.BoundSortName, gc.BoundLinkName, /*gc.LinkController,*/ gc.Heading_CSS, gc.Cell_CSS, gc.Width);
			else if (gc.Type == GridColumnType.Money)  AddMoneyColumn (gc.ID, gc.BoundCellName, gc.Heading, gc.BoundSortName, gc.BoundLinkName, /*gc.LinkController,*/ gc.Heading_CSS, gc.Cell_CSS, gc.Width);
			else									   AddTextColumn  (gc);//.ID, gc.BoundCellName, gc.Heading, gc.BoundSortName, gc.BoundLinkName, /*gc.LinkController,*/ gc.Heading_CSS, gc.Cell_CSS, gc.Width, gc.AlignRight, gc.WrapText);

			if (DefaultSortColumn == "") DefaultSortColumn = ( gc.DefaultSortColumn ? ( gc.DefaultSortDescending ? "^" : "" ) + gc.ID : "");

			if ( ((gc.Type == GridColumnType.Digits) || (gc.Type == GridColumnType.Money)) && (gc.AddToTotalRow) ) AddTotalRow(gc.ID, gc.BoundSortName);
		}

        public void AddTextColumn(GridColumn gc)
        {
            autoPopulate(gc.BoundCellName, ref gc.Heading, ref gc.Heading_CSS, gc.Width, 150);
            gc.BoundCell = new GridColumn.BoundField(gc.BoundCellName);
            gc.BoundLink = new GridColumn.BoundField(gc.BoundLinkName);
            gc.BoundSort = new GridColumn.BoundField(gc.BoundSortName);
            gc.BoundTotal = new GridColumn.BoundField(gc.BoundCellName);
            Columns.Add(gc);
            ColsByID.Add(gc.ID, gc);
        }

		public void AddTextColumn(string id, string boundCellName, string heading = "", string boundSortFieldName = "", string boundLinkFieldName = "", /*string linkController = "", */string heading_CSS = "", string cell_CSS = "", int width = -1, bool alignRight = false, bool wrapText = false)
		{
			autoPopulate(boundCellName, ref heading, ref heading_CSS, width, 150);
			GridColumn gc = new GridColumn(GridColumnType.Text, boundCellName, heading, width, id, boundSortFieldName, boundLinkFieldName, heading_CSS, cell_CSS, alignRight: alignRight);
			gc.BoundCell = new GridColumn.BoundField(boundCellName);
			gc.BoundLink = new GridColumn.BoundField(boundLinkFieldName);
			gc.BoundSort = new GridColumn.BoundField(boundSortFieldName);
			gc.BoundTotal = new GridColumn.BoundField(boundCellName);
			gc.WrapText = wrapText;
			Columns.Add(gc);
			ColsByID.Add(id, gc);
		}

		public void AddToggleColumn(string id, string boundCellName, string heading = "", string boundSortFieldName = "", string boundLinkFieldName = "", /*string linkController = "", */string heading_CSS = "", string cell_CSS = "", int width = -1, string trueFalse = "Yes|No")
        {
            autoPopulate(boundCellName, ref heading, ref heading_CSS, width, 150);
            GridColumn gc = new GridColumn(GridColumnType.Toggle, boundCellName, heading, width, id, boundSortFieldName, boundLinkFieldName, heading_CSS, cell_CSS);
            gc.BoundCell = new GridColumn.BoundField(boundCellName);
            gc.BoundLink = new GridColumn.BoundField(boundLinkFieldName);
            gc.BoundSort = new GridColumn.BoundField(boundSortFieldName);
            gc.BoundTotal = new GridColumn.BoundField(boundCellName);
            Columns.Add(gc);
            ColsByID.Add(id, gc);
            string[] tokens = trueFalse.Split(new char[] {'|'},StringSplitOptions.None);
            if (tokens.Length == 2)
            {
                gc.ToggleTrueText = tokens[0];
                gc.ToggleFalseText = tokens[1];
            }
        }

        public void AddDateColumn(string id, string boundCellName, string heading = "", string boundSortFieldName = "", string boundLinkFieldName = "", /*string linkController = "", */string heading_CSS = "", string cell_CSS = "", int width = -1, string nullText = "-")
        {
            autoPopulate(boundCellName, ref heading, ref heading_CSS, width, 70);
            GridColumn gc = new GridColumn(GridColumnType.Date, boundCellName, heading, width, id, boundSortFieldName, boundLinkFieldName, heading_CSS, cell_CSS);
            gc.CellFormat = "dd MMM yyyy";
            gc.NullFormat = nullText;
            gc.BoundCell = new GridColumn.BoundField(boundCellName);
            gc.BoundLink = new GridColumn.BoundField(boundLinkFieldName);
            gc.BoundSort = new GridColumn.BoundField(boundSortFieldName);
            gc.BoundTotal = new GridColumn.BoundField(boundCellName);
            Columns.Add(gc);
            ColsByID.Add(id, gc);
        }

        public void AddDigitsColumn(string id, string boundCellName, string heading = "", string boundSortFieldName = "", string boundLinkFieldName = "", /*string linkController = "", */string heading_CSS = "", string cell_CSS = "", int width = -1)
        {
            autoPopulate(boundCellName, ref heading, ref heading_CSS, width, 50);
            GridColumn gc = new GridColumn(GridColumnType.Digits, boundCellName, heading, width, id, boundSortFieldName, boundLinkFieldName, heading_CSS, cell_CSS);
            gc.CellFormat = "###,###,##0";
            gc.BoundCell = new GridColumn.BoundField(boundCellName);
            gc.BoundLink = new GridColumn.BoundField(boundLinkFieldName);
            gc.BoundSort = new GridColumn.BoundField(boundSortFieldName);
            gc.BoundTotal = new GridColumn.BoundField(boundCellName);
            Columns.Add(gc);
            ColsByID.Add(id, gc);
        }

        public void AddMoneyColumn(string id, string boundCellName, string heading = "", string boundSortFieldName = "", string boundLinkFieldName = "", /*string linkController = "",*/ string heading_CSS = "", string cell_CSS = "", int width = -1)
        {
            autoPopulate(boundCellName, ref heading, ref heading_CSS, width, 50);
            GridColumn gc = new GridColumn(GridColumnType.Money, boundCellName, heading, width, id, boundSortFieldName, boundLinkFieldName, heading_CSS, cell_CSS);
            gc.CellFormat = "£##,###,##0.00";
            gc.BoundCell = new GridColumn.BoundField(boundCellName);
            gc.BoundLink = new GridColumn.BoundField(boundLinkFieldName);
            gc.BoundSort = new GridColumn.BoundField(boundSortFieldName);
            Columns.Add(gc);
            ColsByID.Add(id, gc);
        }

        /// <summary>
        /// Adds an Finite List Text Column to the grid
        /// It will be sortable, filterable, left justified
        /// </summary>
        /// <param name="ID">Unique ID (letters/numbers/_ only)</param>
        /// <param name="width">Width of Column (defaults to 75 if 0 or negative)</param>
        /// <param name="boundCellName">Property or Field name for this column</param>
        /// <param name="headingText">Column Heading</param>
        /// <param name="heading_CSS">css classNames separated by spaces giving CSS for the column HeadingText</param>
        /// <param name="cell_CSS">css classNames separated by spaces giving CSS for the column cells</param>
        /// <param name="ddlGenerator">Delegate returning a List of strings to populate a DropDownList. First item will always be no filter, so text should be '--all--', '-Any-' or '-Select-'.</param>
        public void AddOptionColumn(string id, string boundCellName, string heading = "", string boundSortFieldName = "", string boundLinkFieldName = "", /*string linkController = "", */string heading_CSS = "", string cell_CSS = "", int width = -1, List<object> selectOptions = null)
        {
            autoPopulate(boundCellName, ref heading, ref heading_CSS, width, 50);
            GridColumn gc = new GridColumn(GridColumnType.Text, boundCellName, heading, width, id, boundSortFieldName, boundLinkFieldName, heading_CSS, cell_CSS, selectOptions: selectOptions);
            gc.BoundCell = new GridColumn.BoundField(boundCellName);
            gc.BoundLink = new GridColumn.BoundField(boundLinkFieldName);
            gc.BoundSort = new GridColumn.BoundField(boundSortFieldName);
            Columns.Add(gc);
            ColsByID.Add(id, gc);
        }

        private static void autoPopulate(string boundFieldName, ref string heading, ref string heading_CSS, int width, int defaultWidth)
        {
            if (heading.Length == 0) heading = boundFieldName;
        }

        public void AddColumn(GridColumn gridColumn)
        {
            Columns.Add(gridColumn);
            ColsByID.Add(gridColumn.ID, gridColumn);
        }

		#endregion


		#region Total Row Methods

		public void AddTotalRow(string id, string boundFieldToBeAccumulated = null)
        {
            ColsByID[id].BoundTotal = new GridColumn.BoundField( ( (boundFieldToBeAccumulated == null) ? ColsByID[id].BoundCell.name : boundFieldToBeAccumulated ) );
            ColsByID[id].AddToTotalRow = true;
            HasTotalRow = true;
        }

        //public void AddTotalRow(string id)
        //{
        //    ColsByID[id].AddToTotalRow = true;
        //    ColsByID[id].BoundTotal = new GridColumn.BoundField(ColsByID[id].BoundCell.name);
        //    HasTotals = true;
        //}

		//public void Accumulate(string id, int? rowValue)
		//{
		//	if (rowValue.HasValue)
		//		ColsByID[id].Total += (Double)rowValue;
		//}

		//public void Accumulate(string id, decimal? rowValue)
		//{
		//	if (rowValue.HasValue)
		//		ColsByID[id].Total += (Double)rowValue;
		//}

		#endregion


		#region Filter Methods

		void ApplyFilter(NameValueCollection pairs)
        {
            bool no_filtered_column_found = true;
            foreach (GridColumn gc in Columns) gc.Filter = "";

            foreach (string kv in pairs.Keys)
            {
				if (kv.Substring(0,3).ToLower() == "ddl")
				{
					string s = kv.Substring(3);
					foreach (GridColumn gc in Columns)
					{
						if (gc.ID == s)
						{
							string filtValue = pairs[kv].Trim();
							if (filtValue.Length > 0) no_filtered_column_found = false;
							gc.Filter = pairs[kv].ToUpper();
						}
					}
				}

				else
				{
					foreach (GridColumn gc in Columns)
					{
						if (gc.ID == kv)
						{
							string filtValue = pairs[kv].Trim();
							if (filtValue.Length > 0) no_filtered_column_found = false;
							gc.Filter = pairs[kv].ToUpper();
						}
					}
				}
            }

            if (no_filtered_column_found)
            {
                GridRows = TotalRows.ToList();
            }
            else
            {
                List<object> remainingRows = null;

                foreach (GridColumn gc in Columns.Where(n => n.Filter.Length > 0).ToList())
                {
                    remainingRows = ( (remainingRows == null) ? TotalRows : GridRows);
                    SortedList<string, object> SortedGridRows = new SortedList<string,object>(1000);
                    foreach (object remainingRow in remainingRows)
                    {
						string a = gc.BoundCell.fetchData(remainingRow, "").ToUpper();
						string b = gc.BoundSort.fetchData(remainingRow, "").ToUpper();
                        string textToCheck = ( (gc.BoundSort.name.Length > 0) ? b : a );
                        if (textToCheck.Contains(gc.Filter))
                        {
                            string key = textToCheck + "###" + SortedGridRows.Count.ToString();
                            SortedGridRows.Add(key,remainingRow);
                        }
                    }
                    GridRows = SortedGridRows.Values.ToList();
                }
            }
            CountFiltered = GridRows.Count;
        }

		//public void ClearFilters()
		//      {
		//          foreach (GridColumn gc in Columns) gc.Filter = "";
		//      }

		//public void ApplyNoFilter()
		//{
		//    GridRows = TotalRows.ToList();
		//    CountFiltered = GridRows.Count;
		//}
		
		#endregion


		#region Sort Methods

		public class SortByColumn : IComparer<object>
		{
			GridColumn.BoundField sortByColumn;
			bool upsideDown;

			public SortByColumn(GridColumn.BoundField colID, bool reverse)
			{
				sortByColumn = colID;
				upsideDown = reverse;
			}

			public int Compare(object obj1, object obj2)
			{
				//if (sortByColumn.isUndetermined) return 0;

				string s1 = sortByColumn.fetchData(obj1, "");
				string s2 = sortByColumn.fetchData(obj2, "");

				if (upsideDown) return string.Compare(s2, s1, true);
				return string.Compare(s1,s2,true);
			}
		}

		#endregion


		#region Render Grid

		public string RenderGrid()
        {
			exportColumns = Columns.Where(z => !z.OnlyDisplay).ToList();
			displayColumns = Columns.Where(z => !z.OnlyExport).ToList();

			HandleExports();

			StringBuilder sb = new StringBuilder();
			sb.AppendLine("<fieldset class=\"maxWidth"+ Width + "\"><h2 class=\"fakeLegend\">" + Legend + "</h2>");

			if (HelpText.Length > 0)
			{
				sb.AppendLine("<div class=\"row\">");
				sb.AppendLine("<div class=\"col-lg-11\">");
				sb.AppendLine("<div><div class=\"form-group\">");
				sb.AppendLine("<textarea id = \"hlp" + ItemNameSingular + "\", class=\"helpBlock\", style = \"height: 22px;\", @readonly = \"readonly\">" + HelpText + "</textarea>");
				sb.AppendLine("</div></div>");
				sb.AppendLine("</div>");
				sb.AppendLine("<div class=\"col-lg-1\">");
				sb.AppendLine("<div><div class=\"form-group\"><input id=\"btn" + ItemNameSingular + "Help\" class=\"morebtn\" type=\"button\" value=\"read more...\" onclick=\"btn" + ItemNameSingular + "HelpClick();\"></div></div>");
				sb.AppendLine("</div>");
				sb.AppendLine("</div>");
			}

			sb.Append(GridHeader);
			if (GridRows.Count > 0) sb.AppendLine(GridBody);
			else sb.AppendLine("There are no " + ItemNamePlural.ToLower() + " to display</br></br>");

			sb.AppendLine("<input id='" + ID + "_isExportXML'   name='" + ID + "_isExportXML'   type='hidden' data-ays-ignore=\"true\" value='" + (IsExportXML ? "true" : "false") + "' />");
			sb.AppendLine("<input id='" + ID + "_isExportCSV'   name='" + ID + "_isExportCSV'   type='hidden' data-ays-ignore=\"true\" value='" + (IsExportCSV ? "true" : "false") + "' />");
			sb.AppendLine("<input id='" + ID + "_isEdit'        name='" + ID + "_isEdit'        type='hidden' data-ays-ignore=\"true\" value='" + (IsEdit ? "true" : "false") + "' />");
			sb.AppendLine("<input id='" + ID + "_isDelete'      name='" + ID + "_isDelete'      type='hidden' data-ays-ignore=\"true\" value='" + (IsDelete ? "true" : "false") + "' />");
			sb.AppendLine("<input id='" + ID + "_isAdd'         name='" + ID + "_isAdd'         type='hidden' data-ays-ignore=\"true\" value='" + (IsAdd ? "true" : "false") + "' />");
			sb.AppendLine("<input id='" + ID + "_pageSize'      name='" + ID + "_pageSize'      type='hidden' data-ays-ignore=\"true\" value='" + PageSize.ToString() + "' />");
			sb.AppendLine("<input id='" + ID + "_pageOffset'    name='" + ID + "_pageOffset'    type='hidden' data-ays-ignore=\"true\" value='" + PageOffset.ToString() + "' />");
			sb.AppendLine("<input id='" + ID + "_primaryKey'    name='" + ID + "_primaryKey'    type='hidden' data-ays-ignore=\"true\" value='" + "-999" + "' />");
			sb.AppendLine("<input id='" + ID + "_sortColumn'    name='" + ID + "_sortColumn'    type='hidden' data-ays-ignore=\"true\" value='" + SortColumn + "' />");
			sb.AppendLine("<input id='" + ID + "_filter'        name='" + ID + "_filter'        type='hidden' data-ays-ignore=\"true\" value='" + FilterValue + "' />");
			sb.AppendLine("<input id='" + ID + "_isSave'        name='" + ID + "_isSave'        type='hidden' data-ays-ignore=\"true\" value='false' />");
			sb.AppendLine("<input id='" + ID + "_TopClick'      name='" + ID + "_TopClick'      type='hidden' data-ays-ignore=\"true\" value='0' />");
			sb.AppendLine("<input id='" + ID + "_PreviousClick' name='" + ID + "_PreviousClick' type='hidden' data-ays-ignore=\"true\" value='0' />");
			sb.AppendLine("<input id='" + ID + "_NextClick'     name='" + ID + "_NextClick'     type='hidden' data-ays-ignore=\"true\" value='0' />");
			sb.AppendLine("<input id='" + ID + "_BottomClick'   name='" + ID + "_BottomClick'   type='hidden' data-ays-ignore=\"true\" value='0' />");
			sb.AppendLine("<input id='" + ID + "_ShowAll'       name='" + ID + "_ShowAll'       type='hidden' data-ays-ignore=\"true\" value='0' />");
			sb.AppendLine("<input id='" + ID + "_ShowPageSize'  name='" + ID + "_ShowPageSize'  type='hidden' data-ays-ignore=\"true\" value='0' />");
			sb.AppendLine("</fieldset>");

            return sb.ToString();
        }

		#endregion


		#region Handle Exports

        void HandleExports()
        {
            string filename = ( (Session != null) ? Session.ApplicationTitle : "DCC Application" ) + " - " + Legend + " - " + DateTime.Now.ToString("yyyy-MM-dd HH.mm") + ( ExportTo_CSV ? ".csv" : ".xml" );

			#region Export to CSV
			if (IsExportCSV && ExportTo_CSV)
            {
                IsExportCSV = false;
                EnteredRowErrors = new SortedList<string, string>(10);

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = "application/ms-excel";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename=" + filename);

                try
                {
					string tmp = Path.GetTempFileName();
                    using (StreamWriter sw = new StreamWriter(new FileStream(tmp, FileMode.Open, FileAccess.ReadWrite), Encoding.UTF8))
                    {
                        GridColumn.BoundField boundSort = new GridColumn.BoundField();
                        if (SortColumn.Length > 0) boundSort = this[SortColumn.Replace("^", "")].BoundSortEffect;

                        int exportedCount = 0;
                        string delim = "";
                        foreach (GridColumn gc in Columns)
                        {
                            string headText = gc.Heading;
                            sw.Write(delim + "\"" + headText.Replace("\"", "\"\"") + "\"");
                            delim = ",";
                        }
                        sw.WriteLine();

                        foreach (object row in GridRows.OrderBy(z => z, new SortByColumn(boundSort, SortColumn.StartsWith("^"))).ToList())
                        {
                            exportedCount++;
                            delim = "";
                            foreach (GridColumn gc in Columns)
                            {
                                string cellText = gc.BoundCell.fetchData(row, gc.CellFormat).Replace("(null)", "");

								if (cellText.Contains("<span class"))
								{
									cellText = cellText.Replace("<span class='red'>", "").Replace("</span>", "");
								}
                                sw.Write(delim + "\"" + cellText.Replace("\"", "\"\"") + "\"");
                                delim = ",";
                            }
                            sw.WriteLine();
                        }
                        sw.Flush();
                        sw.Close();
                        HttpContext.Current.Response.WriteFile(tmp);
                        HttpContext.Current.Response.End();
                    }
                }
                catch (Exception e)
                {
                    EnteredRowErrors.Add("*", "Export to " + filename + " failed: " + e.Message);
                }
            }
			#endregion

			#region Export to XML
			if (IsExportXML && ExportTo_XML)
            {
                IsExportXML = false;
                EnteredRowErrors = new SortedList<string, string>(10);

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = "application/xml";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename=" + filename);

                try
                {
                    string tmp = Path.GetTempFileName();
                    using (StreamWriter sw = new StreamWriter(new FileStream(tmp, FileMode.Open, FileAccess.ReadWrite), Encoding.UTF8))
                    {
                        GridColumn.BoundField boundSort = new GridColumn.BoundField();
                        if (SortColumn.Length > 0) boundSort = this[SortColumn.Replace("^", "")].BoundSortEffect;
                        int exportedCount = 0;

                        sw.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
                        sw.WriteLine(@"<"+ItemNamePlural+">");

                        foreach (object row in GridRows.OrderBy(z => z, new SortByColumn(boundSort, SortColumn.StartsWith("^"))).ToList())
                        {
                            sw.WriteLine(@"  <" + ItemNameSingular + ">");
                            exportedCount++;
                            foreach (GridColumn gc in Columns)
                            {
                                string colname = gc.Heading.Trim().Replace(" ", "_");
                                string cellText = gc.BoundCell.fetchData(row, gc.CellFormat).Replace("(null)", "");
                                sw.Write("    <" + colname + ">" + cellText.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;") + "</" + colname + ">");   //  probably other format errors need coping with
                            }
                            sw.WriteLine();
                            sw.WriteLine(@"  </" + ItemNameSingular + ">");
                        }
                        sw.WriteLine(@"</" + ItemNamePlural + ">");
                        sw.Flush();
                        sw.Close();
                        HttpContext.Current.Response.WriteFile(tmp);
                        HttpContext.Current.Response.End();
                    }
                }
                catch (Exception e)
                {
                    EnteredRowErrors.Add("*", "Export to " + filename + " failed: " + e.Message);
                }
			}
			#endregion
        }

		#endregion


		#region GridHeader

		public string GridHeader
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<div class='gridHeader'>");
                if (IsAdd || IsEdit || IsDelete)
                {
                    sb.AppendLine("<div style='border: 2px solid #66afe9; border-radius: 5px; padding: 10px; margin: 10px;'><span class='bold' style='padding-right: 20px;'>");
                    string actionTxt = "Save";
                    if      (IsAdd)    sb.AppendLine(@"Adding "  + ItemNameSingular);
                    else if (IsEdit)   sb.AppendLine(@"Editing " + ItemNameSingular);
                    else if (IsDelete)
                    {
                        if (SelectedRowLinkKeys.Count == 1) sb.AppendLine(@"Deleting " + ItemNameSingular);
                        else                                sb.AppendLine(@"Deleting " + SelectedRowLinkKeys.Count.ToString() + " "+ItemNamePlural);
                        actionTxt = "Confirm";
                    }
                    sb.AppendLine("</span>");
                    sb.AppendLine(@"<input type='button' class='gridButton' value='" + actionTxt + @"' onclick='$(""#" + ID + @"_isSave"").val(""true"");this.form.submit();' alt='"+actionTxt+" changes' title='"+actionTxt+" changes' data-ays-ignore=\"true\" />");
                    sb.AppendLine(@"<input type='button' class='gridButton' value='Cancel' onclick='$(""#" + ID + @"_isEdit"").val(""false"");$(""#" + ID + @"_isAdd"").val(""false"");$(""#" + ID + @"_isDelete"").val(""false"");this.form.submit();' alt='Cancel changes' title='Cancel changes' data-ays-ignore=""true"" />");
                    sb.Append(RenderError);
                    sb.AppendLine("</div>");
                }
                else
                {
					if (!IsReadOnly)
					{
						if ((AddActionController != null) && (AddActionController.ToUpper() != "NONE"))
						{
							if (InlineAdd != null)
							{
								sb.AppendLine(@"<input type='button' class='gridButton' value='" + AddButtonText + @"' onclick='$(""#" + ID + @"_isAdd"").val(""true"");this.form.submit();' alt='Add"+Article+ItemNameSingular+@"' title='Add"+Article+ItemNameSingular+@"' data-ays-ignore=""true"" />");
							}
							else
							{
								if ((AddActionController ?? "").Length > 0)
								{
									string addClickJS = "window.location$SEARCH$='" + AddActionController + "'";
									addClickJS = addClickJS.Replace("$SEARCH$", (AddActionController.StartsWith("?") ? ".search" : ""));
									sb.AppendLine(@"<input type='button' class='gridButton' value='" + AddButtonText + @"' onclick=""" + addClickJS.Replace("$ID$", ID) + @""" alt='Add"+Article+ItemNameSingular+@"' title='Add"+Article+ItemNameSingular+@"' data-ays-ignore=""true"" />");
								}
							}
						}
					}

					//if (GridRows.Count > 0)
                    if ( (EditActionController != null) && (!HideEditButton) && (EditActionController.ToUpper() != "NONE") && (GridRows.Count > 0))  // added gridrows condition july 2015
					{
                        if (InlineEdit != null)
                        {
                            sb.AppendLine(@"<input type='button' class='gridButton' value='Edit' onclick='$(""#" + ID + @"_isEdit"").val(""true"");this.form.submit();' alt='Edit the highlighted" + Article + ItemNameSingular + @"' title='Edit the highlighted" + Article + ItemNameSingular + @"' data-ays-ignore=""true"" />");
                        }
                        else
                        {
                            if ((EditActionController ?? "").Length > 0)
                            {
                                string editClickJS = "window.location$SEARCH$='" + EditActionController + (EditActionController.Contains("?") ? "&" : "?") + "primaryKey='+document.getElementById('$ID$_primaryKey').value;";
                                editClickJS = editClickJS.Replace("$SEARCH$", (EditActionController.StartsWith("?") ? ".search" : ""));
                                sb.AppendLine(@"<input type='button' class='gridButton' value='Edit' onclick=""" + editClickJS.Replace("$ID$", ID) + @""" alt='Edit the highlighted" + Article + ItemNameSingular + @"' title='Edit the highlighted" + Article + ItemNameSingular + @"' data-ays-ignore=""true"" />");
                            }
                        }
                    }

					if ( (!IsReadOnly) && (InlineDelete != null) && (GridRows.Count > 0) )  // added gridrows condition july 2015
					{
						if (IsDelete)
						{
							string deleteClickJS = "window.location$SEARCH$='" + DeleteActionController + (DeleteActionController.Contains("?") ? "&" : "?") + "primaryKey='+document.getElementById('$ID$_primaryKey').value;";
							deleteClickJS = deleteClickJS.Replace("$SEARCH$", (DeleteActionController.StartsWith("?") ? ".search" : ""));
							sb.AppendLine(@"<input type='button' class='gridButton' value='Delete' onclick=""" + deleteClickJS.Replace("$ID$", ID) + @""" alt='Delete the highlighted" + Article+ItemNameSingular + @"(s)' title='Delete the highlighted" + Article + ItemNameSingular + @"(s)' data-ays-ignore=""true"" />");
						}
						else
						{
							sb.AppendLine(@"<input type='button' class='gridButton' value='Delete' onclick='$(""#" + ID + @"_isDelete"").val(""true"");this.form.submit();' alt='Delete the highlighted" + Article + ItemNameSingular + @"(s)' title='Delete the highlighted" + Article + ItemNameSingular + @"(s)' data-ays-ignore=""true"" />");
						}
					}

                    if ( (TotalRows.Count > 1) && (!HideFilterButton) )
                    {
                        string filterClickJS;
                        if (IsFilter)
                        {
                            filterClickJS = "var searchParams=window.location.search; if (searchParams.length>1) { searchParams='&'+searchParams.substring(1); } window.location.search='?action=" + ID + @"_FiltOff';";
                            sb.AppendLine(@"<input type='button' class='gridButton' value='Filter Off' onclick=""" + filterClickJS + @""" alt='Turn Filter Off' title='Turn Filter Off' data-ays-ignore=""true"" />");
                        }
                        else
                        {
                            filterClickJS = "var searchParams=window.location.search; if (searchParams.length>1) { searchParams='&'; } window.location.search='?action=" + ID + @"_FiltOn'+searchParams;";
                            sb.AppendLine(@"<input type='button' class='gridButton' value='Filter' onclick=""" + filterClickJS + @""" alt='Turn Filter On' title='Turn Filter On' data-ays-ignore=""true"" />");
                        }
                    }

                    if (ExportTo_CSV)     sb.AppendLine(@"<input type='button' class='gridButton' value='Export to CSV' onclick='$(""#" + ID + @"_isExportCSV"").val(""true"");this.form.submit();' alt='Export filtered rows to CSV' title='Export filtered rows to CSV' data-ays-ignore=""true"" />");
                    if (ExportTo_XML)     sb.AppendLine(@"<input type='button' class='gridButton' value='Export to XML' onclick='$(""#" + ID + @"_isExportXML"").val(""true"");this.form.submit();' alt='Export filtered rows to XML' title='Export filtered rows to XML' data-ays-ignore=""true"" />");
                    if (!HidePrintButton) sb.AppendLine(@"<input type='button' class='gridButton' value='Print' onclick='window.print();' alt='Print page' title='Print page' data-ays-ignore=""true"" />");

                    sb.Append(RenderError);
                }
                sb.AppendLine("<div style='float: right;'>");
                foreach (ActionDefinition ad in actions)
                {
                    sb.AppendLine(@"<input type='button' class='gridButton' value='"+ad.labelTxt+"' alt='"+ad.altTxt+"' title='"+ad.altTxt+"' onclick=\"window.location.assign='"+ad.uri+"';\" data-ays-ignore=\"true\" />");
                }
                sb.AppendLine("</div>");
                sb.AppendLine("</div>");

                return sb.ToString();
            }
        }

		#endregion


		#region GridBody

		string GridBody
        {
            get
            {
                if (HasTotalRow)
                {
                    foreach (GridColumn gc in Columns)
                    {
                        if (gc.AddToTotalRow) gc.Total = 0;
                    }
                }

				/////////////////////////////////////////// BUILDING THE COLUMN HEADERS //////////////////////////////////////////////
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<div class='table-responsive';'>");
                sb.AppendLine("<table class='gridTable'>");
                sb.AppendLine("<tr class='gridHeaderRow'>");
                if (HasSelector) sb.AppendLine("<th class='width40'></th>");
                foreach (GridColumn gc in Columns)
                {
                    string th = "<th class='%3 minWidth" + gc.Width + "' alt='Sort by " + gc.Heading + "' title='Sort by " + gc.Heading + "' onclick='document.getElementById(\"" + ID + "_sortColumn\").value=\"%0\";$(\"form\").submit();'>%1</th>";
                    string token0 = (SortColumn == gc.ID ? "^" : "") + gc.ID ;
                    string token1 = gc.Heading_CSS.Length > 0 ? "<div class='" + gc.Heading_CSS + "'>%2</div>" : "%2";
                    string token2 = gc.Heading;
					string token3 = "";
					if (DisallowSorting)
					{
						token3 = ( gc.AlignRight ? "gridColumnHeaderSortlessRight" : "gridColumnHeaderSortless");
					}
					else
					{
						if (gc.AlignRight)
						{
								 if (SortColumn == gc.ID)       token3 = "gridColumnHeaderSortDownRight";
							else if (SortColumn == "^" + gc.ID) token3 = "gridColumnHeaderSortUpRight";
							else                                token3 = "gridColumnHeaderNoSortRight";
						}
						else
						{
								 if (SortColumn == gc.ID)       token3 = "gridColumnHeaderSortDown";
							else if (SortColumn == "^" + gc.ID) token3 = "gridColumnHeaderSortUp";
							else                                token3 = "gridColumnHeaderNoSort";
						}
					}
					if (RotateColumnHeaders) token3 += " rotate90";
					sb.AppendLine(th.Replace("%0", token0).Replace("%1", token1).Replace("%2", token2).Replace("%3", token3));
                }
                sb.AppendLine("</tr>");
                if (IsFilter) sb.Append(RenderFilter);
                if (IsAdd)    sb.Append(RenderAdd);
                if (IsEdit)   sb.Append(RenderEdit);

                if (GridRowsHeader != null)
                {
                    List<List<string>> flyRows = GridRowsHeader();
                    RenderFlyRows(sb, flyRows);
                }

                string emptyGroupingKey = "~+~+~+~+~";
                string lastGroupingKey = emptyGroupingKey;
                int groupKount = 0;

                int rowShade = -1;
                GridColumn.BoundField boundSort = new GridColumn.BoundField();
                if (SortColumn.Length>0) boundSort = this[SortColumn.Replace("^","")].BoundSortEffect;

                StringBuilder keys = new StringBuilder();

                if (GridRows.Count < 25)
                {
                    List<GridColumn> linkedColumns = new List<GridColumn>(5);
                    foreach (GridColumn gc in Columns)
                    {
                        if (gc.BoundLink.name.Length > 0) linkedColumns.Add(gc);
                    }

                    string delim = "";
                    foreach (object row in GridRows.OrderBy(z => z, new SortByColumn(boundSort, SortColumn.StartsWith("^"))).ToList())
                    {
                        foreach (GridColumn gc in linkedColumns)
                        {
                            keys.Append(delim + gc.BoundLink.fetchData(row, ""));
                            delim = "/";
                        }
                    }
                }

                foreach (object row in GridRows.OrderBy(z=>z, new SortByColumn(boundSort, SortColumn.StartsWith("^"))).Skip(PageOffset).Take(PageSize).ToList())
                {
                    // GWW - DECEMBER 2022
                    //
                    // Added functionality to allow a datagrid to navigate to several different views
                    // Add a property called "OverrideEditActionResultNumber" to the class the datagrid is based on
                    // Then amend the "editActionResult" property setting in the controller to multiple paths separated by |
                    // The grid will then use the path based on the OverrideEditActionResultNumber

                    if ( (row.GetType().GetProperty("OverrideEditActionResult") != null) && (EditActionControllerTokens.Contains("|")) )
                    {
                        string[] tokens = EditActionControllerTokens.Split('|');
                        int i = (int)row.GetType().GetProperty("OverrideEditActionResult").GetValue(row, null);
                        if (i > 0) EditActionController = tokens[i -1];
                    }



                    rowShade++;
                    if (rowShade == 2) rowShade = 0;

                    if (EveryRow != null) EveryRow(row);

                    string delim = "";
                    string thisGroupingKey = "";
                    foreach (GridColumn gc in Columns)
                    {
                        if (!gc.IsGroupingColumn) continue;
                        thisGroupingKey += delim + gc.BoundCell.fetchData(row, gc.CellFormat);
                        delim = "+|+";
                    }

                    if (lastGroupingKey != thisGroupingKey)
                    {
                        if (lastGroupingKey != emptyGroupingKey)
                        {
                            groupKount++;
                            if (ChangeFooter != null)
                            {
                                List<List<string>> flyRows = ChangeFooter(lastGroupingKey,thisGroupingKey,groupKount);
                                RenderFlyRows(sb, flyRows);
                            }
                        }

                        lastGroupingKey = thisGroupingKey;

                        if (ChangeHeader != null)
                        {
                            List<List<string>> flyRows = ChangeHeader(lastGroupingKey, thisGroupingKey, groupKount);
                            RenderFlyRows(sb, flyRows);
                        }
                    }

                    int linkKey = (int)boundSelectorLink.fetchData(row);

                    bool isEditRow = false;
                    if (IsEdit && SelectedRowLinkKeys[0] == linkKey)
                    {
                        isEditRow = true;
                        sb.Append("<tr>");
                    }
                    else
                    {
                        var rowClass = (rowShade == 0) ? "gridRow" : "gridRowAlternate";

                        var customRowClass = GetRowCssClass(row);
                        if (!string.IsNullOrWhiteSpace(customRowClass))
                            rowClass += " " + customRowClass;

                        sb.Append("<tr class='" + rowClass + "'>");
                    }

                    if (HasSelector)
                    {
                        string linkKeyTxt = linkKey.ToString();
                        string selectorID = ID + "_selector_" + linkKeyTxt;
                        bool   isChecked  = (SelectedRowLinkKeys.Contains(linkKey));

                        if ((IsEdit || IsDelete) && !isChecked)
                        {
                            sb.Append("<td style='width:20px;'></td>");
                        }
                        else
                        {
                            string checkedText = (isChecked ? "CHECKED" : "");
                            string actionText = "document.getElementById(\"" + ID + "_primaryKey\").value=" + linkKey + ";";
                            if (IsEdit) actionText = "return false;";
                            sb.Append("<td><input type='checkbox' ID='" + selectorID + "' name='" + selectorID + "' style='width:20px; margin-left:4px;' onclick='" + actionText + "' " + checkedText + " data-ays-ignore=\"true\" /></td>");
                        }
                    }




					/////////////////////////////////////////// BUILDING THE CELL CONTENT //////////////////////////////////////////////
                    foreach (GridColumn gc in Columns)
                    {
						string td;
						if (gc.WrapText)
							td = "<td class='gridCell" + (gc.AlignRight ? "Right" : "") + " minWidth" + gc.Width + "'><div>$cell</div></td>";
						else
							td = "<td class='gridCell" + (gc.AlignRight ? "Right" : "") + " minWidth" + gc.Width + "'><div>$cell</div></td>";
                        string cellText = gc.BoundCell.fetchData(row, gc.CellFormat).Replace("(null)","");
						string cellContentClass = "gridCellContent" + (gc.AlignRight ? "Right" : "");

                        if (gc.Cell_CSS.Length > 0) cellContentClass += " " + gc.Cell_CSS;
                        if (gc.Type == GridColumnType.Toggle)
                        {
                            cellText = (cellText == "Yes" ? gc.ToggleTrueText : gc.ToggleFalseText);
                        }

                        if (isEditRow && gc.CanEdit)
                        {
                            string v = cellText;
                            if (EnteredRow != null && EnteredRow.Keys.Contains(gc.ID)) v = EnteredRow[gc.ID];
                            v = v.Replace("'", "&#39;");

                            string err = "";
                            if (EnteredRowErrors != null && EnteredRowErrors.Keys.Contains(gc.ID)) err = EnteredRowErrors[gc.ID].Replace("'", "&#39;");

                            string idPrefix = "Edit_";

                            if (gc.Live) idPrefix = "Live_" + linkKey.ToString();
                            switch (gc.Type)
                            {
                                case GridColumnType.Toggle:
                                    cellText = "<input type='checkbox' id='Edit_" + ID + "_" + gc.ID + "' name='Edit_" + ID + "_" + gc.ID + "' class='" + gc.Heading_CSS + "' value='on'  style='border: 2px solid #66afe9; border-radius: 5px; margin: 10px 0px;' " + (cellText == gc.ToggleTrueText?"CHECKED":"") + " data-ays-ignore=\"true\" />";
                                    break;
                                case GridColumnType.Date:
                                    cellText=  "<input type='text' class='width85 " + gc.Heading_CSS + "' style='border: 2px solid #66afe9; border-radius: 5px; margin: 10px 0px;' data-jqui-dpicker-dateformat='dd M yy' data-jqui-type='datepicker' data-val='true' data-val-date='Must be a date.' data-val-required='Must select the date' id='" +idPrefix+ ID + "_" + gc.ID + "' name='"+idPrefix + ID + "_" + gc.ID + "' value='" + v + "' data-ays-ignore=\"true\" />";
                                    cellText+= "<span class='field-validation-valid' data-valmsg-for='"+idPrefix + ID + "_" + gc.ID + "' data-valmsg-replace='true'></span>";
                                    break;
                                default:
                                    cellText = "<input type='text' id='Edit_" + ID + "_" + gc.ID + "' name='Edit_" + ID + "_" + gc.ID + "' class='gridEditCell minWidth" + (gc.Width -10).ToString() + "' value='" + v + "' data-ays-ignore=\"true\" />";
                                    break;
                            }

                            if (err.Length > 0)
                            {
                                cellText+="<span class='red bold'>" + err + "</span><br />";
                            }
                        }
                        else
                        {
                            if (gc.BoundLink.name.Length > 0)
                            {
                                string cellLink = gc.LinkController + gc.BoundLink.fetchData(row, "");
                                if (gc.LinkController.Contains("?keys=&")) cellLink = cellLink.Replace("?keys=", "?keys=" + keys.ToString());

								if (OpenLinksInNewTab)
	                                cellText = "<a class='" + cellContentClass + "' href='" + cellLink + "' target='_blank'>" + cellText + "</a>";
								else
	                                cellText = "<a class='" + cellContentClass + "' href='" + cellLink + "'>" + cellText + "</a>";
                            }
                        }

						if (FullRowSelect)
						{
                            if (OpenLinksInNewTab)
								cellText = "<a class='" + cellContentClass + "' href='" + EditActionController + "?primaryKey=" + linkKey + "' target='_blank'>" + cellText + "</a></div></td>";
							else
								cellText = "<a class='" + cellContentClass + "' href='" + EditActionController + "?primaryKey=" + linkKey + "'>" + cellText + "</a></div></td>";
						}
						else
	                        cellText = "<a class='" + cellContentClass + "'>" + cellText + "</a>";

						sb.Append(td.Replace("$cell", cellText));

                        if (gc.AddToTotalRow) gc.Total += (double)(gc.BoundTotal.fetchData(row));
                    }

                    sb.Append("</tr>");
                }

                if (lastGroupingKey != emptyGroupingKey)
                {
                    if (ChangeFooter != null)
                    {
                        List<List<string>> flyRows = ChangeFooter(lastGroupingKey, "", groupKount);
                        RenderFlyRows(sb, flyRows);
                    }
                }

                if (HasTotalRow)
                {
                    sb.AppendLine("<tr>");
                    if (HasSelector) sb.AppendLine("<td></td>");
                    foreach (GridColumn gc in Columns)
                    {
                        if (gc.AddToTotalRow)
						{
							if (gc.Type == GridColumnType.Money)
								sb.AppendLine("<td><div class='gridTotalCell'>" + DCC_Formatting.Money((decimal)gc.Total, true) + "</div></td>");
							else
								sb.AppendLine("<td><div class='gridTotalCell'>" + gc.Total.ToString(gc.CellFormat) + "</div></td>");
						}
						else sb.AppendLine("<td><div></div></td>");
                    }
                    sb.AppendLine("</tr>");
                }

                if (GridRowsFooter != null)
                {
                    List<List<string>> flyRows = GridRowsFooter();
                    RenderFlyRows(sb, flyRows);
                }

				sb.Append("<tr><td colspan=100%>" + GridFooter + "</td></tr>");
                sb.AppendLine("</table>");
				sb.AppendLine("</div>");
                return sb.ToString();
            }
        }

		#endregion


		#region GridFooter

		public String GridFooter
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                int firstShown = PageOffset +1;
                int lastShown = Math.Min( (PageOffset + PageSize), CountFiltered);
                string shown = "";

				if (GridRows.Count > 0)
				{
					sb.Append("<div class='marginBottom30'>");

					sb.Append("<div class='gridFooterLeft'>");
					if (TotalRows.Count > PageSize)
					{
						sb.AppendLine("<input type='button' class='gridButton' value='Top'      name='" + ID + "_TopButton'    id='" + ID + "_TopButton'    title='Go to the first page'    alt='Go to the first page'    data-ays-ignore='true' onclick = 'if (document.getElementById(\""+ID+"_TopClick\"))      { document.getElementById(\""+ID+"_TopClick\")     .value = \"1\"; this.form.submit(); } ' />");
						sb.AppendLine("<input type='button' class='gridButton' value='Previous' name='" + ID + "_UpButton'     id='" + ID + "_UpButton'     title='Go to the previous page' alt='Go to the previous page' data-ays-ignore='true' onclick = 'if (document.getElementById(\""+ID+"_PreviousClick\")) { document.getElementById(\""+ID+"_PreviousClick\").value = \"1\"; this.form.submit(); } ' />");
						sb.AppendLine("<input type='button' class='gridButton' value='Next'     name='" + ID + "_DownButton'   id='" + ID + "_DownButton'   title='Go to the next page'     alt='Go to the next page'     data-ays-ignore='true' onclick = 'if (document.getElementById(\""+ID+"_NextClick\"))     { document.getElementById(\""+ID+"_NextClick\")    .value = \"1\"; this.form.submit(); } ' />");
						sb.AppendLine("<input type='button' class='gridButton' value='Bottom'   name='" + ID + "_BottomButton' id='" + ID + "_BottomButton' title='Go to the last page'     alt='Go to the last page'     data-ays-ignore='true' onclick = 'if (document.getElementById(\""+ID+"_BottomClick\"))   { document.getElementById(\""+ID+"_BottomClick\")  .value = \"1\"; this.form.submit(); } ' />");
					}
					sb.Append("</div>");
					if ((lastShown -firstShown -1 != CountFiltered) && (firstShown != lastShown && CountFiltered>0)) shown = firstShown.ToString("#,###,###,##0") + " - " + lastShown.ToString("#,###,###,##0") + " of ";

					sb.Append("<div class='gridFooterRight'>");
					if (!HideShowAllButton)
					{
						sb.AppendLine("<input type='button' class='gridButton' value='Show All'              name='" + ID + "_ShowAll'      id='" + ID + "_ShowAll'      title='Show all rows'              alt='Show all rows'              data-ays-ignore='true' onclick = 'if (document.getElementById(\""+ID+"_ShowAll\"))      { document.getElementById(\""+ID+"_ShowAll\")      .value = \"1\"; this.form.submit(); } ' />");
						sb.AppendLine("<input type='button' class='gridButton' value='Show " + PageSize + "' name='" + ID + "_ShowPageSize' id='" + ID + "_ShowPageSize' title='Show " + PageSize + " rows' alt='Show " + PageSize + " rows' data-ays-ignore='true' onclick = 'if (document.getElementById(\""+ID+"_ShowPageSize\")) { document.getElementById(\""+ID+"_ShowPageSize\") .value = \"1\"; this.form.submit(); } ' />");
					}
					sb.Append("Showing " + shown + CountFiltered.ToString("#,###,###,##0") + " " + (CountFiltered == 1 ? ItemNameSingular : ItemNamePlural) + (IsFilter ? " filtered from " + Count.ToString() : ""));
					sb.Append("</div>");
					
					sb.Append("</div>");
				}

				return sb.ToString();
            }
        }

		#endregion


		#region Other Render Methods

		private static void RenderFlyRows(StringBuilder sb, List<List<string>> flyRows)
        {
            foreach (List<string> flyRow in flyRows)
            {
                StringBuilder fr = new StringBuilder();
                fr.Append("<tr>");
                if (flyRow.Count > 0)
                {
                    foreach (string s in flyRow) fr.Append("<td>" + s + "</td>");
                }
                else fr.Append("<td>&nbsp;</td>");
                fr.Append("</tr>");
                sb.AppendLine(fr.ToString());
            }
        }

        string RenderFilter
        {
            get
            {
				StringBuilder sb = new StringBuilder();
                sb.Append("<tr>");
                if (HasSelector) sb.Append("<td></td>");
                foreach (GridColumn gc in Columns)
                {
					if (gc.SelectOptions != null)
					{
						string select = "<select class='minWidth" + (gc.Width -20).ToString() + "' id='ddl" + gc.ID + "' name='ddl" + gc.ID + "' onchange = 'form.submit();' data-ays-ignore=\"true\">";
						select += "<option value='-' disabled='true' selected='true'>-</option>";
						foreach (object selectOption in gc.SelectOptions)
						{
							string a1 = (string)selectOption.GetType().GetProperty("Key").GetValue(selectOption, null);
							string a2 = (string)selectOption.GetType().GetProperty("Description").GetValue(selectOption, null);
							foreach (object o in TotalRows)
							{
								string s = (string)o.GetType().GetProperty(gc.BoundCell.name).GetValue(o, null);
								if (s == a2)
								{
									select += "<option value='" + a2 + "'>" + a2 + "</option>";
									break;
								}
							}
						}
						select += "</select>";
						sb.Append("<td class='gridCell'>" + select + "</td>");
					}
					else
					{
						sb.Append("<td class='gridCell'><input class='gridFilterCell minWidth" + (gc.Width -20).ToString() + "' id='" + gc.ID + "' name='" + gc.ID + "' type='text' onchange = 'form.submit();' data-ays-ignore=\"true\" class=\"" + gc.Heading_CSS + "\" value='" + HttpContext.Current.Server.HtmlEncode(gc.Filter) + "'></td>");
					}
				}
                sb.Append("</tr>");
                return sb.ToString();
            }
        }

        string RenderAdd
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<tr>");
                if (HasSelector) sb.Append("<td></td>");
                foreach (GridColumn gc in Columns)
                {
                    sb.Append("<td>");
                    string v="";
                    if (EnteredRow != null && EnteredRow.Keys.Contains(gc.ID)) v = EnteredRow[gc.ID].Replace("'", "&#39;");

                    if (gc.CanInsert)
                    {
                        switch (gc.Type)
                        {
                            case GridColumnType.Toggle:
                            {
                                sb.Append("<input type='checkbox' id='Add_" + ID + "_" + gc.ID + "' name='Add_" + ID + "_" + gc.ID + "' class='" + gc.Heading_CSS + "' value='on'  style='border: 2px solid #66afe9; border-radius: 5px; margin: 10px 0px;' " + (v == gc.ToggleTrueText ? "CHECKED" : "") + " data-ays-ignore=\"true\" />");
                                break;
                            }
                            case GridColumnType.Date:
                            {
                                sb.Append("<input type='text' class='form-control width85 "+gc.Heading_CSS+"' style='border: 2px solid #66afe9; border-radius: 5px; margin: 10px 0px;' data-jqui-dpicker-dateformat='dd M yy' data-jqui-type='datepicker' data-val='true' data-val-date='Must be a date.' data-val-required='Must select the date' id='Add_" + ID + "_" + gc.ID + "' name='Add_" + ID + "_" + gc.ID + "' value='"+v+"' data-ays-ignore=\"true\" />");
                                sb.Append("<span class='field-validation-valid' data-valmsg-for='Add_" + ID + "_" + gc.ID+"' data-valmsg-replace='true'></span>");
                                break;
                            }
                            default:
                            {
                                sb.Append("<input type='text' id='Add_" + ID + "_" + gc.ID + "' name='Add_" + ID + "_" + gc.ID + "' class='" + gc.Heading_CSS + "' value='" + v + "'  style='border: 2px solid #66afe9; border-radius: 5px; margin: 10px 0px;' data-ays-ignore=\"true\" />");
                                break;
                            }
                        }

                        string err = "";
                        if (EnteredRowErrors != null && EnteredRowErrors.Keys.Contains(gc.ID)) err = EnteredRowErrors[gc.ID].Replace("'", "&#39;");
                        if (err.Length > 0) sb.Append("<br /><span class='red bold'>" + err + "</span>");
                    }
                    else sb.Append("<span class='" + gc.Heading_CSS + "'>" + v + "</span>");

                    if (EnteredRowErrors != null && EnteredRowErrors.Keys.Contains("*"))
                    {
                        string err = EnteredRowErrors["*"].Replace("'", "&#39;");
                        sb.Append("<br /><span class='red bold'>" + err + "</span>");
                        EnteredRowErrors.Remove("*");
                    }

                    sb.Append("</td>");

                }
                sb.Append("</tr>");
                return sb.ToString();
            }
        }

        string RenderEdit
        {
            get
            {
                if (EnteredRowErrors == null) return "";
                if (!EnteredRowErrors.ContainsKey("*")) return "";
                string wholeRowError = EnteredRowErrors["*"];
                EnteredRowErrors.Remove("*");
                return "<tr style='background-color: wheat;'><td></td><td colspan='99'><span class='red bold' style='border: 2px solid #66afe9; border-radius: 5px; padding: 10px; margin: 10px;'>" + wholeRowError + "</span></td></tr>";
            }
        }

        string RenderError
        {
            get
            {
                if (EnteredRowErrors == null) return "";
                if (!EnteredRowErrors.ContainsKey("*")) return "";
                string wholeRowError = EnteredRowErrors["*"];
                EnteredRowErrors.Remove("*");
                return "<span class='red bold' style='padding: 10px; margin: 10px;'>" + wholeRowError + "</span>";
            }
        }

		#endregion

    }


	#region GridColumn Class

	public enum GridColumnType {Text, Digits, Money, Date, Time, Toggle, Option };

    public class GridColumn
    {

		// Properties

        public string         ID;
        public GridColumnType Type;
        public string         Heading;
		public int            Width;
        public string         Heading_CSS = "";
        public string         Cell_CSS = "";
        public string         Filter = "";
        public double         Total = 0;
        public string         BoundCellName;
        public string         BoundSortName;
        public string         BoundLinkName;
        public string         TrueFalse;
        public BoundField     BoundCell;
        public BoundField     BoundSort;
        public BoundField     BoundLink;
        public BoundField     BoundTotal;
        public string         LinkController;
        public string         Format = "";
        public string         CellFormat = "";
        public string         NullFormat = "-";
        public bool           IsGroupingColumn = false;
        public bool           CanEdit = true;
        public bool           CanInsert = true;
        public bool           OnlyExport = false;
        public bool           OnlyDisplay = false;
		public List<object>   SelectOptions;
		public bool           AlignRight = false;
        public bool           Live = false;
        public string         ToggleTrueText = "Yes";
        public string         ToggleFalseText = "No";
		public bool           DefaultSortColumn;
		public bool           DefaultSortDescending;
		public bool           AddToTotalRow = false;
		public bool           WrapText = false;



		// Display Properties

        public BoundField BoundSortEffect { get { return (BoundSort.name.Length > 0 ? BoundSort : BoundCell); } }



		// Utilities

        public override string ToString() { return "ID = " + ID; }



		// Constructors

        public GridColumn(
			GridColumnType type,
			string         boundField = "",
			string         heading = "Heading",
			int            width = 100,
			string         id = "",
			string         boundSortField = "",
			string         boundLink = "",
			string         heading_CSS = "",
			string         cell_CSS = "",
			//string         linkController = "",
			List<object>   selectOptions = null,
			bool           alignRight = false,
			string         trueFalse = "",
			bool           defaultSortColumn = false,
			bool           defaultSortDescending = false,
			bool           addToTotalRow = false,
			bool           wrapText = false,
			bool           canEdit = true)
        {
            Type = type;
			BoundCellName = boundField;
			BoundSortName = boundSortField;
			BoundLinkName = boundLink;
            Heading = heading;
			Width = width;
            ID = id;
            Heading_CSS = heading_CSS;
            Cell_CSS = cell_CSS;
			//LinkController = linkController;
			AlignRight = ( (alignRight) || (Type == GridColumnType.Money) || (Type == GridColumnType.Date) || (Type == GridColumnType.Digits) );
			TrueFalse = trueFalse;
			DefaultSortColumn = defaultSortColumn;
			DefaultSortDescending = defaultSortDescending;
			AddToTotalRow = addToTotalRow;
			WrapText = wrapText;
			CanEdit = canEdit;

            if (cell_CSS.Length == 0) Cell_CSS = Heading_CSS;
            switch (type)
            {
                case GridColumnType.Text:
                {
                    break;
                }
                case GridColumnType.Digits:
                case GridColumnType.Money:
                case GridColumnType.Date:
                {
                    //Cell_CSS += " alignRight padRight7";  removed jan 2020
                    Cell_CSS += " alignRight";
                    break;
                }
                case GridColumnType.Time:
                {
                    break;
                }
                case GridColumnType.Toggle:
                {
                    break;
                }
            }
            SelectOptions = selectOptions;
        }

        internal string  fetchCell (object pagedRow) { return BoundCell.fetchData(pagedRow, ((Type == GridColumnType.Date) || (Type == GridColumnType.Money)) ? Format : "");  }
        internal decimal fetchValue(object pagedRow) { return BoundCell.fetchData(pagedRow);   }
        internal string  fetchLink (object pagedRow) { return BoundLink.fetchData(pagedRow, ""); }

        /// <summary>
        /// Structure to access fields and properties by Reflection
        /// </summary>
        public struct BoundField
        {
            /// <summary>
            /// Formatted string usefull for debugging
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return "[" + name + "] " + (isUndetermined ? "Undetermined" : "") + (isField ? "Field" : "") + (isProperty ? "Property" : "");
            }
            /// <summary>
            /// true if given name hasn't been determined to be Field or Property yet
            /// </summary>
            public bool isUndetermined;
            /// <summary>
            /// true if given name is a Field
            /// </summary>
            public bool isField;
            /// <summary>
            /// true if given name is a Property
            /// </summary>
            public bool isProperty;
            /// <summary>
            /// given name
            /// </summary>
            public string name;

            /// <summary>
            /// Info
            /// </summary>
            public PropertyInfo propInfo;

            /// <summary>
            /// Info
            /// </summary>
            public FieldInfo fieldInfo;

            ///// <summary>
            ///// constructor
            ///// </summary>
            //public BoundField()
            //{
            //    name = "";
            //    isUndetermined = true;
            //    isField = false;
            //    isProperty = false;
            //    fieldInfo = null;
            //    propInfo = null;
            //}
            /// <summary>
            /// constructor
            /// </summary>
            /// <param name="pName">given name</param>
            public BoundField(string pName)
            {
                name = pName;
                isUndetermined = true;
                isField = false;
                isProperty = false;
                fieldInfo = null;
                propInfo = null;
            }
            /// <summary>
            /// Determines which sort of member the given name is
            /// </summary>
            /// <param name="pagedRow"></param>
            private void determineField_or_Property(object pagedRow)
            {
                if (pagedRow == null) throw new NullReferenceException("Object passed to DCC_Grid.boundField.determineField_or_Property() is null " + this.ToString());

                isUndetermined = false;
                isField = false;
                isProperty = false;
                if (name == null) return;
                if (name.Length == 0) return;

                PropertyInfo[] px = pagedRow.GetType().GetProperties();
                foreach (PropertyInfo pi in px)
                {
                    if (pi.Name == name)
                    {
                        isProperty = true;
                        propInfo = pi;
                        return;
                    }
                }

                FieldInfo[] fx = pagedRow.GetType().GetFields();
                foreach (FieldInfo fi in fx)
                {
                    if (fi.Name == name)
                    {
                        isField = true;
                        fieldInfo = fi;
                        return;
                    }
                }

                throw new Exception("DCC_Grid.boundField [" + this.ToString() + "] is neither a field nor a property");
            }
            /// <summary>
            /// Extracts member value
            /// </summary>
            /// <param name="obj">containing object</param>
            /// <returns>string from given field/property</returns>
            public string getString(object obj)
            {
                return fetchData(obj, "");
            }

            /// <summary>
            /// Extracts member value with format
            /// </summary>
            /// <param name="item">containing object</param>
            /// <param name="format">required format (only relevant for DateTime and number objects) Can be two | separated strings - the second is for 0 values</param>
            /// <returns>string from given field/property</returns>
            public string fetchData(object item, string format)
            {
                object cellItem = fetchRaw(item);
                string[] formatTokens = format.Split('|');
                if (formatTokens.Length == 1) formatTokens = (format + "|" + format).Split('|');
                if (cellItem == null)     return "(null)";
                if (cellItem is string)   return (string)cellItem;
                if (cellItem is bool)     return ((bool)cellItem ? "Yes" : "No");
                if (cellItem is byte)     { byte     ci = (byte)cellItem;     return (ci == 0 ? ci.ToString(formatTokens[1]) : ci.ToString(formatTokens[0])); }
                if (cellItem is short)    { short    ci = (short)cellItem;    return (ci == 0 ? ci.ToString(formatTokens[1]) : ci.ToString(formatTokens[0])); }
                if (cellItem is int)      { int      ci = (int)cellItem;      return (ci == 0 ? ci.ToString(formatTokens[1]) : ci.ToString(formatTokens[0])); }
                if (cellItem is long)     { long     ci = (long)cellItem;     return (ci == 0 ? ci.ToString(formatTokens[1]) : ci.ToString(formatTokens[0])); }
                if (cellItem is float)    { float    ci = (float)cellItem;    return (ci == 0 ? ci.ToString(formatTokens[1]) : ci.ToString(formatTokens[0])); }
                if (cellItem is double)   { double   ci = (double)cellItem;   return (ci == 0 ? ci.ToString(formatTokens[1]) : ci.ToString(formatTokens[0])); }
                if (cellItem is decimal)  { decimal  ci = (decimal)cellItem;  return (ci == 0 ? ci.ToString(formatTokens[1]) : ci.ToString(formatTokens[0])); }
                if (cellItem is DateTime) { DateTime ci = (DateTime)cellItem; return (ci.Year < 1980 ? ci.ToString(formatTokens[1]) : ci.ToString(formatTokens[0])); }
                return "\"" + name + "\" is unexpected type [" + cellItem.GetType() + "]";
            }

            /// <summary>
            /// Extracts numeric value of any cell
            /// </summary>
            /// <param name="item">containing object</param>
            /// <returns>decimal value of cell</returns>
            public decimal fetchData(object item)
            {
                object cellItem = fetchRaw(item);
                if (cellItem == null)    return 0;
                if (cellItem is short)   return (decimal)((short)cellItem);
                if (cellItem is int)     return (decimal)((int)cellItem);
                if (cellItem is long)    return (decimal)((long)cellItem);
                if (cellItem is float)   return (decimal)((float)cellItem);
                if (cellItem is double)  return (decimal)((double)cellItem);
                if (cellItem is decimal) return (decimal)cellItem;
                return 0;
            }

            /// <summary>
            /// Extracts raw object from any cell
            /// </summary>
            /// <param name="item">containing object</param>
            /// <returns>cell contents as an object</returns>
            public object fetchRaw(object item)
            {
                if (isUndetermined) determineField_or_Property(item);
                if (!isField && !isProperty) return "\"" + name + "\" is not a property or a field.";

				object cellItem = null;
                if (isField)    cellItem = item.GetType().GetField(name).GetValue(item);
                if (isProperty) cellItem = DataBinder.Eval(item, name);
                return cellItem;
            }
        }

    }

	#endregion

}
