using System;
using UnityEngine;

[ExecuteInEditMode]
public class BarFillEnd : MonoBehaviour
{
    [SerializeField]
    protected RectTransform image;
    private float fillAmount;

    public virtual float FillAmount
    {
        get => 
            this.fillAmount;
        set
        {
            this.fillAmount = value;
            this.image.anchoredPosition = new Vector2(base.GetComponent<RectTransform>().rect.width * value, this.image.anchoredPosition.y);
            this.image.gameObject.SetActive((value != 0f) && (value != 1f));
        }
    }
}

