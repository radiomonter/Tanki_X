namespace YamlDotNet.Core
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    internal class SimpleKey
    {
        private readonly Cursor cursor;

        public SimpleKey()
        {
            this.cursor = new Cursor();
        }

        public SimpleKey(bool isPossible, bool isRequired, int tokenNumber, Cursor cursor)
        {
            this.IsPossible = isPossible;
            this.IsRequired = isRequired;
            this.TokenNumber = tokenNumber;
            this.cursor = new Cursor(cursor);
        }

        public bool IsPossible { get; set; }

        public bool IsRequired { get; private set; }

        public int TokenNumber { get; private set; }

        public int Index =>
            this.cursor.Index;

        public int Line =>
            this.cursor.Line;

        public int LineOffset =>
            this.cursor.LineOffset;

        public YamlDotNet.Core.Mark Mark =>
            this.cursor.Mark();
    }
}

