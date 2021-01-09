namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using TMPro;
    using UnityEngine;

    public class TMPLink : MonoBehaviour
    {
        private Camera cam;
        private Canvas canvas;
        private int selectedLink = -1;
        private TMP_Text tmpText;

        private string GetColoredLinkText(string text) => 
            "<color=blue>" + text + "</color>";

        private void HighlightLink(int linkIndex)
        {
            TMP_LinkInfo info = this.tmpText.textInfo.linkInfo[linkIndex];
            string[] textArray1 = new string[] { "<u><link=", info.GetLinkID(), ">", info.GetLinkText(), "</link></u>" };
            string oldValue = string.Concat(textArray1);
            this.tmpText.text = this.tmpText.text.Replace(oldValue, this.GetColoredLinkText(oldValue));
            Cursors.SwitchToCursor(CursorType.HAND);
        }

        private void LateUpdate()
        {
            if (TMP_TextUtilities.IsIntersectingRectTransform(this.tmpText.rectTransform, Input.mousePosition, this.cam))
            {
                int linkIndex = TMP_TextUtilities.FindIntersectingLink(this.tmpText, Input.mousePosition, this.cam);
                if ((this.selectedLink != -1) && (linkIndex != this.selectedLink))
                {
                    this.UnhighlightLink(this.selectedLink);
                }
                if ((linkIndex != -1) && (linkIndex != this.selectedLink))
                {
                    this.HighlightLink(linkIndex);
                }
                this.selectedLink = linkIndex;
            }
            if (Input.GetMouseButtonDown(0) && (this.selectedLink != -1))
            {
                Application.OpenURL(this.tmpText.textInfo.linkInfo[this.selectedLink].GetLinkID());
            }
        }

        private void Start()
        {
            this.tmpText = base.gameObject.GetComponent<TMP_Text>();
            if (!ReferenceEquals(this.tmpText.GetType(), typeof(TextMeshProUGUI)))
            {
                this.cam = Camera.main;
            }
            else
            {
                this.canvas = base.gameObject.GetComponentInParent<Canvas>();
                if (this.canvas != null)
                {
                    this.cam = (this.canvas.renderMode != RenderMode.ScreenSpaceOverlay) ? this.canvas.worldCamera : null;
                }
            }
        }

        private void UnhighlightLink(int linkIndex)
        {
            TMP_LinkInfo info = this.tmpText.textInfo.linkInfo[linkIndex];
            string[] textArray1 = new string[] { "<u><link=", info.GetLinkID(), ">", info.GetLinkText(), "</link></u>" };
            string newValue = string.Concat(textArray1);
            this.tmpText.text = this.tmpText.text.Replace(this.GetColoredLinkText(newValue), newValue);
            Cursors.SwitchToDefaultCursor();
        }
    }
}

