using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;

namespace DCC_Standard
{

    #region DCC_GenderSelectList

    /// <summary>
    /// SelectList for Gender
    /// </summary>
    class GenderList : List<object>
    {

        public GenderList() : base(new List<Object> { })
        {
            Add(new { value = "", text = "-- not set --" });
            Add(new { value = "M", text = "Male" });
            Add(new { value = "F", text = "Female" });
            Add(new { value = "N", text = "Non-Binary" });
        }

    }

    /// <summary>
    /// Male/Female/--Not Set-- in a SelectList
    /// </summary>
    public class DCC_GenderSelectList : SelectList
    {

        public DCC_GenderSelectList(object selectedValue) : base(new GenderList(), "value", "text", selectedValue)
        {
        }

        public DCC_GenderSelectList(List<Object> items, string dataValueField, string dataTextField, object selectedValue) : base(items, dataValueField, dataTextField, selectedValue)
        {
        }

    }

    #endregion


    #region DCC_YesNoSelectList

    /// <summary>
    /// Yes/No for SelectList
    /// </summary>
    class YesNoList : List<object>
	{

		public YesNoList() : base(new List<Object> { })
		{
			Add(new { value = "", text = "-- not set --" });
			Add(new { value = "No", text = "No" });
			Add(new { value = "Yes", text = "Yes" });
		}

	}

    /// <summary>
    /// Yes?No/--Not Set-- in a SelectList
    /// </summary>
    public class DCC_YesNoSelectList : SelectList
    {

        public DCC_YesNoSelectList(object selectedValue) : base(new YesNoList(), "value", "text", selectedValue)
        {
        }

        public DCC_YesNoSelectList(List<Object> items, string dataValueField, string dataTextField, object selectedValue) : base(items, dataValueField, dataTextField, selectedValue)
        {
        }
    }

	#endregion


    #region YesNoNotApplicableList

    /// <summary>
    /// Yes/No for SelectList
    /// </summary>
    class YesNoNotApplicableList : List<object>
	{

		public YesNoNotApplicableList() : base(new List<Object> { })
		{
			Add(new { value = "", text = "-- not set --" });
			Add(new { value = "N/A", text = "N/A" });
			Add(new { value = "No", text = "No" });
			Add(new { value = "Yes", text = "Yes" });
		}

	}

    /// <summary>
    /// Yes?No/--Not Set-- in a SelectList
    /// </summary>
    public class DCC_YesNoNotApplicableSelectList : SelectList
    {

        public DCC_YesNoNotApplicableSelectList(object selectedValue) : base(new YesNoNotApplicableList(), "value", "text", selectedValue)
        {
        }

        public DCC_YesNoNotApplicableSelectList(List<Object> items, string dataValueField, string dataTextField, object selectedValue) : base(items, dataValueField, dataTextField, selectedValue)
        {
        }
    }

	#endregion


	#region DCC_DaysSelectList

	/// <summary>
	/// List of days of week for SelectList
	/// </summary>
	class DaysList : List<Object>
    {

        public DaysList() : base(new List<Object>
        {
            new { value = 1 , text = "Monday"    },
            new { value = 2 , text = "Tuesday"   },
            new { value = 3 , text = "Wednesday" },
            new { value = 4 , text = "Thursday"  },
            new { value = 5 , text = "Friday"    },
            new { value = 6 , text = "Saturday"  },
            new { value = 7 , text = "Sunday"    } } )
        {
        }

    }

    /// <summary>
    /// Days of the week in a SelectList
    /// </summary>
    public class DCC_DaysSelectList : SelectList
    {

        public DCC_DaysSelectList(object selectedValue) : base(new DaysList(), "value", "text", selectedValue)
        {
        }

        public DCC_DaysSelectList(List<Object> items, string dataValueField, string dataTextField, object selectedValue) : base(items, dataValueField, dataTextField, selectedValue)
        {
        }

    }

    #endregion


    #region DCC_HoursSelectList

    /// <summary>
    /// List of hours for Select List
    /// </summary>
    class HoursList : List<Object>
    {
        public HoursList() : base(new List<Object>
        {
            new { value =  0 , text = "00" },
            new { value =  1 , text = "01" },
            new { value =  2 , text = "02" },
            new { value =  3 , text = "03" },
            new { value =  4 , text = "04" },
            new { value =  5 , text = "05" },
            new { value =  6 , text = "06" },
            new { value =  7 , text = "07" },
            new { value =  8 , text = "08" },
            new { value =  9 , text = "09" },
            new { value = 10 , text = "10" },
            new { value = 11 , text = "11" },
            new { value = 12 , text = "12" },
            new { value = 13 , text = "13" },
            new { value = 14 , text = "14" },
            new { value = 15 , text = "15" },
            new { value = 16 , text = "16" },
            new { value = 17 , text = "17" },
            new { value = 18 , text = "18" },
            new { value = 19 , text = "19" },
            new { value = 20 , text = "20" },
            new { value = 21 , text = "21" },
            new { value = 22 , text = "22" },
            new { value = 23 , text = "23" } })
        {
        }
    }

