using System;
using UnityEngine;
using UnityEngine.UI;

public class NotificationRowComponent : MonoBehaviour
{
    private void Awake()
    {
        HorizontalLayoutGroup component = base.GetComponent<HorizontalLayoutGroup>();
        if (Screen.height > 0x438)
        {
            component.spacing = Screen.width / 5;
        }
    }
}

