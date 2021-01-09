namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [StructLayout(LayoutKind.Sequential, Size=1)]
    public struct IndexRange
    {
        public bool Equals(IndexRange other) => 
            (this.Position == other.Position) && (this.Count == other.Count);

        public override bool Equals(object obj) => 
            !ReferenceEquals(null, obj) ? ((obj is IndexRange) && this.Equals((IndexRange) obj)) : false;

        public override int GetHashCode() => 
            (this.Position * 0x18d) ^ this.Count;

        public int Position { get; private set; }
        public int Count { get; private set; }
        [ProtocolTransient]
        public int EndPosition =>
            (this.Position + this.Count) - 1;
        [ProtocolTransient]
        public bool Empty =>
            this.Count == 0;
        public static IndexRange CreateFromPositionAndCount(int position, int count) => 
            new IndexRange { 
                Position = Mathf.Max(0, position),
                Count = Mathf.Max(0, count)
            };

        public static IndexRange CreateFromBeginAndEnd(int position, int endPosition)
        {
            IndexRange range = new IndexRange {
                Position = Mathf.Max(0, position)
            };
            range.Count = Mathf.Max(0, (endPosition - range.Position) + 1);
            return range;
        }

        public static IndexRange ParseString(string rangeString)
        {
            string str = rangeString.Replace("[", string.Empty).Replace("]", string.Empty);
            int index = str.IndexOf('-');
            if (index > 0)
            {
                return CreateFromBeginAndEnd(int.Parse(str.Substring(0, index)), int.Parse(str.Substring(index + 1)));
            }
            return new IndexRange();
        }

        public void CalculateDifference(IndexRange newRange, out IndexRange removedLow, out IndexRange removedHigh, out IndexRange addedLow, out IndexRange addedHigh)
        {
            removedLow = new IndexRange();
            removedHigh = new IndexRange();
            addedLow = new IndexRange();
            addedHigh = new IndexRange();
            if (newRange.Position > this.Position)
            {
                removedLow.Position = this.Position;
                removedLow.Count = Mathf.Min(newRange.Position, this.EndPosition + 1) - this.Position;
            }
            else if (newRange.Position < this.Position)
            {
                addedLow.Position = newRange.Position;
                addedLow.Count = Mathf.Min(this.Position, newRange.EndPosition + 1) - newRange.Position;
            }
            if (newRange.EndPosition < this.EndPosition)
            {
                removedHigh.Position = Mathf.Max(newRange.EndPosition + 1, this.Position);
                removedHigh.Count = (this.EndPosition - removedHigh.Position) + 1;
            }
            else if (newRange.EndPosition > this.EndPosition)
            {
                addedHigh.Position = Mathf.Max(this.EndPosition + 1, newRange.Position);
                addedHigh.Count = (newRange.EndPosition - addedHigh.Position) + 1;
            }
        }

        public static bool operator ==(IndexRange a, IndexRange b) => 
            (a.Position == b.Position) && (a.Count == b.Count);

        public static bool operator !=(IndexRange a, IndexRange b) => 
            !(a == b);

        public bool Contains(int index) => 
            (index >= this.Position) && (index <= this.EndPosition);

        public IndexRange Intersect(IndexRange range) => 
            CreateFromBeginAndEnd(Mathf.Max(this.Position, range.Position), Mathf.Min(this.EndPosition, range.EndPosition));

        public void Reset()
        {
            this.Position = 0;
            this.Count = 0;
        }

        public override string ToString() => 
            $"[{this.Position}-{this.EndPosition}]";
    }
}

