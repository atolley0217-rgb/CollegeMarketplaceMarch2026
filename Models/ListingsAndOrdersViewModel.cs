using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CollegeMarketplaceMarch2026.Models
{
    public class ListingsAndOrdersViewModel
    {
        public List<ListingModel> Listings { get; set; }
        public List<ListingModel> Orders { get; set; }
    }
}