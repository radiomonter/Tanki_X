using System;
using UnityEngine;

public class FitIn : MonoBehaviour
{
    [SerializeField]
    private RectTransform content;
    [SerializeField]
    private RectTransform viewport;
    private Animator animator;

    private void Awake()
    {
        this.animator = base.GetComponent<Animator>();
    }

    private unsafe void Update()
    {
        RectTransform component = base.GetComponent<RectTransform>();
        float num = (-component.anchoredPosition.y - this.content.anchoredPosition.y) + component.rect.height;
        Vector2 anchoredPosition = this.content.anchoredPosition;
        if ((num > this.viewport.rect.height) && this.animator.GetBool("selected"))
        {
            Vector2* vectorPtr1 = &anchoredPosition;
            vectorPtr1->y += num - this.viewport.rect.height;
            this.content.anchoredPosition = anchoredPosition;
        }
        if (((anchoredPosition.y + this.viewport.rect.height) > this.content.rect.height) && !this.animator.GetBool("selected"))
        {
            anchoredPosition.y = Mathf.Max((float) 0f, (float) (this.content.rect.height - this.viewport.rect.height));
            this.content.anchoredPosition = anchoredPosition;
        }
    }
}

