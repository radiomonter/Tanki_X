using System;
using UnityEngine;

public class ColliderTest : MonoBehaviour
{
    private Vector3 hitPoint;
    private bool flag;

    private void Update()
    {
        RaycastHit hit;
        if (Input.GetKeyDown(KeyCode.Mouse0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            base.GetComponent<ForceFieldEffect>().DrawWave(hit.point, false);
            Debug.Log("Draw");
        }
    }
}

