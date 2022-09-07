using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Gnostice.StarDocsSDK;

namespace OrgProject
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static StarDocs starDocs;

        protected void Application_Start()
        {

            // applay authorize in hole controller
            GlobalFilters.Filters.Add(new AuthorizeAttribute());



            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            //////setup connection StarDocs
            //try
            //{
            //    starDocs = new StarDocs(
            //               new ConnectionInfo(
            //           new Uri("https://api.gnostice.com/stardocs/v1"),
            //           "203b5135b4cd49ef8396d6c14aae8c54",
            //           "c796ad8aadf0490dafc6a772b2ef3884")
            //           ); starDocs.Auth.loginApp();
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

            HttpCookie cookie = HttpContext.Current.Request.Cookies["Language"];
            if (cookie != null && cookie.Value != null)
            {
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cookie.Value);
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(cookie.Value);
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en");
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
            }

        }


    }
}
