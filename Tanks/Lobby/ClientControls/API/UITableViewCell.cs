namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;

    [RequireComponent(typeof(RectTransform))]
    public class UITableViewCell : MonoBehaviour
    {
        private TableViewCellRemoved cellRemoved;
        [SerializeField]
        private int index;
        public bool removed;
        private float moveSpeed = 600f;
        private UITableView tableView;
        private RectTransform cellRect;
        private Vector2 targetPosition;
        private Animator animator;
        private bool moveToPosition;

        private void Awake()
        {
            this.cellRect = base.GetComponent<RectTransform>();
            Vector2 vector = new Vector2(0f, 1f);
            this.cellRect.anchorMax = vector;
            this.cellRect.anchorMin = vector;
            this.cellRect.pivot = vector;
            this.tableView = base.GetComponentInParent<UITableView>();
            this.animator = base.GetComponent<Animator>();
        }

        protected void OnDestroy()
        {
            this.cellRemoved = null;
        }

        protected void OnDisable()
        {
            this.animator.SetBool("show", false);
            this.animator.SetBool("remove", false);
            if (this.cellRemoved != null)
            {
                this.cellRemoved(this);
            }
        }

        public void Remove(bool toRight)
        {
            if (!base.gameObject.activeSelf)
            {
                this.Removed();
            }
            else
            {
                this.animator.SetBool("toRight", toRight);
                this.animator.SetBool("remove", true);
            }
        }

        private void Removed()
        {
            if (this.cellRemoved != null)
            {
                this.cellRemoved(this);
            }
        }

        private void Update()
        {
            if (this.moveToPosition)
            {
                float num = Vector2.Distance(this.cellRect.anchoredPosition, this.targetPosition);
                if (num > 0.1f)
                {
                    this.cellRect.anchoredPosition = Vector2.Lerp(this.cellRect.anchoredPosition, this.targetPosition, (Time.deltaTime / num) * this.moveSpeed);
                }
                else
                {
                    this.moveToPosition = false;
                }
            }
        }

        public void UpdatePosition()
        {
            this.moveToPosition = true;
        }

        public void UpdatePositionImmidiate()
        {
            this.cellRect.anchoredPosition = this.targetPosition;
        }

        public TableViewCellRemoved CellRemoved
        {
            get => 
                this.cellRemoved;
            set => 
                this.cellRemoved = value;
        }

        public int Index
        {
            get => 
                this.index;
            set
            {
                this.index = value;
                this.targetPosition = this.tableView.PositionForRowAtIndex(this.index);
            }
        }

        public RectTransform CellRect =>
            this.cellRect;
    }
}

