namespace MIConvexHull
{
    using System;

    internal sealed class FaceList
    {
        private ConvexFaceInternal first;
        private ConvexFaceInternal last;

        public void Add(ConvexFaceInternal face)
        {
            if (face.InList)
            {
                if (this.first.VerticesBeyond.Count < face.VerticesBeyond.Count)
                {
                    this.Remove(face);
                    this.AddFirst(face);
                }
            }
            else
            {
                face.InList = true;
                if ((this.first != null) && (this.first.VerticesBeyond.Count < face.VerticesBeyond.Count))
                {
                    this.first.Previous = face;
                    face.Next = this.first;
                    this.first = face;
                }
                else
                {
                    if (this.last != null)
                    {
                        this.last.Next = face;
                    }
                    face.Previous = this.last;
                    this.last = face;
                    this.first ??= face;
                }
            }
        }

        private void AddFirst(ConvexFaceInternal face)
        {
            face.InList = true;
            this.first.Previous = face;
            face.Next = this.first;
            this.first = face;
        }

        public void Remove(ConvexFaceInternal face)
        {
            if (face.InList)
            {
                face.InList = false;
                if (face.Previous != null)
                {
                    face.Previous.Next = face.Next;
                }
                else if (face.Previous == null)
                {
                    this.first = face.Next;
                }
                if (face.Next != null)
                {
                    face.Next.Previous = face.Previous;
                }
                else if (face.Next == null)
                {
                    this.last = face.Previous;
                }
                face.Next = null;
                face.Previous = null;
            }
        }

        public ConvexFaceInternal First =>
            this.first;
    }
}

