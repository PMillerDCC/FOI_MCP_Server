using System;
using System.Globalization;
using System.Web.Mvc;

public class CommaSafeDecimalModelBinder : IModelBinder
{
    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
        ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

        if (valueResult == null || string.IsNullOrWhiteSpace(valueResult.AttemptedValue))
            return null;

        string raw = valueResult.AttemptedValue;

        // Remove thousands separators
        raw = raw.Replace(",", "");

        // Normalise decimal separator to dot
        raw = raw.Replace(",", "").Replace(" ", "").Trim();

        decimal parsed;
        if (decimal.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out parsed))
            return parsed;

        bindingContext.ModelState.AddModelError(bindingContext.ModelName, $"The value '{valueResult.AttemptedValue}' is not a valid decimal.");
        return null;
    }
}