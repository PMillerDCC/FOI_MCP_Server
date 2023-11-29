using System.Data.Common;
using System.Web.Mvc;

namespace Prospect
{

    public class FilterConfig
    {

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute
            {
                ExceptionType = typeof(DbException),
                View = "Error", // Error.cshtml is a view in the Shared folder.
                Order = 2
            });
            filters.Add(new HandleErrorAttribute());
        }

    }

}
