namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    [AddComponentMenu("Layout/Simple Layout Element", 0x8d), RequireComponent(typeof(RectTransform)), ExecuteInEditMode]
    public class SimpleLayoutElement : UIBehaviour, ISimpleLayoutElement, ILayoutIgnorer
    {
        [SerializeField]
        private bool m_IgnoreLayout;
        [SerializeField]
        private float m_FlexibleWidth = -1f;
        [SerializeField]
        private float m_FlexibleHeight = -1f;
        [SerializeField]
        private float m_MinWidth = -1f;
        [SerializeField]
        private float m_MinHeight = -1f;
        [SerializeField]
        private float m_MaxWidth = -1f;
        [SerializeField]
        private float m_MaxHeight = -1f;

        protected SimpleLayoutElement()
        {
        }

        protected override void OnBeforeTransformParentChanged()
        {
            this.SetDirty();
        }

        protected override void OnDidApplyAnimationProperties()
        {
            this.SetDirty();
        }

        protected override void OnDisable()
        {
            this.SetDirty();
            base.OnDisable();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            this.SetDirty();
        }

        protected override void OnTransformParentChanged()
        {
            this.SetDirty();
        }

        protected void SetDirty()
        {
            if (this.IsActive())
            {
                LayoutRebuilder.MarkLayoutForRebuild(base.transform as RectTransform);
            }
        }

        public static bool SetStruct<T>(ref T currentValue, T newValue) where T: struct
        {
            if (currentValue.Equals(newValue))
            {
                return false;
            }
            currentValue = newValue;
            return true;
        }

        public virtual bool ignoreLayout
        {
            get => 
                this.m_IgnoreLayout;
            set
            {
                if (SetStruct<bool>(ref this.m_IgnoreLayout, value))
                {
                    this.SetDirty();
                }
            }
        }

        public virtual float flexibleWidth
        {
            get => 
                ((this.m_FlexibleWidth <= 0f) || ((this.m_MaxWidth > 0f) && (this.m_MaxWidth <= this.m_MinWidth))) ? 0f : this.m_FlexibleWidth;
            set
            {
                if (SetStruct<float>(ref this.m_FlexibleWidth, value))
                {
                    this.SetDirty();
                }
            }
        }

        public virtual float flexibleHeight
        {
            get => 
                ((this.m_FlexibleHeight <= 0f) || ((this.m_MaxHeight > 0f) && (this.m_MaxHeight <= this.m_MinHeight))) ? 0f : this.m_FlexibleHeight;
            set
            {
                if (SetStruct<float>(ref this.m_FlexibleHeight, value))
                {
                    this.SetDirty();
                }
            }
        }

        public virtual float minWidth
        {
            get => 
                this.m_MinWidth;
            set
            {
                if (SetStruct<float>(ref this.m_MinWidth, value))
                {
                    this.SetDirty();
                }
            }
        }

        public virtual float minHeight
        {
            get => 
                this.m_MinHeight;
            set
            {
                if (SetStruct<float>(ref this.m_MinHeight, value))
                {
                    this.SetDirty();
                }
            }
        }

        public virtual float maxWidth
        {
            get
            {
                if (((this.m_MaxWidth <= this.m_MinWidth) && (this.m_MaxWidth > 0f)) || (this.m_FlexibleWidth <= 0f))
                {
                    return this.m_MinWidth;
                }
                return this.m_MaxWidth;
            }
            set
            {
                if (SetStruct<float>(ref this.m_MaxWidth, value))
                {
                    this.SetDirty();
                }
            }
        }

        public virtual float maxHeight
        {
            get
            {
                if (((this.m_MaxHeight <= this.m_MinHeight) && (this.m_MaxHeight > 0f)) || (this.m_FlexibleHeight <= 0f))
                {
                    return this.m_MinHeight;
                }
                return this.m_MaxHeight;
            }
            set
            {
                if (SetStruct<float>(ref this.m_MaxHeight, value))
                {
                    this.SetDirty();
                }
            }
        }

        public virtual int layoutPriority =>
            1;
    }
}

