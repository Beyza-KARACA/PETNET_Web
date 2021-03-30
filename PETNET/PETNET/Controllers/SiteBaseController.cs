using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace PETNET.Controllers
{
    public class SiteBaseController : Controller
    {
        // GET: SiteBase
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            if (Request.QueryString["language"] != null)
            {
                if (Request.QueryString["language"] == "tr")
                {
                    Session["language"] = "tr";
                    Session["culture"] = "TR";
                }
                else if (Request.QueryString["language"] == "en")
                {
                    Session["language"] = "en";
                    Session["culture"] = "US";
                }
                else if (Request.QueryString["language"] == "fr")
                {
                    Session["language"] = "fr";
                    Session["culture"] = "FR";
                }
            }

            var language = Session["language"] ?? "tr";
            var culture = Session["culture"] ?? "TR";

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo($"{language}-{culture}");
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo($"{language}-{culture}");
            return base.BeginExecuteCore(callback, state);
        }
    }
}