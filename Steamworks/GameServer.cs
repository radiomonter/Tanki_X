namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    public static class GameServer
    {
        public static bool BSecure()
        {
            InteropHelp.TestIfPlatformSupported();
            return NativeMethods.SteamGameServer_BSecure();
        }

        public static HSteamPipe GetHSteamPipe()
        {
            InteropHelp.TestIfPlatformSupported();
            return (HSteamPipe) NativeMethods.SteamGameServer_GetHSteamPipe();
        }

        public static HSteamUser GetHSteamUser()
        {
            InteropHelp.TestIfPlatformSupported();
            return (HSteamUser) NativeMethods.SteamGameServer_GetHSteamUser();
        }

        public static CSteamID GetSteamID()
        {
            InteropHelp.TestIfPlatformSupported();
            return (CSteamID) NativeMethods.SteamGameServer_GetSteamID();
        }

        public static bool Init(uint unIP, ushort usSteamPort, ushort usGamePort, ushort usQueryPort, EServerMode eServerMode, string pchVersionString)
        {
            InteropHelp.TestIfPlatformSupported();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchVersionString))
            {
                return NativeMethods.SteamGameServer_Init(unIP, usSteamPort, usGamePort, usQueryPort, eServerMode, handle);
            }
        }

        public static void ReleaseCurrentThreadMemory()
        {
            InteropHelp.TestIfPlatformSupported();
            NativeMethods.SteamGameServer_ReleaseCurrentThreadMemory();
        }

        public static void RunCallbacks()
        {
            InteropHelp.TestIfPlatformSupported();
            NativeMethods.SteamGameServer_RunCallbacks();
        }

        public static void Shutdown()
        {
            InteropHelp.TestIfPlatformSupported();
            NativeMethods.SteamGameServer_Shutdown();
        }
    }
}

