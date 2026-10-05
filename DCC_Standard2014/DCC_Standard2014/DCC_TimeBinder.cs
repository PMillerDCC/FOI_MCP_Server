using System;
using System.Web.Mvc;

namespace DCC_Standard
{

    public class DCC_TimeBinder : IModelBinder
    {

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            string key = bindingContext.ModelName;
            var value = bindingContext.ValueProvider.GetValue(key);
            if (value == null || string.IsNullOrEmpty(value.AttemptedValue)) return null;

            bindingContext.ModelState.SetModelValue(key, value);

            var hours = ((string[])value.RawValue)[0];
            var minutes = ((string[])value.RawValue)[1];
            var time = new TimeSpan(Convert.ToInt32(hours), Convert.ToInt32(minutes), 0);

            return time;
        }

    }

}
