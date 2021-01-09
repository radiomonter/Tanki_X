namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    public static class SteamMusicRemote
    {
        public static bool BActivationSuccess(bool bValue)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMusicRemote_BActivationSuccess(bValue);
        }

        public static bool BIsCurrentMusicRemote()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMusicRemote_BIsCurrentMusicRemote();
        }

        public static bool CurrentEntryDidChange()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMusicRemote_CurrentEntryDidChange();
        }

        public static bool CurrentEntryIsAvailable(bool bAvailable)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMusicRemote_CurrentEntryIsAvailable(bAvailable);
        }

        public static bool CurrentEntryWillChange()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMusicRemote_CurrentEntryWillChange();
        }

        public static bool DeregisterSteamMusicRemote()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMusicRemote_DeregisterSteamMusicRemote();
        }

        public static bool EnableLooped(bool bValue)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMusicRemote_EnableLooped(bValue);
        }

        public static bool EnablePlaylists(bool bValue)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMusicRemote_EnablePlaylists(bValue);
        }

        public static bool EnablePlayNext(bool bValue)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMusicRemote_EnablePlayNext(bValue);
        }

        public static bool EnablePlayPrevious(bool bValue)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMusicRemote_EnablePlayPrevious(bValue);
        }

        public static bool EnableQueue(bool bValue)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMusicRemote_EnableQueue(bValue);
        }

        public static bool EnableShuffled(bool bValue)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMusicRemote_EnableShuffled(bValue);
        }

        public static bool PlaylistDidChange()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMusicRemote_PlaylistDidChange();
        }

        public static bool PlaylistWillChange()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMusicRemote_PlaylistWillChange();
        }

        public static bool QueueDidChange()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMusicRemote_QueueDidChange();
        }

        public static bool QueueWillChange()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMusicRemote_QueueWillChange();
        }

        public static bool RegisterSteamMusicRemote(string pchName)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchName))
            {
                return NativeMethods.ISteamMusicRemote_RegisterSteamMusicRemote(handle);
            }
        }

        public static bool ResetPlaylistEntries()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMusicRemote_ResetPlaylistEntries();
        }

        public static bool ResetQueueEntries()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMusicRemote_ResetQueueEntries();
        }

        public static bool SetCurrentPlaylistEntry(int nID)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMusicRemote_SetCurrentPlaylistEntry(nID);
        }

        public static bool SetCurrentQueueEntry(int nID)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMusicRemote_SetCurrentQueueEntry(nID);
        }

        public static bool SetDisplayName(string pchDisplayName)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchDisplayName))
            {
                return NativeMethods.ISteamMusicRemote_SetDisplayName(handle);
            }
        }

        public static bool SetPlaylistEntry(int nID, int nPosition, string pchEntryText)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchEntryText))
            {
                return NativeMethods.ISteamMusicRemote_SetPlaylistEntry(nID, nPosition, handle);
            }
        }

        public static bool SetPNGIcon_64x64(byte[] pvBuffer, uint cbBufferLength)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMusicRemote_SetPNGIcon_64x64(pvBuffer, cbBufferLength);
        }

        public static bool SetQueueEntry(int nID, int nPosition, string pchEntryText)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchEntryText))
            {
                return NativeMethods.ISteamMusicRemote_SetQueueEntry(nID, nPosition, handle);
            }
        }

        public static bool UpdateCurrentEntryCoverArt(byte[] pvBuffer, uint cbBufferLength)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMusicRemote_UpdateCurrentEntryCoverArt(pvBuffer, cbBufferLength);
        }

        public static bool UpdateCurrentEntryElapsedSeconds(int nValue)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMusicRemote_UpdateCurrentEntryElapsedSeconds(nValue);
        }

        public static bool UpdateCurrentEntryText(string pchText)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchText))
            {
                return NativeMethods.ISteamMusicRemote_UpdateCurrentEntryText(handle);
            }
        }

        public static bool UpdateLooped(bool bValue)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMusicRemote_UpdateLooped(bValue);
        }

        public static bool UpdatePlaybackStatus(AudioPlayback_Status nStatus)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMusicRemote_UpdatePlaybackStatus(nStatus);
        }

        public static bool UpdateShuffled(bool bValue)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMusicRemote_UpdateShuffled(bValue);
        }

        public static bool UpdateVolume(float flValue)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMusicRemote_UpdateVolume(flValue);
        }
    }
}

