using System;
using System.Runtime.Serialization;

namespace OrdersService.Controllers
{
    [Serializable]
    public class OrderServiceException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public OrderServiceException()
        {
        }

        public OrderServiceException(string message) : base(message)
        {
        }

        public OrderServiceException(string message, Exception inner) : base(message, inner)
        {
        }

        protected OrderServiceException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}