using Azure.Data.Tables;

namespace API.Interfaces
{
    public interface IDBFactory
    {
        public Task<TableClient> GetTableClient();
    }
}
