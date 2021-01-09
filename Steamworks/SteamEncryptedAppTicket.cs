namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    public static class SteamEncryptedAppTicket
    {
        public static bool BDecryptTicket(byte[] rgubTicketEncrypted, uint cubTicketEncrypted, byte[] rgubTicketDecrypted, ref uint pcubTicketDecrypted, byte[] rgubKey, int cubKey)
        {
            InteropHelp.TestIfPlatformSupported();
            return NativeMethods.BDecryptTicket(rgubTicketEncrypted, cubTicketEncrypted, rgubTicketDecrypted, ref pcubTicketDecrypted, rgubKey, cubKey);
        }

        public static bool BIsTicketForApp(byte[] rgubTicketDecrypted, uint cubTicketDecrypted, AppId_t nAppID)
        {
            InteropHelp.TestIfPlatformSupported();
            return NativeMethods.BIsTicketForApp(rgubTicketDecrypted, cubTicketDecrypted, nAppID);
        }

        public static bool BUserIsVacBanned(byte[] rgubTicketDecrypted, uint cubTicketDecrypted)
        {
            InteropHelp.TestIfPlatformSupported();
            return NativeMethods.BUserIsVacBanned(rgubTicketDecrypted, cubTicketDecrypted);
        }

        public static bool BUserOwnsAppInTicket(byte[] rgubTicketDecrypted, uint cubTicketDecrypted, AppId_t nAppID)
        {
            InteropHelp.TestIfPlatformSupported();
            return NativeMethods.BUserOwnsAppInTicket(rgubTicketDecrypted, cubTicketDecrypted, nAppID);
        }

        public static uint GetTicketAppID(byte[] rgubTicketDecrypted, uint cubTicketDecrypted)
        {
            InteropHelp.TestIfPlatformSupported();
            return NativeMethods.GetTicketAppID(rgubTicketDecrypted, cubTicketDecrypted);
        }

        public static uint GetTicketIssueTime(byte[] rgubTicketDecrypted, uint cubTicketDecrypted)
        {
            InteropHelp.TestIfPlatformSupported();
            return NativeMethods.GetTicketIssueTime(rgubTicketDecrypted, cubTicketDecrypted);
        }

        public static void GetTicketSteamID(byte[] rgubTicketDecrypted, uint cubTicketDecrypted, out CSteamID psteamID)
        {
            InteropHelp.TestIfPlatformSupported();
            NativeMethods.GetTicketSteamID(rgubTicketDecrypted, cubTicketDecrypted, out psteamID);
        }

        public static byte[] GetUserVariableData(byte[] rgubTicketDecrypted, uint cubTicketDecrypted, out uint pcubUserData)
        {
            InteropHelp.TestIfPlatformSupported();
            byte[] destination = new byte[pcubUserData];
            Marshal.Copy(NativeMethods.GetUserVariableData(rgubTicketDecrypted, cubTicketDecrypted, out pcubUserData), destination, 0, (int) pcubUserData);
            return destination;
        }
    }
}

