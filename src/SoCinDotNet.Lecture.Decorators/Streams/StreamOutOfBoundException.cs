using System;
using System.Runtime.Serialization;

namespace SoCinDotNet.Lecture.Decorators.Streams
{
    [Serializable]
    public sealed class StreamOutOfBoundException : Exception
    {
        public StreamOutOfBoundException()
        {
        }

        public StreamOutOfBoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public StreamOutOfBoundException(string message) : base(message)
        {
        }

        public StreamOutOfBoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
