using System;
using System.Runtime.CompilerServices;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerWithAction : MonoBehaviour
{
    [Header("Time To Action"), SerializeField]
    private float _startTime;
    [Header("Action"), SerializeField]
    private Button.ButtonClickedEvent _onTimeEndAction;
    [Header("Description"), SerializeField]
    private LocalizedField _actionDescription;
    [SerializeField]
    private TextMeshProUGUI _descriptionText;

    private void OnEnable()
    {
        if (this.CurrentTime <= this.StartTime)
        {
            this.CurrentTime = this.StartTime;
        }
    }

    private void Update()
    {
        if (this.CurrentTime > 0f)
        {
            if ((this.DescriptionText != null) && !string.IsNullOrEmpty(this.ActionDescription.Value))
            {
                this.DescriptionText.text = string.Format((string) this.ActionDescription, this.CurrentTime);
            }
            this.CurrentTime -= Time.deltaTime;
            if (this.CurrentTime <= 0f)
            {
                this.CurrentTime = 0f;
                this.OnTimeEndAction.Invoke();
            }
        }
    }

    public float StartTime
    {
        get => 
            this._startTime;
        set => 
            this._startTime = value;
    }

    public Button.ButtonClickedEvent OnTimeEndAction
    {
        get => 
            this._onTimeEndAction;
        set => 
            this._onTimeEndAction = value;
    }

    public LocalizedField ActionDescription
    {
        get => 
            this._actionDescription;
        set => 
            this._actionDescription = value;
    }

    public TextMeshProUGUI DescriptionText
    {
        get => 
            this._descriptionText;
        set => 
            this._descriptionText = value;
    }

    public float CurrentTime { get; set; }
}

