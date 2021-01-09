namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using UnityEngine;

    public class FreeCameraController : MonoBehaviour
    {
        public float xSpeed = 250f;
        public float ySpeed = 120f;
        public float yMinLimit = -20f;
        public float yMaxLimit = 80f;
        public float moveSpeed = 1f;
        private float x;
        private float y;

        private static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360f)
            {
                angle += 360f;
            }
            if (angle > 360f)
            {
                angle -= 360f;
            }
            return Mathf.Clamp(angle, min, max);
        }

        private void Init()
        {
            Vector3 eulerAngles = base.transform.localRotation.eulerAngles;
            this.x = eulerAngles.y;
            this.y = eulerAngles.x;
        }

        private void LateUpdate()
        {
            if ((GUIUtility.hotControl == 0) && (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift)))
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
                {
                    this.x += (Input.GetAxis("Mouse X") * this.xSpeed) * 0.02f;
                    this.y -= (Input.GetAxis("Mouse Y") * this.ySpeed) * 0.02f;
                    this.y = ClampAngle(this.y, this.yMinLimit, this.yMaxLimit);
                }
                Quaternion quaternion = Quaternion.Euler(this.y, this.x, 0f);
                base.transform.localRotation = quaternion;
                if (Input.GetMouseButton(1))
                {
                    Vector3 translation = new Vector3 {
                        x = this.moveSpeed * Input.GetAxis("Horizontal"),
                        y = this.moveSpeed * Input.GetAxis("Deep"),
                        z = this.moveSpeed * Input.GetAxis("Vertical")
                    };
                    if ((translation.x != 0.0) || ((translation.y != 0.0) || (translation.z != 0.0)))
                    {
                        base.transform.Translate(translation);
                    }
                }
            }
        }

        private void OnEnable()
        {
            this.Init();
        }

        private void Start()
        {
            this.Init();
        }
    }
}

