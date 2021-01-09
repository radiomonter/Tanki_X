namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    public static class SteamGameServerUtils
    {
        public static bool BOverlayNeedsPresent()
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUtils_BOverlayNeedsPresent();
        }

        public static SteamAPICall_t CheckFileSignature(string szFileName)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(szFileName))
            {
                return (SteamAPICall_t) NativeMethods.ISteamGameServerUtils_CheckFileSignature(handle);
            }
        }

        public static ESteamAPICallFailure GetAPICallFailureReason(SteamAPICall_t hSteamAPICall)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUtils_GetAPICallFailureReason(hSteamAPICall);
        }

        public static bool GetAPICallResult(SteamAPICall_t hSteamAPICall, IntPtr pCallback, int cubCallback, int iCallbackExpected, out bool pbFailed)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUtils_GetAPICallResult(hSteamAPICall, pCallback, cubCallback, iCallbackExpected, out pbFailed);
        }

        public static AppId_t GetAppID()
        {
            InteropHelp.TestIfAvailableGameServer();
            return (AppId_t) NativeMethods.ISteamGameServerUtils_GetAppID();
        }

        public static EUniverse GetConnectedUniverse()
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUtils_GetConnectedUniverse();
        }

        public static bool GetCSERIPPort(out uint unIP, out ushort usPort)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUtils_GetCSERIPPort(out unIP, out usPort);
        }

        public static byte GetCurrentBatteryPower()
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUtils_GetCurrentBatteryPower();
        }

        public static bool GetEnteredGamepadTextInput(out string pchText, uint cchText)
        {
            InteropHelp.TestIfAvailableGameServer();
            IntPtr ptr = Marshal.AllocHGlobal((int) cchText);
            bool flag = NativeMethods.ISteamGameServerUtils_GetEnteredGamepadTextInput(ptr, cchText);
            pchText = !flag ? null : InteropHelp.PtrToStringUTF8(ptr);
            Marshal.FreeHGlobal(ptr);
            return flag;
        }

        public static uint GetEnteredGamepadTextLength()
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUtils_GetEnteredGamepadTextLength();
        }

        public static bool GetImageRGBA(int iImage, byte[] pubDest, int nDestBufferSize)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUtils_GetImageRGBA(iImage, pubDest, nDestBufferSize);
        }

        public static bool GetImageSize(int iImage, out uint pnWidth, out uint pnHeight)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUtils_GetImageSize(iImage, out pnWidth, out pnHeight);
        }

        public static uint GetIPCCallCount()
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUtils_GetIPCCallCount();
        }

        public static string GetIPCountry()
        {
            InteropHelp.TestIfAvailableGameServer();
            return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamGameServerUtils_GetIPCountry());
        }

        public static uint GetSecondsSinceAppActive()
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUtils_GetSecondsSinceAppActive();
        }

        public static uint GetSecondsSinceComputerActive()
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUtils_GetSecondsSinceComputerActive();
        }

        public static uint GetServerRealTime()
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUtils_GetServerRealTime();
        }

        public static string GetSteamUILanguage()
        {
            InteropHelp.TestIfAvailableGameServer();
            return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamGameServerUtils_GetSteamUILanguage());
        }

        public static bool IsAPICallCompleted(SteamAPICall_t hSteamAPICall, out bool pbFailed)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUtils_IsAPICallCompleted(hSteamAPICall, out pbFailed);
        }

        public static bool IsOverlayEnabled()
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUtils_IsOverlayEnabled();
        }

        public static bool IsSteamInBigPictureMode()
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUtils_IsSteamInBigPictureMode();
        }

        public static bool IsSteamRunningInVR()
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUtils_IsSteamRunningInVR();
        }

        public static void SetOverlayNotificationInset(int nHorizontalInset, int nVerticalInset)
        {
            InteropHelp.TestIfAvailableGameServer();
            NativeMethods.ISteamGameServerUtils_SetOverlayNotificationInset(nHorizontalInset, nVerticalInset);
        }

        public static void SetOverlayNotificationPosition(ENotificationPosition eNotificationPosition)
        {
            InteropHelp.TestIfAvailableGameServer();
            NativeMethods.ISteamGameServerUtils_SetOverlayNotificationPosition(eNotificationPosition);
        }

        public static void SetWarningMessageHook(SteamAPIWarningMessageHook_t pFunction)
        {
            InteropHelp.TestIfAvailableGameServer();
            NativeMethods.ISteamGameServerUtils_SetWarningMessageHook(pFunction);
        }

        public static bool ShowGamepadTextInput(EGamepadTextInputMode eInputMode, EGamepadTextInputLineMode eLineInputMode, string pchDescription, uint unCharMax, string pchExistingText)
        {
            bool flag;
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchDescription))
            {
                using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pchExistingText))
                {
                    flag = NativeMethods.ISteamGameServerUtils_ShowGamepadTextInput(eInputMode, eLineInputMode, handle, unCharMax, handle2);
                }
            }
            return flag;
        }

        public static void StartVRDashboard()
        {
            InteropHelp.TestIfAvailableGameServer();
            NativeMethods.ISteamGameServerUtils_StartVRDashboard();
        }
    }
}

