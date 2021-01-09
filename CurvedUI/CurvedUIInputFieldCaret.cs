namespace CurvedUI
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class CurvedUIInputFieldCaret : MonoBehaviour
    {
        private InputField myField;
        private RectTransform myCaret;
        private Color origCaretColor;
        private Color origSelectionColor;
        private bool selectingText;
        private int lastCharDist = 2;
        private float blinkTimer;

        private void Awake()
        {
            this.myField = base.GetComponent<InputField>();
        }

        private void BlinkCaret()
        {
            this.blinkTimer += Time.deltaTime;
            if (this.blinkTimer >= this.myField.caretBlinkRate)
            {
                this.blinkTimer = 0f;
                this.myCaret.gameObject.SetActive(!this.selectingText ? !this.myCaret.gameObject.activeSelf : true);
            }
        }

        private void CreateCaret()
        {
            GameObject obj2 = new GameObject("CurvedUICaret");
            obj2.AddComponent<RectTransform>();
            obj2.AddComponent<Image>();
            obj2.AddComponent<CurvedUIVertexEffect>();
            obj2.transform.SetParent(base.transform);
            obj2.transform.localScale = Vector3.one;
            (obj2.transform as RectTransform).anchoredPosition3D = Vector3.zero;
            (obj2.transform as RectTransform).pivot = new Vector2(0f, 1f);
            obj2.GetComponent<Image>().color = this.myField.caretColor;
            this.myCaret = obj2.transform as RectTransform;
            obj2.transform.SetAsFirstSibling();
            this.myField.customCaretColor = true;
            this.origCaretColor = this.myField.caretColor;
            this.myField.caretColor = new Color(0f, 0f, 0f, 0f);
            this.origSelectionColor = this.myField.selectionColor;
            this.myField.selectionColor = new Color(0f, 0f, 0f, 0f);
            obj2.gameObject.SetActive(false);
        }

        private Vector2 GetLocalPositionInText(int charNo)
        {
            if (!this.myField.isFocused)
            {
                return Vector2.zero;
            }
            TextGenerator cachedTextGenerator = this.myField.textComponent.cachedTextGenerator;
            if (charNo > (cachedTextGenerator.characterCount - 1))
            {
                charNo = cachedTextGenerator.characterCount - 1;
            }
            if (charNo > 0)
            {
                UICharInfo info = cachedTextGenerator.characters[charNo - 1];
                return new Vector2(((info.cursorPos.x + info.charWidth) / this.myField.textComponent.pixelsPerUnit) + this.lastCharDist, info.cursorPos.y / this.myField.textComponent.pixelsPerUnit);
            }
            UICharInfo info2 = cachedTextGenerator.characters[charNo];
            return new Vector2(info2.cursorPos.x / this.myField.textComponent.pixelsPerUnit, info2.cursorPos.y / this.myField.textComponent.pixelsPerUnit);
        }

        private void Update()
        {
            if (this.selected)
            {
                this.UpdateCaret();
            }
        }

        private void UpdateCaret()
        {
            if (this.myCaret == null)
            {
                this.CreateCaret();
            }
            Vector2 localPositionInText = this.GetLocalPositionInText(this.myField.caretPosition);
            if (this.myField.selectionFocusPosition == this.myField.selectionAnchorPosition)
            {
                this.selectingText = false;
                this.myCaret.sizeDelta = new Vector2((float) this.myField.caretWidth, (float) this.myField.textComponent.fontSize);
                this.myCaret.anchoredPosition = localPositionInText;
                this.myCaret.GetComponent<Image>().color = this.origCaretColor;
            }
            else
            {
                this.selectingText = true;
                Vector2 vector2 = new Vector2(this.GetLocalPositionInText(this.myField.selectionAnchorPosition).x - this.GetLocalPositionInText(this.myField.selectionFocusPosition).x, this.GetLocalPositionInText(this.myField.selectionAnchorPosition).y - this.GetLocalPositionInText(this.myField.selectionFocusPosition).y);
                localPositionInText = (vector2.x >= 0f) ? this.GetLocalPositionInText(this.myField.selectionFocusPosition) : this.GetLocalPositionInText(this.myField.selectionAnchorPosition);
                vector2 = new Vector2(Mathf.Abs(vector2.x), Mathf.Abs(vector2.y) + this.myField.textComponent.fontSize);
                this.myCaret.sizeDelta = new Vector2(vector2.x, vector2.y);
                this.myCaret.anchoredPosition = localPositionInText;
                this.myCaret.GetComponent<Image>().color = this.origSelectionColor;
            }
            this.BlinkCaret();
        }

        private bool selected =>
            (this.myField != null) && this.myField.isFocused;

        public Color CaretColor
        {
            get => 
                this.origCaretColor;
            set => 
                this.origCaretColor = value;
        }

        public Color SelectionColor
        {
            get => 
                this.origSelectionColor;
            set => 
                this.origSelectionColor = value;
        }

        public float CaretBlinkRate
        {
            get => 
                this.myField.caretBlinkRate;
            set => 
                this.myField.caretBlinkRate = value;
        }
    }
}

