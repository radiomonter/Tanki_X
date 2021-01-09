namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [SerialVersionUID(0x8d2e6e10a64bc0aL)]
    public class ScoreTableComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private RectTransform headerContainer;
        [SerializeField]
        protected ScoreTableRowComponent rowPrefab;
        private HashSet<RectTransform> excluded = new HashSet<RectTransform>();
        private bool dirty;
        private bool headerDirty;
        public float rowHeight = 66f;
        public float rowSpacing = 5f;
        private Queue<ScoreTableRowComponent> rowsCache = new Queue<ScoreTableRowComponent>();
        private bool useRowsCache;
        private List<ScoreTableRowIndicator> rowIndicators;

        public ScoreTableRowComponent AddRow()
        {
            if (!this.useRowsCache || (this.rowsCache.Count == 0))
            {
                ScoreTableRowComponent component2 = Instantiate<ScoreTableRowComponent>(this.rowPrefab);
                component2.AddIndicators(this.rowIndicators);
                component2.transform.SetParent(base.transform, false);
                this.SetDirty();
                return component2;
            }
            ScoreTableRowComponent component = this.rowsCache.Dequeue();
            component.gameObject.SetActive(true);
            component.transform.SetParent(base.transform, false);
            component.transform.localScale = Vector3.one;
            this.SetDirty();
            return component;
        }

        public void Clear()
        {
            IEnumerator enumerator = base.transform.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform) enumerator.Current;
                    if (current != this.headerContainer)
                    {
                        Destroy(current.gameObject);
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            this.SetHeight(0f);
            this.ClearCache();
        }

        public void ClearCache()
        {
            if (this.useRowsCache)
            {
                while (this.rowsCache.Count > 0)
                {
                    ScoreTableRowComponent component = this.rowsCache.Dequeue();
                    Destroy(component.gameObject);
                }
            }
        }

        public void InitRowsCache(int cacheSize, List<ScoreTableRowIndicator> rowIndicators)
        {
            for (int i = 0; i < cacheSize; i++)
            {
                ScoreTableRowComponent item = Instantiate<ScoreTableRowComponent>(this.rowPrefab);
                item.AddIndicators(rowIndicators);
                item.gameObject.SetActive(false);
                this.rowsCache.Enqueue(item);
            }
            this.rowIndicators = rowIndicators;
            this.useRowsCache = true;
        }

        private void LateUpdate()
        {
            Animator animator = base.GetComponent<Animator>();
            bool flag = (animator == null) || animator.GetBool("Visible");
            if (this.dirty && flag)
            {
                this.UpdatePositions();
                this.dirty = false;
                this.excluded.Clear();
            }
            if (this.headerDirty)
            {
                this.headerDirty = false;
                ScoreTableHeaderComponent component = base.GetComponent<ScoreTableHeaderComponent>();
                if (component != null)
                {
                    component.SetDirty();
                }
            }
        }

        public void RemoveRow(ScoreTableRowComponent row)
        {
            if (!this.useRowsCache)
            {
                this.excluded.Add((RectTransform) row.transform);
                DestroyImmediate(row.gameObject);
                this.SetDirty();
            }
            else
            {
                this.rowsCache.Enqueue(row);
                foreach (ScoreTableRowIndicator indicator in row.indicators.Values)
                {
                    EntityBehaviour component = indicator.GetComponent<EntityBehaviour>();
                    if (component != null)
                    {
                        component.DetachFromEntity();
                    }
                }
                row.gameObject.SetActive(false);
                row.transform.SetParent(null, false);
                this.SetDirty();
            }
        }

        public void SetDirty()
        {
            this.dirty = true;
            this.headerDirty = true;
        }

        public void SetHeaderDirty()
        {
            this.headerDirty = true;
        }

        private void SetHeight(float height)
        {
            Vector2 sizeDelta = ((RectTransform) base.transform).sizeDelta;
            sizeDelta.y = height;
            ((RectTransform) base.transform).sizeDelta = sizeDelta;
            LayoutElement component = base.GetComponent<LayoutElement>();
            if (component != null)
            {
                component.preferredHeight = height;
            }
        }

        public void UpdatePositions()
        {
            int num = 0;
            int index = 0;
            int childCount = base.transform.childCount;
            while (index < childCount)
            {
                RectTransform child = (RectTransform) base.transform.GetChild(index);
                if ((child != null) && !this.excluded.Contains(child))
                {
                    ScoreTableRowComponent component = child.GetComponent<ScoreTableRowComponent>();
                    if (component == null)
                    {
                        LayoutRebuilder.MarkLayoutForRebuild(child);
                    }
                    else if (component.Position != 0)
                    {
                        num++;
                        Vector2 anchoredPosition = child.anchoredPosition;
                        anchoredPosition.y = -(component.Position * (this.rowHeight + this.rowSpacing));
                        child.anchoredPosition = anchoredPosition;
                    }
                }
                index++;
            }
            int num5 = 0;
            int num6 = 0;
            int num7 = base.transform.childCount;
            while (num6 < num7)
            {
                RectTransform child = (RectTransform) base.transform.GetChild(num6);
                ScoreTableRowComponent component = child.GetComponent<ScoreTableRowComponent>();
                if ((component != null) && (component.Position == 0))
                {
                    num5++;
                    Vector2 anchoredPosition = child.anchoredPosition;
                    anchoredPosition.y = -(((num + num5) + 1) * (this.rowHeight + this.rowSpacing));
                    child.anchoredPosition = anchoredPosition;
                }
                num6++;
            }
            float height = ((num5 + num) * (this.rowHeight + this.rowSpacing)) + this.rowHeight;
            this.SetHeight(height);
        }
    }
}

