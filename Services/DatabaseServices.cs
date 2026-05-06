using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Mvc;
using CollegeMarketplaceMarch2026.Models;   // adjust to your namespace

namespace CollegeMarketplaceMarch2026.Services
{
    public class DatabaseServices
    {
        private readonly string _connectionString;

        public DatabaseServices()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["CollegeMarketplaceDB"].ConnectionString;
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

                string query = "SELECT UserID, FirstName, LastName, Email, PhoneNumber, Password, IsAdmin FROM Users";

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
                            Password = reader.GetString(5),
                            IsAdmin = reader.GetBoolean(6)
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
                                Password = reader.GetString(5),
                                IsAdmin = reader.GetBoolean(6)
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
                string query = @"INSERT INTO Users (UserID, FirstName, LastName, Email, PhoneNumber, Password, IsAdmin)
                         VALUES (@UserID, @FirstName, @LastName, @Email, @PhoneNumber, @Password, @IsAdmin)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@UserID", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
                    cmd.Parameters.Add("@FirstName", SqlDbType.VarChar, 50).Value = user.FirstName;
                    cmd.Parameters.Add("@LastName", SqlDbType.VarChar, 50).Value = user.LastName;
                    cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value = user.Email;
                    cmd.Parameters.Add("@PhoneNumber", SqlDbType.VarChar, 20).Value = user.PhoneNumber;
                    cmd.Parameters.Add("@Password", SqlDbType.VarChar, 255).Value = user.Password;
                    cmd.Parameters.Add("@IsAdmin", SqlDbType.Bit).Value = false;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // ---------------------------
        // Check for User by Email and Pass
        // ---------------------------
        public UserModel GetUserByEmailAndPassword(string email, string pass)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string query = @"SELECT * FROM Users
                         WHERE Email = @Email AND Password = @Password";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", pass);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new UserModel
                            {
                                UserID = (Guid)reader["UserID"],
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString(),
                                PhoneNumber = reader["PhoneNumber"]?.ToString(),
                                Password = reader["Password"].ToString(),
                                IsAdmin = (bool)reader["IsAdmin"]
                            };
                        }
                    }
                }
            }

            return null;
        }

        // ---------------------------
        // Update User
        // ---------------------------
        public async Task UpdateUser(Guid userId, bool isAdmin)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                string query = @"UPDATE Users
                         SET IsAdmin = @IsAdmin
                         WHERE UserID = @UserID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@IsAdmin", isAdmin);

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
                            ListingID = (Guid)reader["ListingID"],
                            UserID = (Guid)reader["UserID"],
                            ItemName = reader["ItemName"]?.ToString(),
                            ItemDesc = reader["ItemDesc"]?.ToString(),
                            ItemType = reader["ItemType"]?.ToString(),
                            SellPrice = Convert.ToDecimal(reader["SellPrice"]),
                            DateListed = Convert.ToDateTime(reader["DateListed"])
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
                                ListingID = (Guid)reader["ListingID"],
                                UserID = (Guid)reader["UserID"],
                                ItemName = reader["ItemName"]?.ToString(),
                                ItemDesc = reader["ItemDesc"]?.ToString(),
                                ItemType = reader["ItemType"]?.ToString(),
                                SellPrice = Convert.ToDecimal(reader["SellPrice"]),
                                DateListed = Convert.ToDateTime(reader["DateListed"])
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
                                ListingID = (Guid)reader["ListingID"],
                                UserID = (Guid)reader["UserID"],
                                ItemName = reader["ItemName"]?.ToString(),
                                ItemDesc = reader["ItemDesc"]?.ToString(),
                                ItemType = reader["ItemType"]?.ToString(),
                                SellPrice = Convert.ToDecimal(reader["SellPrice"]),
                                DateListed = Convert.ToDateTime(reader["DateListed"])
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

                string query = @"SELECT L.ListingID, L.UserID, L.ItemName, L.ItemDesc, L.ItemType, L.SellPrice, L.DateListed
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
                                ListingID = (Guid)reader["ListingID"],
                                UserID = (Guid)reader["UserID"],
                                ItemName = reader["ItemName"]?.ToString(),
                                ItemDesc = reader["ItemDesc"]?.ToString(),
                                ItemType = reader["ItemType"]?.ToString(),
                                SellPrice = Convert.ToDecimal(reader["SellPrice"]),
                                DateListed = Convert.ToDateTime(reader["DateListed"])
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
                    cmd.Parameters.AddWithValue("@ListingID", Guid.NewGuid());
                    cmd.Parameters.AddWithValue("@UserID", listing.UserID);
                    cmd.Parameters.AddWithValue("@ItemName", listing.ItemName);
                    cmd.Parameters.AddWithValue("@ItemDesc", listing.ItemDesc);
                    cmd.Parameters.AddWithValue("@ItemType", listing.ItemType);
                    cmd.Parameters.AddWithValue("@SellPrice", listing.SellPrice);
                    cmd.Parameters.AddWithValue("@DateListed", DateTime.Now);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task UpdateListing(ListingModel listing)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                string query = @"UPDATE Listing SET ItemName = @ItemName, ItemDesc = @ItemDesc, 
                                        ItemType = @ItemType, SellPrice = @SellPrice 
                                        WHERE ListingId = @ListingID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ListingID", listing.ListingID);
                    cmd.Parameters.AddWithValue("@ItemName", listing.ItemName);
                    cmd.Parameters.AddWithValue("@ItemDesc", listing.ItemDesc);
                    cmd.Parameters.AddWithValue("@ItemType", listing.ItemType);
                    cmd.Parameters.AddWithValue("@SellPrice", listing.SellPrice);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task DeleteListing(Guid listingId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                string query = @"DELETE FROM Listing WHERE ListingId = @ListingID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ListingID", listingId);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        #endregion

        #region Transactions
        public async Task CreateTransaction(Guid listingId, Guid userId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                string query = @"INSERT INTO Transactions (TransactionID, UserId, ListingId, TransactionDate)
                         VALUES (@TransactionID, @UserId, @ListingId, @TransactionDate)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TransactionID", Guid.NewGuid());
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@ListingId", listingId);
                    cmd.Parameters.AddWithValue("@TransactionDate", DateTime.Now);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        #endregion
    }
}