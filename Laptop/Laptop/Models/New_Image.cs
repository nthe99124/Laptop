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
    
    public partial class New_Image
    {
        public int ID { get; set; }
        public Nullable<int> ID_New { get; set; }
        public string Image { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
        public Nullable<System.DateTime> updated_at { get; set; }
    
        public virtual News News { get; set; }
    }
}