namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using System.Collections.Generic;

    public class SortedLinkedList<T> : LinkedList<T> where T: IComparable
    {
        public void AddOrdered(T value)
        {
            for (LinkedListNode<T> node = base.First; node != null; node = node.Next)
            {
                if (node.Value.CompareTo(value) < 0)
                {
                    base.AddBefore(node, new LinkedListNode<T>(value));
                    return;
                }
            }
            base.AddLast(new LinkedListNode<T>(value));
        }
    }
}

