using API.Exceptions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;

namespace UnitTest
{
    public class TenantNullExceptionTests
    {
        private const string Message = "Message";
        private const int EntityId = 1;

        [Fact]
        public void WhenNoArgs_ThenSetMessageToDefault()
        {
            TenantNullException? ex =
                new();

            // Save the full ToString() value, including the exception message and stack trace.
            var exceptionToString = ex.ToString();

            // Round-trip the exception: Serialize and de-serialize with a BinaryFormatter
            using (var stream = new MemoryStream())
            {
            // "Save" object state
            JsonSerializer.Serialize(stream, ex);

            // Re-use the same stream for de-serialization
            stream.Seek(0, 0);

            // Replace the original exception with de-serialized one
            ex = JsonSerializer.Deserialize<TenantNullException>(stream);
            }

            // Double-check that the exception message and stack trace (owned by the base Exception) are preserved
            Assert.Equal(exceptionToString, ex?.ToString());
        }

        [Fact]
        public void WhenMessageSpecified_ThenSetMessage()
        {
            TenantNullException ex =
                new(Message);

            Assert.Equal(Message, ex.Message);
        }

        [Fact]
        public void WhenMessageAndInnerExSpecified_ThenSetMessageAndInnerEx()
        {
            var innerException = new Exception();
            TenantNullException ex =
                new(Message, innerException);

            Assert.Equal(Message, ex.Message);
            Assert.Same(innerException, ex.InnerException);
        }

    }
}
