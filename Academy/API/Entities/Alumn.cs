using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Azure;
using Azure.Data.Tables;

namespace API.Entities
{
    public record Alumn : ITableEntity
    {
        // Represents TenantId which represents University
        public string PartitionKey { get; set; } = default!;
        // Represents NIF/PESEL/DNI
        public string RowKey { get; set; } = default!;
        public string Name { get; set; } = String.Empty;
        public string Surname { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public DateTimeOffset DateOfBirth { get; set; }
        public DateTimeOffset StartingDay { get; set; }
        public string Course { get; set; } = String.Empty;
        public string Image { get; set; } = String.Empty;
        public string Address { get; set; } = String.Empty;
        public string Country { get; set; } = String.Empty;
        public string Degree { get; set; } = String.Empty;
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
