using API.Interfaces;
using Azure.Data.Tables;
using Moq;
using FluentAssertions;
using API.Entities;
using API.Services;
using Azure;
using System.Linq.Expressions;
using API;
using API.DTO;

namespace UnitTest
{
    
    public class TableStorageServiceTests
    {
        private readonly Mock<IDBFactory> dBFactory;
        
        private readonly TableStorageService service;

        private readonly Mock<TableClient> tableMock;

        private readonly String validPartitionKey = Utilities.GetFakeAlumn().PartitionKey;

        private readonly String validRowKey = Utilities.GetFakeAlumn().RowKey;

        public TableStorageServiceTests()
        {

            dBFactory = new();
            service = new TableStorageService(dBFactory.Object);
            tableMock = new();
            dBFactory.Setup(Factory => Factory.GetTableClient()).ReturnsAsync(tableMock.Object);
        }

        [Fact]
        public void GetAllEntityAsyncTest_ShouldReturnAllAlumns()
        {           

            List<Alumn> expectedAlumns = new() { 
                Utilities.GetFakeAlumn(), 
                Utilities.GetFakeAlumn(), 
                Utilities.GetFakeAlumn() 
            };
            IEnumerable<Alumn> IExpectedAlumns = expectedAlumns; 


            var page = Page<Alumn>.FromValues(expectedAlumns, default, new Mock<Response>().Object);
            var pageable = Pageable<Alumn>.FromPages(new[] { page });

            tableMock.Setup(expression: table => table.Query<Alumn>(It.IsAny<Expression<Func<Alumn, bool>>>(), default, default, default)).Returns(pageable);

            var alumns = service.GetAllEntityAsync(validPartitionKey);
            alumns.Result?.Should().BeEquivalentTo(
                   IExpectedAlumns.Select(alumn => alumn.AsGetDto())
            );

        }
        
        [Fact]
        public async Task GetEntityAsyncById_WithUnexistentTenantAndId_ShouldNotReturnTheAlumn()
        {
  
            tableMock.Setup(expression: table => table.GetEntityAsync<Alumn>(validPartitionKey, validRowKey, null, default))
                .Callback(() => throw new Azure.RequestFailedException("Invalid key"));

            var alumn = await service.GetEntityAsyncById(validPartitionKey, validRowKey);

            alumn.Should().BeNull();
        }
        
        [Fact]
        public async Task GetEntityAsyncById_WithExistentTenantAndId_ShouldReturnTheAlumn()
        {
            Mock<Response<Alumn>> response = new();
            Alumn validAlumn = Utilities.GetFakeAlumn();
            
            tableMock.Setup(expression: table => table.GetEntityAsync<Alumn>(validPartitionKey, validRowKey, null,default))
                .ReturnsAsync(response.Object);

            response.Setup(alumn => alumn.Value).Returns(validAlumn);

            var alumn = await service.GetEntityAsyncById(validPartitionKey, validRowKey);

            alumn.Should().BeEquivalentTo(validAlumn.AsGetDto());
        }

        

        [Fact]
        public async void UpsertEntityAsync_ShouldReturnAddedStudent()
        {
            Mock<Response>? response = new();
            Alumn validAlumn = Utilities.GetFakeAlumn();
            tableMock.Setup(expression: table => table.UpsertEntityAsync(validAlumn, default, default))
                .ReturnsAsync(response.Object);

            var alumn = await service.UpsertEntityAsync(validAlumn.AsCreateDto(), validPartitionKey);

            alumn.Should().BeEquivalentTo(validAlumn.AsGetDto());
        }
        
        [Fact]
        public async void DeleteEntityAsync_WithAnAlumn_ShouldVerifyTheMethodIsCalled()
        {
            Mock<Response>? response = new();
            Alumn alumn = Utilities.GetFakeAlumn();
            tableMock.Setup(expression: table => table.DeleteEntityAsync(alumn.PartitionKey, alumn.RowKey, default, default))
                .ReturnsAsync(response.Object);

            await service.DeleteEntityAsync(alumn.PartitionKey, alumn.RowKey);

            tableMock.Verify(r => r.DeleteEntityAsync(alumn.PartitionKey, alumn.RowKey, default, default));
            
        }
    } 
    
}
