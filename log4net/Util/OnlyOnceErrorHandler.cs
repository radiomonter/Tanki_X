namespace log4net.Util
{
    using log4net.Core;
    using System;

    public class OnlyOnceErrorHandler : IErrorHandler
    {
        private DateTime m_enabledDate;
        private bool m_firstTime;
        private string m_message;
        private System.Exception m_exception;
        private log4net.Core.ErrorCode m_errorCode;
        private readonly string m_prefix;
        private static readonly Type declaringType = typeof(OnlyOnceErrorHandler);

        public OnlyOnceErrorHandler()
        {
            this.m_firstTime = true;
            this.m_prefix = string.Empty;
        }

        public OnlyOnceErrorHandler(string prefix)
        {
            this.m_firstTime = true;
            this.m_prefix = prefix;
        }

        public void Error(string message)
        {
            this.Error(message, null, log4net.Core.ErrorCode.GenericFailure);
        }

        public void Error(string message, System.Exception e)
        {
            this.Error(message, e, log4net.Core.ErrorCode.GenericFailure);
        }

        public void Error(string message, System.Exception e, log4net.Core.ErrorCode errorCode)
        {
            if (this.m_firstTime)
            {
                this.FirstError(message, e, errorCode);
            }
        }

        public virtual void FirstError(string message, System.Exception e, log4net.Core.ErrorCode errorCode)
        {
            this.m_enabledDate = DateTime.Now;
            this.m_errorCode = errorCode;
            this.m_exception = e;
            this.m_message = message;
            this.m_firstTime = false;
            if (LogLog.InternalDebugging && !LogLog.QuietMode)
            {
                string[] textArray1 = new string[] { "[", this.m_prefix, "] ErrorCode: ", errorCode.ToString(), ". ", message };
                LogLog.Error(declaringType, string.Concat(textArray1), e);
            }
        }

        public void Reset()
        {
            this.m_enabledDate = DateTime.MinValue;
            this.m_errorCode = log4net.Core.ErrorCode.GenericFailure;
            this.m_exception = null;
            this.m_message = null;
            this.m_firstTime = true;
        }

        public bool IsEnabled =>
            this.m_firstTime;

        public DateTime EnabledDate =>
            this.m_enabledDate;

        public string ErrorMessage =>
            this.m_message;

        public System.Exception Exception =>
            this.m_exception;

        public log4net.Core.ErrorCode ErrorCode =>
            this.m_errorCode;
    }
}

