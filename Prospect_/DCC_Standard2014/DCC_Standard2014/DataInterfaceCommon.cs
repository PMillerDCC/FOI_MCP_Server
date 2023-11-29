using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Web;
using Oracle.ManagedDataAccess.Client;

namespace DCC_Standard
{

    #region Standard Business Object class
    public interface IBusinessObject
    {
        bool Read(DataRow dr);
        int  Insert(DCC_Database di);
        bool Update(DCC_Database di);
        bool Delete(DCC_Database di);
    }
    #endregion

    #region DCC_Database class

    /// <summary>
    /// Main class for linking to database.
    /// </summary>
    public class DCC_Database : IDisposable
    {
        public DCC_DataLayer DataLayer;
        public string ConnectionStringID = "";

        /// <summary>
        /// Constructor based on an existing DataLayer.
        /// </summary>
        /// <param name="dataLayer">Instance of an existing cDCCDataLayer.</param>
        public DCC_Database(DCC_DataLayer dataLayer)
        {
            DataLayer = dataLayer;
        }

        /// <summary>
        /// Constructor with details picked up from configuration file.
        /// </summary>
        /// <param name="databaseType">Type of the database. ODP is Oracle Data Provider and Oracle is for Microsoft Data Provider</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="applicationName">The name of the application using the class.</param>
        public DCC_Database(string databaseType, string databaseName, string applicationName)
        {
            ConnectionStringID = databaseName;

            switch (databaseType.ToUpper())
            {
                case "SQLSERVER": { DataLayer = new SQLServer_DCC_DataLayer(databaseName, applicationName); break; }
                case "ORACLE":    { DataLayer = new DCC_OracleDataLayer(databaseName, applicationName); break; }
                //case "ODP":     { DataLayer = new DCC_ODPDataLayer(pDatabaseName, pApplicationName); break; }
                //case "ODAC":    { DataLayer = new DCC_ODAC_DataLayer(pDatabaseName, pApplicationName); break; }
                case "OLEDB":     { DataLayer = new OleDB_DCC_DataLayer(databaseName, applicationName); break; }
                case "ODBC":      { DataLayer = new ODBC_DCC_DataLayer(databaseName, applicationName); break; }
                default: { throw new Exception("DB001 - Unexpected Database Type supplied: " + databaseType); }
            }
        }

        /// <summary>
        /// Explicit method to free any Database resources.
        /// </summary>
        public void Dispose()
        {
            if (DataLayer != null) DataLayer.Dispose();
        }

        public void HandleErrors()
        {
            DataLayer.HandleErrors();
        }

        [Obsolete("is broken so use MultiUser class members instead")]
        public string ObtainLock(string lockKey, bool isCreate, string userName)
        {
            //WindowsIdentity ident = WindowsIdentity.GetCurrent();
            //WindowsPrincipal user = new WindowsPrincipal(ident);
            // string username=user.Identity.Name;

            DCC_DatabaseFrame dif = new DCC_DatabaseFrame("Lock_Set");
            dif.AddParameter("ID",lockKey,20);
            dif.AddParameter("Stamp",DateTime.Now.ToUniversalTime());
            dif.AddParameter("isCreate",isCreate);
            dif.AddParameter("UserName",userName,150);
            dif.AddParameter("StampTooOld", DateTime.Now.AddMinutes(-20).ToUniversalTime());
            DataLayer.SelectData(dif);

            if (dif.DataSelected.Tables.Count == 2)
            {
                if (dif.DataSelected.Tables[0].Rows.Count == 0 && dif.DataSelected.Tables[1].Rows.Count == 1) return "";
                if (dif.DataSelected.Tables[0].Rows.Count == 1)
                {
                    string lockingUser = DCC_GetColumn.String(dif.DataSelected.Tables[0].Rows[0], "_User");
                    if (lockingUser == userName) return "";
                    return lockingUser;
                }
            }
            throw new DataException("Multi-User lock failed for lockKey = [" + lockKey+"]");
        }

        [Obsolete("is broken so use MultiUser class members instead")]
        public void ReleaseLock(string lockKey)
        {
            ReleaseLock(lockKey, false);
        }

        [Obsolete("is broken so use MultiUser class members instead")]
        public void ReleaseLock(string lockKey, bool allForUser)
        {
            string uName="*";

            if (allForUser)
            {
                //WindowsIdentity ident = WindowsIdentity.GetCurrent();
                //WindowsPrincipal user = new WindowsPrincipal(ident);
                // uName=user.Identity.Name;

                uName = HttpContext.Current.User.Identity.Name;      // TODO  non web version
            }

            DCC_DatabaseFrame dif = new DCC_DatabaseFrame("Lock_Clear");
            dif.AddParameter("ID",lockKey,20);
            dif.AddParameter("username",uName,150);
            DataLayer.SelectData(dif);

            if (dif.DataSelected.Tables.Count == 1 && dif.DataSelected.Tables[0].Rows.Count == 0) return ;

            throw new DataException("Multi-User unlock failed for lockKey = [" + lockKey+"] ["+uName+"]");
        }


        #region Method to test if a table exists in a database - may not be the best place for this
        public static bool DoesTableExist(DCC_Database db, string requiredTable)
        {
            string requiredTABLE = requiredTable.ToUpper();
            DataTable tabs = db.DataLayer.FetchSchema("Tables");
            foreach (DataRow dr in tabs.Rows)
            {
                if ((dr[2] as string).ToUpper() == requiredTABLE) return true;
            }
            return false;
        }
        #endregion

    }

    #endregion

    #region ProcessDataReaderRow class

    /// <summary>
    /// Delegate for function to be called for every row provided by a DBDataReader.
    /// </summary>
    /// <param name="pDataReader">DataReader instance (populated inside the calling process).</param>
    /// <returns>True if success.</returns>
    public delegate bool ProcessDataReaderRow(DbDataReader dr);

    interface IDCC_DataLayer : IDisposable
    {
        void NonQuery(DCC_DatabaseFrame pDataInterface);
        bool NonQuery(string pSQLCommand);
        bool NonQuery(string pSQLCommand, List<DCC_DatabaseParameter> pParameterList);
        bool NonQuery(string pSQLCommand, List<DCC_DatabaseParameter> pParameterList, int pExpectedRows);

        int NonQueryPK(string pSQLCommand, List<DCC_DatabaseParameter> pParameterList);

        void SelectData(DCC_DatabaseFrame pDataInterface);
        DataSet SelectData(string pSQLCommand);
        DataSet SelectData(string pSQLCommand, List<DCC_DatabaseParameter> pParameterList);

        void SelectWithDataReader(DCC_DatabaseFrame pDataInterface);
        int SelectWithDataReader(string pSQLCommand, ProcessDataReaderRow processDRRow);
        int SelectWithDataReader(string pSQLCommand, List<DCC_DatabaseParameter> pParameterList, ProcessDataReaderRow processDRRow);

        void SelectSingleRow(DCC_DatabaseFrame pDataInterface);
        SortedList<string, object> SelectSingleRow(string pSQLCommand);
        SortedList<string, object> SelectSingleRow(string pSQLCommand, bool pIsDirectSQL);

        string Fetch_DB_Culture();
    }

    #endregion

    #region DCC_DatabaseParameter class

    /// <summary>
    /// SQL stored procedure parameters.
    /// </summary>
    public struct DCC_DatabaseParameter
    {
        internal string Name;
        internal string Type;
        internal int Length;
        internal object Value;
        /// <summary>
        /// Marker indicating tables involved in Cache
        /// </summary>
        /// <param name="ppCache">Database Table name affected by SELECT, UPDATE, INSERT or DELETE</param>
        public DCC_DatabaseParameter(string ppCache)
        {
            Name = ppCache.ToUpper();
            Type = DCC_DataLayer.cacheTableMarker;
            Length = 0;
            Value = null;
        }

        /// <summary>
        /// SQL Stored Procedure Parameters
        /// </summary>
        /// <param name="type">'S'tring,'I'nteger (32bit),'16' Integer (16bit),'D'ecimal, 'B'oolean,'DT' datetime</param>
        /// <param name="name">Parameter name (without language specific marker (e.g. '@')).  Also can be '_PRIMARYKEY' to map to return value from the sp.</param>
        /// <param name="value">Value for the parameter</param>
        [Obsolete("this method is no longer used.")]
        public DCC_DatabaseParameter(string type, string name, object value)
        {
            Name = name;
            Type = type;
            Length = 0;
            Value = value;
        }
        public DCC_DatabaseParameter(string name, DCC_BooleanField value)
        {
            Name = name;
            Type = "B";
            Length = 0;
            Value = (bool)value;
        }
        public DCC_DatabaseParameter(string name, DCC_StringField value)
        {
            Name = name;
            Type = "S";
            Length = value.maxLen;
            Value = (string)value;
        }
        public DCC_DatabaseParameter(string name, DCC_IntBase value)
        {
            Name = name;
            Type = "I";
            Length = 0;
            Value = (int)value;
        }
        public DCC_DatabaseParameter(string name, Enum value)
        {
            Name = name;
            Type = "I";
            Length = 0;
            Value = value;
        }
        public DCC_DatabaseParameter(string name, DCC_MoneyField value)
        {
            Name = name;
            Type = "D";
            Length = 0;
            Value = (decimal)value;
        }
        public DCC_DatabaseParameter(string name, DCC_DateField value)
        {
            Name = name;
            Type = "DT";
            Length = 0;
            if (value.HasValue)
            {
                Value = (DateTime)value;
            }
            else
            {
                Value = null;
            }
        }

        /// <summary>
        /// SQL Stored Procedure Parameters (with max string length)
        /// </summary>
        /// <param name="ppName">StoredProcedure Parameter name (without language specific marker (e.g. '@')).  Also can be '_PRIMARYKEY' to map to return value from the sp.</param>
        /// <param name="ppValue">Value for the parameter</param>
        /// <param name="ppType">'S'tring,'I'nteger (32bit),'16' Integer (16bit),'D'ecimal, 'B'oolean,'DT' date, 'G' guid</param>
        /// <param name="ppLength">0 for all except 'S'tring where is specifies allowed max chars</param>
        internal DCC_DatabaseParameter(string name, object value, string type, int length)
        {
            Name = name;
            Type = type;
            Length = length;
            Value = value;
        }
    }

    #endregion

    #region DCC_DatabaseFrame class

    /// <summary>
    /// Used to pass information to and from the actual database operations.
    /// </summary>
    public class DCC_DatabaseFrame
    {

        #region Constants

        /// <summary>
        /// Constant specifying parameter direction OUT
        /// </summary>
        public const string OUT = "<";
        /// <summary>
        /// Constant specifying parameter direction IN
        /// </summary>
        public const string IN = ">";
        /// <summary>
        /// Constant specifying parameter is a Cursor
        /// </summary>
        public const string CURSOR = "<OC";
        /// <summary>
        /// Constant specifying parameter is an Integer
        /// </summary>
        public const string INTEGER = "I";
        /// <summary>
        /// Constant specifying parameter is an Integer with direction OUT
        /// </summary>
        public const string OUTINTEGER = OUT + INTEGER;
        /// <summary>
        /// Constant specifying number of rows affected by the non-query is unknown
        /// </summary>
        public const int UNKNOWN = -1;
        /// <summary>
        /// Constant specifying that the length of the string parameter value is not tested
        /// </summary>
        public const int UNSPECIFIED = 0;
        /// <summary>
        /// Constant specifying that the string parameter is to be of type CLOB
        /// </summary>
        public const int CLOB = -1;

        #endregion

        #region Locals

        internal bool ok=false;
        private int expectedRows = 1;
        internal int rows = -1;
        internal int key = -1;
        private string commandText = "";
        internal TimeSpan took = new TimeSpan(0);
        internal bool fromCache = false;

        /// <summary>
        /// Delegate function to be called on each row found by the DBDataReader.
        ///
        /// Signature is "bool foo(DbDataReader dataReader".
        /// </summary>
        public ProcessDataReaderRow RowAction = null;

        /// <summary>
        /// Returned DataSet (from SelectData functions in cDCC_DataLayer).
        /// </summary>
        public DataSet DataSelected = null;
        /// <summary>
        /// Returned result set (from SelectSingleRow operation)
        /// </summary>
        public SortedList<string, object> columnSet=new SortedList<string,object>(5);

        #endregion

        #region Properties

        /// <summary>
        /// The status after the operation - true for success, false for failure
        /// </summary>
        public bool OK   { get { return ok; } }
        /// <summary>
        /// The execution time of the command
        /// </summary>
        public TimeSpan ExecutionTime   { get { return took; } }
        /// <summary>
        /// True if data was cached as a DataSet
        /// </summary>
        public bool DataFromCachedDataSet  { get { return fromCache; } }
        /// <summary>
        /// The number of data rows affected by the non-query transaction
        /// </summary>
        public int Rows   { get { return rows; } }
        /// <summary>
        /// For NonQuery, this gives the expected rows affected by the transaction. The transaction will not be committed if this does not match - unless set to DCC_DB_Frame.UKNOWN.
        /// The default value is 1.
        /// </summary>
        public int ExpectedRows {  get { return expectedRows; }  set { expectedRows = value; }  }
        /// <summary>
        /// For Inserts, this return the primary key created
        /// </summary>
        public int Key   {  get { return key; }    }
        /// <summary>
        /// The name of the SQL Stored Procedure or Package to be executed
        /// </summary>
        public string CommandText {  get { return commandText; } set { commandText = value; }   }

        /// <summary>
        /// Convenience - synoynm for DataSelected.Tables[0]
        /// Returns the first DataTable in the result set
        /// </summary>
        public DataTable TableZero { get { return DataSelected.Tables[0]; } }

        /// <summary>
        /// Convenience - synoynm for DataSelected.Tables[0].Rows
        /// Returns the Rows of the first DataTable in the result set
        /// </summary>
        public DataRowCollection TableZeroRows { get { return DataSelected.Tables[0].Rows; } }
        /// <summary>
        /// Convenience - synoynm for DataSelected.Tables[0].Rows[0]
        /// Returns the first Row of the first DataTable in the result set
        /// </summary>
        public DataRow RowZero { get { return DataSelected.Tables[0].Rows[0]; } }
        /// <summary>
        /// Convenience - synoynm for DataSelected because not easy to find and edit all 'ds'
        /// </summary>
        [Obsolete("Synonym for DataSelected")]
        public DataSet ds { get { return DataSelected; } }
        /// <summary>
        /// True if results set exists and has at least one table
        /// </summary>
        public bool hasTables
        {
            get
            {
                if (DataSelected == null) return false;
                if (DataSelected.Tables == null) return false;
                return DataSelected.Tables.Count > 0;
            }
        }

        #endregion

        internal List<DCC_DatabaseParameter> paramList = new List<DCC_DatabaseParameter>(5);
        internal bool noisyMode = true;

        #region Constructors

        /// <summary>
        /// Default empty constructor
        /// </summary>
        public DCC_DatabaseFrame()
        {
        }

        /// <summary>
        /// Construct with just command name supplied
        /// </summary>
        /// <param name="pCommandName">Procedure Name</param>
        public DCC_DatabaseFrame(string pCommandName)
        {
            commandText = pCommandName;
        }

        #endregion

        /// <summary>
        /// Reset the DCC_DB_Frame, but leave CommandName, Parameters, and ExpectedRows untouched
        /// </summary>
        public void Reset()
        {
            ok = false;
            rows = -1;
            key = -1;
            DataSelected = null;
        }

