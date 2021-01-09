namespace Tanks.Lobby.ClientLoading.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.System.Data.Statics.ClientConfigurator.API;
    using Platform.System.Data.Statics.ClientYaml.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class BattleHintsComponent : MonoBehaviour, Component
    {
        public string hintsConfig;
        private Text text;
        private List<string> hints;
        private int currentHintIndex;
        private int updateTimeInSec;
        private float lastChangeHintTime;

        private void Awake()
        {
            this.text = base.GetComponent<Text>();
            this.ParseConfig();
            this.currentHintIndex = -1;
            this.SetNextHintText();
        }

        private void OnDisable()
        {
            if ((Time.realtimeSinceStartup - this.lastChangeHintTime) > 2f)
            {
                this.SetNextHintText();
            }
        }

        private void OnEnable()
        {
            this.lastChangeHintTime = Time.realtimeSinceStartup;
        }

        private void ParseConfig()
        {
            YamlNode childNode = ConfigurationService.GetConfig(this.hintsConfig).GetChildNode("battleHints");
            this.hints = childNode.GetChildListValues("collection");
            for (int i = 0; i < this.hints.Count; i++)
            {
                char[] trimChars = new char[] { '\n' };
                this.hints[i] = this.hints[i].TrimEnd(trimChars);
            }
            this.updateTimeInSec = int.Parse(childNode.GetStringValue("updateTimeInSec"));
        }

        private void SetNextHintText()
        {
            this.currentHintIndex = Random.Range(0, this.hints.Count);
            this.text.text = this.hints[this.currentHintIndex];
        }

        private void Update()
        {
            if (((this.hints != null) && (this.hints.Count > 1)) && ((Time.realtimeSinceStartup - this.lastChangeHintTime) >= this.updateTimeInSec))
            {
                this.SetNextHintText();
                this.lastChangeHintTime = Time.realtimeSinceStartup;
            }
        }

        [Inject]
        public static Platform.System.Data.Statics.ClientConfigurator.API.ConfigurationService ConfigurationService { get; set; }
    }
}

