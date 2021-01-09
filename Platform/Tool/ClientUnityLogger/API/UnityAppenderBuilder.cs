namespace Platform.Tool.ClientUnityLogger.API
{
    using Platform.Library.ClientLogger.Impl;
    using System;

    public class UnityAppenderBuilder : AppenderBuilder
    {
        public UnityAppenderBuilder()
        {
            base.Init(new UnityAppender());
        }
    }
}

