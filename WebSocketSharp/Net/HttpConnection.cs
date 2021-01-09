namespace WebSocketSharp.Net
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Security;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;

    internal sealed class HttpConnection
    {
        private byte[] _buffer;
        private const int _bufferLength = 0x2000;
        private HttpListenerContext _context;
        private bool _contextBound;
        private StringBuilder _currentLine;
        private InputState _inputState;
        private RequestStream _inputStream;
        private HttpListener _lastListener;
        private LineState _lineState;
        private EndPointListener _listener;
        private ResponseStream _outputStream;
        private int _position;
        private HttpListenerPrefix _prefix;
        private MemoryStream _requestBuffer;
        private int _reuses;
        private bool _secure;
        private Socket _socket;
        private System.IO.Stream _stream;
        private object _sync;
        private int _timeout;
        private Timer _timer;
        [CompilerGenerated]
        private static TimerCallback <>f__mg$cache0;
        [CompilerGenerated]
        private static AsyncCallback <>f__mg$cache1;
        [CompilerGenerated]
        private static AsyncCallback <>f__mg$cache2;

        internal HttpConnection(Socket socket, EndPointListener listener)
        {
            this._socket = socket;
            this._listener = listener;
            this._secure = listener.IsSecure;
            NetworkStream innerStream = new NetworkStream(socket, false);
            if (!this._secure)
            {
                this._stream = innerStream;
            }
            else
            {
                ServerSslConfiguration sslConfiguration = listener.SslConfiguration;
                SslStream stream2 = new SslStream(innerStream, false, sslConfiguration.ClientCertificateValidationCallback);
                stream2.AuthenticateAsServer(sslConfiguration.ServerCertificate, sslConfiguration.ClientCertificateRequired, sslConfiguration.EnabledSslProtocols, sslConfiguration.CheckCertificateRevocation);
                this._stream = stream2;
            }
            this._sync = new object();
            this._timeout = 0x15f90;
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new TimerCallback(HttpConnection.onTimeout);
            }
            this._timer = new Timer(<>f__mg$cache0, this, -1, -1);
            this.init();
        }

        public void BeginReadRequest()
        {
            this._buffer ??= new byte[0x2000];
            if (this._reuses == 1)
            {
                this._timeout = 0x3a98;
            }
            try
            {
                this._timer.Change(this._timeout, -1);
                if (<>f__mg$cache2 == null)
                {
                    <>f__mg$cache2 = new AsyncCallback(HttpConnection.onRead);
                }
                this._stream.BeginRead(this._buffer, 0, 0x2000, <>f__mg$cache2, this);
            }
            catch
            {
                this.close();
            }
        }

        private void close()
        {
            lock (this._sync)
            {
                if (this._socket != null)
                {
                    this.disposeTimer();
                    this.disposeRequestBuffer();
                    this.disposeStream();
                    this.closeSocket();
                }
                else
                {
                    return;
                }
            }
            this.unbind();
            this.removeConnection();
        }

        public void Close()
        {
            this.Close(false);
        }

        internal void Close(bool force)
        {
            if (this._socket != null)
            {
                lock (this._sync)
                {
                    if (this._socket != null)
                    {
                        if (force)
                        {
                            if (this._outputStream != null)
                            {
                                this._outputStream.Close(true);
                            }
                        }
                        else
                        {
                            this.GetResponseStream().Close(false);
                            if (!this._context.Response.CloseConnection && this._context.Request.FlushInput())
                            {
                                this._reuses++;
                                this.disposeRequestBuffer();
                                this.unbind();
                                this.init();
                                this.BeginReadRequest();
                                goto TR_0002;
                            }
                        }
                        this.close();
                    }
                TR_0002:;
                }
            }
        }

        private void closeSocket()
        {
            try
            {
                this._socket.Shutdown(SocketShutdown.Both);
            }
            catch
            {
            }
            this._socket.Close();
            this._socket = null;
        }

        private void disposeRequestBuffer()
        {
            if (this._requestBuffer != null)
            {
                this._requestBuffer.Dispose();
                this._requestBuffer = null;
            }
        }

        private void disposeStream()
        {
            if (this._stream != null)
            {
                this._inputStream = null;
                this._outputStream = null;
                this._stream.Dispose();
                this._stream = null;
            }
        }

        private void disposeTimer()
        {
            if (this._timer != null)
            {
                try
                {
                    this._timer.Change(-1, -1);
                }
                catch
                {
                }
                this._timer.Dispose();
                this._timer = null;
            }
        }

        public RequestStream GetRequestStream(long contentLength, bool chunked)
        {
            RequestStream stream;
            if ((this._inputStream != null) || (this._socket == null))
            {
                return this._inputStream;
            }
            lock (this._sync)
            {
                if (this._socket == null)
                {
                    stream = this._inputStream;
                }
                else
                {
                    byte[] buffer = this._requestBuffer.GetBuffer();
                    int length = (int) this._requestBuffer.Length;
                    this.disposeRequestBuffer();
                    if (!chunked)
                    {
                        this._inputStream = new RequestStream(this._stream, buffer, this._position, length - this._position, contentLength);
                    }
                    else
                    {
                        this._context.Response.SendChunked = true;
                        this._inputStream = new ChunkedRequestStream(this._stream, buffer, this._position, length - this._position, this._context);
                    }
                    stream = this._inputStream;
                }
            }
            return stream;
        }

        public ResponseStream GetResponseStream()
        {
            ResponseStream stream;
            if ((this._outputStream != null) || (this._socket == null))
            {
                return this._outputStream;
            }
            lock (this._sync)
            {
                if (this._socket == null)
                {
                    stream = this._outputStream;
                }
                else
                {
                    HttpListener listener = this._context.Listener;
                    bool ignoreWriteExceptions = (listener == null) || listener.IgnoreWriteExceptions;
                    this._outputStream = new ResponseStream(this._stream, this._context.Response, ignoreWriteExceptions);
                    stream = this._outputStream;
                }
            }
            return stream;
        }

        private void init()
        {
            this._context = new HttpListenerContext(this);
            this._inputState = InputState.RequestLine;
            this._inputStream = null;
            this._lineState = LineState.None;
            this._outputStream = null;
            this._position = 0;
            this._prefix = null;
            this._requestBuffer = new MemoryStream();
        }

        private static void onRead(IAsyncResult asyncResult)
        {
            HttpConnection asyncState = (HttpConnection) asyncResult.AsyncState;
            if (asyncState._socket != null)
            {
                lock (asyncState._sync)
                {
                    if (asyncState._socket != null)
                    {
                        int count = -1;
                        int length = 0;
                        try
                        {
                            asyncState._timer.Change(-1, -1);
                            count = asyncState._stream.EndRead(asyncResult);
                            asyncState._requestBuffer.Write(asyncState._buffer, 0, count);
                            length = (int) asyncState._requestBuffer.Length;
                        }
                        catch (Exception exception)
                        {
                            if ((asyncState._requestBuffer != null) && (asyncState._requestBuffer.Length > 0L))
                            {
                                asyncState.SendError(exception.Message, 400);
                            }
                            else
                            {
                                asyncState.close();
                            }
                            goto TR_0002;
                        }
                        if (count > 0)
                        {
                            if (!asyncState.processInput(asyncState._requestBuffer.GetBuffer(), length))
                            {
                                if (<>f__mg$cache1 == null)
                                {
                                    <>f__mg$cache1 = new AsyncCallback(HttpConnection.onRead);
                                }
                                asyncState._stream.BeginRead(asyncState._buffer, 0, 0x2000, <>f__mg$cache1, asyncState);
                            }
                            else
                            {
                                if (!asyncState._context.HasError)
                                {
                                    asyncState._context.Request.FinishInitialization();
                                }
                                if (!asyncState._context.HasError)
                                {
                                    if (asyncState._listener.BindContext(asyncState._context))
                                    {
                                        HttpListener objB = asyncState._context.Listener;
                                        if (!ReferenceEquals(asyncState._lastListener, objB))
                                        {
                                            asyncState.removeConnection();
                                            if (objB.AddConnection(asyncState))
                                            {
                                                asyncState._lastListener = objB;
                                            }
                                            else
                                            {
                                                asyncState.close();
                                                goto TR_0002;
                                            }
                                        }
                                        if (objB.RegisterContext(asyncState._context))
                                        {
                                            asyncState._contextBound = true;
                                        }
                                    }
                                    else
                                    {
                                        asyncState.SendError("Invalid host", 400);
                                    }
                                }
                                else
                                {
                                    asyncState.SendError();
                                }
                            }
                        }
                        else
                        {
                            asyncState.close();
                        }
                    }
                TR_0002:;
                }
            }
        }

        private static void onTimeout(object state)
        {
            ((HttpConnection) state).close();
        }

        private bool processInput(byte[] data, int length)
        {
            bool flag;
            this._currentLine ??= new StringBuilder(0x40);
            int read = 0;
            try
            {
                while (true)
                {
                    string header = this.readLineFrom(data, this._position, length, out read);
                    if (header != null)
                    {
                        this._position += read;
                        if (header.Length == 0)
                        {
                            if (this._inputState == InputState.RequestLine)
                            {
                                continue;
                            }
                            if (this._position > 0x8000)
                            {
                                this._context.ErrorMessage = "Headers too long";
                            }
                            this._currentLine = null;
                            flag = true;
                        }
                        else
                        {
                            if (this._inputState != InputState.RequestLine)
                            {
                                this._context.Request.AddHeader(header);
                            }
                            else
                            {
                                this._context.Request.SetRequestLine(header);
                                this._inputState = InputState.Headers;
                            }
                            if (!this._context.HasError)
                            {
                                continue;
                            }
                            flag = true;
                        }
                    }
                    else
                    {
                        this._position += read;
                        if (this._position < 0x8000)
                        {
                            return false;
                        }
                        this._context.ErrorMessage = "Headers too long";
                        return true;
                    }
                    break;
                }
            }
            catch (Exception exception)
            {
                this._context.ErrorMessage = exception.Message;
                flag = true;
            }
            return flag;
        }

        private string readLineFrom(byte[] buffer, int offset, int length, out int read)
        {
            read = 0;
            for (int i = offset; (i < length) && (this._lineState != LineState.Lf); i++)
            {
                read++;
                byte num2 = buffer[i];
                if (num2 == 13)
                {
                    this._lineState = LineState.Cr;
                }
                else if (num2 == 10)
                {
                    this._lineState = LineState.Lf;
                }
                else
                {
                    this._currentLine.Append((char) num2);
                }
            }
            if (this._lineState != LineState.Lf)
            {
                return null;
            }
            this._lineState = LineState.None;
            string str = this._currentLine.ToString();
            this._currentLine.Length = 0;
            return str;
        }

        private void removeConnection()
        {
            if (this._lastListener != null)
            {
                this._lastListener.RemoveConnection(this);
            }
            else
            {
                this._listener.RemoveConnection(this);
            }
        }

        public void SendError()
        {
            this.SendError(this._context.ErrorMessage, this._context.ErrorStatus);
        }

        public void SendError(string message, int status)
        {
            if (this._socket != null)
            {
                lock (this._sync)
                {
                    if (this._socket != null)
                    {
                        try
                        {
                            HttpListenerResponse response = this._context.Response;
                            response.StatusCode = status;
                            response.ContentType = "text/html";
                            StringBuilder builder = new StringBuilder(0x40);
                            builder.AppendFormat("<html><body><h1>{0} {1}", status, response.StatusDescription);
                            if ((message != null) && (message.Length > 0))
                            {
                                builder.AppendFormat(" ({0})</h1></body></html>", message);
                            }
                            else
                            {
                                builder.Append("</h1></body></html>");
                            }
                            Encoding encoding = Encoding.UTF8;
                            byte[] bytes = encoding.GetBytes(builder.ToString());
                            response.ContentEncoding = encoding;
                            response.ContentLength64 = bytes.LongLength;
                            response.Close(bytes, true);
                        }
                        catch
                        {
                            this.Close(true);
                        }
                    }
                }
            }
        }

        private void unbind()
        {
            if (this._contextBound)
            {
                this._listener.UnbindContext(this._context);
                this._contextBound = false;
            }
        }

        public bool IsClosed =>
            ReferenceEquals(this._socket, null);

        public bool IsSecure =>
            this._secure;

        public IPEndPoint LocalEndPoint =>
            (IPEndPoint) this._socket.LocalEndPoint;

        public HttpListenerPrefix Prefix
        {
            get => 
                this._prefix;
            set => 
                this._prefix = value;
        }

        public IPEndPoint RemoteEndPoint =>
            (IPEndPoint) this._socket.RemoteEndPoint;

        public int Reuses =>
            this._reuses;

        public System.IO.Stream Stream =>
            this._stream;
    }
}

