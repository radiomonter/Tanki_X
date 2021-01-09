namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct CGameID : IEquatable<CGameID>, IComparable<CGameID>
    {
        public ulong m_GameID;
        public CGameID(ulong GameID)
        {
            this.m_GameID = GameID;
        }

        public CGameID(AppId_t nAppID)
        {
            this.m_GameID = 0UL;
            this.SetAppID(nAppID);
        }

        public CGameID(AppId_t nAppID, uint nModID)
        {
            this.m_GameID = 0UL;
            this.SetAppID(nAppID);
            this.SetType(EGameIDType.k_EGameIDTypeGameMod);
            this.SetModID(nModID);
        }

        public bool IsSteamApp() => 
            this.Type() == EGameIDType.k_EGameIDTypeApp;

        public bool IsMod() => 
            this.Type() == EGameIDType.k_EGameIDTypeGameMod;

        public bool IsShortcut() => 
            this.Type() == EGameIDType.k_EGameIDTypeShortcut;

        public bool IsP2PFile() => 
            this.Type() == EGameIDType.k_EGameIDTypeP2P;

        public AppId_t AppID() => 
            new AppId_t((uint) (this.m_GameID & ((ulong) 0xffffffL)));

        public EGameIDType Type() => 
            (EGameIDType) ((int) ((this.m_GameID >> 0x18) & ((ulong) 0xffL)));

        public uint ModID() => 
            (uint) ((this.m_GameID >> 0x20) & 0xffffffffUL);

        public bool IsValid()
        {
            switch (this.Type())
            {
                case EGameIDType.k_EGameIDTypeApp:
                    return (this.AppID() != AppId_t.Invalid);

                case EGameIDType.k_EGameIDTypeGameMod:
                    return ((this.AppID() != AppId_t.Invalid) && ((this.ModID() & 0x80000000) != 0));

                case EGameIDType.k_EGameIDTypeShortcut:
                    return ((this.ModID() & 0x80000000) != 0);

                case EGameIDType.k_EGameIDTypeP2P:
                    return ((this.AppID() == AppId_t.Invalid) && ((this.ModID() & 0x80000000) != 0));
            }
            return false;
        }

        public void Reset()
        {
            this.m_GameID = 0UL;
        }

        public void Set(ulong GameID)
        {
            this.m_GameID = GameID;
        }

        private void SetAppID(AppId_t other)
        {
            this.m_GameID = (this.m_GameID & 18446744073692774400UL) | ((ulong) ((((uint) other) & 0xffffffL) << 0));
        }

        private void SetType(EGameIDType other)
        {
            this.m_GameID = (this.m_GameID & 18446744069431361535UL) | ((ulong) ((((long) other) & 0xffL) << 0x18));
        }

        private void SetModID(uint other)
        {
            this.m_GameID = (this.m_GameID & 0xffffffffUL) | ((other & 0xffffffffUL) << 0x20);
        }

        public override string ToString() => 
            this.m_GameID.ToString();

        public override bool Equals(object other) => 
            (other is CGameID) && (this == ((CGameID) other));

        public override int GetHashCode() => 
            this.m_GameID.GetHashCode();

        public static bool operator ==(CGameID x, CGameID y) => 
            x.m_GameID == y.m_GameID;

        public static bool operator !=(CGameID x, CGameID y) => 
            !(x == y);

        public static explicit operator CGameID(ulong value) => 
            new CGameID(value);

        public static explicit operator ulong(CGameID that) => 
            that.m_GameID;

        public bool Equals(CGameID other) => 
            this.m_GameID == other.m_GameID;

        public int CompareTo(CGameID other) => 
            this.m_GameID.CompareTo(other.m_GameID);
        public enum EGameIDType
        {
            k_EGameIDTypeApp,
            k_EGameIDTypeGameMod,
            k_EGameIDTypeShortcut,
            k_EGameIDTypeP2P
        }
    }
}

