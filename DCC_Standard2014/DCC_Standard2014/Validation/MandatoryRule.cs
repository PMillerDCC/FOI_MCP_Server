using System;
using System.Globalization;
using System.Windows.Controls;

namespace DCC_Standard
{

	public class MandatoryRule : ValidationRule
    {

		public string Name { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(String.IsNullOrEmpty((string)value))
            {
                if (Name.Length == 0) Name = "Field";
                return new ValidationResult(false, Name + " is mandatory.");
            }
            return ValidationResult.ValidResult;
        }

    }

}
