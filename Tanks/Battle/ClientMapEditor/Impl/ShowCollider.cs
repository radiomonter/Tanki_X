namespace Tanks.Battle.ClientMapEditor.Impl
{
    using System;
    using UnityEngine;

    public class ShowCollider : MonoBehaviour
    {
        public bool showGeometry;
        public Color edgeColor;
        public Color faceColor;
        private Vector3 position;
        private Quaternion rotation;
        private Vector3 scale;

        private void OnDrawGizmos()
        {
            if (this.showGeometry)
            {
                if (base.GetComponent<BoxCollider>() != null)
                {
                    this.position = base.transform.position + new Vector3(base.GetComponent<BoxCollider>().center.x, base.GetComponent<BoxCollider>().center.y, base.GetComponent<BoxCollider>().center.z);
                    this.scale = new Vector3(base.transform.localScale.x, base.transform.localScale.y, base.transform.localScale.z);
                    Gizmos.color = this.faceColor;
                    Gizmos.DrawCube(this.position, this.scale);
                    Gizmos.color = this.edgeColor;
                    Gizmos.DrawWireCube(this.position, this.scale);
                }
                if (base.GetComponent<SphereCollider>() != null)
                {
                    SphereCollider component = base.GetComponent<SphereCollider>();
                    this.position = base.transform.position + new Vector3(component.center.x, component.center.y, component.center.z);
                    float radius = component.radius;
                    Gizmos.color = this.faceColor;
                    Gizmos.DrawSphere(this.position, radius);
                    Gizmos.color = this.edgeColor;
                    Gizmos.DrawWireSphere(this.position, radius);
                }
            }
        }
    }
}

