using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CollegeMarketplaceMarch2026.Controllers
{
    public class HomeController : Controller
    {
        //Anthony was here
        //Anthony was here to test pull
        //Twyla test
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListingsAndOrders()
        {
            ViewBag.Message = "Your application listings and orders page.";

            return View();
        }

        public ActionResult NewListing()
        {
            ViewBag.Message = "Your new listing page.";

            return View();
        }
        public ActionResult EditListing()
        {
            ViewBag.Message = "Your new listing page.";

            return View();
        }
        public ActionResult ViewListing()
        {
            ViewBag.Message = "Your new listing page.";

            return View();
        }
        public ActionResult AdminPortal()
        {
            ViewBag.Message = "Your application admin portal page.";

            return View();
        }
    }
}