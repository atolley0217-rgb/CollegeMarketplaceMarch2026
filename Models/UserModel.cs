using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CollegeMarketplaceMarch2026.Models
{
    public class UserModel
    {
        public Guid UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
    }
}