//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SIS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Unit
    {
        public Unit()
        {
            this.Product_Info = new HashSet<Product_Info>();
        }
    
        public int unit_id { get; set; }
        public string unit_code { get; set; }
        public string unit_name { get; set; }
        public string created_by { get; set; }
        public Nullable<System.DateTime> created_date { get; set; }
        public Nullable<bool> active_flag { get; set; }
    
        public virtual ICollection<Product_Info> Product_Info { get; set; }
    }
}