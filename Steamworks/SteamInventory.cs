﻿namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    public static class SteamInventory
    {
        public static bool AddPromoItem(out SteamInventoryResult_t pResultHandle, SteamItemDef_t itemDef)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamInventory_AddPromoItem(out pResultHandle, itemDef);
        }

        public static bool AddPromoItems(out SteamInventoryResult_t pResultHandle, SteamItemDef_t[] pArrayItemDefs, uint unArrayLength)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamInventory_AddPromoItems(out pResultHandle, pArrayItemDefs, unArrayLength);
        }

        public static bool CheckResultSteamID(SteamInventoryResult_t resultHandle, CSteamID steamIDExpected)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamInventory_CheckResultSteamID(resultHandle, steamIDExpected);
        }

        public static bool ConsumeItem(out SteamInventoryResult_t pResultHandle, SteamItemInstanceID_t itemConsume, uint unQuantity)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamInventory_ConsumeItem(out pResultHandle, itemConsume, unQuantity);
        }

        public static bool DeserializeResult(out SteamInventoryResult_t pOutResultHandle, byte[] pBuffer, uint unBufferSize, bool bRESERVED_MUST_BE_FALSE = false)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamInventory_DeserializeResult(out pOutResultHandle, pBuffer, unBufferSize, bRESERVED_MUST_BE_FALSE);
        }

        public static void DestroyResult(SteamInventoryResult_t resultHandle)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamInventory_DestroyResult(resultHandle);
        }

        public static bool ExchangeItems(out SteamInventoryResult_t pResultHandle, SteamItemDef_t[] pArrayGenerate, uint[] punArrayGenerateQuantity, uint unArrayGenerateLength, SteamItemInstanceID_t[] pArrayDestroy, uint[] punArrayDestroyQuantity, uint unArrayDestroyLength)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamInventory_ExchangeItems(out pResultHandle, pArrayGenerate, punArrayGenerateQuantity, unArrayGenerateLength, pArrayDestroy, punArrayDestroyQuantity, unArrayDestroyLength);
        }

        public static bool GenerateItems(out SteamInventoryResult_t pResultHandle, SteamItemDef_t[] pArrayItemDefs, uint[] punArrayQuantity, uint unArrayLength)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamInventory_GenerateItems(out pResultHandle, pArrayItemDefs, punArrayQuantity, unArrayLength);
        }

        public static bool GetAllItems(out SteamInventoryResult_t pResultHandle)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamInventory_GetAllItems(out pResultHandle);
        }

        public static bool GetEligiblePromoItemDefinitionIDs(CSteamID steamID, SteamItemDef_t[] pItemDefIDs, ref uint punItemDefIDsArraySize)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamInventory_GetEligiblePromoItemDefinitionIDs(steamID, pItemDefIDs, ref punItemDefIDsArraySize);
        }

        public static bool GetItemDefinitionIDs(SteamItemDef_t[] pItemDefIDs, out uint punItemDefIDsArraySize)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamInventory_GetItemDefinitionIDs(pItemDefIDs, out punItemDefIDsArraySize);
        }

        public static bool GetItemDefinitionProperty(SteamItemDef_t iDefinition, string pchPropertyName, out string pchValueBuffer, ref uint punValueBufferSizeOut)
        {
            InteropHelp.TestIfAvailableClient();
            IntPtr ptr = Marshal.AllocHGlobal((int) punValueBufferSizeOut);
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pchPropertyName))
            {
                bool flag = NativeMethods.ISteamInventory_GetItemDefinitionProperty(iDefinition, handle, ptr, ref punValueBufferSizeOut);
                pchValueBuffer = !flag ? null : InteropHelp.PtrToStringUTF8(ptr);
                Marshal.FreeHGlobal(ptr);
                return flag;
            }
        }

        public static bool GetItemsByID(out SteamInventoryResult_t pResultHandle, SteamItemInstanceID_t[] pInstanceIDs, uint unCountInstanceIDs)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamInventory_GetItemsByID(out pResultHandle, pInstanceIDs, unCountInstanceIDs);
        }

        public static bool GetResultItems(SteamInventoryResult_t resultHandle, SteamItemDetails_t[] pOutItemsArray, ref uint punOutItemsArraySize)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamInventory_GetResultItems(resultHandle, pOutItemsArray, ref punOutItemsArraySize);
        }

        public static EResult GetResultStatus(SteamInventoryResult_t resultHandle)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamInventory_GetResultStatus(resultHandle);
        }

        public static uint GetResultTimestamp(SteamInventoryResult_t resultHandle)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamInventory_GetResultTimestamp(resultHandle);
        }

        public static bool GrantPromoItems(out SteamInventoryResult_t pResultHandle)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamInventory_GrantPromoItems(out pResultHandle);
        }

        public static bool LoadItemDefinitions()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamInventory_LoadItemDefinitions();
        }

        public static SteamAPICall_t RequestEligiblePromoItemDefinitionsIDs(CSteamID steamID)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamInventory_RequestEligiblePromoItemDefinitionsIDs(steamID);
        }

        public static void SendItemDropHeartbeat()
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamInventory_SendItemDropHeartbeat();
        }

        public static bool SerializeResult(SteamInventoryResult_t resultHandle, byte[] pOutBuffer, out uint punOutBufferSize)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamInventory_SerializeResult(resultHandle, pOutBuffer, out punOutBufferSize);
        }

        public static bool TradeItems(out SteamInventoryResult_t pResultHandle, CSteamID steamIDTradePartner, SteamItemInstanceID_t[] pArrayGive, uint[] pArrayGiveQuantity, uint nArrayGiveLength, SteamItemInstanceID_t[] pArrayGet, uint[] pArrayGetQuantity, uint nArrayGetLength)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamInventory_TradeItems(out pResultHandle, steamIDTradePartner, pArrayGive, pArrayGiveQuantity, nArrayGiveLength, pArrayGet, pArrayGetQuantity, nArrayGetLength);
        }

        public static bool TransferItemQuantity(out SteamInventoryResult_t pResultHandle, SteamItemInstanceID_t itemIdSource, uint unQuantity, SteamItemInstanceID_t itemIdDest)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamInventory_TransferItemQuantity(out pResultHandle, itemIdSource, unQuantity, itemIdDest);
        }

        public static bool TriggerItemDrop(out SteamInventoryResult_t pResultHandle, SteamItemDef_t dropListDefinition)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamInventory_TriggerItemDrop(out pResultHandle, dropListDefinition);
        }
    }
}

