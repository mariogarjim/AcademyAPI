using Azure.Data.Tables;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Core;
using API.Entities;
using API.Interfaces;
using API.DTO;
using System.Collections.Generic;
using Azure;
using System.Diagnostics.Metrics;

namespace API.Services
{
    public class TableStorageService : ITableStorageService
    {      
        private readonly IDBFactory _dbFactory;

        public TableStorageService(IDBFactory DBFactory)
        {
            _dbFactory = DBFactory;
        }

        // --- CRUD Methods ---

        public async Task<IEnumerable<GetAlumnDto>> GetAllEntityAsync(string tenant)
        {
            var tableClient = await _dbFactory.GetTableClient();
            return tableClient.Query<Alumn>(filter: alumn => alumn.PartitionKey.Equals(tenant)).Select(alumn => alumn.AsGetDto());
        }

        public async Task<GetAlumnDto?> GetEntityAsyncById(string tenant, string id)
        {
            var tableClient = await _dbFactory.GetTableClient();
            try
            {
                var alumn = await tableClient.GetEntityAsync<Alumn>(tenant, id);
                return alumn.Value.AsGetDto();
            }
            catch (RequestFailedException)
            {
                return null;
            }
        }

        public async Task<GetAlumnDto> UpsertEntityAsync(CreateAlumnDto student, string tenant)
        {
            var tableClient = await _dbFactory.GetTableClient();
            var addedStudent = student.AsAlumnEntity(tenant);
            await tableClient.UpsertEntityAsync(addedStudent);
            return addedStudent.AsGetDto();
        }
        public async Task DeleteEntityAsync(string tenant, string id)
        {
            var tableClient = await _dbFactory.GetTableClient();
            await tableClient.DeleteEntityAsync(tenant, id);
        }
    }
}
