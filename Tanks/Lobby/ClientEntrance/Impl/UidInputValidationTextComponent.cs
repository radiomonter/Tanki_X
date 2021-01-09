namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;

    public class UidInputValidationTextComponent : LocalizedControl, Component
    {
        public string LoginContainsRestrictedSymbols { get; set; }

        public string LoginAlreadyInUse { get; set; }

        public string LoginIsTooShort { get; set; }

        public string LoginIsTooLong { get; set; }

        public string LoginContainsSpecialSymbolsInARow { get; set; }

        public string LoginBeginsWithSpecialSymbol { get; set; }

        public string LoginEndsWithSpecialSymbol { get; set; }
    }
}

