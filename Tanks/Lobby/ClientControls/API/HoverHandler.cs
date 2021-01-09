namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;

    public class HoverHandler : MonoBehaviour
    {
        private Camera camera;
        private bool _pointerIn;

        private void OnEnable()
        {
            this.camera = base.GetComponentInParent<Canvas>().worldCamera;
        }

        private void Update()
        {
            bool flag = false;
            RaycastHit[] hitArray2 = Physics.RaycastAll(this.camera.ScreenPointToRay(Input.mousePosition), 100f);
            int index = 0;
            while (true)
            {
                if (index < hitArray2.Length)
                {
                    RaycastHit hit = hitArray2[index];
                    if (hit.collider.gameObject != base.gameObject)
                    {
                        index++;
                        continue;
                    }
                    flag = true;
                }
                if (flag && !this.pointerIn)
                {
                    this.pointerIn = true;
                }
                else if (!flag && this.pointerIn)
                {
                    this.pointerIn = false;
                }
                return;
            }
        }

        protected virtual bool pointerIn
        {
            get => 
                this._pointerIn;
            set => 
                this._pointerIn = value;
        }
    }
}

