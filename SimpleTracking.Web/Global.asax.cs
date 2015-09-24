using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SimpleTracking.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        internal static string BingMapsKey = "AlAVTbe0GdeAnzr6Vmxo7N3-uXmOy28QhZ8T2n3F5UHaMFRXpYrnuKcgP4clEe75";

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ControllerBuilder.Current.SetControllerFactory(new StructureMapControllerFactory());

            ViewEngines.Engines.Add(new RazorViewEngine());

            IocConfiguration.Configure();
        }
    }
}