using System;
using System.Collections.Generic;
using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeyboardNavigation : MonoBehaviour
{
    private HashSet<Selectable> traversed = new HashSet<Selectable>();

    private Selectable FindDown(Selectable current)
    {
        if (this.traversed.Contains(current))
        {
            return null;
        }
        this.traversed.Add(current);
        Selectable selectable = current.FindSelectableOnDown();
        return (!this.IsValidSelectable(selectable) ? this.FindDown(selectable) : selectable);
    }

    private Selectable FindUp(Selectable current)
    {
        if (this.traversed.Contains(current))
        {
            return null;
        }
        this.traversed.Add(current);
        Selectable selectable = current.FindSelectableOnUp();
        return (!this.IsValidSelectable(selectable) ? this.FindUp(selectable) : selectable);
    }

    private bool HasCustomNavigation(Selectable current)
    {
        InputFieldReturnSelector component = current.gameObject.GetComponent<InputFieldReturnSelector>();
        return ((component != null) && component.CanNavigateToSelectable());
    }

    private bool IsValidSelectable(Selectable selectable) => 
        (selectable == null) || (selectable.interactable && selectable.gameObject.activeSelf);

    private void Update()
    {
        Selectable selectable = null;
        if (EventSystem.current != null)
        {
            GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
            if (currentSelectedGameObject != null)
            {
                Selectable component = currentSelectedGameObject.GetComponent<Selectable>();
                if (component != null)
                {
                    if (Input.GetKeyDown(KeyCode.Tab))
                    {
                        this.traversed.Clear();
                        selectable = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ? this.FindUp(component) : this.FindDown(component);
                    }
                    else if (Input.GetKeyDown(KeyCode.Return))
                    {
                        this.traversed.Clear();
                        if ((component is InputField) && !this.HasCustomNavigation(component))
                        {
                            selectable = this.FindDown(component);
                        }
                    }
                    if (selectable != null)
                    {
                        selectable.Select();
                    }
                }
            }
        }
    }
}

