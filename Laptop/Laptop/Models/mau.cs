using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Laptop.Models
{
    public class mau
    {
        public int ID { get; set; }
        public int ID_Product { get; set; }
        public int ID_Color { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Image { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updateed_at { get; set; }
    }
}