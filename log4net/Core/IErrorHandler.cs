namespace log4net.Core
{
    using System;

    public interface IErrorHandler
    {
        void Error(string message);
        void Error(string message, Exception e);
        void Error(string message, Exception e, ErrorCode errorCode);
    }
}

