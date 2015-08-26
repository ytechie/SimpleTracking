using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure.Storage.Table;
using SimpleTracking.ShipperInterface.Geocoding;

namespace SimpleTracking.Web.Controllers
{
    public class GeocodeController : Controller
    {
        //
        // GET: /Geocode/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Upload()
        {
            foreach (string file in Request.Files)
            {
                var hpf = Request.Files[file];
                if (hpf == null)
                    continue;
                
                using (var sr = new StreamReader(hpf.InputStream))
                {
                    var batch = new List<CityRecord>();
                    var db = new GeocodeDb();

                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        var city = new CityRecord(line);
                        batch.Add(city);

                        if (batch.Count == 100)
                        {
                            db.InsertCities(batch);
                            batch.Clear();
                        }
                    }

                    if (batch.Count > 0)
                    {
                        db.InsertCities(batch);
                    }
                }
                
            }

            return RedirectToAction("Index");
        }

        
    }
}
