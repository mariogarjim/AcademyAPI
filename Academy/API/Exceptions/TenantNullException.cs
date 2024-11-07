using System.Runtime.Serialization;

namespace API.Exceptions
{
    /*
     * This Exception refers to the fact that there is no
     * tenant provided in the header of the received request.
     */
    [Serializable]
    public class TenantNullException : Exception
    {
        public TenantNullException()
        {
        }

        public TenantNullException(string message) 
            : base(message)
        {
        }

        public TenantNullException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected TenantNullException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
