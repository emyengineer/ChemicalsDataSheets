using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChemicalsSheets.Controllers
{
    public class ChemicalProductsController : Controller
    {
        // GET: ChemicalProducts
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Save()
        {
            return View();
        }
    }
}