namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public class TMPInputFormatter : BaseTMPInputFormatter
    {
        [SerializeField]
        private List<int> spacePositions;
        [SerializeField]
        private string spaceChar;

        public TMPInputFormatter()
        {
            List<int> list = new List<int> { 
                4,
                8,
                12
            };
            this.spacePositions = list;
            this.spaceChar = " ";
        }

        protected override string ClearFormat(string text) => 
            !string.IsNullOrEmpty(this.spaceChar) ? text.Replace(this.spaceChar, string.Empty) : text;

        protected override string FormatAt(char symbol, int charIndex) => 
            char.IsDigit(symbol) ? (!this.spacePositions.Contains(charIndex) ? symbol.ToString() : (this.spaceChar + symbol)) : string.Empty;
    }
}

