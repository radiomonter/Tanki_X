namespace Steamworks
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public static class SteamGameServerUGC
    {
        public static bool AddExcludedTag(UGCQueryHandle_t handle, string pTagName)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pTagName))
            {
                return NativeMethods.ISteamGameServerUGC_AddExcludedTag(handle, handle2);
            }
        }

        public static bool AddItemKeyValueTag(UGCUpdateHandle_t handle, string pchKey, string pchValue)
        {
            bool flag;
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pchKey))
            {
                using (InteropHelp.UTF8StringHandle handle3 = new InteropHelp.UTF8StringHandle(pchValue))
                {
                    flag = NativeMethods.ISteamGameServerUGC_AddItemKeyValueTag(handle, handle2, handle3);
                }
            }
            return flag;
        }

        public static bool AddItemPreviewFile(UGCUpdateHandle_t handle, string pszPreviewFile, EItemPreviewType type)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pszPreviewFile))
            {
                return NativeMethods.ISteamGameServerUGC_AddItemPreviewFile(handle, handle2, type);
            }
        }

        public static bool AddItemPreviewVideo(UGCUpdateHandle_t handle, string pszVideoID)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pszVideoID))
            {
                return NativeMethods.ISteamGameServerUGC_AddItemPreviewVideo(handle, handle2);
            }
        }

        public static SteamAPICall_t AddItemToFavorites(AppId_t nAppId, PublishedFileId_t nPublishedFileID)
        {
            InteropHelp.TestIfAvailableGameServer();
            return (SteamAPICall_t) NativeMethods.ISteamGameServerUGC_AddItemToFavorites(nAppId, nPublishedFileID);
        }

        public static bool AddRequiredKeyValueTag(UGCQueryHandle_t handle, string pKey, string pValue)
        {
            bool flag;
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pKey))
            {
                using (InteropHelp.UTF8StringHandle handle3 = new InteropHelp.UTF8StringHandle(pValue))
                {
                    flag = NativeMethods.ISteamGameServerUGC_AddRequiredKeyValueTag(handle, handle2, handle3);
                }
            }
            return flag;
        }

        public static bool AddRequiredTag(UGCQueryHandle_t handle, string pTagName)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pTagName))
            {
                return NativeMethods.ISteamGameServerUGC_AddRequiredTag(handle, handle2);
            }
        }

        public static bool BInitWorkshopForGameServer(DepotId_t unWorkshopDepotID, string pszFolder)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pszFolder))
            {
                return NativeMethods.ISteamGameServerUGC_BInitWorkshopForGameServer(unWorkshopDepotID, handle);
            }
        }

        public static SteamAPICall_t CreateItem(AppId_t nConsumerAppId, EWorkshopFileType eFileType)
        {
            InteropHelp.TestIfAvailableGameServer();
            return (SteamAPICall_t) NativeMethods.ISteamGameServerUGC_CreateItem(nConsumerAppId, eFileType);
        }

        public static UGCQueryHandle_t CreateQueryAllUGCRequest(EUGCQuery eQueryType, EUGCMatchingUGCType eMatchingeMatchingUGCTypeFileType, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage)
        {
            InteropHelp.TestIfAvailableGameServer();
            return (UGCQueryHandle_t) NativeMethods.ISteamGameServerUGC_CreateQueryAllUGCRequest(eQueryType, eMatchingeMatchingUGCTypeFileType, nCreatorAppID, nConsumerAppID, unPage);
        }

        public static UGCQueryHandle_t CreateQueryUGCDetailsRequest(PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs)
        {
            InteropHelp.TestIfAvailableGameServer();
            return (UGCQueryHandle_t) NativeMethods.ISteamGameServerUGC_CreateQueryUGCDetailsRequest(pvecPublishedFileID, unNumPublishedFileIDs);
        }

        public static UGCQueryHandle_t CreateQueryUserUGCRequest(AccountID_t unAccountID, EUserUGCList eListType, EUGCMatchingUGCType eMatchingUGCType, EUserUGCListSortOrder eSortOrder, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage)
        {
            InteropHelp.TestIfAvailableGameServer();
            return (UGCQueryHandle_t) NativeMethods.ISteamGameServerUGC_CreateQueryUserUGCRequest(unAccountID, eListType, eMatchingUGCType, eSortOrder, nCreatorAppID, nConsumerAppID, unPage);
        }

        public static bool DownloadItem(PublishedFileId_t nPublishedFileID, bool bHighPriority)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUGC_DownloadItem(nPublishedFileID, bHighPriority);
        }

        public static bool GetItemDownloadInfo(PublishedFileId_t nPublishedFileID, out ulong punBytesDownloaded, out ulong punBytesTotal)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUGC_GetItemDownloadInfo(nPublishedFileID, out punBytesDownloaded, out punBytesTotal);
        }

        public static bool GetItemInstallInfo(PublishedFileId_t nPublishedFileID, out ulong punSizeOnDisk, out string pchFolder, uint cchFolderSize, out uint punTimeStamp)
        {
            InteropHelp.TestIfAvailableGameServer();
            IntPtr ptr = Marshal.AllocHGlobal((int) cchFolderSize);
            bool flag = NativeMethods.ISteamGameServerUGC_GetItemInstallInfo(nPublishedFileID, out punSizeOnDisk, ptr, cchFolderSize, out punTimeStamp);
            pchFolder = !flag ? null : InteropHelp.PtrToStringUTF8(ptr);
            Marshal.FreeHGlobal(ptr);
            return flag;
        }

        public static uint GetItemState(PublishedFileId_t nPublishedFileID)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUGC_GetItemState(nPublishedFileID);
        }

        public static EItemUpdateStatus GetItemUpdateProgress(UGCUpdateHandle_t handle, out ulong punBytesProcessed, out ulong punBytesTotal)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUGC_GetItemUpdateProgress(handle, out punBytesProcessed, out punBytesTotal);
        }

        public static uint GetNumSubscribedItems()
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUGC_GetNumSubscribedItems();
        }

        public static bool GetQueryUGCAdditionalPreview(UGCQueryHandle_t handle, uint index, uint previewIndex, out string pchURLOrVideoID, uint cchURLSize, out string pchOriginalFileName, uint cchOriginalFileNameSize, out EItemPreviewType pPreviewType)
        {
            InteropHelp.TestIfAvailableGameServer();
            IntPtr ptr = Marshal.AllocHGlobal((int) cchURLSize);
            IntPtr ptr2 = Marshal.AllocHGlobal((int) cchOriginalFileNameSize);
            bool flag = NativeMethods.ISteamGameServerUGC_GetQueryUGCAdditionalPreview(handle, index, previewIndex, ptr, cchURLSize, ptr2, cchOriginalFileNameSize, out pPreviewType);
            pchURLOrVideoID = !flag ? null : InteropHelp.PtrToStringUTF8(ptr);
            Marshal.FreeHGlobal(ptr);
            pchOriginalFileName = !flag ? null : InteropHelp.PtrToStringUTF8(ptr2);
            Marshal.FreeHGlobal(ptr2);
            return flag;
        }

        public static bool GetQueryUGCChildren(UGCQueryHandle_t handle, uint index, PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUGC_GetQueryUGCChildren(handle, index, pvecPublishedFileID, cMaxEntries);
        }

        public static bool GetQueryUGCKeyValueTag(UGCQueryHandle_t handle, uint index, uint keyValueTagIndex, out string pchKey, uint cchKeySize, out string pchValue, uint cchValueSize)
        {
            InteropHelp.TestIfAvailableGameServer();
            IntPtr ptr = Marshal.AllocHGlobal((int) cchKeySize);
            IntPtr ptr2 = Marshal.AllocHGlobal((int) cchValueSize);
            bool flag = NativeMethods.ISteamGameServerUGC_GetQueryUGCKeyValueTag(handle, index, keyValueTagIndex, ptr, cchKeySize, ptr2, cchValueSize);
            pchKey = !flag ? null : InteropHelp.PtrToStringUTF8(ptr);
            Marshal.FreeHGlobal(ptr);
            pchValue = !flag ? null : InteropHelp.PtrToStringUTF8(ptr2);
            Marshal.FreeHGlobal(ptr2);
            return flag;
        }

        public static bool GetQueryUGCMetadata(UGCQueryHandle_t handle, uint index, out string pchMetadata, uint cchMetadatasize)
        {
            InteropHelp.TestIfAvailableGameServer();
            IntPtr ptr = Marshal.AllocHGlobal((int) cchMetadatasize);
            bool flag = NativeMethods.ISteamGameServerUGC_GetQueryUGCMetadata(handle, index, ptr, cchMetadatasize);
            pchMetadata = !flag ? null : InteropHelp.PtrToStringUTF8(ptr);
            Marshal.FreeHGlobal(ptr);
            return flag;
        }

        public static uint GetQueryUGCNumAdditionalPreviews(UGCQueryHandle_t handle, uint index)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUGC_GetQueryUGCNumAdditionalPreviews(handle, index);
        }

        public static uint GetQueryUGCNumKeyValueTags(UGCQueryHandle_t handle, uint index)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUGC_GetQueryUGCNumKeyValueTags(handle, index);
        }

        public static bool GetQueryUGCPreviewURL(UGCQueryHandle_t handle, uint index, out string pchURL, uint cchURLSize)
        {
            InteropHelp.TestIfAvailableGameServer();
            IntPtr ptr = Marshal.AllocHGlobal((int) cchURLSize);
            bool flag = NativeMethods.ISteamGameServerUGC_GetQueryUGCPreviewURL(handle, index, ptr, cchURLSize);
            pchURL = !flag ? null : InteropHelp.PtrToStringUTF8(ptr);
            Marshal.FreeHGlobal(ptr);
            return flag;
        }

        public static bool GetQueryUGCResult(UGCQueryHandle_t handle, uint index, out SteamUGCDetails_t pDetails)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUGC_GetQueryUGCResult(handle, index, out pDetails);
        }

        public static bool GetQueryUGCStatistic(UGCQueryHandle_t handle, uint index, EItemStatistic eStatType, out ulong pStatValue)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUGC_GetQueryUGCStatistic(handle, index, eStatType, out pStatValue);
        }

        public static uint GetSubscribedItems(PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUGC_GetSubscribedItems(pvecPublishedFileID, cMaxEntries);
        }

        public static SteamAPICall_t GetUserItemVote(PublishedFileId_t nPublishedFileID)
        {
            InteropHelp.TestIfAvailableGameServer();
            return (SteamAPICall_t) NativeMethods.ISteamGameServerUGC_GetUserItemVote(nPublishedFileID);
        }

        public static bool ReleaseQueryUGCRequest(UGCQueryHandle_t handle)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUGC_ReleaseQueryUGCRequest(handle);
        }

        public static SteamAPICall_t RemoveItemFromFavorites(AppId_t nAppId, PublishedFileId_t nPublishedFileID)
        {
            InteropHelp.TestIfAvailableGameServer();
            return (SteamAPICall_t) NativeMethods.ISteamGameServerUGC_RemoveItemFromFavorites(nAppId, nPublishedFileID);
        }

        public static bool RemoveItemKeyValueTags(UGCUpdateHandle_t handle, string pchKey)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pchKey))
            {
                return NativeMethods.ISteamGameServerUGC_RemoveItemKeyValueTags(handle, handle2);
            }
        }

        public static bool RemoveItemPreview(UGCUpdateHandle_t handle, uint index)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUGC_RemoveItemPreview(handle, index);
        }

        public static SteamAPICall_t RequestUGCDetails(PublishedFileId_t nPublishedFileID, uint unMaxAgeSeconds)
        {
            InteropHelp.TestIfAvailableGameServer();
            return (SteamAPICall_t) NativeMethods.ISteamGameServerUGC_RequestUGCDetails(nPublishedFileID, unMaxAgeSeconds);
        }

        public static SteamAPICall_t SendQueryUGCRequest(UGCQueryHandle_t handle)
        {
            InteropHelp.TestIfAvailableGameServer();
            return (SteamAPICall_t) NativeMethods.ISteamGameServerUGC_SendQueryUGCRequest(handle);
        }

        public static bool SetAllowCachedResponse(UGCQueryHandle_t handle, uint unMaxAgeSeconds)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUGC_SetAllowCachedResponse(handle, unMaxAgeSeconds);
        }

        public static bool SetCloudFileNameFilter(UGCQueryHandle_t handle, string pMatchCloudFileName)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pMatchCloudFileName))
            {
                return NativeMethods.ISteamGameServerUGC_SetCloudFileNameFilter(handle, handle2);
            }
        }

        public static bool SetItemContent(UGCUpdateHandle_t handle, string pszContentFolder)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pszContentFolder))
            {
                return NativeMethods.ISteamGameServerUGC_SetItemContent(handle, handle2);
            }
        }

        public static bool SetItemDescription(UGCUpdateHandle_t handle, string pchDescription)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pchDescription))
            {
                return NativeMethods.ISteamGameServerUGC_SetItemDescription(handle, handle2);
            }
        }

        public static bool SetItemMetadata(UGCUpdateHandle_t handle, string pchMetaData)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pchMetaData))
            {
                return NativeMethods.ISteamGameServerUGC_SetItemMetadata(handle, handle2);
            }
        }

        public static bool SetItemPreview(UGCUpdateHandle_t handle, string pszPreviewFile)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pszPreviewFile))
            {
                return NativeMethods.ISteamGameServerUGC_SetItemPreview(handle, handle2);
            }
        }

        public static bool SetItemTags(UGCUpdateHandle_t updateHandle, IList<string> pTags)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUGC_SetItemTags(updateHandle, (IntPtr) new InteropHelp.SteamParamStringArray(pTags));
        }

        public static bool SetItemTitle(UGCUpdateHandle_t handle, string pchTitle)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pchTitle))
            {
                return NativeMethods.ISteamGameServerUGC_SetItemTitle(handle, handle2);
            }
        }

        public static bool SetItemUpdateLanguage(UGCUpdateHandle_t handle, string pchLanguage)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pchLanguage))
            {
                return NativeMethods.ISteamGameServerUGC_SetItemUpdateLanguage(handle, handle2);
            }
        }

        public static bool SetItemVisibility(UGCUpdateHandle_t handle, ERemoteStoragePublishedFileVisibility eVisibility)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUGC_SetItemVisibility(handle, eVisibility);
        }

        public static bool SetLanguage(UGCQueryHandle_t handle, string pchLanguage)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pchLanguage))
            {
                return NativeMethods.ISteamGameServerUGC_SetLanguage(handle, handle2);
            }
        }

        public static bool SetMatchAnyTag(UGCQueryHandle_t handle, bool bMatchAnyTag)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUGC_SetMatchAnyTag(handle, bMatchAnyTag);
        }

        public static bool SetRankedByTrendDays(UGCQueryHandle_t handle, uint unDays)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUGC_SetRankedByTrendDays(handle, unDays);
        }

        public static bool SetReturnAdditionalPreviews(UGCQueryHandle_t handle, bool bReturnAdditionalPreviews)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUGC_SetReturnAdditionalPreviews(handle, bReturnAdditionalPreviews);
        }

        public static bool SetReturnChildren(UGCQueryHandle_t handle, bool bReturnChildren)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUGC_SetReturnChildren(handle, bReturnChildren);
        }

        public static bool SetReturnKeyValueTags(UGCQueryHandle_t handle, bool bReturnKeyValueTags)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUGC_SetReturnKeyValueTags(handle, bReturnKeyValueTags);
        }

        public static bool SetReturnLongDescription(UGCQueryHandle_t handle, bool bReturnLongDescription)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUGC_SetReturnLongDescription(handle, bReturnLongDescription);
        }

        public static bool SetReturnMetadata(UGCQueryHandle_t handle, bool bReturnMetadata)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUGC_SetReturnMetadata(handle, bReturnMetadata);
        }

        public static bool SetReturnOnlyIDs(UGCQueryHandle_t handle, bool bReturnOnlyIDs)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUGC_SetReturnOnlyIDs(handle, bReturnOnlyIDs);
        }

        public static bool SetReturnTotalOnly(UGCQueryHandle_t handle, bool bReturnTotalOnly)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamGameServerUGC_SetReturnTotalOnly(handle, bReturnTotalOnly);
        }

        public static bool SetSearchText(UGCQueryHandle_t handle, string pSearchText)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pSearchText))
            {
                return NativeMethods.ISteamGameServerUGC_SetSearchText(handle, handle2);
            }
        }

        public static SteamAPICall_t SetUserItemVote(PublishedFileId_t nPublishedFileID, bool bVoteUp)
        {
            InteropHelp.TestIfAvailableGameServer();
            return (SteamAPICall_t) NativeMethods.ISteamGameServerUGC_SetUserItemVote(nPublishedFileID, bVoteUp);
        }

        public static UGCUpdateHandle_t StartItemUpdate(AppId_t nConsumerAppId, PublishedFileId_t nPublishedFileID)
        {
            InteropHelp.TestIfAvailableGameServer();
            return (UGCUpdateHandle_t) NativeMethods.ISteamGameServerUGC_StartItemUpdate(nConsumerAppId, nPublishedFileID);
        }

        public static SteamAPICall_t StartPlaytimeTracking(PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs)
        {
            InteropHelp.TestIfAvailableGameServer();
            return (SteamAPICall_t) NativeMethods.ISteamGameServerUGC_StartPlaytimeTracking(pvecPublishedFileID, unNumPublishedFileIDs);
        }

        public static SteamAPICall_t StopPlaytimeTracking(PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs)
        {
            InteropHelp.TestIfAvailableGameServer();
            return (SteamAPICall_t) NativeMethods.ISteamGameServerUGC_StopPlaytimeTracking(pvecPublishedFileID, unNumPublishedFileIDs);
        }

        public static SteamAPICall_t StopPlaytimeTrackingForAllItems()
        {
            InteropHelp.TestIfAvailableGameServer();
            return (SteamAPICall_t) NativeMethods.ISteamGameServerUGC_StopPlaytimeTrackingForAllItems();
        }

        public static SteamAPICall_t SubmitItemUpdate(UGCUpdateHandle_t handle, string pchChangeNote)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pchChangeNote))
            {
                return (SteamAPICall_t) NativeMethods.ISteamGameServerUGC_SubmitItemUpdate(handle, handle2);
            }
        }

        public static SteamAPICall_t SubscribeItem(PublishedFileId_t nPublishedFileID)
        {
            InteropHelp.TestIfAvailableGameServer();
            return (SteamAPICall_t) NativeMethods.ISteamGameServerUGC_SubscribeItem(nPublishedFileID);
        }

        public static void SuspendDownloads(bool bSuspend)
        {
            InteropHelp.TestIfAvailableGameServer();
            NativeMethods.ISteamGameServerUGC_SuspendDownloads(bSuspend);
        }

        public static SteamAPICall_t UnsubscribeItem(PublishedFileId_t nPublishedFileID)
        {
            InteropHelp.TestIfAvailableGameServer();
            return (SteamAPICall_t) NativeMethods.ISteamGameServerUGC_UnsubscribeItem(nPublishedFileID);
        }

        public static bool UpdateItemPreviewFile(UGCUpdateHandle_t handle, uint index, string pszPreviewFile)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pszPreviewFile))
            {
                return NativeMethods.ISteamGameServerUGC_UpdateItemPreviewFile(handle, index, handle2);
            }
        }

        public static bool UpdateItemPreviewVideo(UGCUpdateHandle_t handle, uint index, string pszVideoID)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pszVideoID))
            {
                return NativeMethods.ISteamGameServerUGC_UpdateItemPreviewVideo(handle, index, handle2);
            }
        }
    }
}

