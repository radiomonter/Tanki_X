namespace YamlDotNet.Core
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    internal class StringLookAheadBuffer : ILookAheadBuffer
    {
        private readonly string value;

        public StringLookAheadBuffer(string value)
        {
            this.value = value;
        }

        private bool IsOutside(int index) => 
            index >= this.value.Length;

        public char Peek(int offset)
        {
            int index = this.Position + offset;
            return (!this.IsOutside(index) ? this.value[index] : '\0');
        }

        public void Skip(int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length", "The length must be positive.");
            }
            this.Position += length;
        }

        public int Position { get; private set; }

        public int Length =>
            this.value.Length;

        public bool EndOfInput =>
            this.IsOutside(this.Position);
    }
}