        // NOTE - It is deliberate that byte and short are emitted as type 'I' - otherwise literal values of '0' are considered as byte which won't map to an int parameter.
        public void AddParameter(string pName, bool pValue)     { paramList.Add(new DCC_DatabaseParameter(pName, pValue, "B", 0)); }
        public void AddParameter(string pName, byte pValue)     { paramList.Add(new DCC_DatabaseParameter(pName, pValue, "I", 0)); }
        public void AddParameter(string pName, short pValue)    { paramList.Add(new DCC_DatabaseParameter(pName, pValue, "I", 0)); }
        public void AddParameter(string pName, int pValue)      { paramList.Add(new DCC_DatabaseParameter(pName, pValue, "I", 0)); }
        public void AddParameter(string pName, long pValue)     { paramList.Add(new DCC_DatabaseParameter(pName, pValue, "64", 0)); }
        public void AddParameter(string pName, decimal pValue)  { paramList.Add(new DCC_DatabaseParameter(pName, pValue, "D", 0)); }
        public void AddParameter(string pName, float pValue)    { paramList.Add(new DCC_DatabaseParameter(pName, pValue, "F", 0)); }
        public void AddParameter(string pName, double pValue)   { paramList.Add(new DCC_DatabaseParameter(pName, pValue, "FF", 0)); }
        public void AddParameter(string pName, DateTime pValue) { paramList.Add(new DCC_DatabaseParameter(pName, pValue, "DT", 0)); }
        public void AddParameter(string pName, Guid pValue)     { paramList.Add(new DCC_DatabaseParameter(pName, pValue, "G", 0)); }
        public void AddParameter(string pName, bool? pValue)    { if (pValue.HasValue) paramList.Add(new DCC_DatabaseParameter(pName, pValue.Value, "B", 0));  else paramList.Add(new DCC_DatabaseParameter(pName, DBNull.Value, "B", 0)); }
        public void AddParameter(string pName, byte? pValue)    { if (pValue.HasValue) paramList.Add(new DCC_DatabaseParameter(pName, pValue.Value, "I", 0));  else paramList.Add(new DCC_DatabaseParameter(pName, DBNull.Value, "I", 0)); }
        public void AddParameter(string pName, short? pValue)   { if (pValue.HasValue) paramList.Add(new DCC_DatabaseParameter(pName, pValue.Value, "I", 0));  else paramList.Add(new DCC_DatabaseParameter(pName, DBNull.Value, "I", 0)); }
        public void AddParameter(string pName, int? pValue)     { if (pValue.HasValue) paramList.Add(new DCC_DatabaseParameter(pName, pValue.Value, "I", 0));  else paramList.Add(new DCC_DatabaseParameter(pName, DBNull.Value, "I", 0)); }
        public void AddParameter(string pName, long? pValue)    { if (pValue.HasValue) paramList.Add(new DCC_DatabaseParameter(pName, pValue.Value, "64", 0)); else paramList.Add(new DCC_DatabaseParameter(pName, DBNull.Value, "64", 0)); }
        public void AddParameter(string pName, Decimal? pValue) { if (pValue.HasValue) paramList.Add(new DCC_DatabaseParameter(pName, pValue.Value, "D", 0));  else paramList.Add(new DCC_DatabaseParameter(pName, DBNull.Value, "D", 0)); }
        public void AddParameter(string pName, Double? pValue)  { if (pValue.HasValue) paramList.Add(new DCC_DatabaseParameter(pName, pValue.Value, "FF", 0)); else paramList.Add(new DCC_DatabaseParameter(pName, DBNull.Value, "FF", 0)); }
        public void AddParameter(string pName, Single? pValue)  { if (pValue.HasValue) paramList.Add(new DCC_DatabaseParameter(pName, pValue.Value, "F", 0));  else paramList.Add(new DCC_DatabaseParameter(pName, DBNull.Value, "F", 0)); }
        public void AddParameter(string pName, DateTime? pValue){ if (pValue.HasValue) paramList.Add(new DCC_DatabaseParameter(pName, pValue.Value, "DT", 0)); else paramList.Add(new DCC_DatabaseParameter(pName, DBNull.Value, "DT", 0)); }
        public void AddParameter(string pName, Guid? pValue)    { if (pValue.HasValue) paramList.Add(new DCC_DatabaseParameter(pName, pValue.Value, "G", 0));  else paramList.Add(new DCC_DatabaseParameter(pName, DBNull.Value, "G", 0)); }

        /// <summary>
        /// Add a named non-string parameter for the stored procedure.
        /// </summary>
        /// <param name="name">Name of the stored procedure parameter (do not include the SqlServer '@' prefix).</param>
        /// <param name="value">The value to be passed.  Supported types are: int,short,bool,DateTime,decimal and byte[]. String parameters are passed using another overload. There is a special syntax for direction OUT parameters ... use constants of DCC_DB_Frame.CURSOR or DCC_DB_Frame.OUTINTEGER.</param>
        public void AddParameter(string name, object value)
        {
            if (value == null)   { paramList.Add(new DCC_DatabaseParameter(name, null, "I", 0)); return; }
            if (value is byte[]) { paramList.Add(new DCC_DatabaseParameter(name, value, "BLOB", 0)); return; }
            if (value is string)
            {
                string work_string = (string)value;
                if (work_string == "<I")  { paramList.Add(new DCC_DatabaseParameter(name, null, work_string, 0)); return; }
                if (work_string == "<OC") { paramList.Add(new DCC_DatabaseParameter(name, null, work_string, 0)); return; }
                throw new ArgumentException("In DCC_DB_Frame.AddParameter(pName,pValue) the type of pValue is string for pName ["+name+"]. Please use DCC_DB_Frame.AddParameter(pName,pValue,pMaxLength) for strings.");
            }
            throw new ArgumentException("In DCC_DB_Frame.AddParameter(pName,pValue) for pName ["+name+"] the type of pValue is "+name.GetType()+"(i.e. not int,short,bool,long,double,float,decimal,byte[],DateTime or GUID");
        }

        /// <summary>
        /// Add a named string parameter for the stored procedure.
        /// </summary>
        /// <param name="name">Name of the stored procedure parameter (do not include the SqlServer '@' prefix).</param>
        /// <param name="value">The string to be passed.</param>
        /// <param name="maxLength">The maximum length allowed.  Special values DCC_DB_Frame.CLOB means store into a CLOB, and DCC_DB_Frame.NOTSPECIFIED which means no check will be made on the string length, possibly causing runtime errors.</param>
        public void AddParameter(string name, object value, int maxLength )
        {
            if (maxLength == CLOB)
            {
                if (value == null || (value is String && ((string)value).Length==0) )
                {
                    paramList.Add(new DCC_DatabaseParameter(name, " ", "CLOB", 0));
                    return;
                }
            }
            if (value == null)
            {
                paramList.Add(new DCC_DatabaseParameter(name, null, "S", 1));
                return;
            }
            if (value is string)
            {
                if (maxLength == CLOB)
                {
                    paramList.Add(new DCC_DatabaseParameter(name, value, "CLOB", 0));
                    return;
                }
                paramList.Add(new DCC_DatabaseParameter(name, value, "S", maxLength));
                return;
            }
            throw new ArgumentException("In DCC_DB_Frame.AddParameter(ppName,ppValue,ppMaxLength) the type of ppValue is not string. Please use DCC_DB_Frame.AddParameter(ppName,ppValue) for non-strings.");
        }

        /// <summary>
        /// Indicate SQL Table involved in this transaction so that SELECTs are cached, and the caches destroyed by UPDATEs, DELETEs or INSERTs.
        /// </summary>
        /// <param name="name"></param>
        public void AddTable(string name)
        {
            paramList.Add(new DCC_DatabaseParameter(name));
        }

        /// <summary>
        /// Makes the DEBUG log trace much quieter
        /// </summary>
        public void Quiet()
        {
            noisyMode = false;
        }

        /// <summary>
        /// Change a named string parameter for the stored procedure.
        /// </summary>
        /// <param name="name">Name of the stored procedure parameter (do not include the SqlServer '@' prefix).</param>
        /// <param name="newValue">The string to be passed.</param>
        /// <param name="newMaxLength">The maximum length allowed.  Special values DCC_DateInterface.CLOB means store into a CLOB, and DCC_DateInterface.NOTSPECIFIED which means no check will be made on the string length, possibly causing runtime errors.</param>
        public void ChangeParameter(string name, string newValue, int newMaxLength )
        {
            RemoveParameter(name);
            AddParameter(name,newValue,newMaxLength);
        }

        /// <summary>
        /// Change a named non-string parameter for the stored procedure.
        /// </summary>
        /// <param name="name">Name of the stored procedure parameter (do not include the SqlServer '@' prefix).</param>
        /// <param name="newValue">The value to be passed.  Supported types are: int,short,bool,DateTime,decimal and byte[]. String parameters are passed using another overload. There is a special syntax for direction OUT parameters ... use constants of DCC_DateInterface.CURSOR or DCC_DateInterface.OUTINTEGER.</param>
        public void ChangeParameter(string name, object newValue)
        {
            RemoveParameter(name);
            AddParameter(name,newValue);
        }

        /// <summary>
        /// Remove a named parameter for the stored procedure.
        /// </summary>
        /// <param name="name">Name of the stored procedure parameter (do not include the SqlServer '@' prefix).</param>
        private bool RemoveParameter(string name)
        {
            int hit = -1;
            for (int i = 0; i < paramList.Count; i++)
            {
                if (paramList[i].Name.ToUpper() == name.ToUpper()) { hit = i; break; }
            }
            if (hit > -1)
            {
                paramList.RemoveAt(hit);
                return true;
            }
            return false;
        }
    }

    #endregion

    #region DCC_DataLayer class

    /// <summary>
    /// Class providing instrumented, logged, error checked access to databases.
    /// </summary>
    public abstract class DCC_DataLayer : IDCC_DataLayer
    {
        static bool isLocalHost = false;        // true if running client and server on same machine
        static bool isWebApp = false;           // true if Web Application (so requires HTML output, and cannot do MessageBox)
        string HostIP = "";
        string ClientIP = "";

        DCC_DatabaseFrame thisCommand = null;

        /// <summary>
        /// Database connection string.
        /// </summary>
        public string connectionString;

        internal bool isReadOnly = false;

        bool isOpened = false;                  // true when connection has been opened
        /// <summary>
        /// Default timeout (in seconds).
        /// </summary>
        protected int timeOut = 120;             // GWW changed from 10
        /// <summary>
        /// if true, then suppress the tracing.
        /// </summary>
        protected bool IsDebugMode = false;             //
        /// <summary>
        /// Result from the operation.
        /// </summary>
        protected DataSet result;               //

        // error handling
        bool isInError = false;                 // becomes true if unrecoverable error
        /// <summary>
        /// details of the error that has just occured
        /// </summary>
        protected ErrorReport lastError = null;

        // identity info for error logging
        string connectID = "";
        public string callingSoftware = "";
        public string userIdentity = "~notset~";
        public string userMachine = "~notset~";
        public bool IsOpen { get { return isOpened; } }
        public ErrorReport LastError { get { return lastError; } }

        public override string ToString()
        {
            return "DataLayer - ConnectID: " + connectID + "  " + (isOpened ? "Opened" : "Not Open");
        }

        #region Special Markers
        /// <summary>
        /// constant string used to prefix a direct SQL statement (otherwise assumed to be a procedure name) *** EXCEPTIONAL USE ONLY
        /// </summary>
        public const string directSQLcommandMarker = "**";
        public const string quietMarker = "!";
        internal const string cacheTableMarker = "$";
        /// <summary>
        /// constant string used as prefix to a folder name to indicate CSV files for OLEDB
        /// </summary>
        public const string csvTableMarker = "*CSV*";
        /// <summary>
        /// constant string used as prefix to a folder name to indicate XLS files for OLEDB
        /// </summary>
        public const string xlsTableMarker = "*XLS*";
        /// <summary>
        /// constant string used as prefix to a folder name to indicate XLSX files for OLEDB
        /// </summary>
        public const string xlsxTableMarker = "*XLSX*";

        const string cacheParameterDivider = "_|_";
        #endregion

        /// <summary>
        /// Empty constructor
        /// </summary>
        public DCC_DataLayer()
        {
        }

		public int CacheLife = 0;

        /// <summary>
        /// Common Constructor Code
        /// </summary>
        /// <param name="databaseType">Underlying Database Type - used only for documenting debugging - e.g. 'ORACLE'</param>
        /// <param name="connectionID">Name of connection string in .CONFIG file (needs to be a pair of strings because "_Local" is appended automatically if client and server same machine and is web application - e.g. name="Procurement" name="Procurement_Local"</param>
        /// <param name="callingSoftwareID">Name or other identifier for the application - used only for documenting debugging</param>
        protected void newDCC_DataLayer(string databaseType, string connectionID, string callingSoftwareID)
        {
            string s = ConfigurationManager.AppSettings["DebugMode"] ?? "true";
            IsDebugMode = (s.ToLower() == "true");

            ClientIP = HostIP = "(local)";
            isInError = false;
            connectID = connectionID;
            callingSoftware = callingSoftwareID;
            isWebApp = isLocalHost = false;
            if (HttpContext.Current != null)
            {
                isWebApp = true;
                if (HttpContext.Current.Request != null)
                {
                    isLocalHost = HttpContext.Current.Request.IsLocal;
                    if (!isLocalHost)
                    {
                        ClientIP = (HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]??"(no Client info)");
                        HostIP = (HttpContext.Current.Request.Url.Host ?? "(no Host info)");
                    }
                }
            }
            else
            {
                isLocalHost = true;
            }

            userIdentity = (isWebApp?(HttpContext.Current.Request.ServerVariables["LOGON_USER"]??"" + " " + HttpContext.Current.User.Identity.Name):"");

            if (IsDebugMode)
            {
                DebugTrace.WriteLine();
                DebugTrace.WriteLine("============ "+databaseType+" DCC_DataLayer \"" + callingSoftware + " / " + connectID + "\" constructed ====== ");
                if (isWebApp) DebugTrace.WriteLine("   SessionID:  " + HttpContext.Current.Session.SessionID + "   for " + userIdentity + " on " + ClientIP);
            }

            try
            {
                OpenDatabase(connectID);
            }
            catch (DCCDataLayerException eHappened)
            {
                isInError = true;
                lastError = new ErrorReport(eHappened.errorID,
                                            eHappened.userMessage,
                                            eHappened.supportMessage + (eHappened.InnerException != null ? eHappened.InnerException.Message : ""),
                                            eHappened.StackTrace,
                                            ErrorLevel.SevereError);
                HandleErrors();
            }

            //WindowsIdentity ident = WindowsIdentity.GetCurrent();
            //WindowsPrincipal user = new WindowsPrincipal(ident);
            //userMachine = Environment.MachineName;
        }

        /// <summary>
        /// Explicit method to free any database resources.
        /// </summary>
        public void Dispose()
         {
             isInError = false;
             CloseDatabaseConnection();
            if (IsDebugMode)
            {
                DebugTrace.WriteLine();
                DebugTrace.WriteLine("============ DCC_DataLayer disposed ====== ");
                if (isWebApp) DebugTrace.WriteLine("   SessionID:  " + HttpContext.Current.Session.SessionID + "   by "+userIdentity + " on "+ClientIP);
            }
        }

        /// <summary>
        /// Change SQL timeout from default of 10 secs.
        /// </summary>
        /// <param name="pNewTimeout">New SQL Timeout in seconds - range 1-120 - uses limit if value provided out-of-bounds.</param>
        public void SetTimeout(int pNewTimeout)
        {
            pNewTimeout = Math.Max(1, pNewTimeout);
            pNewTimeout = Math.Min(120, pNewTimeout);
            timeOut = pNewTimeout;
        }

        /// <summary>
        /// Change SQL timeout from default to ridiculous 1 hour.
        /// </summary>
        public void SetRidiculousTimeout()
        {
            timeOut = 60 * 60;
        }

        /// <summary>
        /// Inhibit the voluminous tracing.
        /// </summary>
        public void SetSilent()
        {
            IsDebugMode = false;
        }

        /// <summary>
        /// Inhibit the voluminous tracing.
        /// </summary>
        /// <param name="pIsDebugMode">True is quiet, False is noisy</param>
        public void SetSilent(bool pIsDebugMode)
        {
            IsDebugMode = pIsDebugMode;
        }

        /// <summary>
        /// Inhibit the voluminous tracing.
        /// </summary>
        public bool IsReadOnly
        {
            get { return isReadOnly; }
            set { isReadOnly = value; }
        }


        public string Fetch_DB_Culture()
        {
            return FetchCulture();
        }

        #region Shells for NonQuery()

