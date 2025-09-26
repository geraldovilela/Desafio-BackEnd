using System;

namespace RentalApp.Messaging.Config.CustomExceptions
{
    public class MessagePublishingException : Exception
    {
        public MessagePublishingException(string message) : base(message) { }
        public MessagePublishingException(string message, Exception innerException) : base(message, innerException) { }
    }

}
