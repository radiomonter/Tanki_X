namespace Tanks.Battle.ClientCore.Impl
{
    using System;
    using UnityEngine;

    public class UnitMoveSmootherTestBehaviour : MonoBehaviour
    {
        private unsafe void Update()
        {
            RaycastHit hit;
            Rigidbody componentInChildren = base.GetComponentInChildren<Rigidbody>();
            UnitMoveSmootherComponent component = base.GetComponentInChildren<UnitMoveSmootherComponent>();
            if (Input.GetMouseButtonUp(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Vector3 point = hit.point;
                Vector3* vectorPtr1 = &point;
                vectorPtr1->y += 0.5f;
                component.BeforeSetMovement();
                Vector3 vector2 = point;
                componentInChildren.transform.position = vector2;
                componentInChildren.position = vector2;
                Quaternion quaternion = Quaternion.LookRotation(Vector3.left);
                componentInChildren.transform.rotation = quaternion;
                componentInChildren.rotation = quaternion;
                componentInChildren.velocity = Vector3.zero;
                componentInChildren.angularVelocity = Vector3.zero;
                componentInChildren.ResetInertiaTensor();
                component.AfterSetMovement();
            }
        }
    }
}

