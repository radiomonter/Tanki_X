namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    public static class SteamGameServer
    {
        public static SteamAPICall_t AssociateWithClan(CSteamID steamIDClan)
        {
            InteropHelp.TestIfAvailableGameServer();
            return (SteamAPICall_t) NativeMethods.ISteamGameServer_AssociateWithClan(steamIDClan);
        }

        public static EBeginAuthSessionResult BeginAuthSession(byte[] pAuthTicket, int cbAuthTicket, CSteamID steamID)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServer_BeginAuthSession(pAuthTicket, cbAuthTicket, steamID);
        }

        public static bool BLoggedOn()
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServer_BLoggedOn();
        }

        public static bool BSecure()
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServer_BSecure();
        }

        public static bool BUpdateUserData(CSteamID steamIDUser, string pchPlayerName, uint uScore)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchPlayerName))
            {
                return NativeMethods.ISteamGameServer_BUpdateUserData(steamIDUser, handle, uScore);
            }
        }

        public static void CancelAuthTicket(HAuthTicket hAuthTicket)
        {
            InteropHelp.TestIfAvailableGameServer();
            NativeMethods.ISteamGameServer_CancelAuthTicket(hAuthTicket);
        }

        public static void ClearAllKeyValues()
        {
            InteropHelp.TestIfAvailableGameServer();
            NativeMethods.ISteamGameServer_ClearAllKeyValues();
        }

        public static SteamAPICall_t ComputeNewPlayerCompatibility(CSteamID steamIDNewPlayer)
        {
            InteropHelp.TestIfAvailableGameServer();
            return (SteamAPICall_t) NativeMethods.ISteamGameServer_ComputeNewPlayerCompatibility(steamIDNewPlayer);
        }

        public static CSteamID CreateUnauthenticatedUserConnection()
        {
            InteropHelp.TestIfAvailableGameServer();
            return (CSteamID) NativeMethods.ISteamGameServer_CreateUnauthenticatedUserConnection();
        }

        public static void EnableHeartbeats(bool bActive)
        {
            InteropHelp.TestIfAvailableGameServer();
            NativeMethods.ISteamGameServer_EnableHeartbeats(bActive);
        }

        public static void EndAuthSession(CSteamID steamID)
        {
            InteropHelp.TestIfAvailableGameServer();
            NativeMethods.ISteamGameServer_EndAuthSession(steamID);
        }

        public static void ForceHeartbeat()
        {
            InteropHelp.TestIfAvailableGameServer();
            NativeMethods.ISteamGameServer_ForceHeartbeat();
        }

        public static HAuthTicket GetAuthSessionTicket(byte[] pTicket, int cbMaxTicket, out uint pcbTicket)
        {
            InteropHelp.TestIfAvailableGameServer();
            return (HAuthTicket) NativeMethods.ISteamGameServer_GetAuthSessionTicket(pTicket, cbMaxTicket, out pcbTicket);
        }

        public static void GetGameplayStats()
        {
            InteropHelp.TestIfAvailableGameServer();
            NativeMethods.ISteamGameServer_GetGameplayStats();
        }

        public static int GetNextOutgoingPacket(byte[] pOut, int cbMaxOut, out uint pNetAdr, out ushort pPort)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServer_GetNextOutgoingPacket(pOut, cbMaxOut, out pNetAdr, out pPort);
        }

        public static uint GetPublicIP()
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServer_GetPublicIP();
        }

        public static SteamAPICall_t GetServerReputation()
        {
            InteropHelp.TestIfAvailableGameServer();
            return (SteamAPICall_t) NativeMethods.ISteamGameServer_GetServerReputation();
        }

        public static CSteamID GetSteamID()
        {
            InteropHelp.TestIfAvailableGameServer();
            return (CSteamID) NativeMethods.ISteamGameServer_GetSteamID();
        }

        public static bool HandleIncomingPacket(byte[] pData, int cbData, uint srcIP, ushort srcPort)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServer_HandleIncomingPacket(pData, cbData, srcIP, srcPort);
        }

        public static bool InitGameServer(uint unIP, ushort usGamePort, ushort usQueryPort, uint unFlags, AppId_t nGameAppId, string pchVersionString)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchVersionString))
            {
                return NativeMethods.ISteamGameServer_InitGameServer(unIP, usGamePort, usQueryPort, unFlags, nGameAppId, handle);
            }
        }

        public static void LogOff()
        {
            InteropHelp.TestIfAvailableGameServer();
            NativeMethods.ISteamGameServer_LogOff();
        }

        public static void LogOn(string pszToken)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pszToken))
            {
                NativeMethods.ISteamGameServer_LogOn(handle);
            }
        }

        public static void LogOnAnonymous()
        {
            InteropHelp.TestIfAvailableGameServer();
            NativeMethods.ISteamGameServer_LogOnAnonymous();
        }

        public static bool RequestUserGroupStatus(CSteamID steamIDUser, CSteamID steamIDGroup)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServer_RequestUserGroupStatus(steamIDUser, steamIDGroup);
        }

        public static bool SendUserConnectAndAuthenticate(uint unIPClient, byte[] pvAuthBlob, uint cubAuthBlobSize, out CSteamID pSteamIDUser)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServer_SendUserConnectAndAuthenticate(unIPClient, pvAuthBlob, cubAuthBlobSize, out pSteamIDUser);
        }

        public static void SendUserDisconnect(CSteamID steamIDUser)
        {
            InteropHelp.TestIfAvailableGameServer();
            NativeMethods.ISteamGameServer_SendUserDisconnect(steamIDUser);
        }

        public static void SetBotPlayerCount(int cBotplayers)
        {
            InteropHelp.TestIfAvailableGameServer();
            NativeMethods.ISteamGameServer_SetBotPlayerCount(cBotplayers);
        }

        public static void SetDedicatedServer(bool bDedicated)
        {
            InteropHelp.TestIfAvailableGameServer();
            NativeMethods.ISteamGameServer_SetDedicatedServer(bDedicated);
        }

        public static void SetGameData(string pchGameData)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchGameData))
            {
                NativeMethods.ISteamGameServer_SetGameData(handle);
            }
        }

        public static void SetGameDescription(string pszGameDescription)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pszGameDescription))
            {
                NativeMethods.ISteamGameServer_SetGameDescription(handle);
            }
        }

        public static void SetGameTags(string pchGameTags)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchGameTags))
            {
                NativeMethods.ISteamGameServer_SetGameTags(handle);
            }
        }

        public static void SetHeartbeatInterval(int iHeartbeatInterval)
        {
            InteropHelp.TestIfAvailableGameServer();
            NativeMethods.ISteamGameServer_SetHeartbeatInterval(iHeartbeatInterval);
        }

        public static void SetKeyValue(string pKey, string pValue)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pKey))
            {
                using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pValue))
                {
                    NativeMethods.ISteamGameServer_SetKeyValue(handle, handle2);
                }
            }
        }

        public static void SetMapName(string pszMapName)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pszMapName))
            {
                NativeMethods.ISteamGameServer_SetMapName(handle);
            }
        }

        public static void SetMaxPlayerCount(int cPlayersMax)
        {
            InteropHelp.TestIfAvailableGameServer();
            NativeMethods.ISteamGameServer_SetMaxPlayerCount(cPlayersMax);
        }

        public static void SetModDir(string pszModDir)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pszModDir))
            {
                NativeMethods.ISteamGameServer_SetModDir(handle);
            }
        }

        public static void SetPasswordProtected(bool bPasswordProtected)
        {
            InteropHelp.TestIfAvailableGameServer();
            NativeMethods.ISteamGameServer_SetPasswordProtected(bPasswordProtected);
        }

        public static void SetProduct(string pszProduct)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pszProduct))
            {
                NativeMethods.ISteamGameServer_SetProduct(handle);
            }
        }

        public static void SetRegion(string pszRegion)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pszRegion))
            {
                NativeMethods.ISteamGameServer_SetRegion(handle);
            }
        }

        public static void SetServerName(string pszServerName)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pszServerName))
            {
                NativeMethods.ISteamGameServer_SetServerName(handle);
            }
        }

        public static void SetSpectatorPort(ushort unSpectatorPort)
        {
            InteropHelp.TestIfAvailableGameServer();
            NativeMethods.ISteamGameServer_SetSpectatorPort(unSpectatorPort);
        }

        public static void SetSpectatorServerName(string pszSpectatorServerName)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pszSpectatorServerName))
            {
                NativeMethods.ISteamGameServer_SetSpectatorServerName(handle);
            }
        }

        public static EUserHasLicenseForAppResult UserHasLicenseForApp(CSteamID steamID, AppId_t appID)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServer_UserHasLicenseForApp(steamID, appID);
        }

        public static bool WasRestartRequested()
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServer_WasRestartRequested();
        }
    }
}

