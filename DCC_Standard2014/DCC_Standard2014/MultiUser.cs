using System;
using System.Data;
using System.Web;

namespace DCC_Standard
{
    public static class MultiUser
    {

        public struct LockInfo
        {
            public bool isLock;
            public bool isMyLock;
            public string objectGroup;
            public string identity;
            public string user;
            public DateTime? stamp;
            public string session;
        }
        
        public static string EngageLock(DCC_Database db, string objectGrouping, string identity)
        {
            string username = HttpContext.Current.Request.ServerVariables["LOGON_USER"] ?? "-----";
            string sessionID = HttpContext.Current.Session.SessionID;
            DCC_DatabaseFrame dif = new DCC_DatabaseFrame("_LockSet");
            dif.AddParameter("Object", objectGrouping, 0);
            dif.AddParameter("Key", identity, 0);
            dif.AddParameter("Stamp", DateTime.Now);
            dif.AddParameter("UserName", username, 0);
            dif.AddParameter("StampTooOld", DateTime.Now.AddMinutes(-20));
            dif.AddParameter("Session", sessionID, 0);
            db.DataLayer.SelectData(dif);
            if (dif.DataSelected.Tables.Count == 2)
            {
                if (dif.TableZeroRows.Count == 0 && dif.DataSelected.Tables[1].Rows.Count == 1) return "";
                if (dif.TableZeroRows.Count == 1)
                {
                    string lockingSession = DCC_GetColumn.String(dif.TableZero.Rows[0], "_Session");
                    if (lockingSession == sessionID) return "";
                    return DCC_GetColumn.String(dif.TableZero.Rows[0], "_User");
                }
            }
            return "(--error--)";
        }
 
        public static LockInfo GetLock(DCC_Database db, string objectGrouping, string identity)
        {
            LockInfo li = new LockInfo();

            DCC_DatabaseFrame dif = new DCC_DatabaseFrame("_LockCheck");
            dif.AddParameter("Object", objectGrouping, 0);
            dif.AddParameter("Key", identity, 0);
            db.DataLayer.SelectData(dif);
            if (dif.DataSelected.Tables.Count == 1)
            {
                if (dif.TableZeroRows.Count == 0) return li;
                string sessionID = HttpContext.Current.Session.SessionID;
                foreach (DataRow dr in dif.TableZeroRows)
                {
                    li.isLock = true;
                    li.identity = identity;
                    li.objectGroup = objectGrouping;
                    li.session = DCC_GetColumn.String(dr, "_session");
                    li.stamp = DCC_GetColumn.DateTimeQ(dr, "_lock_Stamp");
                    li.user = DCC_GetColumn.String(dr, "_user");
                    if (li.session == sessionID)
                    {
                        li.isMyLock = true;
                    }
                    return li;
                }
            }
            return li;
        }

        public static string ReleaseLock(DCC_Database db, string objectGrouping, string identity)
        {
            DCC_DatabaseFrame dif = new DCC_DatabaseFrame("_LockClear");
            dif.AddParameter("Object", objectGrouping, 0);
            dif.AddParameter("Key", identity, 0);
            dif.AddParameter("Session", HttpContext.Current.Session.SessionID, 0);
            db.DataLayer.SelectData(dif);
            if (dif.DataSelected.Tables.Count == 1)
            {
                if (dif.TableZero.Rows.Count == 0) return "";
            }
            return "(--error--)";
        }
    }

}
