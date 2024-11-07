using API.DTO;
using API.Entities;
using Azure;

namespace API.Interfaces
{
    public interface ITableStorageService
    {
        Task<IEnumerable<GetAlumnDto>> GetAllEntityAsync(string tenant);
        Task<GetAlumnDto?> GetEntityAsyncById(string tenant, string id);
        Task<GetAlumnDto> UpsertEntityAsync(CreateAlumnDto student, string tenant);
        Task DeleteEntityAsync(string tenant, string id);
    }
}
