namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;

    public class PauseGUIComponent : MonoBehaviour, Component
    {
        private void OnGUI()
        {
            if (this.ShowMessage)
            {
                Vector3 vector = new Vector3(500f, 200f);
                Rect screenRect = new Rect((((float) Screen.width) / 2f) - (vector.x / 2f), (((float) Screen.height) / 2f) - (vector.y / 2f), vector.x, vector.y);
                GUILayout.BeginArea(screenRect);
                GUILayout.BeginVertical(new GUILayoutOption[0]);
                GUILayout.Label(this.MessageText, ClientGraphicsActivator.guiStyle, new GUILayoutOption[0]);
                GUILayout.EndVertical();
                GUILayout.EndArea();
            }
        }

        public bool ShowMessage { get; set; }

        public string MessageText { get; set; }
    }
}

