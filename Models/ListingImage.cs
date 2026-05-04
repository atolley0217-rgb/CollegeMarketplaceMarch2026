using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CollegeMarketplaceMarch2026.Models
{
    public class ListingImage
    {
        public Guid ImageId { get; set; }
        public Guid ListingId { get; set; }
        public byte[] ImageData { get; set; }
        public string ContentType { get; set; }
        public int ImageOrder { get; set; }
    }
}