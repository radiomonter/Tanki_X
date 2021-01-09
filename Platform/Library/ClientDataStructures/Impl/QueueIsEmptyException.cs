namespace Platform.Library.ClientDataStructures.Impl
{
    using System;

    public class QueueIsEmptyException : InvalidOperationException
    {
        public QueueIsEmptyException() : base("Queue is empty")
        {
        }
    }
}

