namespace WebSocketSharp.Server
{
    using System;
    using WebSocketSharp;
    using WebSocketSharp.Net.WebSockets;

    public abstract class WebSocketServiceHost
    {
        protected WebSocketServiceHost()
        {
        }

        protected abstract WebSocketBehavior CreateSession();
        internal void Start()
        {
            this.Sessions.Start();
        }

        internal void StartSession(WebSocketContext context)
        {
            this.CreateSession().Start(context, this.Sessions);
        }

        internal void Stop(ushort code, string reason)
        {
            CloseEventArgs e = new CloseEventArgs(code, reason);
            bool receive = !code.IsReserved();
            this.Sessions.Stop(e, !receive ? null : WebSocketFrame.CreateCloseFrame(e.PayloadData, false).ToArray(), receive);
        }

        internal ServerState State =>
            this.Sessions.State;

        public abstract bool KeepClean { get; set; }

        public abstract string Path { get; }

        public abstract WebSocketSessionManager Sessions { get; }

        public abstract System.Type Type { get; }

        public abstract TimeSpan WaitTime { get; set; }
    }
}

