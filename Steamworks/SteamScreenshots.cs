namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    public static class SteamScreenshots
    {
        public static ScreenshotHandle AddScreenshotToLibrary(string pchFilename, string pchThumbnailFilename, int nWidth, int nHeight)
        {
            ScreenshotHandle handle3;
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchFilename))
            {
                using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pchThumbnailFilename))
                {
                    handle3 = (ScreenshotHandle) NativeMethods.ISteamScreenshots_AddScreenshotToLibrary(handle, handle2, nWidth, nHeight);
                }
            }
            return handle3;
        }

        public static ScreenshotHandle AddVRScreenshotToLibrary(EVRScreenshotType eType, string pchFilename, string pchVRFilename)
        {
            ScreenshotHandle handle3;
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchFilename))
            {
                using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pchVRFilename))
                {
                    handle3 = (ScreenshotHandle) NativeMethods.ISteamScreenshots_AddVRScreenshotToLibrary(eType, handle, handle2);
                }
            }
            return handle3;
        }

        public static void HookScreenshots(bool bHook)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamScreenshots_HookScreenshots(bHook);
        }

        public static bool IsScreenshotsHooked()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamScreenshots_IsScreenshotsHooked();
        }

        public static bool SetLocation(ScreenshotHandle hScreenshot, string pchLocation)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchLocation))
            {
                return NativeMethods.ISteamScreenshots_SetLocation(hScreenshot, handle);
            }
        }

        public static bool TagPublishedFile(ScreenshotHandle hScreenshot, PublishedFileId_t unPublishedFileID)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamScreenshots_TagPublishedFile(hScreenshot, unPublishedFileID);
        }

        public static bool TagUser(ScreenshotHandle hScreenshot, CSteamID steamID)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamScreenshots_TagUser(hScreenshot, steamID);
        }

        public static void TriggerScreenshot()
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamScreenshots_TriggerScreenshot();
        }

        public static ScreenshotHandle WriteScreenshot(byte[] pubRGB, uint cubRGB, int nWidth, int nHeight)
        {
            InteropHelp.TestIfAvailableClient();
            return (ScreenshotHandle) NativeMethods.ISteamScreenshots_WriteScreenshot(pubRGB, cubRGB, nWidth, nHeight);
        }
    }
}

