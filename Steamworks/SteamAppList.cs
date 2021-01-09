namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    public static class SteamAppList
    {
        public static int GetAppBuildId(AppId_t nAppID)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamAppList_GetAppBuildId(nAppID);
        }

        public static int GetAppInstallDir(AppId_t nAppID, out string pchDirectory, int cchNameMax)
        {
            InteropHelp.TestIfAvailableClient();
            IntPtr ptr = Marshal.AllocHGlobal(cchNameMax);
            int num = NativeMethods.ISteamAppList_GetAppInstallDir(nAppID, ptr, cchNameMax);
            pchDirectory = (num == -1) ? null : InteropHelp.PtrToStringUTF8(ptr);
            Marshal.FreeHGlobal(ptr);
            return num;
        }

        public static int GetAppName(AppId_t nAppID, out string pchName, int cchNameMax)
        {
            InteropHelp.TestIfAvailableClient();
            IntPtr ptr = Marshal.AllocHGlobal(cchNameMax);
            int num = NativeMethods.ISteamAppList_GetAppName(nAppID, ptr, cchNameMax);
            pchName = (num == -1) ? null : InteropHelp.PtrToStringUTF8(ptr);
            Marshal.FreeHGlobal(ptr);
            return num;
        }

        public static uint GetInstalledApps(AppId_t[] pvecAppID, uint unMaxAppIDs)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamAppList_GetInstalledApps(pvecAppID, unMaxAppIDs);
        }

        public static uint GetNumInstalledApps()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamAppList_GetNumInstalledApps();
        }
    }
}

