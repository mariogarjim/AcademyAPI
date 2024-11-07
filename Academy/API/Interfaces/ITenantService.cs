using API.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface ITenantService
    {
        public Tenant? GetTenant();
    }
}