        /// <summary>
        /// Execute a NonQuery
        /// </summary>
        /// <param name="pDataInterface">DCC Data Interface</param>
        public void NonQuery(DCC_DatabaseFrame pDataInterface)
        {
            thisCommand=pDataInterface;
            DCC_DB_NonQuery enq=NonQueryBase(pDataInterface.CommandText, pDataInterface.paramList, pDataInterface.ExpectedRows);
            pDataInterface.ok = enq.ok;
            pDataInterface.rows = enq.rows;
        }
        /// <summary>
        /// Execute a NonQuery with no parameters (defaults to 1 expected row affected).
        /// </summary>
        /// <param name="pSQLCommand">Name of stored procedure.</param>
        /// <returns>True for success, false for failure.</returns>
        public bool NonQuery(string pSQLCommand)
        {
            return NonQuery(pSQLCommand, new List<DCC_DatabaseParameter>(), 1);
        }
        /// <summary>
        /// Execute a NonQuery (defaults to 1 expected row affected).
        /// </summary>
        /// <param name="pSQLCommand">Name of stored procedure.</param>
        /// <param name="pParamterList">List of parameters for the stored procedure.</param>
        /// <returns>True for success, false for failure.</returns>
        public bool NonQuery(string pSQLCommand, List<DCC_DatabaseParameter> pParamterList)
        {
            return NonQuery(pSQLCommand, pParamterList, 1);
        }
        /// <summary>
        /// Execute a NonQuery.
        /// </summary>
        /// <param name="pSQLCommand">Name of stored procedure.</param>
        /// <param name="pParameterList">List of parameters for the stored procedure.</param>
        /// <param name="pExpectedRows">Expected Rows that the stored procedure will return as affected.</param>
        /// <returns>True for success, false for failure.</returns>
        public bool NonQuery(string pSQLCommand, List<DCC_DatabaseParameter> pParameterList,int pExpectedRows)
        {
            return NonQueryBase(pSQLCommand, pParameterList, pExpectedRows).ok;
        }

        private DCC_DB_NonQuery NonQueryBase(string pSQLCommand, List<DCC_DatabaseParameter> pParameterList, int pExpectedRows)
        {
            DCC_DB_NonQuery reply=new DCC_DB_NonQuery();
            try
            {
                if (!isOpened) throw new DCCDataLayerException("Error 1006", "Database not opened", "Database not opened");
                reply= executeNonQueryBase(pSQLCommand, pParameterList, pExpectedRows);
            }
            catch (DCC_DataLayer.DCCDataLayerException eHappened)
            {
                isInError = true;
                lastError = new ErrorReport(
                    eHappened.errorID,
                    eHappened.userMessage,
                    eHappened.supportMessage,
                    eHappened.StackTrace,
                    ErrorLevel.SevereError
                    );

                HandleErrors();
                reply.ok = false;
                return reply;
            }
            catch (Exception eOther)
            {
                isInError = true;
                lastError = new ErrorReport(
                    callingSoftware + " DCC_DataLayer",
                    "Unexpected exception",
                    eOther.Message,
                    eOther.StackTrace,
                    ErrorLevel.SevereError
                    );

                HandleErrors();
                reply.ok = false;
                return reply;
            }
            return reply;
        }
        /// <summary>
        /// Execute a NonQuery which inserts one row into a table with a primary key.
        /// </summary>
        /// <param name="pDataInterface">DCC Data Interface.</param>
        public int NonQueryPK(DCC_DatabaseFrame pDataInterface)
        {
            thisCommand=pDataInterface;
            pDataInterface.key=NonQueryPK(pDataInterface.CommandText, pDataInterface.paramList, pDataInterface.ExpectedRows);
            if (pDataInterface.key < 0) pDataInterface.ok = false;
            return pDataInterface.key;
        }
        /// <summary>
        /// Execute a NonQuery which inserts one row into a table with a primary key.
        /// </summary>
        /// <param name="pSQLCommand">Name of stored procedure.</param>
        /// <param name="pParamterList">List of parameters for the stored procedure.</param>
        /// <returns>New primary key value if success, -99 for failure.</returns>
        public int NonQueryPK(string pSQLCommand, List<DCC_DatabaseParameter> pParameterList)
        {
            return NonQueryPK(pSQLCommand, pParameterList, 1);
        }
        /// <summary>
        /// Execute a NonQuery which inserts (usually) one row into a table with a primary key.
        /// </summary>
        /// <param name="pSQLCommand">Name of stored procedure.</param>
        /// <param name="pParamterList">List of parameters for the stored procedure.</param>
        /// <param name="pExpectedRows">Number of rows expected to be affected. -1 if not known.</param>
        /// <returns>New primary key value if success, -99 for failure.</returns>
        public int NonQueryPK(string pSQLCommand, List<DCC_DatabaseParameter> pParameterList,int pExpectedRows)
        {
            try
            {
                if (!isOpened) throw new DCCDataLayerException("Error 1006","Database not opened","Database not opened");
                int newPrimaryKey=executeNonQueryPK(pSQLCommand, pParameterList, pExpectedRows);
                return newPrimaryKey;
            }
            catch (DCC_DataLayer.DCCDataLayerException eHappened)
            {
                isInError = true;
                lastError = new ErrorReport(
                    eHappened.errorID,
                    eHappened.userMessage,
                    eHappened.supportMessage+(eHappened.InnerException!=null?eHappened.InnerException.Message:""),
                    eHappened.StackTrace,
                    ErrorLevel.SevereError
                    );

                HandleErrors();
                return -99;
            }
            catch (Exception eOther)
            {
                isInError = true;
                lastError = new ErrorReport(
                    callingSoftware+" DCC_DataLayer",
                    "Unexpected exception",
                    eOther.Message+(eOther.InnerException!=null?eOther.InnerException.Message:""),
                    eOther.StackTrace+(eOther.InnerException!=null?eOther.InnerException.StackTrace:""),
                    ErrorLevel.SevereError
                    );

                HandleErrors();
                return -99;
            }
        }

        #endregion

        #region Shells for SelectData()

        public void SelectData(DCC_DatabaseFrame pDataInterface)
		{
			SelectData(pDataInterface, 0);
		}

        /// <summary>
        /// Execute Select.
        /// </summary>
        /// <param name="pDataInterface">DCC Data Interface</param>
        /// <param name="pCacheLife">Enter the cache life of the query in minutes (1440 = 1 day etc.)</param>
        public void SelectData(DCC_DatabaseFrame pDataInterface, int pCacheLife = 0)
        {
			CacheLife = pCacheLife;
            thisCommand=pDataInterface;
            pDataInterface.DataSelected = SelectData(pDataInterface.CommandText, pDataInterface.paramList);
            pDataInterface.ok = true;
        }

        /// <summary>
        /// Execute Select (with no parameters).
        /// </summary>
        /// <param name="pSQLCommand">Name of the stored procedure.</param>
        /// <returns>DataSet, will be for null or empty for failures.</returns>
        public DataSet SelectData(string pSQLCommand)
        {
            return SelectData(pSQLCommand, null);
        }

        /// <summary>
        /// Execute Select.
        /// </summary>
        /// <param name="pSQLCommand">Name of the stored procedure.</param>
        /// <param name="pParameterList">List of parameters for the stored procedure.</param>
        /// <returns>DataSet, will be for null or empty for failures.</returns>
        public DataSet SelectData(string pSQLCommand,List<DCC_DatabaseParameter> pParameterList)
        {
            try
            {
                if (!isOpened) throw new DCCDataLayerException("Error 1006", "Database not opened", "Database not opened");
                return FillDS(pSQLCommand, pParameterList);
            }
            catch (DCC_DataLayer.DCCDataLayerException eHappened)
            {
                isInError = true;
                lastError = new ErrorReport(
                    eHappened.errorID,
                    eHappened.userMessage,
                    eHappened.supportMessage + (eHappened.InnerException != null ? eHappened.InnerException.Message : ""),
                    eHappened.StackTrace,
                    ErrorLevel.SevereError
                    );

                HandleErrors();
                return null;
            }
            catch (Exception eOther)
            {
                isInError = true;
                lastError = new ErrorReport(
                    callingSoftware+" DCC_DataLayer",
                    "Unexpected exception",
                    eOther.Message+(eOther.InnerException!=null?eOther.InnerException.Message:""),
                    eOther.StackTrace+(eOther.InnerException!=null?eOther.InnerException.StackTrace:""),
                    ErrorLevel.SevereError
                    );

                HandleErrors();
                return null;
            }
        }

        #endregion

        #region Shells for SelectWithDataReader()

        /// <summary>
        /// Execute Select via a DataReader delegate.
        /// </summary>
        /// <param name="pDataInterface">Data Interface Frame</param>
        public void SelectWithDataReader(DCC_DatabaseFrame pDataInterface)
        {
            pDataInterface.rows = SelectWithDataReader(pDataInterface.CommandText, pDataInterface.paramList,pDataInterface.RowAction);
        }
        /// <summary>
        /// Execute Select (with no parameters).
        /// </summary>
        /// <param name="procedureName">Name of Stored Procedure</param>
        /// <param name="processDRRow">Delegate to be executed for each row read</param>
        /// <returns>count of records in the datareader recordset</returns>
        public int SelectWithDataReader(string pSQLCommand,ProcessDataReaderRow pProcessDataReaderRow)
        {
            return SelectWithDataReader(pSQLCommand, null,pProcessDataReaderRow);
        }
        /// <summary>
        /// Executes Select
        /// </summary>
        /// <param name="procedureName">Name of Stored Procedure</param>
        /// <param name="plist">List of Stored Procedures Parameters</param>
        /// <param name="processDRRow">Delegate to be executed for each row read</param>
        /// <returns>count of records in the datareader recordset</returns>
        public int SelectWithDataReader(string pSQLCommand, List<DCC_DatabaseParameter> pParameterList, ProcessDataReaderRow pProcessDataReaderRow)
        {
            if (pProcessDataReaderRow==null)
            {
                throw new DCCDataLayerException("Error", "Delegate not set.", "Delegate for SelectWithDataReader does not have an action.");
            }
            try
            {
                if (!isOpened) throw new DCCDataLayerException("Error 1006","Database not opened","Database not opened");
                return GetDataReader(pSQLCommand, pParameterList, pProcessDataReaderRow);
            }
            catch (DCC_DataLayer.DCCDataLayerException eHappened)
            {
                isInError = true;
                lastError = new ErrorReport(
                    eHappened.errorID,
                    eHappened.userMessage,
                    eHappened.supportMessage+(eHappened.InnerException!=null?eHappened.InnerException.Message:""),
                    eHappened.StackTrace,
                    ErrorLevel.SevereError
                    );

                HandleErrors();
                return -1;
            }
            catch (Exception eOther)
            {
                isInError = true;
                lastError = new ErrorReport(
                    callingSoftware+" DCC_DataLayer",
                    "Unexpected exception",
                    eOther.Message+(eOther.InnerException!=null?eOther.InnerException.Message:""),
                    eOther.StackTrace+(eOther.InnerException!=null?eOther.InnerException.StackTrace:""),
                    ErrorLevel.SevereError
                    );

                HandleErrors();
                return -1;
            }
        }

        #endregion

        #region Shells for SelectSingleRow()

        /// <summary>
        /// Execute a single row Select.
        /// </summary>
        /// <param name="dif">Data Interface Frame</param>
        public void SelectSingleRow(DCC_DatabaseFrame pDataInterface)
        {
            SortedList<string, object> paras = new SortedList<string, object>();
            string fullSQL = appendParas(pDataInterface.CommandText,pDataInterface.paramList,paras);

            pDataInterface.columnSet = FillDR(fullSQL,paras);
        }
        /// <summary>
        /// Execute a single row Select.
        /// </summary>
        /// <param name="pSQLCommand">Name of the stored procedure.</param>
        /// <returns>SortedList of column names and values.</returns>
        public SortedList<string, object> SelectSingleRow(string pSQLCommand)
        {
            return SelectSingleRow(pSQLCommand, false);
        }
        /// <summary>
        /// Executes a single row Select.
        /// </summary>
        /// <param name="pSQLCommand">Name of the stored procedure.</param>
        /// <param name="pIsDirectSQL">(Deprecated) True if pSQLCommand is an actual SQL statement.</param>
        /// <returns>SortedList of column names and values.</returns>
        public SortedList<string,object> SelectSingleRow(string pSQLCommand,bool pIsDirectSQL)
        {
            try
            {
                if (!isOpened) throw new DCCDataLayerException("Error 1006","Database not opened","Database not opened");
                return FillDR(pSQLCommand,pIsDirectSQL);
            }
            catch (DCC_DataLayer.DCCDataLayerException eHappened)
            {
                isInError = true;
                lastError = new ErrorReport(
                    eHappened.errorID,
                    eHappened.userMessage,
                    eHappened.supportMessage+(eHappened.InnerException!=null?eHappened.InnerException.Message:""),
                    eHappened.StackTrace,
                    ErrorLevel.SevereError
                    );

                HandleErrors();
                return null;
            }
            catch (Exception eOther)
            {
                isInError = true;
                lastError = new ErrorReport(
                    callingSoftware+" DCC_DataLayer",
                    "Unexpected exception",
                    eOther.Message+(eOther.InnerException!=null?eOther.InnerException.Message:""),
                    eOther.StackTrace+(eOther.InnerException!=null?eOther.InnerException.StackTrace:""),
                    ErrorLevel.SevereError
                    );

                HandleErrors();
                return null;
            }
        }

        #endregion

        #region Database Open and Close

        /// <summary>
        /// Open database.
        /// If server is same machine as client, first attempts to open a connection called pConnectionString + "_Local"
        /// </summary>
        /// <param name="pConnectionString">Name of ConnectionString inside .CONFIG file.  If preceeded by the csvTableMarker string, then string is name of a CSV file to be opened by OleDB.</param>
        /// <returns>True for success, false for failure.</returns>
        private  bool OpenDatabase(string pConnectionString)
        {
            string tryID = pConnectionString;
            if (pConnectionString.StartsWith(csvTableMarker))
            {
                //connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+pConnectionString.Substring(csvTableMarker.Length)+";Extended Properties='text;HDR=yes;FMT=Delimited'";
                connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pConnectionString.Substring(csvTableMarker.Length) + ";Extended Properties='text;HDR=yes;FMT=Delimited'";
            }
            else
            {
                if (pConnectionString.StartsWith(xlsxTableMarker))
                {
                    //connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+pConnectionString.Substring(xlsTableMarker.Length)+";Extended Properties='text;HDR=yes;FMT=Delimited'";
                    connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pConnectionString.Substring(xlsxTableMarker.Length) + ";Extended Properties='Excel 12.0 Xml;HDR=yes'";
                }
                else
                {
                    if (pConnectionString.StartsWith(xlsTableMarker))
                    {
                        //connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+pConnectionString.Substring(xlsTableMarker.Length)+";Extended Properties='text;HDR=yes;FMT=Delimited'";
                        connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pConnectionString.Substring(xlsTableMarker.Length) + ";Extended Properties='Excel 8.0;HDR=yes'";
                    }
                    else
                    {
                        connectionString = "";
                        //if (isLocalHost) tryID = pConnectionString + "_Local";
                        ConnectionStringSettings cs = ConnectionStringHelpers.FetchConnectionStringSettings(tryID);
                        if (cs == null && isLocalHost) cs = ConnectionStringHelpers.FetchConnectionStringSettings(pConnectionString);

                        if (cs != null) connectionString = cs.ConnectionString;
                    }
                }
            }

            if (connectionString == null)
            {
                throw new DCCDataLayerException("Error 1008N","Connection String invalid","Connection String from ["+tryID+"] is null ");
            }

            if (connectionString == "")
            {
                throw new DCCDataLayerException("Error 1008N","Connection String invalid","Connection String from ["+tryID+"] is empty");
            }

            try
            {
                if (ConnectDatabase(connectionString)) return true;
            }
            catch(Exception e)
            {
                throw new DCCDataLayerException("Error 1008","Connection String invalid",e.Message+"\r\nConnectionString from ["+tryID+"] was [" + ConnectionStringHelpers.MaskConnectionString(connectionString)+"]");
            }

            bool isGood = false;
            string errText =  "ID: ["+tryID+"]  produced connection string ["+ConnectionStringHelpers.MaskConnectionString(connectionString)+@"]
";
            int sleepTime = 2000; // ms
            int retries = 5;
            while (retries > 0)
            {
                try
                {
                    isGood = OpenDatabase();
                    if (!isGood) throw new Exception("Connection failed to open");
                }
                catch (Exception e)
                {
                    // SQLServer "Timeout expired.  The timeout period elapsed prior to completion of the operation or the server is not responding."
                    // SQLServer "Timeout expired.  The timeout period elapsed prior to obtaining a connection from the pool.  This may have occurred because all pooled connections were in use and max pool size was reached."
                    //           "A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond" "Unable to connect to the remote server"
                    // Oracle    "ORA-12170: TNS:Connect timeout occurred"
                    // Oracle    "ORA-12154: TNS:could not resolve service name"
                    // Oracle    "ORA-12154: TNS:could not resolve the connect identifier specified"
                    // Oracle    "OCIEnvCreate failed with return code -1 but error message text was not available."

                    if (!e.Message.ToLower().Contains(" time"))
                    {
                        errText = @"
Error open() failed:
" + e.Message + @"
" + e.InnerException ?? ""+@"
";
                        throw new DCCDataLayerException("Error 1109", "Unable to open database", errText+"\r\nConnectionString was [" + ConnectionStringHelpers.MaskConnectionString(connectionString) + "]");
                    }

                    errText += @"
" + DateTime.Now.ToLongTimeString() + (" Attempt: " + (6 - retries).ToString()) + @"
Error open() failed:
" + e.Message + @"
" + e.InnerException??"" + @"
------------------
";
                }
                if (isGood) { break; }
                if (--retries>0) Thread.Sleep(sleepTime *= 2);   // 2 secs,  4 secs,  8 secs,  16 secs,   32 secs   so  max 62 seconds
            }
            if (errText.Length > 0)
            {
                errText += @"
" + DateTime.Now.ToLongTimeString() + (isGood ? " succeeded" : " given up") + @"
------------------
";
                if (!isGood)
                {
                    throw new DCCDataLayerException("Error 1009","Unable to open database",errText+"\r\nConnectionString was [" + ConnectionStringHelpers.MaskConnectionString(connectionString)+"]");
                }
            }
            isOpened = isGood;

            return isGood;
        }

