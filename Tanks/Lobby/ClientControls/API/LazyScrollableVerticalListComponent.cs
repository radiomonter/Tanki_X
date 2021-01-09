namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    [RequireComponent(typeof(ScrollRect))]
    public class LazyScrollableVerticalListComponent : MonoBehaviour, Component
    {
        private ScrollRect scrollRect;
        private LayoutElement aboveViewportPlaceholder;
        private LayoutElement belowViewportPlaceholder;
        private static readonly Vector3[] corners = new Vector3[4];

        public void AdjustChildrenVisibility()
        {
            this.AdjustPlaceholdersSiblingIndices();
            float spacing = this.scrollRect.content.GetComponent<VerticalLayoutGroup>().spacing;
            int index = 0;
            float offset = 0f;
            while (true)
            {
                if (index < this.scrollRect.content.childCount)
                {
                    RectTransform child = (RectTransform) this.scrollRect.content.GetChild(index);
                    if (this.IsPlaceholder(child))
                    {
                        index++;
                        continue;
                    }
                    this.AlignInactiveChild(child, offset);
                    if (!AreTransformsOverlaps(child, this.scrollRect.viewport))
                    {
                        offset += child.rect.height + spacing;
                        if (child.gameObject.activeSelf)
                        {
                            child.gameObject.SetActive(false);
                        }
                        index++;
                        continue;
                    }
                }
                this.SetPlaceholderHeight(this.aboveViewportPlaceholder, (offset <= 0f) ? 0f : (offset - spacing));
                while (true)
                {
                    if (index < this.scrollRect.content.childCount)
                    {
                        RectTransform child = (RectTransform) this.scrollRect.content.GetChild(index);
                        if (this.IsPlaceholder(child))
                        {
                            index++;
                            continue;
                        }
                        this.AlignInactiveChild(child, offset);
                        if (AreTransformsOverlaps(child, this.scrollRect.viewport))
                        {
                            offset += child.rect.height + spacing;
                            if (!child.gameObject.activeSelf)
                            {
                                child.gameObject.SetActive(true);
                            }
                            index++;
                            continue;
                        }
                    }
                    float num4 = 0f;
                    RectTransform transform = (RectTransform) this.belowViewportPlaceholder.transform;
                    while (index < this.scrollRect.content.childCount)
                    {
                        RectTransform child = (RectTransform) this.scrollRect.content.GetChild(index);
                        if (this.IsPlaceholder(child))
                        {
                            index++;
                            continue;
                        }
                        this.AlignInactiveChild(child, -transform.anchoredPosition.y + num4);
                        num4 += child.rect.height + spacing;
                        if (child.gameObject.activeSelf)
                        {
                            child.gameObject.SetActive(false);
                        }
                        index++;
                    }
                    this.SetPlaceholderHeight(this.belowViewportPlaceholder, (num4 <= 0f) ? 0f : (num4 - spacing));
                    return;
                }
            }
        }

        public void AdjustPlaceholdersSiblingIndices()
        {
            this.Init();
            this.aboveViewportPlaceholder.transform.SetAsFirstSibling();
            this.belowViewportPlaceholder.transform.SetAsLastSibling();
        }

        private void AlignInactiveChild(RectTransform inactiveItem, float offset)
        {
            if (!inactiveItem.gameObject.activeSelf)
            {
                inactiveItem.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, offset, inactiveItem.rect.height);
            }
        }

        private static bool AreTransformsOverlaps(RectTransform a, RectTransform b)
        {
            a.GetWorldCorners(corners);
            Rect rect = new Rect(corners[0], corners[2] - corners[0]);
            b.GetWorldCorners(corners);
            Rect other = new Rect(corners[0], corners[2] - corners[0]);
            return rect.Overlaps(other);
        }

        private void Awake()
        {
            this.Init();
        }

        private LayoutElement CreatePlacehodler(string name)
        {
            LayoutElement element = new GameObject(name).AddComponent<LayoutElement>();
            RectTransform component = element.gameObject.GetComponent<RectTransform>();
            component.SetParent(this.scrollRect.content, false);
            component.pivot = new Vector2(0f, 1f);
            element.minHeight = 0f;
            element.preferredHeight = 0f;
            return element;
        }

        private void CreatePlaceholders()
        {
            this.aboveViewportPlaceholder = this.CreatePlacehodler("ABOVE_PLACEHOLDER");
            this.belowViewportPlaceholder = this.CreatePlacehodler("BELOW_PLACEHOLDER");
            this.AdjustPlaceholdersSiblingIndices();
        }

        private void Init()
        {
            if (this.scrollRect == null)
            {
                this.scrollRect = base.GetComponent<ScrollRect>();
                this.CreatePlaceholders();
            }
        }

        private bool IsPlaceholder(RectTransform transform) => 
            (transform.gameObject == this.aboveViewportPlaceholder.gameObject) || (transform.gameObject == this.belowViewportPlaceholder.gameObject);

        private void OnDisable()
        {
            this.aboveViewportPlaceholder.minHeight = 0f;
            this.aboveViewportPlaceholder.preferredHeight = 0f;
            this.belowViewportPlaceholder.minHeight = 0f;
            this.belowViewportPlaceholder.preferredHeight = 0f;
        }

        private void OnValueChange(Vector2 value)
        {
            this.AdjustChildrenVisibility();
        }

        private void SetPlaceholderHeight(LayoutElement placeholder, float height)
        {
            placeholder.minHeight = height;
            placeholder.preferredHeight = placeholder.preferredHeight;
            placeholder.gameObject.SetActive(placeholder.minHeight > 0f);
        }

        private void Start()
        {
            this.scrollRect.onValueChanged.AddListener(new UnityAction<Vector2>(this.OnValueChange));
        }
    }
}

