namespace CurvedUI
{
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class CurvedUIEnabler : MonoBehaviour
    {
        public void RefreshCurve()
        {
            foreach (Graphic graphic in base.GetComponentsInChildren<Graphic>(true))
            {
                if (graphic.GetComponent<CurvedUIVertexEffect>() == null)
                {
                    graphic.gameObject.AddComponent<CurvedUIVertexEffect>();
                    graphic.SetAllDirty();
                }
            }
            foreach (InputField field in base.GetComponentsInChildren<InputField>(true))
            {
                if (field.GetComponent<CurvedUIInputFieldCaret>() == null)
                {
                    field.gameObject.AddComponent<CurvedUIInputFieldCaret>();
                }
            }
            foreach (TextMeshProUGUI ougui in base.GetComponentsInChildren<TextMeshProUGUI>(true))
            {
                if (ougui.GetComponent<CurvedUITMP>() == null)
                {
                    ougui.gameObject.AddComponent<CurvedUITMP>();
                    ougui.SetAllDirty();
                }
            }
        }
    }
}

