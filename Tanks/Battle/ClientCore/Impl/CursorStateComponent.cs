namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class CursorStateComponent : Component
    {
        private bool cursorVisible = true;
        private CursorLockMode cursorLockState;

        public CursorLockMode CursorLockState
        {
            get => 
                this.cursorLockState;
            set => 
                this.cursorLockState = value;
        }

        public bool CursorVisible
        {
            get => 
                this.cursorVisible;
            set => 
                this.cursorVisible = value;
        }
    }
}

