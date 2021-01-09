namespace Tanks.Battle.ClientCore.Impl
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public interface InputManager
    {
        void ActivateContext(string contextName);
        void ChangeInputActionKey(InputActionId actionId, InputActionContextId contextId, int keyId, KeyCode newKeyCode);
        bool CheckAction(string actionName);
        bool CheckMouseButtonInAllActiveContexts(string actionName, int mouseButton);
        void ClearActions();
        void DeactivateContext(string contextName);
        void DeleteKeyBinding(InputActionId actionId, InputActionContextId contextId, int id);
        InputAction GetAction(InputActionId actionId, InputActionContextId contextId);
        bool GetActionKeyDown(string actionName);
        bool GetActionKeyUp(string actionName);
        float GetAxis(string name, bool mustExistForAllContext = false);
        float GetAxisOrKey(string actionName);
        InputKeyCode GetCurrentKeyPressed();
        bool GetKey(KeyCode keyCode);
        bool GetKeyDown(KeyCode keyCode);
        bool GetKeyUp(KeyCode keyCode);
        bool GetMouseButton(int mouseButton);
        bool GetMouseButtonDown(int mouseButton);
        bool GetMouseButtonUp(int mouseButton);
        float GetUnityAxis(string axisName);
        bool IsAnyKey();
        void RegisterDefaultInputAction(InputAction inputAction);
        void RegisterInputAction(InputAction action);
        void ResetToDefaultActions();
        void Resume();
        void ResumeAtNextFrame();
        void Suspend();
        void Update();
    }
}

