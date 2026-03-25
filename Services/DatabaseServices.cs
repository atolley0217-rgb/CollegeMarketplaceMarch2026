using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
        #endregion
    }
}