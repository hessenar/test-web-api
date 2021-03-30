using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Order
    {
        public string SystemType { get; set; }
        public bool Processed { get; set; }
        public string SourceOrder { get; set; }
        public long? OrderNumber { get; set; }
        public long Id { get; set; }
        public string ConvertedOrder { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
