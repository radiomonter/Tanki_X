namespace Platform.Library.ClientUnityIntegration.API
{
    using System;

    public interface Loader : IDisposable
    {
        byte[] Bytes { get; }

        float Progress { get; }

        bool IsDone { get; }

        string URL { get; }

        string Error { get; }
    }
}

