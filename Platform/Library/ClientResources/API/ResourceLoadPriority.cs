namespace Platform.Library.ClientResources.API
{
    using System;
    using System.ComponentModel;

    [DefaultValue(0)]
    public enum ResourceLoadPriority
    {
        LOW = -100,
        USUAL = 0,
        HIGH = 100
    }
}

