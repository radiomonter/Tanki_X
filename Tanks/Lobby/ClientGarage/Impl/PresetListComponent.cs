namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class PresetListComponent : UIBehaviour, Component, IScrollHandler, IEventSystemHandler
    {
        [SerializeField]
        private GameObject elementPrefab;
        [SerializeField]
        private RectTransform contentRoot;
        [SerializeField]
        private Button leftButton;
        [SerializeField]
        private Button rightButton;
        [SerializeField]
        private float speed = 100f;
        [SerializeField]
        private GameObject buyButton;
        [SerializeField]
        private GaragePrice xBuyPrice;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private LocalizedField lockedByRankLocalizedText;
        [SerializeField]
        private TextMeshProUGUI lockedByRankText;
        private int targetElement;
        private float targetPos;
        private bool immediate;

        public GameObject AddElement()
        {
            GameObject obj2 = Instantiate<GameObject>(this.elementPrefab, this.contentRoot, false);
            if (this.ElementsCount == 0)
            {
                this.SendSelectedEvent(0);
            }
            return obj2;
        }

        protected override void Awake()
        {
            this.leftButton.onClick.AddListener(new UnityAction(this.ScrollLeft));
            this.rightButton.onClick.AddListener(new UnityAction(this.ScrollRight));
        }

        public void Clear()
        {
            this.contentRoot.DestroyChildren();
        }

        private RectTransform GetChild(int index) => 
            (RectTransform) this.contentRoot.transform.GetChild(index);

        private float GetElementPos(int index) => 
            (this.ElementsCount != 0) ? this.GetChild(index).anchoredPosition.x : 0f;

        private int GetNextElement(int delta)
        {
            int num = this.targetElement + delta;
            if (num >= this.ElementsCount)
            {
                num = this.ElementsCount - 1;
            }
            if (num < 0)
            {
                num = 0;
            }
            return num;
        }

        private float GetPos() => 
            -this.contentRoot.anchoredPosition.x;

        private unsafe void Move(float deltaPos)
        {
            Vector2 anchoredPosition = this.contentRoot.anchoredPosition;
            Vector2* vectorPtr1 = &anchoredPosition;
            vectorPtr1->x -= deltaPos;
            this.contentRoot.anchoredPosition = anchoredPosition;
        }

        public void OnScroll(PointerEventData eventData)
        {
            if (eventData.scrollDelta.y < 0f)
            {
                this.ScrollRight();
            }
            else if (eventData.scrollDelta.y > 0f)
            {
                this.ScrollLeft();
            }
        }

        public void RemoveElement(int index)
        {
            if (index != this.targetElement)
            {
                if ((index < this.targetElement) && (this.targetElement > 0))
                {
                    this.targetElement--;
                }
            }
            else if (index > 0)
            {
                this.Scroll(-1, true);
            }
            else
            {
                this.Scroll(1, true);
            }
            Destroy(this.GetChild(index).gameObject);
        }

        public void Scroll(int delta, bool immediate)
        {
            int nextElement = this.GetNextElement(delta);
            this.ScrollToElement(nextElement, immediate, true);
        }

        public void ScrollLeft()
        {
            this.Scroll(-1, false);
        }

        public void ScrollRight()
        {
            this.Scroll(1, false);
        }

        public void ScrollToElement(int elementIndex, bool immediate, bool sendSelected)
        {
            this.targetElement = elementIndex;
            this.targetPos = this.GetElementPos(this.targetElement);
            this.immediate = immediate;
            if (immediate)
            {
                float deltaPos = this.targetPos - this.GetPos();
                this.Move(deltaPos);
            }
            if (sendSelected)
            {
                this.SendSelectedEvent(this.targetElement);
            }
        }

        private void SendSelectedEvent(int elementIndex)
        {
            EntityBehaviour component = this.GetChild(elementIndex).GetComponent<EntityBehaviour>();
            if ((component != null) && (component.Entity != null))
            {
                EngineService.Engine.ScheduleEvent<ListItemSelectedEvent>(component.Entity);
            }
        }

        public void SetBuyButtonEnabled(bool enabled)
        {
            this.animator.SetBool("BuyEnabled", enabled);
        }

        public void SetLockedByRankTextEnabled(bool enabled)
        {
            this.animator.SetBool("LockedByRankTextEnabled", enabled);
        }

        private void Update()
        {
            if (this.immediate)
            {
                this.targetPos = this.GetElementPos(this.targetElement);
            }
            if (!Mathf.Approximately(this.GetPos(), this.targetPos))
            {
                float b = this.speed * Time.deltaTime;
                float deltaPos = this.targetPos - this.GetPos();
                if (this.immediate)
                {
                    this.Move(deltaPos);
                    this.immediate = false;
                }
                else
                {
                    float num3 = (deltaPos < 0f) ? -Mathf.Min(-deltaPos, b) : Mathf.Min(deltaPos, b);
                    this.Move(num3);
                }
            }
            if (Input.GetButtonDown("MoveRight"))
            {
                if (!InputFieldComponent.IsAnyInputFieldInFocus())
                {
                    this.ScrollRight();
                }
            }
            else if (Input.GetButtonDown("MoveLeft") && !InputFieldComponent.IsAnyInputFieldInFocus())
            {
                this.ScrollLeft();
            }
            if (TutorialCanvas.Instance.IsShow)
            {
                if (this.leftButton.gameObject.activeSelf)
                {
                    this.leftButton.gameObject.SetActive(false);
                }
                if (this.rightButton.gameObject.activeSelf)
                {
                    this.rightButton.gameObject.SetActive(false);
                }
            }
            else
            {
                float num4 = 1E-06f;
                bool flag = (this.ElementsCount > 1) && (this.GetPos() > (this.GetElementPos(1) - num4));
                if (flag != this.leftButton.gameObject.activeSelf)
                {
                    this.leftButton.gameObject.SetActive(flag);
                }
                bool flag2 = (this.ElementsCount > 1) && (this.GetPos() < (this.GetElementPos(this.ElementsCount - 2) + num4));
                if (flag2 != this.rightButton.gameObject.activeSelf)
                {
                    this.rightButton.gameObject.SetActive(flag2);
                }
            }
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineService EngineService { get; set; }

        public GameObject BuyButton =>
            this.buyButton;

        public GaragePrice XBuyPrice =>
            this.xBuyPrice;

        public BuyConfirmationDialog BuyDialog =>
            FindObjectOfType<Dialogs60Component>().Get<BuyConfirmationDialog>();

        public LocalizedField LockedByRankLocalizedText =>
            this.lockedByRankLocalizedText;

        public string LockedByRankText
        {
            set => 
                this.lockedByRankText.text = value;
        }

        public RectTransform ContentRoot =>
            this.contentRoot;

        public int ElementsCount =>
            this.contentRoot.transform.childCount;

        public int SelectedItemIndex =>
            this.targetElement;
    }
}

