namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class FlagVisualRotate : MonoBehaviour
    {
        public GameObject flag;
        public Transform tank;
        public float timerUpsideDown;
        public float scale;
        public float origin;
        public float distanceForRotateFlag;
        private Transform child;
        private float targetAngle;
        private float curentAngle;
        private float timeSinceUpsideDown;
        private Vector3 direction;
        private Vector3 newPos;
        private Vector3 deltaPos;
        private Vector3 oldPos;
        public Component sprite;
        public Sprite3D spriteComponent;

        private void Start()
        {
            this.child = this.flag.transform.GetChild(0);
            this.spriteComponent = this.flag.transform.GetComponent<Sprite3D>();
        }

        private void Update()
        {
            if (this.flag.transform.parent != null)
            {
                this.newPos = this.tank.position;
                this.deltaPos = this.newPos - this.oldPos;
                this.direction = this.tank.InverseTransformDirection(this.deltaPos);
                if (this.direction.z > this.distanceForRotateFlag)
                {
                    this.targetAngle = 0f;
                }
                if (this.direction.z < -this.distanceForRotateFlag)
                {
                    this.targetAngle = -180f;
                }
                this.curentAngle = Mathf.LerpAngle(this.curentAngle, this.targetAngle - this.flag.transform.parent.localEulerAngles.y, Time.deltaTime);
                this.child.transform.localEulerAngles = new Vector3(0f, this.curentAngle, 0f);
                this.oldPos = this.tank.position;
                if (this.flag.transform.up.y > 0f)
                {
                    this.timeSinceUpsideDown = 0f;
                    this.spriteComponent.scale = 0f;
                    this.spriteComponent.originY = this.origin;
                }
                else
                {
                    this.timeSinceUpsideDown += Time.deltaTime;
                    if (this.timeSinceUpsideDown >= this.timerUpsideDown)
                    {
                        this.spriteComponent.scale = this.scale;
                        this.spriteComponent.originY = this.origin;
                    }
                }
            }
        }
    }
}

