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
    
    public partial class Purchase_Transactions_vw
    {
        public int pt_id { get; set; }
        public string purchase_name { get; set; }
        public Nullable<System.DateTime> expected_date { get; set; }
        public Nullable<System.DateTime> purchase_date { get; set; }
        public int product_purchase_id { get; set; }
        public string status { get; set; }
        public Nullable<int> product_info_id { get; set; }
        public Nullable<double> purchase_qty { get; set; }
        public Nullable<double> purchase_new_price { get; set; }
        public Nullable<bool> active_flag { get; set; }
        public string product_code { get; set; }
        public string product_desc { get; set; }
        public string category_name { get; set; }
        public string sub_category_name { get; set; }
    }
}