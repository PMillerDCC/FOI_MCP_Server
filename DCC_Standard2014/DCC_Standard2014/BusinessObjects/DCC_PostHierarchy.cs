using System;
using System.Data;

namespace DCC_Standard
{


    [Serializable]

    public class DCC_PostHierarchy
    {

        // Properties

        public string PostReference;
        public string PostNo;
        public string LevelNo;
        public string PersonReference;
        public string EmployeeNo;
        public string Level01;
        public string Level01PostReference;
        public string Level01PostNo;
        public string Level01PostHolder;
        public string Level02;
        public string Level02PostReference;
        public string Level02PostNo;
        public string Level02PostHolder;
        public string Level03;
        public string Level03PostReference;
        public string Level03PostNo;
        public string Level03PostHolder;
        public string Level04;
        public string Level04PostReference;
        public string Level04PostNo;
        public string Level04PostHolder;
        public string Level05;
        public string Level05PostReference;
        public string Level05PostNo;
        public string Level05PostHolder;
        public string Level06;
        public string Level06PostReference;
        public string Level06PostNo;
        public string Level06PostHolder;
        public string Level07;
        public string Level07PostReference;
        public string Level07PostNo;
        public string Level07PostHolder;
        public string Level08;
        public string Level08PostReference;
        public string Level08PostNo;
        public string Level08PostHolder;
        public string Level09;
        public string Level09PostReference;
        public string Level09PostNo;
        public string Level09PostHolder;
        public string Level10;
        public string Level10PostReference;
        public string Level10PostNo;
        public string Level10PostHolder;
        public string Level11;
        public string Level11PostReference;
        public string Level11PostNo;
        public string Level11PostHolder;
        public string Level12;
        public string Level12PostReference;
        public string Level12PostNo;
        public string Level12PostHolder;



        // Utilities

        public override string ToString() { return EmployeeNo + " " + PostNo; }



        // Constructors

        public DCC_PostHierarchy()
        {
        }

        public DCC_PostHierarchy(DataRow dr)
        {
            Load(dr);
        }

        public DCC_PostHierarchy(DCC_Session session, string pPostNo = "")
        {
            //if (session.DatabaseConnections.Contains("SALP"))
            //{
            //    DCC_Database db = new DCC_Database("ORACLE", "SALP", session.ApplicationTitle);

            if (session.ResourceLinkDatabaseConnection != null)
            {
                DCC_Database db = session.ResourceLinkDatabaseConnection;
                DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("**select * from dcc_mv_rrs_p2p_occ where post_number = :pPostNo");
                dbf.AddTable("DCC_MV_RRS_P2P_OCC");
                dbf.AddParameter("pPostNo", pPostNo, 10);
                db.DataLayer.SelectData(dbf);
                if (dbf.hasTables) foreach (DataRow dr in dbf.TableZeroRows) Load(dr);
            }
        }



        // Methods

