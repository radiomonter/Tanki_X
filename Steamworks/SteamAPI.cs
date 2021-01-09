namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    public static class SteamAPI
    {
        public static HSteamPipe GetHSteamPipe()
        {
            InteropHelp.TestIfPlatformSupported();
            return (HSteamPipe) NativeMethods.SteamAPI_GetHSteamPipe();
        }

        public static HSteamUser GetHSteamUser()
        {
            InteropHelp.TestIfPlatformSupported();
            return (HSteamUser) NativeMethods.SteamAPI_GetHSteamUser();
        }

        public static HSteamUser GetHSteamUserCurrent()
        {
            InteropHelp.TestIfPlatformSupported();
            return (HSteamUser) NativeMethods.Steam_GetHSteamUserCurrent();
        }

        public static bool Init()
        {
            InteropHelp.TestIfPlatformSupported();
            return NativeMethods.SteamAPI_Init();
        }

        public static bool InitSafe() => 
            Init();

        public static bool IsSteamRunning()
        {
            InteropHelp.TestIfPlatformSupported();
            return NativeMethods.SteamAPI_IsSteamRunning();
        }

        public static void ReleaseCurrentThreadMemory()
        {
            InteropHelp.TestIfPlatformSupported();
            NativeMethods.SteamAPI_ReleaseCurrentThreadMemory();
        }

        public static bool RestartAppIfNecessary(AppId_t unOwnAppID)
        {
            InteropHelp.TestIfPlatformSupported();
            return NativeMethods.SteamAPI_RestartAppIfNecessary(unOwnAppID);
        }

        public static void RunCallbacks()
        {
            InteropHelp.TestIfPlatformSupported();
            NativeMethods.SteamAPI_RunCallbacks();
        }

        public static void Shutdown()
        {
            InteropHelp.TestIfPlatformSupported();
            NativeMethods.SteamAPI_Shutdown();
        }
    }
}

