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
    
    public partial class Expens
    {
        public int expenses_id { get; set; }
        public string expenses_type { get; set; }
        public Nullable<System.DateTime> transaction_date { get; set; }
        public Nullable<double> expenses_amount { get; set; }
        public string remarks { get; set; }
        public Nullable<int> created_by { get; set; }
        public Nullable<System.DateTime> created_date { get; set; }
        public Nullable<bool> active_flag { get; set; }
        public Nullable<int> staff_id { get; set; }
    
        public virtual Staff_Info Staff_Info { get; set; }
        public virtual Staff_Info Staff_Info1 { get; set; }
    }
}
