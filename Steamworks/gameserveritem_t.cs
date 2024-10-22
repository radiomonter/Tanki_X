﻿namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    [Serializable, StructLayout(LayoutKind.Sequential, Size=0x174, Pack=4)]
    public class gameserveritem_t
    {
        public servernetadr_t m_NetAdr;
        public int m_nPing;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bHadSuccessfulResponse;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bDoNotRefresh;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x20)]
        private byte[] m_szGameDir;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x20)]
        private byte[] m_szMap;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x40)]
        private byte[] m_szGameDescription;
        public uint m_nAppID;
        public int m_nPlayers;
        public int m_nMaxPlayers;
        public int m_nBotPlayers;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bPassword;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bSecure;
        public uint m_ulTimeLastPlayed;
        public int m_nServerVersion;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x40)]
        private byte[] m_szServerName;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x80)]
        private byte[] m_szGameTags;
        public CSteamID m_steamID;
        public string GetGameDir() => 
            Encoding.UTF8.GetString(this.m_szGameDir, 0, Array.IndexOf<byte>(this.m_szGameDir, 0));

        public void SetGameDir(string dir)
        {
            this.m_szGameDir = Encoding.UTF8.GetBytes(dir + '\0');
        }

        public string GetMap() => 
            Encoding.UTF8.GetString(this.m_szMap, 0, Array.IndexOf<byte>(this.m_szMap, 0));

        public void SetMap(string map)
        {
            this.m_szMap = Encoding.UTF8.GetBytes(map + '\0');
        }

        public string GetGameDescription() => 
            Encoding.UTF8.GetString(this.m_szGameDescription, 0, Array.IndexOf<byte>(this.m_szGameDescription, 0));

        public void SetGameDescription(string desc)
        {
            this.m_szGameDescription = Encoding.UTF8.GetBytes(desc + '\0');
        }

        public string GetServerName() => 
            (this.m_szServerName[0] != 0) ? Encoding.UTF8.GetString(this.m_szServerName, 0, Array.IndexOf<byte>(this.m_szServerName, 0)) : this.m_NetAdr.GetConnectionAddressString();

        public void SetServerName(string name)
        {
            this.m_szServerName = Encoding.UTF8.GetBytes(name + '\0');
        }

        public string GetGameTags() => 
            Encoding.UTF8.GetString(this.m_szGameTags, 0, Array.IndexOf<byte>(this.m_szGameTags, 0));

        public void SetGameTags(string tags)
        {
            this.m_szGameTags = Encoding.UTF8.GetBytes(tags + '\0');
        }
    }
}

