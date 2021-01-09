namespace Steamworks
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public static class SteamUGC
    {
        public static bool AddExcludedTag(UGCQueryHandle_t handle, string pTagName)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pTagName))
            {
                return NativeMethods.ISteamUGC_AddExcludedTag(handle, handle2);
            }
        }

        public static bool AddItemKeyValueTag(UGCUpdateHandle_t handle, string pchKey, string pchValue)
        {
            bool flag;
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pchKey))
            {
                using (InteropHelp.UTF8StringHandle handle3 = new InteropHelp.UTF8StringHandle(pchValue))
                {
                    flag = NativeMethods.ISteamUGC_AddItemKeyValueTag(handle, handle2, handle3);
                }
            }
            return flag;
        }

        public static bool AddItemPreviewFile(UGCUpdateHandle_t handle, string pszPreviewFile, EItemPreviewType type)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pszPreviewFile))
            {
                return NativeMethods.ISteamUGC_AddItemPreviewFile(handle, handle2, type);
            }
        }

        public static bool AddItemPreviewVideo(UGCUpdateHandle_t handle, string pszVideoID)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pszVideoID))
            {
                return NativeMethods.ISteamUGC_AddItemPreviewVideo(handle, handle2);
            }
        }

        public static SteamAPICall_t AddItemToFavorites(AppId_t nAppId, PublishedFileId_t nPublishedFileID)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamUGC_AddItemToFavorites(nAppId, nPublishedFileID);
        }

        public static bool AddRequiredKeyValueTag(UGCQueryHandle_t handle, string pKey, string pValue)
        {
            bool flag;
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pKey))
            {
                using (InteropHelp.UTF8StringHandle handle3 = new InteropHelp.UTF8StringHandle(pValue))
                {
                    flag = NativeMethods.ISteamUGC_AddRequiredKeyValueTag(handle, handle2, handle3);
                }
            }
            return flag;
        }

        public static bool AddRequiredTag(UGCQueryHandle_t handle, string pTagName)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pTagName))
            {
                return NativeMethods.ISteamUGC_AddRequiredTag(handle, handle2);
            }
        }

        public static bool BInitWorkshopForGameServer(DepotId_t unWorkshopDepotID, string pszFolder)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pszFolder))
            {
                return NativeMethods.ISteamUGC_BInitWorkshopForGameServer(unWorkshopDepotID, handle);
            }
        }

        public static SteamAPICall_t CreateItem(AppId_t nConsumerAppId, EWorkshopFileType eFileType)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamUGC_CreateItem(nConsumerAppId, eFileType);
        }

        public static UGCQueryHandle_t CreateQueryAllUGCRequest(EUGCQuery eQueryType, EUGCMatchingUGCType eMatchingeMatchingUGCTypeFileType, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage)
        {
            InteropHelp.TestIfAvailableClient();
            return (UGCQueryHandle_t) NativeMethods.ISteamUGC_CreateQueryAllUGCRequest(eQueryType, eMatchingeMatchingUGCTypeFileType, nCreatorAppID, nConsumerAppID, unPage);
        }

        public static UGCQueryHandle_t CreateQueryUGCDetailsRequest(PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs)
        {
            InteropHelp.TestIfAvailableClient();
            return (UGCQueryHandle_t) NativeMethods.ISteamUGC_CreateQueryUGCDetailsRequest(pvecPublishedFileID, unNumPublishedFileIDs);
        }

        public static UGCQueryHandle_t CreateQueryUserUGCRequest(AccountID_t unAccountID, EUserUGCList eListType, EUGCMatchingUGCType eMatchingUGCType, EUserUGCListSortOrder eSortOrder, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage)
        {
            InteropHelp.TestIfAvailableClient();
            return (UGCQueryHandle_t) NativeMethods.ISteamUGC_CreateQueryUserUGCRequest(unAccountID, eListType, eMatchingUGCType, eSortOrder, nCreatorAppID, nConsumerAppID, unPage);
        }

        public static bool DownloadItem(PublishedFileId_t nPublishedFileID, bool bHighPriority)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUGC_DownloadItem(nPublishedFileID, bHighPriority);
        }

        public static bool GetItemDownloadInfo(PublishedFileId_t nPublishedFileID, out ulong punBytesDownloaded, out ulong punBytesTotal)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUGC_GetItemDownloadInfo(nPublishedFileID, out punBytesDownloaded, out punBytesTotal);
        }

        public static bool GetItemInstallInfo(PublishedFileId_t nPublishedFileID, out ulong punSizeOnDisk, out string pchFolder, uint cchFolderSize, out uint punTimeStamp)
        {
            InteropHelp.TestIfAvailableClient();
            IntPtr ptr = Marshal.AllocHGlobal((int) cchFolderSize);
            bool flag = NativeMethods.ISteamUGC_GetItemInstallInfo(nPublishedFileID, out punSizeOnDisk, ptr, cchFolderSize, out punTimeStamp);
            pchFolder = !flag ? null : InteropHelp.PtrToStringUTF8(ptr);
            Marshal.FreeHGlobal(ptr);
            return flag;
        }

        public static uint GetItemState(PublishedFileId_t nPublishedFileID)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUGC_GetItemState(nPublishedFileID);
        }

        public static EItemUpdateStatus GetItemUpdateProgress(UGCUpdateHandle_t handle, out ulong punBytesProcessed, out ulong punBytesTotal)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUGC_GetItemUpdateProgress(handle, out punBytesProcessed, out punBytesTotal);
        }

        public static uint GetNumSubscribedItems()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUGC_GetNumSubscribedItems();
        }

        public static bool GetQueryUGCAdditionalPreview(UGCQueryHandle_t handle, uint index, uint previewIndex, out string pchURLOrVideoID, uint cchURLSize, out string pchOriginalFileName, uint cchOriginalFileNameSize, out EItemPreviewType pPreviewType)
        {
            InteropHelp.TestIfAvailableClient();
            IntPtr ptr = Marshal.AllocHGlobal((int) cchURLSize);
            IntPtr ptr2 = Marshal.AllocHGlobal((int) cchOriginalFileNameSize);
            bool flag = NativeMethods.ISteamUGC_GetQueryUGCAdditionalPreview(handle, index, previewIndex, ptr, cchURLSize, ptr2, cchOriginalFileNameSize, out pPreviewType);
            pchURLOrVideoID = !flag ? null : InteropHelp.PtrToStringUTF8(ptr);
            Marshal.FreeHGlobal(ptr);
            pchOriginalFileName = !flag ? null : InteropHelp.PtrToStringUTF8(ptr2);
            Marshal.FreeHGlobal(ptr2);
            return flag;
        }

        public static bool GetQueryUGCChildren(UGCQueryHandle_t handle, uint index, PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUGC_GetQueryUGCChildren(handle, index, pvecPublishedFileID, cMaxEntries);
        }

        public static bool GetQueryUGCKeyValueTag(UGCQueryHandle_t handle, uint index, uint keyValueTagIndex, out string pchKey, uint cchKeySize, out string pchValue, uint cchValueSize)
        {
            InteropHelp.TestIfAvailableClient();
            IntPtr ptr = Marshal.AllocHGlobal((int) cchKeySize);
            IntPtr ptr2 = Marshal.AllocHGlobal((int) cchValueSize);
            bool flag = NativeMethods.ISteamUGC_GetQueryUGCKeyValueTag(handle, index, keyValueTagIndex, ptr, cchKeySize, ptr2, cchValueSize);
            pchKey = !flag ? null : InteropHelp.PtrToStringUTF8(ptr);
            Marshal.FreeHGlobal(ptr);
            pchValue = !flag ? null : InteropHelp.PtrToStringUTF8(ptr2);
            Marshal.FreeHGlobal(ptr2);
            return flag;
        }

        public static bool GetQueryUGCMetadata(UGCQueryHandle_t handle, uint index, out string pchMetadata, uint cchMetadatasize)
        {
            InteropHelp.TestIfAvailableClient();
            IntPtr ptr = Marshal.AllocHGlobal((int) cchMetadatasize);
            bool flag = NativeMethods.ISteamUGC_GetQueryUGCMetadata(handle, index, ptr, cchMetadatasize);
            pchMetadata = !flag ? null : InteropHelp.PtrToStringUTF8(ptr);
            Marshal.FreeHGlobal(ptr);
            return flag;
        }

        public static uint GetQueryUGCNumAdditionalPreviews(UGCQueryHandle_t handle, uint index)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUGC_GetQueryUGCNumAdditionalPreviews(handle, index);
        }

        public static uint GetQueryUGCNumKeyValueTags(UGCQueryHandle_t handle, uint index)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUGC_GetQueryUGCNumKeyValueTags(handle, index);
        }

        public static bool GetQueryUGCPreviewURL(UGCQueryHandle_t handle, uint index, out string pchURL, uint cchURLSize)
        {
            InteropHelp.TestIfAvailableClient();
            IntPtr ptr = Marshal.AllocHGlobal((int) cchURLSize);
            bool flag = NativeMethods.ISteamUGC_GetQueryUGCPreviewURL(handle, index, ptr, cchURLSize);
            pchURL = !flag ? null : InteropHelp.PtrToStringUTF8(ptr);
            Marshal.FreeHGlobal(ptr);
            return flag;
        }

        public static bool GetQueryUGCResult(UGCQueryHandle_t handle, uint index, out SteamUGCDetails_t pDetails)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUGC_GetQueryUGCResult(handle, index, out pDetails);
        }

        public static bool GetQueryUGCStatistic(UGCQueryHandle_t handle, uint index, EItemStatistic eStatType, out ulong pStatValue)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUGC_GetQueryUGCStatistic(handle, index, eStatType, out pStatValue);
        }

        public static uint GetSubscribedItems(PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUGC_GetSubscribedItems(pvecPublishedFileID, cMaxEntries);
        }

        public static SteamAPICall_t GetUserItemVote(PublishedFileId_t nPublishedFileID)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamUGC_GetUserItemVote(nPublishedFileID);
        }

        public static bool ReleaseQueryUGCRequest(UGCQueryHandle_t handle)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUGC_ReleaseQueryUGCRequest(handle);
        }

        public static SteamAPICall_t RemoveItemFromFavorites(AppId_t nAppId, PublishedFileId_t nPublishedFileID)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamUGC_RemoveItemFromFavorites(nAppId, nPublishedFileID);
        }

        public static bool RemoveItemKeyValueTags(UGCUpdateHandle_t handle, string pchKey)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pchKey))
            {
                return NativeMethods.ISteamUGC_RemoveItemKeyValueTags(handle, handle2);
            }
        }

        public static bool RemoveItemPreview(UGCUpdateHandle_t handle, uint index)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUGC_RemoveItemPreview(handle, index);
        }

        public static SteamAPICall_t RequestUGCDetails(PublishedFileId_t nPublishedFileID, uint unMaxAgeSeconds)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamUGC_RequestUGCDetails(nPublishedFileID, unMaxAgeSeconds);
        }

        public static SteamAPICall_t SendQueryUGCRequest(UGCQueryHandle_t handle)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamUGC_SendQueryUGCRequest(handle);
        }

        public static bool SetAllowCachedResponse(UGCQueryHandle_t handle, uint unMaxAgeSeconds)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUGC_SetAllowCachedResponse(handle, unMaxAgeSeconds);
        }

        public static bool SetCloudFileNameFilter(UGCQueryHandle_t handle, string pMatchCloudFileName)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pMatchCloudFileName))
            {
                return NativeMethods.ISteamUGC_SetCloudFileNameFilter(handle, handle2);
            }
        }

        public static bool SetItemContent(UGCUpdateHandle_t handle, string pszContentFolder)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pszContentFolder))
            {
                return NativeMethods.ISteamUGC_SetItemContent(handle, handle2);
            }
        }

        public static bool SetItemDescription(UGCUpdateHandle_t handle, string pchDescription)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pchDescription))
            {
                return NativeMethods.ISteamUGC_SetItemDescription(handle, handle2);
            }
        }

        public static bool SetItemMetadata(UGCUpdateHandle_t handle, string pchMetaData)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pchMetaData))
            {
                return NativeMethods.ISteamUGC_SetItemMetadata(handle, handle2);
            }
        }

        public static bool SetItemPreview(UGCUpdateHandle_t handle, string pszPreviewFile)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pszPreviewFile))
            {
                return NativeMethods.ISteamUGC_SetItemPreview(handle, handle2);
            }
        }

        public static bool SetItemTags(UGCUpdateHandle_t updateHandle, IList<string> pTags)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUGC_SetItemTags(updateHandle, (IntPtr) new InteropHelp.SteamParamStringArray(pTags));
        }

        public static bool SetItemTitle(UGCUpdateHandle_t handle, string pchTitle)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pchTitle))
            {
                return NativeMethods.ISteamUGC_SetItemTitle(handle, handle2);
            }
        }

        public static bool SetItemUpdateLanguage(UGCUpdateHandle_t handle, string pchLanguage)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pchLanguage))
            {
                return NativeMethods.ISteamUGC_SetItemUpdateLanguage(handle, handle2);
            }
        }

        public static bool SetItemVisibility(UGCUpdateHandle_t handle, ERemoteStoragePublishedFileVisibility eVisibility)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUGC_SetItemVisibility(handle, eVisibility);
        }

        public static bool SetLanguage(UGCQueryHandle_t handle, string pchLanguage)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pchLanguage))
            {
                return NativeMethods.ISteamUGC_SetLanguage(handle, handle2);
            }
        }

        public static bool SetMatchAnyTag(UGCQueryHandle_t handle, bool bMatchAnyTag)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUGC_SetMatchAnyTag(handle, bMatchAnyTag);
        }

        public static bool SetRankedByTrendDays(UGCQueryHandle_t handle, uint unDays)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUGC_SetRankedByTrendDays(handle, unDays);
        }

        public static bool SetReturnAdditionalPreviews(UGCQueryHandle_t handle, bool bReturnAdditionalPreviews)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUGC_SetReturnAdditionalPreviews(handle, bReturnAdditionalPreviews);
        }

        public static bool SetReturnChildren(UGCQueryHandle_t handle, bool bReturnChildren)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUGC_SetReturnChildren(handle, bReturnChildren);
        }

        public static bool SetReturnKeyValueTags(UGCQueryHandle_t handle, bool bReturnKeyValueTags)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUGC_SetReturnKeyValueTags(handle, bReturnKeyValueTags);
        }

        public static bool SetReturnLongDescription(UGCQueryHandle_t handle, bool bReturnLongDescription)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUGC_SetReturnLongDescription(handle, bReturnLongDescription);
        }

        public static bool SetReturnMetadata(UGCQueryHandle_t handle, bool bReturnMetadata)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUGC_SetReturnMetadata(handle, bReturnMetadata);
        }

        public static bool SetReturnOnlyIDs(UGCQueryHandle_t handle, bool bReturnOnlyIDs)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUGC_SetReturnOnlyIDs(handle, bReturnOnlyIDs);
        }

        public static bool SetReturnTotalOnly(UGCQueryHandle_t handle, bool bReturnTotalOnly)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamUGC_SetReturnTotalOnly(handle, bReturnTotalOnly);
        }

        public static bool SetSearchText(UGCQueryHandle_t handle, string pSearchText)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pSearchText))
            {
                return NativeMethods.ISteamUGC_SetSearchText(handle, handle2);
            }
        }

        public static SteamAPICall_t SetUserItemVote(PublishedFileId_t nPublishedFileID, bool bVoteUp)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamUGC_SetUserItemVote(nPublishedFileID, bVoteUp);
        }

        public static UGCUpdateHandle_t StartItemUpdate(AppId_t nConsumerAppId, PublishedFileId_t nPublishedFileID)
        {
            InteropHelp.TestIfAvailableClient();
            return (UGCUpdateHandle_t) NativeMethods.ISteamUGC_StartItemUpdate(nConsumerAppId, nPublishedFileID);
        }

        public static SteamAPICall_t StartPlaytimeTracking(PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamUGC_StartPlaytimeTracking(pvecPublishedFileID, unNumPublishedFileIDs);
        }

        public static SteamAPICall_t StopPlaytimeTracking(PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamUGC_StopPlaytimeTracking(pvecPublishedFileID, unNumPublishedFileIDs);
        }

        public static SteamAPICall_t StopPlaytimeTrackingForAllItems()
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamUGC_StopPlaytimeTrackingForAllItems();
        }

        public static SteamAPICall_t SubmitItemUpdate(UGCUpdateHandle_t handle, string pchChangeNote)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pchChangeNote))
            {
                return (SteamAPICall_t) NativeMethods.ISteamUGC_SubmitItemUpdate(handle, handle2);
            }
        }

        public static SteamAPICall_t SubscribeItem(PublishedFileId_t nPublishedFileID)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamUGC_SubscribeItem(nPublishedFileID);
        }

        public static void SuspendDownloads(bool bSuspend)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamUGC_SuspendDownloads(bSuspend);
        }

        public static SteamAPICall_t UnsubscribeItem(PublishedFileId_t nPublishedFileID)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamUGC_UnsubscribeItem(nPublishedFileID);
        }

        public static bool UpdateItemPreviewFile(UGCUpdateHandle_t handle, uint index, string pszPreviewFile)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pszPreviewFile))
            {
                return NativeMethods.ISteamUGC_UpdateItemPreviewFile(handle, index, handle2);
            }
        }

        public static bool UpdateItemPreviewVideo(UGCUpdateHandle_t handle, uint index, string pszVideoID)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle2 = new InteropHelp.UTF8StringHandle(pszVideoID))
            {
                return NativeMethods.ISteamUGC_UpdateItemPreviewVideo(handle, index, handle2);
            }
        }
    }
}

