namespace WebSocketSharp.Server
{
    using System;
    using WebSocketSharp;

    internal class WebSocketServiceHost<TBehavior> : WebSocketServiceHost where TBehavior: WebSocketBehavior
    {
        private Func<TBehavior> _initializer;
        private Logger _logger;
        private string _path;
        private WebSocketSessionManager _sessions;

        internal WebSocketServiceHost(string path, Func<TBehavior> initializer, Logger logger)
        {
            this._path = path;
            this._initializer = initializer;
            this._logger = logger;
            this._sessions = new WebSocketSessionManager(logger);
        }

        protected override WebSocketBehavior CreateSession() => 
            this._initializer();

        public override bool KeepClean
        {
            get => 
                this._sessions.KeepClean;
            set
            {
                string message = this._sessions.State.CheckIfAvailable(true, false, false);
                if (message != null)
                {
                    this._logger.Error(message);
                }
                else
                {
                    this._sessions.KeepClean = value;
                }
            }
        }

        public override string Path =>
            this._path;

        public override WebSocketSessionManager Sessions =>
            this._sessions;

        public override System.Type Type =>
            typeof(TBehavior);

        public override TimeSpan WaitTime
        {
            get => 
                this._sessions.WaitTime;
            set
            {
                string message = this._sessions.State.CheckIfAvailable(true, false, false) ?? value.CheckIfValidWaitTime();
                if (message != null)
                {
                    this._logger.Error(message);
                }
                else
                {
                    this._sessions.WaitTime = value;
                }
            }
        }
    }
}

