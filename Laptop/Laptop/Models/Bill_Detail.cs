//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Laptop.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Bill_Detail
    {
        public int ID { get; set; }
        public Nullable<int> ID_Bill { get; set; }
        public Nullable<int> ID_Product_Color { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
        public Nullable<System.DateTime> updated_at { get; set; }
        public Nullable<int> rate { get; set; }
        public string comment { get; set; }
        public Nullable<System.DateTime> ratetime { get; set; }
        public Nullable<decimal> order_price { get; set; }
    
        public virtual Bill Bill { get; set; }
        public virtual Product_Color Product_Color { get; set; }
    }
}