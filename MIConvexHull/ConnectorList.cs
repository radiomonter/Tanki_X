namespace MIConvexHull
{
    using System;

    internal sealed class ConnectorList
    {
        private FaceConnector first;
        private FaceConnector last;

        public void Add(FaceConnector element)
        {
            if (this.last != null)
            {
                this.last.Next = element;
            }
            element.Previous = this.last;
            this.last = element;
            this.first ??= element;
        }

        private void AddFirst(FaceConnector connector)
        {
            this.first.Previous = connector;
            connector.Next = this.first;
            this.first = connector;
        }

        public void Remove(FaceConnector connector)
        {
            if (connector.Previous != null)
            {
                connector.Previous.Next = connector.Next;
            }
            else if (connector.Previous == null)
            {
                this.first = connector.Next;
            }
            if (connector.Next != null)
            {
                connector.Next.Previous = connector.Previous;
            }
            else if (connector.Next == null)
            {
                this.last = connector.Previous;
            }
            connector.Next = null;
            connector.Previous = null;
        }

        public FaceConnector First =>
            this.first;
    }
}

