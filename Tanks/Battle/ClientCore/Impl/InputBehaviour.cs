namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class InputBehaviour : MonoBehaviour
    {
        private void Update()
        {
            if (InputManager != null)
            {
                InputManager.Update();
            }
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }
    }
}

