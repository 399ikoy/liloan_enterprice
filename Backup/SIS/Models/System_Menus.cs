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
    
    public partial class System_Menus
    {
        public System_Menus()
        {
            this.System_Menu_Roles = new HashSet<System_Menu_Roles>();
        }
    
        public int system_menus_id { get; set; }
        public string sm_description { get; set; }
        public string sm_action_name { get; set; }
        public string sm_controller { get; set; }
        public Nullable<bool> active_flag { get; set; }
    
        public virtual ICollection<System_Menu_Roles> System_Menu_Roles { get; set; }
    }
}
