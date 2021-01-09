namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;

    public class QiwiAccountFormatterComponent : BehaviourComponent
    {
        private ICollection<string> codes;
        [SerializeField]
        private List<int> spaces;
        protected TMP_InputField input;
        private bool formating;
        private Regex digitsOnly = new Regex(@"[^\d]");
        private Regex commonPhoneRegex = new Regex("^[1-9]{1}[0-9]{3,18}$");
        private StringBuilder formated = new StringBuilder(30);

        private void Awake()
        {
            this.input = base.GetComponent<TMP_InputField>();
            this.input.onValueChanged.AddListener(new UnityAction<string>(this.Format));
        }

        private string ClearFormat(string text) => 
            this.digitsOnly.Replace(text, string.Empty);

        [DebuggerHidden]
        private IEnumerator DelayFocus() => 
            new <DelayFocus>c__Iterator0 { $this = this };

        private void Format(string text)
        {
            if (!this.formating)
            {
                base.GetComponent<Animator>().SetBool("HasError", false);
                this.formating = true;
                this.formated.Length = 0;
                int num = this.input.stringPosition - this.GetSpacesBeforeCarret();
                text = this.ClearFormat(text);
                string str = string.Empty;
                foreach (string str2 in this.codes)
                {
                    if (text.StartsWith(str2) && (str.Length < str2.Length))
                    {
                        str = str2;
                        break;
                    }
                }
                int startIndex = 0 + str.Length;
                this.formated.Append(str);
                int num3 = 0;
                int num4 = 0;
                while (true)
                {
                    if (startIndex < text.Length)
                    {
                        if (num3 < this.spaces.Count)
                        {
                            int length = Math.Min(text.Length - startIndex, this.spaces[num3++]);
                            if (startIndex <= num)
                            {
                                num4++;
                            }
                            this.formated.Append(" ");
                            this.formated.Append(text.Substring(startIndex, length));
                            startIndex += length;
                            continue;
                        }
                        this.formated.Append(text.Substring(startIndex));
                    }
                    this.input.text = this.formated.ToString();
                    this.input.stringPosition = num + num4;
                    this.input.caretPosition = num + num4;
                    this.formating = false;
                    return;
                }
            }
        }

        private int GetSpacesBeforeCarret()
        {
            int num = 0;
            for (int i = 0; i < this.input.stringPosition; i++)
            {
                if (this.input.text[i] == ' ')
                {
                    num++;
                }
            }
            return num;
        }

        private void OnEnable()
        {
            base.StartCoroutine(this.DelayFocus());
        }

        public void SetCodes(ICollection<string> codes)
        {
            this.codes = codes;
        }

        public bool IsValidPhoneNumber
        {
            get
            {
                bool flag;
                string input = this.ClearFormat(this.input.text);
                if (this.codes == null)
                {
                    return false;
                }
                using (IEnumerator<string> enumerator = this.codes.GetEnumerator())
                {
                    while (true)
                    {
                        if (enumerator.MoveNext())
                        {
                            string current = enumerator.Current;
                            if (!input.StartsWith(current))
                            {
                                continue;
                            }
                            flag = this.commonPhoneRegex.IsMatch(input);
                        }
                        else
                        {
                            return false;
                        }
                        break;
                    }
                }
                return flag;
            }
        }

        public string Account =>
            "+" + this.ClearFormat(this.input.text);

        [CompilerGenerated]
        private sealed class <DelayFocus>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal QiwiAccountFormatterComponent $this;
            internal object $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.$current = new WaitForSeconds(0.1f);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                    {
                        int length = this.$this.input.text.Length;
                        this.$this.input.selectionFocusPosition = length;
                        this.$this.input.selectionAnchorPosition = length;
                        this.$PC = -1;
                        break;
                    }
                    default:
                        break;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }
    }
}

