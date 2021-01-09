namespace YamlDotNet.Core
{
    using System;

    internal interface ILookAheadBuffer
    {
        char Peek(int offset);
        void Skip(int length);

        bool EndOfInput { get; }
    }
}

