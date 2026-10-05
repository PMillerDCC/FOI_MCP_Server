using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DCC_Standard
{

    #region Attributes for DCC field classes

    public class ui_Label : Attribute
    {   string it;
        public string val() { return (val(true)); }
        public string val(bool makeNBSP) { return (makeNBSP?it.Replace(" ","&nbsp;"):it); }
        public ui_Label(string v) {it=v;}
    }

    public class ui_MaxLen : Attribute
    {   int it;
        public int val { get { return it; } }
        public ui_MaxLen(int v) {it=v;}
    }

    public class ui_EntryMask : Attribute
    {   string it;
        public string val { get { return it; } }
        public ui_EntryMask(string v) {it=v;}
    }

    public class rv_MinMax : Attribute
    {   string minV;
        string maxV;
        public string minVal { get { return minV; } }
        public string maxVal { get { return maxV; } }
        public rv_MinMax(string vMin,string vMax) {minV=vMin.Replace(",","");maxV=vMax.Replace(",","");}
    }

    public class rv_Type : Attribute
    {   ValidationDataType VDT;
        public ValidationDataType vdt { get { return VDT; } }
        public rv_Type(string vType)
        {
            switch (vType)
            {
                case "DT": VDT = ValidationDataType.Date;     break;
                case "F":  VDT = ValidationDataType.Double;   break;
                case "I":  VDT = ValidationDataType.Integer;  break;
                case "D":  VDT = ValidationDataType.Currency; break;
                case "S":  VDT = ValidationDataType.String;   break;
                default:   VDT = ValidationDataType.String;   break;
            }
        }
    }

    public class rev_RegExp : Attribute
    {   string it;
        public string val { get { return it; } }
        public rev_RegExp(string v) {it=v;}
    }

    public class val_Error : Attribute
    {   string it;
        public string val { get {return it;}}
        public val_Error(string v) {it=v;}
    }

    public class val_Summary : Attribute
    {   string it;
        public string val { get {return it;}}
        public val_Summary(string v) {it=v;}
    }

    #endregion


    #region DCC Fields

    [rv_Type("B")]
    public abstract class DCC_BooleanField
    {
        protected bool it;
        public bool raw { get { return it; } }
        public override string ToString() { return (it?"True":"False"); }
        public virtual string txt   { get { return (it?"True":"False");} }
        public string txtOnOff      { get { return (it?  "On":"Off"  );} }
        public string txtYesNo      { get { return (it? "Yes":"No"   );} }
        public string txt10         { get { return (it?   "1":"0"    );} }

        public DCC_BooleanField() { it = false; }
        public DCC_BooleanField(object value)
        {
            if (value is bool )       { it =         (bool)value;          return; }
            if (value is CheckBox)    { it =    ((CheckBox)value).Checked; return; }
            if (value is RadioButton) { it = ((RadioButton)value).Checked; return; }
            throw new ArgumentException("No code written yet to construct a DCC_BooleanField from type " + value.GetType().ToString());
        }
        public virtual void exDB(object dataRow_or_dataReader, string columnName) { it = DCC_GetColumn.Bit(dataRow_or_dataReader, columnName); }
        public static implicit operator bool(DCC_BooleanField v) { return v.it; }
    }
    

    
    [rv_Type("S")]
    [ui_MaxLen(20)]
    [ui_Label("")]
    [rev_RegExp(".*")]
    [val_Error("")]
    [val_Summary("")]
    public abstract class DCC_StringField
    {
        protected string it;
        public string raw { get { return it; } }
        public override string ToString() { return it; }
        public virtual string txt { get { return it; } }
        public int maxLen { get { return ((ui_MaxLen)Attribute.GetCustomAttribute(GetType(), typeof(ui_MaxLen))).val; } }
        public string label { get { return ((ui_Label)Attribute.GetCustomAttribute(GetType(), typeof(ui_Label))).val(); } }
        public string regExp { get { return ((rev_RegExp)Attribute.GetCustomAttribute(GetType(), typeof(rev_RegExp))).val; } }
        public string error { get { return ((val_Error)Attribute.GetCustomAttribute(GetType(), typeof(val_Error))).val; } }
        public string summary { get { return ((val_Summary)Attribute.GetCustomAttribute(GetType(), typeof(val_Summary))).val; } }

        public DCC_StringField() { it = ""; }
        public DCC_StringField(object value)
        {
            if (value is string ) { it =   (string)value;       return; }
            if (value is TextBox) { it = ((TextBox)value).Text; return; }
            if (value is int    ) { it =     ((int)value).ToString(); return; }
            throw new ArgumentException("No code written yet to construct a DCC_StringField from type " + value.GetType().ToString());
        }
        public DCC_StringField(object dr, string colName) { it = DCC_GetColumn.String(dr, colName); }
        public virtual void exDB(object dataRow_or_dataReader, string columnName) { it = DCC_GetColumn.String(dataRow_or_dataReader, columnName); }

        public static implicit operator string(DCC_StringField v) { return v.it; }
    }

    
    
    [rv_MinMax("-2,147,483,648","2,147,483,64")]
    [rv_Type("I")]
    public abstract class DCC_IntBase 
    {
        protected int it;
        public int value { get { return it; } set { it = value; } }
        public string raw { get { return it.ToString(); } }
        public override string ToString() { return it.ToString("0"); }
        public virtual string txt { get { return it.ToString(); } }
        public virtual string sortTxt { get { return it.ToString("0").PadLeft(maxLen); } }

        public DCC_IntBase() { it = 0; }
        public DCC_IntBase(object value)
        {
            if (value is short  ) { it =    (short)value;       return; }
            if (value is byte   ) { it =     (byte)value;       return; }
            if (value is int    ) { it =      (int)value;       return; }
            if (value is string)
            {
                string s = (string)value;
                if (int.TryParse((s.Trim().Length == 0 ? "0" : s), out it)) return;
                throw new FormatException("String is not numeric: [" + s + "]");
            }
            if (value is TextBox)
            {
                string s = ((TextBox)value).Text;
                if (int.TryParse((s.Trim().Length == 0 ? "0" : s), out it)) return;
                throw new FormatException("TextBox text field is not numeric: [" + s + "]");
            }
            throw new ArgumentException("No code written yet to construct a DCC_IntField from type " + value.GetType().ToString());
            //  Although could write constructors from long,double,decimal etc seems better to force developer to have to explicity cast to int thereby forcing the developer to think "is this right?"
        }
        public DCC_IntBase(object dataRow_or_dataReader, string columnName)
        {
            if (dataRow_or_dataReader is DataRow)       { it = getInt(     ((DataRow)dataRow_or_dataReader)[columnName]); return; }
            if (dataRow_or_dataReader is DbDataReader)  { it = getInt(((DbDataReader)dataRow_or_dataReader)[columnName]); return; }
            throw new ArgumentException("No code written yet to construct a DCC_IntField from type " + dataRow_or_dataReader.GetType().ToString());
        }

        private int getInt(object dbColumnItem) 
        {   
            try 
            { 
                if (Convert.IsDBNull(dbColumnItem)) return 0; 
                if (dbColumnItem is byte) return (byte)dbColumnItem; 
                if (dbColumnItem is short) return (short)dbColumnItem; 
                if (dbColumnItem is int) return (int)dbColumnItem; 
                if (dbColumnItem is string)
                {
                    string s=(string)dbColumnItem;
                    int i=0;
                    if (!int.TryParse((s.Trim().Length==0?"0":s), out i))  i=0;  // this will convert invalid data to 0
                    return i;
                }
            } catch { return 0; } 
            throw new ArgumentException("No code written yet to construct a DCC_IntField from database type " + dbColumnItem.GetType().ToString());
        }

        public static implicit operator decimal(DCC_IntBase v) { return v.it; }
        public int maxLen { get { return ((ui_MaxLen)Attribute.GetCustomAttribute(GetType(), typeof(ui_MaxLen))).val; } }
        public virtual void exDB(object dataRow_or_dataReader, string columnName) { it = DCC_GetColumn.Integer32(dataRow_or_dataReader, columnName); }
    }

    public class DCC_DB_Key : DCC_IntBase
    {
        public int DB_Key { get { return it; } set { it = value; } }
        protected static int unknownKeyValue { get { return -1; } }
        public DCC_DB_Key(object value) : base(value) { }
        public DCC_DB_Key(object dataRow_or_dataReader, string columnName) : base(dataRow_or_dataReader, columnName) { }
        public static bool operator ==(DCC_DB_Key firstKey,DCC_DB_Key secondKey)
        {
            if (System.Object.ReferenceEquals(firstKey, secondKey)) return true;

            if ((object)firstKey == null) return false;
            if ((object)secondKey == null) return false;

            // Return true if the fields match:
            return (firstKey.it == secondKey.it);
        }
        public static bool operator !=(DCC_DB_Key firstKey,DCC_DB_Key secondKey)
        {
            return ! (firstKey == secondKey);
        }
        public override bool Equals(System.Object obj)
        {
            if (obj == null) return false;

            DCC_DB_Key p = obj as DCC_DB_Key;
            if ((System.Object)p == null) return false;

            return (it == p.it);
        }
        public override int GetHashCode()
        {
            return it;  
        }    
    }
    public class DCC_DB_Key_EqualityComparer : IEqualityComparer<DCC_DB_Key>
    {
        public bool Equals(DCC_DB_Key v1, DCC_DB_Key v2) { return v1 == v2; }
        public int GetHashCode(DCC_DB_Key value)  { return value.GetHashCode(); }
   }

    public class DCC_DB_Primarykey : DCC_DB_Key
    {
        public static DCC_DB_Primarykey unknown { get { return new DCC_DB_Primarykey(DCC_DB_Key.unknownKeyValue); } }
        public DCC_DB_Primarykey(object value) : base(value) { }
        public DCC_DB_Primarykey(object dataRow_or_dataReader, string columnName) : base(dataRow_or_dataReader, columnName) { }
    }
    public class DCC_DB_ForeignKey : DCC_DB_Key
    {
        public static DCC_DB_ForeignKey unknown { get { return new DCC_DB_ForeignKey(DCC_DB_Key.unknownKeyValue); } }
        public DCC_DB_ForeignKey(object value) : base(value) { }
        public DCC_DB_ForeignKey(object dataRow_or_dataReader, string columnName) : base(dataRow_or_dataReader, columnName) { }
    }

    public class DCC_Enum : DCC_IntBase
    {
        public int Enum_Value { get { return it; } set { it = value; } }
        public DCC_Enum(object value) : base(value) { }
        public DCC_Enum(object dataRow_or_dataReader, string columnName) : base(dataRow_or_dataReader, columnName) { }
    }

    public abstract class DCC_IntField : DCC_IntBase
    {
        public int absValue { get { return Math.Abs(value); } }

        public DCC_IntField() : base() {}
        public DCC_IntField(object value) : base(value) {}
        public DCC_IntField(object dataRow_or_dataReader, string columnName) : base(dataRow_or_dataReader,columnName) {}
    }


    
    [rv_MinMax("-79,228,162,514,264,337,593,543,950,335","79,228,162,514,264,337,593,543,950,335")]
    [rv_Type("D")]
    public abstract class DCC_MoneyField 
    {
        private decimal it;
        public decimal value { get { return it; }  set { it = value; }}
        public decimal absValue { get { return Math.Abs(it); } }
        public override string ToString() { return it.ToString("0.00"); }
        public virtual string txt { get { return DCC_Formatting.Money(it,true); } }
        public virtual string sortTxt { get { return it.ToString("0.00").PadLeft(maxLen); } }
        public string raw { get { return it.ToString(); } }
        public DCC_MoneyField() { it = 0; }
        public DCC_MoneyField(decimal value) {it = value;}
        public DCC_MoneyField(TextBox tb)
        {
            if (!decimal.TryParse((tb.Text.Length==0?"0":tb.Text), out it))
            {
                throw new FormatException("TextBox text field is not numeric: [" + tb.Text + "]");
            }
        }
        public static implicit operator decimal(DCC_MoneyField v) { return v.it; }
        public int maxLen { get { return ((ui_MaxLen)Attribute.GetCustomAttribute(GetType(), typeof(ui_MaxLen))).val; } }
        public virtual void exDB(object dataRow_or_dataReader, string columnName) { it = DCC_GetColumn.Decimal(dataRow_or_dataReader, columnName); }
    }


    
    [rv_MinMax("1900 Jan 1 00:00","2019 Dec 31 23:59:59")]
    [rv_Type("DT")]
    public abstract class DCC_DateTimeField 
    {
        protected DateTime? it;
        public DateTime? value { get { return it; }  set { it = value; }}
        public bool HasValue { get { return it.HasValue; } }
        public override string ToString() { return it.Value.ToString("yyyy/MM/dd HH:mm:ss"); }
        public virtual string txt { get { return DCC_Formatting.NullDateTxt(it,"yyyy MMM dd HH:mm:ss","--"); } }
        public virtual string sortTxt { get { return DCC_Formatting.NullDateTxt(it,"yyyyMMddHHmmss","--------------"); } }
        public DateTime? raw { get { return it; } }
        public bool laterThan(DateTime? referenceDate)
        {
            if (!it.HasValue) return true;
            return (it.Value >= referenceDate.Value);
        }
        public bool earlierThan(DateTime? referenceDate)
        {
            if (!it.HasValue) return false;
            return (it.Value <= referenceDate.Value);
        }

        public DCC_DateTimeField() { it = new DateTime?(); }
        public DCC_DateTimeField(DateTime value) {it = value;}
        public DCC_DateTimeField(DateTime? value) {it = (value.HasValue ? value.Value.Date : value);}
        public DCC_DateTimeField(string s)       { populateFromString(s); }
        public DCC_DateTimeField(TextBox tb)     { populateFromString(tb.Text); }
        public DCC_DateTimeField(Label lab)      { populateFromString(lab.Text); }

        private void populateFromString(string s)
        {
            DateTime itNN = new DateTime(1900, 1, 1);
            if (!DateTime.TryParse((s.Length == 0 ? "" : s), out itNN))
            {
                throw new FormatException("Not a valid date and time: [" + s + "]");
            }
            else
            {
                it = itNN;
            }
        }
        public static implicit operator DateTime(DCC_DateTimeField v) { return v.it.Value; }
        public int maxLen { get { return 20; } }

        public virtual void exDB(object dataRow_or_dataReader, string columnName) { it = DCC_GetColumn.DateTimeQ(dataRow_or_dataReader, columnName); }
    }

    [rv_MinMax("1900 Jan 1","2019 Dec 31")]
    [rv_Type("DT")]
    public abstract class DCC_DateField : DCC_DateTimeField
    {
        public override string ToString() { return it.Value.ToString("yyyy/MM/dd"); }
        public new virtual string txt { get { return DCC_Formatting.NullDateTxt(it,"yyyy MMM dd","--"); } }
        public new virtual string sortTxt { get { return DCC_Formatting.NullDateTxt(it,"yyyyMMdd","--------"); } }

        public DCC_DateField() { it = new DateTime?(); }
        public DCC_DateField(DateTime value) {it = value.Date;}
        public DCC_DateField(DateTime? value) { it = (value.HasValue ? value.Value.Date : value); }
        public DCC_DateField(string s)       { populateFromString(s); }
        public DCC_DateField(TextBox tb)     { populateFromString(tb.Text); }
        public DCC_DateField(Label lab)      { populateFromString(lab.Text); }

        private void populateFromString(string s)
        {
            DateTime itNN = new DateTime(1900, 1, 1);
            if (!DateTime.TryParse((s.Length == 0 ? "" : s), out itNN))
            {
                throw new FormatException("Not a valid date: [" + s + "]");
            }
            else
            {
                it = itNN.Date;
            }
        }
        public static implicit operator DateTime(DCC_DateField v) { return v.it.Value; }
        public new int maxLen { get { return 11; } }

        /// <summary>
        /// Returns a period number based on April=1 .... March=12
        /// </summary>
        public int FinancialYearMonth
        {
            get
            {
                if (!raw.HasValue) return 0;
                int fym = raw.Value.Month;
                fym += (fym < 4 ? 9 : -3);
                return fym;
            }
        }
    }

    #endregion


    #region FormHelper

    public class FormHelper
    {

        public static void SetInput<T>(Page thisPage,Control inputControl,Control labelControl, Control displayOnlyControl)
        {
            Type tt = typeof(T);
            ui_Label uiL = (ui_Label)Attribute.GetCustomAttribute(tt, typeof(ui_Label));
            ui_MaxLen uiML = (ui_MaxLen)Attribute.GetCustomAttribute(tt, typeof(ui_MaxLen));
            val_Error revE = (val_Error)Attribute.GetCustomAttribute(tt, typeof(val_Error));
            val_Summary revS = (val_Summary)Attribute.GetCustomAttribute(tt, typeof(val_Summary));
            rev_RegExp revRE = (rev_RegExp)Attribute.GetCustomAttribute(tt, typeof(rev_RegExp));
            rv_MinMax rvMinMax=(rv_MinMax)Attribute.GetCustomAttribute(tt, typeof(rv_MinMax));
            rv_Type rvType=(rv_Type)Attribute.GetCustomAttribute(tt, typeof(rv_Type));

            if (inputControl != null)
            {
                SortedList<string,Control> relevantValidators = new SortedList<string,Control>();
                foreach (Control c in thisPage.Validators)
                {
                    if (c is RequiredFieldValidator)
                    {
                        RequiredFieldValidator rfv = (RequiredFieldValidator)c;
                        if (rfv.ControlToValidate == inputControl.ID)
                        {
                            rfv.Text = revE.val;
                            rfv.ErrorMessage = revS.val.Replace("{label}", uiL.val(false));
                            rfv.SetFocusOnError = true;
                            rfv.Display = ValidatorDisplay.Dynamic;
                            rfv.EnableClientScript = false;
                            relevantValidators.Add("rfv",rfv);
                        }
                    }
                    if (c is RegularExpressionValidator)
                    {
                        RegularExpressionValidator rev = (RegularExpressionValidator)c;
                        if (rev.ControlToValidate == inputControl.ID)
                        {
                            if (inputControl is TextBox)
                            {
                                TextBox tb = (TextBox)inputControl;
                                tb.CausesValidation = true;
                            }
                            else
                            {   // TODO maybe more like this
                                throw new ArgumentException("Input Control " + inputControl.ID + " is unexpected type");
                            }
                            rev.ValidationExpression = revRE.val;
                            rev.Text = revE.val;
                            rev.ErrorMessage = revS.val.Replace("{label}", uiL.val(false));
                            rev.SetFocusOnError = true;
                            rev.Display = ValidatorDisplay.Dynamic;
                            rev.EnableClientScript = false;
                            relevantValidators.Add("rev",rev);
                        }
                    }
                    if (c is RangeValidator)
                    {
                        RangeValidator rv = (RangeValidator)c;
                        if (rv.ControlToValidate == inputControl.ID)
                        {
                            rv.Text = "Range is "+rvMinMax.minVal+" to "+rvMinMax.maxVal;
                            rv.Type = rvType.vdt;
                            rv.MaximumValue = rvMinMax.maxVal;
                            rv.MinimumValue = rvMinMax.minVal;
                            rv.ErrorMessage = revS.val.Replace("{label}", uiL.val(false));
                            rv.SetFocusOnError = true;
                            rv.Display = ValidatorDisplay.Dynamic;
                            rv.EnableClientScript = false;
                            relevantValidators.Add("rv",rv);
                        }
                    }
                }
                foreach (Control c in thisPage.Validators)
                {
                    if (c is CustomValidator)
                    {
                        CustomValidator cv = (CustomValidator)c;
                        if (cv.ControlToValidate == inputControl.ID)
                        {
                            cv.Text = revE.val;
                            cv.ErrorMessage=revS.val.Replace("{label}", uiL.val(false));
                            cv.SetFocusOnError = true;
                            cv.ValidateEmptyText = true;
                            cv.Display = ValidatorDisplay.Dynamic;
                            //cv.EnableClientScript = false;
                            cv.ServerValidate += CommonCustomValidator;
                            foreach (string k in relevantValidators.Keys)
                            {
                                Control cx = relevantValidators[k];
                                cv.Attributes.Add(k, cx.ID);
                            }
                        }
                    }
                }
                ((TextBox)inputControl).MaxLength = uiML.val;
                ((TextBox)inputControl).Columns = uiML.val;
            }
            if (labelControl != null)
            {
                Label lab=(Label)labelControl;
                lab.Text = uiL.val(true);
            }
            if (displayOnlyControl != null)
            {
                // if necessary - FUTURE 
            }
        }


    
        protected static void CommonCustomValidator(object source, ServerValidateEventArgs args)
        {
            bool isFlagged=false;
            CustomValidator cv=(CustomValidator)source;
            foreach (string k in cv.Attributes.Keys)
            {
                string id = cv.Attributes[k];
                foreach (Control c in cv.Page.Validators)
                {
                    if (c.ID == id)
                    {
                        if (c is RequiredFieldValidator)
                        {
                            RequiredFieldValidator rfv = (RequiredFieldValidator)c;
                            if (!rfv.IsValid)
                            {
                                rfv.IsValid = true;
                                isFlagged = true;
                            }
                        }
                        if (c is RegularExpressionValidator)
                        {
                            RegularExpressionValidator rev = (RegularExpressionValidator)c;
                            if (!rev.IsValid)
                            {
                                rev.IsValid = true;
                                isFlagged = true;
                            }
                        }
                        if (c is RangeValidator)
                        {
                            RangeValidator rv = (RangeValidator)c;
                            if (!rv.IsValid)
                            {
                                rv.IsValid = true;
                                isFlagged = true;
                            }
                        }
                        if (c is CustomValidator)
                        {
                            CustomValidator cval = (CustomValidator)c;
                            if (!cval.IsValid)
                            {
                                cval.IsValid = true;
                                isFlagged = true;
                            }
                        }
                    }
                }
            }
            args.IsValid = !isFlagged;
        }
    }

    #endregion


    #region Other standard classes

    [ui_Label("Account Code")]
    [rev_RegExp("\\A[0-9]{6}\\z")]
    [val_Error("Must be 6 digits")]
    [val_Summary("{label} Code format error")]
    [ui_MaxLen(6)]
    public class DCC_AccountCode : DCC_StringField
    {
        public DCC_AccountCode() : base() { }
        public DCC_AccountCode(object sources) : base(sources) {}
    }

    [ui_Label("Cost Centre")]
    [rev_RegExp("\\A[07][0-9]{4}\\z")]
    [val_Error("Must be 5 digits starting with 0 or 7")]
    [val_Summary("{label} format error")]
    [ui_MaxLen(5)]
    public class DCC_CostCentre : DCC_StringField
    {
        public DCC_CostCentre() : base() { }
        public DCC_CostCentre(object sources) : base(sources) {}
    }

    [ui_Label("Narrative")]
    [rev_RegExp("\\A.{1,50}\\z")]
    [val_Error("Must be 1-50 characters")]
    [val_Summary("{label} format error")]
    [ui_MaxLen(50)]
    public class DCC_Narrative : DCC_StringField
    {
        public DCC_Narrative() : base() { }
        public DCC_Narrative(object sources) : base(sources) {}
    }

    [ui_Label("Treasurer Ref")]
    [rev_RegExp("\\A.{0,12}\\z")]
    [val_Error("Max length is 12")]
    [val_Summary("{label} format error")]
    [ui_MaxLen(12)]
    public class DCC_TreasurerRef : DCC_StringField
    {
        public DCC_TreasurerRef() : base() { }
        public DCC_TreasurerRef(object sources) : base(sources) {}
    }

    [ui_Label("Payroll Number")]
    [rev_RegExp("\\A[018X]01[0-9]{9}\\z")]
    [val_Error("Must be 12 digits beginning with '001','101','801' or 'X01'")]
    [val_Summary("{label} format error")]
    [ui_MaxLen(12)]
    public class DCC_PayrollNumber : DCC_StringField
    {
        public DCC_PayrollNumber() : base() { }
        public DCC_PayrollNumber(object sources) : base(sources) {}
    }

    [ui_Label("Week Number")]
    [rev_RegExp("\\A\\s*[0-9]{0,2}\\s*\\z")]
    [val_Error("Must be blank or number 1-53")]
    [val_Summary("{label} format error")]
    [ui_MaxLen(2)]
    [rv_MinMax("0","53")]
    public class DCC_WeekNumber : DCC_IntField
    {
        public DCC_WeekNumber() : base() { }
        public DCC_WeekNumber(object sources) : base(sources) {}
        public DCC_WeekNumber(object dataRow_or_Reader,string columnName) : base(dataRow_or_Reader,columnName) {}
        public override string txt { get { return raw.PadLeft(2).Replace(" 0","  ") ; } }
    }

    [ui_Label("Period Number")]
    [rev_RegExp("\\A\\s*[0-9]{0,2}\\s*\\z")]
    [val_Error("Must be blank or number 1-12")]
    [val_Summary("{label} format error")]
    [ui_MaxLen(2)]
    [rv_MinMax("0","12")]
    public class DCC_PeriodNumber : DCC_IntField
    {
        public DCC_PeriodNumber() : base() { }
        public DCC_PeriodNumber(object sources) : base(sources) {}
        public DCC_PeriodNumber(object dataRow_or_Reader,string columnName) : base(dataRow_or_Reader,columnName) {}
        public override string txt { get { return raw.PadLeft(2).Replace(" 0","  "); } }
    }

    [ui_Label("Amount")]
    [rev_RegExp("\\A\\s*[0-9]{1,7}([\\.]{1}[0-9]{0,2}){0,1}\\s*\\z")]
    [val_Error("Expecting values like 1234.56 or 1234")]
    [val_Summary("{label} format error")]
    [ui_MaxLen(10)]
    public class DCC_Amount : DCC_MoneyField
    {
        public DCC_Amount() : base() { }
        public DCC_Amount(decimal value) : base(value)  {}
        public DCC_Amount(TextBox tb) : base(tb) {}
    }

    [ui_Label("Date")]
    [val_Error("Unrecognised date format - Expecting values like '2011 Jan 13'")]
    [val_Summary("{label} format error")]
    [ui_MaxLen(11)]
    [rv_MinMax("1900/1/1","2025/12/31")]    // TODO  extend 
    public class DCC_LogDate : DCC_DateField
    {
        public DCC_LogDate() : base() { } 
        public DCC_LogDate(string value) : base(value)  {}
        public DCC_LogDate(DateTime value) : base(value)  {}
        public DCC_LogDate(DateTime? value) : base(value)  {}
        public DCC_LogDate(TextBox tb) : base(tb) {}
        public DCC_LogDate(Label tb) : base(tb) {}
    }

    [ui_Label("Manual Entry")]
    public class DCC_ManualEntry : DCC_BooleanField
    {
        public DCC_ManualEntry() : base() { }
        public DCC_ManualEntry(object value) : base(value) { }
    }

    /// <summary>
    /// Public class for an address
    /// </summary>
    public class Address
    {
        public string AddressLine1;  
        public string AddressLine2;  
        public string AddressLine3;  
        public string City;          
        public string PostCode;      
        public string Country;       
    }

    #endregion

}