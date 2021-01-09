using System;
using UnityEngine;

[ExecuteInEditMode]
public class LoginRewardItemTooltip : MonoBehaviour
{
    [SerializeField]
    private RectTransform text;
    [SerializeField]
    private RectTransform back;

    private void Update()
    {
        this.back.sizeDelta = new Vector2(270f, this.text.sizeDelta.y + 30f);
    }

    public string Text
    {
        get => 
            this.text.GetComponent<TextMeshProUGUI>().text;
        set => 
            this.text.GetComponent<TextMeshProUGUI>().text = value;
    }
}

