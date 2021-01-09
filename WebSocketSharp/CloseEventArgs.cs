namespace WebSocketSharp
{
    using System;

    public class CloseEventArgs : EventArgs
    {
        private bool _clean;
        private ushort _code;
        private WebSocketSharp.PayloadData _payloadData;
        private string _reason;

        internal CloseEventArgs()
        {
            this._code = 0x3ed;
            this._payloadData = WebSocketSharp.PayloadData.Empty;
        }

        internal CloseEventArgs(ushort code)
        {
            this._code = code;
        }

        internal CloseEventArgs(CloseStatusCode code) : this((ushort) code)
        {
        }

        internal CloseEventArgs(WebSocketSharp.PayloadData payloadData)
        {
            this._payloadData = payloadData;
            byte[] applicationData = payloadData.ApplicationData;
            int length = applicationData.Length;
            this._code = (length <= 1) ? ((ushort) 0x3ed) : applicationData.SubArray<byte>(0, 2).ToUInt16(ByteOrder.Big);
            this._reason = (length <= 2) ? string.Empty : applicationData.SubArray<byte>(2, (length - 2)).UTF8Decode();
        }

        internal CloseEventArgs(ushort code, string reason)
        {
            this._code = code;
            this._reason = reason;
        }

        internal CloseEventArgs(CloseStatusCode code, string reason) : this((ushort) code, reason)
        {
        }

        internal WebSocketSharp.PayloadData PayloadData
        {
            get
            {
                WebSocketSharp.PayloadData data2 = this._payloadData;
                if (this._payloadData == null)
                {
                    WebSocketSharp.PayloadData local1 = this._payloadData;
                    data2 = this._payloadData = new WebSocketSharp.PayloadData(this._code.Append(this._reason));
                }
                return data2;
            }
        }

        public ushort Code =>
            this._code;

        public string Reason =>
            this._reason ?? string.Empty;

        public bool WasClean
        {
            get => 
                this._clean;
            internal set => 
                this._clean = value;
        }
    }
}

