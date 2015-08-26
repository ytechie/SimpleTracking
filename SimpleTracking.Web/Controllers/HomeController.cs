using System.Web.Mvc;

namespace SimpleTracking.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Track(string trackingNumber)
        {
            return RedirectToAction("Html", "Track", new { id = trackingNumber });
        }
    }
}
