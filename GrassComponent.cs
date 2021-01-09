using System;
using UnityEngine;

public class GrassComponent : MonoBehaviour
{
    public void Disable()
    {
        base.gameObject.SetActive(false);
    }
}

