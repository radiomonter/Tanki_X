namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientUserProfile.Impl;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class TutorialDialog : ECSBehaviour
    {
        private TutorialData tutorialData;
        [SerializeField]
        protected AnimatedText animatedText;
        [SerializeField]
        protected Image backgroundImage;
        [SerializeField]
        protected Button continueButton;
        [SerializeField]
        protected ImageSkin image;
        [SerializeField]
        private TextMeshProUGUI tutorialProgressLabel;
        [SerializeField]
        private GameObject characterBig;
        [SerializeField]
        private GameObject characterSmall;
        private Entity tutorialStep;
        private RectTransform popupPositionRect;
        private TutorialPopupContinue popupContinue;
        [HideInInspector]
        public bool continueOnClick;
        [SerializeField]
        private Material blurMaterial;
        private CanvasGroup canvasGroup;
        [SerializeField]
        private LayerMask highlightLayer;
        private float oldCameraOffset;
        private float newsContainerAlpha;
        private bool highlightHull;
        private bool highlightWeapon;
        private float cameraOffset;
        private Camera highlightCamera;
        private GameObject[] outlines;
        private GameObject outlinePrefab;
        private float showTimer;
        private float minShowTime = 1f;

        private GameObject[] CreateOutlines(GameObject[] tankParts)
        {
            List<GameObject> list = new List<GameObject>();
            for (int i = 0; i < tankParts.Length; i++)
            {
                GameObject obj2 = tankParts[i];
                if (obj2 != null)
                {
                    GameObject item = (this.outlinePrefab != null) ? Instantiate<GameObject>(this.outlinePrefab) : new GameObject("Outline");
                    if (i != 0)
                    {
                    }
                    list.Add(item);
                    item.layer = obj2.layer;
                    item.transform.SetParent(obj2.transform, false);
                    item.transform.localScale = Vector3.one;
                    item.transform.localRotation = Quaternion.identity;
                    item.transform.localPosition = Vector3.zero;
                    if (i != 0)
                    {
                        item.transform.SetParent(list[i - 1].transform, true);
                    }
                    item.SendMessage("Init", obj2);
                }
            }
            foreach (GameObject obj4 in list)
            {
                if (obj4 != null)
                {
                    obj4.SetActive(false);
                }
            }
            return list.ToArray();
        }

        private void HighlightingContinue()
        {
            if (this.tutorialData.Type == TutorialType.HighlightTankPart)
            {
                base.CancelInvoke();
                CameraOffsetBehaviour behaviour = FindObjectOfType<CameraOffsetBehaviour>();
                if (behaviour != null)
                {
                    behaviour.AnimateOffset(this.oldCameraOffset);
                }
                NewsContainerComponent component = FindObjectOfType<NewsContainerComponent>();
                if (component != null)
                {
                    component.GetComponent<CanvasGroup>().alpha = this.newsContainerAlpha;
                }
                foreach (GameObject obj2 in this.outlines)
                {
                    if (obj2 != null)
                    {
                        obj2.SendMessage("Disable");
                    }
                }
            }
        }

        public void HighlightTank()
        {
            if (this.tutorialData.Type == TutorialType.HighlightTankPart)
            {
                string tag = "TankHull";
                string str2 = "TankWeapon";
                CameraOffsetBehaviour behaviour = FindObjectOfType<CameraOffsetBehaviour>();
                if (behaviour != null)
                {
                    this.oldCameraOffset = behaviour.Offset;
                    behaviour.AnimateOffset(this.cameraOffset);
                    Camera outlineCamera = TutorialCanvas.Instance.OutlineCamera;
                    this.highlightCamera = Instantiate<Camera>(outlineCamera);
                    this.highlightCamera.transform.SetParent(behaviour.transform, false);
                    this.highlightCamera.transform.localPosition = Vector3.zero;
                    this.highlightCamera.transform.localEulerAngles = Vector3.zero;
                    this.highlightCamera.depth = outlineCamera.depth + 1f;
                    this.highlightCamera.cullingMask = (int) this.highlightLayer;
                    this.highlightCamera.gameObject.SetActive(true);
                    List<GameObject> list = new List<GameObject>();
                    if (this.highlightHull)
                    {
                        list.Add(GameObject.FindGameObjectWithTag(tag));
                    }
                    if (this.highlightWeapon)
                    {
                        list.Add(GameObject.FindGameObjectWithTag(str2));
                    }
                    this.outlines = this.CreateOutlines(list.ToArray());
                }
                NewsContainerComponent component = FindObjectOfType<NewsContainerComponent>();
                if (component != null)
                {
                    this.newsContainerAlpha = component.GetComponent<CanvasGroup>().alpha;
                    component.GetComponent<CanvasGroup>().alpha = 0f;
                }
                base.Invoke("StartOutlineAnimation", 0.6f);
            }
        }

        public void Init(TutorialData data)
        {
            this.tutorialData = data;
            this.tutorialStep = this.tutorialData.TutorialStep;
            this.animatedText.ResultText = this.tutorialData.Message;
            this.popupPositionRect = this.tutorialData.PopupPositionRect;
            this.InBattleMode = data.InBattleMode;
            if (data.StepCountInTutorial <= 1)
            {
                this.tutorialProgressLabel.gameObject.SetActive(false);
            }
            else
            {
                this.tutorialProgressLabel.gameObject.SetActive(true);
                this.tutorialProgressLabel.text = $"{data.CurrentStepNumber}/{data.StepCountInTutorial}";
            }
            this.continueOnClick = data.ContinueOnClick;
            this.SetupAdditionalImage(this.tutorialData);
            this.InitTankHighlighting(this.tutorialData);
            if (!this.continueOnClick)
            {
                this.continueButton.gameObject.SetActive(false);
            }
            else
            {
                this.continueButton.gameObject.SetActive(true);
                this.continueButton.interactable = true;
                TutorialCanvas.Instance.AddAllowSelectable(this.continueButton);
            }
        }

        private void InitTankHighlighting(TutorialData data)
        {
            if (data.Type == TutorialType.HighlightTankPart)
            {
                this.highlightHull = data.HighlightHull;
                this.highlightWeapon = data.HighlightWeapon;
                this.cameraOffset = data.CameraOffset;
                this.outlinePrefab = data.OutlinePrefab;
            }
        }

        private void OnContinue()
        {
            this.HighlightingContinue();
            if (this.tutorialData.InteractableButton != null)
            {
                this.tutorialData.InteractableButton.onClick.RemoveListener(new UnityAction(this.OnInteractableButtonClick));
            }
            if (this.popupContinue != null)
            {
                this.popupContinue();
                this.popupContinue = null;
            }
        }

        protected void OnDisable()
        {
            this.popupContinue = null;
        }

        private void OnInteractableButtonClick()
        {
            this.OnContinue();
        }

        public void OverrideData(TutorialData data)
        {
            this.tutorialStep = data.TutorialStep;
            this.continueOnClick = data.ContinueOnClick;
        }

        private void SetupAdditionalImage(TutorialData data)
        {
            if (string.IsNullOrEmpty(data.ImageUid))
            {
                this.image.gameObject.SetActive(false);
            }
            else
            {
                this.image.gameObject.SetActive(true);
                this.image.SpriteUid = data.ImageUid;
            }
        }

        private void SetupInteractableButton(TutorialData data)
        {
            if (data.InteractableButton != null)
            {
                data.InteractableButton.onClick.AddListener(new UnityAction(this.OnInteractableButtonClick));
                TutorialCanvas.Instance.AddAllowSelectable(data.InteractableButton);
                data.InteractableButton.interactable = true;
            }
        }

        public void Show()
        {
            this.animatedText.Animate();
            this.showTimer = 0f;
            base.gameObject.SetActive(true);
            this.SetupInteractableButton(this.tutorialData);
            this.HighlightTank();
        }

        private void Start()
        {
            if (this.continueButton != null)
            {
                this.continueButton.onClick.AddListener(new UnityAction(this.OnContinue));
            }
            this.blurMaterial = new Material(this.blurMaterial);
            this.canvasGroup = base.GetComponent<CanvasGroup>();
        }

        private void StartOutlineAnimation()
        {
            Debug.Log("Start outline animation");
            foreach (GameObject obj2 in this.outlines)
            {
                if (obj2 != null)
                {
                    obj2.SetActive(true);
                    obj2.GetComponent<Animator>().SetBool("visible", true);
                }
            }
        }

        public void TutorialHidden()
        {
            if (this.highlightCamera != null)
            {
                Destroy(this.highlightCamera.gameObject);
            }
            if (this.outlines != null)
            {
                GameObject[] outlines = this.outlines;
                int index = 0;
                while (true)
                {
                    if (index >= outlines.Length)
                    {
                        this.outlines = null;
                        break;
                    }
                    GameObject obj2 = outlines[index];
                    if (obj2 != null)
                    {
                        Destroy(obj2);
                    }
                    index++;
                }
            }
            Entity tutorialStep = this.tutorialStep;
            this.tutorialStep = null;
            if (tutorialStep != null)
            {
                base.ScheduleEvent<TutorialStepCompleteEvent>(tutorialStep);
            }
        }

        private void Update()
        {
            this.showTimer += Time.deltaTime;
            if (Input.GetMouseButtonUp(0))
            {
                if (this.animatedText.TextAnimation)
                {
                    this.animatedText.ForceComplete();
                }
                else if (this.continueOnClick && (this.showTimer > this.minShowTime))
                {
                    this.OnContinue();
                }
            }
            else
            {
                if (InputMapping.Cancel && (Environment.CommandLine.Contains("completeTutorialByEsc") && (this.tutorialStep != null)))
                {
                    base.ScheduleEvent<CompleteTutorialByEscEvent>(this.tutorialStep);
                    TutorialCanvas.Instance.Hide();
                }
                RectTransform component = base.GetComponent<RectTransform>();
                if (this.popupPositionRect == null)
                {
                    component.position = (Vector3) new Vector2(2000f, 2000f);
                }
                else
                {
                    component.pivot = this.popupPositionRect.pivot;
                    component.position = this.popupPositionRect.position;
                }
                this.blurMaterial.SetColor("_TintColor", new Color(0f, 0f, 0f, this.canvasGroup.alpha * 0.5f));
                this.blurMaterial.SetFloat("_Size", this.canvasGroup.alpha * 7f);
            }
        }

        public bool InBattleMode
        {
            set
            {
                this.characterBig.SetActive(!value);
                this.characterSmall.SetActive(value);
                base.GetComponent<HorizontalLayoutGroup>().childAlignment = !value ? TextAnchor.MiddleLeft : TextAnchor.UpperLeft;
            }
        }

        public TutorialPopupContinue PopupContinue
        {
            get => 
                this.popupContinue;
            set => 
                this.popupContinue = value;
        }
    }
}

