﻿namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    public static class SteamController
    {
        public static void ActivateActionSet(ControllerHandle_t controllerHandle, ControllerActionSetHandle_t actionSetHandle)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamController_ActivateActionSet(controllerHandle, actionSetHandle);
        }

        public static ControllerActionSetHandle_t GetActionSetHandle(string pszActionSetName)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pszActionSetName))
            {
                return (ControllerActionSetHandle_t) NativeMethods.ISteamController_GetActionSetHandle(handle);
            }
        }

        public static ControllerAnalogActionData_t GetAnalogActionData(ControllerHandle_t controllerHandle, ControllerAnalogActionHandle_t analogActionHandle)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamController_GetAnalogActionData(controllerHandle, analogActionHandle);
        }

        public static ControllerAnalogActionHandle_t GetAnalogActionHandle(string pszActionName)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pszActionName))
            {
                return (ControllerAnalogActionHandle_t) NativeMethods.ISteamController_GetAnalogActionHandle(handle);
            }
        }

        public static int GetAnalogActionOrigins(ControllerHandle_t controllerHandle, ControllerActionSetHandle_t actionSetHandle, ControllerAnalogActionHandle_t analogActionHandle, EControllerActionOrigin[] originsOut)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamController_GetAnalogActionOrigins(controllerHandle, actionSetHandle, analogActionHandle, originsOut);
        }

        public static int GetConnectedControllers(ControllerHandle_t[] handlesOut)
        {
            InteropHelp.TestIfAvailableClient();
            if (handlesOut.Length != 0x10)
            {
                throw new ArgumentException("handlesOut must be the same size as Constants.STEAM_CONTROLLER_MAX_COUNT!");
            }
            return NativeMethods.ISteamController_GetConnectedControllers(handlesOut);
        }

        public static ControllerHandle_t GetControllerForGamepadIndex(int nIndex)
        {
            InteropHelp.TestIfAvailableClient();
            return (ControllerHandle_t) NativeMethods.ISteamController_GetControllerForGamepadIndex(nIndex);
        }

        public static ControllerActionSetHandle_t GetCurrentActionSet(ControllerHandle_t controllerHandle)
        {
            InteropHelp.TestIfAvailableClient();
            return (ControllerActionSetHandle_t) NativeMethods.ISteamController_GetCurrentActionSet(controllerHandle);
        }

        public static ControllerDigitalActionData_t GetDigitalActionData(ControllerHandle_t controllerHandle, ControllerDigitalActionHandle_t digitalActionHandle)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamController_GetDigitalActionData(controllerHandle, digitalActionHandle);
        }

        public static ControllerDigitalActionHandle_t GetDigitalActionHandle(string pszActionName)
        {
            InteropHelp.TestIfAvailableClient();
            using (InteropHelp.UTF8StringHandle handle = new InteropHelp.UTF8StringHandle(pszActionName))
            {
                return (ControllerDigitalActionHandle_t) NativeMethods.ISteamController_GetDigitalActionHandle(handle);
            }
        }

        public static int GetDigitalActionOrigins(ControllerHandle_t controllerHandle, ControllerActionSetHandle_t actionSetHandle, ControllerDigitalActionHandle_t digitalActionHandle, EControllerActionOrigin[] originsOut)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamController_GetDigitalActionOrigins(controllerHandle, actionSetHandle, digitalActionHandle, originsOut);
        }

        public static int GetGamepadIndexForController(ControllerHandle_t ulControllerHandle)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamController_GetGamepadIndexForController(ulControllerHandle);
        }

        public static string GetGlyphForActionOrigin(EControllerActionOrigin eOrigin)
        {
            InteropHelp.TestIfAvailableClient();
            return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamController_GetGlyphForActionOrigin(eOrigin));
        }

        public static ControllerMotionData_t GetMotionData(ControllerHandle_t controllerHandle)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamController_GetMotionData(controllerHandle);
        }

        public static string GetStringForActionOrigin(EControllerActionOrigin eOrigin)
        {
            InteropHelp.TestIfAvailableClient();
            return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamController_GetStringForActionOrigin(eOrigin));
        }

        public static bool Init()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamController_Init();
        }

        public static void RunFrame()
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamController_RunFrame();
        }

        public static void SetLEDColor(ControllerHandle_t controllerHandle, byte nColorR, byte nColorG, byte nColorB, uint nFlags)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamController_SetLEDColor(controllerHandle, nColorR, nColorG, nColorB, nFlags);
        }

        public static bool ShowAnalogActionOrigins(ControllerHandle_t controllerHandle, ControllerAnalogActionHandle_t analogActionHandle, float flScale, float flXPosition, float flYPosition)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamController_ShowAnalogActionOrigins(controllerHandle, analogActionHandle, flScale, flXPosition, flYPosition);
        }

        public static bool ShowBindingPanel(ControllerHandle_t controllerHandle)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamController_ShowBindingPanel(controllerHandle);
        }

        public static bool ShowDigitalActionOrigins(ControllerHandle_t controllerHandle, ControllerDigitalActionHandle_t digitalActionHandle, float flScale, float flXPosition, float flYPosition)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamController_ShowDigitalActionOrigins(controllerHandle, digitalActionHandle, flScale, flXPosition, flYPosition);
        }

        public static bool Shutdown()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamController_Shutdown();
        }

        public static void StopAnalogActionMomentum(ControllerHandle_t controllerHandle, ControllerAnalogActionHandle_t eAction)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamController_StopAnalogActionMomentum(controllerHandle, eAction);
        }

        public static void TriggerHapticPulse(ControllerHandle_t controllerHandle, ESteamControllerPad eTargetPad, ushort usDurationMicroSec)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamController_TriggerHapticPulse(controllerHandle, eTargetPad, usDurationMicroSec);
        }

        public static void TriggerRepeatedHapticPulse(ControllerHandle_t controllerHandle, ESteamControllerPad eTargetPad, ushort usDurationMicroSec, ushort usOffMicroSec, ushort unRepeat, uint nFlags)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamController_TriggerRepeatedHapticPulse(controllerHandle, eTargetPad, usDurationMicroSec, usOffMicroSec, unRepeat, nFlags);
        }

        public static void TriggerVibration(ControllerHandle_t controllerHandle, ushort usLeftSpeed, ushort usRightSpeed)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamController_TriggerVibration(controllerHandle, usLeftSpeed, usRightSpeed);
        }
    }
}

