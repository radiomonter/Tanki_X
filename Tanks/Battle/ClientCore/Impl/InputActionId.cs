namespace Tanks.Battle.ClientCore.Impl
{
    using System;
    using UnityEngine;

    [Serializable]
    public class InputActionId
    {
        [InputType, SerializeField]
        public string actionTypeName;
        [InputName, SerializeField]
        public string actionName;

        public InputActionId(string actionTypeName, string actionName)
        {
            this.actionTypeName = actionTypeName;
            this.actionName = actionName;
        }

        public override string ToString() => 
            "actionTypeName: " + this.actionTypeName + ", actionName: " + this.actionName;
    }
}

