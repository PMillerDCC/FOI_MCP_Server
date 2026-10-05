using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DCC_Standard
{

    #region DCC_SystemRole

    [Serializable]
    public class DCC_SystemRole
    {

        // Get/Set Properties

        public string Role      { get; set; }
        public bool   IsChecked { get; set; }



        // Display Properties

        public string ID        { get { return Role.Replace(" ", ""); } }



        // Utilities

        public override string ToString()
        {
            return Role + " " + IsChecked.ToString();
        }



        // Constructor

        public DCC_SystemRole(string role, bool isChecked)
        {
            Role = role;
            IsChecked = isChecked;
        }

    }

    #endregion


    #region DCC_SystemRoleList

    [Serializable]
    public class DCC_SystemRoleList : IEnumerable<DCC_SystemRole>
    {


        // Properties

        public List<DCC_SystemRole> All = new List<DCC_SystemRole>();
        public int Count { get { return All.Count; } }



        // Utilities

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (DCC_SystemRole role in All) sb.Append(role.ToString());
            return sb.ToString();
        }



        // Constructor

        public DCC_SystemRoleList()
        {
        }



        // Methods

        public void Add(DCC_SystemRole systemRole)
        {
            All.Add(new DCC_SystemRole(systemRole.Role, systemRole.IsChecked));
        }



        // Enumerator

        public IEnumerator<DCC_SystemRole> GetEnumerator() { foreach (object o in All) yield return (DCC_SystemRole)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }

    }

    #endregion

}
