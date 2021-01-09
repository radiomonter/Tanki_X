namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class LeagueCarouselUIComponent : BehaviourComponent
    {
        public CarouselItemSelected itemSelected;
        [SerializeField]
        private LeagueTitleUIComponent leagueTitlePrefab;
        [SerializeField]
        private RectTransform scrollContent;
        [SerializeField]
        private Button leftScrollButton;
        [SerializeField]
        private Button rightScrollButton;
        [SerializeField]
        private float autoScrollSpeed = 1f;
        [SerializeField]
        private float pageWidth = 400f;
        [SerializeField]
        private float pagesOffset = 20f;
        [SerializeField]
        private int pageCount;
        [SerializeField]
        private int currentPage = 1;
        [SerializeField]
        private bool interactWithScrollView;
        [SerializeField]
        private LocalizedField leagueLocalizedField;

        public LeagueTitleUIComponent AddLeagueItem(Entity entity) => 
            this.GetNewLeagueTitleLayout(entity);

        private void Clear()
        {
            this.scrollContent.DestroyChildren();
        }

        private LeagueTitleUIComponent GetNewLeagueTitleLayout(Entity entity)
        {
            LeagueTitleUIComponent component = Instantiate<LeagueTitleUIComponent>(this.leagueTitlePrefab);
            component.transform.SetParent(this.scrollContent, false);
            component.gameObject.SetActive(true);
            component.Name = entity.GetComponent<LeagueNameComponent>().Name + " " + this.leagueLocalizedField.Value;
            component.Icon = entity.GetComponent<LeagueIconComponent>().SpriteUid;
            return component;
        }

        private void OnDisable()
        {
            this.Clear();
        }

        private void ScrollLeft()
        {
            this.SelectPage(Mathf.Max(1, this.currentPage - 1));
        }

        private void ScrollRight()
        {
            this.SelectPage(Mathf.Min(this.pageCount, this.currentPage + 1));
        }

        public void SelectItem(Entity entity)
        {
            LeagueTitleUIComponent[] componentsInChildren = base.GetComponentsInChildren<LeagueTitleUIComponent>();
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                if (componentsInChildren[i].LeagueEntity.Equals(entity))
                {
                    this.SelectPage(i + 1);
                    return;
                }
            }
            this.SelectPage(1);
        }

        private void SelectPage(int page)
        {
            this.currentPage = page;
            this.interactWithScrollView = false;
            if (this.itemSelected != null)
            {
                this.itemSelected(this.CurrentLeague);
            }
            this.SetButtons();
        }

        private void SetButtons()
        {
            this.leftScrollButton.gameObject.SetActive(this.currentPage != 1);
            this.rightScrollButton.gameObject.SetActive(this.currentPage != base.GetComponentsInChildren<LeagueTitleUIComponent>().Length);
        }

        private void Start()
        {
            this.leftScrollButton.onClick.AddListener(new UnityAction(this.ScrollLeft));
            this.rightScrollButton.onClick.AddListener(new UnityAction(this.ScrollRight));
        }

        private void Update()
        {
            if (!this.interactWithScrollView)
            {
                this.pageCount = this.scrollContent.childCount;
                Vector2 b = new Vector2((-(this.currentPage - 1) * this.pageWidth) - this.pagesOffset, this.scrollContent.anchoredPosition.y);
                this.scrollContent.anchoredPosition = Vector2.Lerp(this.scrollContent.anchoredPosition, b, this.autoScrollSpeed * Time.deltaTime);
            }
        }

        public LeagueTitleUIComponent CurrentLeague =>
            base.GetComponentsInChildren<LeagueTitleUIComponent>()[this.currentPage - 1];
    }
}

