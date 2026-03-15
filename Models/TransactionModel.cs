using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CollegeMarketplaceMarch2026.Models
{
    public class TransactionModel
    {
        public Guid TransactionID { get; set; }
        public Guid UserID { get; set; }
        public Guid ListingID { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}