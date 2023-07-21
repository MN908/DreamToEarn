using DreamToEarn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DreamToEarn.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {            
            return View();
        }

        [AllowAnonymous]
        public ActionResult Services()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Teams()
        {
            return View();
        }
    }
}