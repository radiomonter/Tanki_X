using System;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationCodeSendAgainComponent : MonoBehaviour
{
    public LayoutElement buttonLayoutElement;
    public TextMeshProUGUI timerLabel;
    public Button sendAgainButton;
    private long lastRequestTicks;
    private long timer;
    private long emailSendThresholdInSeconds = 60;

    public void HideTimer()
    {
        this.sendAgainButton.interactable = true;
        this.buttonLayoutElement.preferredHeight = 50f;
        this.timerLabel.gameObject.SetActive(false);
    }

    public void ShowTimer(long emailSendThreshold)
    {
        this.sendAgainButton.interactable = false;
        this.lastRequestTicks = DateTime.Now.Ticks;
        this.emailSendThresholdInSeconds = emailSendThreshold;
        this.buttonLayoutElement.preferredHeight = 80f;
        this.timerLabel.gameObject.SetActive(true);
    }

    private void Update()
    {
        TimeSpan span = new TimeSpan(DateTime.Now.Ticks - this.lastRequestTicks);
        long totalSeconds = (long) span.TotalSeconds;
        long num3 = this.emailSendThresholdInSeconds - totalSeconds;
        if (num3 > 0L)
        {
            this.Timer = num3;
        }
        else if (this.timerLabel.gameObject.activeSelf)
        {
            this.HideTimer();
        }
    }

    public long Timer
    {
        get => 
            this.timer;
        set
        {
            this.timer = value;
            string str = LocalizationUtils.Localize("ec7ff56e-6fba-4947-87d0-a2a753c0974a").Replace("%seconds%", this.timer.ToString());
            if (this.timerLabel.text != str)
            {
                this.timerLabel.text = str;
            }
        }
    }
}

