namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using System;
    using System.Text.RegularExpressions;
    using Tanks.Lobby.ClientControls.API;

    public class CapsInputFormatter : BaseInputFormatter
    {
        private static Regex allowedSymbols = new Regex("[A-Za-z ]");

        protected override string ClearFormat(string text) => 
            text;

        protected override string FormatAt(char symbol, int charIndex)
        {
            string input = symbol.ToString();
            return (allowedSymbols.IsMatch(input) ? input.ToUpper() : string.Empty);
        }
    }
}

