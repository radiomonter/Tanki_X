namespace Tanks.Lobby.ClientGarage.API
{
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    [ExecuteInEditMode, RequireComponent(typeof(GridLayoutGroup))]
    public class FlexibleWidthColumnGridLayout : UIBehaviour
    {
        [SerializeField]
        private RectTransform viewport;

        protected override void Awake()
        {
            if (this.viewport == null)
            {
                this.viewport = (RectTransform) base.transform.parent;
            }
        }

        private void CalculateWidthCell()
        {
            float width = this.viewport.rect.width;
            GridLayoutGroup component = base.GetComponent<GridLayoutGroup>();
            if (component.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
            {
                int constraintCount = component.constraintCount;
                component.cellSize = new Vector2((float) ((int) ((width - (component.spacing.x * (constraintCount - 1))) / ((float) constraintCount))), component.cellSize.y);
            }
        }

        protected override void OnEnable()
        {
            this.CalculateWidthCell();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            this.CalculateWidthCell();
        }

        protected override void OnTransformParentChanged()
        {
            this.CalculateWidthCell();
        }
    }
}

