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
    
    public partial class Current_Order_Items
    {
        public int product_purchase_id { get; set; }
        public Nullable<int> product_info_id { get; set; }
        public Nullable<double> purchase_qty { get; set; }
        public Nullable<bool> active_flag { get; set; }
        public string product_desc { get; set; }
        public string product_code { get; set; }
        public string category_name { get; set; }
        public string sub_category_name { get; set; }
        public Nullable<double> product_orig_price { get; set; }
        public Nullable<double> product_qty { get; set; }
        public string unit_code { get; set; }
    }
}