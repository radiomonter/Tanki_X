namespace Tanks.Battle.ClientCore.Impl
{
    using System;
    using UnityEngine;

    [Serializable]
    public class InputActionContextId
    {
        [InputType, SerializeField]
        public string contextTypeName;
        [InputName, SerializeField]
        public string contextName = BasicContexts.BATTLE_CONTEXT;

        public InputActionContextId(string contextTypeName)
        {
            this.contextTypeName = contextTypeName;
        }

        public override bool Equals(object obj)
        {
            InputActionContextId id = (InputActionContextId) obj;
            return (id.contextName.Equals(this.contextName) && id.contextTypeName.Equals(this.contextTypeName));
        }

        public override string ToString() => 
            "contextTypeName: " + this.contextTypeName + ", contextName: " + this.contextName;
    }
}

