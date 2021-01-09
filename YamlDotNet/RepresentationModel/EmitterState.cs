namespace YamlDotNet.RepresentationModel
{
    using System;
    using System.Collections.Generic;

    internal class EmitterState
    {
        private readonly HashSet<string> emittedAnchors = new HashSet<string>();

        public HashSet<string> EmittedAnchors =>
            this.emittedAnchors;
    }
}

