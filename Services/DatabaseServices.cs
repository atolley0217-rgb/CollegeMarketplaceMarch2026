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
            UserModel user = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string query = @"SELECT UserID, FirstName, LastName, Email, PhoneNumber, Password
                         FROM Users
                         WHERE UserID = @UserID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new UserModel
                            {
                                UserID = reader.GetGuid(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                Email = reader.GetString(3),
                                PhoneNumber = reader.GetString(4),
                                Password = reader.GetString(5)
                            };
                        }
                    }
                }
            }

            return user;
        }

        // ---------------------------
        // Create New User
        // ---------------------------
        public async Task CreateUser(UserModel user)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                string query = @"INSERT INTO Users (UserID, FirstName, LastName, Email, PhoneNumber, Password)
                         VALUES (@UserID, @FirstName, @LastName, @Email, @PhoneNumber, @Password)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", user.UserID);
                    cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", user.LastName);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Password", user.Password);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // ---------------------------
        // Check for User by Email and Pass
        // ---------------------------
        public bool CheckLogin(string email, string pass)
        {
            bool validLogin = false;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string query = @"SELECT COUNT(*) FROM Users
                         WHERE Email = @Email AND Password = @Password";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", pass);

                    int count = (int)cmd.ExecuteScalar();
                    validLogin = count > 0;
                }
            }

            return validLogin;
        }

        // ---------------------------
        // Update User
        // ---------------------------
        public async Task UpdateUser(UserModel user)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                string query = @"UPDATE Users
                         SET FirstName = @FirstName,
                             LastName = @LastName,
                             Email = @Email,
                             PhoneNumber = @PhoneNumber,
                             Password = @Password
                         WHERE UserID = @UserID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", user.UserID);
                    cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", user.LastName);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Password", user.Password);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // ---------------------------
        // Delete User
        // ---------------------------
        public async Task DeleteUser(Guid userId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                string query = @"DELETE FROM Users WHERE UserID = @UserID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
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
            ListingModel listing = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string query = @"SELECT ListingID, UserID, ItemName, ItemDesc, ItemType, SellPrice, DateListed
                         FROM Listing
                         WHERE ListingID = @ListingID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ListingID", listingId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            listing = new ListingModel
                            {
                                ListingID = reader.GetGuid(0),
                                UserID = reader.GetGuid(1),
                                ItemName = reader.GetString(2),
                                ItemDesc = reader.GetString(3),
                                ItemType = reader.GetString(4),
                                SellPrice = reader.GetDecimal(5),
                                DateListed = reader.GetDateTime(6)
                            };
                        }
                    }
                }
            }

            return listing;
        }

        // ---------------------------
        // Get Listings by User ID
        // ---------------------------
        public List<ListingModel> GetListingsByUserId(Guid userID)
        {
            var listings = new List<ListingModel>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string query = @"SELECT ListingID, UserID, ItemName, ItemDesc, ItemType, SellPrice, DateListed
                         FROM Listing
                         WHERE UserID = @UserID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);

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
            }

            return listings;
        }

        // ---------------------------
        // Get Transaction Listings by User ID
        // ---------------------------
        public List<ListingModel> GetTransactionListingsByUserId(Guid userID)
        {
            var listings = new List<ListingModel>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string query = @"
            SELECT L.ListingID, L.UserID, L.ItemName, L.ItemDesc, L.ItemType, L.SellPrice, L.DateListed
            FROM Transactions T
            INNER JOIN Listing L ON T.ListingID = L.ListingID
            WHERE T.UserID = @UserID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);

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
            }

            return listings;
        }
        // ---------------------------
        // Create New Transaction
        // ---------------------------
        public async Task CreateListing(ListingModel listing)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                string query = @"INSERT INTO Listing (ListingID, UserID, ItemName, ItemDesc, ItemType, SellPrice, DateListed)
                         VALUES (@ListingID, @UserID, @ItemName, @ItemDesc, @ItemType, @SellPrice, @DateListed)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ListingID", listing.ListingID);
                    cmd.Parameters.AddWithValue("@UserID", listing.UserID);
                    cmd.Parameters.AddWithValue("@ItemName", listing.ItemName);
                    cmd.Parameters.AddWithValue("@ItemDesc", listing.ItemDesc);
                    cmd.Parameters.AddWithValue("@ItemType", listing.ItemType);
                    cmd.Parameters.AddWithValue("@SellPrice", listing.SellPrice);
                    cmd.Parameters.AddWithValue("@DateListed", listing.DateListed);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        #endregion
    }
}