    /// <summary>
    /// Hours of the day in a SelectList
    /// </summary>
    public class DCC_HoursSelectList : SelectList
    {

        public DCC_HoursSelectList(object selectedValue) : base(new HoursList(), "value", "text", selectedValue)
        {
        }

        public DCC_HoursSelectList(List<Object> items, string dataValueField, string dataTextField, object selectedValue) : base(items, dataValueField, dataTextField, selectedValue)
        {
        }

    }

    #endregion


    #region DCC_MinutesSelectList
    /// <summary>
    /// List of minutes for Select List
    /// </summary>
    class MinutesList : List<Object>
    {
        public MinutesList() : base(new List<Object>
        {
            new { value =  0 , text = "00" },
            new { value =  1 , text = "01" },
            new { value =  2 , text = "02" },
            new { value =  3 , text = "03" },
            new { value =  4 , text = "04" },
            new { value =  5 , text = "05" },
            new { value =  6 , text = "06" },
            new { value =  7 , text = "07" },
            new { value =  8 , text = "08" },
            new { value =  9 , text = "09" },
            new { value = 10 , text = "10" },
            new { value = 11 , text = "11" },
            new { value = 12 , text = "12" },
            new { value = 13 , text = "13" },
            new { value = 14 , text = "14" },
            new { value = 15 , text = "15" },
            new { value = 16 , text = "16" },
            new { value = 17 , text = "17" },
            new { value = 18 , text = "18" },
            new { value = 19 , text = "19" },
            new { value = 20 , text = "20" },
            new { value = 21 , text = "21" },
            new { value = 22 , text = "22" },
            new { value = 23 , text = "23" },
            new { value = 24 , text = "24" },
            new { value = 25 , text = "25" },
            new { value = 26 , text = "26" },
            new { value = 27 , text = "27" },
            new { value = 28 , text = "28" },
            new { value = 29 , text = "29" },
            new { value = 30 , text = "30" },
            new { value = 31 , text = "31" },
            new { value = 32 , text = "32" },
            new { value = 33 , text = "33" },
            new { value = 34 , text = "34" },
            new { value = 35 , text = "35" },
            new { value = 36 , text = "36" },
            new { value = 37 , text = "37" },
            new { value = 38 , text = "38" },
            new { value = 39 , text = "39" },
            new { value = 40 , text = "40" },
            new { value = 41 , text = "41" },
            new { value = 42 , text = "42" },
            new { value = 43 , text = "43" },
            new { value = 44 , text = "44" },
            new { value = 45 , text = "45" },
            new { value = 46 , text = "46" },
            new { value = 47 , text = "47" },
            new { value = 48 , text = "48" },
            new { value = 49 , text = "49" },
            new { value = 50 , text = "50" },
            new { value = 51 , text = "51" },
            new { value = 52 , text = "52" },
            new { value = 53 , text = "53" },
            new { value = 54 , text = "54" },
            new { value = 55 , text = "55" },
            new { value = 56 , text = "56" },
            new { value = 57 , text = "57" },
            new { value = 58 , text = "58" },
            new { value = 59 , text = "59" } })
        {
        }
    }

    /// <summary>
    /// Minutes in a SelectList
    /// </summary>
    public class DCC_MinutesSelectList : SelectList
    {

        public DCC_MinutesSelectList(object selectedValue) : base(new MinutesList(), "value", "text", selectedValue)
        {
        }

        public DCC_MinutesSelectList(List<Object> items, string dataValueField, string dataTextField, object selectedValue) : base(items, dataValueField, dataTextField, selectedValue)
        {
        }
    }

    #endregion


    #region QuarterHoursSelectList
    /// <summary>
    /// List of quarter hours for Select List
    /// </summary>
    class QuarterHoursList : List<Object>
    {
        public QuarterHoursList() : base(new List<Object>
        {
            new { value =  0 , text = "00" },
            new { value = 15 , text = "15" },
            new { value = 30 , text = "30" },
            new { value = 45 , text = "45" } })
        {
        }
    }

