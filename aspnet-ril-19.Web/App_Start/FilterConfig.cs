﻿using System.Web;
using System.Web.Mvc;

namespace aspnet_ril_19.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
