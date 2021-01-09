namespace Platform.Library.ClientResources.API
{
    using System;
    using UnityEngine;

    public interface LoadAssetFromBundleRequest
    {
        void StartExecute();

        Object Asset { get; }

        bool IsDone { get; }

        bool IsStarted { get; }

        AssetBundle Bundle { get; }

        string ObjectName { get; }

        Type ObjectType { get; }
    }
}

