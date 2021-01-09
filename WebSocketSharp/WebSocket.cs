namespace WebSocketSharp
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.IO;
    using System.Net.Security;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using WebSocketSharp.Net;
    using WebSocketSharp.Net.WebSockets;

    public class WebSocket : IDisposable
    {
        private AuthenticationChallenge _authChallenge;
        private string _base64Key;
        private bool _client;
        private Action _closeContext;
        private CompressionMethod _compression;
        private WebSocketContext _context;
        private WebSocketSharp.Net.CookieCollection _cookies;
        private NetworkCredential _credentials;
        private bool _emitOnPing;
        private bool _enableRedirection;
        private AutoResetEvent _exitReceiving;
        private string _extensions;
        private bool _extensionsRequested;
        private object _forConn;
        private object _forMessageEventQueue;
        private object _forSend;
        private MemoryStream _fragmentsBuffer;
        private bool _fragmentsCompressed;
        private Opcode _fragmentsOpcode;
        private const string _guid = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
        private Func<WebSocketContext, string> _handshakeRequestChecker;
        private bool _ignoreExtensions;
        private bool _inContinuation;
        private volatile bool _inMessage;
        private volatile Logger _logger;
        private Action<MessageEventArgs> _message;
        private Queue<MessageEventArgs> _messageEventQueue;
        private uint _nonceCount;
        private string _origin;
        private bool _preAuth;
        private string _protocol;
        private string[] _protocols;
        private bool _protocolsRequested;
        private NetworkCredential _proxyCredentials;
        private Uri _proxyUri;
        private volatile WebSocketState _readyState;
        private AutoResetEvent _receivePong;
        private bool _secure;
        private ClientSslConfiguration _sslConfig;
        private Stream _stream;
        private TcpClient _tcpClient;
        private Uri _uri;
        private const string _version = "13";
        private TimeSpan _waitTime;
        internal static readonly byte[] EmptyBytes = new byte[0];
        internal static readonly int FragmentLength = 0x3f8;
        internal static readonly RandomNumberGenerator RandomNumber = new RNGCryptoServiceProvider();
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EventHandler<CloseEventArgs> OnClose;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EventHandler<ErrorEventArgs> OnError;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EventHandler<MessageEventArgs> OnMessage;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EventHandler OnOpen;

        public event EventHandler<CloseEventArgs> OnClose
        {
            add
            {
                EventHandler<CloseEventArgs> onClose = this.OnClose;
                while (true)
                {
                    EventHandler<CloseEventArgs> objB = onClose;
                    onClose = Interlocked.CompareExchange<EventHandler<CloseEventArgs>>(ref this.OnClose, objB + value, onClose);
                    if (ReferenceEquals(onClose, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                EventHandler<CloseEventArgs> onClose = this.OnClose;
                while (true)
                {
                    EventHandler<CloseEventArgs> objB = onClose;
                    onClose = Interlocked.CompareExchange<EventHandler<CloseEventArgs>>(ref this.OnClose, objB - value, onClose);
                    if (ReferenceEquals(onClose, objB))
                    {
                        return;
                    }
                }
            }
        }

        public event EventHandler<ErrorEventArgs> OnError
        {
            add
            {
                EventHandler<ErrorEventArgs> onError = this.OnError;
                while (true)
                {
                    EventHandler<ErrorEventArgs> objB = onError;
                    onError = Interlocked.CompareExchange<EventHandler<ErrorEventArgs>>(ref this.OnError, objB + value, onError);
                    if (ReferenceEquals(onError, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                EventHandler<ErrorEventArgs> onError = this.OnError;
                while (true)
                {
                    EventHandler<ErrorEventArgs> objB = onError;
                    onError = Interlocked.CompareExchange<EventHandler<ErrorEventArgs>>(ref this.OnError, objB - value, onError);
                    if (ReferenceEquals(onError, objB))
                    {
                        return;
                    }
                }
            }
        }

        public event EventHandler<MessageEventArgs> OnMessage
        {
            add
            {
                EventHandler<MessageEventArgs> onMessage = this.OnMessage;
                while (true)
                {
                    EventHandler<MessageEventArgs> objB = onMessage;
                    onMessage = Interlocked.CompareExchange<EventHandler<MessageEventArgs>>(ref this.OnMessage, objB + value, onMessage);
                    if (ReferenceEquals(onMessage, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                EventHandler<MessageEventArgs> onMessage = this.OnMessage;
                while (true)
                {
                    EventHandler<MessageEventArgs> objB = onMessage;
                    onMessage = Interlocked.CompareExchange<EventHandler<MessageEventArgs>>(ref this.OnMessage, objB - value, onMessage);
                    if (ReferenceEquals(onMessage, objB))
                    {
                        return;
                    }
                }
            }
        }

        public event EventHandler OnOpen
        {
            add
            {
                EventHandler onOpen = this.OnOpen;
                while (true)
                {
                    EventHandler objB = onOpen;
                    onOpen = Interlocked.CompareExchange<EventHandler>(ref this.OnOpen, objB + value, onOpen);
                    if (ReferenceEquals(onOpen, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                EventHandler onOpen = this.OnOpen;
                while (true)
                {
                    EventHandler objB = onOpen;
                    onOpen = Interlocked.CompareExchange<EventHandler>(ref this.OnOpen, objB - value, onOpen);
                    if (ReferenceEquals(onOpen, objB))
                    {
                        return;
                    }
                }
            }
        }

        public WebSocket(string url, params string[] protocols)
        {
            string str;
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }
            if (url.Length == 0)
            {
                throw new ArgumentException("An empty string.", "url");
            }
            if (!url.TryCreateWebSocketUri(out this._uri, out str))
            {
                throw new ArgumentException(str, "url");
            }
            if ((protocols != null) && (protocols.Length > 0))
            {
                str = protocols.CheckIfValidProtocols();
                if (str != null)
                {
                    throw new ArgumentException(str, "protocols");
                }
                this._protocols = protocols;
            }
            this._base64Key = CreateBase64Key();
            this._client = true;
            this._logger = new Logger();
            this._message = new Action<MessageEventArgs>(this.messagec);
            this._secure = this._uri.Scheme == "wss";
            this._waitTime = TimeSpan.FromSeconds(5.0);
            this.init();
        }

        internal WebSocket(HttpListenerWebSocketContext context, string protocol)
        {
            this._context = context;
            this._protocol = protocol;
            this._closeContext = new Action(context.Close);
            this._logger = context.Log;
            this._message = new Action<MessageEventArgs>(this.messages);
            this._secure = context.IsSecureConnection;
            this._stream = context.Stream;
            this._waitTime = TimeSpan.FromSeconds(1.0);
            this.init();
        }

        internal WebSocket(TcpListenerWebSocketContext context, string protocol)
        {
            this._context = context;
            this._protocol = protocol;
            this._closeContext = new Action(context.Close);
            this._logger = context.Log;
            this._message = new Action<MessageEventArgs>(this.messages);
            this._secure = context.IsSecureConnection;
            this._stream = context.Stream;
            this._waitTime = TimeSpan.FromSeconds(1.0);
            this.init();
        }

        private bool accept()
        {
            bool flag;
            lock (this._forConn)
            {
                string str;
                if (this.checkIfAvailable(true, false, false, false, out str))
                {
                    try
                    {
                        if (this.acceptHandshake())
                        {
                            this._readyState = 1;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (Exception exception)
                    {
                        this._logger.Fatal(exception.ToString());
                        this.fatal("An exception has occurred while accepting.", exception);
                        return false;
                    }
                    flag = true;
                }
                else
                {
                    this._logger.Error(str);
                    this.error("An error has occurred in accepting.", null);
                    flag = false;
                }
            }
            return flag;
        }

        public void Accept()
        {
            string str;
            if (!this.checkIfAvailable(false, true, true, false, false, false, out str))
            {
                this._logger.Error(str);
                this.error("An error has occurred in accepting.", null);
            }
            else if (this.accept())
            {
                this.open();
            }
        }

        public void AcceptAsync()
        {
            string str;
            <AcceptAsync>c__AnonStorey7 storey = new <AcceptAsync>c__AnonStorey7 {
                $this = this
            };
            if (!this.checkIfAvailable(false, true, true, false, false, false, out str))
            {
                this._logger.Error(str);
                this.error("An error has occurred in accepting.", null);
            }
            else
            {
                storey.acceptor = new Func<bool>(this.accept);
                storey.acceptor.BeginInvoke(new AsyncCallback(storey.<>m__0), null);
            }
        }

        private bool acceptHandshake()
        {
            string str;
            this._logger.Debug($"A request from {this._context.UserEndPoint}:
{this._context}");
            if (!this.checkHandshakeRequest(this._context, out str))
            {
                this.sendHttpResponse(this.createHandshakeFailureResponse(HttpStatusCode.BadRequest));
                this._logger.Fatal(str);
                this.fatal("An error has occurred while accepting.", CloseStatusCode.ProtocolError);
                return false;
            }
            if (!this.customCheckHandshakeRequest(this._context, out str))
            {
                this.sendHttpResponse(this.createHandshakeFailureResponse(HttpStatusCode.BadRequest));
                this._logger.Fatal(str);
                this.fatal("An error has occurred while accepting.", CloseStatusCode.PolicyViolation);
                return false;
            }
            this._base64Key = this._context.Headers["Sec-WebSocket-Key"];
            if (this._protocol != null)
            {
                this.processSecWebSocketProtocolHeader(this._context.SecWebSocketProtocols);
            }
            if (!this._ignoreExtensions)
            {
                this.processSecWebSocketExtensionsClientHeader(this._context.Headers["Sec-WebSocket-Extensions"]);
            }
            return this.sendHttpResponse(this.createHandshakeResponse());
        }

        internal static string CheckCloseParameters(ushort code, string reason, bool client) => 
            code.IsCloseStatusCode() ? ((code != 0x3ed) ? (((code != 0x3f2) || client) ? (((code != 0x3f3) || !client) ? ((reason.IsNullOrEmpty() || (reason.UTF8Encode().Length <= 0x7b)) ? null : "A reason has greater than the allowable max size.") : "ServerError cannot be used by a client.") : "MandatoryExtension cannot be used by a server.") : (reason.IsNullOrEmpty() ? null : "NoStatus cannot have a reason.")) : "An invalid close status code.";

        internal static string CheckCloseParameters(CloseStatusCode code, string reason, bool client) => 
            (code != CloseStatusCode.NoStatus) ? (((code != CloseStatusCode.MandatoryExtension) || client) ? (((code != CloseStatusCode.ServerError) || !client) ? ((reason.IsNullOrEmpty() || (reason.UTF8Encode().Length <= 0x7b)) ? null : "A reason has greater than the allowable max size.") : "ServerError cannot be used by a client.") : "MandatoryExtension cannot be used by a server.") : (reason.IsNullOrEmpty() ? null : "NoStatus cannot have a reason.");

        private bool checkHandshakeRequest(WebSocketContext context, out string message)
        {
            message = null;
            if (context.RequestUri == null)
            {
                message = "Specifies an invalid Request-URI.";
                return false;
            }
            if (!context.IsWebSocketRequest)
            {
                message = "Not a WebSocket handshake request.";
                return false;
            }
            NameValueCollection headers = context.Headers;
            if (!this.validateSecWebSocketKeyHeader(headers["Sec-WebSocket-Key"]))
            {
                message = "Includes no Sec-WebSocket-Key header, or it has an invalid value.";
                return false;
            }
            if (!this.validateSecWebSocketVersionClientHeader(headers["Sec-WebSocket-Version"]))
            {
                message = "Includes no Sec-WebSocket-Version header, or it has an invalid value.";
                return false;
            }
            if (!this.validateSecWebSocketProtocolClientHeader(headers["Sec-WebSocket-Protocol"]))
            {
                message = "Includes an invalid Sec-WebSocket-Protocol header.";
                return false;
            }
            if (this._ignoreExtensions || this.validateSecWebSocketExtensionsClientHeader(headers["Sec-WebSocket-Extensions"]))
            {
                return true;
            }
            message = "Includes an invalid Sec-WebSocket-Extensions header.";
            return false;
        }

        private bool checkHandshakeResponse(HttpResponse response, out string message)
        {
            message = null;
            if (response.IsRedirect)
            {
                message = "Indicates the redirection.";
                return false;
            }
            if (response.IsUnauthorized)
            {
                message = "Requires the authentication.";
                return false;
            }
            if (!response.IsWebSocketResponse)
            {
                message = "Not a WebSocket handshake response.";
                return false;
            }
            NameValueCollection headers = response.Headers;
            if (!this.validateSecWebSocketAcceptHeader(headers["Sec-WebSocket-Accept"]))
            {
                message = "Includes no Sec-WebSocket-Accept header, or it has an invalid value.";
                return false;
            }
            if (!this.validateSecWebSocketProtocolServerHeader(headers["Sec-WebSocket-Protocol"]))
            {
                message = "Includes no Sec-WebSocket-Protocol header, or it has an invalid value.";
                return false;
            }
            if (!this.validateSecWebSocketExtensionsServerHeader(headers["Sec-WebSocket-Extensions"]))
            {
                message = "Includes an invalid Sec-WebSocket-Extensions header.";
                return false;
            }
            if (this.validateSecWebSocketVersionServerHeader(headers["Sec-WebSocket-Version"]))
            {
                return true;
            }
            message = "Includes an invalid Sec-WebSocket-Version header.";
            return false;
        }

        private bool checkIfAvailable(bool connecting, bool open, bool closing, bool closed, out string message)
        {
            message = null;
            if (!connecting && (this._readyState == null))
            {
                message = "This operation isn't available in: connecting";
                return false;
            }
            if (!open && (this._readyState == 1))
            {
                message = "This operation isn't available in: open";
                return false;
            }
            if (!closing && (this._readyState == 2))
            {
                message = "This operation isn't available in: closing";
                return false;
            }
            if (closed || (this._readyState != 3))
            {
                return true;
            }
            message = "This operation isn't available in: closed";
            return false;
        }

        private bool checkIfAvailable(bool client, bool server, bool connecting, bool open, bool closing, bool closed, out string message)
        {
            message = null;
            if (!client && this._client)
            {
                message = "This operation isn't available in: client";
                return false;
            }
            if (server || this._client)
            {
                return this.checkIfAvailable(connecting, open, closing, closed, out message);
            }
            message = "This operation isn't available in: server";
            return false;
        }

        internal static string CheckPingParameter(string message, out byte[] bytes)
        {
            bytes = message.UTF8Encode();
            return ((bytes.Length <= 0x7d) ? null : "A message has greater than the allowable max size.");
        }

        private bool checkReceivedFrame(WebSocketFrame frame, out string message)
        {
            message = null;
            bool isMasked = frame.IsMasked;
            if (this._client && isMasked)
            {
                message = "A frame from the server is masked.";
                return false;
            }
            if (!this._client && !isMasked)
            {
                message = "A frame from a client isn't masked.";
                return false;
            }
            if (this._inContinuation && frame.IsData)
            {
                message = "A data frame has been received while receiving continuation frames.";
                return false;
            }
            if (frame.IsCompressed && (this._compression == CompressionMethod.None))
            {
                message = "A compressed frame has been received without any agreement for it.";
                return false;
            }
            if (frame.Rsv2 == Rsv.On)
            {
                message = "The RSV2 of a frame is non-zero without any negotiation for it.";
                return false;
            }
            if (frame.Rsv3 != Rsv.On)
            {
                return true;
            }
            message = "The RSV3 of a frame is non-zero without any negotiation for it.";
            return false;
        }

        internal static string CheckSendParameter(byte[] data) => 
            (data != null) ? null : "'data' is null.";

        internal static string CheckSendParameter(FileInfo file) => 
            (file != null) ? null : "'file' is null.";

        internal static string CheckSendParameter(string data) => 
            (data != null) ? null : "'data' is null.";

        internal static string CheckSendParameters(Stream stream, int length) => 
            (stream != null) ? (stream.CanRead ? ((length >= 1) ? null : "'length' is less than 1.") : "'stream' cannot be read.") : "'stream' is null.";

        private void close(CloseEventArgs e, bool send, bool receive, bool received)
        {
            lock (this._forConn)
            {
                if (this._readyState != 2)
                {
                    if (this._readyState != 3)
                    {
                        send = send && (this._readyState == 1);
                        receive = receive && send;
                        this._readyState = 2;
                    }
                    else
                    {
                        this._logger.Info("The connection has been closed.");
                        return;
                    }
                }
                else
                {
                    this._logger.Info("The closing is already in progress.");
                    return;
                }
            }
            this._logger.Trace("Begin closing the connection.");
            byte[] frameAsBytes = !send ? null : WebSocketFrame.CreateCloseFrame(e.PayloadData, this._client).ToArray();
            e.WasClean = this.closeHandshake(frameAsBytes, receive, received);
            this.releaseResources();
            this._logger.Trace("End closing the connection.");
            this._readyState = 3;
            try
            {
                this.OnClose.Emit<CloseEventArgs>(this, e);
            }
            catch (Exception exception)
            {
                this._logger.Error(exception.ToString());
                this.error("An exception has occurred during the OnClose event.", exception);
            }
        }

        public void Close()
        {
            string str;
            if (this.checkIfAvailable(true, true, false, false, out str))
            {
                this.close(new CloseEventArgs(), true, true, false);
            }
            else
            {
                this._logger.Error(str);
                this.error("An error has occurred in closing the connection.", null);
            }
        }

        public void Close(ushort code)
        {
            string message = this._readyState.CheckIfAvailable(true, true, false, false) ?? CheckCloseParameters(code, null, this._client);
            if (message != null)
            {
                this._logger.Error(message);
                this.error("An error has occurred in closing the connection.", null);
            }
            else if (code == 0x3ed)
            {
                this.close(new CloseEventArgs(), true, true, false);
            }
            else
            {
                bool send = !code.IsReserved();
                this.close(new CloseEventArgs(code), send, send, false);
            }
        }

        public void Close(CloseStatusCode code)
        {
            string message = this._readyState.CheckIfAvailable(true, true, false, false) ?? CheckCloseParameters(code, null, this._client);
            if (message != null)
            {
                this._logger.Error(message);
                this.error("An error has occurred in closing the connection.", null);
            }
            else if (code == CloseStatusCode.NoStatus)
            {
                this.close(new CloseEventArgs(), true, true, false);
            }
            else
            {
                bool send = !code.IsReserved();
                this.close(new CloseEventArgs(code), send, send, false);
            }
        }

        internal void Close(HttpResponse response)
        {
            this._readyState = 2;
            this.sendHttpResponse(response);
            this.releaseServerResources();
            this._readyState = 3;
        }

        internal void Close(HttpStatusCode code)
        {
            this.Close(this.createHandshakeFailureResponse(code));
        }

        public void Close(ushort code, string reason)
        {
            string message = this._readyState.CheckIfAvailable(true, true, false, false) ?? CheckCloseParameters(code, reason, this._client);
            if (message != null)
            {
                this._logger.Error(message);
                this.error("An error has occurred in closing the connection.", null);
            }
            else if (code == 0x3ed)
            {
                this.close(new CloseEventArgs(), true, true, false);
            }
            else
            {
                bool send = !code.IsReserved();
                this.close(new CloseEventArgs(code, reason), send, send, false);
            }
        }

        public void Close(CloseStatusCode code, string reason)
        {
            string message = this._readyState.CheckIfAvailable(true, true, false, false) ?? CheckCloseParameters(code, reason, this._client);
            if (message != null)
            {
                this._logger.Error(message);
                this.error("An error has occurred in closing the connection.", null);
            }
            else if (code == CloseStatusCode.NoStatus)
            {
                this.close(new CloseEventArgs(), true, true, false);
            }
            else
            {
                bool send = !code.IsReserved();
                this.close(new CloseEventArgs(code, reason), send, send, false);
            }
        }

        internal void Close(CloseEventArgs e, byte[] frameAsBytes, bool receive)
        {
            lock (this._forConn)
            {
                if (this._readyState != 2)
                {
                    if (this._readyState != 3)
                    {
                        this._readyState = 2;
                    }
                    else
                    {
                        this._logger.Info("The connection has been closed.");
                        return;
                    }
                }
                else
                {
                    this._logger.Info("The closing is already in progress.");
                    return;
                }
            }
            e.WasClean = this.closeHandshake(frameAsBytes, receive, false);
            this.releaseServerResources();
            this.releaseCommonResources();
            this._readyState = 3;
            try
            {
                this.OnClose.Emit<CloseEventArgs>(this, e);
            }
            catch (Exception exception)
            {
                this._logger.Error(exception.ToString());
            }
        }

        private void closeAsync(CloseEventArgs e, bool send, bool receive, bool received)
        {
            <closeAsync>c__AnonStorey1 storey = new <closeAsync>c__AnonStorey1 {
                closer = new Action<CloseEventArgs, bool, bool, bool>(this.close)
            };
            storey.closer.BeginInvoke(e, send, receive, received, new AsyncCallback(storey.<>m__0), null);
        }

        public void CloseAsync()
        {
            string str;
            if (this.checkIfAvailable(true, true, false, false, out str))
            {
                this.closeAsync(new CloseEventArgs(), true, true, false);
            }
            else
            {
                this._logger.Error(str);
                this.error("An error has occurred in closing the connection.", null);
            }
        }

        public void CloseAsync(ushort code)
        {
            string message = this._readyState.CheckIfAvailable(true, true, false, false) ?? CheckCloseParameters(code, null, this._client);
            if (message != null)
            {
                this._logger.Error(message);
                this.error("An error has occurred in closing the connection.", null);
            }
            else if (code == 0x3ed)
            {
                this.closeAsync(new CloseEventArgs(), true, true, false);
            }
            else
            {
                bool send = !code.IsReserved();
                this.closeAsync(new CloseEventArgs(code), send, send, false);
            }
        }

        public void CloseAsync(CloseStatusCode code)
        {
            string message = this._readyState.CheckIfAvailable(true, true, false, false) ?? CheckCloseParameters(code, null, this._client);
            if (message != null)
            {
                this._logger.Error(message);
                this.error("An error has occurred in closing the connection.", null);
            }
            else if (code == CloseStatusCode.NoStatus)
            {
                this.closeAsync(new CloseEventArgs(), true, true, false);
            }
            else
            {
                bool send = !code.IsReserved();
                this.closeAsync(new CloseEventArgs(code), send, send, false);
            }
        }

        public void CloseAsync(ushort code, string reason)
        {
            string message = this._readyState.CheckIfAvailable(true, true, false, false) ?? CheckCloseParameters(code, reason, this._client);
            if (message != null)
            {
                this._logger.Error(message);
                this.error("An error has occurred in closing the connection.", null);
            }
            else if (code == 0x3ed)
            {
                this.closeAsync(new CloseEventArgs(), true, true, false);
            }
            else
            {
                bool send = !code.IsReserved();
                this.closeAsync(new CloseEventArgs(code, reason), send, send, false);
            }
        }

        public void CloseAsync(CloseStatusCode code, string reason)
        {
            string message = this._readyState.CheckIfAvailable(true, true, false, false) ?? CheckCloseParameters(code, reason, this._client);
            if (message != null)
            {
                this._logger.Error(message);
                this.error("An error has occurred in closing the connection.", null);
            }
            else if (code == CloseStatusCode.NoStatus)
            {
                this.closeAsync(new CloseEventArgs(), true, true, false);
            }
            else
            {
                bool send = !code.IsReserved();
                this.closeAsync(new CloseEventArgs(code, reason), send, send, false);
            }
        }

        private bool closeHandshake(byte[] frameAsBytes, bool receive, bool received)
        {
            bool flag = (frameAsBytes != null) && this.sendBytes(frameAsBytes);
            received = received || ((receive && (flag && (this._exitReceiving != null))) && this._exitReceiving.WaitOne(this._waitTime));
            bool flag2 = flag && received;
            this._logger.Debug($"Was clean?: {flag2}
  sent: {flag}
  received: {received}");
            return flag2;
        }

        private bool connect()
        {
            bool flag;
            lock (this._forConn)
            {
                string str;
                if (this.checkIfAvailable(true, false, false, true, out str))
                {
                    try
                    {
                        this._readyState = 0;
                        if (this.doHandshake())
                        {
                            this._readyState = 1;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (Exception exception)
                    {
                        this._logger.Fatal(exception.ToString());
                        this.fatal("An exception has occurred while connecting.", exception);
                        return false;
                    }
                    flag = true;
                }
                else
                {
                    this._logger.Error(str);
                    this.error("An error has occurred in connecting.", null);
                    flag = false;
                }
            }
            return flag;
        }

        public void Connect()
        {
            string str;
            if (!this.checkIfAvailable(true, false, true, false, false, true, out str))
            {
                this._logger.Error(str);
                this.error("An error has occurred in connecting.", null);
            }
            else if (this.connect())
            {
                this.open();
            }
        }

        public void ConnectAsync()
        {
            string str;
            <ConnectAsync>c__AnonStorey8 storey = new <ConnectAsync>c__AnonStorey8 {
                $this = this
            };
            if (!this.checkIfAvailable(true, false, true, false, false, true, out str))
            {
                this._logger.Error(str);
                this.error("An error has occurred in connecting.", null);
            }
            else
            {
                storey.connector = new Func<bool>(this.connect);
                storey.connector.BeginInvoke(new AsyncCallback(storey.<>m__0), null);
            }
        }

        internal static string CreateBase64Key()
        {
            byte[] data = new byte[0x10];
            RandomNumber.GetBytes(data);
            return Convert.ToBase64String(data);
        }

        private string createExtensions()
        {
            StringBuilder builder = new StringBuilder(80);
            if (this._compression != CompressionMethod.None)
            {
                string[] parameters = new string[] { "server_no_context_takeover", "client_no_context_takeover" };
                string str = this._compression.ToExtensionString(parameters);
                builder.AppendFormat("{0}, ", str);
            }
            int length = builder.Length;
            if (length <= 2)
            {
                return null;
            }
            builder.Length = length - 2;
            return builder.ToString();
        }

        private HttpResponse createHandshakeFailureResponse(HttpStatusCode code)
        {
            HttpResponse response = HttpResponse.CreateCloseResponse(code);
            response.Headers["Sec-WebSocket-Version"] = "13";
            return response;
        }

        private HttpRequest createHandshakeRequest()
        {
            HttpRequest request = HttpRequest.CreateWebSocketRequest(this._uri);
            NameValueCollection headers = request.Headers;
            if (!this._origin.IsNullOrEmpty())
            {
                headers["Origin"] = this._origin;
            }
            headers["Sec-WebSocket-Key"] = this._base64Key;
            this._protocolsRequested = this._protocols != null;
            if (this._protocolsRequested)
            {
                headers["Sec-WebSocket-Protocol"] = this._protocols.ToString<string>(", ");
            }
            this._extensionsRequested = this._compression != CompressionMethod.None;
            if (this._extensionsRequested)
            {
                headers["Sec-WebSocket-Extensions"] = this.createExtensions();
            }
            headers["Sec-WebSocket-Version"] = "13";
            AuthenticationResponse response = null;
            if ((this._authChallenge != null) && (this._credentials != null))
            {
                response = new AuthenticationResponse(this._authChallenge, this._credentials, this._nonceCount);
                this._nonceCount = response.NonceCount;
            }
            else if (this._preAuth)
            {
                response = new AuthenticationResponse(this._credentials);
            }
            if (response != null)
            {
                headers["Authorization"] = response.ToString();
            }
            if (this._cookies.Count > 0)
            {
                request.SetCookies(this._cookies);
            }
            return request;
        }

        private HttpResponse createHandshakeResponse()
        {
            HttpResponse response = HttpResponse.CreateWebSocketResponse();
            NameValueCollection headers = response.Headers;
            headers["Sec-WebSocket-Accept"] = CreateResponseKey(this._base64Key);
            if (this._protocol != null)
            {
                headers["Sec-WebSocket-Protocol"] = this._protocol;
            }
            if (this._extensions != null)
            {
                headers["Sec-WebSocket-Extensions"] = this._extensions;
            }
            if (this._cookies.Count > 0)
            {
                response.SetCookies(this._cookies);
            }
            return response;
        }

        internal static string CreateResponseKey(string base64Key)
        {
            StringBuilder builder = new StringBuilder(base64Key, 0x40);
            builder.Append("258EAFA5-E914-47DA-95CA-C5AB0DC85B11");
            return Convert.ToBase64String(new SHA1CryptoServiceProvider().ComputeHash(builder.ToString().UTF8Encode()));
        }

        private bool customCheckHandshakeRequest(WebSocketContext context, out string message)
        {
            bool flag1;
            message = null;
            if (this._handshakeRequestChecker == null)
            {
                flag1 = true;
            }
            else
            {
                string str;
                message = str = this._handshakeRequestChecker(context);
                flag1 = ReferenceEquals(str, null);
            }
            return flag1;
        }

        private MessageEventArgs dequeueFromMessageEventQueue()
        {
            lock (this._forMessageEventQueue)
            {
                return ((this._messageEventQueue.Count <= 0) ? null : this._messageEventQueue.Dequeue());
            }
        }

        private bool doHandshake()
        {
            string str;
            this.setClientStream();
            HttpResponse response = this.sendHandshakeRequest();
            if (!this.checkHandshakeResponse(response, out str))
            {
                this._logger.Fatal(str);
                this.fatal("An error has occurred while connecting.", CloseStatusCode.ProtocolError);
                return false;
            }
            if (this._protocolsRequested)
            {
                this._protocol = response.Headers["Sec-WebSocket-Protocol"];
            }
            if (this._extensionsRequested)
            {
                this.processSecWebSocketExtensionsServerHeader(response.Headers["Sec-WebSocket-Extensions"]);
            }
            this.processCookies(response.Cookies);
            return true;
        }

        private void enqueueToMessageEventQueue(MessageEventArgs e)
        {
            lock (this._forMessageEventQueue)
            {
                this._messageEventQueue.Enqueue(e);
            }
        }

        private void error(string message, Exception exception)
        {
            try
            {
                this.OnError.Emit<ErrorEventArgs>(this, new ErrorEventArgs(message, exception));
            }
            catch (Exception exception2)
            {
                this._logger.Error(exception2.ToString());
            }
        }

        private void fatal(string message, Exception exception)
        {
            CloseStatusCode code = !(exception is WebSocketException) ? CloseStatusCode.Abnormal : ((WebSocketException) exception).Code;
            this.fatal(message, code);
        }

        private void fatal(string message, CloseStatusCode code)
        {
            this.close(new CloseEventArgs(code, message), !code.IsReserved(), false, false);
        }

        private void init()
        {
            this._compression = CompressionMethod.None;
            this._cookies = new WebSocketSharp.Net.CookieCollection();
            this._forConn = new object();
            this._forSend = new object();
            this._messageEventQueue = new Queue<MessageEventArgs>();
            this._forMessageEventQueue = ((ICollection) this._messageEventQueue).SyncRoot;
            this._readyState = 0;
        }

        internal void InternalAccept()
        {
            try
            {
                if (this.acceptHandshake())
                {
                    this._readyState = 1;
                }
                else
                {
                    return;
                }
            }
            catch (Exception exception)
            {
                this._logger.Fatal(exception.ToString());
                this.fatal("An exception has occurred while accepting.", exception);
                return;
            }
            this.open();
        }

        private void message()
        {
            MessageEventArgs args = null;
            lock (this._forMessageEventQueue)
            {
                if (this._inMessage)
                {
                    return;
                }
                else if (this._messageEventQueue.Count == 0)
                {
                    return;
                }
                else if (this._readyState == 1)
                {
                    this._inMessage = true;
                    args = this._messageEventQueue.Dequeue();
                }
                else
                {
                    return;
                }
            }
            this._message(args);
        }

        private void messagec(MessageEventArgs e)
        {
            while (true)
            {
                try
                {
                    this.OnMessage.Emit<MessageEventArgs>(this, e);
                }
                catch (Exception exception)
                {
                    this._logger.Error(exception.ToString());
                    this.error("An exception has occurred during an OnMessage event.", exception);
                }
                object obj2 = this._forMessageEventQueue;
                lock (obj2)
                {
                    if ((this._messageEventQueue.Count == 0) || (this._readyState != 1))
                    {
                        this._inMessage = false;
                        break;
                    }
                    e = this._messageEventQueue.Dequeue();
                }
            }
        }

        private void messages(MessageEventArgs e)
        {
            <messages>c__AnonStorey2 storey = new <messages>c__AnonStorey2 {
                e = e,
                $this = this
            };
            try
            {
                this.OnMessage.Emit<MessageEventArgs>(this, storey.e);
            }
            catch (Exception exception)
            {
                this._logger.Error(exception.ToString());
                this.error("An exception has occurred during an OnMessage event.", exception);
            }
            lock (this._forMessageEventQueue)
            {
                if ((this._messageEventQueue.Count == 0) || (this._readyState != 1))
                {
                    this._inMessage = false;
                    return;
                }
                else
                {
                    storey.e = this._messageEventQueue.Dequeue();
                }
            }
            ThreadPool.QueueUserWorkItem(new WaitCallback(storey.<>m__0));
        }

        private void open()
        {
            this._inMessage = true;
            this.startReceiving();
            try
            {
                this.OnOpen.Emit(this, EventArgs.Empty);
            }
            catch (Exception exception)
            {
                this._logger.Error(exception.ToString());
                this.error("An exception has occurred during the OnOpen event.", exception);
            }
            MessageEventArgs args = null;
            lock (this._forMessageEventQueue)
            {
                if ((this._messageEventQueue.Count == 0) || (this._readyState != 1))
                {
                    this._inMessage = false;
                    return;
                }
                else
                {
                    args = this._messageEventQueue.Dequeue();
                }
            }
            this._message.BeginInvoke(args, ar => this._message.EndInvoke(ar), null);
        }

        public bool Ping()
        {
            byte[] frameAsBytes = !this._client ? WebSocketFrame.EmptyPingBytes : WebSocketFrame.CreatePingFrame(true).ToArray();
            return this.Ping(frameAsBytes, this._waitTime);
        }

        public bool Ping(string message)
        {
            byte[] buffer;
            if ((message == null) || (message.Length == 0))
            {
                return this.Ping();
            }
            string str = CheckPingParameter(message, out buffer);
            if (str == null)
            {
                return this.Ping(WebSocketFrame.CreatePingFrame(buffer, this._client).ToArray(), this._waitTime);
            }
            this._logger.Error(str);
            this.error("An error has occurred in sending a ping.", null);
            return false;
        }

        internal bool Ping(byte[] frameAsBytes, TimeSpan timeout)
        {
            if (this._readyState != 1)
            {
                return false;
            }
            if (!this.send(frameAsBytes))
            {
                return false;
            }
            AutoResetEvent event2 = this._receivePong;
            return ((event2 != null) ? event2.WaitOne(timeout) : false);
        }

        private bool processCloseFrame(WebSocketFrame frame)
        {
            PayloadData payloadData = frame.PayloadData;
            this.close(new CloseEventArgs(payloadData), !payloadData.IncludesReservedCloseStatusCode, false, true);
            return false;
        }

        private void processCookies(WebSocketSharp.Net.CookieCollection cookies)
        {
            if (cookies.Count != 0)
            {
                this._cookies.SetOrRemove(cookies);
            }
        }

        private bool processDataFrame(WebSocketFrame frame)
        {
            this.enqueueToMessageEventQueue(!frame.IsCompressed ? new MessageEventArgs(frame) : new MessageEventArgs(frame.Opcode, frame.PayloadData.ApplicationData.Decompress(this._compression)));
            return true;
        }

        private bool processFragmentFrame(WebSocketFrame frame)
        {
            if (!this._inContinuation)
            {
                if (frame.IsContinuation)
                {
                    return true;
                }
                this._fragmentsOpcode = frame.Opcode;
                this._fragmentsCompressed = frame.IsCompressed;
                this._fragmentsBuffer = new MemoryStream();
                this._inContinuation = true;
            }
            this._fragmentsBuffer.WriteBytes(frame.PayloadData.ApplicationData, 0x400);
            if (frame.IsFinal)
            {
                using (this._fragmentsBuffer)
                {
                    byte[] rawData = !this._fragmentsCompressed ? this._fragmentsBuffer.ToArray() : this._fragmentsBuffer.DecompressToArray(this._compression);
                    this.enqueueToMessageEventQueue(new MessageEventArgs(this._fragmentsOpcode, rawData));
                }
                this._fragmentsBuffer = null;
                this._inContinuation = false;
            }
            return true;
        }

        private bool processPingFrame(WebSocketFrame frame)
        {
            if (this.send(new WebSocketFrame(Opcode.Pong, frame.PayloadData, this._client).ToArray()))
            {
                this._logger.Trace("Returned a pong.");
            }
            if (this._emitOnPing)
            {
                this.enqueueToMessageEventQueue(new MessageEventArgs(frame));
            }
            return true;
        }

        private bool processPongFrame(WebSocketFrame frame)
        {
            this._receivePong.Set();
            this._logger.Trace("Received a pong.");
            return true;
        }

        private bool processReceivedFrame(WebSocketFrame frame)
        {
            string str;
            if (!this.checkReceivedFrame(frame, out str))
            {
                throw new WebSocketException(CloseStatusCode.ProtocolError, str);
            }
            frame.Unmask();
            return (!frame.IsFragment ? (!frame.IsData ? (!frame.IsPing ? (!frame.IsPong ? (!frame.IsClose ? this.processUnsupportedFrame(frame) : this.processCloseFrame(frame)) : this.processPongFrame(frame)) : this.processPingFrame(frame)) : this.processDataFrame(frame)) : this.processFragmentFrame(frame));
        }

        private void processSecWebSocketExtensionsClientHeader(string value)
        {
            if (value != null)
            {
                StringBuilder builder = new StringBuilder(80);
                bool flag = false;
                char[] separators = new char[] { ',' };
                foreach (string str in value.SplitHeaderValue(separators))
                {
                    string str2 = str.Trim();
                    if (!flag && str2.IsCompressionExtension(CompressionMethod.Deflate))
                    {
                        this._compression = CompressionMethod.Deflate;
                        string[] parameters = new string[] { "client_no_context_takeover", "server_no_context_takeover" };
                        builder.AppendFormat("{0}, ", this._compression.ToExtensionString(parameters));
                        flag = true;
                    }
                }
                int length = builder.Length;
                if (length > 2)
                {
                    builder.Length = length - 2;
                    this._extensions = builder.ToString();
                }
            }
        }

        private void processSecWebSocketExtensionsServerHeader(string value)
        {
            if (value == null)
            {
                this._compression = CompressionMethod.None;
            }
            else
            {
                this._extensions = value;
            }
        }

        private void processSecWebSocketProtocolHeader(IEnumerable<string> values)
        {
            if (!values.Contains<string>(p => (p == this._protocol)))
            {
                this._protocol = null;
            }
        }

        private bool processUnsupportedFrame(WebSocketFrame frame)
        {
            this._logger.Fatal("An unsupported frame:" + frame.PrintToString(false));
            this.fatal("There is no way to handle it.", CloseStatusCode.PolicyViolation);
            return false;
        }

        private void releaseClientResources()
        {
            if (this._stream != null)
            {
                this._stream.Dispose();
                this._stream = null;
            }
            if (this._tcpClient != null)
            {
                this._tcpClient.Close();
                this._tcpClient = null;
            }
        }

        private void releaseCommonResources()
        {
            if (this._fragmentsBuffer != null)
            {
                this._fragmentsBuffer.Dispose();
                this._fragmentsBuffer = null;
                this._inContinuation = false;
            }
            if (this._receivePong != null)
            {
                this._receivePong.Close();
                this._receivePong = null;
            }
            if (this._exitReceiving != null)
            {
                this._exitReceiving.Close();
                this._exitReceiving = null;
            }
        }

        private void releaseResources()
        {
            if (this._client)
            {
                this.releaseClientResources();
            }
            else
            {
                this.releaseServerResources();
            }
            this.releaseCommonResources();
        }

        private void releaseServerResources()
        {
            if (this._closeContext != null)
            {
                this._closeContext();
                this._closeContext = null;
                this._stream = null;
                this._context = null;
            }
        }

        private bool send(byte[] frameAsBytes)
        {
            bool flag;
            lock (this._forConn)
            {
                if (this._readyState == 1)
                {
                    flag = this.sendBytes(frameAsBytes);
                }
                else
                {
                    this._logger.Error("The sending has been interrupted.");
                    flag = false;
                }
            }
            return flag;
        }

        private bool send(Opcode opcode, Stream stream)
        {
            lock (this._forSend)
            {
                Stream stream2 = stream;
                bool compressed = false;
                bool flag2 = false;
                try
                {
                    if (this._compression != CompressionMethod.None)
                    {
                        stream = stream.Compress(this._compression);
                        compressed = true;
                    }
                    flag2 = this.send(opcode, stream, compressed);
                    if (!flag2)
                    {
                        this.error("The sending has been interrupted.", null);
                    }
                }
                catch (Exception exception)
                {
                    this._logger.Error(exception.ToString());
                    this.error("An exception has occurred while sending data.", exception);
                }
                finally
                {
                    if (compressed)
                    {
                        stream.Dispose();
                    }
                    stream2.Dispose();
                }
                return flag2;
            }
        }

        private bool send(Opcode opcode, Stream stream, bool compressed)
        {
            long length = stream.Length;
            if (length == 0L)
            {
                return this.send(Fin.Final, opcode, EmptyBytes, compressed);
            }
            long num2 = length / ((long) FragmentLength);
            int count = (int) (length % ((long) FragmentLength));
            byte[] buffer = null;
            if (num2 == 0L)
            {
                buffer = new byte[count];
                return ((stream.Read(buffer, 0, count) == count) && this.send(Fin.Final, opcode, buffer, compressed));
            }
            buffer = new byte[FragmentLength];
            if ((num2 == 1L) && (count == 0))
            {
                return ((stream.Read(buffer, 0, FragmentLength) == FragmentLength) && this.send(Fin.Final, opcode, buffer, compressed));
            }
            if ((stream.Read(buffer, 0, FragmentLength) != FragmentLength) || !this.send(Fin.More, opcode, buffer, compressed))
            {
                return false;
            }
            long num4 = (count != 0) ? (num2 - 1L) : (num2 - 2L);
            for (long i = 0L; i < num4; i += 1L)
            {
                if ((stream.Read(buffer, 0, FragmentLength) != FragmentLength) || !this.send(Fin.More, Opcode.Cont, buffer, compressed))
                {
                    return false;
                }
            }
            if (count == 0)
            {
                count = FragmentLength;
            }
            else
            {
                buffer = new byte[count];
            }
            return ((stream.Read(buffer, 0, count) == count) && this.send(Fin.Final, Opcode.Cont, buffer, compressed));
        }

        private bool send(Fin fin, Opcode opcode, byte[] data, bool compressed)
        {
            bool flag;
            lock (this._forConn)
            {
                if (this._readyState == 1)
                {
                    flag = this.sendBytes(new WebSocketFrame(fin, opcode, data, compressed, this._client).ToArray());
                }
                else
                {
                    this._logger.Error("The sending has been interrupted.");
                    flag = false;
                }
            }
            return flag;
        }

        public void Send(byte[] data)
        {
            string message = this._readyState.CheckIfAvailable(false, true, false, false) ?? CheckSendParameter(data);
            if (message == null)
            {
                this.send(Opcode.Binary, new MemoryStream(data));
            }
            else
            {
                this._logger.Error(message);
                this.error("An error has occurred in sending data.", null);
            }
        }

        public void Send(FileInfo file)
        {
            string message = this._readyState.CheckIfAvailable(false, true, false, false) ?? CheckSendParameter(file);
            if (message == null)
            {
                this.send(Opcode.Binary, file.OpenRead());
            }
            else
            {
                this._logger.Error(message);
                this.error("An error has occurred in sending data.", null);
            }
        }

        public void Send(string data)
        {
            string message = this._readyState.CheckIfAvailable(false, true, false, false) ?? CheckSendParameter(data);
            if (message == null)
            {
                this.send(Opcode.Text, new MemoryStream(data.UTF8Encode()));
            }
            else
            {
                this._logger.Error(message);
                this.error("An error has occurred in sending data.", null);
            }
        }

        internal void Send(Opcode opcode, byte[] data, Dictionary<CompressionMethod, byte[]> cache)
        {
            lock (this._forSend)
            {
                lock (this._forConn)
                {
                    if (this._readyState != 1)
                    {
                        this._logger.Error("The sending has been interrupted.");
                    }
                    else
                    {
                        try
                        {
                            byte[] buffer;
                            if (!cache.TryGetValue(this._compression, out buffer))
                            {
                                buffer = new WebSocketFrame(Fin.Final, opcode, data.Compress(this._compression), this._compression != CompressionMethod.None, false).ToArray();
                                cache.Add(this._compression, buffer);
                            }
                            this.sendBytes(buffer);
                        }
                        catch (Exception exception)
                        {
                            this._logger.Error(exception.ToString());
                        }
                    }
                }
            }
        }

        internal void Send(Opcode opcode, Stream stream, Dictionary<CompressionMethod, Stream> cache)
        {
            Monitor.Enter(this._forSend);
            try
            {
                Stream stream2;
                if (cache.TryGetValue(this._compression, out stream2))
                {
                    stream2.Position = 0L;
                }
                else
                {
                    stream2 = stream.Compress(this._compression);
                    cache.Add(this._compression, stream2);
                }
                this.send(opcode, stream2, this._compression != CompressionMethod.None);
            }
            catch (Exception exception)
            {
                this._logger.Error(exception.ToString());
            }
            finally
            {
                object obj2;
                Monitor.Exit(obj2);
            }
        }

        private void sendAsync(Opcode opcode, Stream stream, Action<bool> completed)
        {
            <sendAsync>c__AnonStorey3 storey = new <sendAsync>c__AnonStorey3 {
                completed = completed,
                $this = this,
                sender = new Func<Opcode, Stream, bool>(this.send)
            };
            storey.sender.BeginInvoke(opcode, stream, new AsyncCallback(storey.<>m__0), null);
        }

        public void SendAsync(byte[] data, Action<bool> completed)
        {
            string message = this._readyState.CheckIfAvailable(false, true, false, false) ?? CheckSendParameter(data);
            if (message == null)
            {
                this.sendAsync(Opcode.Binary, new MemoryStream(data), completed);
            }
            else
            {
                this._logger.Error(message);
                this.error("An error has occurred in sending data.", null);
            }
        }

        public void SendAsync(FileInfo file, Action<bool> completed)
        {
            string message = this._readyState.CheckIfAvailable(false, true, false, false) ?? CheckSendParameter(file);
            if (message == null)
            {
                this.sendAsync(Opcode.Binary, file.OpenRead(), completed);
            }
            else
            {
                this._logger.Error(message);
                this.error("An error has occurred in sending data.", null);
            }
        }

        public void SendAsync(string data, Action<bool> completed)
        {
            string message = this._readyState.CheckIfAvailable(false, true, false, false) ?? CheckSendParameter(data);
            if (message == null)
            {
                this.sendAsync(Opcode.Text, new MemoryStream(data.UTF8Encode()), completed);
            }
            else
            {
                this._logger.Error(message);
                this.error("An error has occurred in sending data.", null);
            }
        }

        public void SendAsync(Stream stream, int length, Action<bool> completed)
        {
            <SendAsync>c__AnonStorey9 storey = new <SendAsync>c__AnonStorey9 {
                length = length,
                completed = completed,
                $this = this
            };
            string message = this._readyState.CheckIfAvailable(false, true, false, false) ?? CheckSendParameters(stream, storey.length);
            if (message == null)
            {
                stream.ReadBytesAsync(storey.length, new Action<byte[]>(storey.<>m__0), new Action<Exception>(storey.<>m__1));
            }
            else
            {
                this._logger.Error(message);
                this.error("An error has occurred in sending data.", null);
            }
        }

        private bool sendBytes(byte[] bytes)
        {
            try
            {
                this._stream.Write(bytes, 0, bytes.Length);
                return true;
            }
            catch (Exception exception)
            {
                this._logger.Error(exception.ToString());
                return false;
            }
        }

        private HttpResponse sendHandshakeRequest()
        {
            HttpRequest request = this.createHandshakeRequest();
            HttpResponse response = this.sendHttpRequest(request, 0x15f90);
            if (response.IsUnauthorized)
            {
                string str = response.Headers["WWW-Authenticate"];
                this._logger.Warn($"Received an authentication requirement for '{str}'.");
                if (str.IsNullOrEmpty())
                {
                    this._logger.Error("No authentication challenge is specified.");
                    return response;
                }
                this._authChallenge = AuthenticationChallenge.Parse(str);
                if (this._authChallenge == null)
                {
                    this._logger.Error("An invalid authentication challenge is specified.");
                    return response;
                }
                if ((this._credentials != null) && (!this._preAuth || (this._authChallenge.Scheme == AuthenticationSchemes.Digest)))
                {
                    if (response.HasConnectionClose)
                    {
                        this.releaseClientResources();
                        this.setClientStream();
                    }
                    AuthenticationResponse response2 = new AuthenticationResponse(this._authChallenge, this._credentials, this._nonceCount);
                    this._nonceCount = response2.NonceCount;
                    request.Headers["Authorization"] = response2.ToString();
                    response = this.sendHttpRequest(request, 0x3a98);
                }
            }
            if (response.IsRedirect)
            {
                string str2 = response.Headers["Location"];
                this._logger.Warn($"Received a redirection to '{str2}'.");
                if (this._enableRedirection)
                {
                    Uri uri;
                    string str3;
                    if (str2.IsNullOrEmpty())
                    {
                        this._logger.Error("No url to redirect is located.");
                        return response;
                    }
                    if (!str2.TryCreateWebSocketUri(out uri, out str3))
                    {
                        this._logger.Error("An invalid url to redirect is located: " + str3);
                        return response;
                    }
                    this.releaseClientResources();
                    this._uri = uri;
                    this._secure = uri.Scheme == "wss";
                    this.setClientStream();
                    return this.sendHandshakeRequest();
                }
            }
            return response;
        }

        private HttpResponse sendHttpRequest(HttpRequest request, int millisecondsTimeout)
        {
            this._logger.Debug("A request to the server:\n" + request.ToString());
            HttpResponse response = request.GetResponse(this._stream, millisecondsTimeout);
            this._logger.Debug("A response to this request:\n" + response.ToString());
            return response;
        }

        private bool sendHttpResponse(HttpResponse response)
        {
            this._logger.Debug("A response to this request:\n" + response.ToString());
            return this.sendBytes(response.ToByteArray());
        }

        private void sendProxyConnectRequest()
        {
            HttpRequest request = HttpRequest.CreateConnectRequest(this._uri);
            HttpResponse response = this.sendHttpRequest(request, 0x15f90);
            if (response.IsProxyAuthenticationRequired)
            {
                string str = response.Headers["Proxy-Authenticate"];
                this._logger.Warn($"Received a proxy authentication requirement for '{str}'.");
                if (str.IsNullOrEmpty())
                {
                    throw new WebSocketException("No proxy authentication challenge is specified.");
                }
                AuthenticationChallenge challenge = AuthenticationChallenge.Parse(str);
                if (challenge == null)
                {
                    throw new WebSocketException("An invalid proxy authentication challenge is specified.");
                }
                if (this._proxyCredentials != null)
                {
                    if (response.HasConnectionClose)
                    {
                        this.releaseClientResources();
                        this._tcpClient = new TcpClient(this._proxyUri.DnsSafeHost, this._proxyUri.Port);
                        this._stream = this._tcpClient.GetStream();
                    }
                    request.Headers["Proxy-Authorization"] = new AuthenticationResponse(challenge, this._proxyCredentials, 0).ToString();
                    response = this.sendHttpRequest(request, 0x3a98);
                }
                if (response.IsProxyAuthenticationRequired)
                {
                    throw new WebSocketException("A proxy authentication is required.");
                }
            }
            if (response.StatusCode[0] != '2')
            {
                throw new WebSocketException("The proxy has failed a connection to the requested host and port.");
            }
        }

        private void setClientStream()
        {
            if (this._proxyUri == null)
            {
                this._tcpClient = new TcpClient(this._uri.DnsSafeHost, this._uri.Port);
                this._stream = this._tcpClient.GetStream();
            }
            else
            {
                this._tcpClient = new TcpClient(this._proxyUri.DnsSafeHost, this._proxyUri.Port);
                this._stream = this._tcpClient.GetStream();
                this.sendProxyConnectRequest();
            }
            if (this._secure)
            {
                ClientSslConfiguration sslConfiguration = this.SslConfiguration;
                string targetHost = sslConfiguration.TargetHost;
                if (targetHost != this._uri.DnsSafeHost)
                {
                    throw new WebSocketException(CloseStatusCode.TlsHandshakeFailure, "An invalid host name is specified.");
                }
                try
                {
                    SslStream stream = new SslStream(this._stream, false, sslConfiguration.ServerCertificateValidationCallback, sslConfiguration.ClientCertificateSelectionCallback);
                    stream.AuthenticateAsClient(targetHost, sslConfiguration.ClientCertificates, sslConfiguration.EnabledSslProtocols, sslConfiguration.CheckCertificateRevocation);
                    this._stream = stream;
                }
                catch (Exception exception1)
                {
                    throw new WebSocketException(CloseStatusCode.TlsHandshakeFailure, exception1);
                }
            }
        }

        public void SetCookie(Cookie cookie)
        {
            string str;
            if (!this.checkIfAvailable(true, false, true, false, false, true, out str))
            {
                this._logger.Error(str);
                this.error("An error has occurred in setting a cookie.", null);
            }
            else
            {
                lock (this._forConn)
                {
                    if (!this.checkIfAvailable(true, false, false, true, out str))
                    {
                        this._logger.Error(str);
                        this.error("An error has occurred in setting a cookie.", null);
                    }
                    else if (cookie == null)
                    {
                        this._logger.Error("'cookie' is null.");
                        this.error("An error has occurred in setting a cookie.", null);
                    }
                    else
                    {
                        lock (this._cookies.SyncRoot)
                        {
                            this._cookies.SetOrRemove(cookie);
                        }
                    }
                }
            }
        }

        public void SetCredentials(string username, string password, bool preAuth)
        {
            string str;
            if (!this.checkIfAvailable(true, false, true, false, false, true, out str))
            {
                this._logger.Error(str);
                this.error("An error has occurred in setting the credentials.", null);
            }
            else
            {
                lock (this._forConn)
                {
                    if (!this.checkIfAvailable(true, false, false, true, out str))
                    {
                        this._logger.Error(str);
                        this.error("An error has occurred in setting the credentials.", null);
                    }
                    else if (username.IsNullOrEmpty())
                    {
                        this._logger.Warn("The credentials are set back to the default.");
                        this._credentials = null;
                        this._preAuth = false;
                    }
                    else
                    {
                        char[] chars = new char[] { ':' };
                        if (username.Contains(chars) || !username.IsText())
                        {
                            this._logger.Error("'username' contains an invalid character.");
                            this.error("An error has occurred in setting the credentials.", null);
                        }
                        else if (!password.IsNullOrEmpty() && !password.IsText())
                        {
                            this._logger.Error("'password' contains an invalid character.");
                            this.error("An error has occurred in setting the credentials.", null);
                        }
                        else
                        {
                            this._credentials = new NetworkCredential(username, password, this._uri.PathAndQuery, new string[0]);
                            this._preAuth = preAuth;
                        }
                    }
                }
            }
        }

        public void SetProxy(string url, string username, string password)
        {
            string str;
            if (!this.checkIfAvailable(true, false, true, false, false, true, out str))
            {
                this._logger.Error(str);
                this.error("An error has occurred in setting the proxy.", null);
            }
            else
            {
                lock (this._forConn)
                {
                    if (!this.checkIfAvailable(true, false, false, true, out str))
                    {
                        this._logger.Error(str);
                        this.error("An error has occurred in setting the proxy.", null);
                    }
                    else if (url.IsNullOrEmpty())
                    {
                        this._logger.Warn("The proxy url and credentials are set back to the default.");
                        this._proxyUri = null;
                        this._proxyCredentials = null;
                    }
                    else
                    {
                        Uri uri;
                        if (!Uri.TryCreate(url, UriKind.Absolute, out uri) || ((uri.Scheme != "http") || (uri.Segments.Length > 1)))
                        {
                            this._logger.Error("The syntax of a proxy url must be 'http://<host>[:<port>]'.");
                            this.error("An error has occurred in setting the proxy.", null);
                        }
                        else if (username.IsNullOrEmpty())
                        {
                            this._logger.Warn("The proxy credentials are set back to the default.");
                            this._proxyUri = uri;
                            this._proxyCredentials = null;
                        }
                        else
                        {
                            char[] chars = new char[] { ':' };
                            if (username.Contains(chars) || !username.IsText())
                            {
                                this._logger.Error("'username' contains an invalid character.");
                                this.error("An error has occurred in setting the proxy.", null);
                            }
                            else if (!password.IsNullOrEmpty() && !password.IsText())
                            {
                                this._logger.Error("'password' contains an invalid character.");
                                this.error("An error has occurred in setting the proxy.", null);
                            }
                            else
                            {
                                this._proxyUri = uri;
                                this._proxyCredentials = new NetworkCredential(username, password, $"{this._uri.DnsSafeHost}:{this._uri.Port}", new string[0]);
                            }
                        }
                    }
                }
            }
        }

        private void startReceiving()
        {
            <startReceiving>c__AnonStorey4 storey = new <startReceiving>c__AnonStorey4 {
                $this = this
            };
            if (this._messageEventQueue.Count > 0)
            {
                this._messageEventQueue.Clear();
            }
            this._exitReceiving = new AutoResetEvent(false);
            this._receivePong = new AutoResetEvent(false);
            storey.receive = null;
            storey.receive = new Action(storey.<>m__0);
            storey.receive();
        }

        void IDisposable.Dispose()
        {
            this.close(new CloseEventArgs(CloseStatusCode.Away), true, true, false);
        }

        private bool validateSecWebSocketAcceptHeader(string value) => 
            (value != null) && (value == CreateResponseKey(this._base64Key));

        private bool validateSecWebSocketExtensionsClientHeader(string value) => 
            (value == null) || (value.Length > 0);

        private bool validateSecWebSocketExtensionsServerHeader(string value)
        {
            bool flag2;
            if (value == null)
            {
                return true;
            }
            if (value.Length == 0)
            {
                return false;
            }
            if (!this._extensionsRequested)
            {
                return false;
            }
            bool flag = this._compression != CompressionMethod.None;
            char[] separators = new char[] { ',' };
            using (IEnumerator<string> enumerator = value.SplitHeaderValue(separators).GetEnumerator())
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        string str2 = enumerator.Current.Trim();
                        if (!flag || !str2.IsCompressionExtension(this._compression))
                        {
                            flag2 = false;
                        }
                        else
                        {
                            <validateSecWebSocketExtensionsServerHeader>c__AnonStorey5 storey = new <validateSecWebSocketExtensionsServerHeader>c__AnonStorey5();
                            if (!str2.Contains("server_no_context_takeover"))
                            {
                                this._logger.Error("The server hasn't sent back 'server_no_context_takeover'.");
                                flag2 = false;
                            }
                            else
                            {
                                if (!str2.Contains("client_no_context_takeover"))
                                {
                                    this._logger.Warn("The server hasn't sent back 'client_no_context_takeover'.");
                                }
                                storey.method = this._compression.ToExtensionString(new string[0]);
                                char[] chArray2 = new char[] { ';' };
                                if (!str2.SplitHeaderValue(chArray2).Contains<string>(new Func<string, bool>(storey.<>m__0)))
                                {
                                    continue;
                                }
                                flag2 = false;
                            }
                        }
                    }
                    else
                    {
                        return true;
                    }
                    break;
                }
            }
            return flag2;
        }

        private bool validateSecWebSocketKeyHeader(string value) => 
            (value != null) && (value.Length > 0);

        private bool validateSecWebSocketProtocolClientHeader(string value) => 
            (value == null) || (value.Length > 0);

        private bool validateSecWebSocketProtocolServerHeader(string value)
        {
            <validateSecWebSocketProtocolServerHeader>c__AnonStorey6 storey = new <validateSecWebSocketProtocolServerHeader>c__AnonStorey6 {
                value = value
            };
            return ((storey.value != null) ? ((storey.value.Length != 0) ? (this._protocolsRequested && this._protocols.Contains<string>(new Func<string, bool>(storey.<>m__0))) : false) : !this._protocolsRequested);
        }

        private bool validateSecWebSocketVersionClientHeader(string value) => 
            (value != null) && (value == "13");

        private bool validateSecWebSocketVersionServerHeader(string value) => 
            (value == null) || (value == "13");

        internal WebSocketSharp.Net.CookieCollection CookieCollection =>
            this._cookies;

        internal Func<WebSocketContext, string> CustomHandshakeRequestChecker
        {
            get => 
                this._handshakeRequestChecker;
            set => 
                this._handshakeRequestChecker = value;
        }

        internal bool HasMessage
        {
            get
            {
                lock (this._forMessageEventQueue)
                {
                    return (this._messageEventQueue.Count > 0);
                }
            }
        }

        internal bool IgnoreExtensions
        {
            get => 
                this._ignoreExtensions;
            set => 
                this._ignoreExtensions = value;
        }

        internal bool IsConnected =>
            (this._readyState == 1) || (this._readyState == 2);

        public CompressionMethod Compression
        {
            get => 
                this._compression;
            set
            {
                lock (this._forConn)
                {
                    string str;
                    if (this.checkIfAvailable(true, false, true, false, false, true, out str))
                    {
                        this._compression = value;
                    }
                    else
                    {
                        this._logger.Error(str);
                        this.error("An error has occurred in setting the compression.", null);
                    }
                }
            }
        }

        public IEnumerable<Cookie> Cookies =>
            new <>c__Iterator0 { 
                $this=this,
                $PC=-2
            };

        public NetworkCredential Credentials =>
            this._credentials;

        public bool EmitOnPing
        {
            get => 
                this._emitOnPing;
            set => 
                this._emitOnPing = value;
        }

        public bool EnableRedirection
        {
            get => 
                this._enableRedirection;
            set
            {
                lock (this._forConn)
                {
                    string str;
                    if (this.checkIfAvailable(true, false, true, false, false, true, out str))
                    {
                        this._enableRedirection = value;
                    }
                    else
                    {
                        this._logger.Error(str);
                        this.error("An error has occurred in setting the enable redirection.", null);
                    }
                }
            }
        }

        public string Extensions =>
            this._extensions ?? string.Empty;

        public bool IsAlive =>
            this.Ping();

        public bool IsSecure =>
            this._secure;

        public Logger Log
        {
            get => 
                this._logger;
            internal set => 
                this._logger = value;
        }

        public string Origin
        {
            get => 
                this._origin;
            set
            {
                lock (this._forConn)
                {
                    string str;
                    if (!this.checkIfAvailable(true, false, true, false, false, true, out str))
                    {
                        this._logger.Error(str);
                        this.error("An error has occurred in setting the origin.", null);
                    }
                    else if (value.IsNullOrEmpty())
                    {
                        this._origin = value;
                    }
                    else
                    {
                        Uri uri;
                        if (!Uri.TryCreate(value, UriKind.Absolute, out uri) || (uri.Segments.Length > 1))
                        {
                            this._logger.Error("The syntax of an origin must be '<scheme>://<host>[:<port>]'.");
                            this.error("An error has occurred in setting the origin.", null);
                        }
                        else
                        {
                            char[] trimChars = new char[] { '/' };
                            this._origin = value.TrimEnd(trimChars);
                        }
                    }
                }
            }
        }

        public string Protocol
        {
            get => 
                this._protocol ?? string.Empty;
            internal set => 
                this._protocol = value;
        }

        public WebSocketState ReadyState =>
            this._readyState;

        public ClientSslConfiguration SslConfiguration
        {
            get
            {
                ClientSslConfiguration configuration2;
                if (!this._client)
                {
                    configuration2 = null;
                }
                else
                {
                    configuration2 = this._sslConfig;
                    if (this._sslConfig == null)
                    {
                        ClientSslConfiguration local1 = this._sslConfig;
                        configuration2 = this._sslConfig = new ClientSslConfiguration(this._uri.DnsSafeHost);
                    }
                }
                return configuration2;
            }
            set
            {
                lock (this._forConn)
                {
                    string str;
                    if (this.checkIfAvailable(true, false, true, false, false, true, out str))
                    {
                        this._sslConfig = value;
                    }
                    else
                    {
                        this._logger.Error(str);
                        this.error("An error has occurred in setting the ssl configuration.", null);
                    }
                }
            }
        }

        public Uri Url =>
            !this._client ? this._context.RequestUri : this._uri;

        public TimeSpan WaitTime
        {
            get => 
                this._waitTime;
            set
            {
                lock (this._forConn)
                {
                    string str;
                    if (this.checkIfAvailable(true, true, true, false, false, true, out str) && value.CheckWaitTime(out str))
                    {
                        this._waitTime = value;
                    }
                    else
                    {
                        this._logger.Error(str);
                        this.error("An error has occurred in setting the wait time.", null);
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<Cookie>, IEnumerator, IDisposable, IEnumerator<Cookie>
        {
            internal object $locvar0;
            internal IEnumerator $locvar1;
            internal Cookie <cookie>__1;
            internal IDisposable $locvar2;
            internal WebSocket $this;
            internal Cookie $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                uint num = (uint) this.$PC;
                this.$disposing = true;
                this.$PC = -1;
                switch (num)
                {
                    case 1:
                        try
                        {
                            try
                            {
                            }
                            finally
                            {
                                this.$locvar2 = this.$locvar1 as IDisposable;
                                if (this.$locvar2 != null)
                                {
                                    this.$locvar2.Dispose();
                                }
                            }
                        }
                        finally
                        {
                            Monitor.Exit(this.$locvar0);
                        }
                        break;

                    default:
                        break;
                }
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                bool flag = false;
                switch (num)
                {
                    case 0:
                        this.$locvar0 = this.$this._cookies.SyncRoot;
                        Monitor.Enter(this.$locvar0);
                        num = 0xfffffffd;
                        break;

                    case 1:
                        break;

                    default:
                        goto TR_0000;
                }
                try
                {
                    switch (num)
                    {
                        case 1:
                            break;

                        default:
                            this.$locvar1 = this.$this._cookies.GetEnumerator();
                            num = 0xfffffffd;
                            break;
                    }
                    try
                    {
                        switch (num)
                        {
                            default:
                                if (!this.$locvar1.MoveNext())
                                {
                                    break;
                                }
                                this.<cookie>__1 = (Cookie) this.$locvar1.Current;
                                this.$current = this.<cookie>__1;
                                if (!this.$disposing)
                                {
                                    this.$PC = 1;
                                }
                                flag = true;
                                return true;
                        }
                    }
                    finally
                    {
                        if (flag)
                        {
                        }
                        this.$locvar2 = this.$locvar1 as IDisposable;
                        if (this.$locvar2 != null)
                        {
                            this.$locvar2.Dispose();
                        }
                    }
                }
                finally
                {
                    if (flag)
                    {
                    }
                    Monitor.Exit(this.$locvar0);
                }
                this.$PC = -1;
            TR_0000:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<Cookie> IEnumerable<Cookie>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new WebSocket.<>c__Iterator0 { $this = this.$this };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator() => 
                this.System.Collections.Generic.IEnumerable<WebSocketSharp.Net.Cookie>.GetEnumerator();

            Cookie IEnumerator<Cookie>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }

        [CompilerGenerated]
        private sealed class <AcceptAsync>c__AnonStorey7
        {
            internal Func<bool> acceptor;
            internal WebSocket $this;

            internal void <>m__0(IAsyncResult ar)
            {
                if (this.acceptor.EndInvoke(ar))
                {
                    this.$this.open();
                }
            }
        }

        [CompilerGenerated]
        private sealed class <closeAsync>c__AnonStorey1
        {
            internal Action<CloseEventArgs, bool, bool, bool> closer;

            internal void <>m__0(IAsyncResult ar)
            {
                this.closer.EndInvoke(ar);
            }
        }

        [CompilerGenerated]
        private sealed class <ConnectAsync>c__AnonStorey8
        {
            internal Func<bool> connector;
            internal WebSocket $this;

            internal void <>m__0(IAsyncResult ar)
            {
                if (this.connector.EndInvoke(ar))
                {
                    this.$this.open();
                }
            }
        }

        [CompilerGenerated]
        private sealed class <messages>c__AnonStorey2
        {
            internal MessageEventArgs e;
            internal WebSocket $this;

            internal void <>m__0(object state)
            {
                this.$this.messages(this.e);
            }
        }

        [CompilerGenerated]
        private sealed class <sendAsync>c__AnonStorey3
        {
            internal Func<Opcode, Stream, bool> sender;
            internal Action<bool> completed;
            internal WebSocket $this;

            internal void <>m__0(IAsyncResult ar)
            {
                try
                {
                    bool flag = this.sender.EndInvoke(ar);
                    if (this.completed != null)
                    {
                        this.completed(flag);
                    }
                }
                catch (Exception exception)
                {
                    this.$this._logger.Error(exception.ToString());
                    this.$this.error("An exception has occurred during a send callback.", exception);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <SendAsync>c__AnonStorey9
        {
            internal int length;
            internal Action<bool> completed;
            internal WebSocket $this;

            internal void <>m__0(byte[] data)
            {
                int length = data.Length;
                if (length == 0)
                {
                    this.$this._logger.Error("The data cannot be read from 'stream'.");
                    this.$this.error("An error has occurred in sending data.", null);
                }
                else
                {
                    if (length < this.length)
                    {
                        this.$this._logger.Warn($"The length of the data is less than 'length':
  expected: {this.length}
  actual: {length}");
                    }
                    bool flag = this.$this.send(Opcode.Binary, new MemoryStream(data));
                    if (this.completed != null)
                    {
                        this.completed(flag);
                    }
                }
            }

            internal void <>m__1(Exception ex)
            {
                this.$this._logger.Error(ex.ToString());
                this.$this.error("An exception has occurred while sending data.", ex);
            }
        }

        [CompilerGenerated]
        private sealed class <startReceiving>c__AnonStorey4
        {
            internal Action receive;
            internal WebSocket $this;

            internal void <>m__0()
            {
                WebSocketFrame.ReadFrameAsync(this.$this._stream, false, delegate (WebSocketFrame frame) {
                    if (!this.$this.processReceivedFrame(frame) || (this.$this._readyState == 3))
                    {
                        AutoResetEvent event2 = this.$this._exitReceiving;
                        if (event2 != null)
                        {
                            event2.Set();
                        }
                    }
                    else
                    {
                        this.receive();
                        if (!this.$this._inMessage && (this.$this.HasMessage && (this.$this._readyState == 1)))
                        {
                            this.$this.message();
                        }
                    }
                }, delegate (Exception ex) {
                    this.$this._logger.Fatal(ex.ToString());
                    this.$this.fatal("An exception has occurred while receiving.", ex);
                });
            }

            internal void <>m__1(WebSocketFrame frame)
            {
                if (!this.$this.processReceivedFrame(frame) || (this.$this._readyState == 3))
                {
                    AutoResetEvent event2 = this.$this._exitReceiving;
                    if (event2 != null)
                    {
                        event2.Set();
                    }
                }
                else
                {
                    this.receive();
                    if (!this.$this._inMessage && (this.$this.HasMessage && (this.$this._readyState == 1)))
                    {
                        this.$this.message();
                    }
                }
            }

            internal void <>m__2(Exception ex)
            {
                this.$this._logger.Fatal(ex.ToString());
                this.$this.fatal("An exception has occurred while receiving.", ex);
            }
        }

        [CompilerGenerated]
        private sealed class <validateSecWebSocketExtensionsServerHeader>c__AnonStorey5
        {
            internal string method;

            internal bool <>m__0(string t)
            {
                t = t.Trim();
                return (((t != this.method) && (t != "server_no_context_takeover")) && (t != "client_no_context_takeover"));
            }
        }

        [CompilerGenerated]
        private sealed class <validateSecWebSocketProtocolServerHeader>c__AnonStorey6
        {
            internal string value;

            internal bool <>m__0(string p) => 
                p == this.value;
        }
    }
}

