namespace WebSocketSharp
{
    using System;

    public class MessageEventArgs : EventArgs
    {
        private string _data;
        private bool _dataSet;
        private Opcode _opcode;
        private byte[] _rawData;

        internal MessageEventArgs(WebSocketFrame frame)
        {
            this._opcode = frame.Opcode;
            this._rawData = frame.PayloadData.ApplicationData;
        }

        internal MessageEventArgs(Opcode opcode, byte[] rawData)
        {
            if (rawData.LongLength > PayloadData.MaxLength)
            {
                throw new WebSocketException(CloseStatusCode.TooBig);
            }
            this._opcode = opcode;
            this._rawData = rawData;
        }

        public string Data
        {
            get
            {
                if (!this._dataSet)
                {
                    this._data = (this._opcode == Opcode.Binary) ? BitConverter.ToString(this._rawData) : this._rawData.UTF8Decode();
                    this._dataSet = true;
                }
                return this._data;
            }
        }

        public bool IsBinary =>
            this._opcode == Opcode.Binary;

        public bool IsPing =>
            this._opcode == Opcode.Ping;

        public bool IsText =>
            this._opcode == Opcode.Text;

        public byte[] RawData =>
            this._rawData;

        [Obsolete("This property will be removed. Use any of the Is properties instead.")]
        public Opcode Type =>
            this._opcode;
    }
}

