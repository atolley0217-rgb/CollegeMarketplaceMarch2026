using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace CollegeMarketplaceMarch2026.Models
{
    public class ListingModel
    {
        public Guid ListingID { get; set; }
        public Guid UserID { get; set; }
        public string ItemName { get; set; }
        public string ItemDesc { get; set; }
        public string ItemType { get; set; }
        public decimal SellPrice { get; set; }
        public DateTime DateListed { get; set; }
        public List<ListingImage> ListingImages { get; set; } = new List<ListingImage>();
    }
}