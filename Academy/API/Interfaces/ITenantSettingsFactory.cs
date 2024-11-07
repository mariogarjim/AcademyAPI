using API.Settings;
using Microsoft.Extensions.Options;

namespace API.Interfaces
{
    public interface ITenantSettingsFactory
    {
        public IOptions<TenantSettings> GetTenantSettings();
    }
}
