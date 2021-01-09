namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("UI/UI Table view", 0x26)]
    public class UITableView : ScrollRect
    {
        [SerializeField]
        private UITableViewCell cellPrefab;
        private List<UITableViewCell> cellsPool = new List<UITableViewCell>();
        [SerializeField]
        private float CellsSpacing;
        [SerializeField]
        private float CellHeight;
        private List<int> currentVisibleIndexes = new List<int>();

        protected virtual UITableViewCell CellForRowAtIndex(int index)
        {
            if ((index < 0) || (index >= this.NumberOfRows()))
            {
                return null;
            }
            UITableViewCell cell = this.ReusableCell();
            cell.Index = index;
            cell.UpdatePositionImmidiate();
            return cell;
        }

        public void CellRemoved(UITableViewCell cell)
        {
            cell.CellRemoved -= new TableViewCellRemoved(this.CellRemoved);
            Destroy(cell.gameObject);
            foreach (UITableViewCell cell2 in base.content.GetComponentsInChildren<UITableViewCell>())
            {
                cell2.UpdatePosition();
            }
        }

        protected UITableViewCell GetCellByIndex(int index)
        {
            foreach (UITableViewCell cell in base.content.GetComponentsInChildren<UITableViewCell>())
            {
                if ((cell.Index == index) && !cell.removed)
                {
                    return cell;
                }
            }
            return null;
        }

        private List<int> GetVisibleIndexes()
        {
            List<int> list = new List<int>();
            float num3 = (base.viewport.rect.height / (this.CellHeight + this.CellsSpacing)) + 1f;
            int num4 = (int) (base.content.anchoredPosition.y / (this.CellHeight + this.CellsSpacing));
            for (int i = 0; i < num3; i++)
            {
                list.Add(num4 + i);
            }
            return list;
        }

        protected virtual int NumberOfRows() => 
            0;

        protected override void OnDisable()
        {
            base.OnDisable();
            if (Application.isPlaying)
            {
                for (int i = 0; i < this.cellsPool.Count; i++)
                {
                    if (this.cellsPool[i] != null)
                    {
                        Destroy(this.cellsPool[i].gameObject);
                    }
                }
                this.cellsPool.Clear();
            }
        }

        public Vector2 PositionForRowAtIndex(int index) => 
            new Vector2(0f, -((this.CellHeight * index) + (this.CellsSpacing * index)));

        public void RemoveCell(int index, bool toRight)
        {
            if (this.currentVisibleIndexes.Contains(index))
            {
                UITableViewCell cellByIndex = this.GetCellByIndex(index);
                if (cellByIndex != null)
                {
                    this.cellsPool.Remove(cellByIndex);
                    cellByIndex.removed = true;
                    cellByIndex.CellRemoved += new TableViewCellRemoved(this.CellRemoved);
                    if (this.currentVisibleIndexes.Contains(cellByIndex.Index))
                    {
                        this.currentVisibleIndexes.Remove(cellByIndex.Index);
                    }
                    int num = 0;
                    while (true)
                    {
                        if (num >= this.currentVisibleIndexes.Count)
                        {
                            cellByIndex.Remove(toRight);
                            break;
                        }
                        UITableViewCell cell2 = this.GetCellByIndex(this.currentVisibleIndexes[num]);
                        if ((cell2 != null) && (cell2.Index > cellByIndex.Index))
                        {
                            List<int> list;
                            int num2;
                            cell2.Index--;
                            (list = this.currentVisibleIndexes)[num2 = num] = list[num2] - 1;
                        }
                        num++;
                    }
                }
            }
        }

        private UITableViewCell ReusableCell()
        {
            UITableViewCell cell2;
            using (List<UITableViewCell>.Enumerator enumerator = this.cellsPool.GetEnumerator())
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        UITableViewCell current = enumerator.Current;
                        if (current.gameObject.activeSelf)
                        {
                            continue;
                        }
                        current.gameObject.SetActive(true);
                        cell2 = current;
                    }
                    else
                    {
                        UITableViewCell item = Instantiate<UITableViewCell>(this.cellPrefab, base.content);
                        item.gameObject.SetActive(true);
                        this.cellsPool.Add(item);
                        return item;
                    }
                    break;
                }
            }
            return cell2;
        }

        private void Update()
        {
            if (Application.isPlaying)
            {
                this.UpdateCells();
                this.UpdateContentHeight();
            }
        }

        private void UpdateCells()
        {
            if (Application.isPlaying)
            {
                List<int> visibleIndexes = this.GetVisibleIndexes();
                if (!visibleIndexes.Equals(this.currentVisibleIndexes))
                {
                    if (visibleIndexes.Count >= this.currentVisibleIndexes.Count)
                    {
                        for (int i = 0; i < visibleIndexes.Count; i++)
                        {
                            if (!this.currentVisibleIndexes.Contains(visibleIndexes[i]))
                            {
                                this.CellForRowAtIndex(visibleIndexes[i]);
                            }
                        }
                    }
                    if (this.currentVisibleIndexes.Count >= visibleIndexes.Count)
                    {
                        for (int i = 0; i < this.currentVisibleIndexes.Count; i++)
                        {
                            if (!visibleIndexes.Contains(this.currentVisibleIndexes[i]))
                            {
                                UITableViewCell cellByIndex = this.GetCellByIndex(this.currentVisibleIndexes[i]);
                                if (cellByIndex != null)
                                {
                                    cellByIndex.gameObject.SetActive(false);
                                }
                            }
                        }
                    }
                    this.currentVisibleIndexes = visibleIndexes;
                }
            }
        }

        private void UpdateContentHeight()
        {
            float y = (this.NumberOfRows() * this.CellHeight) + (this.NumberOfRows() * this.CellsSpacing);
            if ((base.content != null) && (base.content.rect.height != y))
            {
                base.content.sizeDelta = new Vector2(base.content.sizeDelta.x, y);
            }
        }

        public void UpdateTable()
        {
            for (int i = 0; i < this.cellsPool.Count; i++)
            {
                if (this.cellsPool[i] != null)
                {
                    this.cellsPool[i].gameObject.SetActive(false);
                }
            }
            this.currentVisibleIndexes.Clear();
            base.content.anchoredPosition = Vector2.zero;
        }

        public UITableViewCell CellPrefab
        {
            get => 
                this.cellPrefab;
            set => 
                this.cellPrefab = value;
        }
    }
}

