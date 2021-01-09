﻿namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    public static class SteamMatchmaking
    {
        public static int AddFavoriteGame(AppId_t nAppID, uint nIP, ushort nConnPort, ushort nQueryPort, uint unFlags, uint rTime32LastPlayedOnServer)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMatchmaking_AddFavoriteGame(nAppID, nIP, nConnPort, nQueryPort, unFlags, rTime32LastPlayedOnServer);
        }

        public static void AddRequestLobbyListCompatibleMembersFilter(CSteamID steamIDLobby)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamMatchmaking_AddRequestLobbyListCompatibleMembersFilter(steamIDLobby);
        }

        public static void AddRequestLobbyListDistanceFilter(ELobbyDistanceFilter eLobbyDistanceFilter)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamMatchmaking_AddRequestLobbyListDistanceFilter(eLobbyDistanceFilter);
        }

        public static void AddRequestLobbyListFilterSlotsAvailable(int nSlotsAvailable)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamMatchmaking_AddRequestLobbyListFilterSlotsAvailable(nSlotsAvailable);
        }

        public static void AddRequestLobbyListNearValueFilter(string pchKeyToMatch, int nValueToBeCloseTo)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchKeyToMatch))
            {
                NativeMethods.ISteamMatchmaking_AddRequestLobbyListNearValueFilter(handle, nValueToBeCloseTo);
            }
        }

        public static void AddRequestLobbyListNumericalFilter(string pchKeyToMatch, int nValueToMatch, ELobbyComparison eComparisonType)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchKeyToMatch))
            {
                NativeMethods.ISteamMatchmaking_AddRequestLobbyListNumericalFilter(handle, nValueToMatch, eComparisonType);
            }
        }

        public static void AddRequestLobbyListResultCountFilter(int cMaxResults)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamMatchmaking_AddRequestLobbyListResultCountFilter(cMaxResults);
        }

        public static void AddRequestLobbyListStringFilter(string pchKeyToMatch, string pchValueToMatch, ELobbyComparison eComparisonType)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchKeyToMatch))
            {
                using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pchValueToMatch))
                {
                    NativeMethods.ISteamMatchmaking_AddRequestLobbyListStringFilter(handle, handle2, eComparisonType);
                }
            }
        }

        public static SteamAPICall_t CreateLobby(ELobbyType eLobbyType, int cMaxMembers)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamMatchmaking_CreateLobby(eLobbyType, cMaxMembers);
        }

        public static bool DeleteLobbyData(CSteamID steamIDLobby, string pchKey)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchKey))
            {
                return NativeMethods.ISteamMatchmaking_DeleteLobbyData(steamIDLobby, handle);
            }
        }

        public static bool GetFavoriteGame(int iGame, out AppId_t pnAppID, out uint pnIP, out ushort pnConnPort, out ushort pnQueryPort, out uint punFlags, out uint pRTime32LastPlayedOnServer)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMatchmaking_GetFavoriteGame(iGame, out pnAppID, out pnIP, out pnConnPort, out pnQueryPort, out punFlags, out pRTime32LastPlayedOnServer);
        }

        public static int GetFavoriteGameCount()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMatchmaking_GetFavoriteGameCount();
        }

        public static CSteamID GetLobbyByIndex(int iLobby)
        {
            InteropHelp.TestIfAvailableClient();
            return (CSteamID) NativeMethods.ISteamMatchmaking_GetLobbyByIndex(iLobby);
        }

        public static int GetLobbyChatEntry(CSteamID steamIDLobby, int iChatID, out CSteamID pSteamIDUser, byte[] pvData, int cubData, out EChatEntryType peChatEntryType)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMatchmaking_GetLobbyChatEntry(steamIDLobby, iChatID, out pSteamIDUser, pvData, cubData, out peChatEntryType);
        }

        public static string GetLobbyData(CSteamID steamIDLobby, string pchKey)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchKey))
            {
                return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamMatchmaking_GetLobbyData(steamIDLobby, handle));
            }
        }

        public static bool GetLobbyDataByIndex(CSteamID steamIDLobby, int iLobbyData, out string pchKey, int cchKeyBufferSize, out string pchValue, int cchValueBufferSize)
        {
            InteropHelp.TestIfAvailableClient();
            IntPtr ptr = Marshal.AllocHGlobal(cchKeyBufferSize);
            IntPtr ptr2 = Marshal.AllocHGlobal(cchValueBufferSize);
            bool flag = NativeMethods.ISteamMatchmaking_GetLobbyDataByIndex(steamIDLobby, iLobbyData, ptr, cchKeyBufferSize, ptr2, cchValueBufferSize);
            pchKey = !flag ? null : InteropHelp.PtrToStringUTF8(ptr);
            Marshal.FreeHGlobal(ptr);
            pchValue = !flag ? null : InteropHelp.PtrToStringUTF8(ptr2);
            Marshal.FreeHGlobal(ptr2);
            return flag;
        }

        public static int GetLobbyDataCount(CSteamID steamIDLobby)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMatchmaking_GetLobbyDataCount(steamIDLobby);
        }

        public static bool GetLobbyGameServer(CSteamID steamIDLobby, out uint punGameServerIP, out ushort punGameServerPort, out CSteamID psteamIDGameServer)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMatchmaking_GetLobbyGameServer(steamIDLobby, out punGameServerIP, out punGameServerPort, out psteamIDGameServer);
        }

        public static CSteamID GetLobbyMemberByIndex(CSteamID steamIDLobby, int iMember)
        {
            InteropHelp.TestIfAvailableClient();
            return (CSteamID) NativeMethods.ISteamMatchmaking_GetLobbyMemberByIndex(steamIDLobby, iMember);
        }

        public static string GetLobbyMemberData(CSteamID steamIDLobby, CSteamID steamIDUser, string pchKey)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchKey))
            {
                return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamMatchmaking_GetLobbyMemberData(steamIDLobby, steamIDUser, handle));
            }
        }

        public static int GetLobbyMemberLimit(CSteamID steamIDLobby)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMatchmaking_GetLobbyMemberLimit(steamIDLobby);
        }

        public static CSteamID GetLobbyOwner(CSteamID steamIDLobby)
        {
            InteropHelp.TestIfAvailableClient();
            return (CSteamID) NativeMethods.ISteamMatchmaking_GetLobbyOwner(steamIDLobby);
        }

        public static int GetNumLobbyMembers(CSteamID steamIDLobby)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMatchmaking_GetNumLobbyMembers(steamIDLobby);
        }

        public static bool InviteUserToLobby(CSteamID steamIDLobby, CSteamID steamIDInvitee)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMatchmaking_InviteUserToLobby(steamIDLobby, steamIDInvitee);
        }

        public static SteamAPICall_t JoinLobby(CSteamID steamIDLobby)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamMatchmaking_JoinLobby(steamIDLobby);
        }

        public static void LeaveLobby(CSteamID steamIDLobby)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamMatchmaking_LeaveLobby(steamIDLobby);
        }

        public static bool RemoveFavoriteGame(AppId_t nAppID, uint nIP, ushort nConnPort, ushort nQueryPort, uint unFlags)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMatchmaking_RemoveFavoriteGame(nAppID, nIP, nConnPort, nQueryPort, unFlags);
        }

        public static bool RequestLobbyData(CSteamID steamIDLobby)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMatchmaking_RequestLobbyData(steamIDLobby);
        }

        public static SteamAPICall_t RequestLobbyList()
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamMatchmaking_RequestLobbyList();
        }

        public static bool SendLobbyChatMsg(CSteamID steamIDLobby, byte[] pvMsgBody, int cubMsgBody)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMatchmaking_SendLobbyChatMsg(steamIDLobby, pvMsgBody, cubMsgBody);
        }

        public static bool SetLinkedLobby(CSteamID steamIDLobby, CSteamID steamIDLobbyDependent)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMatchmaking_SetLinkedLobby(steamIDLobby, steamIDLobbyDependent);
        }

        public static bool SetLobbyData(CSteamID steamIDLobby, string pchKey, string pchValue)
        {
            bool flag;
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchKey))
            {
                using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pchValue))
                {
                    flag = NativeMethods.ISteamMatchmaking_SetLobbyData(steamIDLobby, handle, handle2);
                }
            }
            return flag;
        }

        public static void SetLobbyGameServer(CSteamID steamIDLobby, uint unGameServerIP, ushort unGameServerPort, CSteamID steamIDGameServer)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamMatchmaking_SetLobbyGameServer(steamIDLobby, unGameServerIP, unGameServerPort, steamIDGameServer);
        }

        public static bool SetLobbyJoinable(CSteamID steamIDLobby, bool bLobbyJoinable)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMatchmaking_SetLobbyJoinable(steamIDLobby, bLobbyJoinable);
        }

        public static void SetLobbyMemberData(CSteamID steamIDLobby, string pchKey, string pchValue)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchKey))
            {
                using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pchValue))
                {
                    NativeMethods.ISteamMatchmaking_SetLobbyMemberData(steamIDLobby, handle, handle2);
                }
            }
        }

        public static bool SetLobbyMemberLimit(CSteamID steamIDLobby, int cMaxMembers)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMatchmaking_SetLobbyMemberLimit(steamIDLobby, cMaxMembers);
        }

        public static bool SetLobbyOwner(CSteamID steamIDLobby, CSteamID steamIDNewOwner)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMatchmaking_SetLobbyOwner(steamIDLobby, steamIDNewOwner);
        }

        public static bool SetLobbyType(CSteamID steamIDLobby, ELobbyType eLobbyType)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamMatchmaking_SetLobbyType(steamIDLobby, eLobbyType);
        }
    }
}

