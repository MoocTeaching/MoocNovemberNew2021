﻿using Mooc.Web.App_Start.Filter;
using System.Web;
using System.Web.Mvc;

namespace Mooc.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandlerGlobalError());
        }
    }
}
