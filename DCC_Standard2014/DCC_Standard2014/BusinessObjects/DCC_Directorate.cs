using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace DCC_Standard
{


    [Serializable]
    public class DCC_Directorate
    {

        // Properties

        public string Service;
        public string Division;
        public string Section;
        public string Team;
        public string HierarchyLevel6;
        public string HierarchyLevel7;
        public string HierarchyLevel8;
        public string HierarchyLevel9;
        public string HierarchyLevel2Id;
        public string HierarchyLevel3Id;
        public string HierarchyLevel4Id;
        public string HierarchyLevel5Id;
        public string HierarchyLevel6Id;
        public string HierarchyLevel7Id;
        public string HierarchyLevel8Id;
        public string HierarchyLevel9Id;
        public string LowestStructureUnitNo;



        // Display Properties

		public string DirectorateText     { get { return ( string.IsNullOrWhiteSpace(Service) ? "" : " / " + Service) + ( string.IsNullOrWhiteSpace(Division) ? "" : " / " + Division) + ( string.IsNullOrWhiteSpace(Section) ? "" : " / " + Section) + ( string.IsNullOrWhiteSpace(Team) ? "" : " / " + Team); } }
		public string HierarchyLevel2Text { get { return ( Service         + " (" + HierarchyLevel2Id + ")" ); } }
		public string HierarchyLevel3Text { get { return ( Division        + " (" + HierarchyLevel3Id + ")" ); } }
		public string HierarchyLevel4Text { get { return ( Section         + " (" + HierarchyLevel4Id + ")" ); } }
		public string HierarchyLevel5Text { get { return ( Team            + " (" + HierarchyLevel5Id + ")" ); } }
		public string HierarchyLevel6Text { get { return ( HierarchyLevel6 + " (" + HierarchyLevel6Id + ")" ); } }
		public string HierarchyLevel7Text { get { return ( HierarchyLevel7 + " (" + HierarchyLevel7Id + ")" ); } }
		public string HierarchyLevel8Text { get { return ( HierarchyLevel8 + " (" + HierarchyLevel8Id + ")" ); } }
		public string HierarchyLevel9Text { get { return ( HierarchyLevel9 + " (" + HierarchyLevel9Id + ")" ); } }



        // Utilities

        public override string ToString() { return DirectorateText; }



        // Constructors

        public DCC_Directorate()
        {
        }

        public DCC_Directorate(DataRow dr)
        {
            Load(dr);
        }

        public DCC_Directorate(string service, string division, string section, string team, string h6, string h7, string h8, string h9, DCC_Session session = null)
        {
            Service = service;
            Division = division;
            Section = section;
            Team = team;
            HierarchyLevel6 = h6;
            HierarchyLevel7 = h7;
            HierarchyLevel8 = h8;
            HierarchyLevel9 = h9;

            if (session != null)
            {
                DCC_Database db = session.ResourceLinkDatabaseConnection;
                DCC_DatabaseFrame dbf;

                if (!string.IsNullOrEmpty(HierarchyLevel9))
                {
                    dbf = new DCC_DatabaseFrame(@"**
                        select distinct
                            hierarchy_level_9,
                            hierarchy_id_9
                        from
                            dcc_v_myworkforce_hier
                        where
                            hierarchy_level_9 = :pHierarchyLevel9
                        order by
                            hierarchy_level_9
                        ");
                    dbf.AddParameter("pHierarchyLevel9", HierarchyLevel9, 100);
                    dbf.AddTable("dcc_v_myworkforce_hier");
                    db.DataLayer.SelectData(dbf);
                    if (dbf.hasTables)
                    {
                        foreach (DataRow dr in dbf.TableZeroRows)
                        {
                            LowestStructureUnitNo = DCC_GetColumn.String(dr, "HIERARCHY_ID_9");
                        }
                    }
                }

                if (!string.IsNullOrEmpty(HierarchyLevel8))
                {
                    dbf = new DCC_DatabaseFrame(@"**
                        select distinct
                            hierarchy_level_8,
                            hierarchy_id_8
                        from
                            dcc_v_myworkforce_hier
                        where
                            hierarchy_level_8 = :pHierarchyLevel8
                        order by
                            hierarchy_level_8
                        ");
                    dbf.AddParameter("pHierarchyLevel8", HierarchyLevel8, 100);
                    dbf.AddTable("dcc_v_myworkforce_hier");
                    db.DataLayer.SelectData(dbf);
                    if (dbf.hasTables)
                    {
                        foreach (DataRow dr in dbf.TableZeroRows)
                        {
                            LowestStructureUnitNo = DCC_GetColumn.String(dr, "HIERARCHY_ID_8");
                        }
                    }
                }

                if (!string.IsNullOrEmpty(HierarchyLevel7))
                {
                    dbf = new DCC_DatabaseFrame(@"**
                        select distinct
                            hierarchy_level_7,
                            hierarchy_id_7
                        from
                            dcc_v_myworkforce_hier
                        where
                            hierarchy_level_7 = :pHierarchyLevel7
                        order by
                            hierarchy_level_7
                        ");
                    dbf.AddParameter("pHierarchyLevel7", HierarchyLevel7, 100);
                    dbf.AddTable("dcc_v_myworkforce_hier");
                    db.DataLayer.SelectData(dbf);
                    if (dbf.hasTables)
                    {
                        foreach (DataRow dr in dbf.TableZeroRows)
                        {
                            LowestStructureUnitNo = DCC_GetColumn.String(dr, "HIERARCHY_ID_7");
                        }
                    }
                }

                if (!string.IsNullOrEmpty(HierarchyLevel6))
                {
                    dbf = new DCC_DatabaseFrame(@"**
                        select distinct
                            hierarchy_level_6,
                            hierarchy_id_6
                        from
                            dcc_v_myworkforce_hier
                        where
                            hierarchy_level_6 = :pHierarchyLevel6
                        order by
                            hierarchy_level_6
                        ");
                    dbf.AddParameter("pHierarchyLevel6", HierarchyLevel6, 100);
                    dbf.AddTable("dcc_v_myworkforce_hier");
                    db.DataLayer.SelectData(dbf);
                    if (dbf.hasTables)
                    {
                        foreach (DataRow dr in dbf.TableZeroRows)
                        {
                            LowestStructureUnitNo = DCC_GetColumn.String(dr, "HIERARCHY_ID_6");
                        }
                    }
                }

                if (!string.IsNullOrEmpty(Team))
                {
                    dbf = new DCC_DatabaseFrame(@"**
                        select distinct
                            hierarchy_level_5,
                            hierarchy_id_5
                        from
                            dcc_v_myworkforce_hier
                        where
                            hierarchy_level_5 = :pHierarchyLevel5
                        order by
                            hierarchy_level_5
                        ");
                    dbf.AddParameter("pHierarchyLevel5", Team, 100);
                    dbf.AddTable("dcc_v_myworkforce_hier");
                    db.DataLayer.SelectData(dbf);
                    if (dbf.hasTables)
                    {
                        foreach (DataRow dr in dbf.TableZeroRows)
                        {
                            LowestStructureUnitNo = DCC_GetColumn.String(dr, "HIERARCHY_ID_5");
                        }
                    }
                }

                if (!string.IsNullOrEmpty(Section))
                {
                    dbf = new DCC_DatabaseFrame(@"**
                        select distinct
                            hierarchy_level_4,
                            hierarchy_id_4
                        from
                            dcc_v_myworkforce_hier
                        where
                            hierarchy_level_4 = :pHierarchyLevel4
                        order by
                            hierarchy_level_4
                        ");
                    dbf.AddParameter("pHierarchyLevel4", Section, 100);
                    dbf.AddTable("dcc_v_myworkforce_hier");
                    db.DataLayer.SelectData(dbf);
                    if (dbf.hasTables)
                    {
                        foreach (DataRow dr in dbf.TableZeroRows)
                        {
                            LowestStructureUnitNo = DCC_GetColumn.String(dr, "HIERARCHY_ID_4");
                        }
                    }
                }

                if (!string.IsNullOrEmpty(Division))
                {
                    dbf = new DCC_DatabaseFrame(@"**
                        select distinct
                            hierarchy_level_3,
                            hierarchy_id_3
                        from
                            dcc_v_myworkforce_hier
                        where
                            hierarchy_level_3 = :pHierarchyLevel3
                        order by
                            hierarchy_level_3
                        ");
                    dbf.AddParameter("pHierarchyLevel3", Division, 100);
                    dbf.AddTable("dcc_v_myworkforce_hier");
                    db.DataLayer.SelectData(dbf);
                    if (dbf.hasTables)
                    {
                        foreach (DataRow dr in dbf.TableZeroRows)
                        {
                            LowestStructureUnitNo = DCC_GetColumn.String(dr, "HIERARCHY_ID_3");
                        }
                    }
                }

                else if (!string.IsNullOrEmpty(Service))
                {
                    dbf = new DCC_DatabaseFrame(@"**
                        select distinct
                            hierarchy_level_2,
                            hierarchy_id_2
                        from
                            dcc_v_myworkforce_hier
                        where
                            hierarchy_level_2 = :pHierarchyLevel2
                        order by
                            hierarchy_level_2
                        ");
                    dbf.AddParameter("pHierarchyLevel2", Service, 100);
                    dbf.AddTable("dcc_v_myworkforce_hier");
                    db.DataLayer.SelectData(dbf);
                    if (dbf.hasTables)
                    {
                        foreach (DataRow dr in dbf.TableZeroRows)
                        {
                            LowestStructureUnitNo = DCC_GetColumn.String(dr, "HIERARCHY_ID_2");
                        }
                    }
                }

            }

        }

        private void Load(DataRow dr)
        {
            if (DCC_GetColumn.Exists(dr, "HIERARCHY_LEVEL_2")) Service                      = DCC_GetColumn.String   (dr, "HIERARCHY_LEVEL_2");
            if (DCC_GetColumn.Exists(dr, "HIERARCHY_ID_2"))    HierarchyLevel2Id            = DCC_GetColumn.String   (dr, "HIERARCHY_ID_2");
            if (DCC_GetColumn.Exists(dr, "HIERARCHY_LEVEL_3")) Division                     = DCC_GetColumn.String   (dr, "HIERARCHY_LEVEL_3");
            if (DCC_GetColumn.Exists(dr, "HIERARCHY_ID_3"))    HierarchyLevel3Id            = DCC_GetColumn.String   (dr, "HIERARCHY_ID_3");
            if (DCC_GetColumn.Exists(dr, "HIERARCHY_LEVEL_4")) Section                      = DCC_GetColumn.String   (dr, "HIERARCHY_LEVEL_4");
            if (DCC_GetColumn.Exists(dr, "HIERARCHY_ID_4"))    HierarchyLevel4Id            = DCC_GetColumn.String   (dr, "HIERARCHY_ID_4");
            if (DCC_GetColumn.Exists(dr, "HIERARCHY_LEVEL_5")) Team                         = DCC_GetColumn.String   (dr, "HIERARCHY_LEVEL_5");
            if (DCC_GetColumn.Exists(dr, "HIERARCHY_ID_5"))    HierarchyLevel5Id            = DCC_GetColumn.String   (dr, "HIERARCHY_ID_5");
        }

    }



    [Serializable]
    public class DCC_DirectorateList : IEnumerable<DCC_Directorate>
    {

        // Properties

        public List<object> ServiceList = new List<object>();
        public List<object> DivisionList = new List<object>();
        public List<object> SectionList = new List<object>();
        public List<object> TeamList = new List<object>();
        public List<object> H6List = new List<object>();
        public List<object> H7List = new List<object>();
        public List<object> H8List = new List<object>();
        public List<object> H9List = new List<object>();



        // Utilities

        public int ServiceCount { get { return ServiceList.Count; } }
        public int DivisionCount { get { return DivisionList.Count; } }
        public int SectionCount { get { return SectionList.Count; } }
        public int TeamCount { get { return TeamList.Count; } }
        public int H6Count { get { return H6List.Count; } }
        public int H7Count { get { return H7List.Count; } }
        public int H8Count { get { return H8List.Count; } }
        public int H9Count { get { return H9List.Count; } }
        public override string ToString() { return "DirectorateList has " + ServiceCount + " services; " + DivisionCount + " divisions; " + SectionCount + " sections; " + TeamCount + " teams;" + H6Count + " level 6s;" + H7Count + " level 7s;" + H8Count + " level 8s;" + H9Count + " level 9s"; }



        // Constructors

        public DCC_DirectorateList(DCC_Session session = null, string pHierarchyLevel2 = null, string pHierarchyLevel3 = null, string pHierarchyLevel4 = null, string pHierarchyLevel5 = null, string pHierarchyLevel6 = null, string pHierarchyLevel7 = null, string pHierarchyLevel8 = null)
        {
            if (session.ResourceLinkDatabaseConnection != null)
            {
                DCC_Database db = session.ResourceLinkDatabaseConnection;
                DCC_DatabaseFrame dbf;

                if (!string.IsNullOrEmpty(pHierarchyLevel2))
                {
                    dbf = new DCC_DatabaseFrame("**select distinct hierarchy_level_3 from dcc_v_myworkforce_hier where hierarchy_level_2 = :pHierarchyLevel2 and hierarchy_level_3 is not null order by hierarchy_level_3");
                    dbf.AddParameter("pHierarchyLevel2", pHierarchyLevel2, 100);
                    dbf.AddTable("dcc_v_myworkforce_estab");
                    db.DataLayer.SelectData(dbf);
                    if (dbf.hasTables)
                    {
                        foreach (DataRow dr in dbf.TableZeroRows)
                        {
                            string division = DCC_GetColumn.String(dr, "HIERARCHY_LEVEL_3");
                            if ( (division != "Academic Supply") && (division != "School/Education Establishments") )
                            {
                                DivisionList.Add(new DCC_Directorate(null, division, null, null, null, null, null, null));
                            }
                        }
                        if (pHierarchyLevel2 == "Children & Young Peoples Services")
                        {
                            DivisionList.Add(new DCC_Directorate(null, "Social Work Academy", null, null, null, null, null, null));
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(pHierarchyLevel3))
                {
                    dbf = new DCC_DatabaseFrame("**select distinct hierarchy_level_4 from dcc_v_myworkforce_hier where hierarchy_level_3 = :pHierarchyLevel3 and hierarchy_level_4 is not null order by hierarchy_level_4");
                    dbf.AddParameter("pHierarchyLevel3", pHierarchyLevel3, 100);
                    dbf.AddTable("dcc_v_myworkforce_estab");
                    db.DataLayer.SelectData(dbf);
                    if (dbf.hasTables)
                    {
                        foreach (DataRow dr in dbf.TableZeroRows)
                        {
                            string section = DCC_GetColumn.String(dr, "HIERARCHY_LEVEL_4");
                            SectionList.Add(new DCC_Directorate(null, null, section, null, null, null, null, null));
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(pHierarchyLevel4))
                {
                    dbf = new DCC_DatabaseFrame("**select distinct hierarchy_level_5 from dcc_v_myworkforce_hier where hierarchy_level_4 = :pHierarchyLevel4 and hierarchy_level_5 is not null order by hierarchy_level_5");
                    dbf.AddParameter("pHierarchyLevel4", pHierarchyLevel4, 100);
                    dbf.AddTable("dcc_v_myworkforce_estab");
                    db.DataLayer.SelectData(dbf);
                    if (dbf.hasTables)
                    {
                        foreach (DataRow dr in dbf.TableZeroRows)
                        {
                            string team = DCC_GetColumn.String(dr, "HIERARCHY_LEVEL_5");
                            TeamList.Add(new DCC_Directorate(null, null, null, team, null, null, null, null));
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(pHierarchyLevel5))
                {
                    dbf = new DCC_DatabaseFrame("**select distinct hierarchy_level_6 from dcc_v_myworkforce_estab where hierarchy_level_5 = :pHierarchyLevel5 and hierarchy_level_6 is not null order by hierarchy_level_6");
                    dbf.AddParameter("pHierarchyLevel5", pHierarchyLevel5, 100);
                    dbf.AddTable("dcc_v_myworkforce_estab");
                    db.DataLayer.SelectData(dbf);
                    if (dbf.hasTables)
                    {
                        foreach (DataRow dr in dbf.TableZeroRows)
                        {
                            string h6 = DCC_GetColumn.String(dr, "HIERARCHY_LEVEL_6");
                            H6List.Add(new DCC_Directorate(null, null, null, null, h6, null, null, null));
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(pHierarchyLevel6))
                {
                    dbf = new DCC_DatabaseFrame("**select distinct hierarchy_level_7 from dcc_v_myworkforce_estab where hierarchy_level_6 = :pHierarchyLevel6 and hierarchy_level_7 is not null order by hierarchy_level_7");
                    dbf.AddParameter("pHierarchyLevel6", pHierarchyLevel6, 100);
                    dbf.AddTable("dcc_v_myworkforce_estab");
                    db.DataLayer.SelectData(dbf);
                    if (dbf.hasTables)
                    {
                        foreach (DataRow dr in dbf.TableZeroRows)
                        {
                            string h7 = DCC_GetColumn.String(dr, "HIERARCHY_LEVEL_7");
                            H7List.Add(new DCC_Directorate(null, null, null, null, null, h7, null, null));
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(pHierarchyLevel7))
                {
                    dbf = new DCC_DatabaseFrame("**select distinct hierarchy_level_8 from dcc_v_myworkforce_estab where hierarchy_level_7 = :pHierarchyLevel7 and hierarchy_level_8 is not null order by hierarchy_level_8");
                    dbf.AddParameter("pHierarchyLevel7", pHierarchyLevel7, 100);
                    dbf.AddTable("dcc_v_myworkforce_estab");
                    db.DataLayer.SelectData(dbf);
                    if (dbf.hasTables)
                    {
                        foreach (DataRow dr in dbf.TableZeroRows)
                        {
                            string h8 = DCC_GetColumn.String(dr, "HIERARCHY_LEVEL_8");
                            H8List.Add(new DCC_Directorate(null, null, null, null, null, null, h8, null));
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(pHierarchyLevel8))
                {
                    dbf = new DCC_DatabaseFrame("**select distinct hierarchy_level_9 from dcc_v_myworkforce_estab where hierarchy_level_8 = :pHierarchyLevel8 and hierarchy_level_9 is not null order by hierarchy_level_9");
                    dbf.AddParameter("pHierarchyLevel8", pHierarchyLevel8, 100);
                    dbf.AddTable("dcc_v_myworkforce_estab");
                    db.DataLayer.SelectData(dbf);
                    if (dbf.hasTables)
                    {
                        foreach (DataRow dr in dbf.TableZeroRows)
                        {
                            string h9 = DCC_GetColumn.String(dr, "HIERARCHY_LEVEL_9");
                            H9List.Add(new DCC_Directorate(null, null, null, null, null, null, null, h9));
                        }
                    }
                }
                else
                {
                    dbf = new DCC_DatabaseFrame  ("**select distinct hierarchy_level_2 from dcc_v_myworkforce_hier where hierarchy_level_2 is not null order by hierarchy_level_2");
                    dbf.AddTable("dcc_v_myworkforce_hier");
                    db.DataLayer.SelectData(dbf);
                    if (dbf.hasTables)
                    {
                        foreach (DataRow dr in dbf.TableZeroRows)
                        {
                            DCC_Directorate d = new DCC_Directorate(dr);
                            ServiceList.Add(new DCC_Directorate(
                                d.Service  + " (" + d.HierarchyLevel2Id + ")",
                                null, null, null, null, null, null, null));
                        }
                    }

                    dbf = new DCC_DatabaseFrame("**select distinct hierarchy_level_2, hierarchy_level_3 from dcc_v_myworkforce_hier where hierarchy_level_3 is not null order by hierarchy_level_2, hierarchy_level_3");
                    dbf.AddTable("dcc_v_myworkforce_hier");
                    db.DataLayer.SelectData(dbf);
                    if (dbf.hasTables)
                    {
                        foreach (DataRow dr in dbf.TableZeroRows)
                        {
                            DCC_Directorate d = new DCC_Directorate(dr);
                            DivisionList.Add(new DCC_Directorate(
                                d.Service  + " (" + d.HierarchyLevel2Id + ")",
                                d.Division + " (" + d.HierarchyLevel3Id + ")",
                                null, null, null, null, null, null));
                        }
                    }

                    dbf = new DCC_DatabaseFrame("**select distinct hierarchy_level_2, hierarchy_level_3, hierarchy_level_4 from dcc_v_myworkforce_hier where hierarchy_level_4 is not null order by hierarchy_level_2, hierarchy_level_3, hierarchy_level_4");
                    dbf.AddTable("dcc_v_myworkforce_hier");
                    db.DataLayer.SelectData(dbf);
                    if (dbf.hasTables)
                    {
                        foreach (DataRow dr in dbf.TableZeroRows)
                        {
                            DCC_Directorate d = new DCC_Directorate(dr);
                            SectionList.Add(new DCC_Directorate(
                                d.Service  + " (" + d.HierarchyLevel2Id + ")",
                                d.Division + " (" + d.HierarchyLevel3Id + ")",
                                d.Section  + " (" + d.HierarchyLevel4Id + ")",
                                null, null, null, null, null));
                        }
                    }

                    dbf = new DCC_DatabaseFrame("**select distinct hierarchy_level_2, hierarchy_level_3, hierarchy_level_4, hierarchy_level_5 from dcc_v_myworkforce_hier where hierarchy_level_5 is not null order by hierarchy_level_2, hierarchy_level_3, hierarchy_level_4, hierarchy_level_5");
                    dbf.AddTable("dcc_v_myworkforce_hier");
                    db.DataLayer.SelectData(dbf);
                    if (dbf.hasTables)
                    {
                        foreach (DataRow dr in dbf.TableZeroRows)
                        {
                            DCC_Directorate d = new DCC_Directorate(dr);
                            TeamList.Add(new DCC_Directorate(
                                d.Service  + " (" + d.HierarchyLevel2Id + ")",
                                d.Division + " (" + d.HierarchyLevel3Id + ")",
                                d.Section  + " (" + d.HierarchyLevel4Id + ")",
                                d.Team     + " (" + d.HierarchyLevel5Id + ")",
                                null, null, null, null));
                        }
                    }

                    // go up to 9????
                }

            }
        }



        // Enumerator

        public IEnumerator<DCC_Directorate> GetEnumerator() { foreach (object o in TeamList) yield return (DCC_Directorate)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in TeamList) yield return "dummy"; }


    }


}
