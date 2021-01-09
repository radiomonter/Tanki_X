namespace YamlDotNet.Core
{
    using System;
    using YamlDotNet.Core.Tokens;

    public interface IScanner
    {
        void ConsumeCurrent();
        bool MoveNext();
        bool MoveNextWithoutConsuming();

        Mark CurrentPosition { get; }

        Token Current { get; }
    }
}

