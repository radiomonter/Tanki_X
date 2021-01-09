namespace Platform.Library.ClientDataStructures.API
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface IPriorityQueue<T> : ICollection<T>, IEnumerable<T>, IEnumerable
    {
        T Dequeue();
        void Enqueue(T item);
        T Peek();
    }
}

