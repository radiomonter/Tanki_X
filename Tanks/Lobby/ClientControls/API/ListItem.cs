namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class ListItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
    {
        public const string DISABLE_MESSAGE = "OnItemDisabled";
        public const string ENABLE_MESSAGE = "OnItemEnabled";
        private static int SELECTED_STATE = UnityEngine.Animator.StringToHash("Selected");
        private static int ENABLED_STATE = UnityEngine.Animator.StringToHash("Enabled");
        private bool pointerOver;
        [SerializeField]
        private RectTransform content;
        private ListItemContent cachedContent;
        private object data;
        private UnityEngine.Animator animator;

        public RectTransform GetContent() => 
            (this.content.childCount != 1) ? null : ((RectTransform) this.content.GetChild(0));

        private void OnItemDisabled()
        {
            this.SetBool(ENABLED_STATE, false);
        }

        private void OnItemEnabled()
        {
            this.SetBool(ENABLED_STATE, true);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.clickCount > 1)
            {
                base.SendMessageUpwards("OnDoubleClick", this, SendMessageOptions.DontRequireReceiver);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            this.pointerOver = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this.pointerOver = false;
        }

        public void PlayDeselectionAnimation()
        {
            this.SetBool(SELECTED_STATE, false);
        }

        public void PlaySelectionAnimation()
        {
            this.SetBool(SELECTED_STATE, true);
        }

        public void Select(bool sendMessage = true)
        {
            if (sendMessage)
            {
                base.SendMessageUpwards("OnItemSelect", this, SendMessageOptions.DontRequireReceiver);
            }
            this.PlaySelectionAnimation();
            if (this.cachedContent != null)
            {
                this.cachedContent.Select();
            }
        }

        private void SetBool(int state, bool value)
        {
            if (base.gameObject.activeInHierarchy && this.Animator.isActiveAndEnabled)
            {
                this.Animator.SetBool(state, value);
            }
        }

        public void SetContent(RectTransform content)
        {
            content.SetParent(this.content, false);
            content.gameObject.SetActive(false);
            content.gameObject.SetActive(true);
            this.cachedContent = content.GetComponent<ListItemContent>();
        }

        private void Update()
        {
            if (this.pointerOver)
            {
                base.SendMessageUpwards("PointerOverContentItem", this, SendMessageOptions.DontRequireReceiver);
            }
        }

        public object Data
        {
            get => 
                this.data;
            set
            {
                this.data = value;
                if (this.cachedContent != null)
                {
                    this.cachedContent.SetDataProvider(this.data);
                }
            }
        }

        private UnityEngine.Animator Animator
        {
            get
            {
                if (this.animator == null)
                {
                    this.animator = base.GetComponent<UnityEngine.Animator>();
                }
                return this.animator;
            }
        }
    }
}

