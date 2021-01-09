namespace Platform.Tool.ClientUnityLogger.API
{
    using System;

    public class FatalException : Exception
    {
        public FatalException()
        {
        }

        public FatalException(string message) : base(message)
        {
        }
    }
}

