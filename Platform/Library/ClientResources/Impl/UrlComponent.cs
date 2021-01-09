namespace Platform.Library.ClientResources.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class UrlComponent : Component
    {
        public UrlComponent()
        {
            this.Caching = true;
        }

        public UrlComponent(string url, Hash128 hash, uint crc)
        {
            this.Url = url;
            this.CRC = crc;
            this.Hash = hash;
            this.Caching = true;
        }

        public string Url { get; set; }

        public Hash128 Hash { get; set; }

        public uint CRC { get; set; }

        public bool Caching { get; set; }

        public bool NoErrorEvent { get; set; }
    }
}

