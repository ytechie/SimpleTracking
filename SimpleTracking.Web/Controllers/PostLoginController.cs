using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace SimpleTracking.Web.Controllers
{
    public class PostLoginController : Controller
    {
        //
        // GET: /PostLogin/

        public ActionResult Index()
        {
            var identity = this.HttpContext.User.Identity;
            var identity2 = System.Threading.Thread.CurrentPrincipal.Identity;
            var name = System.Threading.Thread.CurrentPrincipal.Identity.Name;

            var ca = HttpContext.User.Identity as ClaimsIdentity;
            var ca2 = new List<Claim>();
            foreach (var claim in ca.Claims)
            {
                ca2.Add(claim);
            }

            return View();
        }

    }
}
