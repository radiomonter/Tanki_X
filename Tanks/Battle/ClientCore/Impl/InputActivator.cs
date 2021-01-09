namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class InputActivator : UnityAwareActivator<AutoCompleting>
    {
        public GameObject[] inputBinding;
        [CompilerGenerated]
        private static Action<InputAction> <>f__am$cache0;
        [CompilerGenerated]
        private static Action<InputAction> <>f__am$cache1;

        protected override void Activate()
        {
            string key = "DefaultInputsLoaded";
            if (PlayerPrefs.HasKey(key))
            {
                this.LoadInputActions();
            }
            else
            {
                this.LoadDefaultInputActions();
                PlayerPrefs.SetInt(key, 1);
            }
            InputManager.ActivateContext(BasicContexts.BATTLE_CONTEXT);
            base.gameObject.AddComponent<InputBehaviour>();
        }

        public void LoadDefaultInputActions()
        {
            foreach (GameObject obj2 in this.inputBinding)
            {
                GameObject obj3 = Instantiate<GameObject>(obj2, base.transform);
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = a => InputManager.RegisterDefaultInputAction(a);
                }
                obj3.GetComponents<InputAction>().ForEach<InputAction>(<>f__am$cache0);
            }
        }

        private void LoadInputActions()
        {
            foreach (GameObject obj2 in this.inputBinding)
            {
                GameObject obj3 = Instantiate<GameObject>(obj2, base.transform);
                if (<>f__am$cache1 == null)
                {
                    <>f__am$cache1 = a => InputManager.RegisterInputAction(a);
                }
                obj3.GetComponents<InputAction>().ForEach<InputAction>(<>f__am$cache1);
            }
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }
    }
}

