namespace Tanks.Battle.ClientCore.API
{
    using System;

    public class LayerMasksValidationException : ArgumentOutOfRangeException
    {
        public LayerMasksValidationException(string message) : base(message)
        {
        }
    }
}

