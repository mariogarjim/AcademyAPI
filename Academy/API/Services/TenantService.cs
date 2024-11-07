using API.Exceptions;
using API.Interfaces;
using API.Settings;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace API.Services
{
    public class TenantService : ITenantService
    {
        private readonly TenantSettings _tenantSettings;
        private Tenant _currentTenant = default!;

        public TenantService(IOptions<TenantSettings> tenantSettings, IHttpContextAccessor contextAccessor)
        {
            _tenantSettings = tenantSettings.Value;
            HttpContext? _httpContext = contextAccessor.HttpContext;

            if (_httpContext != null)
            {
                if (_httpContext.Request.Headers.TryGetValue("tenant", out var tenantId))
                {
                    SetTenant(tenantId);
                }
                else
                {
                    throw new TenantNullException();
                }
            }
        }

        private void SetTenant(string? tenantId)
        {
            try
            {
                _currentTenant = _tenantSettings.Tenants.First(tenants => tenants.TID == tenantId);
            }
            catch (Exception)
            {
                throw new KeyNotFoundException("Invalid Tenant!");
            }

        }

        public Tenant GetTenant()
        {
            return _currentTenant;
        }

    }
}
