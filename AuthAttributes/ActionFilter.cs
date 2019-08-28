using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace Hermes.WebManage.AuthAttributes
{
    public class ActionFilter: System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var arrlst = new Dictionary<string, string>();
            arrlst.Add("admin", "ALL");
            arrlst.Add("dtxx", "GetProwlerLocus");
            arrlst.Add("dtxx", "ALL");




        }

    }
}