﻿using System;
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
        public ActionResult Index(String query)
        {
            return View(new PageDataModel(query));
        }
    }
}