    /// <summary>
    /// Quarter Hours in a SelectList
    /// </summary>
    public class DCC_QuarterHoursSelectList : SelectList
    {

        public DCC_QuarterHoursSelectList(object selectedValue) : base(new QuarterHoursList(), "value", "text", selectedValue)
        {
        }

        public DCC_QuarterHoursSelectList(List<Object> items, string dataValueField, string dataTextField, object selectedValue) : base(items, dataValueField, dataTextField, selectedValue)
        {
        }
    }

	#endregion


	#region DCC_EthnicGroupSelectList

	class EthnicGroupList : List<object>
    {

		/// <summary>
		/// SelectList for Ethnic Groups
		/// </summary>
        public EthnicGroupList() : base(new List<Object> { })
        {
            Add(new { value = "", text = "-- not set --" });
            Add(new { value = "Not given", text = "Not given" });
            Add(new { value = "White British", text = "White British" });
            Add(new { value = "White Irish", text = "White Irish" });
            Add(new { value = "Gypsy/Roma", text = "Gypsy/Roma" });
            Add(new { value = "Traveller of Irish Heritage", text = "Traveller of Irish Heritage" });
            Add(new { value = "Any other White background", text = "Any other White background" });
            Add(new { value = "Caribbean", text = "Caribbean" });
            Add(new { value = "White & Black Caribbean", text = "White & Black Caribbean" });
            Add(new { value = "African", text = "African" });
            Add(new { value = "White & Black African", text = "White & Black African" });
            Add(new { value = "Any other Black background", text = "Any other Black background" });
            Add(new { value = "Indian", text = "Indian" });
            Add(new { value = "Pakistani", text = "Pakistani" });
            Add(new { value = "Bangladeshi", text = "Bangladeshi" });
            Add(new { value = "Chinese", text = "Chinese" });
            Add(new { value = "White & Asian", text = "White & Asian" });
            Add(new { value = "Any other Asian background", text = "Any other Asian background" });
            Add(new { value = "Any other ethnic group", text = "Any other ethnic group" });
            Add(new { value = "Any other mixed background", text = "Any other mixed background" });
		}
	}

    /// <summary>
    /// EthnicGroup in a SelectList
    /// </summary>
    public class DCC_EthnicGroupSelectList : SelectList
    {

        public DCC_EthnicGroupSelectList(object selectedValue) : base(new EthnicGroupList(), "value", "text", selectedValue)
        {
        }

        public DCC_EthnicGroupSelectList(List<Object> items, string dataValueField, string dataTextField, object selectedValue) : base(items, dataValueField, dataTextField, selectedValue)
        {
        }
    }

	#endregion


    #region DCC_SpokenLanguageSelectList

    class SpokenLanguageList : List<object>
    {

