namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    public static class SteamUtils
    {
        public static bool BOverlayNeedsPresent()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUtils_BOverlayNeedsPresent();
        }

        public static SteamAPICall_t CheckFileSignature(string szFileName)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(szFileName))
            {
                return (SteamAPICall_t) NativeMethods.ISteamUtils_CheckFileSignature(handle);
            }
        }

        public static ESteamAPICallFailure GetAPICallFailureReason(SteamAPICall_t hSteamAPICall)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUtils_GetAPICallFailureReason(hSteamAPICall);
        }

        public static bool GetAPICallResult(SteamAPICall_t hSteamAPICall, IntPtr pCallback, int cubCallback, int iCallbackExpected, out bool pbFailed)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUtils_GetAPICallResult(hSteamAPICall, pCallback, cubCallback, iCallbackExpected, out pbFailed);
        }

        public static AppId_t GetAppID()
        {
            InteropHelp.TestIfAvailableClient();
            return (AppId_t) NativeMethods.ISteamUtils_GetAppID();
        }

        public static EUniverse GetConnectedUniverse()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUtils_GetConnectedUniverse();
        }

        public static bool GetCSERIPPort(out uint unIP, out ushort usPort)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUtils_GetCSERIPPort(out unIP, out usPort);
        }

        public static byte GetCurrentBatteryPower()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUtils_GetCurrentBatteryPower();
        }

        public static bool GetEnteredGamepadTextInput(out string pchText, uint cchText)
        {
            InteropHelp.TestIfAvailableClient();
            IntPtr ptr = Marshal.AllocHGlobal((int) cchText);
            bool flag = NativeMethods.ISteamUtils_GetEnteredGamepadTextInput(ptr, cchText);
            pchText = !flag ? null : InteropHelp.PtrToStringUTF8(ptr);
            Marshal.FreeHGlobal(ptr);
            return flag;
        }

        public static uint GetEnteredGamepadTextLength()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUtils_GetEnteredGamepadTextLength();
        }

        public static bool GetImageRGBA(int iImage, byte[] pubDest, int nDestBufferSize)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUtils_GetImageRGBA(iImage, pubDest, nDestBufferSize);
        }

        public static bool GetImageSize(int iImage, out uint pnWidth, out uint pnHeight)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUtils_GetImageSize(iImage, out pnWidth, out pnHeight);
        }

        public static uint GetIPCCallCount()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUtils_GetIPCCallCount();
        }

        public static string GetIPCountry()
        {
            InteropHelp.TestIfAvailableClient();
            return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamUtils_GetIPCountry());
        }

        public static uint GetSecondsSinceAppActive()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUtils_GetSecondsSinceAppActive();
        }

        public static uint GetSecondsSinceComputerActive()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUtils_GetSecondsSinceComputerActive();
        }

        public static uint GetServerRealTime()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUtils_GetServerRealTime();
        }

        public static string GetSteamUILanguage()
        {
            InteropHelp.TestIfAvailableClient();
            return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamUtils_GetSteamUILanguage());
        }

        public static bool IsAPICallCompleted(SteamAPICall_t hSteamAPICall, out bool pbFailed)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUtils_IsAPICallCompleted(hSteamAPICall, out pbFailed);
        }

        public static bool IsOverlayEnabled()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUtils_IsOverlayEnabled();
        }

        public static bool IsSteamInBigPictureMode()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUtils_IsSteamInBigPictureMode();
        }

        public static bool IsSteamRunningInVR()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUtils_IsSteamRunningInVR();
        }

        public static void SetOverlayNotificationInset(int nHorizontalInset, int nVerticalInset)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamUtils_SetOverlayNotificationInset(nHorizontalInset, nVerticalInset);
        }

        public static void SetOverlayNotificationPosition(ENotificationPosition eNotificationPosition)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamUtils_SetOverlayNotificationPosition(eNotificationPosition);
        }

        public static void SetWarningMessageHook(SteamAPIWarningMessageHook_t pFunction)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamUtils_SetWarningMessageHook(pFunction);
        }

        public static bool ShowGamepadTextInput(EGamepadTextInputMode eInputMode, EGamepadTextInputLineMode eLineInputMode, string pchDescription, uint unCharMax, string pchExistingText)
        {
            bool flag;
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchDescription))
            {
                using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pchExistingText))
                {
                    flag = NativeMethods.ISteamUtils_ShowGamepadTextInput(eInputMode, eLineInputMode, handle, unCharMax, handle2);
                }
            }
            return flag;
        }

        public static void StartVRDashboard()
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamUtils_StartVRDashboard();
        }
    }
}

