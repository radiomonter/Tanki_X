namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class InputManagerImpl : InputManager
    {
        public static bool SUSPENDED;
        private static float EPS = 0.05f;
        private MultiMap<string, InputAction> contextToActions = new MultiMap<string, InputAction>();
        private MultiMap<InputAction, KeyCode> actionToKeys = new MultiMap<InputAction, KeyCode>();
        private MultiMap<InputAction, string> actionToAxes = new MultiMap<InputAction, string>();
        private MultiMap<string, InputAction> nameToAction = new MultiMap<string, InputAction>();
        private HashSet<int> keysPressed = new HashSet<int>();
        private HashSet<string> activeContexts = new HashSet<string>();
        private Dictionary<InputAction, float> axisVals = new Dictionary<InputAction, float>();
        private HashSet<InputAction> pendingActions = new HashSet<InputAction>();
        private int resumeAtFrame;
        [CompilerGenerated]
        private static Predicate<InputAction> <>f__am$cache0;
        [CompilerGenerated]
        private static Func<KeyCode, bool> <>f__am$cache1;

        private float AbsAxisVal(string axisName) => 
            Math.Abs(this.GetAxisOrKey(axisName));

        public void ActivateContext(string contextName)
        {
            this.activeContexts.Add(contextName);
            this.Update();
        }

        private void AddKeysToMap(InputAction action)
        {
            KeyCode[] keys = action.keys;
            int length = keys.Length;
            for (int i = 0; i < length; i++)
            {
                this.actionToKeys.Add(action, keys[i]);
            }
            MultiKeys[] multiKeys = action.multiKeys;
            int num3 = multiKeys.Length;
            int index = 0;
            while (index < num3)
            {
                KeyCode[] codeArray2 = multiKeys[index].keys;
                length = codeArray2.Length;
                int num5 = 0;
                while (true)
                {
                    if (num5 >= length)
                    {
                        index++;
                        break;
                    }
                    this.actionToKeys.Add(action, codeArray2[num5]);
                    num5++;
                }
            }
            int num6 = action.axes.Length;
            for (int j = 0; j < num6; j++)
            {
                string enumDescription = GetEnumDescription(action.axes[j]);
                this.actionToAxes.Add(action, enumDescription);
            }
        }

        private bool AllActiveContextsContainAction(string name)
        {
            bool flag = false;
            HashSet<string>.Enumerator enumerator = this.activeContexts.GetEnumerator();
            while (enumerator.MoveNext())
            {
                string current = enumerator.Current;
                HashSet<InputAction>.Enumerator enumerator2 = this.contextToActions[current].GetEnumerator();
                bool flag2 = false;
                while (true)
                {
                    if (enumerator2.MoveNext())
                    {
                        if (enumerator2.Current.actionId.actionName != name)
                        {
                            continue;
                        }
                        flag2 = true;
                        flag = true;
                    }
                    if (flag2)
                    {
                        break;
                    }
                    return false;
                }
            }
            return flag;
        }

        private bool AnyAxisInput() => 
            (this.IsAxisActive(MoveActions.FORWARD) || (this.IsAxisActive(MoveActions.BACK) || (this.IsAxisActive(MoveActions.LEFT) || (this.IsAxisActive(MoveActions.RIGHT) || (this.IsAxisActive(ShotActions.SHOT) || this.IsAxisActive(WeaponActions.WEAPON_LEFT)))))) || this.IsAxisActive(WeaponActions.WEAPON_RIGHT);

        private void ChangeInputActionKey(InputActionId actionId, InputActionContextId contextId, int keyId, InputKeyCode newKeyCode)
        {
            foreach (InputAction action in this.nameToAction[actionId.actionName])
            {
                if (Equals(action.contextId, contextId))
                {
                    KeyCode keyCode = (newKeyCode == null) ? KeyCode.None : newKeyCode.keyCode;
                    this.RemoveKeysFromMap(action);
                    this.SetActionKey(action, keyId, keyCode);
                    this.AddKeysToMap(action);
                    this.SaveKeys(action, keyId, keyCode);
                }
            }
        }

        public void ChangeInputActionKey(InputActionId actionId, InputActionContextId contextId, int keyId, KeyCode newKeyCode)
        {
            this.ChangeInputActionKey(actionId, contextId, keyId, new InputKeyCode(newKeyCode));
        }

        public bool CheckAction(string actionName)
        {
            if (!this.Suspended)
            {
                HashSet<string>.Enumerator enumerator = this.activeContexts.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    string current = enumerator.Current;
                    HashSet<InputAction>.Enumerator enumerator2 = this.contextToActions[current].GetEnumerator();
                    while (enumerator2.MoveNext())
                    {
                        InputAction action = enumerator2.Current;
                        if (action.actionId.actionName == actionName)
                        {
                            KeyCode[] codeArray = action.keys;
                            int length = codeArray.Length;
                            int index = 0;
                            while (true)
                            {
                                if (index >= length)
                                {
                                    MultiKeys[] multiKeys = action.multiKeys;
                                    int num3 = multiKeys.Length;
                                    for (int i = 0; i < num3; i++)
                                    {
                                        MultiKeys keys = multiKeys[i];
                                        codeArray = keys.keys;
                                        length = codeArray.Length;
                                        if (length > 0)
                                        {
                                            bool flag = true;
                                            int num5 = 0;
                                            while (true)
                                            {
                                                if (num5 >= length)
                                                {
                                                    if (!flag)
                                                    {
                                                        break;
                                                    }
                                                    return true;
                                                }
                                                flag &= this.keysPressed.Contains((int) codeArray[num5]);
                                                num5++;
                                            }
                                        }
                                    }
                                    break;
                                }
                                if (this.keysPressed.Contains((int) codeArray[index]))
                                {
                                    return true;
                                }
                                index++;
                            }
                        }
                    }
                }
                HashSet<InputAction>.Enumerator enumerator3 = this.pendingActions.GetEnumerator();
                while (enumerator3.MoveNext())
                {
                    InputAction current = enumerator3.Current;
                    if (current.actionId.actionName == actionName)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CheckMouseButtonInAllActiveContexts(string actionName, int mouseButton) => 
            !this.Suspended ? (Input.GetMouseButton(mouseButton) ? this.AllActiveContextsContainAction(actionName) : false) : false;

        public void ClearActions()
        {
            this.keysPressed.Clear();
            this.pendingActions.Clear();
        }

        public void DeactivateContext(string contextName)
        {
            if (this.activeContexts.Contains(contextName))
            {
                HashSet<InputAction> set = this.contextToActions[contextName];
                this.activeContexts.Remove(contextName);
                HashSet<InputAction>.Enumerator enumerator = set.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    InputAction current = enumerator.Current;
                    KeyCode[] keys = current.keys;
                    int length = keys.Length;
                    for (int i = 0; i < length; i++)
                    {
                        if (this.keysPressed.Contains((int) keys[i]))
                        {
                            this.pendingActions.Add(current);
                        }
                    }
                }
                this.ResetAxes(contextName);
            }
        }

        public void DeleteKeyBinding(InputActionId actionId, InputActionContextId contextId, int id)
        {
            this.ChangeInputActionKey(actionId, contextId, id, (InputKeyCode) null);
        }

        private string GeneratePersistentKey(InputAction action, int id) => 
            action.contextId.ToString() + action.actionId + id;

        public InputAction GetAction(InputActionId actionId, InputActionContextId contextId)
        {
            InputAction action2;
            using (HashSet<InputAction>.Enumerator enumerator = this.nameToAction.GetValues(actionId.actionName).GetEnumerator())
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        InputAction current = enumerator.Current;
                        if (!current.contextId.Equals(contextId))
                        {
                            continue;
                        }
                        action2 = current;
                    }
                    else
                    {
                        return null;
                    }
                    break;
                }
            }
            return action2;
        }

        public bool GetActionKeyDown(string actionName)
        {
            if (!this.Suspended)
            {
                HashSet<string>.Enumerator enumerator = this.activeContexts.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    string current = enumerator.Current;
                    HashSet<InputAction>.Enumerator enumerator2 = this.contextToActions[current].GetEnumerator();
                    while (enumerator2.MoveNext())
                    {
                        InputAction action = enumerator2.Current;
                        if (action.actionId.actionName == actionName)
                        {
                            KeyCode[] keys = action.keys;
                            int length = keys.Length;
                            for (int i = 0; i < length; i++)
                            {
                                if (Input.GetKeyDown(keys[i]))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public bool GetActionKeyUp(string actionName)
        {
            if (!this.Suspended)
            {
                HashSet<string>.Enumerator enumerator = this.activeContexts.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    string current = enumerator.Current;
                    HashSet<InputAction>.Enumerator enumerator2 = this.contextToActions[current].GetEnumerator();
                    while (enumerator2.MoveNext())
                    {
                        InputAction action = enumerator2.Current;
                        if (action.actionId.actionName == actionName)
                        {
                            KeyCode[] keys = action.keys;
                            int length = keys.Length;
                            for (int i = 0; i < length; i++)
                            {
                                if (Input.GetKeyUp(keys[i]))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                HashSet<InputAction>.Enumerator enumerator3 = this.pendingActions.GetEnumerator();
                while (enumerator3.MoveNext())
                {
                    InputAction current = enumerator3.Current;
                    if (current.actionId.actionName == actionName)
                    {
                        KeyCode[] keys = current.keys;
                        int length = keys.Length;
                        for (int i = 0; i < length; i++)
                        {
                            if (Input.GetKeyUp(keys[i]))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public float GetAxis(string name, bool mustExistForAllContext)
        {
            if (this.Suspended)
            {
                return 0f;
            }
            HashSet<string>.Enumerator enumerator = this.activeContexts.GetEnumerator();
            float num = 0f;
            while (enumerator.MoveNext())
            {
                string current = enumerator.Current;
                HashSet<InputAction>.Enumerator enumerator2 = this.contextToActions[current].GetEnumerator();
                bool flag = false;
                while (true)
                {
                    if (!enumerator2.MoveNext())
                    {
                        if (!mustExistForAllContext || flag)
                        {
                            break;
                        }
                        return 0f;
                    }
                    InputAction key = enumerator2.Current;
                    if (key.actionId.actionName == name)
                    {
                        float num2;
                        flag = true;
                        if (this.axisVals.TryGetValue(key, out num2) && (num2 != 0f))
                        {
                            num = num2;
                        }
                    }
                }
            }
            if ((num == 0f) && this.CheckAction(name))
            {
                num = 1f;
            }
            return num;
        }

        public float GetAxisOrKey(string actionName)
        {
            int num = !this.CheckAction(actionName) ? 0 : 1;
            return ((num != 1) ? this.GetAxis(actionName, false) : ((float) num));
        }

        public InputKeyCode GetCurrentKeyPressed()
        {
            InputKeyCode code2;
            IEnumerator enumerator = Enum.GetValues(typeof(KeyCode)).GetEnumerator();
            try
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        object current = enumerator.Current;
                        KeyCode key = (KeyCode) current;
                        if (!Input.GetKey(key))
                        {
                            continue;
                        }
                        code2 = new InputKeyCode(key);
                    }
                    else
                    {
                        return null;
                    }
                    break;
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            return code2;
        }

        private static string GetEnumDescription(Enum value)
        {
            DescriptionAttribute[] customAttributes = (DescriptionAttribute[]) value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (((customAttributes == null) || (customAttributes.Length <= 0)) ? value.ToString() : customAttributes[0].Description);
        }

        public bool GetKey(KeyCode keyCode) => 
            !this.Suspended ? this.keysPressed.Contains((int) keyCode) : false;

        public bool GetKeyDown(KeyCode keyCode) => 
            !this.Suspended ? Input.GetKeyDown(keyCode) : false;

        public bool GetKeyUp(KeyCode keyCode) => 
            !this.Suspended ? Input.GetKeyUp(keyCode) : false;

        public bool GetMouseButton(int mouseButton) => 
            !this.Suspended && Input.GetMouseButton(mouseButton);

        public bool GetMouseButtonDown(int mouseButton) => 
            !this.Suspended && Input.GetMouseButtonDown(mouseButton);

        public bool GetMouseButtonUp(int mouseButton) => 
            !this.Suspended && Input.GetMouseButtonUp(mouseButton);

        public float GetUnityAxis(string axisName) => 
            !this.Suspended ? Input.GetAxis(axisName) : 0f;

        public bool IsAnyKey() => 
            !this.Suspended ? (Input.anyKeyDown || this.AnyAxisInput()) : false;

        private bool IsAxisActive(string axisName) => 
            this.AbsAxisVal(axisName) > EPS;

        private void LoadInputAction(InputAction inputAction)
        {
            this.LoadSavedKey(inputAction, 0);
            this.LoadSavedKey(inputAction, 1);
        }

        private void LoadSavedKey(InputAction action, int id)
        {
            string str2 = PlayerPrefs.GetString(this.GeneratePersistentKey(action, id));
            if (!string.IsNullOrEmpty(str2))
            {
                KeyCode keyCode = (KeyCode) Enum.Parse(typeof(KeyCode), str2);
                this.SetActionKey(action, id, keyCode);
            }
        }

        public void RegisterDefaultInputAction(InputAction action)
        {
            this.RegisterInputAction(action);
        }

        public void RegisterInputAction(InputAction action)
        {
            string actionName = action.actionId.actionName;
            this.nameToAction.Add(actionName, action);
            string contextName = action.contextId.contextName;
            this.contextToActions.Add(contextName, action);
            this.LoadInputAction(action);
            this.AddKeysToMap(action);
        }

        private void RemoveKeysFromMap(InputAction action)
        {
            KeyCode[] keys = action.keys;
            int length = keys.Length;
            for (int i = 0; i < length; i++)
            {
                this.actionToKeys.Remove(action, keys[i]);
            }
        }

        private void ResetAxes(string contextName)
        {
            HashSet<InputAction>.Enumerator enumerator = this.contextToActions[contextName].GetEnumerator();
            while (enumerator.MoveNext())
            {
                InputAction current = enumerator.Current;
                if (this.axisVals.ContainsKey(current))
                {
                    this.axisVals[current] = 0f;
                }
            }
        }

        public void ResetToDefaultActions()
        {
            foreach (InputAction action in this.actionToKeys.Keys)
            {
                PlayerPrefs.DeleteKey(this.GeneratePersistentKey(action, 0));
                PlayerPrefs.DeleteKey(this.GeneratePersistentKey(action, 1));
                this.RegisterInputAction(action);
            }
            this.contextToActions.Clear();
            this.actionToKeys.Clear();
            this.actionToAxes.Clear();
            this.nameToAction.Clear();
            Object.FindObjectOfType<InputActivator>().LoadDefaultInputActions();
        }

        public void Resume()
        {
            SUSPENDED = false;
            this.resumeAtFrame = 0;
        }

        public void ResumeAtNextFrame()
        {
            SUSPENDED = false;
            this.resumeAtFrame = Time.frameCount;
        }

        private void SaveKeys(InputAction action, int id, KeyCode keyCode)
        {
            PlayerPrefs.SetString(this.GeneratePersistentKey(action, id), keyCode.ToString());
        }

        private void SetActionKey(InputAction action, int id, KeyCode keyCode)
        {
            if (id >= action.keys.Length)
            {
                KeyCode[] keys = action.keys;
                action.keys = new KeyCode[id + 1];
                for (int i = 0; i < keys.Length; i++)
                {
                    action.keys[i] = keys[i];
                }
            }
            action.keys[id] = keyCode;
        }

        public void Suspend()
        {
            this.ClearActions();
            SUSPENDED = true;
        }

        public void Update()
        {
            if (!this.Suspended)
            {
                HashSet<string>.Enumerator enumerator = this.activeContexts.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    string current = enumerator.Current;
                    HashSet<InputAction>.Enumerator enumerator2 = this.contextToActions[current].GetEnumerator();
                    while (enumerator2.MoveNext())
                    {
                        InputAction action = enumerator2.Current;
                        this.UpdateKeysInActiveContext(action);
                        this.UpdateAxisInActiveContext(action);
                    }
                }
                if (this.pendingActions.Count > 0)
                {
                    if (<>f__am$cache0 == null)
                    {
                        <>f__am$cache0 = delegate (InputAction action) {
                            if (<>f__am$cache1 == null)
                            {
                                <>f__am$cache1 = key => Input.GetKey(key) || Input.GetKeyUp(key);
                            }
                            return !action.keys.Any<KeyCode>(<>f__am$cache1);
                        };
                    }
                    this.pendingActions.RemoveWhere(<>f__am$cache0);
                }
            }
        }

        private void UpdateAxisInActiveContext(InputAction action)
        {
            if (this.actionToAxes.ContainsKey(action))
            {
                HashSet<string>.Enumerator enumerator = this.actionToAxes[action].GetEnumerator();
                while (enumerator.MoveNext())
                {
                    string current = enumerator.Current;
                    float axis = Input.GetAxis(current);
                    bool flag = action.onlyNegativeAxes && !action.onlyPositiveAxes;
                    bool flag2 = !action.onlyNegativeAxes && action.onlyPositiveAxes;
                    float num2 = !action.invertAxes ? ((float) 1) : ((float) (-1));
                    this.axisVals[action] = !flag ? (!flag2 ? (axis * num2) : (((axis * num2) <= 0f) ? 0f : Mathf.Abs(axis))) : (((axis * num2) >= 0f) ? 0f : Mathf.Abs(axis));
                }
            }
        }

        private void UpdateKeysInActiveContext(InputAction action)
        {
            if (this.actionToKeys.ContainsKey(action))
            {
                HashSet<KeyCode>.Enumerator enumerator = this.actionToKeys[action].GetEnumerator();
                while (enumerator.MoveNext())
                {
                    KeyCode current = enumerator.Current;
                    int item = (int) current;
                    bool flag = this.keysPressed.Contains(item);
                    if (Input.GetKey(current))
                    {
                        if (flag)
                        {
                            continue;
                        }
                        this.keysPressed.Add(item);
                        action.StartInputAction();
                        continue;
                    }
                    if (flag)
                    {
                        this.keysPressed.Remove(item);
                        action.StopInputAction();
                    }
                }
            }
        }

        private bool Suspended =>
            SUSPENDED || (Time.frameCount <= this.resumeAtFrame);
    }
}

