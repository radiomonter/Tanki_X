using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using System;
using System.Runtime.CompilerServices;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.Impl;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TutorialIntroDialog : MonoBehaviour
{
    [SerializeField]
    protected AnimatedText animatedText;
    [SerializeField]
    private Button yesButton;
    [SerializeField]
    private Button noButton;
    [SerializeField]
    private Button sarcasmButton;
    [SerializeField]
    private Button startTutorial;
    [SerializeField]
    private Button skipTutorial;
    private float showTimer;
    [SerializeField]
    private LocalizedField yesText;
    [SerializeField]
    private LocalizedField introText;
    [SerializeField]
    private LocalizedField introWithoutQuestionText;
    [SerializeField]
    private LocalizedField confirmText;
    [SerializeField]
    private LocalizedField tipText;
    [SerializeField]
    private LocalizedField sarcasmText;
    private Entity tutorialStep;
    private bool canSkipTutorial;

    private void ContinueTutorial()
    {
        TutorialCanvas.Instance.Hide();
    }

    private void DisableTutorials()
    {
        if (this.tutorialStep != null)
        {
            EngineService.Engine.ScheduleEvent<SkipAllTutorialsEvent>(this.tutorialStep);
            this.tutorialStep = null;
            TutorialCanvas.Instance.Hide();
        }
    }

    public void Show(Entity tutorialStep, bool canSkipTutorial)
    {
        this.tutorialStep = tutorialStep;
        this.canSkipTutorial = canSkipTutorial;
        this.animatedText.ResultText = !canSkipTutorial ? this.introWithoutQuestionText.Value : this.introText.Value;
        this.animatedText.Animate();
        this.showTimer = 0f;
        base.gameObject.SetActive(true);
        this.yesButton.gameObject.SetActive(true);
        this.yesButton.GetComponentInChildren<TextMeshProUGUI>().text = !canSkipTutorial ? "ok" : this.yesText.Value;
        this.noButton.gameObject.SetActive(canSkipTutorial);
        this.sarcasmButton.gameObject.SetActive(true);
        this.startTutorial.gameObject.SetActive(false);
        this.skipTutorial.gameObject.SetActive(false);
        this.yesButton.onClick.RemoveAllListeners();
        this.yesButton.onClick.AddListener(new UnityAction(this.ContinueTutorial));
        this.noButton.onClick.RemoveAllListeners();
        this.noButton.onClick.AddListener(new UnityAction(this.ShowConfirmText));
        this.sarcasmButton.onClick.RemoveAllListeners();
        this.sarcasmButton.onClick.AddListener(new UnityAction(this.ShowSarcasm));
        this.startTutorial.onClick.RemoveAllListeners();
        this.startTutorial.onClick.AddListener(new UnityAction(this.ContinueTutorial));
        this.skipTutorial.onClick.RemoveAllListeners();
        this.skipTutorial.onClick.AddListener(new UnityAction(this.DisableTutorials));
        this.yesButton.interactable = true;
        this.noButton.interactable = true;
        this.sarcasmButton.interactable = true;
        this.sarcasmButton.interactable = true;
        this.startTutorial.interactable = true;
        this.skipTutorial.interactable = true;
    }

    private void ShowConfirmText()
    {
        if (this.canSkipTutorial)
        {
            this.yesButton.gameObject.SetActive(false);
            this.noButton.gameObject.SetActive(false);
            this.sarcasmButton.gameObject.SetActive(false);
            this.animatedText.ResultText = this.confirmText.Value + "\n\n<color=#A0A0A0>" + this.tipText.Value;
            this.animatedText.Animate();
            this.startTutorial.gameObject.SetActive(true);
            this.skipTutorial.gameObject.SetActive(true);
        }
    }

    private void ShowSarcasm()
    {
        this.sarcasmButton.gameObject.SetActive(false);
        this.animatedText.ResultText = this.sarcasmText.Value;
        this.animatedText.Animate();
    }

    public void TutorialHidden()
    {
        base.gameObject.SetActive(false);
        Entity tutorialStep = this.tutorialStep;
        this.tutorialStep = null;
        if (tutorialStep != null)
        {
            EngineService.Engine.ScheduleEvent<TutorialStepCompleteEvent>(tutorialStep);
        }
    }

    [Inject]
    public static EngineServiceInternal EngineService { get; set; }
}

