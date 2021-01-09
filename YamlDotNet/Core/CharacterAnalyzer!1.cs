namespace YamlDotNet.Core
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable]
    internal class CharacterAnalyzer<TBuffer> where TBuffer: ILookAheadBuffer
    {
        private readonly TBuffer buffer;

        public CharacterAnalyzer(TBuffer buffer)
        {
            this.buffer = buffer;
        }

        public int AsDigit(int offset = 0) => 
            this.buffer.Peek(offset) - '0';

        public int AsHex(int offset)
        {
            char ch = this.buffer.Peek(offset);
            return ((ch > '9') ? ((ch > 'F') ? ((ch - 'a') + 10) : ((ch - 'A') + 10)) : (ch - '0'));
        }

        public bool Check(char expected, int offset = 0) => 
            this.buffer.Peek(offset) == expected;

        public bool Check(string expectedCharacters, int offset = 0)
        {
            char ch = this.buffer.Peek(offset);
            return (expectedCharacters.IndexOf(ch) != -1);
        }

        public bool IsAlphaNumericDashOrUnderscore(int offset = 0)
        {
            char ch = this.buffer.Peek(offset);
            return ((((ch >= '0') && (ch <= '9')) || (((ch >= 'A') && (ch <= 'Z')) || (((ch >= 'a') && (ch <= 'z')) || (ch == '_')))) || (ch == '-'));
        }

        public bool IsAscii(int offset = 0) => 
            this.buffer.Peek(offset) <= '\x007f';

        public bool IsBreak(int offset = 0) => 
            this.Check("\r\n\x0085\u2028\u2029", offset);

        public bool IsBreakOrZero(int offset = 0) => 
            this.IsBreak(offset) || this.IsZero(offset);

        public bool IsCrLf(int offset = 0) => 
            this.Check('\r', offset) && this.Check('\n', offset + 1);

        public bool IsDigit(int offset = 0)
        {
            char ch = this.buffer.Peek(offset);
            return ((ch >= '0') && (ch <= '9'));
        }

        public bool IsHex(int offset)
        {
            char ch = this.buffer.Peek(offset);
            return ((((ch >= '0') && (ch <= '9')) || ((ch >= 'A') && (ch <= 'F'))) || ((ch >= 'a') && (ch <= 'f')));
        }

        public bool IsPrintable(int offset = 0)
        {
            char ch = this.buffer.Peek(offset);
            return ((((ch == '\t') || ((ch == '\n') || ((ch == '\r') || ((ch >= ' ') && (ch <= '~'))))) || ((ch == '\x0085') || ((ch >= '\x00a0') && (ch <= 0xd7ff)))) || ((ch >= 0xe000) && (ch <= 0xfffd)));
        }

        public bool IsSpace(int offset = 0) => 
            this.Check(' ', offset);

        public bool IsTab(int offset = 0) => 
            this.Check('\t', offset);

        public bool IsWhite(int offset = 0) => 
            this.IsSpace(offset) || this.IsTab(offset);

        public bool IsWhiteBreakOrZero(int offset = 0) => 
            this.IsWhite(offset) || this.IsBreakOrZero(offset);

        public bool IsZero(int offset = 0) => 
            this.Check('\0', offset);

        public char Peek(int offset) => 
            this.buffer.Peek(offset);

        public void Skip(int length)
        {
            this.buffer.Skip(length);
        }

        public TBuffer Buffer =>
            this.buffer;

        public bool EndOfInput =>
            this.buffer.EndOfInput;
    }
}

