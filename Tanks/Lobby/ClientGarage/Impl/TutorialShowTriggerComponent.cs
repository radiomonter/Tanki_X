namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientGarage.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class TutorialShowTriggerComponent : BehaviourComponent
    {
        [SerializeField]
        protected long tutorialId;
        [SerializeField]
        protected long stepId;
        public EventTriggerType triggerType;
        public TutorialType tutorialType;
        public GameObject[] highlightedRects;
        [HideInInspector]
        public RectTransform popupPositionRect;
        [HideInInspector]
        public bool useOverlay = true;
        [HideInInspector]
        public bool showArrow;
        [HideInInspector]
        public RectTransform arrowPositionRect;
        [HideInInspector]
        public Button selectable;
        [HideInInspector]
        public float cameraOffset;
        [HideInInspector]
        public float showDelay;
        [HideInInspector]
        public float triggerDelay;
        [HideInInspector]
        public Button triggerButton;
        [HideInInspector]
        public TutorialHideTriggerComponent tutorialHideTrigger;
        [HideInInspector]
        public TutorialStepHandler stepCustomHandler;
        [HideInInspector]
        public GameObject outlinePrefab;
        [HideInInspector]
        public bool useImageInPopup;
        [HideInInspector]
        public string imageUid;
        [HideInInspector]
        public bool inBattleMode;
        public string ignorableDialogName;
        private bool isShow;

        private void DelayedTrigger()
        {
            if (!this.isShow && (base.GetComponent<EntityBehaviour>().Entity != null))
            {
                base.ScheduleEvent<TutorialTriggerEvent>(base.GetComponent<EntityBehaviour>().Entity);
            }
        }

        public void DestroyTrigger()
        {
            base.gameObject.SetActive(false);
        }

        public void SetSeleectable(Button selectable)
        {
            this.selectable = selectable;
        }

        public void Show(Entity tutorialStep, int currentStepNumber, int stepCountInTutorial)
        {
            if (!this.isShow)
            {
                this.isShow = true;
                string message = tutorialStep.GetComponent<TutorialStepDataComponent>().Message;
                TutorialHighlightTankStepDataComponent component = !tutorialStep.HasComponent<TutorialHighlightTankStepDataComponent>() ? new TutorialHighlightTankStepDataComponent() : tutorialStep.GetComponent<TutorialHighlightTankStepDataComponent>();
                TutorialData data = new TutorialData {
                    Type = this.tutorialType,
                    TutorialStep = tutorialStep,
                    Message = message,
                    PopupPositionRect = this.popupPositionRect,
                    ShowDelay = this.showDelay,
                    ImageUid = this.imageUid,
                    InteractableButton = this.selectable,
                    OutlinePrefab = this.outlinePrefab,
                    CameraOffset = this.cameraOffset,
                    HighlightHull = component.HighlightHull,
                    HighlightWeapon = component.HighlightWeapon,
                    CurrentStepNumber = currentStepNumber,
                    StepCountInTutorial = stepCountInTutorial,
                    InBattleMode = this.inBattleMode
                };
                switch (this.tutorialType)
                {
                    case TutorialType.Default:
                        data.ContinueOnClick = true;
                        this.ShowMaskedPopupStep(data);
                        break;

                    case TutorialType.Interact:
                        this.ShowInteractStep(data);
                        break;

                    case TutorialType.HighlightTankPart:
                        data.ContinueOnClick = true;
                        this.ShowHighlightTankStep(data);
                        break;

                    case TutorialType.CustomHandler:
                        this.stepCustomHandler.RunStep(data);
                        break;

                    default:
                        break;
                }
                if (this.tutorialHideTrigger != null)
                {
                    this.tutorialHideTrigger.Activate(tutorialStep);
                }
            }
        }

        public bool ShowAllow()
        {
            ITutorialShowStepValidator component = base.GetComponent<ITutorialShowStepValidator>();
            return ((component == null) || component.ShowAllowed(this.stepId));
        }

        private void ShowHighlightTankStep(TutorialData data)
        {
            TutorialCanvas.Instance.Show(data, this.useOverlay, this.highlightedRects, null);
        }

        private void ShowInteractStep(TutorialData data)
        {
            TutorialCanvas.Instance.Show(data, this.useOverlay, this.highlightedRects, this.arrowPositionRect);
        }

        private void ShowMaskedPopupStep(TutorialData data)
        {
            TutorialCanvas.Instance.Show(data, this.useOverlay, this.highlightedRects, this.arrowPositionRect);
        }

        public void Triggered()
        {
            if (this.ShowAllow() && !this.isShow)
            {
                if (this.triggerDelay == 0f)
                {
                    this.DelayedTrigger();
                }
                else
                {
                    base.Invoke("DelayedTrigger", this.triggerDelay);
                }
            }
        }

        private void Update()
        {
            if (((this.triggerType == EventTriggerType.ClickAnyWhere) || (this.triggerType == EventTriggerType.ClickAnyWhereOrDelay)) && Input.GetMouseButtonDown(0))
            {
                base.CancelInvoke();
                this.triggerDelay = 0f;
                this.Triggered();
            }
        }

        public long TutorialId =>
            this.tutorialId;

        public long StepId =>
            this.stepId;

        public enum EventTriggerType
        {
            Awake,
            ClickAnyWhere,
            ClickAnyWhereOrDelay,
            CustomTrigger
        }
    }
}