        /// <summary>
        /// Function to provide the core routine for connecting to a database.
        /// </summary>
        /// <param name="pConnectionString">Connection string.</param>
        /// <returns>True if successful.</returns>
        protected abstract bool ConnectDatabase(string pConnectionString);
        /// <summary>
        /// Function to provide the core routine for opening a database connection.
        /// </summary>
        /// <returns>True if successful.</returns>
        protected abstract bool OpenDatabase();
        /// <summary>
        /// Function to provide the core routine for closing a database connection.
        /// </summary>
        /// <returns>True if successful.</returns>
        protected abstract void CloseDatabase();

        /// <summary>
        /// Closes a database connection
        /// </summary>
        private  void CloseDatabaseConnection()
        {
            if (!isOpened) return;
            CloseDatabase();
            isOpened = false;
        }


        protected abstract string FetchCulture();

        #endregion

        #region Execute SQL etc

        #region Fill DataSet

        // rename parameters to match names in calling methods.
        private DataSet FillDS(string spName,List<DCC_DatabaseParameter> pList)
        {
            SortedList<string, object> paras = new SortedList<string, object>();
            string fullSQL = appendParas(spName,pList,paras);

            return FillDS(fullSQL,paras);
        }
        private DataSet FillDS(string spName)
        {
            return FillDS(spName, new SortedList<string, object>());
        }
        private DataSet FillDS(string spName, SortedList<string,object> paras)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            bool isCached = false;

            if (IsDebugMode)
            {
                DebugTrace.WriteLine();
                DebugTrace.WriteLine("------SelectData------ " + (isWebApp ? "  " + HttpContext.Current.Request.Url + "  for" + userIdentity : "") + " " + connectID);
            }

            string cacheName = "";

            verifyDirectSQLhasNoStrings(spName);

            if (isWebApp)
            {
                if (spName.StartsWith(cacheTableMarker))
                {
                    string[] tokens = spName.Split(new string[] {cacheTableMarker},StringSplitOptions.None);

                    if (tokens.Length==3 && tokens[1].Length > 0)
                    {
                        cacheName = cacheTableMarker + tokens[1];
                        spName = tokens[2];
                        isCached = true;
                    }
                }
                else
                {
                    foreach (string s in paras.Keys)
                    {
                        if (s.StartsWith(cacheTableMarker))
                        {
                            isCached = true;
                            cacheName += cacheTableMarker + s.Substring(cacheTableMarker.Length + 1);
                        }
                    }
                }
            }
            if (isCached)
            {
                cacheName = cacheName.Substring(cacheTableMarker.Length)+cacheParameterDivider+spName;
                foreach (string s in paras.Keys)
                {
                    if (s.StartsWith(cacheTableMarker)) continue;
                    if (s.StartsWith(DCC_DatabaseFrame.CURSOR)) continue;
                    object o = paras[s];
                    string oText = null;
                    if (o is string) oText=(string)o;
                    if (o is short) oText=((short)o).ToString();
                    if (o is int) oText=((int)o).ToString();
                    if (o is long) oText=((long)o).ToString();
                    if (o is float) oText=((float)o).ToString();
                    if (o is double) oText=((double)o).ToString();
                    if (o is decimal) oText=((decimal)o).ToString();
                    if (o is Boolean) oText=((Boolean)o).ToString();
                    if (o is DateTime) oText=((DateTime)o).ToString("yyyyMMddHHmmss");

                    if (oText == null)
                    {
                        throw new Exception("Cache doesn't handle parameter type ["+s+"]");
                    }
                    cacheName += cacheParameterDivider + oText.Trim();
                }
                cacheName+=cacheParameterDivider + connectID;

                object cache = HttpContext.Current.Cache[cacheName] ?? null;

                if (cache != null)
                {
                    result = (DataSet)cache;

                    if (IsDebugMode)
                    {
                        DebugTrace.WriteLine("Retrieved from cache [ " + cacheName + " ]");
                        string zDebug = "";
                        foreach (DataTable dt in result.Tables)
                        {
                            zDebug += " '" + dt.TableName + "' " + dt.Rows.Count.ToString() + " row" + (dt.Rows.Count == 1 ? "" : "s");
                        }
                        DebugTrace.WriteLine("Returned: " + result.Tables.Count.ToString() + " table" + (result.Tables.Count == 1 ? "" : "s") + " ...." + zDebug);
                        DebugTrace.WriteLine("  took " + stopwatch.Elapsed.TotalMilliseconds.ToString("###,##0.0000") + "ms" + (stopwatch.Elapsed.TotalMilliseconds > 500 ? "   Excessive" : ""));
                    }

                    if (thisCommand != null)
                    {
                        thisCommand.took = stopwatch.Elapsed;
                        thisCommand.fromCache = true;
                    }

                    return result;
                }
            }

            result = new DataSet();

            DoSelect(spName, paras);

            if (result.Tables.Count < 1)
            {
                throw new DCCDataLayerException("Error 1003","Database error - DATA NOT READ","No Tables Returned",spName,null);
            }

            if (IsDebugMode)
            {
                string zDebug2 = "";
                foreach (DataTable dt in result.Tables)
                {
                    zDebug2 += " '" + dt.TableName + "' " + dt.Rows.Count.ToString() + " row" + (dt.Rows.Count == 1 ? "" : "s");
                }
                DebugTrace.WriteLine("Returned: " + result.Tables.Count.ToString() + " table" + (result.Tables.Count == 1 ? "" : "s") + " ...." + zDebug2);
                DebugTrace.WriteLine("  took " + stopwatch.Elapsed.TotalMilliseconds.ToString("###,##0.0000") + "ms" + (stopwatch.Elapsed.TotalMilliseconds > 500 ? "   Excessive" : ""));
            }

            if (cacheName.Length > 0)
            {
				if (CacheLife > 0)
				{
					HttpContext.Current.Cache.Insert(
						 cacheName,
						 result,
						 null,
						 DateTime.UtcNow.AddMinutes(CacheLife),
						 System.Web.Caching.Cache.NoSlidingExpiration,
						 System.Web.Caching.CacheItemPriority.Default,
						 null);

                    if (IsDebugMode)
					{
						DebugTrace.WriteLine("Saved to cache until " + DateTime.Now.AddMinutes(10).ToString("HH:mm:ss") + " [ " + cacheName + " ]");
					}
				}
            }
            if (thisCommand != null)
            {
                thisCommand.took = stopwatch.Elapsed;
                thisCommand.fromCache = false;
            }

