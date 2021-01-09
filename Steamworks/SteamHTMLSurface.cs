﻿namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    public static class SteamHTMLSurface
    {
        public static void AddHeader(HHTMLBrowser unBrowserHandle, string pchKey, string pchValue)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchKey))
            {
                using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pchValue))
                {
                    NativeMethods.ISteamHTMLSurface_AddHeader(unBrowserHandle, handle, handle2);
                }
            }
        }

        public static void AllowStartRequest(HHTMLBrowser unBrowserHandle, bool bAllowed)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamHTMLSurface_AllowStartRequest(unBrowserHandle, bAllowed);
        }

        public static void CopyToClipboard(HHTMLBrowser unBrowserHandle)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamHTMLSurface_CopyToClipboard(unBrowserHandle);
        }

        public static SteamAPICall_t CreateBrowser(string pchUserAgent, string pchUserCSS)
        {
            SteamAPICall_t _t;
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchUserAgent))
            {
                using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pchUserCSS))
                {
                    _t = (SteamAPICall_t) NativeMethods.ISteamHTMLSurface_CreateBrowser(handle, handle2);
                }
            }
            return _t;
        }

        public static void ExecuteJavascript(HHTMLBrowser unBrowserHandle, string pchScript)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchScript))
            {
                NativeMethods.ISteamHTMLSurface_ExecuteJavascript(unBrowserHandle, handle);
            }
        }

        public static void FileLoadDialogResponse(HHTMLBrowser unBrowserHandle, IntPtr pchSelectedFiles)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamHTMLSurface_FileLoadDialogResponse(unBrowserHandle, pchSelectedFiles);
        }

        public static void Find(HHTMLBrowser unBrowserHandle, string pchSearchStr, bool bCurrentlyInFind, bool bReverse)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchSearchStr))
            {
                NativeMethods.ISteamHTMLSurface_Find(unBrowserHandle, handle, bCurrentlyInFind, bReverse);
            }
        }

        public static void GetLinkAtPosition(HHTMLBrowser unBrowserHandle, int x, int y)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamHTMLSurface_GetLinkAtPosition(unBrowserHandle, x, y);
        }

        public static void GoBack(HHTMLBrowser unBrowserHandle)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamHTMLSurface_GoBack(unBrowserHandle);
        }

        public static void GoForward(HHTMLBrowser unBrowserHandle)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamHTMLSurface_GoForward(unBrowserHandle);
        }

        public static bool Init()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamHTMLSurface_Init();
        }

        public static void JSDialogResponse(HHTMLBrowser unBrowserHandle, bool bResult)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamHTMLSurface_JSDialogResponse(unBrowserHandle, bResult);
        }

        public static void KeyChar(HHTMLBrowser unBrowserHandle, uint cUnicodeChar, EHTMLKeyModifiers eHTMLKeyModifiers)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamHTMLSurface_KeyChar(unBrowserHandle, cUnicodeChar, eHTMLKeyModifiers);
        }

        public static void KeyDown(HHTMLBrowser unBrowserHandle, uint nNativeKeyCode, EHTMLKeyModifiers eHTMLKeyModifiers)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamHTMLSurface_KeyDown(unBrowserHandle, nNativeKeyCode, eHTMLKeyModifiers);
        }

        public static void KeyUp(HHTMLBrowser unBrowserHandle, uint nNativeKeyCode, EHTMLKeyModifiers eHTMLKeyModifiers)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamHTMLSurface_KeyUp(unBrowserHandle, nNativeKeyCode, eHTMLKeyModifiers);
        }

        public static void LoadURL(HHTMLBrowser unBrowserHandle, string pchURL, string pchPostData)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchURL))
            {
                using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pchPostData))
                {
                    NativeMethods.ISteamHTMLSurface_LoadURL(unBrowserHandle, handle, handle2);
                }
            }
        }

        public static void MouseDoubleClick(HHTMLBrowser unBrowserHandle, EHTMLMouseButton eMouseButton)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamHTMLSurface_MouseDoubleClick(unBrowserHandle, eMouseButton);
        }

        public static void MouseDown(HHTMLBrowser unBrowserHandle, EHTMLMouseButton eMouseButton)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamHTMLSurface_MouseDown(unBrowserHandle, eMouseButton);
        }

        public static void MouseMove(HHTMLBrowser unBrowserHandle, int x, int y)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamHTMLSurface_MouseMove(unBrowserHandle, x, y);
        }

        public static void MouseUp(HHTMLBrowser unBrowserHandle, EHTMLMouseButton eMouseButton)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamHTMLSurface_MouseUp(unBrowserHandle, eMouseButton);
        }

        public static void MouseWheel(HHTMLBrowser unBrowserHandle, int nDelta)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamHTMLSurface_MouseWheel(unBrowserHandle, nDelta);
        }

        public static void PasteFromClipboard(HHTMLBrowser unBrowserHandle)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamHTMLSurface_PasteFromClipboard(unBrowserHandle);
        }

        public static void Reload(HHTMLBrowser unBrowserHandle)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamHTMLSurface_Reload(unBrowserHandle);
        }

        public static void RemoveBrowser(HHTMLBrowser unBrowserHandle)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamHTMLSurface_RemoveBrowser(unBrowserHandle);
        }

        public static void SetBackgroundMode(HHTMLBrowser unBrowserHandle, bool bBackgroundMode)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamHTMLSurface_SetBackgroundMode(unBrowserHandle, bBackgroundMode);
        }

        public static void SetCookie(string pchHostname, string pchKey, string pchValue, string pchPath = "/", uint nExpires = 0, bool bSecure = false, bool bHTTPOnly = false)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchHostname))
            {
                using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pchKey))
                {
                    using (InteropHelp.UTF8StringHandle handle3 = new InteropHelp.UTF8StringHandle(pchValue))
                    {
                        using (InteropHelp.UTF8StringHandle handle4 = new InteropHelp.UTF8StringHandle(pchPath))
                        {
                            NativeMethods.ISteamHTMLSurface_SetCookie(handle, handle2, handle3, handle4, nExpires, bSecure, bHTTPOnly);
                        }
                    }
                }
            }
        }

        public static void SetHorizontalScroll(HHTMLBrowser unBrowserHandle, uint nAbsolutePixelScroll)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamHTMLSurface_SetHorizontalScroll(unBrowserHandle, nAbsolutePixelScroll);
        }

        public static void SetKeyFocus(HHTMLBrowser unBrowserHandle, bool bHasKeyFocus)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamHTMLSurface_SetKeyFocus(unBrowserHandle, bHasKeyFocus);
        }

        public static void SetPageScaleFactor(HHTMLBrowser unBrowserHandle, float flZoom, int nPointX, int nPointY)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamHTMLSurface_SetPageScaleFactor(unBrowserHandle, flZoom, nPointX, nPointY);
        }

        public static void SetSize(HHTMLBrowser unBrowserHandle, uint unWidth, uint unHeight)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamHTMLSurface_SetSize(unBrowserHandle, unWidth, unHeight);
        }

        public static void SetVerticalScroll(HHTMLBrowser unBrowserHandle, uint nAbsolutePixelScroll)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamHTMLSurface_SetVerticalScroll(unBrowserHandle, nAbsolutePixelScroll);
        }

        public static bool Shutdown()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamHTMLSurface_Shutdown();
        }

        public static void StopFind(HHTMLBrowser unBrowserHandle)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamHTMLSurface_StopFind(unBrowserHandle);
        }

        public static void StopLoad(HHTMLBrowser unBrowserHandle)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamHTMLSurface_StopLoad(unBrowserHandle);
        }

        public static void ViewSource(HHTMLBrowser unBrowserHandle)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamHTMLSurface_ViewSource(unBrowserHandle);
        }
    }
}

