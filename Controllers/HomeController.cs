using CollegeMarketplaceMarch2026.Models;
using CollegeMarketplaceMarch2026.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace CollegeMarketplaceMarch2026.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseServices _db = new DatabaseServices();

        public List<UserModel> Users = new List<UserModel>();
        public List<ListingModel> Listings = new List<ListingModel>();
        public List<ListingModel> Orders = new List<ListingModel>();
        public List<ListingModel> UserListings = new List<ListingModel>();

        public UserModel SelectedUser = new UserModel();
        public ListingModel SelectedListing = new ListingModel();

        private Guid GetCurrentUserId()
        {
            if (Session["UserID"] != null)
            {
                return (Guid)Session["UserID"];
            }

            return Guid.Empty;
        }

        public ActionResult Index()
        {
            Listings = _db.GetAllListings();
            return View(Listings);
        }

        public ActionResult ListingsAndOrders()
        {
            Guid userId = GetCurrentUserId();
            if (userId != Guid.Empty)
            {
                var model = new ListingsAndOrdersViewModel
                {
                    Listings = _db.GetListingsByUserId(userId),
                    Orders = _db.GetTransactionListingsByUserId(userId)
                };
                return View(model);
            }
            else
            {
                TempData["ErrorMessage"] = "You must be logged in to view this page.";
                return RedirectToAction("Index");
            }
        }

        public ActionResult NewListing()
        {
            ViewBag.Message = "Your new listing page.";

            return View();
        }

        [HttpPost]
        public ActionResult NewListing(ListingModel model, IEnumerable<HttpPostedFileBase> ListingImages)
        {
            if (ModelState.IsValid)
            {
                model.ListingID = Guid.NewGuid();
                model.UserID = GetCurrentUserId();
                model.DateListed = DateTime.Now;

                if (ListingImages != null)
                {
                    model.ListingImages = new List<byte[]>();

                    foreach (var file in ListingImages)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            using (var binaryReader = new BinaryReader(file.InputStream))
                            {
                                var imageBytes = binaryReader.ReadBytes(file.ContentLength);
                                model.ListingImages.Add(imageBytes);
                            }
                        }
                    }
                }

                _db.CreateListing(model);

                TempData["SuccessMessage"] = "Listing created successfully!";

                return RedirectToAction("ListingsAndOrders");
            }

            return View(model);
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