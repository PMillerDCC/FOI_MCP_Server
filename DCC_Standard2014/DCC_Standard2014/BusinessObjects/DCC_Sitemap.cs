using System.Collections.Generic;
using System.Xml.Serialization;

namespace DCC_Standard
{

	public class NavigationOption
	{
		[XmlElement("title")]
		public string Title { get; set; }  //mandatory

		[XmlElement("href")]
		public string Href { get; set; }

		[XmlElement("isParent")]
		public string IsParent { get; set; } // defaults to no

		[XmlElement("isChild")]
		public string IsChild { get; set; } // defaults to no

		[XmlElement("isDummy")]
		public string IsDummy { get; set; } // defaults to no

		[XmlElement("isDisabled")]
		public string IsDisabled { get; set; } // defaults to no

		[XmlElement("isSeparator")]
		public string IsSeparator { get; set; } // defaults to no

		[XmlElement("restrictedTo")]
		public string RestrictedTo { get; set; }

		public bool IsLastChild { get; set; }

		public NavigationOption()
		{
			IsParent = IsChild = IsDummy = IsDisabled = IsSeparator = "no";
		}
	}


	[XmlRoot("DCC_Navigation")]
	public class DCC_Navigation
	{
		[XmlElement("NavigationOption", typeof(NavigationOption))]
		public List<NavigationOption> OptionList { get; set; }
	}

}
