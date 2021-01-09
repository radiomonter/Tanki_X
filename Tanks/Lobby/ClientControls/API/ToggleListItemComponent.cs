namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(UnityEngine.UI.Toggle))]
    public class ToggleListItemComponent : MonoBehaviour, Component, AttachToEntityListener
    {
        private Platform.Kernel.ECS.ClientEntitySystem.API.Entity entity;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Action<bool> onValueChanged;
        [CompilerGenerated]
        private static Action<bool> <>f__am$cache0;

        public event Action<bool> onValueChanged
        {
            add
            {
                Action<bool> onValueChanged = this.onValueChanged;
                while (true)
                {
                    Action<bool> objB = onValueChanged;
                    onValueChanged = Interlocked.CompareExchange<Action<bool>>(ref this.onValueChanged, objB + value, onValueChanged);
                    if (ReferenceEquals(onValueChanged, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                Action<bool> onValueChanged = this.onValueChanged;
                while (true)
                {
                    Action<bool> objB = onValueChanged;
                    onValueChanged = Interlocked.CompareExchange<Action<bool>>(ref this.onValueChanged, objB - value, onValueChanged);
                    if (ReferenceEquals(onValueChanged, objB))
                    {
                        return;
                    }
                }
            }
        }

        public ToggleListItemComponent()
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = new Action<bool>(ToggleListItemComponent.<onValueChanged>m__0);
            }
            this.onValueChanged = <>f__am$cache0;
        }

        [CompilerGenerated]
        private static void <onValueChanged>m__0(bool state)
        {
        }

        public void AttachedToEntity(Platform.Kernel.ECS.ClientEntitySystem.API.Entity entity)
        {
            this.entity = entity;
        }

        public void AttachToParentToggleGroup()
        {
            this.Toggle.group = base.GetComponentInParent<ToggleGroup>();
        }

        private void OnDisable()
        {
            if (this.Toggle.isOn)
            {
                this.Toggle.isOn = false;
            }
        }

        public void OnValueChangedListener()
        {
            if (!this.Toggle.isOn)
            {
                if (this.entity.HasComponent<ToggleListSelectedItemComponent>())
                {
                    this.entity.RemoveComponent<ToggleListSelectedItemComponent>();
                }
            }
            else
            {
                if (this.entity.HasComponent<ToggleListSelectedItemComponent>())
                {
                    this.entity.RemoveComponent<ToggleListSelectedItemComponent>();
                }
                this.entity.AddComponent<ToggleListSelectedItemComponent>();
            }
            this.onValueChanged(this.Toggle.isOn);
        }

        private void Start()
        {
            this.AttachToParentToggleGroup();
        }

        public Platform.Kernel.ECS.ClientEntitySystem.API.Entity Entity =>
            this.entity;

        public UnityEngine.UI.Toggle Toggle =>
            base.GetComponent<UnityEngine.UI.Toggle>();
    }
}

