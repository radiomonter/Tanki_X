using System;
using UnityEngine;

public class RandomOffset : MonoBehaviour
{
    [SerializeField]
    private float min;
    [SerializeField]
    private float max = 1f;

    private void OnEnable()
    {
        base.GetComponent<Animator>().SetFloat("offset", Random.Range(this.min, this.max));
    }
}

