namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential, Pack=4)]
    public struct CSteamID : IEquatable<CSteamID>, IComparable<CSteamID>
    {
        public static readonly CSteamID Nil;
        public static readonly CSteamID OutofDateGS;
        public static readonly CSteamID LanModeGS;
        public static readonly CSteamID NotInitYetGS;
        public static readonly CSteamID NonSteamGS;
        public ulong m_SteamID;
        public CSteamID(AccountID_t unAccountID, EUniverse eUniverse, EAccountType eAccountType)
        {
            this.m_SteamID = 0UL;
            this.Set(unAccountID, eUniverse, eAccountType);
        }

        public CSteamID(AccountID_t unAccountID, uint unAccountInstance, EUniverse eUniverse, EAccountType eAccountType)
        {
            this.m_SteamID = 0UL;
            this.InstancedSet(unAccountID, unAccountInstance, eUniverse, eAccountType);
        }

        public CSteamID(ulong ulSteamID)
        {
            this.m_SteamID = ulSteamID;
        }

        public void Set(AccountID_t unAccountID, EUniverse eUniverse, EAccountType eAccountType)
        {
            this.SetAccountID(unAccountID);
            this.SetEUniverse(eUniverse);
            this.SetEAccountType(eAccountType);
            if ((eAccountType != EAccountType.k_EAccountTypeClan) && (eAccountType != EAccountType.k_EAccountTypeGameServer))
            {
                this.SetAccountInstance(1);
            }
            else
            {
                this.SetAccountInstance(0);
            }
        }

        public void InstancedSet(AccountID_t unAccountID, uint unInstance, EUniverse eUniverse, EAccountType eAccountType)
        {
            this.SetAccountID(unAccountID);
            this.SetEUniverse(eUniverse);
            this.SetEAccountType(eAccountType);
            this.SetAccountInstance(unInstance);
        }

        public void Clear()
        {
            this.m_SteamID = 0UL;
        }

        public void CreateBlankAnonLogon(EUniverse eUniverse)
        {
            this.SetAccountID(new AccountID_t(0));
            this.SetEUniverse(eUniverse);
            this.SetEAccountType(EAccountType.k_EAccountTypeAnonGameServer);
            this.SetAccountInstance(0);
        }

        public void CreateBlankAnonUserLogon(EUniverse eUniverse)
        {
            this.SetAccountID(new AccountID_t(0));
            this.SetEUniverse(eUniverse);
            this.SetEAccountType(EAccountType.k_EAccountTypeAnonUser);
            this.SetAccountInstance(0);
        }

        public bool BBlankAnonAccount() => 
            ((this.GetAccountID() == new AccountID_t(0)) && this.BAnonAccount()) && (this.GetUnAccountInstance() == 0);

        public bool BGameServerAccount() => 
            (this.GetEAccountType() == EAccountType.k_EAccountTypeGameServer) || (this.GetEAccountType() == EAccountType.k_EAccountTypeAnonGameServer);

        public bool BPersistentGameServerAccount() => 
            this.GetEAccountType() == EAccountType.k_EAccountTypeGameServer;

        public bool BAnonGameServerAccount() => 
            this.GetEAccountType() == EAccountType.k_EAccountTypeAnonGameServer;

        public bool BContentServerAccount() => 
            this.GetEAccountType() == EAccountType.k_EAccountTypeContentServer;

        public bool BClanAccount() => 
            this.GetEAccountType() == EAccountType.k_EAccountTypeClan;

        public bool BChatAccount() => 
            this.GetEAccountType() == EAccountType.k_EAccountTypeChat;

        public bool IsLobby() => 
            (this.GetEAccountType() == EAccountType.k_EAccountTypeChat) && ((this.GetUnAccountInstance() & 0x40000) != 0);

        public bool BIndividualAccount() => 
            (this.GetEAccountType() == EAccountType.k_EAccountTypeIndividual) || (this.GetEAccountType() == EAccountType.k_EAccountTypeConsoleUser);

        public bool BAnonAccount() => 
            (this.GetEAccountType() == EAccountType.k_EAccountTypeAnonUser) || (this.GetEAccountType() == EAccountType.k_EAccountTypeAnonGameServer);

        public bool BAnonUserAccount() => 
            this.GetEAccountType() == EAccountType.k_EAccountTypeAnonUser;

        public bool BConsoleUserAccount() => 
            this.GetEAccountType() == EAccountType.k_EAccountTypeConsoleUser;

        public void SetAccountID(AccountID_t other)
        {
            this.m_SteamID = (this.m_SteamID & 18446744069414584320UL) | ((((uint) other) & 0xffffffffUL) << 0);
        }

        public void SetAccountInstance(uint other)
        {
            this.m_SteamID = (this.m_SteamID & 18442240478377148415UL) | ((ulong) ((other & 0xfffffL) << 0x20));
        }

        public void SetEAccountType(EAccountType other)
        {
            this.m_SteamID = (this.m_SteamID & 18379190079298994175UL) | ((ulong) ((((long) other) & 15) << 0x34));
        }

        public void SetEUniverse(EUniverse other)
        {
            this.m_SteamID = (this.m_SteamID & ((ulong) 0xffffffffffffffL)) | ((ulong) ((((long) other) & 0xffL) << 0x38));
        }

        public void ClearIndividualInstance()
        {
            if (this.BIndividualAccount())
            {
                this.SetAccountInstance(0);
            }
        }

        public bool HasNoIndividualInstance() => 
            this.BIndividualAccount() && (this.GetUnAccountInstance() == 0);

        public AccountID_t GetAccountID() => 
            new AccountID_t((uint) (this.m_SteamID & 0xffffffffUL));

        public uint GetUnAccountInstance() => 
            (uint) ((this.m_SteamID >> 0x20) & ((ulong) 0xfffffL));

        public EAccountType GetEAccountType() => 
            (EAccountType) ((int) ((this.m_SteamID >> 0x34) & 15));

        public EUniverse GetEUniverse() => 
            (EUniverse) ((int) ((this.m_SteamID >> 0x38) & ((ulong) 0xffL)));

        public bool IsValid() => 
            (this.GetEAccountType() > EAccountType.k_EAccountTypeInvalid) && ((this.GetEAccountType() < EAccountType.k_EAccountTypeMax) && ((this.GetEUniverse() > EUniverse.k_EUniverseInvalid) && ((this.GetEUniverse() < EUniverse.k_EUniverseMax) && (((this.GetEAccountType() != EAccountType.k_EAccountTypeIndividual) || ((this.GetAccountID() != new AccountID_t(0)) && (this.GetUnAccountInstance() <= 4))) ? (((this.GetEAccountType() != EAccountType.k_EAccountTypeClan) || ((this.GetAccountID() != new AccountID_t(0)) && (this.GetUnAccountInstance() == 0))) ? ((this.GetEAccountType() != EAccountType.k_EAccountTypeGameServer) || !(this.GetAccountID() == new AccountID_t(0))) : false) : false))));

        public override string ToString() => 
            this.m_SteamID.ToString();

        public override bool Equals(object other) => 
            (other is CSteamID) && (this == ((CSteamID) other));

        public override int GetHashCode() => 
            this.m_SteamID.GetHashCode();

        public static bool operator ==(CSteamID x, CSteamID y) => 
            x.m_SteamID == y.m_SteamID;

        public static bool operator !=(CSteamID x, CSteamID y) => 
            !(x == y);

        public static explicit operator CSteamID(ulong value) => 
            new CSteamID(value);

        public static explicit operator ulong(CSteamID that) => 
            that.m_SteamID;

        public bool Equals(CSteamID other) => 
            this.m_SteamID == other.m_SteamID;

        public int CompareTo(CSteamID other) => 
            this.m_SteamID.CompareTo(other.m_SteamID);

        static CSteamID()
        {
            CSteamID mid = new CSteamID();
            Nil = mid;
            OutofDateGS = new CSteamID(new AccountID_t(0), 0, EUniverse.k_EUniverseInvalid, EAccountType.k_EAccountTypeInvalid);
            LanModeGS = new CSteamID(new AccountID_t(0), 0, EUniverse.k_EUniversePublic, EAccountType.k_EAccountTypeInvalid);
            NotInitYetGS = new CSteamID(new AccountID_t(1), 0, EUniverse.k_EUniverseInvalid, EAccountType.k_EAccountTypeInvalid);
            NonSteamGS = new CSteamID(new AccountID_t(2), 0, EUniverse.k_EUniverseInvalid, EAccountType.k_EAccountTypeInvalid);
        }
    }
}

