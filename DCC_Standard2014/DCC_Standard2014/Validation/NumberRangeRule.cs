using System;
using System.Windows.Controls;

namespace DCC_Standard
{

	public class IntegerRangeRule : ValidationRule
    {
        int min = int.MinValue;
        int max = int.MaxValue;

		public string Name { get; set; }
        public int Min { get { return min; } set { min = value; } }
        public int Max { get { return max; } set { max = value; } }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (!String.IsNullOrEmpty((string)value))
            {
                if (Name.Length == 0) Name = "Field";
                try
                {
                    if (((string)value).Length > 0)
                    {
                        int val = Int32.Parse((String)value);
                        if (val > max) return new ValidationResult(false, Name + " must be <= " + Max + ".");
                        if (val < min) return new ValidationResult(false, Name + " must be >= " + Min + ".");
                    }
                }
                catch (Exception)
                {
                    return new ValidationResult(false, Name + " is not in a correct numeric format.");
                }
            }
            return ValidationResult.ValidResult;
        }

    }

}
