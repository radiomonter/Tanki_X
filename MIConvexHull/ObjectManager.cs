namespace MIConvexHull
{
    using System;

    internal class ObjectManager
    {
        private readonly int Dimension;
        private ConvexHullInternal Hull;
        private int FacePoolSize;
        private int FacePoolCapacity;
        private ConvexFaceInternal[] FacePool;
        private IndexBuffer FreeFaceIndices;
        private FaceConnector ConnectorStack;
        private SimpleList<IndexBuffer> EmptyBufferStack;
        private SimpleList<DeferredFace> DeferredFaceStack;

        public ObjectManager(ConvexHullInternal hull)
        {
            this.Dimension = hull.Dimension;
            this.Hull = hull;
            this.FacePool = hull.FacePool;
            this.FacePoolSize = 0;
            this.FacePoolCapacity = hull.FacePool.Length;
            this.FreeFaceIndices = new IndexBuffer();
            this.EmptyBufferStack = new SimpleList<IndexBuffer>();
            this.DeferredFaceStack = new SimpleList<DeferredFace>();
        }

        private int CreateFace()
        {
            int facePoolSize = this.FacePoolSize;
            ConvexFaceInternal internal2 = new ConvexFaceInternal(this.Dimension, facePoolSize, this.GetVertexBuffer());
            this.FacePoolSize++;
            if (this.FacePoolSize > this.FacePoolCapacity)
            {
                this.ReallocateFacePool();
            }
            this.FacePool[facePoolSize] = internal2;
            return facePoolSize;
        }

        public void DepositConnector(FaceConnector connector)
        {
            if (this.ConnectorStack == null)
            {
                connector.Next = null;
                this.ConnectorStack = connector;
            }
            else
            {
                connector.Next = this.ConnectorStack;
                this.ConnectorStack = connector;
            }
        }

        public void DepositDeferredFace(DeferredFace face)
        {
            this.DeferredFaceStack.Push(face);
        }

        public void DepositFace(int faceIndex)
        {
            int[] adjacentFaces = this.FacePool[faceIndex].AdjacentFaces;
            for (int i = 0; i < adjacentFaces.Length; i++)
            {
                adjacentFaces[i] = -1;
            }
            this.FreeFaceIndices.Push(faceIndex);
        }

        public void DepositVertexBuffer(IndexBuffer buffer)
        {
            buffer.Clear();
            this.EmptyBufferStack.Push(buffer);
        }

        public FaceConnector GetConnector()
        {
            if (this.ConnectorStack == null)
            {
                return new FaceConnector(this.Dimension);
            }
            FaceConnector connectorStack = this.ConnectorStack;
            this.ConnectorStack = this.ConnectorStack.Next;
            connectorStack.Next = null;
            return connectorStack;
        }

        public DeferredFace GetDeferredFace() => 
            (this.DeferredFaceStack.Count == 0) ? new DeferredFace() : this.DeferredFaceStack.Pop();

        public int GetFace() => 
            (this.FreeFaceIndices.Count <= 0) ? this.CreateFace() : this.FreeFaceIndices.Pop();

        public IndexBuffer GetVertexBuffer() => 
            (this.EmptyBufferStack.Count == 0) ? new IndexBuffer() : this.EmptyBufferStack.Pop();

        private void ReallocateFacePool()
        {
            ConvexFaceInternal[] destinationArray = new ConvexFaceInternal[2 * this.FacePoolCapacity];
            bool[] dst = new bool[2 * this.FacePoolCapacity];
            Array.Copy(this.FacePool, destinationArray, this.FacePoolCapacity);
            Buffer.BlockCopy(this.Hull.AffectedFaceFlags, 0, dst, 0, this.FacePoolCapacity);
            this.FacePoolCapacity = 2 * this.FacePoolCapacity;
            this.Hull.FacePool = destinationArray;
            this.FacePool = destinationArray;
            this.Hull.AffectedFaceFlags = dst;
        }
    }
}

