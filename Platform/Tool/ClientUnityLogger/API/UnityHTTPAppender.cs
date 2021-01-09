namespace Platform.Tool.ClientUnityLogger.API
{
    using log4net.Appender;
    using log4net.Core;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Text;
    using UnityEngine;

    public class UnityHTTPAppender : AppenderSkeleton
    {
        private int errorMessagesCounter;
        private int messagesCounter;
        private List<WWWLoader> wwwLoaders = new List<WWWLoader>();

        public UnityHTTPAppender()
        {
            this.url = "http://localhost";
            this.maxErrorMessages = 10;
            this.maxMessages = 0x7fffffff;
            this.timeoutSeconds = 10;
        }

        private void AddLoader(WWW www)
        {
            WWWLoader item = new WWWLoader(www) {
                MaxRestartAttempts = 0,
                TimeoutSeconds = this.timeoutSeconds
            };
            this.wwwLoaders.Add(item);
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            this.RemoveExpiredLoaders();
            this.messagesCounter++;
            if (loggingEvent.Level >= Level.Error)
            {
                this.errorMessagesCounter++;
            }
            if ((this.messagesCounter <= this.maxMessages) && (this.errorMessagesCounter <= this.maxErrorMessages))
            {
                string s = base.RenderLoggingEvent(loggingEvent);
                WWW www = new WWW(this.url, Encoding.UTF8.GetBytes(s));
                this.AddLoader(www);
            }
        }

        private void RemoveExpiredLoaders()
        {
            for (int i = 0; i < this.wwwLoaders.Count; i++)
            {
                WWWLoader loader = this.wwwLoaders[i];
                if (loader.IsDone)
                {
                    if (!string.IsNullOrEmpty(loader.Error))
                    {
                        object[] args = new object[] { loader.Error };
                        Debug.LogWarningFormat("UnityHTTPAppender: {0}", args);
                    }
                    loader.Dispose();
                    this.wwwLoaders.RemoveAt(i);
                    i--;
                }
            }
        }

        public string url { get; set; }

        public int maxErrorMessages { get; set; }

        public int maxMessages { get; set; }

        public int timeoutSeconds { get; set; }
    }
}

