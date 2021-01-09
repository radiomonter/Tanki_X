namespace AmplifyBloom
{
    using System;
    using UnityEngine;

    [Serializable]
    public class VersionInfo
    {
        public const byte Major = 1;
        public const byte Minor = 1;
        public const byte Release = 2;
        private static string StageSuffix = "_dev001";
        [SerializeField]
        private int m_major;
        [SerializeField]
        private int m_minor;
        [SerializeField]
        private int m_release;

        private VersionInfo()
        {
            this.m_major = 1;
            this.m_minor = 1;
            this.m_release = 2;
        }

        private VersionInfo(byte major, byte minor, byte release)
        {
            this.m_major = major;
            this.m_minor = minor;
            this.m_release = release;
        }

        public static VersionInfo Current() => 
            new VersionInfo(1, 1, 2);

        public static bool Matches(VersionInfo version) => 
            ((version.m_major == 1) && (version.m_minor == 1)) && (2 == version.m_release);

        public static string StaticToString() => 
            $"{((byte) 1)}.{((byte) 1)}.{((byte) 2)}" + StageSuffix;

        public override string ToString() => 
            $"{this.m_major}.{this.m_minor}.{this.m_release}" + StageSuffix;

        public int Number =>
            ((this.m_major * 100) + (this.m_minor * 10)) + this.m_release;
    }
}

