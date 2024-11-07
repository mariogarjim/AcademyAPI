using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Settings
{
    public class TenantSettings
    {
        public List<Tenant> Tenants { get; set; }
    }
    public class Tenant
    {
        public string Name { get; set; }
        public string TID { get; set; }
    }
}
