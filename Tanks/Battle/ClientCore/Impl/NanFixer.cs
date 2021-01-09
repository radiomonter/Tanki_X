namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Library.ClientLogger.API;
    using System;
    using UnityEngine;

    public class NanFixer : MonoBehaviour
    {
        private Rigidbody body;
        private Transform tr;
        private long userId;
        private Vector3 prevBodyPosition;
        private Quaternion prevBodyRotation;
        private Vector3 prevBodyVelocity;
        private Vector3 prevBodyAngularVelocity;
        private Vector3 prevPosition;
        private Quaternion prevRotation;
        private int logNumber;
        private const int logThreshold = 100;
        public bool testInjectNan;

        public bool FixAndSave()
        {
            bool flag = this.TryFix();
            if (flag && (this.logNumber < 100))
            {
                object[] args = new object[] { GetPath(this.tr), Time.frameCount, this.userId, this.logNumber };
                LoggerProvider.GetLogger(typeof(PhysicsUtil)).ErrorFormat("NanFixer fix {0} at frame {1}, user: {2}, logNumber: {3}", args);
                this.logNumber++;
            }
            this.SaveState();
            return flag;
        }

        public static string GetPath(Transform current) => 
            (current.parent != null) ? (GetPath(current.parent) + "/" + current.name) : ("/" + current.name);

        public void Init(Rigidbody body, Transform tr, long userId)
        {
            this.body = body;
            this.tr = tr;
            this.userId = userId;
        }

        public void SaveState()
        {
            if (this.body != null)
            {
                this.prevBodyPosition = this.body.position;
                this.prevBodyRotation = this.body.rotation;
                this.prevBodyVelocity = this.body.velocity;
                this.prevBodyAngularVelocity = this.body.angularVelocity;
            }
            if (this.tr != null)
            {
                this.prevPosition = this.tr.position;
                this.prevRotation = this.tr.rotation;
            }
        }

        public bool TryFix()
        {
            int num = 0;
            if (this.body != null)
            {
                if (!PhysicsUtil.ValidateVector3(this.body.position))
                {
                    this.body.position = this.prevBodyPosition;
                    num++;
                }
                if (!PhysicsUtil.ValidateQuaternion(this.body.rotation))
                {
                    this.body.rotation = this.prevBodyRotation;
                    num++;
                }
                if (!PhysicsUtil.ValidateVector3(this.body.velocity))
                {
                    this.body.velocity = this.prevBodyVelocity;
                    num++;
                }
                if (!PhysicsUtil.ValidateVector3(this.body.angularVelocity))
                {
                    this.body.angularVelocity = this.prevBodyAngularVelocity;
                    num++;
                }
            }
            if (this.tr != null)
            {
                if (!PhysicsUtil.ValidateVector3(this.tr.position))
                {
                    this.tr.position = this.prevPosition;
                    num++;
                }
                if (!PhysicsUtil.ValidateQuaternion(this.tr.rotation))
                {
                    this.tr.rotation = this.prevRotation;
                    num++;
                }
            }
            return (num > 0);
        }
    }
}

