namespace Platform.Library.ClientResources.API
{
    using System;

    public enum AssetBundleDiskCacheState
    {
        INIT,
        START_LOAD_FROM_HTTP,
        LOADING_FROM_HTTP,
        LOADING_FROM_MEMORY,
        START_LOAD_FROM_DISK,
        START_WRITE_TO_DISK,
        WRITE_TO_DISK,
        START_LOAD_FROM_MEMORY,
        CREATE_ASSETBUNDLE,
        COMPLETE
    }
}

