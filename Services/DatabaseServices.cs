using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CollegeMarketplaceMarch2026.Models;   // adjust to your namespace

namespace CollegeMarketplaceMarch2026.Services
{
    public class DatabaseServices
    {
        private readonly string _connectionString;

        public DatabaseServices()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["MarketplaceDB"].ConnectionString;
        }

        #region Users
        // ---------------------------
        // Get All Users
        // ---------------------------
        public List<UserModel> GetAllUsers()
        {
            var users = new List<UserModel>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string query = "SELECT UserID, FirstName, LastName, Email, PhoneNumber, Password FROM Users";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new UserModel
                        {
                            UserID = reader.GetGuid(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            Email = reader.GetString(3),
                            PhoneNumber = reader.GetString(4),
                            Password = reader.GetString(5)
                        });
                    }
                }
            }

            return users;
        }

        // ---------------------------
        // Get User By ID
        // ---------------------------
        public UserModel GetUserById(Guid userId)
        {
            var user = new UserModel();

            return user;
        }

        // ---------------------------
        // Check for User by Email and Pass
        // ---------------------------
        public bool CheckLogin(string email, string pass)
        {
            bool validLogin = false;

            return validLogin;
        }

        // ---------------------------
        // Update User
        // ---------------------------
        public async Task UpdateUser(UserModel user)
        {
            //does not need to return value
        }

        // ---------------------------
        // Delete User
        // ---------------------------
        public async Task DeleteUser(Guid userId)
        {
            //does not need to return value
        }
        #endregion

        #region Listings
        // ---------------------------
        // Get All Listings
        // ---------------------------
        public List<ListingModel> GetAllListings()
        {
            var listings = new List<ListingModel>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string query = @"SELECT ListingID, UserID, ItemName, ItemDesc, ItemType, SellPrice, DateListed 
                             FROM Listing";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listings.Add(new ListingModel
                        {
                            ListingID = reader.GetGuid(0),
                            UserID = reader.GetGuid(1),
                            ItemName = reader.GetString(2),
                            ItemDesc = reader.GetString(3),
                            ItemType = reader.GetString(4),
                            SellPrice = reader.GetDecimal(5),
                            DateListed = reader.GetDateTime(6)
                        });
                    }
                }
            }

            return listings;
        }

        // ---------------------------
        // Get Listing by Id
        // ---------------------------
        public ListingModel GetListingById(Guid listingId)
        {
            var listing = new ListingModel();

            return listing;
        }

        // ---------------------------
        // Get Listings by User ID
        // ---------------------------
        public List<ListingModel> GetListingsByUserId(Guid userID)
        {
            var listings = new List<ListingModel>();

            return listings;
        }

        // ---------------------------
        // Get Transaction Listings by User ID
        // ---------------------------
        public List<ListingModel> GetTransactionListingsByUserId(Guid userID)
        {
            var listings = new List<ListingModel>();

            return listings;
        }
        public async Task CreateListing(ListingModel listing)
        {

        }
        #endregion
    }
}