using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Laptop.Models
{
    public class client_bill
    {
        public int ID { get; set; }
        public int ID_Bill { get; set; }
        public int ID_Pro_Co { get; set; }
        public int Quantity { get; set; }
        public string Cus_Name { get; set; }
        public string Bill_Add { get; set; }
        public string Cus_Email { get; set; }
        public string Cus_Gender { get; set; }
        public string Cus_Phone { get; set; }
        public string Pro_Name { get; set; }
        public string Pro_Brand { get; set; }
        public decimal Pro_Price { get; set; }
        public string Pro_Color { get; set; }
        public int Pro_ID { get; set; }
        public string Pro_Image { get; set; }
    }
}