        /// <summary>
        /// SelectList for Spoken Languages
        /// </summary>
        public SpokenLanguageList() : base(new List<Object> { })
        {
            Add(new { value = "", text = "-- not set --" });
            Add(new { value = "Akan", text = "Akan" });
            Add(new { value = "Amharic", text = "Amharic" });
            Add(new { value = "Arabic", text = "Arabic" });
            Add(new { value = "Assamese", text = "Assamese" });
            Add(new { value = "Awadhi", text = "Awadhi" });
            Add(new { value = "Azerbaijani", text = "Azerbaijani" });
            Add(new { value = "Balochi", text = "Balochi" });
            Add(new { value = "Belarusian", text = "Belarusian" });
            Add(new { value = "Bengali", text = "Bengali" });
            Add(new { value = "Bhojpuri", text = "Bhojpuri" });
            Add(new { value = "Burmese", text = "Burmese" });
            Add(new { value = "Cebuano (Visayan)", text = "Cebuano (Visayan)" });
            Add(new { value = "Chewa", text = "Chewa" });
            Add(new { value = "Chhattisgarhi", text = "Chhattisgarhi" });
            Add(new { value = "Chittagonian", text = "Chittagonian" });
            Add(new { value = "Czech", text = "Czech" });
            Add(new { value = "Deccan", text = "Deccan" });
            Add(new { value = "Dhundhari", text = "Dhundhari" });
            Add(new { value = "Dutch", text = "Dutch" });
            Add(new { value = "Eastern Min", text = "Eastern Min" });
            Add(new { value = "English", text = "English" });
            Add(new { value = "French", text = "French" });
            Add(new { value = "Fula", text = "Fula" });
            Add(new { value = "Gan Chinese", text = "Gan Chinese" });
            Add(new { value = "German", text = "German" });
            Add(new { value = "Greek", text = "Greek" });
            Add(new { value = "Gujarati", text = "Gujarati" });
            Add(new { value = "Haitian Creole", text = "Haitian Creole" });
            Add(new { value = "Hakka", text = "Hakka" });
            Add(new { value = "Haryanvi", text = "Haryanvi" });
            Add(new { value = "Hausa", text = "Hausa" });
            Add(new { value = "Hiligaynon/Ilonggo (Visayan)", text = "Hiligaynon/Ilonggo (Visayan)" });
            Add(new { value = "Hindi", text = "Hindi" });
            Add(new { value = "Hmong", text = "Hmong" });
            Add(new { value = "Hungarian", text = "Hungarian" });
            Add(new { value = "Igbo", text = "Igbo" });
            Add(new { value = "Ilocano", text = "Ilocano" });
            Add(new { value = "Italian", text = "Italian" });
            Add(new { value = "Japanese", text = "Japanese" });
            Add(new { value = "Javanese", text = "Javanese" });
            Add(new { value = "Jin", text = "Jin" });
            Add(new { value = "Kannada", text = "Kannada" });
            Add(new { value = "Kazakh", text = "Kazakh" });
            Add(new { value = "Khmer", text = "Khmer" });
            Add(new { value = "Kinyarwanda", text = "Kinyarwanda" });
            Add(new { value = "Kirundi", text = "Kirundi" });
            Add(new { value = "Konkani", text = "Konkani" });
            Add(new { value = "Korean", text = "Korean" });
            Add(new { value = "Kurdish", text = "Kurdish" });
            Add(new { value = "Madurese", text = "Madurese" });
            Add(new { value = "Magahi", text = "Magahi" });
            Add(new { value = "Maithili", text = "Maithili" });
            Add(new { value = "Malagasy", text = "Malagasy" });
            Add(new { value = "Malay", text = "Malay" });
            Add(new { value = "Malayalam", text = "Malayalam" });
            Add(new { value = "Mandarin", text = "Mandarin" });
            Add(new { value = "Marathi", text = "Marathi" });
            Add(new { value = "Marwari", text = "Marwari" });
            Add(new { value = "Mossi", text = "Mossi" });
            Add(new { value = "Nepali", text = "Nepali" });
            Add(new { value = "Northern Min", text = "Northern Min" });
            Add(new { value = "Odia (Oriya)", text = "Odia (Oriya)" });
            Add(new { value = "Oromo", text = "Oromo" });
            Add(new { value = "Pashto", text = "Pashto" });
            Add(new { value = "Persian", text = "Persian" });
            Add(new { value = "Polish", text = "Polish" });
            Add(new { value = "Portuguese", text = "Portuguese" });
            Add(new { value = "Punjabi", text = "Punjabi" });
            Add(new { value = "Quechua", text = "Quechua" });
            Add(new { value = "Romanian", text = "Romanian" });
            Add(new { value = "Russian", text = "Russian" });
            Add(new { value = "Saraiki", text = "Saraiki" });
            Add(new { value = "Serbo-Croatian", text = "Serbo-Croatian" });
            Add(new { value = "Shona", text = "Shona" });
            Add(new { value = "Sindhi", text = "Sindhi" });
            Add(new { value = "Sinhalese", text = "Sinhalese" });
            Add(new { value = "Somali", text = "Somali" });
            Add(new { value = "Southern Min", text = "Southern Min" });
            Add(new { value = "Spanish", text = "Spanish" });
            Add(new { value = "Sundanese", text = "Sundanese" });
            Add(new { value = "Swedish", text = "Swedish" });
            Add(new { value = "Sylheti", text = "Sylheti" });
            Add(new { value = "Tagalog", text = "Tagalog" });
            Add(new { value = "Tamil", text = "Tamil" });
            Add(new { value = "Telugu", text = "Telugu" });
            Add(new { value = "Thai", text = "Thai" });
            Add(new { value = "Turkish", text = "Turkish" });
            Add(new { value = "Turkmen", text = "Turkmen" });
            Add(new { value = "Ukrainian", text = "Ukrainian" });
            Add(new { value = "Urdu", text = "Urdu" });
            Add(new { value = "Uyghur", text = "Uyghur" });
            Add(new { value = "Uzbek", text = "Uzbek" });
            Add(new { value = "Vietnamese", text = "Vietnamese" });
            Add(new { value = "Wu", text = "Wu" });
            Add(new { value = "Xhosa", text = "Xhosa" });
            Add(new { value = "Xiang (Hunanese)", text = "Xiang (Hunanese)" });
            Add(new { value = "Yoruba", text = "Yoruba" });
            Add(new { value = "Yue", text = "Yue" });
            Add(new { value = "Zhuang", text = "Zhuang" });
            Add(new { value = "Zulu", text = "Zulu" });
        }
    }

