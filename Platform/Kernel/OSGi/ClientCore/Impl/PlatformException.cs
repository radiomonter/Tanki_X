namespace Platform.Kernel.OSGi.ClientCore.Impl
{
    using System;

    public class PlatformException : SystemException
    {
        public PlatformException()
        {
        }

        public PlatformException(Exception innerException) : base(string.Empty, innerException)
        {
        }

        public PlatformException(string message) : base(message)
        {
        }

        public PlatformException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