        private void Load(DataRow dr)
        {
            PostReference        = DCC_GetColumn.String(dr, "POST_REF");
            PostNo               = DCC_GetColumn.String(dr, "POST_NUMBER");
            LevelNo              = DCC_GetColumn.String(dr, "LEVEL_NO");
            PersonReference      = DCC_GetColumn.String(dr, "PERSON_REF");
            EmployeeNo           = DCC_GetColumn.String(dr, "EMPLOYEE_NUMBER");
            Level01              = DCC_GetColumn.String(dr, "LEVEL_1");
            Level01PostReference = DCC_GetColumn.String(dr, "LEVEL_1_POST_REF");
            Level01PostNo        = DCC_GetColumn.String(dr, "LEVEL_1_POST_NUMBER");
            Level01PostHolder    = DCC_GetColumn.String(dr, "P2P_LEVEL_1");
            Level02              = DCC_GetColumn.String(dr, "LEVEL_2");
            Level02PostReference = DCC_GetColumn.String(dr, "LEVEL_2_POST_REF");
            Level02PostNo        = DCC_GetColumn.String(dr, "LEVEL_2_POST_NUMBER");
            Level02PostHolder    = DCC_GetColumn.String(dr, "P2P_LEVEL_2");
            Level03              = DCC_GetColumn.String(dr, "LEVEL_3");
            Level03PostReference = DCC_GetColumn.String(dr, "LEVEL_3_POST_REF");
            Level03PostNo        = DCC_GetColumn.String(dr, "LEVEL_3_POST_NUMBER");
            Level03PostHolder    = DCC_GetColumn.String(dr, "P2P_LEVEL_3");
            Level04              = DCC_GetColumn.String(dr, "LEVEL_4");
            Level04PostReference = DCC_GetColumn.String(dr, "LEVEL_4_POST_REF");
            Level04PostNo        = DCC_GetColumn.String(dr, "LEVEL_4_POST_NUMBER");
            Level04PostHolder    = DCC_GetColumn.String(dr, "P2P_LEVEL_4");
            Level05              = DCC_GetColumn.String(dr, "LEVEL_5");
            Level05PostReference = DCC_GetColumn.String(dr, "LEVEL_5_POST_REF");
            Level05PostNo        = DCC_GetColumn.String(dr, "LEVEL_5_POST_NUMBER");
            Level05PostHolder    = DCC_GetColumn.String(dr, "P2P_LEVEL_5");
            Level06              = DCC_GetColumn.String(dr, "LEVEL_6");
            Level06PostReference = DCC_GetColumn.String(dr, "LEVEL_6_POST_REF");
            Level06PostNo        = DCC_GetColumn.String(dr, "LEVEL_6_POST_NUMBER");
            Level06PostHolder    = DCC_GetColumn.String(dr, "P2P_LEVEL_6");
            Level07              = DCC_GetColumn.String(dr, "LEVEL_7");
            Level07PostReference = DCC_GetColumn.String(dr, "LEVEL_7_POST_REF");
            Level07PostNo        = DCC_GetColumn.String(dr, "LEVEL_7_POST_NUMBER");
            Level07PostHolder    = DCC_GetColumn.String(dr, "P2P_LEVEL_7");
            Level08              = DCC_GetColumn.String(dr, "LEVEL_8");
            Level08PostReference = DCC_GetColumn.String(dr, "LEVEL_8_POST_REF");
            Level08PostNo        = DCC_GetColumn.String(dr, "LEVEL_8_POST_NUMBER");
            Level08PostHolder    = DCC_GetColumn.String(dr, "P2P_LEVEL_8");
            Level09              = DCC_GetColumn.String(dr, "LEVEL_9");
            Level09PostReference = DCC_GetColumn.String(dr, "LEVEL_9_POST_REF");
            Level09PostNo        = DCC_GetColumn.String(dr, "LEVEL_9_POST_NUMBER");
            Level09PostHolder    = DCC_GetColumn.String(dr, "P2P_LEVEL_9");
            Level10              = DCC_GetColumn.String(dr, "LEVEL_10");
            Level10PostReference = DCC_GetColumn.String(dr, "LEVEL_10_POST_REF");
            Level10PostNo        = DCC_GetColumn.String(dr, "LEVEL_10_POST_NUMBER");
            Level10PostHolder    = DCC_GetColumn.String(dr, "P2P_LEVEL_10");
            Level11              = DCC_GetColumn.String(dr, "LEVEL_11");
            Level11PostReference = DCC_GetColumn.String(dr, "LEVEL_11_POST_REF");
            Level11PostNo        = DCC_GetColumn.String(dr, "LEVEL_11_POST_NUMBER");
            Level11PostHolder    = DCC_GetColumn.String(dr, "P2P_LEVEL_11");
            Level12              = DCC_GetColumn.String(dr, "LEVEL_12");
            Level12PostReference = DCC_GetColumn.String(dr, "LEVEL_12_POST_REF");
            Level12PostNo        = DCC_GetColumn.String(dr, "LEVEL_12_POST_NUMBER");
            Level12PostHolder    = DCC_GetColumn.String(dr, "P2P_LEVEL_12");
        }

    }



}
