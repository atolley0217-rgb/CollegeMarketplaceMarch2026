using CollegeMarketplaceMarch2026.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CollegeMarketplaceMarch2026.Controllers
{
    public class HomeController : Controller
    {
        public List<UserModel> Users = new List<UserModel>();
        public List<UserModel> Listings = new List<UserModel>();
        public List<UserModel> Orders = new List<UserModel>();
        public List<UserModel> UserListings = new List<UserModel>();

        public UserModel SelectedUser = new UserModel();
        public ListingModel SelectedListing = new ListingModel();

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
            ViewBag.SelectedListing = SelectedListing;

            return View();
        }
        public ActionResult ViewListing()
        {
            ViewBag.Message = "Your new listing page.";
            ViewBag.SelectedListing = SelectedListing;

            return View();
        }
        public ActionResult AdminPortal()
        {
            ViewBag.Message = "Your application admin portal page.";

            return View();
        }
    }
}