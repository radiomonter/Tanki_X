namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using UnityEngine;

    public class SimpleSelectableComponent : ECSBehaviour, Component, AttachToEntityListener
    {
        private Entity entity;
        [SerializeField]
        private GameObject selection;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Action<SimpleSelectableComponent, bool> selectEvent;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Action<SimpleSelectableComponent> destroyEvent;
        private bool hasSelected;
        [CompilerGenerated]
        private static Action<SimpleSelectableComponent, bool> <>f__am$cache0;
        [CompilerGenerated]
        private static Action<SimpleSelectableComponent> <>f__am$cache1;

        private event Action<SimpleSelectableComponent> destroyEvent
        {
            add
            {
                Action<SimpleSelectableComponent> destroyEvent = this.destroyEvent;
                while (true)
                {
                    Action<SimpleSelectableComponent> objB = destroyEvent;
                    destroyEvent = Interlocked.CompareExchange<Action<SimpleSelectableComponent>>(ref this.destroyEvent, objB + value, destroyEvent);
                    if (ReferenceEquals(destroyEvent, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                Action<SimpleSelectableComponent> destroyEvent = this.destroyEvent;
                while (true)
                {
                    Action<SimpleSelectableComponent> objB = destroyEvent;
                    destroyEvent = Interlocked.CompareExchange<Action<SimpleSelectableComponent>>(ref this.destroyEvent, objB - value, destroyEvent);
                    if (ReferenceEquals(destroyEvent, objB))
                    {
                        return;
                    }
                }
            }
        }

        private event Action<SimpleSelectableComponent, bool> selectEvent
        {
            add
            {
                Action<SimpleSelectableComponent, bool> selectEvent = this.selectEvent;
                while (true)
                {
                    Action<SimpleSelectableComponent, bool> objB = selectEvent;
                    selectEvent = Interlocked.CompareExchange<Action<SimpleSelectableComponent, bool>>(ref this.selectEvent, objB + value, selectEvent);
                    if (ReferenceEquals(selectEvent, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                Action<SimpleSelectableComponent, bool> selectEvent = this.selectEvent;
                while (true)
                {
                    Action<SimpleSelectableComponent, bool> objB = selectEvent;
                    selectEvent = Interlocked.CompareExchange<Action<SimpleSelectableComponent, bool>>(ref this.selectEvent, objB - value, selectEvent);
                    if (ReferenceEquals(selectEvent, objB))
                    {
                        return;
                    }
                }
            }
        }

        public SimpleSelectableComponent()
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = new Action<SimpleSelectableComponent, bool>(SimpleSelectableComponent.<selectEvent>m__0);
            }
            this.selectEvent = <>f__am$cache0;
            <>f__am$cache1 ??= new Action<SimpleSelectableComponent>(SimpleSelectableComponent.<destroyEvent>m__1);
            this.destroyEvent = <>f__am$cache1;
        }

        [CompilerGenerated]
        private static void <destroyEvent>m__1(SimpleSelectableComponent obj)
        {
        }

        [CompilerGenerated]
        private static void <selectEvent>m__0(SimpleSelectableComponent obj, bool selected)
        {
        }

        public void AddDestroyHandler(Action<SimpleSelectableComponent> handler)
        {
            this.destroyEvent += handler;
        }

        public void AddHandler(Action<SimpleSelectableComponent, bool> handler)
        {
            this.selectEvent += handler;
        }

        public void AttachedToEntity(Entity entity)
        {
            this.entity = entity;
        }

        public void Awake()
        {
            this.selection.SetActive(false);
        }

        public void ChangeState()
        {
            this.Select(true);
        }

        private void OnDestroy()
        {
            this.destroyEvent(this);
        }

        public void Select(bool selected)
        {
            if (this.hasSelected != selected)
            {
                this.selectEvent(this, selected);
                if (selected)
                {
                    base.ScheduleEvent<ListItemSelectedEvent>(this.entity);
                }
                else
                {
                    base.ScheduleEvent<ListItemDeselectedEvent>(this.entity);
                }
                this.hasSelected = selected;
                this.selection.SetActive(selected);
            }
        }
    }
}

