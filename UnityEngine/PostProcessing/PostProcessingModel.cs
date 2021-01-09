namespace UnityEngine.PostProcessing
{
    using System;
    using UnityEngine;

    [Serializable]
    public abstract class PostProcessingModel
    {
        [SerializeField, GetSet("enabled")]
        private bool m_Enabled;

        protected PostProcessingModel()
        {
        }

        public virtual void OnValidate()
        {
        }

        public abstract void Reset();

        public bool enabled
        {
            get => 
                this.m_Enabled;
            set
            {
                this.m_Enabled = value;
                if (value)
                {
                    this.OnValidate();
                }
            }
        }
    }
}

