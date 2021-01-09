namespace Tanks.Tool.TankViewer.API
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class TargetCameraController : MonoBehaviour
    {
        public Transform target;
        public float distance = 10f;
        public float xSpeed = 250f;
        public float ySpeed = 120f;
        public float yMinLimit = -20f;
        public float yMaxLimit = 80f;
        public float moveSpeed = 1f;
        public float autoRotateSpeed = 1f;
        public Transform defaultCameraTransform;
        private float x;
        private float y;
        private int rotationMode;

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

        private void LateUpdate()
        {
            if (this.target != null)
            {
                if (this.AutoRotate)
                {
                    base.transform.RotateAround(this.target.position, Vector3.up, Time.deltaTime * this.autoRotateSpeed);
                    Vector3 eulerAngles = base.transform.localRotation.eulerAngles;
                    this.x = eulerAngles.y;
                    this.y = eulerAngles.x;
                }
                else
                {
                    this.distance -= Input.GetAxis("Mouse ScrollWheel");
                    if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
                    {
                        this.x += (Input.GetAxis("Mouse X") * this.xSpeed) * 0.02f;
                        this.y -= (Input.GetAxis("Mouse Y") * this.ySpeed) * 0.02f;
                        this.y = ClampAngle(this.y, this.yMinLimit, this.yMaxLimit);
                    }
                    Quaternion quaternion2 = Quaternion.Euler(this.y, this.x, 0f);
                    base.transform.localRotation = quaternion2;
                    Vector3 vector2 = this.target.position + (quaternion2 * new Vector3(0f, 0f, -this.distance));
                    base.transform.localPosition = vector2;
                }
            }
        }

        public void SetDefaultTransform()
        {
            if (this.defaultCameraTransform != null)
            {
                base.transform.rotation = this.defaultCameraTransform.rotation;
                base.transform.position = this.defaultCameraTransform.position;
                this.distance = Vector3.Distance(base.transform.position, this.target.position);
            }
            Vector3 eulerAngles = base.transform.localRotation.eulerAngles;
            this.x = eulerAngles.y;
            this.y = eulerAngles.x;
        }

        private void Start()
        {
            this.SetDefaultTransform();
        }

        public bool AutoRotate { get; set; }
    }
}

