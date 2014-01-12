using System;
using System.Linq;
using System.Globalization;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using HalloDal.Models;
using HalloDal.Migrations;
using System.Data.Entity;
using Hallo.Users;
using System.Threading;

namespace Hallo {
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication {
        protected void Application_Start() {
            log4net.Config.XmlConfigurator.Configure();

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            HalloContext context = new HalloContext();
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<HalloContext, HalloDal.Migrations.Configuration>());
            Application["FrontpageLinks"] = context.FrontpageLinks.OrderBy(x => x.Order).ToList();
        }

        public void Session_Start() {
            Session["FrontpageLinks"] = Application["FrontpageLinks"];
        }

        public void Request_Start() {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("da-DK");
        }

    }
}