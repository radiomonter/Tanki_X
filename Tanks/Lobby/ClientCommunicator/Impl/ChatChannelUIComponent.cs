namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class ChatChannelUIComponent : BehaviourComponent, AttachToEntityListener
    {
        private Entity entity;
        [SerializeField]
        private GameObject tab;
        [SerializeField]
        private Color selectedColor;
        [SerializeField]
        private Color unselectedColor;
        [SerializeField]
        private Image back;
        [SerializeField]
        private ImageSkin icon;
        [SerializeField]
        private TMP_Text name;
        [SerializeField]
        private GameObject badge;
        [SerializeField]
        private TMP_Text counterText;
        private int counter;

        public void AttachedToEntity(Entity entity)
        {
            this.entity = entity;
        }

        public void Deselect()
        {
            this.back.color = this.unselectedColor;
        }

        public string GetSpriteUid() => 
            this.icon.SpriteUid;

        public void OnClick()
        {
            if (this.entity != null)
            {
                ECSBehaviour.EngineService.Engine.ScheduleEvent(new SelectChannelEvent(), this.entity);
            }
        }

        public void Select()
        {
            this.back.color = this.selectedColor;
        }

        public void SetIcon(string spriteUid)
        {
            this.icon.SpriteUid = spriteUid;
        }

        public GameObject Tab
        {
            get => 
                this.tab;
            set => 
                this.tab = value;
        }

        public string Name
        {
            get => 
                this.name.text;
            set => 
                this.name.text = value;
        }

        public int Unread
        {
            get => 
                this.counter;
            set
            {
                this.counter = value;
                this.badge.SetActive(this.counter > 0);
                this.counterText.text = this.counter.ToString();
            }
        }
    }
}

