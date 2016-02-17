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
    
    public partial class Receipt
    {
        public Receipt()
        {
            this.Deliveries = new HashSet<Delivery>();
            this.Product_Sales = new HashSet<Product_Sales>();
        }
    
        public int receipt_id { get; set; }
        public string customer_name { get; set; }
        public string shipping_status { get; set; }
        public string payment_status { get; set; }
        public Nullable<double> discount { get; set; }
        public Nullable<int> created_by { get; set; }
        public Nullable<System.DateTime> created_date { get; set; }
        public Nullable<bool> active_flag { get; set; }
    
        public virtual ICollection<Delivery> Deliveries { get; set; }
        public virtual ICollection<Product_Sales> Product_Sales { get; set; }
        public virtual Staff_Info Staff_Info { get; set; }
    }
}
