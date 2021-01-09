namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    public static class SteamApps
    {
        public static bool BGetDLCDataByIndex(int iDLC, out AppId_t pAppID, out bool pbAvailable, out string pchName, int cchNameBufferSize)
        {
            InteropHelp.TestIfAvailableClient();
            IntPtr ptr = Marshal.AllocHGlobal(cchNameBufferSize);
            bool flag = NativeMethods.ISteamApps_BGetDLCDataByIndex(iDLC, out pAppID, out pbAvailable, ptr, cchNameBufferSize);
            pchName = !flag ? null : InteropHelp.PtrToStringUTF8(ptr);
            Marshal.FreeHGlobal(ptr);
            return flag;
        }

        public static bool BIsAppInstalled(AppId_t appID)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamApps_BIsAppInstalled(appID);
        }

        public static bool BIsCybercafe()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamApps_BIsCybercafe();
        }

        public static bool BIsDlcInstalled(AppId_t appID)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamApps_BIsDlcInstalled(appID);
        }

        public static bool BIsLowViolence()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamApps_BIsLowViolence();
        }

        public static bool BIsSubscribed()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamApps_BIsSubscribed();
        }

        public static bool BIsSubscribedApp(AppId_t appID)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamApps_BIsSubscribedApp(appID);
        }

        public static bool BIsSubscribedFromFreeWeekend()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamApps_BIsSubscribedFromFreeWeekend();
        }

        public static bool BIsVACBanned()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamApps_BIsVACBanned();
        }

        public static int GetAppBuildId()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamApps_GetAppBuildId();
        }

        public static uint GetAppInstallDir(AppId_t appID, out string pchFolder, uint cchFolderBufferSize)
        {
            InteropHelp.TestIfAvailableClient();
            IntPtr ptr = Marshal.AllocHGlobal((int) cchFolderBufferSize);
            uint num = NativeMethods.ISteamApps_GetAppInstallDir(appID, ptr, cchFolderBufferSize);
            pchFolder = (num == 0) ? null : InteropHelp.PtrToStringUTF8(ptr);
            Marshal.FreeHGlobal(ptr);
            return num;
        }

        public static CSteamID GetAppOwner()
        {
            InteropHelp.TestIfAvailableClient();
            return (CSteamID) NativeMethods.ISteamApps_GetAppOwner();
        }

        public static string GetAvailableGameLanguages()
        {
            InteropHelp.TestIfAvailableClient();
            return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamApps_GetAvailableGameLanguages());
        }

        public static bool GetCurrentBetaName(out string pchName, int cchNameBufferSize)
        {
            InteropHelp.TestIfAvailableClient();
            IntPtr ptr = Marshal.AllocHGlobal(cchNameBufferSize);
            bool flag = NativeMethods.ISteamApps_GetCurrentBetaName(ptr, cchNameBufferSize);
            pchName = !flag ? null : InteropHelp.PtrToStringUTF8(ptr);
            Marshal.FreeHGlobal(ptr);
            return flag;
        }

        public static string GetCurrentGameLanguage()
        {
            InteropHelp.TestIfAvailableClient();
            return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamApps_GetCurrentGameLanguage());
        }

        public static int GetDLCCount()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamApps_GetDLCCount();
        }

        public static bool GetDlcDownloadProgress(AppId_t nAppID, out ulong punBytesDownloaded, out ulong punBytesTotal)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamApps_GetDlcDownloadProgress(nAppID, out punBytesDownloaded, out punBytesTotal);
        }

        public static uint GetEarliestPurchaseUnixTime(AppId_t nAppID)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamApps_GetEarliestPurchaseUnixTime(nAppID);
        }

        public static SteamAPICall_t GetFileDetails(string pszFileName)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pszFileName))
            {
                return (SteamAPICall_t) NativeMethods.ISteamApps_GetFileDetails(handle);
            }
        }

        public static uint GetInstalledDepots(AppId_t appID, DepotId_t[] pvecDepots, uint cMaxDepots)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamApps_GetInstalledDepots(appID, pvecDepots, cMaxDepots);
        }

        public static string GetLaunchQueryParam(string pchKey)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchKey))
            {
                return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamApps_GetLaunchQueryParam(handle));
            }
        }

        public static void InstallDLC(AppId_t nAppID)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamApps_InstallDLC(nAppID);
        }

        public static bool MarkContentCorrupt(bool bMissingFilesOnly)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamApps_MarkContentCorrupt(bMissingFilesOnly);
        }

        public static void RequestAllProofOfPurchaseKeys()
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamApps_RequestAllProofOfPurchaseKeys();
        }

        public static void RequestAppProofOfPurchaseKey(AppId_t nAppID)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamApps_RequestAppProofOfPurchaseKey(nAppID);
        }

        public static void UninstallDLC(AppId_t nAppID)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamApps_UninstallDLC(nAppID);
        }
    }
}

