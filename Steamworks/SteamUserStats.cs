namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    public static class SteamUserStats
    {
        public static SteamAPICall_t AttachLeaderboardUGC(SteamLeaderboard_t hSteamLeaderboard, UGCHandle_t hUGC)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamUserStats_AttachLeaderboardUGC(hSteamLeaderboard, hUGC);
        }

        public static bool ClearAchievement(string pchName)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchName))
            {
                return NativeMethods.ISteamUserStats_ClearAchievement(handle);
            }
        }

        public static SteamAPICall_t DownloadLeaderboardEntries(SteamLeaderboard_t hSteamLeaderboard, ELeaderboardDataRequest eLeaderboardDataRequest, int nRangeStart, int nRangeEnd)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamUserStats_DownloadLeaderboardEntries(hSteamLeaderboard, eLeaderboardDataRequest, nRangeStart, nRangeEnd);
        }

        public static SteamAPICall_t DownloadLeaderboardEntriesForUsers(SteamLeaderboard_t hSteamLeaderboard, CSteamID[] prgUsers, int cUsers)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamUserStats_DownloadLeaderboardEntriesForUsers(hSteamLeaderboard, prgUsers, cUsers);
        }

        public static SteamAPICall_t FindLeaderboard(string pchLeaderboardName)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchLeaderboardName))
            {
                return (SteamAPICall_t) NativeMethods.ISteamUserStats_FindLeaderboard(handle);
            }
        }

        public static SteamAPICall_t FindOrCreateLeaderboard(string pchLeaderboardName, ELeaderboardSortMethod eLeaderboardSortMethod, ELeaderboardDisplayType eLeaderboardDisplayType)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchLeaderboardName))
            {
                return (SteamAPICall_t) NativeMethods.ISteamUserStats_FindOrCreateLeaderboard(handle, eLeaderboardSortMethod, eLeaderboardDisplayType);
            }
        }

        public static bool GetAchievement(string pchName, out bool pbAchieved)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchName))
            {
                return NativeMethods.ISteamUserStats_GetAchievement(handle, out pbAchieved);
            }
        }

        public static bool GetAchievementAchievedPercent(string pchName, out float pflPercent)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchName))
            {
                return NativeMethods.ISteamUserStats_GetAchievementAchievedPercent(handle, out pflPercent);
            }
        }

        public static bool GetAchievementAndUnlockTime(string pchName, out bool pbAchieved, out uint punUnlockTime)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchName))
            {
                return NativeMethods.ISteamUserStats_GetAchievementAndUnlockTime(handle, out pbAchieved, out punUnlockTime);
            }
        }

        public static string GetAchievementDisplayAttribute(string pchName, string pchKey)
        {
            string str;
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchName))
            {
                using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pchKey))
                {
                    str = InteropHelp.PtrToStringUTF8(NativeMethods.ISteamUserStats_GetAchievementDisplayAttribute(handle, handle2));
                }
            }
            return str;
        }

        public static int GetAchievementIcon(string pchName)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchName))
            {
                return NativeMethods.ISteamUserStats_GetAchievementIcon(handle);
            }
        }

        public static string GetAchievementName(uint iAchievement)
        {
            InteropHelp.TestIfAvailableClient();
            return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamUserStats_GetAchievementName(iAchievement));
        }

        public static bool GetDownloadedLeaderboardEntry(SteamLeaderboardEntries_t hSteamLeaderboardEntries, int index, out LeaderboardEntry_t pLeaderboardEntry, int[] pDetails, int cDetailsMax)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUserStats_GetDownloadedLeaderboardEntry(hSteamLeaderboardEntries, index, out pLeaderboardEntry, pDetails, cDetailsMax);
        }

        public static bool GetGlobalStat(string pchStatName, out double pData)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchStatName))
            {
                return NativeMethods.ISteamUserStats_GetGlobalStat_(handle, out pData);
            }
        }

        public static bool GetGlobalStat(string pchStatName, out long pData)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchStatName))
            {
                return NativeMethods.ISteamUserStats_GetGlobalStat(handle, out pData);
            }
        }

        public static int GetGlobalStatHistory(string pchStatName, double[] pData, uint cubData)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchStatName))
            {
                return NativeMethods.ISteamUserStats_GetGlobalStatHistory_(handle, pData, cubData);
            }
        }

        public static int GetGlobalStatHistory(string pchStatName, long[] pData, uint cubData)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchStatName))
            {
                return NativeMethods.ISteamUserStats_GetGlobalStatHistory(handle, pData, cubData);
            }
        }

        public static ELeaderboardDisplayType GetLeaderboardDisplayType(SteamLeaderboard_t hSteamLeaderboard)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUserStats_GetLeaderboardDisplayType(hSteamLeaderboard);
        }

        public static int GetLeaderboardEntryCount(SteamLeaderboard_t hSteamLeaderboard)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUserStats_GetLeaderboardEntryCount(hSteamLeaderboard);
        }

        public static string GetLeaderboardName(SteamLeaderboard_t hSteamLeaderboard)
        {
            InteropHelp.TestIfAvailableClient();
            return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamUserStats_GetLeaderboardName(hSteamLeaderboard));
        }

        public static ELeaderboardSortMethod GetLeaderboardSortMethod(SteamLeaderboard_t hSteamLeaderboard)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUserStats_GetLeaderboardSortMethod(hSteamLeaderboard);
        }

        public static int GetMostAchievedAchievementInfo(out string pchName, uint unNameBufLen, out float pflPercent, out bool pbAchieved)
        {
            InteropHelp.TestIfAvailableClient();
            IntPtr ptr = Marshal.AllocHGlobal((int) unNameBufLen);
            int num = NativeMethods.ISteamUserStats_GetMostAchievedAchievementInfo(ptr, unNameBufLen, out pflPercent, out pbAchieved);
            pchName = (num == -1) ? null : InteropHelp.PtrToStringUTF8(ptr);
            Marshal.FreeHGlobal(ptr);
            return num;
        }

        public static int GetNextMostAchievedAchievementInfo(int iIteratorPrevious, out string pchName, uint unNameBufLen, out float pflPercent, out bool pbAchieved)
        {
            InteropHelp.TestIfAvailableClient();
            IntPtr ptr = Marshal.AllocHGlobal((int) unNameBufLen);
            int num = NativeMethods.ISteamUserStats_GetNextMostAchievedAchievementInfo(iIteratorPrevious, ptr, unNameBufLen, out pflPercent, out pbAchieved);
            pchName = (num == -1) ? null : InteropHelp.PtrToStringUTF8(ptr);
            Marshal.FreeHGlobal(ptr);
            return num;
        }

        public static uint GetNumAchievements()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUserStats_GetNumAchievements();
        }

        public static SteamAPICall_t GetNumberOfCurrentPlayers()
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamUserStats_GetNumberOfCurrentPlayers();
        }

        public static bool GetStat(string pchName, out int pData)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchName))
            {
                return NativeMethods.ISteamUserStats_GetStat(handle, out pData);
            }
        }

        public static bool GetStat(string pchName, out float pData)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchName))
            {
                return NativeMethods.ISteamUserStats_GetStat_(handle, out pData);
            }
        }

        public static bool GetUserAchievement(CSteamID steamIDUser, string pchName, out bool pbAchieved)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchName))
            {
                return NativeMethods.ISteamUserStats_GetUserAchievement(steamIDUser, handle, out pbAchieved);
            }
        }

        public static bool GetUserAchievementAndUnlockTime(CSteamID steamIDUser, string pchName, out bool pbAchieved, out uint punUnlockTime)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchName))
            {
                return NativeMethods.ISteamUserStats_GetUserAchievementAndUnlockTime(steamIDUser, handle, out pbAchieved, out punUnlockTime);
            }
        }

        public static bool GetUserStat(CSteamID steamIDUser, string pchName, out int pData)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchName))
            {
                return NativeMethods.ISteamUserStats_GetUserStat(steamIDUser, handle, out pData);
            }
        }

        public static bool GetUserStat(CSteamID steamIDUser, string pchName, out float pData)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchName))
            {
                return NativeMethods.ISteamUserStats_GetUserStat_(steamIDUser, handle, out pData);
            }
        }

        public static bool IndicateAchievementProgress(string pchName, uint nCurProgress, uint nMaxProgress)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchName))
            {
                return NativeMethods.ISteamUserStats_IndicateAchievementProgress(handle, nCurProgress, nMaxProgress);
            }
        }

        public static bool RequestCurrentStats()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUserStats_RequestCurrentStats();
        }

        public static SteamAPICall_t RequestGlobalAchievementPercentages()
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamUserStats_RequestGlobalAchievementPercentages();
        }

        public static SteamAPICall_t RequestGlobalStats(int nHistoryDays)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamUserStats_RequestGlobalStats(nHistoryDays);
        }

        public static SteamAPICall_t RequestUserStats(CSteamID steamIDUser)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamUserStats_RequestUserStats(steamIDUser);
        }

        public static bool ResetAllStats(bool bAchievementsToo)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUserStats_ResetAllStats(bAchievementsToo);
        }

        public static bool SetAchievement(string pchName)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchName))
            {
                return NativeMethods.ISteamUserStats_SetAchievement(handle);
            }
        }

        public static bool SetStat(string pchName, int nData)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchName))
            {
                return NativeMethods.ISteamUserStats_SetStat(handle, nData);
            }
        }

        public static bool SetStat(string pchName, float fData)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchName))
            {
                return NativeMethods.ISteamUserStats_SetStat_(handle, fData);
            }
        }

        public static bool StoreStats()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUserStats_StoreStats();
        }

        public static bool UpdateAvgRateStat(string pchName, float flCountThisSession, double dSessionLength)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchName))
            {
                return NativeMethods.ISteamUserStats_UpdateAvgRateStat(handle, flCountThisSession, dSessionLength);
            }
        }

        public static SteamAPICall_t UploadLeaderboardScore(SteamLeaderboard_t hSteamLeaderboard, ELeaderboardUploadScoreMethod eLeaderboardUploadScoreMethod, int nScore, int[] pScoreDetails, int cScoreDetailsCount)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamUserStats_UploadLeaderboardScore(hSteamLeaderboard, eLeaderboardUploadScoreMethod, nScore, pScoreDetails, cScoreDetailsCount);
        }
    }
}

