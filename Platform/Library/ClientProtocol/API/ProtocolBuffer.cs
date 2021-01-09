namespace Platform.Library.ClientProtocol.API
{
    using Platform.Library.ClientProtocol.Impl;
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;

    public class ProtocolBuffer
    {
        public ProtocolBuffer() : this(new Platform.Library.ClientProtocol.Impl.OptionalMap(), new MemoryStreamData())
        {
        }

        public ProtocolBuffer(IOptionalMap optionalMap, MemoryStreamData stream)
        {
            this.OptionalMap = optionalMap;
            this.Data = stream;
        }

        public void Clear()
        {
            this.OptionalMap.Clear();
            this.Data.Clear();
        }

        public void Flip()
        {
            this.Data.Flip();
        }

        public override string ToString() => 
            (StreamDumper.Dump(this.Data.Stream) + Environment.NewLine) + this.OptionalMap.ToString() + Environment.NewLine;

        public IOptionalMap OptionalMap { get; private set; }

        public MemoryStreamData Data { get; private set; }

        public BinaryReader Reader =>
            this.Data.Reader;

        public BinaryWriter Writer =>
            this.Data.Writer;
    }
}

