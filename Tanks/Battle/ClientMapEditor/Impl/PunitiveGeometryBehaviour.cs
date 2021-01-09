namespace Tanks.Battle.ClientMapEditor.Impl
{
    using System;
    using System.Diagnostics;
    using UnityEngine;

    [ExecuteInEditMode, RequireComponent(typeof(BoxCollider))]
    public class PunitiveGeometryBehaviour : EditorBehavior
    {
        public bool showGeometry;
        public AnticheatAction anticheatAction;

        public void Initialize(AnticheatAction anticheatAction)
        {
            this.anticheatAction = anticheatAction;
        }

        private void OnDrawGizmos()
        {
            if (this.showGeometry)
            {
                Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
                Gizmos.DrawCube(base.transform.position + new Vector3(base.GetComponent<BoxCollider>().center.x, base.GetComponent<BoxCollider>().center.y, base.GetComponent<BoxCollider>().center.z), new Vector3(base.GetComponent<BoxCollider>().size.x, base.GetComponent<BoxCollider>().size.y, base.GetComponent<BoxCollider>().size.z));
            }
        }

        [Conditional("UNITY_EDITOR")]
        private void Update()
        {
            BoxCollider component = base.GetComponent<BoxCollider>();
            if (component.transform.rotation != Quaternion.identity)
            {
                Debug.LogWarning("Punitive boxes can not be rotated");
                component.transform.rotation = Quaternion.identity;
            }
            if (component.transform.localScale != Vector3.one)
            {
                Debug.LogWarning("Punitive boxes can not be scaled");
                component.transform.localScale = Vector3.one;
            }
        }
    }
}

