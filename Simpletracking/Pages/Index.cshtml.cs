using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Simpletracking.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {

        }

        public ActionResult Track(string trackingNumber)
        {
            return RedirectToAction("Html", "Track", new { id = trackingNumber });
        }
    }
}
