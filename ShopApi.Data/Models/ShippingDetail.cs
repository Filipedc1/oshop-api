using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApi.Data.Models
{
    public class ShippingDetail
    {
        public int ShippingDetailId     { get; set; }
        public string Name              { get; set; }
        public string AddressLine1      { get; set; }
        public string AddressLine2         { get; set; }
        public string City                 { get; set; }
    }
}