    /// <summary>
    /// SpokenLanguage in a SelectList
    /// </summary>
    public class DCC_SpokenLanguageSelectList : SelectList
    {

        public DCC_SpokenLanguageSelectList(object selectedValue) : base(new SpokenLanguageList(), "value", "text", selectedValue)
        {
        }

        public DCC_SpokenLanguageSelectList(List<Object> items, string dataValueField, string dataTextField, object selectedValue) : base(items, dataValueField, dataTextField, selectedValue)
        {
        }
    }

    #endregion


    #region DCC_SecurityQuestionSelectList

    /// <summary>
    /// SelectList for SecurityQuestion
    /// </summary>
    class SecurityQuestionList : List<object>
    {

        public SecurityQuestionList() : base(new List<Object> { })
        {
            Add(new { value = "0", text = "-- not set --" });
            Add(new { value = "1", text = "What is your mother's maiden name?" });
            Add(new { value = "2", text = "What is your father's middle name?" });
            Add(new { value = "3", text = "On what street did you live on as a child?" });
            Add(new { value = "4", text = "In what town or city were you born?" });
            Add(new { value = "5", text = "What school did you first attend?" });
            Add(new { value = "6", text = "What was the make of your first car?" });
            Add(new { value = "7", text = "What is the name of your favourite pet?" });
            Add(new { value = "8", text = "What is your favourite place?" });
            Add(new { value = "9", text = "Who is your favourite actor/actress?" });
            Add(new { value = "10", text = "Who is your favourite musician?" });
            Add(new { value = "11", text = "Who is your favourite artist?" });
        }


    }

    /// <summary>
    /// Male/Female/--Not Set-- in a SelectList
    /// </summary>
    public class DCC_SecurityQuestionSelectList : SelectList
    {

        public DCC_SecurityQuestionSelectList(object selectedValue) : base(new SecurityQuestionList(), "value", "text", selectedValue)
        {
        }

        public DCC_SecurityQuestionSelectList(List<Object> items, string dataValueField, string dataTextField, object selectedValue) : base(items, dataValueField, dataTextField, selectedValue)
        {
        }

		public string GetDescription(string pValue)
		{
			string description = "not found";

			foreach (object o in Items)
			{
				IList<PropertyInfo> properties = new List<PropertyInfo>(o.GetType().GetProperties());
				string value = (string)properties[0].GetValue(o, null);
				string text = (string)properties[1].GetValue(o, null);
				if (value == pValue)
				{
					description = text;
					break;
				}
			}

			return description;
		}

    }

    #endregion


    #region DCC_FinancialYearSelectList

    /// <summary>
    /// Financial years in a SelectList
    /// </summary>
    class FinancialYearList : List<object>
	{

		public FinancialYearList() : base(new List<Object> { })
		{
            string fy1, fy2, fy3;
            DateTime date = DateTime.Now;

            fy1 = ( (date.AddYears(0).Month > 3)
                ? date.AddYears( 0).ToString("yy") + "/" + date.AddYears(1).ToString("yy")
                : date.AddYears(-1).ToString("yy") + "/" + date.AddYears(0).ToString("yy")
                );

            fy2 = ( (date.AddYears(1).Month > 3)
                ? date.AddYears(1).ToString("yy") + "/" + date.AddYears(2).ToString("yy")
                : date.AddYears(0).ToString("yy") + "/" + date.AddYears(1).ToString("yy")
                );

            fy3 = ( (date.AddYears(2).Month > 3)
                ? date.AddYears(2).ToString("yy") + "/" + date.AddYears(3).ToString("yy")
                : date.AddYears(1).ToString("yy") + "/" + date.AddYears(2).ToString("yy")
                );

			Add(new { value = "", text = "-- not set --" });
			Add(new { value = fy1, text = fy1 });
			Add(new { value = fy2, text = fy2 });
			Add(new { value = fy3, text = fy3 });
		}

	}

    /// <summary>
    /// The current and next 2 financial years/--Not Set-- in a SelectList
    /// </summary>
    public class DCC_FinancialYearSelectList : SelectList
    {

        public DCC_FinancialYearSelectList(object selectedValue) : base(new FinancialYearList(), "value", "text", selectedValue)
        {
        }

        public DCC_FinancialYearSelectList(List<Object> items, string dataValueField, string dataTextField, object selectedValue) : base(items, dataValueField, dataTextField, selectedValue)
        {
        }
    }

	#endregion

}
