namespace Tanks.Lobby.ClientNavigation.API
{
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class FatalErrorScreenBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Text header;
        [SerializeField]
        private Text text;
        [SerializeField]
        private TextMeshProUGUI restart;
        [SerializeField]
        private TextMeshProUGUI quit;
        [SerializeField]
        private TextMeshProUGUI report;
        [SerializeField]
        private ReportButtonBehaviour reportButtonBehaviour;

        private void Start()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            if (ErrorScreenData.data != null)
            {
                this.header.text = ErrorScreenData.data.HeaderText;
                this.text.text = ErrorScreenData.data.ErrorText;
                this.restart.text = ErrorScreenData.data.RestartButtonLabel;
                this.quit.text = ErrorScreenData.data.ExitButtonLabel;
                this.report.text = ErrorScreenData.data.ReportButtonLabel;
                if (ErrorScreenData.data.ReConnectTime > 0)
                {
                    base.gameObject.AddComponent<ReConnectBehaviour>().ReConnectTime = ErrorScreenData.data.ReConnectTime;
                    this.restart.transform.parent.gameObject.SetActive(false);
                }
            }
        }
    }
}

