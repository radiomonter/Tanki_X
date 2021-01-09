namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    public static class SteamGameServerStats
    {
        public static bool ClearUserAchievement(CSteamID steamIDUser, string pchName)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchName))
            {
                return NativeMethods.ISteamGameServerStats_ClearUserAchievement(steamIDUser, handle);
            }
        }

        public static bool GetUserAchievement(CSteamID steamIDUser, string pchName, out bool pbAchieved)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchName))
            {
                return NativeMethods.ISteamGameServerStats_GetUserAchievement(steamIDUser, handle, out pbAchieved);
            }
        }

        public static bool GetUserStat(CSteamID steamIDUser, string pchName, out int pData)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchName))
            {
                return NativeMethods.ISteamGameServerStats_GetUserStat(steamIDUser, handle, out pData);
            }
        }

        public static bool GetUserStat(CSteamID steamIDUser, string pchName, out float pData)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchName))
            {
                return NativeMethods.ISteamGameServerStats_GetUserStat_(steamIDUser, handle, out pData);
            }
        }

        public static SteamAPICall_t RequestUserStats(CSteamID steamIDUser)
        {
            InteropHelp.TestIfAvailableGameServer();
            return (SteamAPICall_t) NativeMethods.ISteamGameServerStats_RequestUserStats(steamIDUser);
        }

        public static bool SetUserAchievement(CSteamID steamIDUser, string pchName)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchName))
            {
                return NativeMethods.ISteamGameServerStats_SetUserAchievement(steamIDUser, handle);
            }
        }

        public static bool SetUserStat(CSteamID steamIDUser, string pchName, int nData)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchName))
            {
                return NativeMethods.ISteamGameServerStats_SetUserStat(steamIDUser, handle, nData);
            }
        }

        public static bool SetUserStat(CSteamID steamIDUser, string pchName, float fData)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchName))
            {
                return NativeMethods.ISteamGameServerStats_SetUserStat_(steamIDUser, handle, fData);
            }
        }

        public static SteamAPICall_t StoreUserStats(CSteamID steamIDUser)
        {
            InteropHelp.TestIfAvailableGameServer();
            return (SteamAPICall_t) NativeMethods.ISteamGameServerStats_StoreUserStats(steamIDUser);
        }

        public static bool UpdateUserAvgRateStat(CSteamID steamIDUser, string pchName, float flCountThisSession, double dSessionLength)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchName))
            {
                return NativeMethods.ISteamGameServerStats_UpdateUserAvgRateStat(steamIDUser, handle, flCountThisSession, dSessionLength);
            }
        }
    }
}

