using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settings
{
    public class TenantSettings
    {
        public List<Tenant> Tenants { get; set; } = default!; 
    }
    public class Tenant
    {
        public string? Name { get; set; } = default!;
        public string TID { get; set; } = default!;
    }
}
