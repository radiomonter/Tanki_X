namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class TutorialCanvas : MonoBehaviour
    {
        public static TutorialCanvas Instance;
        [SerializeField]
        public GameObject overlay;
        [SerializeField]
        private TutorialIntroDialog introDialog;
        [SerializeField]
        public TutorialDialog dialog;
        [SerializeField]
        public TutorialArrow arrow;
        [SerializeField]
        private GameObject tutorialScreen;
        [SerializeField]
        private GameObject skipTutorialButton;
        private List<Selectable> disabledSelectables = new List<Selectable>();
        private List<Selectable> allowSelectables = new List<Selectable>();
        private List<GameObject> additionalMaskRects = new List<GameObject>();
        private bool isShow;
        private bool isPaused;
        private bool allowCancelHandler = true;
        private List<GameObject> maskedObjects = new List<GameObject>();
        [SerializeField]
        private CanvasGroup backgroundCanvasGroup;
        private Material backgroundMaterial;
        [SerializeField]
        private Canvas overlayCanvas;
        [SerializeField]
        private Camera outlineCamera;
        [SerializeField]
        private Camera tutorialCamera;

        public void AddAdditionalMaskRect(GameObject maskRect)
        {
            this.additionalMaskRects.Add(maskRect);
        }

        public void AddAllowSelectable(Selectable selectable)
        {
            if (!this.allowSelectables.Contains(selectable))
            {
                this.allowSelectables.Add(selectable);
            }
        }

        private void Awake()
        {
            Image component = this.backgroundCanvasGroup.GetComponent<Image>();
            this.backgroundMaterial = new Material(component.material);
            component.material = this.backgroundMaterial;
        }

        public void BlockInteractable()
        {
            foreach (Selectable selectable in FindObjectsOfType<Selectable>())
            {
                if (selectable.interactable && (!this.allowSelectables.Contains(selectable) && (selectable.gameObject != this.skipTutorialButton)))
                {
                    this.disabledSelectables.Add(selectable);
                    selectable.interactable = false;
                }
            }
        }

        public void CardsNotificationsEnabled(bool value)
        {
            base.GetComponent<Animator>().SetBool("showOverlay", !value);
        }

        private void CheckForOverlayCamera()
        {
            if (this.overlayCanvas.worldCamera == null)
            {
                this.overlayCanvas.worldCamera = GameObject.Find("UICamera").GetComponent<Camera>();
                this.overlayCanvas.planeDistance = 10f;
            }
        }

        private void ClearMasks()
        {
            foreach (GameObject obj2 in this.maskedObjects)
            {
                if (obj2 != null)
                {
                    GraphicRaycaster component = obj2.GetComponent<GraphicRaycaster>();
                    if (component != null)
                    {
                        Destroy(component);
                    }
                    Canvas canvas = obj2.GetComponent<Canvas>();
                    if (canvas != null)
                    {
                        Destroy(canvas);
                    }
                }
            }
            Canvas.ForceUpdateCanvases();
            this.maskedObjects.Clear();
        }

        public void Continue()
        {
            if (this.isPaused)
            {
                Debug.Log("Continue");
                this.ToggleMask(true);
                base.GetComponent<Animator>().SetBool("pause", false);
                this.overlayCanvas.GetComponent<Animator>().SetBool("pause", false);
            }
        }

        private void CreateMask(GameObject rect)
        {
            if (rect.GetComponent<Canvas>() == null)
            {
                Canvas canvas = rect.gameObject.AddComponent<Canvas>();
                canvas.overrideSorting = true;
                canvas.sortingLayerName = "Overlay";
                canvas.sortingOrder = 30;
                rect.gameObject.AddComponent<GraphicRaycaster>();
                this.maskedObjects.Add(rect.gameObject);
            }
        }

        private void CreateMasks(GameObject[] rects)
        {
            this.ClearMasks();
            if (rects != null)
            {
                foreach (GameObject obj2 in rects)
                {
                    this.CreateMask(obj2);
                }
            }
            foreach (GameObject obj3 in this.additionalMaskRects)
            {
                this.CreateMask(obj3);
            }
            this.additionalMaskRects.Clear();
        }

        private void DelayedShow()
        {
            MainScreenComponent.Instance.ToggleNews(false);
            this.BlockInteractable();
            base.CancelInvoke();
            base.GetComponent<Animator>().SetBool("show", true);
            this.overlayCanvas.GetComponent<Animator>().SetBool("show", true);
            this.dialog.PopupContinue += new TutorialPopupContinue(this.Hide);
            this.dialog.Show();
            this.isShow = true;
            this.tutorialScreen.SetActive(true);
        }

        private void Hidden()
        {
            if (this.isShow)
            {
                this.allowCancelHandler = true;
                this.isShow = false;
                this.UnblockInteractable();
                this.ClearMasks();
                this.tutorialCamera.gameObject.SetActive(false);
                this.allowSelectables.Clear();
                this.arrow.gameObject.SetActive(false);
                this.dialog.gameObject.SetActive(false);
                this.tutorialScreen.SetActive(false);
                this.skipTutorialButton.SetActive(false);
                MainScreenComponent.Instance.ToggleNews(true);
                this.dialog.TutorialHidden();
                this.introDialog.TutorialHidden();
            }
        }

        public void Hide()
        {
            base.CancelInvoke();
            if (base.GetComponent<Animator>().GetBool("show"))
            {
                base.GetComponent<Animator>().SetBool("show", false);
                this.overlayCanvas.GetComponent<Animator>().SetBool("show", false);
            }
            else
            {
                this.isShow = true;
                this.Hidden();
            }
        }

        public void Pause()
        {
            Debug.Log("Pause");
            this.ToggleMask(false);
            this.isPaused = true;
            base.GetComponent<Animator>().SetBool("pause", true);
            this.overlayCanvas.GetComponent<Animator>().SetBool("pause", true);
        }

        private void SetArrowPosition(RectTransform arrowPositionRect)
        {
            this.arrow.Setup(arrowPositionRect);
            this.arrow.gameObject.SetActive(true);
        }

        public void SetupActivePopup(TutorialData data)
        {
            this.dialog.OverrideData(data);
        }

        public void Show(TutorialData data, bool useOverlay, GameObject[] highlightedRects = null, RectTransform arrowPositionRect = null)
        {
            this.tutorialCamera.gameObject.SetActive(true);
            this.CheckForOverlayCamera();
            this.dialog.Init(data);
            this.allowCancelHandler = false;
            this.overlay.SetActive(useOverlay);
            this.CreateMasks(highlightedRects);
            this.BlockInteractable();
            if (arrowPositionRect != null)
            {
                this.SetArrowPosition(arrowPositionRect);
            }
            base.Invoke("DelayedShow", data.ShowDelay);
            this.tutorialScreen.SetActive(true);
        }

        public void ShowIntroDialog(TutorialData data, bool canSkipTutorial)
        {
            this.tutorialCamera.gameObject.SetActive(true);
            this.BlockInteractable();
            this.CheckForOverlayCamera();
            this.overlay.SetActive(true);
            this.introDialog.Show(data.TutorialStep, canSkipTutorial);
            base.GetComponent<Animator>().SetBool("show", true);
            this.overlayCanvas.GetComponent<Animator>().SetBool("show", true);
            this.isShow = true;
            this.tutorialScreen.SetActive(true);
            MainScreenComponent.Instance.ToggleNews(false);
        }

        public void ShowOverlay()
        {
            base.GetComponent<Animator>().SetBool("show", false);
        }

        private void Start()
        {
            Instance = this;
        }

        private void ToggleMask(bool value)
        {
            foreach (GameObject obj2 in this.maskedObjects)
            {
                if (obj2 != null)
                {
                    Canvas component = obj2.GetComponent<Canvas>();
                    if (component != null)
                    {
                        component.enabled = value;
                    }
                }
            }
        }

        public void UnblockInteractable()
        {
            foreach (Selectable selectable in this.disabledSelectables)
            {
                if (selectable != null)
                {
                    selectable.interactable = true;
                }
            }
            this.disabledSelectables.Clear();
        }

        private void Update()
        {
            this.CheckForOverlayCamera();
            this.backgroundMaterial.SetColor("_TintColor", new Color(0.078f, 0.078f, 0.078f, this.backgroundCanvasGroup.alpha * 0.8f));
            this.backgroundMaterial.SetFloat("_Size", this.backgroundCanvasGroup.alpha * 6f);
        }

        public GameObject SkipTutorialButton =>
            this.skipTutorialButton;

        public bool IsShow =>
            this.isShow;

        public bool IsPaused =>
            this.isPaused;

        public bool AllowCancelHandler =>
            this.allowCancelHandler;

        public Camera OutlineCamera =>
            this.outlineCamera;
    }
}

