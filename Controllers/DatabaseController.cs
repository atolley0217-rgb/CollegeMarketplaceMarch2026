using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CollegeMarketplaceMarch2026.Controllers
{
    public class DatabaseController : Controller
    {
        private readonly DatabaseServices _db = new DatabaseServices();

        public ActionResult Users()
        {
            var users = _db.GetAllUsers();
            return View(users);
        }

        public ActionResult Listings()
        {
            var listings = _db.GetAllListings();
            return View(listings);
        }
    }
}