namespace Tanks.Lobby.ClientMatchMaking.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using TMPro;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    [RequireComponent(typeof(Button)), RequireComponent(typeof(Animator))]
    public class GameModeSelectButtonComponent : BehaviourComponent, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
    {
        [SerializeField]
        private TextMeshProUGUI modeTitle;
        [SerializeField]
        private TextMeshProUGUI modeDescription;
        [SerializeField]
        private GameObject blockLayer;
        [SerializeField]
        private GameObject restriction;
        [SerializeField]
        private ImageSkin modeImage;
        [SerializeField]
        private Material grayscaleMaterial;
        [SerializeField]
        private GameObject notAvailableForNotSquadLeaderLabel;
        [SerializeField]
        private GameObject notAvailableInSquadLabel;
        private bool pointerInside;

        private void OnEnable()
        {
            this.SetAvailableForSquadMode(false, false, false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            this.pointerInside = true;
            if (!TutorialCanvas.Instance.IsShow || base.GetComponent<Button>().interactable)
            {
                base.GetComponent<Animator>().SetTrigger("ShowDescription");
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this.pointerInside = false;
            if (!TutorialCanvas.Instance.IsShow || base.GetComponent<Button>().interactable)
            {
                base.GetComponent<Animator>().SetTrigger("HideDescription");
            }
        }

        public void SetAvailableForSquadMode(bool userInSquadNow, bool userIsSquadLeader = false, bool modeIsDefault = false)
        {
            this.notAvailableInSquadLabel.gameObject.SetActive(false);
            this.notAvailableForNotSquadLeaderLabel.gameObject.SetActive(false);
            if (!this.Restricted && userInSquadNow)
            {
                if (userIsSquadLeader && modeIsDefault)
                {
                    this.notAvailableInSquadLabel.gameObject.SetActive(true);
                }
                else if (!userIsSquadLeader)
                {
                    this.notAvailableForNotSquadLeaderLabel.gameObject.SetActive(true);
                }
            }
        }

        public void SetImage(string spriteUid)
        {
            this.modeImage.SpriteUid = spriteUid;
            this.modeImage.enabled = true;
        }

        public void SetInactive()
        {
            this.Restricted = true;
            this.SetAvailableForSquadMode(true, false, false);
            this.blockLayer.gameObject.SetActive(true);
            base.GetComponent<Button>().interactable = false;
            this.modeImage.gameObject.GetComponent<Image>().material = this.grayscaleMaterial;
        }

        public void SetRestricted(bool restricted)
        {
            this.Restricted = restricted;
            this.restriction.gameObject.SetActive(restricted);
            this.blockLayer.gameObject.SetActive(restricted);
            CheckForTutorialEvent eventInstance = new CheckForTutorialEvent();
            base.ScheduleEvent(eventInstance, new EntityStub());
            this.SetAvailableForSquadMode(false, false, false);
            if (!restricted && eventInstance.TutorialIsActive)
            {
                base.GetComponent<Button>().interactable = false;
            }
            else
            {
                base.GetComponent<Button>().interactable = !restricted;
            }
        }

        public string GameModeTitle
        {
            get => 
                this.modeTitle.text;
            set => 
                this.modeTitle.text = value;
        }

        public string ModeDescription
        {
            get => 
                this.modeDescription.text;
            set => 
                this.modeDescription.text = value;
        }

        public bool Restricted { get; private set; }
    }
}

