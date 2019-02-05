using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Psychotype_HSE.Models;
using System.Diagnostics;

namespace Psychotype.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(PageDataModel model)
        {
            if (model != null)
                Debug.WriteLine("Index: ", model.Id);
            else
                Debug.WriteLine("Index: null");
            return View(model);
        }

        [HttpPost]
        public ActionResult MostPopularWords(PageDataModel mod)
        {
            PageDataModel model = null;
            if (mod != null && mod.Id != "")
            {
                model = new PageDataModel(mod.Id);
            }
            
            return RedirectToAction("Index", model);
        }
    }
}