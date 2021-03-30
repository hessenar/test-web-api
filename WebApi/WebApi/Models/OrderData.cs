using System;
using System.Collections.Generic;

namespace WebApi.Models
{
    public class OrderData
    {
        public string OrderNumber { get; set; }
        public List<Product> Products { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
