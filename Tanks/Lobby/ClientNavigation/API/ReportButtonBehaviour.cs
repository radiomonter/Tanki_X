namespace Tanks.Lobby.ClientNavigation.API
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    [RequireComponent(typeof(Button))]
    public class ReportButtonBehaviour : MonoBehaviour
    {
        [SerializeField]
        private string defaultReportUrl;

        private void Awake()
        {
            this.ReportUrl = this.defaultReportUrl;
            base.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OpenUrl));
        }

        private void OpenUrl()
        {
            Application.OpenURL(this.ReportUrl);
        }

        public string ReportUrl { get; set; }
    }
}

