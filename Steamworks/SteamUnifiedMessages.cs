namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    public static class SteamUnifiedMessages
    {
        public static bool GetMethodResponseData(ClientUnifiedMessageHandle hHandle, byte[] pResponseBuffer, uint unResponseBufferSize, bool bAutoRelease)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUnifiedMessages_GetMethodResponseData(hHandle, pResponseBuffer, unResponseBufferSize, bAutoRelease);
        }

        public static bool GetMethodResponseInfo(ClientUnifiedMessageHandle hHandle, out uint punResponseSize, out EResult peResult)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUnifiedMessages_GetMethodResponseInfo(hHandle, out punResponseSize, out peResult);
        }

        public static bool ReleaseMethod(ClientUnifiedMessageHandle hHandle)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUnifiedMessages_ReleaseMethod(hHandle);
        }

        public static ClientUnifiedMessageHandle SendMethod(string pchServiceMethod, byte[] pRequestBuffer, uint unRequestBufferSize, ulong unContext)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchServiceMethod))
            {
                return (ClientUnifiedMessageHandle) NativeMethods.ISteamUnifiedMessages_SendMethod(handle, pRequestBuffer, unRequestBufferSize, unContext);
            }
        }

        public static bool SendNotification(string pchServiceNotification, byte[] pNotificationBuffer, uint unNotificationBufferSize)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchServiceNotification))
            {
                return NativeMethods.ISteamUnifiedMessages_SendNotification(handle, pNotificationBuffer, unNotificationBufferSize);
            }
        }
    }
}

