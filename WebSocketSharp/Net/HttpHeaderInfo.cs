namespace WebSocketSharp.Net
{
    using System;

    internal class HttpHeaderInfo
    {
        private string _name;
        private HttpHeaderType _type;

        internal HttpHeaderInfo(string name, HttpHeaderType type)
        {
            this._name = name;
            this._type = type;
        }

        public bool IsMultiValue(bool response) => 
            ((this._type & HttpHeaderType.MultiValue) != HttpHeaderType.MultiValue) ? (!response ? this.IsMultiValueInRequest : this.IsMultiValueInResponse) : (!response ? this.IsRequest : this.IsResponse);

        public bool IsRestricted(bool response) => 
            ((this._type & HttpHeaderType.Restricted) == HttpHeaderType.Restricted) && (!response ? this.IsRequest : this.IsResponse);

        internal bool IsMultiValueInRequest =>
            (this._type & HttpHeaderType.MultiValueInRequest) == HttpHeaderType.MultiValueInRequest;

        internal bool IsMultiValueInResponse =>
            (this._type & HttpHeaderType.MultiValueInResponse) == HttpHeaderType.MultiValueInResponse;

        public bool IsRequest =>
            (this._type & HttpHeaderType.Request) == HttpHeaderType.Request;

        public bool IsResponse =>
            (this._type & HttpHeaderType.Response) == HttpHeaderType.Response;

        public string Name =>
            this._name;

        public HttpHeaderType Type =>
            this._type;
    }
}

