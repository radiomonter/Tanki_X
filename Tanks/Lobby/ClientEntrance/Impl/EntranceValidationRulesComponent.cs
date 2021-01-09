namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;

    public class EntranceValidationRulesComponent : Component
    {
        private static readonly Regex MATCH_EVERYTHING = new Regex(".+");
        private static readonly Regex MATCH_NOTHING = new Regex("(?!)");
        private Regex loginRegex = MATCH_EVERYTHING;
        private Regex loginSymbolsRegex = MATCH_EVERYTHING;
        private Regex loginBeginingRegex = MATCH_EVERYTHING;
        private Regex loginEndingRegex = MATCH_EVERYTHING;
        private Regex loginSpecTogetherRegex = MATCH_NOTHING;
        private Regex emailRegex = MATCH_NOTHING;
        private Regex passwordRegex = MATCH_EVERYTHING;

        public bool AreSpecSymbolsTogether(string login) => 
            this.loginSpecTogetherRegex.IsMatch(login);

        public bool IsEmailValid(string email) => 
            this.emailRegex.IsMatch(email);

        public bool IsLoginBeginingValid(string login) => 
            this.loginBeginingRegex.IsMatch(login);

        public bool IsLoginEndingValid(string login) => 
            this.loginEndingRegex.IsMatch(login);

        public bool IsLoginSymbolsValid(string login) => 
            this.loginSymbolsRegex.IsMatch(login);

        public bool IsLoginTooLong(string login) => 
            login.Length > this.maxLoginLength;

        public bool IsLoginTooShort(string login) => 
            login.Length < this.minLoginLength;

        public bool IsLoginValid(string login) => 
            (!this.IsLoginTooShort(login) && (!this.IsLoginTooLong(login) && (this.IsLoginSymbolsValid(login) && (this.IsLoginBeginingValid(login) && (this.IsLoginEndingValid(login) && !this.AreSpecSymbolsTogether(login)))))) && this.loginRegex.IsMatch(login);

        public bool IsPasswordSymbolsValid(string password) => 
            this.passwordRegex.IsMatch(password);

        public bool IsPasswordTooLong(string password) => 
            password.Length > this.maxPasswordLength;

        public bool IsPasswordTooShort(string password) => 
            password.Length < this.minPasswordLength;

        public int minLoginLength { get; set; }

        public int maxLoginLength { get; set; }

        public int minPasswordLength { get; set; }

        public int maxPasswordLength { get; set; }

        public int maxCaptchaLength { get; set; }

        public int minEmailLength { get; set; }

        public int maxEmailLength { get; set; }

        public string LoginRegex
        {
            get => 
                this.loginRegex.ToString();
            set => 
                this.loginRegex = new Regex(value);
        }

        public string LoginSymbolsRegex
        {
            get => 
                this.loginSymbolsRegex.ToString();
            set => 
                this.loginSymbolsRegex = new Regex(value);
        }

        public string LoginBeginingRegex
        {
            get => 
                this.loginBeginingRegex.ToString();
            set => 
                this.loginBeginingRegex = new Regex(value);
        }

        public string LoginEndingRegex
        {
            get => 
                this.loginEndingRegex.ToString();
            set => 
                this.loginEndingRegex = new Regex(value);
        }

        public string LoginSpecTogetherRegex
        {
            get => 
                this.loginSpecTogetherRegex.ToString();
            set => 
                this.loginSpecTogetherRegex = new Regex(value);
        }

        public string PasswordRegex
        {
            get => 
                this.passwordRegex.ToString();
            set => 
                this.passwordRegex = new Regex(value);
        }

        public string EmailRegex
        {
            get => 
                this.emailRegex.ToString();
            set => 
                this.emailRegex = new Regex(value);
        }
    }
}

