namespace WebSocketSharp.Server
{
    using System;
    using WebSocketSharp.Net;

    public class HttpRequestEventArgs : EventArgs
    {
        private HttpListenerRequest _request;
        private HttpListenerResponse _response;

        internal HttpRequestEventArgs(HttpListenerContext context)
        {
            this._request = context.Request;
            this._response = context.Response;
        }

        public HttpListenerRequest Request =>
            this._request;

        public HttpListenerResponse Response =>
            this._response;
    }
}

