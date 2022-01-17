using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Laptop.Models
{
    public class GioHang
    {
        
        public string Image { get; set; }
        public string Name  { get; set; }
        public string Color  { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Order_Price { get; set; }
        public int ID { get; set; }
        public int ID_pro { get; set; }
        public int ID_p { get; set; }
        public int ID_cus { get; set; }
        public int Quantity { get; set; }
        public int Quantity_Purchased { get; set; }
        public int Total_Price { get; set; }
    }
}