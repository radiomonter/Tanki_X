namespace Tanks.Lobby.ClientNavigation.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class DialogsSystem : ECSSystem
    {
        [OnEventFire]
        public void AddScreenLockClickListener(NodeAddedEvent e, SingleNode<ScreenLockComponent> screenLock)
        {
            screenLock.component.gameObject.AddComponent<DialogsOuterClickListener>().ClickAction = new Action<PointerEventData>(this.OnClick);
        }

        [OnEventFire]
        public void CloseDialog(NodeRemoveEvent e, SingleNode<ActiveScreenComponent> screen, [JoinAll] SingleNode<Dialogs60Component> dialogs)
        {
            dialogs.component.CloseAll(string.Empty);
        }

        [OnEventComplete]
        public void MergeDialogs(NodeAddedEvent e, SingleNode<Dialogs60Component> newDialogs, [JoinAll, Combine] SingleNode<Dialogs60Component> dialogs)
        {
            if (!ReferenceEquals(newDialogs.Entity, dialogs.Entity))
            {
                while (newDialogs.component.transform.childCount > 0)
                {
                    newDialogs.component.transform.GetChild(0).SetParent(dialogs.component.transform, false);
                }
                Object.Destroy(newDialogs.component.gameObject);
            }
        }

        private void OnClick(PointerEventData eventData)
        {
            DialogsOuterClickEvent eventInstance = new DialogsOuterClickEvent {
                EventData = eventData
            };
            EngineService.Engine.ScheduleEvent(eventInstance, EngineService.EntityStub);
        }

        [OnEventFire]
        public void SendCancelEventToDialogs(DialogsOuterClickEvent e, Node node, [JoinAll] SingleNode<Dialogs60Component> dialogs)
        {
            IEnumerator enumerator = dialogs.component.transform.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform) enumerator.Current;
                    ExecuteEvents.Execute<ICancelHandler>(current.gameObject, e.EventData, ExecuteEvents.cancelHandler);
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
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }
    }
}

