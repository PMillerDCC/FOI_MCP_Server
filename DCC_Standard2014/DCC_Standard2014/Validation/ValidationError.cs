using System.Text;

namespace DCC_Standard
{

	public class ValidationError
    {

		// Properties

        public string ID;
        public string msg;
        public string propertyName;
		
        public string Description
        {
            get
            {
                StringBuilder buidler = new StringBuilder();
                return buidler.Append("Property Name='")
							  .Append(propertyName)
							  .Append("', ID='")
							  .Append(ID)
							  .Append("', Msg='")
							  .Append(msg)
							  .Append("'").ToString();
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            ValidationError other = obj as ValidationError;
            if (other == null) return false;
            return Equals(other);
        }

        public bool Equals(ValidationError other)
        {
            if (other == null) return false;
            return (this.propertyName == other.propertyName && this.ID == other.ID);
        }

        public override int GetHashCode()
        {
            return propertyName.GetHashCode() ^ ID.GetHashCode();
        }



		// Utilities

        public override string ToString() { return msg; }



		// Constructor

        public ValidationError(string propName, string errorID, string errorMessage)
        {
            propertyName = propName;
            ID = errorID;
            msg = errorMessage;
        }

    }

}
