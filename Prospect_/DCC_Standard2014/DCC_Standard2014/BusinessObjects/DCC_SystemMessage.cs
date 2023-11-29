using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace DCC_Standard
{

    #region DCC_SystemMessage

    [Serializable]
    public class DCC_SystemMessage
    {


        // Database Fields

        int       k_SystemMessage = -1;
        int?      q_SystemUser;
        int       messageType;
        string    messageText;                       // 400 wide
        DateTime? expiryDate;
        bool?     seen;
        string    logonID;
        string    accessLevel;



        // Get/Set Properties

        public int       K_SystemMessage  { get { return k_SystemMessage; } set { k_SystemMessage = value; } }
        public int?      Q_SystemUser     { get { return q_SystemUser   ; } set { q_SystemUser    = value; } }
        public int       MessageType      { get { return messageType    ; } set { messageType     = value; } }
        public string    MessageText      { get { return messageText    ; } set { messageText     = value; } }
        public DateTime? ExpiryDate       { get { return expiryDate     ; } set { expiryDate      = value; } }
        public bool?     Seen             { get { return seen           ; } set { seen            = value; } }
        public string    LogonID          { get { return logonID        ; } set { logonID         = value; } }
        public string    AccessLevel      { get { return accessLevel    ; } set { accessLevel     = value; } }



        // Display Properties

        public string MessageTypeText     { get { return messageType.ToString();}}
        public string MessageTextText     { get { return messageText;}}
        public string ExpiryDateText      { get { return DCC_Formatting.NullDateTxt(expiryDate);}}
        public string ExpiryDateSort      { get { return DCC_Formatting.NullDateTxt(expiryDate,"yyyyMMdd");}}
        public string SeenText            { get { if (!seen.HasValue) return "?"; return (seen.Value ? "Yes":"No");}}



        // Utilities

        public int PrimaryKey             { get { return K_SystemMessage; } set { K_SystemMessage=value; } }
        public string Key                 { get { return PrimaryKey.ToString(); } }
        public bool IsNew                 { get { return PrimaryKey == -1; } }
        public override string ToString() { return "Key: " + Key + "  " + MessageTextText + "  " + ExpiryDateText + "  " + SeenText; }



        // DropDownLists

        public class DCC_SystemUserItem { public int Id { get; set; } public string Name { get; set; } }
        public List<DCC_SystemUserItem> SystemUserItems;
        public int SelectedSystemUserId { get; set; }



        // Constructors

        public DCC_SystemMessage()
        {
        }

        public DCC_SystemMessage(DataRow dr)
        {
            GetColumns(dr);
        }

        public DCC_SystemMessage(DCC_Session session, int k_SystemMessage)
        {
            // Set defaults when new
            if (k_SystemMessage == -1)
            {
            }

            // Get values from DB if not new
            else
            {
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("sSystemMessage");
                dbf.AddTable("SystemMessage");
                dbf.AddParameter("K_SystemMessage", k_SystemMessage);
                session.AutoDatabaseConnection.DataLayer.SelectData(dbf);
                if (dbf.hasTables && dbf.TableZeroRows.Count == 1) GetColumns(dbf.RowZero);
            }

            PopulateDropDownLists(session.AutoDatabaseConnection);
        }



        // Methods

        public bool GetColumns(DataRow dr)
        {
            if (dr == null) return false;

            k_SystemMessage = DCC_GetColumn.Integer32 (dr, "k_SystemMessage");
            q_SystemUser    = DCC_GetColumn.Integer32Q(dr, "q_SystemUser");
            messageType     = DCC_GetColumn.Integer32 (dr, "messageType");
            messageText     = DCC_GetColumn.String    (dr, "messageText");
            expiryDate      = DCC_GetColumn.DateTimeQ (dr, "expiryDate");
            seen            = DCC_GetColumn.BitQ      (dr, "seen");

            if (DCC_GetColumn.Exists(dr, "LogonID")) logonID = DCC_GetColumn.String(dr, "LogonID");
            if (DCC_GetColumn.Exists(dr, "AccessLevel")) accessLevel = DCC_GetColumn.String(dr, "AccessLevel");

            return true;
        }

        public int Insert(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("iSystemMessage");
            dbf.AddTable("SystemMessage");
            SetCommonParameters(dbf);
            dbf.AddParameter("_PRIMARYKEY", DCC_DatabaseFrame.OUTINTEGER);
            int newKey = db.DataLayer.NonQueryPK(dbf);
            if (newKey > -1) PrimaryKey = newKey;
            return newKey;
        }

        public bool Update(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uSystemMessage");
            dbf.AddTable("SystemMessage");
            dbf.AddParameter("K_SystemMessage", PrimaryKey);
            SetCommonParameters(dbf);
            db.DataLayer.NonQuery(dbf);
            return dbf.OK;
        }

        public bool Delete(DCC_Database db)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("dSystemMessage");
            dbf.AddTable("SystemMessage");
            dbf.AddParameter("K_SystemMessage", PrimaryKey);
            db.DataLayer.NonQuery(dbf);
            return dbf.OK;
        }

        private void SetCommonParameters(DCC_DatabaseFrame dbf)
        {
            dbf.AddParameter("Q_SystemUser", q_SystemUser    );
            dbf.AddParameter("MessageType" , messageType     );
            dbf.AddParameter("MessageText" , messageText, 400);
            dbf.AddParameter("ExpiryDate"  , expiryDate      );
            dbf.AddParameter("Seen"        , seen            );
        }
        
        public void PopulateDropDownLists(DCC_Database db)
        {

            SystemUserItems = new List<DCC_SystemUserItem>();
            DCC_SystemUserList systemUserList = new DCC_SystemUserList(db);
            foreach (DCC_SystemUser su in systemUserList)
            {
                DCC_SystemUserItem i = new DCC_SystemUserItem();
                i.Id = su.K_SystemUser;
                i.Name = su.LogonID;
                SystemUserItems.Add(i);
            }

        }

    }

    #endregion


    #region DCC_SystemMessageList

    [Serializable]
    public class DCC_SystemMessageList : IEnumerable<DCC_SystemMessage>
    {


        // Database Fields

        public List<object> All = new List<object>();
        public SortedList<string, List<object>> DynamicSortBy = new SortedList<string, List<object>>();



        // Properties

        public int Count { get { return All.Count; } }



        // Utilities

        public override string ToString() { return "SystemMessageList has " + Count + " items"; }



        // Constructors

        public DCC_SystemMessageList()
        {
        }

        public DCC_SystemMessageList(DCC_Database db, int k_SystemUser = -1)
        {
            DCC_DatabaseFrame dbf;
            if (k_SystemUser == -1)
                dbf = new DCC_DatabaseFrame("sSystemMessageList");
            else
            {
                dbf = new DCC_DatabaseFrame("sSystemMessageListBySystemUser");
                dbf.AddParameter("Q_SystemUser", k_SystemUser);
            }
            dbf.AddTable("SystemMessage");
            db.DataLayer.SelectData(dbf);
            if (dbf.hasTables)
            {
                foreach (DataRow dr in dbf.TableZeroRows) Add(new DCC_SystemMessage(dr));
            }
        }
        


        // Methods

        public bool Add(DCC_SystemMessage item)
        {
            All.Add(item);
            return true;
        }



        // Enumerator

        public IEnumerator<DCC_SystemMessage> GetEnumerator() { foreach (object o in All) yield return (DCC_SystemMessage)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }

    }

    #endregion
    

    #region DCC_SystemMessageGrid

    public class DCC_SystemMessageGrid : DCC_Grid
    {
        
        public DCC_SystemMessageGrid(DCC_Session session)
        {
            SetItemName("System Message", "System Messages");
            AddTextColumn("A1", "LogonID",        "To",           width: 150);
            AddTextColumn("A2", "MessageText",    "Message Text", width: 300);
            AddDateColumn("A3", "ExpiryDateText", "Expiry Date",  width:  80, boundSortFieldName: "ExpiryDateSort");
            AddTextColumn("A4", "SeenText",       "Seen",         width:  40);
            DCC_SystemMessageList messageList = new DCC_SystemMessageList(session.AutoDatabaseConnection);
            TotalRows = messageList.All;
            Count = CountFiltered = TotalRows.Count;
        }

    }

    #endregion

}
