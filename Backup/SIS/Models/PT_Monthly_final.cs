using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIS.Models
{
    using System;
    using System.Collections.Generic;
    public partial class PT_Monthly_final
    {
        public int Year_PT { get; set; }
        public int Month_PT { get; set; }
        public Nullable<double> totalPT { get; set; }
        public Nullable<int> no_item { get; set; }
    }
}