namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.Impl.Tutorial;
    using UnityEngine;
    using UnityEngine.UI;

    public class SlotView : MonoBehaviour
    {
        public int moduleCard3DScale;
        public TooltipShowBehaviour tooltip;
        public Image dropInnerGlow;
        public Image dropOuterGlow;
        private DragAndDropCell dragAndDropCell;

        private void Awake()
        {
            this.dragAndDropCell = base.GetComponent<DragAndDropCell>();
            if (this.dropInnerGlow)
            {
                this.dropInnerGlow.gameObject.SetActive(false);
            }
            if (this.dropOuterGlow)
            {
                this.dropOuterGlow.gameObject.SetActive(false);
            }
        }

        public void CancelHighlightForDrop()
        {
            this.dropInnerGlow.gameObject.SetActive(false);
            this.dropOuterGlow.gameObject.SetActive(false);
            SlotItemView item = this.GetItem();
            if (item != null)
            {
                item.HighlightEnable = true;
            }
        }

        [DebuggerHidden]
        private IEnumerator DelayedTurnOnRenderAboveAll() => 
            new <DelayedTurnOnRenderAboveAll>c__Iterator0 { $this = this };

        public SlotItemView GetItem() => 
            this.dragAndDropCell.GetItem()?.GetComponent<SlotItemView>();

        public bool HasItem() => 
            this.dragAndDropCell.GetItem() != null;

        public void HighlightForDrop()
        {
            SlotItemView item = this.GetItem();
            if (item == null)
            {
                this.dropInnerGlow.gameObject.SetActive(true);
            }
            else
            {
                this.dropOuterGlow.gameObject.SetActive(true);
                item.HighlightEnable = false;
            }
        }

        public void OnItemPlace(DragAndDropItem item)
        {
            SlotItemView component = item.GetComponent<SlotItemView>();
            this.UpdateItemTransform(component);
            component.HighlightEnable = true;
        }

        public virtual void SetItem(SlotItemView item)
        {
            item.transform.SetParent(base.transform, false);
            this.UpdateItemTransform(item);
        }

        public void TurnOffRenderAboveAll()
        {
            base.StopAllCoroutines();
            if (!ModulesTutorialUtil.TUTORIAL_MODE && (base.gameObject.GetComponent<Canvas>() != null))
            {
                Destroy(base.gameObject.GetComponent<GraphicRaycaster>());
                Destroy(base.gameObject.GetComponent<Canvas>());
                Destroy(base.gameObject.GetComponent<CanvasGroup>());
                RectTransform component = base.gameObject.GetComponent<RectTransform>();
                Vector3 vector = component.anchoredPosition3D;
                vector.z = 0f;
                component.anchoredPosition3D = vector;
            }
        }

        public void TurnOnRenderAboveAll()
        {
            if (!ModulesTutorialUtil.TUTORIAL_MODE)
            {
                base.StartCoroutine(this.DelayedTurnOnRenderAboveAll());
            }
        }

        protected void UpdateItemTransform(SlotItemView item)
        {
            item.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            item.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            item.SetScaleToCard3D((float) this.moduleCard3DScale);
        }

        [CompilerGenerated]
        private sealed class <DelayedTurnOnRenderAboveAll>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal RectTransform <rectTransform>__0;
            internal Vector3 <pos>__0;
            internal Canvas <canvas>__0;
            internal CanvasGroup <canvasGroup>__0;
            internal SlotView $this;
            internal object $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.$current = new WaitForEndOfFrame();
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        this.<rectTransform>__0 = this.$this.gameObject.GetComponent<RectTransform>();
                        this.<pos>__0 = this.<rectTransform>__0.anchoredPosition3D;
                        this.<pos>__0.z = NewModulesScreenUIComponent.OVER_SCREEN_Z_OFFSET;
                        this.<rectTransform>__0.anchoredPosition3D = this.<pos>__0;
                        this.<canvas>__0 = this.$this.gameObject.GetComponent<Canvas>();
                        if (this.<canvas>__0 == null)
                        {
                            this.<canvas>__0 = this.$this.gameObject.AddComponent<Canvas>();
                        }
                        this.<canvas>__0.renderMode = RenderMode.WorldSpace;
                        this.<canvas>__0.overrideSorting = true;
                        this.<canvas>__0.sortingOrder = 30;
                        this.$this.gameObject.AddComponent<GraphicRaycaster>();
                        this.<canvasGroup>__0 = this.$this.gameObject.AddComponent<CanvasGroup>();
                        this.<canvasGroup>__0.blocksRaycasts = true;
                        this.<canvasGroup>__0.ignoreParentGroups = true;
                        this.<canvasGroup>__0.interactable = false;
                        this.$PC = -1;
                        break;

                    default:
                        break;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }
    }
}

