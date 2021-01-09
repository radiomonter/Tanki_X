namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    public static class SteamFriends
    {
        public static void ActivateGameOverlay(string pchDialog)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchDialog))
            {
                NativeMethods.ISteamFriends_ActivateGameOverlay(handle);
            }
        }

        public static void ActivateGameOverlayInviteDialog(CSteamID steamIDLobby)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamFriends_ActivateGameOverlayInviteDialog(steamIDLobby);
        }

        public static void ActivateGameOverlayToStore(AppId_t nAppID, EOverlayToStoreFlag eFlag)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamFriends_ActivateGameOverlayToStore(nAppID, eFlag);
        }

        public static void ActivateGameOverlayToUser(string pchDialog, CSteamID steamID)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchDialog))
            {
                NativeMethods.ISteamFriends_ActivateGameOverlayToUser(handle, steamID);
            }
        }

        public static void ActivateGameOverlayToWebPage(string pchURL)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchURL))
            {
                NativeMethods.ISteamFriends_ActivateGameOverlayToWebPage(handle);
            }
        }

        public static void ClearRichPresence()
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamFriends_ClearRichPresence();
        }

        public static bool CloseClanChatWindowInSteam(CSteamID steamIDClanChat)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamFriends_CloseClanChatWindowInSteam(steamIDClanChat);
        }

        public static SteamAPICall_t DownloadClanActivityCounts(CSteamID[] psteamIDClans, int cClansToRequest)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamFriends_DownloadClanActivityCounts(psteamIDClans, cClansToRequest);
        }

        public static SteamAPICall_t EnumerateFollowingList(uint unStartIndex)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamFriends_EnumerateFollowingList(unStartIndex);
        }

        public static CSteamID GetChatMemberByIndex(CSteamID steamIDClan, int iUser)
        {
            InteropHelp.TestIfAvailableClient();
            return (CSteamID) NativeMethods.ISteamFriends_GetChatMemberByIndex(steamIDClan, iUser);
        }

        public static bool GetClanActivityCounts(CSteamID steamIDClan, out int pnOnline, out int pnInGame, out int pnChatting)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamFriends_GetClanActivityCounts(steamIDClan, out pnOnline, out pnInGame, out pnChatting);
        }

        public static CSteamID GetClanByIndex(int iClan)
        {
            InteropHelp.TestIfAvailableClient();
            return (CSteamID) NativeMethods.ISteamFriends_GetClanByIndex(iClan);
        }

        public static int GetClanChatMemberCount(CSteamID steamIDClan)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamFriends_GetClanChatMemberCount(steamIDClan);
        }

        public static int GetClanChatMessage(CSteamID steamIDClanChat, int iMessage, out string prgchText, int cchTextMax, out EChatEntryType peChatEntryType, out CSteamID psteamidChatter)
        {
            InteropHelp.TestIfAvailableClient();
            IntPtr ptr = Marshal.AllocHGlobal(cchTextMax);
            int num = NativeMethods.ISteamFriends_GetClanChatMessage(steamIDClanChat, iMessage, ptr, cchTextMax, out peChatEntryType, out psteamidChatter);
            prgchText = (num == 0) ? null : InteropHelp.PtrToStringUTF8(ptr);
            Marshal.FreeHGlobal(ptr);
            return num;
        }

        public static int GetClanCount()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamFriends_GetClanCount();
        }

        public static string GetClanName(CSteamID steamIDClan)
        {
            InteropHelp.TestIfAvailableClient();
            return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetClanName(steamIDClan));
        }

        public static CSteamID GetClanOfficerByIndex(CSteamID steamIDClan, int iOfficer)
        {
            InteropHelp.TestIfAvailableClient();
            return (CSteamID) NativeMethods.ISteamFriends_GetClanOfficerByIndex(steamIDClan, iOfficer);
        }

        public static int GetClanOfficerCount(CSteamID steamIDClan)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamFriends_GetClanOfficerCount(steamIDClan);
        }

        public static CSteamID GetClanOwner(CSteamID steamIDClan)
        {
            InteropHelp.TestIfAvailableClient();
            return (CSteamID) NativeMethods.ISteamFriends_GetClanOwner(steamIDClan);
        }

        public static string GetClanTag(CSteamID steamIDClan)
        {
            InteropHelp.TestIfAvailableClient();
            return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetClanTag(steamIDClan));
        }

        public static CSteamID GetCoplayFriend(int iCoplayFriend)
        {
            InteropHelp.TestIfAvailableClient();
            return (CSteamID) NativeMethods.ISteamFriends_GetCoplayFriend(iCoplayFriend);
        }

        public static int GetCoplayFriendCount()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamFriends_GetCoplayFriendCount();
        }

        public static SteamAPICall_t GetFollowerCount(CSteamID steamID)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamFriends_GetFollowerCount(steamID);
        }

        public static CSteamID GetFriendByIndex(int iFriend, EFriendFlags iFriendFlags)
        {
            InteropHelp.TestIfAvailableClient();
            return (CSteamID) NativeMethods.ISteamFriends_GetFriendByIndex(iFriend, iFriendFlags);
        }

        public static AppId_t GetFriendCoplayGame(CSteamID steamIDFriend)
        {
            InteropHelp.TestIfAvailableClient();
            return (AppId_t) NativeMethods.ISteamFriends_GetFriendCoplayGame(steamIDFriend);
        }

        public static int GetFriendCoplayTime(CSteamID steamIDFriend)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamFriends_GetFriendCoplayTime(steamIDFriend);
        }

        public static int GetFriendCount(EFriendFlags iFriendFlags)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamFriends_GetFriendCount(iFriendFlags);
        }

        public static int GetFriendCountFromSource(CSteamID steamIDSource)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamFriends_GetFriendCountFromSource(steamIDSource);
        }

        public static CSteamID GetFriendFromSourceByIndex(CSteamID steamIDSource, int iFriend)
        {
            InteropHelp.TestIfAvailableClient();
            return (CSteamID) NativeMethods.ISteamFriends_GetFriendFromSourceByIndex(steamIDSource, iFriend);
        }

        public static bool GetFriendGamePlayed(CSteamID steamIDFriend, out FriendGameInfo_t pFriendGameInfo)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamFriends_GetFriendGamePlayed(steamIDFriend, out pFriendGameInfo);
        }

        public static int GetFriendMessage(CSteamID steamIDFriend, int iMessageID, out string pvData, int cubData, out EChatEntryType peChatEntryType)
        {
            InteropHelp.TestIfAvailableClient();
            IntPtr ptr = Marshal.AllocHGlobal(cubData);
            int num = NativeMethods.ISteamFriends_GetFriendMessage(steamIDFriend, iMessageID, ptr, cubData, out peChatEntryType);
            pvData = (num == 0) ? null : InteropHelp.PtrToStringUTF8(ptr);
            Marshal.FreeHGlobal(ptr);
            return num;
        }

        public static string GetFriendPersonaName(CSteamID steamIDFriend)
        {
            InteropHelp.TestIfAvailableClient();
            return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetFriendPersonaName(steamIDFriend));
        }

        public static string GetFriendPersonaNameHistory(CSteamID steamIDFriend, int iPersonaName)
        {
            InteropHelp.TestIfAvailableClient();
            return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetFriendPersonaNameHistory(steamIDFriend, iPersonaName));
        }

        public static EPersonaState GetFriendPersonaState(CSteamID steamIDFriend)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamFriends_GetFriendPersonaState(steamIDFriend);
        }

        public static EFriendRelationship GetFriendRelationship(CSteamID steamIDFriend)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamFriends_GetFriendRelationship(steamIDFriend);
        }

        public static string GetFriendRichPresence(CSteamID steamIDFriend, string pchKey)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchKey))
            {
                return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetFriendRichPresence(steamIDFriend, handle));
            }
        }

        public static string GetFriendRichPresenceKeyByIndex(CSteamID steamIDFriend, int iKey)
        {
            InteropHelp.TestIfAvailableClient();
            return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetFriendRichPresenceKeyByIndex(steamIDFriend, iKey));
        }

        public static int GetFriendRichPresenceKeyCount(CSteamID steamIDFriend)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamFriends_GetFriendRichPresenceKeyCount(steamIDFriend);
        }

        public static int GetFriendsGroupCount()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamFriends_GetFriendsGroupCount();
        }

        public static FriendsGroupID_t GetFriendsGroupIDByIndex(int iFG)
        {
            InteropHelp.TestIfAvailableClient();
            return (FriendsGroupID_t) NativeMethods.ISteamFriends_GetFriendsGroupIDByIndex(iFG);
        }

        public static int GetFriendsGroupMembersCount(FriendsGroupID_t friendsGroupID)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamFriends_GetFriendsGroupMembersCount(friendsGroupID);
        }

        public static void GetFriendsGroupMembersList(FriendsGroupID_t friendsGroupID, CSteamID[] pOutSteamIDMembers, int nMembersCount)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamFriends_GetFriendsGroupMembersList(friendsGroupID, pOutSteamIDMembers, nMembersCount);
        }

        public static string GetFriendsGroupName(FriendsGroupID_t friendsGroupID)
        {
            InteropHelp.TestIfAvailableClient();
            return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetFriendsGroupName(friendsGroupID));
        }

        public static int GetFriendSteamLevel(CSteamID steamIDFriend)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamFriends_GetFriendSteamLevel(steamIDFriend);
        }

        public static int GetLargeFriendAvatar(CSteamID steamIDFriend)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamFriends_GetLargeFriendAvatar(steamIDFriend);
        }

        public static int GetMediumFriendAvatar(CSteamID steamIDFriend)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamFriends_GetMediumFriendAvatar(steamIDFriend);
        }

        public static string GetPersonaName()
        {
            InteropHelp.TestIfAvailableClient();
            return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetPersonaName());
        }

        public static EPersonaState GetPersonaState()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamFriends_GetPersonaState();
        }

        public static string GetPlayerNickname(CSteamID steamIDPlayer)
        {
            InteropHelp.TestIfAvailableClient();
            return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetPlayerNickname(steamIDPlayer));
        }

        public static int GetSmallFriendAvatar(CSteamID steamIDFriend)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamFriends_GetSmallFriendAvatar(steamIDFriend);
        }

        public static uint GetUserRestrictions()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamFriends_GetUserRestrictions();
        }

        public static bool HasFriend(CSteamID steamIDFriend, EFriendFlags iFriendFlags)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamFriends_HasFriend(steamIDFriend, iFriendFlags);
        }

        public static bool InviteUserToGame(CSteamID steamIDFriend, string pchConnectString)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchConnectString))
            {
                return NativeMethods.ISteamFriends_InviteUserToGame(steamIDFriend, handle);
            }
        }

        public static bool IsClanChatAdmin(CSteamID steamIDClanChat, CSteamID steamIDUser)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamFriends_IsClanChatAdmin(steamIDClanChat, steamIDUser);
        }

        public static bool IsClanChatWindowOpenInSteam(CSteamID steamIDClanChat)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamFriends_IsClanChatWindowOpenInSteam(steamIDClanChat);
        }

        public static SteamAPICall_t IsFollowing(CSteamID steamID)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamFriends_IsFollowing(steamID);
        }

        public static bool IsUserInSource(CSteamID steamIDUser, CSteamID steamIDSource)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamFriends_IsUserInSource(steamIDUser, steamIDSource);
        }

        public static SteamAPICall_t JoinClanChatRoom(CSteamID steamIDClan)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamFriends_JoinClanChatRoom(steamIDClan);
        }

        public static bool LeaveClanChatRoom(CSteamID steamIDClan)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamFriends_LeaveClanChatRoom(steamIDClan);
        }

        public static bool OpenClanChatWindowInSteam(CSteamID steamIDClanChat)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamFriends_OpenClanChatWindowInSteam(steamIDClanChat);
        }

        public static bool ReplyToFriendMessage(CSteamID steamIDFriend, string pchMsgToSend)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchMsgToSend))
            {
                return NativeMethods.ISteamFriends_ReplyToFriendMessage(steamIDFriend, handle);
            }
        }

        public static SteamAPICall_t RequestClanOfficerList(CSteamID steamIDClan)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamFriends_RequestClanOfficerList(steamIDClan);
        }

        public static void RequestFriendRichPresence(CSteamID steamIDFriend)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamFriends_RequestFriendRichPresence(steamIDFriend);
        }

        public static bool RequestUserInformation(CSteamID steamIDUser, bool bRequireNameOnly)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamFriends_RequestUserInformation(steamIDUser, bRequireNameOnly);
        }

        public static bool SendClanChatMessage(CSteamID steamIDClanChat, string pchText)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchText))
            {
                return NativeMethods.ISteamFriends_SendClanChatMessage(steamIDClanChat, handle);
            }
        }

        public static void SetInGameVoiceSpeaking(CSteamID steamIDUser, bool bSpeaking)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamFriends_SetInGameVoiceSpeaking(steamIDUser, bSpeaking);
        }

        public static bool SetListenForFriendsMessages(bool bInterceptEnabled)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamFriends_SetListenForFriendsMessages(bInterceptEnabled);
        }

        public static SteamAPICall_t SetPersonaName(string pchPersonaName)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchPersonaName))
            {
                return (SteamAPICall_t) NativeMethods.ISteamFriends_SetPersonaName(handle);
            }
        }

        public static void SetPlayedWith(CSteamID steamIDUserPlayedWith)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamFriends_SetPlayedWith(steamIDUserPlayedWith);
        }

        public static bool SetRichPresence(string pchKey, string pchValue)
        {
            bool flag;
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchKey))
            {
                using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pchValue))
                {
                    flag = NativeMethods.ISteamFriends_SetRichPresence(handle, handle2);
                }
            }
            return flag;
        }
    }
}

