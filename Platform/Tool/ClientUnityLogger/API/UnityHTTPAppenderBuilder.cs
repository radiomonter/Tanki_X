namespace Platform.Tool.ClientUnityLogger.API
{
    using Platform.Library.ClientLogger.Impl;
    using System;

    public class UnityHTTPAppenderBuilder : AppenderBuilder
    {
        public UnityHTTPAppenderBuilder(string url)
        {
            UnityHTTPAppender appender = new UnityHTTPAppender {
                url = url
            };
            base.Init(appender);
        }
    }
}

