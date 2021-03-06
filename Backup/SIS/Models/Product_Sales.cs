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
    
    public partial class Product_Sales
    {
        public int product_sales_id { get; set; }
        public Nullable<int> product_info_id { get; set; }
        public Nullable<double> quantity_sold { get; set; }
        public Nullable<int> receipt_id { get; set; }
        public Nullable<int> sold_by { get; set; }
        public Nullable<System.DateTime> sold_date { get; set; }
        public Nullable<bool> active_flag { get; set; }
    
        public virtual Product_Info Product_Info { get; set; }
        public virtual Receipt Receipt { get; set; }
        public virtual Staff_Info Staff_Info { get; set; }
    }
}
