using CollegeMarketplaceMarch2026.Models;
using CollegeMarketplaceMarch2026.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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

        public ActionResult Index()
        {
            Listings = _db.GetAllListings();
            return View(Listings);
        }
        public ActionResult AdminPortal()
        {
            ViewBag.Message = "Your application admin portal page.";

            return View();
        }
        #region User

        private UserModel GetCurrentUser()
        {
            return Session["User"] as UserModel;
        }
        [HttpPost]
        public async Task<ActionResult> SignUp(UserModel model)
        {
            if (ModelState.IsValid)
            {
                model.UserID = Guid.NewGuid();
                model.IsAdmin = false;

                await _db.CreateUser(model);

                Session["User"] = model;

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<ActionResult> LogIn(UserModel model)
        {
            if (ModelState.IsValid)
            {
                UserModel user = _db.GetUserByEmailAndPassword(model.Email, model.Password);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "Invalid email or password.";
                }
                else
                {
                    Session["User"] = user;
                    TempData["SuccessMessage"] = $"Welcome back, {user.FirstName}!";
                }
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Listings
        public ActionResult ListingsAndOrders()
        {
            UserModel user = GetCurrentUser();
            if (user != null)
            {
                var model = new ListingsAndOrdersViewModel
                {
                    Listings = _db.GetListingsByUserId(user.UserID),
                    Orders = _db.GetTransactionListingsByUserId(user.UserID)
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
                UserModel user = GetCurrentUser();
                model.UserID = user.UserID;
                model.DateListed = DateTime.Now;

                if (ListingImages != null)
                {
                    model.ListingImages = new List<ListingImage>();

                    int order = 1;

                    foreach (var file in ListingImages)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            using (var binaryReader = new BinaryReader(file.InputStream))
                            {
                                var imageBytes = binaryReader.ReadBytes(file.ContentLength);

                                model.ListingImages.Add(new ListingImage
                                {
                                    ImageId = Guid.NewGuid(),
                                    ListingId = model.ListingID,
                                    ImageData = imageBytes,
                                    ContentType = file.ContentType,
                                    ImageOrder = order++
                                });
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
        
        public ActionResult EditListing(Guid id)
        {
            var model = _db.GetListingById(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult EditListing(ListingModel model, IEnumerable<HttpPostedFileBase> ListingImages)
        {
            if (ModelState.IsValid)
            {
                if (ListingImages != null)
                {
                    model.ListingImages = new List<ListingImage>();

                    int order = 1;

                    foreach (var file in ListingImages)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            using (var binaryReader = new BinaryReader(file.InputStream))
                            {
                                var imageBytes = binaryReader.ReadBytes(file.ContentLength);

                                model.ListingImages.Add(new ListingImage
                                {
                                    ImageId = Guid.NewGuid(),
                                    ListingId = model.ListingID,
                                    ImageData = imageBytes,
                                    ContentType = file.ContentType,
                                    ImageOrder = order++
                                });
                            }
                        }
                    }
                }

                _db.UpdateListing(model);

                TempData["SuccessMessage"] = "Listing updated successfully!";

                return RedirectToAction("ListingsAndOrders");
            }

            return View(model);
        }
        public ActionResult ViewListing(Guid id)
        {
            var model = _db.GetListingById(id);

            return View(model);
        }
        [HttpPost]
        public ActionResult ViewListing(ListingModel listing)
        {
            var user = GetCurrentUser();

            _db.CreateTransaction(listing.ListingID, user.UserID);

            return RedirectToAction("ListingsAndOrders");
        }
        #endregion


    }
}