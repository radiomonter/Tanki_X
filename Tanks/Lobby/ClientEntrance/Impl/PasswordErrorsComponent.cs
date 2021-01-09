namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;

    public class PasswordErrorsComponent : LocalizedControl, Component
    {
        public string PasswordContainsRestrictedSymbols { get; set; }

        public string PasswordIsTooSimple { get; set; }

        public string PasswordIsTooShort { get; set; }

        public string PasswordIsTooLong { get; set; }

        public string PasswordsDoNotMatch { get; set; }

        public string PasswordEqualsLogin { get; set; }
    }
}

