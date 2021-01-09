namespace Platform.Library.ClientProtocol.Impl
{
    using System;
    using System.IO;

    public abstract class StreamData<T> : StreamData where T: System.IO.Stream, new()
    {
        private T streamData;

        protected StreamData()
        {
            this.streamData = Activator.CreateInstance<T>();
        }

        public override System.IO.Stream Stream =>
            this.streamData;

        public T CastedStream =>
            this.streamData;
    }
}

