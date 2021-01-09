namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class InteractionTooltipContent : BehaviourComponent, ITooltipContent
    {
        [SerializeField]
        private GameObject responceMessagePrefab;
        private RectTransform rect;

        protected virtual void Awake()
        {
            this.rect = base.GetComponent<RectTransform>();
        }

        public void Hide()
        {
            TooltipController.Instance.HideTooltip();
        }

        public virtual void Init(object data)
        {
        }

        private bool PointerOutsideMenu() => 
            !RectTransformUtility.RectangleContainsScreenPoint(this.rect, Input.mousePosition);

        protected void ShowResponse(string respondText)
        {
            GameObject obj2 = Instantiate<GameObject>(this.responceMessagePrefab);
            obj2.transform.SetParent(base.transform.parent.parent, false);
            obj2.GetComponent<RectTransform>().position = Input.mousePosition;
            obj2.GetComponentInChildren<TextMeshProUGUI>().text = respondText;
            obj2.SetActive(true);
            Destroy(obj2, obj2.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length);
        }

        public void Update()
        {
            bool keyUp = Input.GetKeyUp(KeyCode.Escape);
            if (((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && this.PointerOutsideMenu()) || keyUp)
            {
                this.Hide();
            }
        }
    }
}

