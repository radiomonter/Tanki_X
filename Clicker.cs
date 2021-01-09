using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Clicker : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
    private static Clicker selected;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (selected != null)
        {
            selected.GetComponent<Animator>().SetBool("selected", false);
        }
        if (selected == this)
        {
            selected = null;
        }
        else
        {
            selected = this;
            base.GetComponent<Animator>().SetBool("selected", true);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        base.GetComponent<Animator>().SetBool("over", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        base.GetComponent<Animator>().SetBool("over", false);
    }

    private void Start()
    {
    }

    private void Update()
    {
    }
}

