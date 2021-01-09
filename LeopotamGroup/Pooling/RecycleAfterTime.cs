namespace LeopotamGroup.Pooling
{
    using System;
    using UnityEngine;

    public sealed class RecycleAfterTime : MonoBehaviour
    {
        [SerializeField]
        private float _timeout = 1f;
        private float _endTime;

        private void LateUpdate()
        {
            if (Time.time >= this._endTime)
            {
                this.OnRecycle();
            }
        }

        private void OnEnable()
        {
            this._endTime = Time.time + this._timeout;
        }

        private void OnRecycle()
        {
            IPoolObject component = base.GetComponent<IPoolObject>();
            if (component != null)
            {
                component.PoolRecycle(true);
            }
            else
            {
                base.gameObject.SetActive(false);
            }
        }

        public float Timeout
        {
            get => 
                this._timeout;
            set => 
                this._timeout = value;
        }
    }
}

