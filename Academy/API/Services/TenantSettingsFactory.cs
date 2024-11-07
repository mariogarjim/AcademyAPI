using API.Interfaces;
using API.Settings;
using Microsoft.Extensions.Options;

namespace API.Services
{
    public class TenantSettingsFactory : ITenantSettingsFactory
    {
        private readonly IConfiguration _configuration;


        public TenantSettingsFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public IOptions<TenantSettings> GetTenantSettings()
        {
            IOptions<TenantSettings> _tenantSettings;
            var tenantsThing = _configuration.GetSection("Tenants");
            var tenants = tenantsThing.Get<List<Tenant>>();
            
            TenantSettings ts = new()
            {
                Tenants = tenants
            };
            _tenantSettings = Options.Create(ts);

            return _tenantSettings;

        }
    }
}