            return result;
        }

        /// <summary>
        /// Select data from database.
        /// </summary>
        protected abstract void DoSelect(string spName, SortedList<string, object> pParameters);

        public DataTable FetchSchema(string collectionName)
        {
            return DoSchema(collectionName);
        }

        /// <summary>
        /// Fetch Schema from database.
        /// </summary>
        protected abstract DataTable DoSchema(string collectionName);

        #endregion

        #region Fill Single DataRow
        private SortedList<string, object> FillDR(string spName)
        {
            return FillDR(spName, null,false);
        }

        private SortedList<string, object> FillDR(string spName,bool isDirect)
        {
            return FillDR(spName, null,isDirect);
        }

        private SortedList<string,object> FillDR(string spName,SortedList<string,object> paras)
        {
            if (spName.StartsWith(quietMarker)) spName = spName.Substring(quietMarker.Length);
            if (spName.StartsWith(directSQLcommandMarker)) return FillDR(spName.Substring(directSQLcommandMarker.Length), paras, true);
            return FillDR(spName, paras,false);
        }

        private SortedList<string,object> FillDR(string spName,SortedList<string,object> paras,bool DirectSQL)
        {
            if (IsDebugMode)
            {
                DebugTrace.WriteLine();
                DebugTrace.WriteLine("------SelectSingleRow------ " + (isWebApp ? "  " + HttpContext.Current.Request.Url+"  for"+userIdentity : "") + " " + connectID);
            }

            verifyDirectSQLhasNoStrings(spName);

            SortedList<string,object> results=new SortedList<string,object>(10);

             coreFillDR(spName, paras, DirectSQL, results);

            if (IsDebugMode)
            {
                DebugTrace.WriteLine("Results returned: "+(results.Count.ToString()));
            }
            
            return results;
        }

        protected virtual void coreFillDR(string spName, SortedList<string, object> paras, bool DirectSQL, SortedList<string, object> results)
        {
        }

        #endregion

        #region GetDataReader

        private int GetDataReader(string spName, ProcessDataReaderRow rowAction)
        {
            return GetDataReader(spName, null,rowAction,false);
        }

        private int GetDataReader(string spName, List<DCC_DatabaseParameter> pList, ProcessDataReaderRow rowAction)
        {
            SortedList<string, object> paras = new SortedList<string, object>();
            string fullSQL = appendParas(spName, pList, paras);

            if (fullSQL.StartsWith(quietMarker)) fullSQL = fullSQL.Substring(quietMarker.Length);
            if (fullSQL.StartsWith(directSQLcommandMarker))
                return GetDataReader(fullSQL.Substring(directSQLcommandMarker.Length), paras, rowAction, true);

            return GetDataReader(fullSQL, paras,rowAction,false);
        }
        private int GetDataReader(string spName,SortedList<string,object> paras, ProcessDataReaderRow rowAction,bool DirectSQL)
        {
            DbDataReader myDR = null;

            if (IsDebugMode)
            {
                DebugTrace.WriteLine();
                DebugTrace.WriteLine("------SelectWithDataReader------ "+ DateTime.Now.ToString("HH:mm:ss")+(isWebApp?"  "+HttpContext.Current.Request.Url+"  for "+userIdentity:""));
            }
            
            myDR = coreGetDR(spName, paras, DirectSQL, myDR);
            if (!myDR.HasRows)
            {
                if (IsDebugMode)
                {
                    DebugTrace.WriteLine("DataReader has no rows");
                }
                return -1;
            }

            int rKount = 0;
            while (myDR.Read())
            {
                if (!rowAction(myDR)) break;
                rKount++;
            }

            if (IsDebugMode)
            {
                DebugTrace.WriteLine("DataReader has "+rKount.ToString()+" rows");
            }

            myDR.Close();
            return rKount;
        }

        protected abstract DbDataReader coreGetDR(string spName, SortedList<string, object> paras, bool DirectSQL, DbDataReader myDR);

        #endregion

        #region execute Non-Query

        public struct DCC_DB_NonQuery
        {   public bool ok;    // success ?
            public int rows;   // rows returned
            public int newPK;  // new Primary Key
            public int returnENQ;  // value returned by an executeNonQuery
        }

        private int executeNonQueryPK(string spName, List<DCC_DatabaseParameter> pList)
        {
            return executeNonQueryPK(spName, pList, 1);
        }

        private int executeNonQueryPK(string spName, List<DCC_DatabaseParameter> pList, int expectedRows)
        {
            SortedList<string, object> paras = new SortedList<string, object>();
            string fullSQL = appendParas(spName,pList,paras);

            DCC_DB_NonQuery enq = doENQ(fullSQL, paras, expectedRows);

            return enq.newPK;
        }
        private bool executeNonQuery(string spName)
        {
            DCC_DB_NonQuery enq = doENQ(spName, new SortedList<string,object>(0), 1);
            return enq.ok;
        }
        private bool executeNonQuery(string spName, List<DCC_DatabaseParameter> pList)
        {
            return executeNonQuery(spName,pList,1);
        }
        private bool executeNonQuery(string spName,List<DCC_DatabaseParameter> pList,int expectedRows)
        {
            SortedList<string, object> paras = new SortedList<string, object>();
            string fullSQL = appendParas(spName,pList,paras);

            DCC_DB_NonQuery enq = doENQ(fullSQL, paras, expectedRows);
            return enq.ok;
        }

        private bool executeNonQuery(string spName,SortedList<string,object> paras)
        {
            DCC_DB_NonQuery enq = doENQ(spName, paras, 1);
            return enq.ok;
        }

        private DCC_DB_NonQuery executeNonQueryBase(string spName)
        {
            return doENQ(spName, new SortedList<string,object>(0), 1);
        }
        private DCC_DB_NonQuery executeNonQueryBase(string spName, List<DCC_DatabaseParameter> pList)
        {
            return executeNonQueryBase(spName,pList,1);
        }
        private DCC_DB_NonQuery executeNonQueryBase(string spName,List<DCC_DatabaseParameter> pList,int expectedRows)
        {
            SortedList<string, object> paras = new SortedList<string, object>();
            string fullSQL = appendParas(spName,pList,paras);

            return doENQ(fullSQL, paras, expectedRows);
        }

        private DCC_DB_NonQuery executeNonQueryBase(string spName,SortedList<string,object> paras)
        {
            return doENQ(spName, paras, 1);
        }

        private DCC_DB_NonQuery executeNonQueryBase(string spName,SortedList<string,object> paras,bool silent)
        {
            return doENQ(spName, paras, 1, silent);
        }

        protected DCC_DB_NonQuery doENQ(string spName,SortedList<string,object> paras,int expectedRows)
        {
            return doENQ(spName, paras, expectedRows, false);
        }

        protected DCC_DB_NonQuery doENQ(string spName,SortedList<string,object> paras,int expectedRows,bool silent)
        {
            if (IsDebugMode)
            {
                DebugTrace.WriteLine();
                DebugTrace.WriteLine("------NonQuery------ " + (isWebApp ? "  " + HttpContext.Current.Request.Url + "  for " + userIdentity : "") + "  " + connectID);
            }

            DCC_DB_NonQuery theResult = new DCC_DB_NonQuery();
            theResult.ok = false;
            theResult.rows = -1;
            theResult.newPK = -99;
            bool hasPK = false;

            verifyDirectSQLhasNoStrings(spName);

            if (isWebApp)
            {
                foreach (string sK in paras.Keys)
                {
                    if (sK.StartsWith(cacheTableMarker))
                    {
                        string invalidate = sK.Substring(cacheTableMarker.Length + 1);
                        List<string> deadCaches = new List<string>(5);
                        System.Collections.IDictionaryEnumerator cacheEnum = HttpContext.Current.Cache.GetEnumerator();
                        while (cacheEnum.MoveNext())
                        {
                            string s = (string)cacheEnum.Key;
                            string[] s0 = s.Split(new string[] { cacheParameterDivider }, StringSplitOptions.None);
                            string[] s1 = s0[0].Split('$');
                            foreach (string s2 in s1)
                            {
                                if (s2 == invalidate) deadCaches.Add(s);
                            }
                        }
                        foreach (string dc in deadCaches)
                        {
                            HttpContext.Current.Cache.Remove(dc);
                            if (IsDebugMode)
                            {
                                DebugTrace.WriteLine("Removed cache [ " + dc + " ]");
                            }
                        }
                    }
                }
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            DoNonQuery(spName, paras, expectedRows, ref theResult, ref hasPK);

            if (IsDebugMode)
            {
                DebugTrace.WriteLine("Success: " + (theResult.ok ? "OK" : "Failed") + "   Rows: " + theResult.rows.ToString() + (hasPK ? "  primarykey: " + theResult.newPK.ToString() : ""));
                DebugTrace.WriteLine("  took " + stopwatch.Elapsed.TotalMilliseconds.ToString("###,##0.0000") + "ms" + (stopwatch.Elapsed.TotalMilliseconds > 500 ? "   Excessive" : ""));
            }
            if (thisCommand != null)
            {
                thisCommand.took = stopwatch.Elapsed;
            }

            return (theResult);
        }

        protected abstract void DoNonQuery(string spName, SortedList<string, object> paras, int expectedRows, ref DCC_DB_NonQuery theResult, ref bool hasPK);

        #endregion

        #region Direct SQL

        public bool DirectSQL(string pSQLStatement)
        {
            return DirectSQL(pSQLStatement, 1);
        }

        public bool DirectSQL(string pSQLStatement, int pExpectedRows)
        {
            if (IsDebugMode)
            {
                DebugTrace.WriteLine();
                DebugTrace.WriteLine("------DirectSQL------ "+ DateTime.Now.ToString("HH:mm:ss")+(isWebApp?"  "+HttpContext.Current.Request.Url+"  for "+userIdentity:"")+"  "+connectID);
            }
            verifyDirectSQLhasNoStrings(pSQLStatement);
            int rows = -1;
            if (pSQLStatement.Length == 0) return true;
            rows = coreDirectSQL(pSQLStatement, rows);
            if (IsDebugMode)
            {
                DebugTrace.WriteLine("Rows: "+rows.ToString());
            }
            return (rows == pExpectedRows);
        }

        protected abstract int coreDirectSQL(string pSQLStatement, int pRows);

        #endregion

        #region Transactions

        public void TransactionBegin()
        {
            DirectSQL("BEGIN TRANSACTION;");
        }

        public void TransactionEnd(bool pDoCommit)
        {
            DirectSQL((pDoCommit ? "COMMIT" : "ROLLBACK") + " TRANSACTION;");
        }
        #endregion

        #endregion

        #region Shells and Helpers

        /*  From earlier releases - not supported for the time being
 *
 *
        /// <summary>
        /// Insert an object into the given table.
        /// Object properties must match table column names.
        /// Appropriate SQL statement is generated and executed ... INSERT INTO xxxx (col1, col2 ...) VALUES (.....);
        ///
        /// </summary>
        /// <param name="pObject">Object to be inserted into table.</param>
        /// <param name="pTableName">Table in which the object is to be inserted.</param>
        /// <returns>True for success, false if failure.</returns>
        public bool AutoInsert(object pObject,string pTableName)
        {
            int reply = AutoInsert(pObject, pTableName, false);

            if (reply > -1) return true;
            return false;
        }
        /// <summary>
        /// Insert an object into a table.
        /// Object properties must match table column names.
        /// Appropriate SQL statement is generated and executed ... INSERT INTO xxxx (col1, col2 ...) VALUES (.....);
        ///
        /// </summary>
        /// <param name="pObject">Object to be inserted into table.</param>
        /// <param name="pTableName">Table in which the object is to be inserted.</param>
        /// <param name="pGetPrimaryKey">True to return primary key for the new row.</param>
        /// <returns>-1 if failure, 0 if pGetPrimaryKey was false otherwise the primary key of the new row.</returns>
        public int AutoInsert(object pObject, string pTableName, bool pGetPrimaryKey)
        {
            updateFrame uf = insert.insertObject(pObject);

            string SQL = "**INSERT INTO " + pTableName;
            SortedList<string, object> paras = makeParameterisedSQL(uf, ref SQL);

            if (!pGetPrimaryKey)
            {
                return (executeNonQuery(SQL, paras) ? 0 : -1);
            }
            else
            {
                DCC_DB_Frame dif = new DCC_DB_Frame();
                SQL += "SELECT @@ROWCOUNT as rows,SCOPE_IDENTITY() as newID,@@IDENTITY as identityKey;";
                SortedList<string, object> newRow = FillDR(SQL, paras);
                if (newRow.Count == 0) return -999;
                object r2 = newRow["rows"]; dif.rows = (Convert.IsDBNull(r2) ? -999 : (int)r2);
                object n1 = newRow["newID"]; dif.key = (Convert.IsDBNull(n1) ? -1 : (int)((decimal)n1));
                if (dif.Key == -1)
                {
                    object i1 = newRow["identityKey"]; dif.key = (Convert.IsDBNull(i1) ? -1 : (int)((decimal)i1));
                }
                if (dif.Rows == 1) return dif.Key;
                return -1;
            }
        }

        /// <summary>
        /// (only used by AutoInsert - to convert format of parameters generated in an update frame)
        /// </summary>
        /// <param name="uf"></param>
        /// <param name="SQL"></param>
        /// <returns></returns>
        private static SortedList<string, object> makeParameterisedSQL(updateFrame uf, ref string SQL)
        {
            SortedList<string, object> paras = new SortedList<string, object>(20);

            string f = "";
            string v = "";
            string delim = "";
            for (int fv = 0; fv < uf.updateValues.Count; fv++)
            {
                if (uf.updateFields[fv].ToUpper().StartsWith("K_")) continue;
                //if (uf.updateFields[fv].StartsWith("_")) continue;
                if (uf.updateFields[fv].ToUpper().StartsWith("LU_")) continue;

                string typeID="";   // used for setting sql parameter type - internal so is generalised
                switch (uf.updateTypes[fv].ToString())
                {
                    case "System.Int32":      {  typeID="I"; break;  }
                    case "System.String":     {  typeID="S"; break;  }
                    case "System.DateTime":   {  typeID="DT"; break; }
                    case "System.Boolean":    {  typeID="B"; break;  }
                    case "System.Double":     {  typeID="D"; break;  }
                    case "System.Decimal":    {  typeID="D"; break;  }
                    default: {
                        if (uf.updateValues[fv] == null) typeID = "S"; // if null then cannot determine type, so mark as String which will be OK as a @parameter
                        break; }
                }

                f += delim + uf.updateFields[fv];
                v += delim + "@" + uf.updateFields[fv];
                delim = ",";

                object po = uf.updateValues[fv];
                string pk= typeID+"@"+uf.updateFields[fv];
                if (po == null) po = DBNull.Value;
                if (!paras.ContainsKey(pk))
                {
                    paras.Add(pk, po);
                }
                else
                {
                    paras[pk] = po;
                }

            }
            SQL+=" (" + f + ") VALUES (" + v + ");";
            return paras;
        }

*
*/


        private static string appendParas(string doSql, List<DCC_DatabaseParameter> pList, SortedList<string, object> paras)
        {
            if (pList != null)
            {
                foreach (DCC_DatabaseParameter p in pList)
                {
                    string pKey = p.Type + "@" + p.Name;
                    if (p.Type == "S") pKey = "S@" + p.Length.ToString() + "@" + p.Name;
                    paras.Add(pKey, p.Value);
                }
            }
            return doSql;
        }

        protected bool convertParas(SortedList<string, object> paras, DbCommand comm)
        {
            bool hasPrimaryKey = false;

            if (paras != null)
            {
                foreach (string para in paras.Keys)
                {
                    if (para.StartsWith(cacheTableMarker)) continue; // ignore Cache indicators

                    DbParameter dbp=null;
                    string parameterMarker = "";

                    if (comm is SqlCommand) { dbp = new SqlParameter(); parameterMarker = "@"; }
					if (comm is OracleCommand) dbp = new OracleParameter();
                    //  comm is Oracle.DataAccess.Client.OracleCommand  handled by alternative overloaded convertParas function
                    if (comm is OleDbCommand) dbp = new OleDbParameter();

                    if (dbp==null) throw new DCCDataLayerException("DB8001","System Error","Unexpected 'comm' type passed to 'convertParas'");

                    //dbp.DbType = DbType.Object;
                    dbp.Value = DBNull.Value;

                    int spLen = 127;
                    string[] pparts = para.Split('@');
                    if (pparts.Length == 3)
                    {
                        if (pparts[0] == "S")
                        {
                            try
                            {
                                spLen = int.Parse(pparts[1]);
                            }
                            catch { }
                        }
                        pparts[1] = pparts[2];
                    }
                    dbp.ParameterName = parameterMarker + pparts[1];
                    dbp.Direction = ParameterDirection.Input;
                    if (pparts[0].Contains("="))
                    {
                        dbp.Direction = ParameterDirection.ReturnValue;
                        pparts[0] = pparts[0].Replace("=", "");
                    }
                    if (pparts[0].Contains("<>"))
                    {
                        dbp.Direction = ParameterDirection.InputOutput;
                        pparts[0] = pparts[0].Replace("<>", "");
                    }
                    if (pparts[0].Contains("<"))
                    {
                        dbp.Direction = ParameterDirection.Output;
                        pparts[0] = pparts[0].Replace("<", "");
                        hasPrimaryKey = true;
                    }
                    if (pparts[0].Contains(">"))
                    {
                        dbp.Direction = ParameterDirection.Input;
                        pparts[0] = pparts[0].Replace(">", "");
                    }
                    switch (pparts[0])
                    {
                        case "OC":
                            {
                                dbp.ParameterName = "!!" + dbp.ParameterName;
                                break;
                            }
                        case "64":
                            {
                                dbp.DbType = DbType.Int64;
                                break;
                            }
                        case "I":
                            {
                                dbp.DbType = DbType.Int32;
                                break;
                            }
                        case "16":
                            {
                                dbp.DbType = DbType.Int16;
                                break;
                            }
                        case "8":
                            {
                                dbp.DbType = DbType.Byte;
                                break;
                            }
                        case "B":
                            {
                                dbp.DbType = DbType.Boolean;
                                break;
                            }
                        case "S":
                            {
                                dbp.DbType = DbType.String;
                                dbp.Size = spLen;
                                break;
                            }
                        case "DT":
                            {
                                dbp.DbType = DbType.DateTime;
                                break;
                            }
                        case "BLOB":
                            {
                                dbp.ParameterName = ((this is DCC_OracleDataLayer /*|| this is DCC_ODAC_DataLayer*/)?"!B!":"") + dbp.ParameterName;
                                break;
                            }
                        case "CLOB":
                            {
                                dbp.ParameterName = ((this is DCC_OracleDataLayer /*|| this is DCC_ODAC_DataLayer*/)?"!C!":"") + dbp.ParameterName;
                                break;
                            }
                        case "D":
                            {
                                dbp.DbType = DbType.Decimal;
                                break;
                            }
                        case "FF":
                            {
                                dbp.DbType = DbType.Double;
                                break;
                            }
                        case "F":
                            {
                                dbp.DbType = DbType.Single;
                                break;
                            }
                        case "G":
                            {
                                dbp.DbType = DbType.Guid;
                                break;
                            }
                        //case "?":
                        //    {
                        //        dbp.DbType = DbType.Object;  //  use for null where cannot determine C# datatype
                        //        break;
                        //    }
                        default:
                            {
                                isInError = true;
                                lastError = new ErrorReport("0911", "Database coding error - UNEXPECTED Parameter type", "SQL was: " + comm.CommandText + "<br />Para:" + para, "", ErrorLevel.SevereError);
                                break;
                            }
                    }
                    dbp.Value = paras[para] ?? DBNull.Value;

					if (dbp.ParameterName == "@_PRIMARYKEY")
					{
						dbp.Direction = ParameterDirection.Output;
						hasPrimaryKey = true;
					}

                    comm.Parameters.Add(dbp);
                }
            }
            return hasPrimaryKey;
        }
        //protected bool convertParas(SortedList<string, object> paras, Oracle.DataAccess.Client.OracleCommand comm)
        //{
        //    bool hasPrimaryKey = false;

        //    if (paras != null)
        //    {
        //        foreach (string para in paras.Keys)
        //        {
        //            if (para.StartsWith(cacheTableMarker)) continue; // ignore Cache indicators

        //            DbParameter dbp=null;
        //            string parameterMarker = "";

        //            dbp = new Oracle.DataAccess.Client.OracleParameter();

        //            if (dbp==null) throw new DCCDataLayerException("DB8001","System Error","Unexpected 'comm' type passed to 'convertParastest'");

        //            //dbp.DbType = DbType.Object;
        //            dbp.Value = DBNull.Value;

        //            int spLen = 127;
        //            string[] pparts = para.Split('@');
        //            if (pparts.Length == 3)
        //            {
        //                if (pparts[0] == "S")
        //                {
        //                    try
        //                    {
        //                        spLen = int.Parse(pparts[1]);
        //                    }
        //                    catch { }
        //                }
        //                pparts[1] = pparts[2];
        //            }
        //            dbp.ParameterName = parameterMarker + pparts[1];
        //            dbp.Direction = ParameterDirection.Input;
        //            if (pparts[0].Contains("="))
        //            {
        //                dbp.Direction = ParameterDirection.ReturnValue;
        //                pparts[0] = pparts[0].Replace("=", "");
        //            }
        //            if (pparts[0].Contains("<>"))
        //            {
        //                dbp.Direction = ParameterDirection.InputOutput;
        //                pparts[0] = pparts[0].Replace("<>", "");
        //            }
        //            if (pparts[0].Contains("<"))
        //            {
        //                dbp.Direction = ParameterDirection.Output;
        //                pparts[0] = pparts[0].Replace("<", "");
        //                hasPrimaryKey = true;
        //            }
        //            if (pparts[0].Contains(">"))
        //            {
        //                dbp.Direction = ParameterDirection.Input;
        //                pparts[0] = pparts[0].Replace(">", "");
        //            }
        //            switch (pparts[0])
        //            {
        //                case "OC":
        //                    {
        //                        dbp.ParameterName = "!!" + dbp.ParameterName;
        //                        break;
        //                    }
        //                case "64":
        //                    {
        //                        dbp.DbType = DbType.Int64;
        //                        break;
        //                    }
        //                case "I":
        //                    {
        //                        dbp.DbType = DbType.Int32;
        //                        break;
        //                    }
        //                case "16":
        //                    {
        //                        dbp.DbType = DbType.Int16;
        //                        break;
        //                    }
        //                case "8":
        //                    {
        //                        dbp.DbType = DbType.Byte;
        //                        break;
        //                    }
        //                case "B":
        //                    {
        //                        dbp.DbType = DbType.Boolean;
        //                        break;
        //                    }
        //                case "S":
        //                    {
        //                        dbp.DbType = DbType.String;
        //                        dbp.Size = spLen;
        //                        break;
        //                    }
        //                case "DT":
        //                    {
        //                        dbp.DbType = DbType.DateTime;
        //                        break;
        //                    }
        //                case "BLOB":
        //                    {
        //                        dbp.ParameterName = ((this is DCC_ODPDataLayer /*|| this is DCC_ODAC_DataLayer*/) ? "!B!" : "") + dbp.ParameterName;

        //                        break;
        //                    }
        //                case "CLOB":
        //                    {
        //                       dbp.ParameterName = ((this is DCC_ODPDataLayer /*|| this is DCC_ODAC_DataLayer*/) ? "!C!" : "") + dbp.ParameterName;

        //                        break;
        //                    }
        //                case "D":
        //                    {
        //                        dbp.DbType = DbType.Decimal;
        //                        break;
        //                    }
        //                case "FF":
        //                    {
        //                        dbp.DbType = DbType.Double;
        //                        break;
        //                    }
        //                case "F":
        //                    {
        //                        dbp.DbType = DbType.Single;
        //                        break;
        //                    }
        //                //case "?":
        //                //    {
        //                //        dbp.DbType = DbType.Object;  //  use for null where cannot determine C# datatype
        //                //        break;
        //                //    }
        //                default:
        //                    {
        //                        isInError = true;
        //                        lastError = new ErrorReport("0911", "Database coding error - UNEXPECTED Parameter type", "SQL was: " + comm.CommandText + "<br />Para:" + para, "", ErrorLevel.SevereError);
        //                        break;
        //                    }
        //            }
        //            dbp.Value = paras[para] ?? DBNull.Value;

        //            if (dbp.ParameterName == "@_PRIMARYKEY")
        //            {
        //                dbp.Direction = ParameterDirection.Output;
        //                hasPrimaryKey = true;
        //            }
        //            comm.Parameters.Add(dbp);
        //        }
        //    }
        //    return hasPrimaryKey;
        //}

        private static void verifyDirectSQLhasNoStrings(string spName)
        {
            //if (spName.StartsWith(quietMarker)) spName = spName.Substring(quietMarker.Length);

            //if (spName.StartsWith(directSQLcommandMarker))
            //{
            //    if (spName.Contains("'"))
            //    {
            //        throw new DCCDataLayerException("Error 5001", "Database Error", "Illegal character found in SQL", spName, new ArgumentException("Direct SQL cannot contain single quotes"));
            //    }
            //}
        }
        #endregion

        #region ErrorHandling

        /// <summary>
        /// Routine to handle any errors.
        /// </summary>
        public void HandleErrors()
        {
            if (isInError)
            {
                lastError.ReportErrorsToUser(isWebApp);
            }
        }

        [Serializable()]
        public class DCCDataLayerException : System.Exception
        {
            public string errorID = "";
            public string userMessage = "";
            public string supportMessage = "";

            public DCCDataLayerException() : base() { }
            public DCCDataLayerException(string message) : base(message) { }

            public DCCDataLayerException(string errorID, string userMessage, string message) : base(message)
            {
                //  NEW STUFF
                string s = ConfigurationManager.AppSettings["DebugMode"] ?? "true";
                bool IsDebugMode = (s.ToLower() == "true");
                if (IsDebugMode)
                {
                    DebugTrace.WriteLine(Message);
                }
                this.errorID=errorID;
                this.userMessage = userMessage;
                this.supportMessage += message;
            }

            public DCCDataLayerException(string errorID, string userMessage, string supportMessage, object command, System.Exception inner) : base(supportMessage, inner)
            {
                //  NEW STUFF
                string s = ConfigurationManager.AppSettings["DebugMode"] ?? "true";
                bool IsDebugMode = (s.ToLower() == "true");
                if (IsDebugMode)
                {
                    DebugTrace.WriteLine(Message);
                    if (inner != null)
                    {
                        DebugTrace.WriteLine(inner.Message);
                        if (inner.InnerException != null) DebugTrace.WriteLine(inner.InnerException.Message);
                    }
                }
                this.errorID=errorID;
                this.userMessage = userMessage;
                this.supportMessage = supportMessage+"\r\n";
                StringBuilder sb = new StringBuilder();
                string cText = "";
                CommandType cType = CommandType.Text;

                if (command is DbCommand)
                {
                    System.Data.Common.DbCommand dbComm = (System.Data.Common.DbCommand)command;
                    cText = dbComm.CommandText;
                    foreach (System.Data.Common.DbParameter p in dbComm.Parameters)
                    {
                        try
                        {
                            sb.Append("\r\n" + p.ParameterName + " (" + p.DbType.ToString() + (p.Size == 0 ? "" : " " + p.Size.ToString()) + ") = [" + (p.Value == null ? "(null)" : p.Value.ToString()) + "]\t");
                        }
                        catch
                        {
                            sb.Append("\r\nUnrecognised Parameter");
                        }
                    }
                    cType = dbComm.CommandType;

                    switch (cType)
                    {
                        case (CommandType.Text):
                            {
                                this.supportMessage += "  Command:" + cText + "  Paras:" + sb.ToString();
                                break;
                            }
                        case (CommandType.StoredProcedure):
                            {
                                this.supportMessage += "  Stored Procedure:" + cText + "  Paras:" + sb.ToString();
                                break;
                            }
                        case (CommandType.TableDirect):
                            {
                                this.supportMessage += "  Table:" + cText + "  Paras:" + sb.ToString();
                                break;
                            }
                    }
                }
                if (command is string)
                {   this.supportMessage+=" Command: "+(command as string);
                }

                if (inner != null)
                {
                    this.supportMessage += "\r\n\r\nNested Exception Info:";

                    int innerK = 0;
                    Exception i = inner;
                    while (i != null && innerK<25)
                    {
                        string innerKs = "["+(++innerK).ToString()+"]";
                        if (i is DCC_DataLayer.DCCDataLayerException) this.supportMessage += "\r\n\tError ID" + innerKs + ":\r\n\t\t" + ((i as DCCDataLayerException).errorID ?? "-");
                        this.supportMessage += "\r\n\tSource" + innerKs + ":\r\n\t\t" + (i.Source ?? "-")
                                            + "\r\n\tMessage" + innerKs + ":\r\n\t\t" + (i.Message ?? "-")
                                            + "\r\n\tStack" + innerKs + ":\r\n\t\t" + (i.StackTrace ?? "-");
                        i = i.InnerException;
                    }
                }
                this.supportMessage += "\r\n\r\n";
            }

            // Constructor needed for serialization
            // when exception propagates from a remoting server to the client.
            protected DCCDataLayerException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            {
            }
        }
        #endregion

        #region Version Control

        public struct VersionControlFrame
        {
            public bool isEmpty;
            public int MajorVersion;
            public int MinorVersion;
            public int Build;
            public DateTime ReleaseDate;
            public string text;
            public string versionText
            {
                get
                {
                    if (isEmpty) return "?.?.?";
                    return MajorVersion.ToString() + "." + MinorVersion.ToString() + "." + Build.ToString();
                }
            }
        }

        public VersionControlFrame ReadVersion(string SoftwareID)
        {
            VersionControlFrame vc=new VersionControlFrame();
            vc.isEmpty = true;

            try
            {
                DCC_DatabaseFrame dif=new DCC_DatabaseFrame(directSQLcommandMarker+"SELECT * From _VersionControl where SoftwareIdentityCode=@software");
                dif.AddParameter("software", SoftwareID,20);
                SelectSingleRow(dif);
                SortedList<string, object> newRow = dif.columnSet;

                if (newRow.Count == 0) return vc;

                vc.isEmpty = false;
                object major = newRow["MajorVersion"]; vc.MajorVersion = (Convert.IsDBNull(major) ? -999 : (int)major);
                object minor = newRow["MinorVersion"]; vc.MinorVersion = (Convert.IsDBNull(minor) ? -999 : (int)minor);
                object build = newRow["Build"];        vc.Build        = (Convert.IsDBNull(build) ? -999 : (int)build);
                object rdate = newRow["ReleaseDate"];  vc.ReleaseDate  = (Convert.IsDBNull(rdate) ? DateTime.MinValue : (DateTime)rdate);
            }
            catch  {}
            return vc;
        }

        public bool UpdateVersion(VersionControlFrame vc,string softwareID)
        {
            return UpdateVersion(vc, softwareID, "");
        }

        public bool UpdateVersion(VersionControlFrame vc,string softwareID,string softwareDescription)
        {
            DCC_DatabaseFrame dif=new DCC_DatabaseFrame(directSQLcommandMarker+"UPDATE _versionControl set MajorVersion=@majVer,MinorVersion=@minVer,Build=@build,ReleaseDate=@stamp where SoftwareIdentityCode=@software");
            dif.AddParameter("majVer", vc.MajorVersion);
            dif.AddParameter("minVer", vc.MinorVersion);
            dif.AddParameter("build",  vc.Build);
            dif.AddParameter("stamp", DateTime.Now);
            dif.AddParameter("software", softwareID,20);
            dif.ExpectedRows = DCC_DatabaseFrame.UNKNOWN;
            NonQuery(dif);
            if (dif.Rows>0) return true; // don't worry about 2 or more
            dif.CommandText=directSQLcommandMarker+"INSERT INTO _versionControl (SoftwareIdentityCode,MajorVersion,MinorVersion,Build,ReleaseDate,SoftwareDescription,VC_ID) values (@software,@majVer,@minVer,@build,@stamp,@desc,@VC_ID)";
            dif.AddParameter("desc", softwareDescription, 100);
            dif.AddParameter("VC_ID", Guid.NewGuid());
            dif.ExpectedRows = 1;
            NonQuery(dif);
            return (dif.ok);
        }

        #endregion

    }

    #endregion

    #region DCC_GetColumn class

    /// <summary>
    /// Extract given types from Dataset row by column
    ///
    /// DBNull is converted to 0, false, empty string, or 1/1/1900 as appropriate unless a nullable type is requested.
    ///
    ///
    /// </summary>
    public class DCC_GetColumn
    {

        #region Logical Types
        /// <summary>
        /// Logical - Boolean
        /// </summary>
        /// <param name="dr0">DataRow or DbDataReader</param>
        /// <param name="col">string column name or int Column Index</param>
        /// <param name="checkColumn">true (default) if column name or index must be checked for existence, false skips column name or index validation</param>
        /// <returns>DBNulls return false</returns>
        public static bool Bit(object dr0, object col, bool checkColumn=true)
        {
            if (checkColumn) CheckColumnExists(col, dr0);
            if (col is String)
            {
                string colName = (string)col;
                if (dr0 is DataRow)      { DataRow      dr = (DataRow)dr0;       try { if (Convert.IsDBNull(dr[colName])) return false; else return (bool)dr[colName]; } catch { return false; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colName])) return false; else return (bool)dr[colName]; } catch { return false; } }
            }
            if (col is int || col is short || col is long || col is byte)
            {
                int colNumber = (int)col;
                if (dr0 is DataRow)      { DataRow      dr = (DataRow)dr0;       try { if (Convert.IsDBNull(dr[colNumber])) return false; else return (bool)dr[colNumber]; } catch { return false; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colNumber])) return false; else return (bool)dr[colNumber]; } catch { return false; } }
            }
            return false;
        }

        /// <summary>
        /// Nullable  Logical - Boolean
        /// </summary>
        /// <param name="dr0">DataRow</param>
        /// <param name="col">string Column Name or int Column Index</param>
        /// <param name="checkColumn">true (default) if column name or index must be checked for existence, false skips column name or index validation</param>
        /// <returns>DBNull return false</returns>
        public static bool? BitQ(object dr0, object col, bool checkColumn=true)
        {
            if (checkColumn) CheckColumnExists(col, dr0);
            if (col is String)
            {
                string colName = (string)col;
                if (dr0 is DataRow)      { DataRow      dr = (DataRow)dr0;       try { if (Convert.IsDBNull(dr[colName])) return null; else return (bool?)dr[colName]; } catch { return null; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colName])) return null; else return (bool?)dr[colName]; } catch { return null; } }
            }
            if (col is int || col is short || col is long || col is byte)
            {
                int colNumber = (int)col;
                if (dr0 is DataRow)      { DataRow      dr = (DataRow)dr0;       try { if (Convert.IsDBNull(dr[colNumber])) return null; else return (bool?)dr[colNumber]; } catch { return null; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colNumber])) return null; else return (bool?)dr[colNumber]; } catch { return null; } }
            }
            return null;
        }
        #endregion

        #region Numeric Types

        /// <summary>
        /// 8 Bits - byte
        /// </summary>
        /// <param name="dr0">DataRow or DbDataReader</param>
        /// <param name="col">string Column Name or int Column Index</param>
        /// <param name="checkColumn">true (default) if column name or index must be checked for existence, false skips column name or index validation</param>
        /// <param name="caughtValue">value returned if try block is caught (default=0)</param>
        /// <param name="nullValue">value returned if DBNull found (default=0)</param>
        /// <returns>cast column value or caughtValue or nullValue</returns>
        public static byte Integer08(object dr0, object col, bool checkColumn=true, byte caughtValue=0, byte nullValue=0)
        {
            if (checkColumn) CheckColumnExists(col, dr0);
            if (col is String)
            {
                string colName = (string)col;
                if (dr0 is DataRow)      { DataRow      dr =      (DataRow)dr0;  try { if (Convert.IsDBNull(dr[colName])) return nullValue; else return (byte)dr[colName]; } catch { return caughtValue; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colName])) return nullValue; else return (byte)dr[colName]; } catch { return caughtValue; } }
            }
            if (col is int || col is short || col is long || col is byte)
            {
                int colNumber = (int)col;
                if (dr0 is DataRow)      { DataRow      dr =      (DataRow)dr0;  try { if (Convert.IsDBNull(dr[colNumber])) return nullValue; else return (byte)dr[colNumber]; } catch { return caughtValue; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colNumber])) return nullValue; else return (byte)dr[colNumber]; } catch { return caughtValue; } }
            }
            return caughtValue;
        }

        /// <summary>
        /// Nullable 8 Bits - byte
        /// </summary>
        /// <param name="dr0">DataRow or DbDataReader</param>
        /// <param name="col">string Column Name or int Column Index</param>
        /// <param name="checkColumn">true (default) if column name or index must be checked for existence, false skips column name or index validation</param>
        /// <returns>DBNull return null</returns>
        public static byte? Integer08Q(object dr0, object col, bool checkColumn=true)
        {
            if (checkColumn) CheckColumnExists(col, dr0);
            if (col is String)
            {
                string colName = (string)col;
                if (dr0 is DataRow)      { DataRow      dr =      (DataRow)dr0;  try { if (Convert.IsDBNull(dr[colName])) return null; else return (byte?)dr[colName]; } catch { return null; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colName])) return null; else return (byte?)dr[colName]; } catch { return null; } }
            }
            if (col is int || col is short || col is long || col is byte)
            {
                int colNumber = (int)col;
                if (dr0 is DataRow)      { DataRow      dr =      (DataRow)dr0;  try { if (Convert.IsDBNull(dr[colNumber])) return null; else return (byte?)dr[colNumber]; } catch { return null; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colNumber])) return null; else return (byte?)dr[colNumber]; } catch { return null; } }
            }
            return null;
        }

        /// <summary>
        /// 16 Bits - short
        /// </summary>
        /// <param name="dr0">DataRow or DbDataReader</param>
        /// <param name="col">string Column Name or int Column Index</param>
        /// <param name="checkColumn">true (default) if column name or index must be checked for existence, false skips column name or index validation</param>
        /// <param name="caughtValue">value returned if try block is caught (default=0)</param>
        /// <param name="nullValue">value returned if DBNull found (default=0)</param>
        /// <returns>cast column value or caughtValue or nullValue</returns>
        public static short Integer16(object dr0, object col, bool checkColumn=true, short caughtValue=0, short nullValue=0)
        {
            if (checkColumn) CheckColumnExists(col, dr0);
            if (col is String)
            {
                string colName = (string)col;
                if (dr0 is DataRow)      { DataRow      dr =      (DataRow)dr0;  try { if (Convert.IsDBNull(dr[colName])) return nullValue; else return (short)dr[colName]; } catch { return caughtValue; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colName])) return nullValue; else return (short)dr[colName]; } catch { return caughtValue; } }
            }
            if (col is int || col is short || col is long || col is byte)
            {
                int colNumber = (int)col;
                if (dr0 is DataRow)      { DataRow      dr =      (DataRow)dr0;  try { if (Convert.IsDBNull(dr[colNumber])) return nullValue; else return (short)dr[colNumber]; } catch { return caughtValue; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colNumber])) return nullValue; else return (short)dr[colNumber]; } catch { return caughtValue; } }
            }
            return caughtValue;
        }

        /// <summary>
        /// Nullable 16 Bits - short
        /// </summary>
        /// <param name="dr0">DataRow or DbDataReader</param>
        /// <param name="col">string Column Name or int Column Index</param>
        /// <param name="checkColumn">true (default) if column name or index must be checked for existence, false skips column name or index validation</param>
        /// <returns>DBNull return null</returns>
        public static short? Integer16Q(object dr0, object col, bool checkColumn=true)
        {
            if (checkColumn) CheckColumnExists(col, dr0);
            if (col is String)
            {
                string colName = (string)col;
                if (dr0 is DataRow)      { DataRow      dr =      (DataRow)dr0;  try { if (Convert.IsDBNull(dr[colName])) return null; else return (short?)dr[colName]; } catch { return null; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colName])) return null; else return (short?)dr[colName]; } catch { return null; } }
            }
            if (col is int || col is short || col is long || col is byte)
            {
                int colNumber = (int)col;
                if (dr0 is DataRow)      { DataRow      dr =      (DataRow)dr0;  try { if (Convert.IsDBNull(dr[colNumber])) return null; else return (short?)dr[colNumber]; } catch { return null; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colNumber])) return null; else return (short?)dr[colNumber]; } catch { return null; } }
            }
            return null;
        }

        /// <summary>
        /// 32 Bits - int
        /// </summary>
        /// <param name="dr0">DataRow or DbDataReader</param>
        /// <param name="col">string Column Name or int Column Index</param>
        /// <param name="checkColumn">true (default) if column name or index must be checked for existence, false skips column name or index validation</param>
        /// <param name="caughtValue">value returned if try block is caught (default=0)</param>
        /// <param name="nullValue">value returned if DBNull found (default=0)</param>
        /// <returns>cast column value or caughtValue or nullValue</returns>
        public static int Integer32(object dr0, object col, bool checkColumn=true, int caughtValue=0, int nullValue=0)
        {
            if (checkColumn) CheckColumnExists(col, dr0);
            if (col is String)
            {
                string colName = (string)col;
                if (dr0 is DataRow)      { DataRow      dr =      (DataRow)dr0;  try { if (Convert.IsDBNull(dr[colName])) return nullValue; else return (int)dr[colName]; } catch { return caughtValue; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colName])) return nullValue; else return (int)dr[colName]; } catch { return caughtValue; } }
            }
            if (col is int || col is short || col is long || col is byte)
            {
                int colNumber = (int)col;
                if (dr0 is DataRow)      { DataRow      dr =      (DataRow)dr0;  try { if (Convert.IsDBNull(dr[colNumber])) return nullValue; else return (int)dr[colNumber]; } catch { return caughtValue; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colNumber])) return nullValue; else return (int)dr[colNumber]; } catch { return caughtValue; } }
            }
            return caughtValue;
        }
        [Obsolete("Deprecated in favour of GetColumn.Integer32")]
        public static int N32(object dr0, object col, bool checkColumn = true, int caughtValue = 0, int nullValue = 0)
        { return Integer32(dr0, col, checkColumn, caughtValue, nullValue); }

        /// <summary>
        /// Nullable 32 Bits - int
        /// </summary>
        /// <param name="dr0">DataRow or DbDataReader</param>
        /// <param name="col">string Column Name or int Column Index</param>
        /// <param name="checkColumn">true (default) if column name or index must be checked for existence, false skips column name or index validation</param>
        /// <returns>DBNull return null</returns>
        public static int? Integer32Q(object dr0, object col, bool checkColumn=true)
        {
            if (checkColumn) CheckColumnExists(col, dr0);
            if (col is String)
            {
                string colName = (string)col;
                if (dr0 is DataRow)      { DataRow      dr =      (DataRow)dr0;  try { if (Convert.IsDBNull(dr[colName])) return null; else return (int?)dr[colName]; } catch { return null; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colName])) return null; else return (int?)dr[colName]; } catch { return null; } }
            }
            if (col is int || col is short || col is long || col is byte)
            {
                int colNumber = (int)col;
                if (dr0 is DataRow)      { DataRow      dr =      (DataRow)dr0;  try { if (Convert.IsDBNull(dr[colNumber])) return null; else return (int?)dr[colNumber]; } catch { return null; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colNumber])) return null; else return (int?)dr[colNumber]; } catch { return null; } }
            }
            return null;
        }

        /// <summary>
        /// 64 Bits - long
        /// </summary>
        /// <param name="dr0">DataRow or DbDataReader</param>
        /// <param name="col">string Column Name or int Column Index</param>
        /// <param name="checkColumn">true (default) if column name or index must be checked for existence, false skips column name or index validation</param>
        /// <param name="caughtValue">value returned if try block is caught (default=0)</param>
        /// <param name="nullValue">value returned if DBNull found (default=0)</param>
        /// <returns>cast column value or caughtValue or nullValue</returns>
        public static long Integer64(object dr0, object col, bool checkColumn=true, long caughtValue=0, long nullValue=0)
        {
            if (checkColumn) CheckColumnExists(col, dr0);
            if (col is String)
            {
                string colName = (string)col;
                if (dr0 is DataRow)      { DataRow      dr =      (DataRow)dr0;  try { if (Convert.IsDBNull(dr[colName])) return nullValue; else return (long)dr[colName]; } catch { return caughtValue; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colName])) return nullValue; else return (long)dr[colName]; } catch { return caughtValue; } }
            }
            if (col is int || col is short || col is long || col is byte)
            {
                int colNumber = (int)col;
                if (dr0 is DataRow)      { DataRow      dr =      (DataRow)dr0;  try { if (Convert.IsDBNull(dr[colNumber])) return nullValue; else return (long)dr[colNumber]; } catch { return caughtValue; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colNumber])) return nullValue; else return (long)dr[colNumber]; } catch { return caughtValue; } }
            }
            return caughtValue;
        }

        /// <summary>
        /// Nullable 64 Bits - long
        /// </summary>
        /// <param name="dr0">DataRow or DbDataReader</param>
        /// <param name="col">string Column Name or int Column Index</param>
        /// <param name="checkColumn">true (default) if column name or index must be checked for existence, false skips column name or index validation</param>
        /// <returns>DBNull return null</returns>
        public static long? Integer64Q(object dr0, object col, bool checkColumn=true)
        {
            if (checkColumn) CheckColumnExists(col, dr0);
            if (col is String)
            {
                string colName = (string)col;
                if (dr0 is DataRow)      { DataRow      dr =      (DataRow)dr0;  try { if (Convert.IsDBNull(dr[colName])) return null; else return (long?)dr[colName]; } catch { return null; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colName])) return null; else return (long?)dr[colName]; } catch { return null; } }
            }
            if (col is int || col is short || col is long || col is byte)
            {
                int colNumber = (int)col;
                if (dr0 is DataRow)      { DataRow      dr =      (DataRow)dr0;  try { if (Convert.IsDBNull(dr[colNumber])) return null; else return (long?)dr[colNumber]; } catch { return null; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colNumber])) return null; else return (long?)dr[colNumber]; } catch { return null; } }
            }
            return null;
        }

        /// <summary>
        /// Double
        /// </summary>
        /// <param name="dr0">DataRow or DbDataReader</param>
        /// <param name="col">string Column Name or int Column Index</param>
        /// <param name="checkColumn">true (default) if column name or index must be checked for existence, false skips column name or index validation</param>
        /// <param name="caughtValue">value returned if try block is caught (default=0)</param>
        /// <param name="nullValue">value returned if DBNull found (default=0)</param>
        /// <returns>cast column value or caughtValue or nullValue</returns>
        public static double Double(object dr0, object col, bool checkColumn=true, double caughtValue=0, double nullValue=0)
        {
            if (checkColumn) CheckColumnExists(col, dr0);
            if (col is String)
            {
                string colName = (string)col;
                if (dr0 is DataRow)      { DataRow      dr = (DataRow)dr0;       try { if (Convert.IsDBNull(dr[colName])) return nullValue; else return (double)dr[colName]; } catch { return caughtValue; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colName])) return nullValue; else return (double)dr[colName]; } catch { return caughtValue; } }
            }
            if (col is int || col is short || col is long || col is byte)
            {
                int colNumber = (int)col;
                if (dr0 is DataRow)      { DataRow      dr = (DataRow)dr0;       try { if (Convert.IsDBNull(dr[colNumber])) return nullValue; else return (double)dr[colNumber]; } catch { return caughtValue; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colNumber])) return nullValue; else return (double)dr[colNumber]; } catch { return caughtValue; } }
            }
            return caughtValue;
        }

        /// <summary>
        /// Nullable double
        /// </summary>
        /// <param name="dr0">DataRow or DbDataReader</param>
        /// <param name="col">string Column Name or int Column Index</param>
        /// <param name="checkColumn">true (default) if column name or index must be checked for existence, false skips column name or index validation</param>
        /// <returns>DBNull return null</returns>
        public static double? DoubleQ(object dr0, object col, bool checkColumn=true)
        {
            if (checkColumn) CheckColumnExists(col, dr0);
            if (col is String)
            {
                string colName = (string)col;
                if (dr0 is DataRow)      { DataRow      dr = (DataRow)dr0;       try { if (Convert.IsDBNull(dr[colName])) return null; else return (double?)dr[colName]; } catch { return null; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colName])) return null; else return (double?)dr[colName]; } catch { return null; } }
            }
            if (col is int || col is short || col is long || col is byte)
            {
                int colNumber = (int)col;
                if (dr0 is DataRow)      { DataRow      dr = (DataRow)dr0;       try { if (Convert.IsDBNull(dr[colNumber])) return null; else return (double?)dr[colNumber]; } catch { return null; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colNumber])) return null; else return (double?)dr[colNumber]; } catch { return null; } }
            }
            return null;
        }

        /// <summary>
        /// Float
        /// </summary>
        /// <param name="dr0">DataRow or DbDataReader</param>
        /// <param name="col">string Column Name or int Column Index</param>
        /// <param name="checkColumn">true (default) if column name or index must be checked for existence, false skips column name or index validation</param>
        /// <param name="caughtValue">value returned if try block is caught (default=0)</param>
        /// <param name="nullValue">value returned if DBNull found (default=0)</param>
        /// <returns>cast column value or caughtValue or nullValue</returns>
        public static float Float(object dr0, object col, bool checkColumn=true, float caughtValue=0, float nullValue=0)
        {
            if (checkColumn) CheckColumnExists(col, dr0);
            if (col is String)
            {
                string colName = (string)col;
                if (dr0 is DataRow)      { DataRow      dr = (DataRow)dr0;       try { if (Convert.IsDBNull(dr[colName])) return nullValue; else return (float)dr[colName]; } catch { return caughtValue; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colName])) return nullValue; else return (float)dr[colName]; } catch { return caughtValue; } }
            }
            if (col is int || col is short || col is long || col is byte)
            {
                int colNumber = (int)col;
                if (dr0 is DataRow)      { DataRow      dr = (DataRow)dr0;       try { if (Convert.IsDBNull(dr[colNumber])) return nullValue; else return (float)dr[colNumber]; } catch { return caughtValue; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colNumber])) return nullValue; else return (float)dr[colNumber]; } catch { return caughtValue; } }
            }
            return caughtValue;
        }

        /// <summary>
        /// Nullable float
        /// </summary>
        /// <param name="dr0">DataRow or DbDataReader</param>
        /// <param name="col">string Column Name or int Column Index</param>
        /// <param name="checkColumn">true (default) if column name or index must be checked for existence, false skips column name or index validation</param>
        /// <returns>DBNull return null</returns>
        public static float? FloatQ(object dr0, object col, bool checkColumn=true)
        {
            if (checkColumn) CheckColumnExists(col, dr0);
            if (col is String)
            {
                string colName = (string)col;
                if (dr0 is DataRow)      { DataRow      dr = (DataRow)dr0;       try { if (Convert.IsDBNull(dr[colName])) return null; else return (float?)dr[colName]; } catch { return null; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colName])) return null; else return (float?)dr[colName]; } catch { return null; } }
            }
            if (col is int || col is short || col is long || col is byte)
            {
                int colNumber = (int)col;
                if (dr0 is DataRow)      { DataRow      dr = (DataRow)dr0;       try { if (Convert.IsDBNull(dr[colNumber])) return null; else return (float?)dr[colNumber]; } catch { return null; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colNumber])) return null; else return (float?)dr[colNumber]; } catch { return null; } }
            }
            return null;
        }

        /// <summary>
        /// Decimal
        /// </summary>
        /// <param name="dr0">DataRow or DbDataReader</param>
        /// <param name="col">string Column Name or int Column Index</param>
        /// <param name="checkColumn">true (default) if column name or index must be checked for existence, false skips column name or index validation</param>
        /// <param name="caughtValue">value returned if try block is caught (default=0)</param>
        /// <param name="nullValue">value returned if DBNull found (default=0)</param>
        /// <returns>cast column value or caughtValue or nullValue</returns>
        public static decimal Decimal(object dr0, object col, bool checkColumn=true, decimal caughtValue=0, decimal nullValue=0)
        {
            if (checkColumn) CheckColumnExists(col, dr0);
            if (col is String)
            {
                string colName = (string)col;
                if (dr0 is DataRow)      { DataRow      dr = (DataRow)dr0;       try { if (Convert.IsDBNull(dr[colName])) return nullValue; else return (decimal)dr[colName]; } catch { return caughtValue; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colName])) return nullValue; else return (decimal)dr[colName]; } catch { return caughtValue; } }
            }
            if (col is int || col is short || col is long || col is byte)
            {
                int colNumber = (int)col;
                if (dr0 is DataRow)      { DataRow      dr = (DataRow)dr0;       try { if (Convert.IsDBNull(dr[colNumber])) return nullValue; else return (decimal)dr[colNumber]; } catch { return caughtValue; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colNumber])) return nullValue; else return (decimal)dr[colNumber]; } catch { return caughtValue; } }
            }
            return caughtValue;
        }
        [Obsolete("Deprecated in favour of GetColumn.Decimal")]
        public static decimal Dec(object dr0, object col, bool checkColumn = true, decimal caughtValue = 0, decimal nullValue = 0)
        { return Decimal(dr0, col, checkColumn, caughtValue, nullValue); }

        /// <summary>
        /// Nullable decimal
        /// </summary>
        /// <param name="dr0">DataRow or DbDataReader</param>
        /// <param name="col">string Column Name or int Column Index</param>
        /// <param name="checkColumn">true (default) if column name or index must be checked for existence, false skips column name or index validation</param>
        /// <returns>DBNull return null</returns>
        public static decimal? DecimalQ(object dr0, object col, bool checkColumn=true)
        {
            if (checkColumn) CheckColumnExists(col, dr0);
            if (col is String)
            {
                string colName = (string)col;
                if (dr0 is DataRow)      { DataRow      dr = (DataRow)dr0;       try { if (Convert.IsDBNull(dr[colName])) return null; else return (decimal?)dr[colName]; } catch { return null; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colName])) return null; else return (decimal?)dr[colName]; } catch { return null; } }
            }
            if (col is int || col is short || col is long || col is byte)
            {
                int colNumber = (int)col;
                if (dr0 is DataRow)      { DataRow      dr = (DataRow)dr0;       try { if (Convert.IsDBNull(dr[colNumber])) return null; else return (decimal?)dr[colNumber]; } catch { return null; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colNumber])) return null; else return (decimal?)dr[colNumber]; } catch { return null; } }
            }
            return null;
        }

        /// <summary>
        /// 8 Bits - Byte Array
        /// </summary>
        /// <param name="dr0">DataRow or DbDataReader</param>
        /// <param name="col">string Column Name or int Column Index</param>
        /// <param name="checkColumn">true (default) if column name or index must be checked for existence, false skips column name or index validation</param>
        /// <returns>DBNull return null</returns>
        public static byte[] ByteArray(object dr0, object col, bool checkColumn=true)
        {
            if (checkColumn) CheckColumnExists(col, dr0);
            if (col is String)
            {
                string colName = (string)col;
                if (dr0 is DataRow)      { DataRow      dr =      (DataRow)dr0;  try { if (Convert.IsDBNull(dr[colName])) return null; else return (byte[])dr[colName]; } catch { return null; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colName])) return null; else return (byte[])dr[colName]; } catch { return null; } }
            }
            if (col is int || col is short || col is long || col is byte)
            {
                int colNumber = (int)col;
                if (dr0 is DataRow)      { DataRow      dr =      (DataRow)dr0;  try { if (Convert.IsDBNull(dr[colNumber])) return null; else return (byte[])dr[colNumber]; } catch { return null; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colNumber])) return null; else return (byte[])dr[colNumber]; } catch { return null; } }
            }
            return null;
        }

        #endregion

        #region Date / DateTime / TimeSpan

        /// <summary>
        /// DateTime
        /// </summary>
        /// <param name="dr0">DataRow or DbDataReader</param>
        /// <param name="col">string Column Name or int Column Index</param>
        /// <param name="checkColumn">true (default) if column name or index must be checked for existence, false skips column name or index validation</param>
        /// <param name="caughtValue">value returned if try block is caught (default = 1 Jan 1900)</param>
        /// <param name="nullValue">value returned if DBNull found (default = 1 Jan 1900)</param>
        /// <returns>cast column value or caughtValue or nullValue</returns>
        // http://stackoverflow.com/questions/2894542/c-sharp-4-default-parameter-values-how-to-assign-a-default-datetime-object-valu
        public static DateTime DateTime(object dr0, object col, bool checkColumn=true, DateTime? caughtValue=null, DateTime? nullValue=null)
        {
            if (caughtValue == null) caughtValue = new DateTime(1900, 1, 1);
            if (nullValue == null) nullValue = new DateTime(1900, 1, 1);

            if (checkColumn) CheckColumnExists(col, dr0);
            if (col is String)
            {
                string colName = (string)col;
                if (dr0 is DataRow)      { DataRow      dr = (DataRow)dr0;       try { if (Convert.IsDBNull(dr[colName])) return nullValue.Value; else return (DateTime)dr[colName]; } catch { return caughtValue.Value; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colName])) return nullValue.Value; else return (DateTime)dr[colName]; } catch { return caughtValue.Value; } }
            }
            if (col is int || col is short || col is long || col is byte)
            {
                int colNumber = (int)col;
                if (dr0 is DataRow)      { DataRow      dr = (DataRow)dr0;       try { if (Convert.IsDBNull(dr[colNumber])) return nullValue.Value; else return (DateTime)dr[colNumber]; } catch { return caughtValue.Value; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colNumber])) return nullValue.Value; else return (DateTime)dr[colNumber]; } catch { return caughtValue.Value; } }
            }
            return caughtValue.Value;
        }

        /// <summary>
        /// Nullable DateTime
        /// </summary>
        /// <param name="dr0">DataRow or DbDataReader</param>
        /// <param name="col">Column Name</param>
        /// <param name="checkColumn">true (default) if column name or index must be checked for existence, false skips column name or index validation</param>
        /// <returns>DBNull return null</returns>
        public static DateTime? DateTimeQ(object dr0, string col, bool checkColumn=true)
        {
            if (checkColumn) CheckColumnExists(col, dr0);
            if (dr0 is DataRow)      { DataRow      dr = (DataRow)dr0;       try { if (Convert.IsDBNull(dr[col])) return null; else return (DateTime?)dr[col]; } catch { return null; } }
            if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[col])) return null; else return (DateTime?)dr[col]; } catch { return null; } }
            return null;
        }

        /// <summary>
        /// TimeSpan
        /// </summary>
        /// <param name="dr0">DataRow or DbDataReader</param>
        /// <param name="col">string Column Name or int Column Index</param>
        /// <param name="checkColumn">true (default) if column name or index must be checked for existence, false skips column name or index validation</param>
        /// <param name="caughtValue">value returned if try block is caught (default = 0)</param>
        /// <param name="nullValue">value returned if DBNull found (default = 0)</param>
        /// <returns>cast column value or caughtValue or nullValue</returns>
        // http://stackoverflow.com/questions/2894542/c-sharp-4-default-parameter-values-how-to-assign-a-default-datetime-object-valu
        public static TimeSpan Duration(object dr0, object col, bool checkColumn=true, TimeSpan? caughtValue=null, TimeSpan? nullValue=null)
        {
            if (caughtValue == null) caughtValue = new TimeSpan(0);
            if (nullValue == null) nullValue = new TimeSpan(0);

            if (checkColumn) CheckColumnExists(col, dr0);
            if (col is String)
            {
                string colName = (string)col;
                if (dr0 is DataRow)      { DataRow      dr = (DataRow)dr0;       try { if (Convert.IsDBNull(dr[colName])) return nullValue.Value; else return (TimeSpan)dr[colName]; } catch { return caughtValue.Value; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colName])) return nullValue.Value; else return (TimeSpan)dr[colName]; } catch { return caughtValue.Value; } }
            }
            if (col is int || col is short || col is long || col is byte)
            {
                int colNumber = (int)col;
                if (dr0 is DataRow)      { DataRow      dr = (DataRow)dr0;       try { if (Convert.IsDBNull(dr[colNumber])) return nullValue.Value; else return (TimeSpan)dr[colNumber]; } catch { return caughtValue.Value; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colNumber])) return nullValue.Value; else return (TimeSpan)dr[colNumber]; } catch { return caughtValue.Value; } }
            }
            return caughtValue.Value;
        }

        /// <summary>
        /// Nullable TimeSpan
        /// </summary>
        /// <param name="dr0">DataRow or DbDataReader</param>
        /// <param name="col">Column Name</param>
        /// <param name="checkColumn">true (default) if column name or index must be checked for existence, false skips column name or index validation</param>
        /// <returns>DBNull return null</returns>
        public static TimeSpan? DurationQ(object dr0, string col, bool checkColumn=true)
        {
            if (checkColumn) CheckColumnExists(col, dr0);
            if (dr0 is DataRow)      { DataRow      dr = (DataRow)dr0;       try { if (Convert.IsDBNull(dr[col])) return null; else return (TimeSpan?)dr[col]; } catch { return null; } }
            if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[col])) return null; else return (TimeSpan?)dr[col]; } catch { return null; } }
            return null;
        }

        #endregion

        #region Strings / Guids / Chars

        /// <summary>
        /// GUID
        /// </summary>
        /// <param name="dr0">DataRow or DbDataReader</param>
        /// <param name="col">string Column Name or int Column Index</param>
        /// <param name="checkColumn">true (default) if column name or index must be checked for existence, false skips column name or index validation</param>
        /// <param name="caughtValue">value returned if try block is caught (default = 0)</param>
        /// <param name="nullValue">value returned if DBNull found (default = 0)</param>
        /// <returns>cast column value or caughtValue or nullValue</returns>
        // http://stackoverflow.com/questions/2894542/c-sharp-4-default-parameter-values-how-to-assign-a-default-datetime-object-valu
        public static Guid GUID(object dr0, object col, bool checkColumn=true, Guid? caughtValue=null, Guid? nullValue=null)
        {
            if (caughtValue == null) caughtValue = new Guid();
            if (nullValue == null) nullValue = new Guid();

            if (checkColumn) CheckColumnExists(col, dr0);

            if (col is String)
            {
                string colName = (string)col;
                if (dr0 is DataRow)      { DataRow      dr = (DataRow)dr0;       try { if (Convert.IsDBNull(dr[colName])) return nullValue.Value; else return (Guid)dr[colName]; } catch { return caughtValue.Value; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colName])) return nullValue.Value; else return (Guid)dr[colName]; } catch { return caughtValue.Value; } }
            }
            if (col is int || col is short || col is long || col is byte)
            {
                int colNumber = (int)col;
                if (dr0 is DataRow)      { DataRow      dr = (DataRow)dr0;       try { if (Convert.IsDBNull(dr[colNumber])) return nullValue.Value; else return (Guid)dr[colNumber]; } catch { return caughtValue.Value; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colNumber])) return nullValue.Value; else return (Guid)dr[colNumber]; } catch { return caughtValue.Value; } }
            }
            return caughtValue.Value;
        }

        /// <summary>
        /// GUID
        /// </summary>
        /// <param name="dr0">DataRow or DbDataReader</param>
        /// <param name="col">Column Name</param>
        /// <param name="checkColumn">true (default) if column name or index must be checked for existence, false skips column name or index validation</param>
        /// <returns>DBNull return null</returns>
        public static Guid? GUIDQ(object dr0, string col, bool checkColumn=true)
        {
            if (checkColumn) CheckColumnExists(col, dr0);
            if (dr0 is DataRow)      { DataRow      dr = (DataRow)dr0;       try { if (Convert.IsDBNull(dr[col])) return null; else return (Guid?)dr[col]; } catch { return null; } }
            if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[col])) return null; else return (Guid?)dr[col]; } catch { return null; } }
            return null;
        }

        /// <summary>
        /// char
        /// </summary>
        /// <param name="dr0">DataRow or DbDataReader</param>
        /// <param name="col">string Column Name or int Column Index</param>
        /// <param name="checkColumn">true (default) if column name or index must be checked for existence, false skips column name or index validation</param>
        /// <param name="caughtValue">value returned if try block is caught (default = ' ')</param>
        /// <param name="nullValue">value returned if DBNull found (default = ' ')</param>
        /// <returns>cast column value or caughtValue or nullValue</returns>
        public static char Character(object dr0, object col, bool checkColumn=true, char caughtValue=' ', char nullValue=' ')
        {
            if (checkColumn) CheckColumnExists(col, dr0);

            if (col is String)
            {
                string colName = (string)col;
                if (dr0 is DataRow)      { DataRow      dr = (DataRow)dr0;       try { if (Convert.IsDBNull(dr[colName])) return nullValue; else return (char)dr[colName]; } catch { return caughtValue; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colName])) return nullValue; else return (char)dr[colName]; } catch { return caughtValue; } }
            }
            if (col is int || col is short || col is long || col is byte)
            {
                int colNumber = (int)col;
                if (dr0 is DataRow)      { DataRow      dr = (DataRow)dr0;       try { if (Convert.IsDBNull(dr[colNumber])) return nullValue; else return (char)dr[colNumber]; } catch { return caughtValue; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colNumber])) return nullValue; else return (char)dr[colNumber]; } catch { return caughtValue; } }
            }
            return caughtValue;
        }

        /// <summary>
        /// char
        /// </summary>
        /// <param name="dr0">DataRow or DbDataReader</param>
        /// <param name="col">Column Name</param>
        /// <param name="checkColumn">true (default) if column name or index must be checked for existence, false skips column name or index validation</param>
        /// <returns>DBNull return null</returns>
        public static char? CharacterQ(object dr0, string col, bool checkColumn=true)
        {
            if (checkColumn) CheckColumnExists(col, dr0);
            if (dr0 is DataRow)      { DataRow      dr = (DataRow)dr0;       try { if (Convert.IsDBNull(dr[col])) return null; else return (char?)dr[col]; } catch { return null; } }
            if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[col])) return null; else return (char?)dr[col]; } catch { return null; } }
            return null;
        }

        /// <summary>
        /// Extract given column from dataset row
        /// </summary>
        /// <param name="dr0">DataRow or dbDataReader</param>
        /// <param name="col">string Column Name or int Column Index</param>
        /// <param name="doNotTrim">true if leading and trailing spaces must be preserved, false (default) always trims.</param>
        /// <param name="checkColumn">true (default) if column name or index must be checked for existence, false skips column name or index validation</param>
        /// <param name="caughtValue">value returned if try block is caught (default = ' ')</param>
        /// <param name="nullValue">value returned if DBNull found (default = ' ')</param>
        /// <returns>cast column value or caughtValue or nullValue</returns>
        public static string String(object dr0, object col, bool doNotTrim=false, bool checkColumn=true, string caughtValue="", string nullValue="")
        {
            string result = caughtValue;

            if (checkColumn) CheckColumnExists(col, dr0);

            if (col is String)
            {
                string colName = (string)col;
                if (dr0 is DataRow)      { DataRow      dr = (DataRow)dr0;       try { if (Convert.IsDBNull(dr[colName])) return nullValue; else result=(string)dr[colName]; } catch { return caughtValue; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colName])) return nullValue; else result=(string)dr[colName]; } catch { return caughtValue; } }
            }
            if (col is int || col is short || col is long || col is byte)
            {
                int colNumber = (int)col;
                if (dr0 is DataRow)      { DataRow      dr = (DataRow)dr0;       try { if (Convert.IsDBNull(dr[colNumber])) return nullValue; else result=(string)dr[colNumber]; } catch { return caughtValue; } }
                if (dr0 is DbDataReader) { DbDataReader dr = (DbDataReader)dr0;  try { if (Convert.IsDBNull(dr[colNumber])) return nullValue; else result=(string)dr[colNumber]; } catch { return caughtValue; } }
            }

            if (!doNotTrim) result = result.Trim();
            return result.Replace("&#34;", @"""").Replace("&#59;", ";").Replace("&#58;", ":");
        }
        [Obsolete("Deprecated in favour of GetColumn.String")]
        public static string Str(object dr0, object col, bool doNotTrim=false, bool checkColumn=true, string caughtValue="", string nullValue="")
        { return String(dr0, col, doNotTrim, checkColumn, caughtValue, nullValue); }

        #endregion

        #region Helper Functions

        public static string GetColumnType(DataRow dr, string columnName)
        {
            if (dr == null) return "! Null Row";
            if (columnName == null) return "! Null ColumnName";
            if (columnName.Length == 0) return "! Empty ColumnName";
            if (!dr.Table.Columns.Contains(columnName)) return "! No such column";

            object x = dr[columnName];
            if (x is DBNull) return "DBNull";
            if (x is String) return "String";
            if (x is int) return "Int";
            if (x is float) return "Float";
            if (x is double) return "Double";
            if (x is DateTime) return "DateTime";

            return x.GetType().ToString();
        }

        private static void CheckColumnExists(object col, object dataSource)
        {
            int checkColNo = int.MinValue;

            string checkCol = "!";
            if (col is int)
            {   checkColNo = (int)col;
                checkCol = "#" + checkColNo.ToString();
                if (checkColNo<0) throw new ArgumentException("Requested column number cannot be negative.");
            }
            if (col is string) checkCol = ((string)col ?? "").ToUpper();
            if (checkCol == "!")
                throw new ArgumentException("Requested column is neither a string name nor an integer number.");

            if (dataSource == null)
                throw new ArgumentException("Provided DataRow or DataReader is null when checking for Column [" + checkCol + "].");

            string cols = "";
            string delim = "";

            if (dataSource is DataRow)
            {
                DataTable dt = ((DataRow)dataSource).Table;

                if (checkColNo != int.MinValue)
                {   if (checkColNo<dt.Columns.Count) return;
                }
                else
                {   if (dt.Columns.Contains(checkCol)) return;
                }

                foreach (DataColumn dc in dt.Columns)
                {
                    cols += delim + dc.ColumnName;
                    delim = ", ";
                }
                throw new ArgumentException("Column [" + checkCol + "] not found. There are " + dt.Columns.Count + " available columns named [" + cols + "]");
            }

            if (dataSource is DbDataReader)
            {
                DataTable xT = ((DbDataReader)dataSource).GetSchemaTable();

                if (checkColNo != int.MinValue)
                {
                    if (checkColNo<xT.Rows.Count) return;
                }

                foreach (DataRow xR in xT.Rows)
                {
                    string colName = (string)xR["ColumnName"];
                    if (colName.ToUpper() == checkCol) return;           // allow #col to loop thru here - will not match anything, but will generate our error message
                    cols += delim + colName;
                    delim = ", ";
                }
                throw new ArgumentException("Column [" + checkCol + "] not found. There are " + xT.Rows.Count + " available columns named [" + cols + "]");
            }
            throw new ArgumentException("DataSource provided to getColumn() is neither a DataRow nor a DbDataReader");
        }

        /// <summary>
        /// Check DataRow contains the given column
        /// </summary>
        /// <param name="dr">DataRow to test</param>
        /// <param name="columnName">Name of column</param>
        /// <returns>true if column is found, false if either parameter is null or columnName is empty</returns>
        public static bool Exists(DataRow dr, string columnName)
        {
            if (dr == null) return false;
            if (columnName == null) return false;
            if (columnName.Length == 0) return false;
            return (dr.Table.Columns.Contains(columnName));
        }
        /// <summary>
        /// Check DataReader contains the given column
        /// </summary>
        /// <param name="dr">DataReader to test</param>
        /// <param name="columnName">Name of column</param>
        /// <returns>true if column is found, false if either parameter is null or columnName is empty</returns>
        public static bool Exists(DbDataReader dr, string columnName)
        {
            if (dr == null) return false;
            if (columnName == null) return false;
            if (columnName.Length == 0) return false;
            return (dr.GetSchemaTable().Columns.Contains(columnName));
        }

        #endregion

    }

    #endregion

}
