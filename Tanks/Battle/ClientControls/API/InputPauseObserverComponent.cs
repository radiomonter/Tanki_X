namespace Tanks.Battle.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class InputPauseObserverComponent : ECSBehaviour, Component, AttachToEntityListener
    {
        private bool inputChanged;
        private float lastChangeTime;
        private Entity entity;
        [SerializeField]
        private float delayInSec;

        public void AttachedToEntity(Entity entity)
        {
            this.inputChanged = false;
            this.entity = entity;
            InputField component = base.GetComponent<InputField>();
            if (component != null)
            {
                component.onValueChanged.AddListener(new UnityAction<string>(this.OnInputValueChange));
            }
            else
            {
                TMP_InputField field2 = base.GetComponent<TMP_InputField>();
                if (field2 != null)
                {
                    field2.onValueChanged.AddListener(new UnityAction<string>(this.OnInputValueChange));
                    field2.onSelect.AddListener(new UnityAction<string>(this.OnInputValueChange));
                }
            }
        }

        private void OnInputValueChange(string arg0)
        {
            this.inputChanged = true;
            this.lastChangeTime = UnityTime.time;
        }

        public void Update()
        {
            if (this.inputChanged && ((UnityTime.time - this.lastChangeTime) > this.delayInSec))
            {
                base.ScheduleEvent<InputPausedEvent>(this.entity);
                this.inputChanged = false;
            }
        }

        [Inject]
        public static Platform.Library.ClientUnityIntegration.API.UnityTime UnityTime { get; set; }
    }
}

