using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CurvedUIInputModule : StandaloneInputModule
{
    [SerializeField]
    private CUIControlMethod controlMethod;
    [SerializeField]
    private string submitButtonName = "Fire1";
    [SerializeField]
    private bool gazeUseTimedClick;
    [SerializeField]
    private float gazeClickTimer = 2f;
    [SerializeField]
    private float gazeClickTimerDelay = 1f;
    [SerializeField]
    private Image gazeTimedClickProgressImage;
    [SerializeField]
    private float worldSpaceMouseSensitivity = 1f;
    [SerializeField]
    private Hand usedHand = Hand.Right;
    private static bool disableOtherInputModulesOnStart = true;
    private static CurvedUIInputModule instance;
    private GameObject currentDragging;
    private GameObject currentPointedAt;
    private float gazeTimerProgress;
    private Ray customControllerRay;
    private float dragThreshold = 10f;
    private bool pressedDown;
    private bool pressedLastFrame;
    private Vector3 lastMouseOnScreenPos = ((Vector3) Vector2.zero);
    private Vector2 worldSpaceMouseInCanvasSpace = Vector2.zero;
    private Vector2 lastWorldSpaceMouseOnCanvas = Vector2.zero;
    private Vector2 worldSpaceMouseOnCanvasDelta = Vector2.zero;

    protected override void Awake()
    {
        if (Application.isPlaying)
        {
            Instance = this;
            base.Awake();
        }
    }

    private static T EnableInputModule<T>() where T: BaseInputModule
    {
        bool flag = true;
        EventSystem system = FindObjectOfType<EventSystem>();
        if (system == null)
        {
            Debug.LogError("CurvedUI: Your EventSystem component is missing from the scene! Unity Canvas will not track interactions without it.");
            return null;
        }
        foreach (BaseInputModule module in system.GetComponents<BaseInputModule>())
        {
            if (module is T)
            {
                flag = false;
                module.enabled = true;
            }
            else if (disableOtherInputModulesOnStart)
            {
                module.enabled = false;
            }
        }
        if (flag)
        {
            system.gameObject.AddComponent<T>();
        }
        return system.GetComponent<T>();
    }

    public override void Process()
    {
        switch (this.controlMethod)
        {
            case CUIControlMethod.GAZE:
                this.ProcessGaze();
                break;

            case CUIControlMethod.WORLD_MOUSE:
                if (Input.touchCount > 0)
                {
                    this.worldSpaceMouseOnCanvasDelta = Input.GetTouch(0).deltaPosition * this.worldSpaceMouseSensitivity;
                }
                else
                {
                    this.worldSpaceMouseOnCanvasDelta = new Vector2((Input.mousePosition - this.lastMouseOnScreenPos).x, (Input.mousePosition - this.lastMouseOnScreenPos).y) * this.worldSpaceMouseSensitivity;
                    this.lastMouseOnScreenPos = Input.mousePosition;
                }
                this.lastWorldSpaceMouseOnCanvas = this.worldSpaceMouseInCanvasSpace;
                this.worldSpaceMouseInCanvasSpace += this.worldSpaceMouseOnCanvasDelta;
                base.Process();
                break;

            case CUIControlMethod.CUSTOM_RAY:
                this.ProcessCustomRayController();
                break;

            case CUIControlMethod.VIVE:
                this.ProcessViveControllers();
                break;

            case CUIControlMethod.OCULUS_TOUCH:
                this.ProcessOculusTouchController();
                break;

            default:
                base.Process();
                break;
        }
    }

    protected virtual void ProcessCustomRayController()
    {
        PointerEventData buttonData = this.GetMousePointerEventData(0).GetButtonState(PointerEventData.InputButton.Left).eventData.buttonData;
        base.SendUpdateEventToSelectedObject();
        PointerEventData eventData = buttonData;
        this.currentPointedAt = eventData.pointerCurrentRaycast.gameObject;
        this.ProcessDownRelease(eventData, this.pressedDown && !this.pressedLastFrame, !this.pressedDown && this.pressedLastFrame);
        this.ProcessMove(eventData);
        if (this.pressedDown)
        {
            this.ProcessDrag(eventData);
            if (!Mathf.Approximately(eventData.scrollDelta.sqrMagnitude, 0f))
            {
                ExecuteEvents.ExecuteHierarchy<IScrollHandler>(ExecuteEvents.GetEventHandler<IScrollHandler>(eventData.pointerCurrentRaycast.gameObject), eventData, ExecuteEvents.scrollHandler);
            }
        }
        this.pressedLastFrame = this.pressedDown;
    }

    protected virtual void ProcessDownRelease(PointerEventData eventData, bool down, bool released)
    {
        GameObject gameObject = eventData.pointerCurrentRaycast.gameObject;
        if (down)
        {
            eventData.eligibleForClick = true;
            eventData.delta = Vector2.zero;
            eventData.dragging = false;
            eventData.useDragThreshold = true;
            eventData.pressPosition = eventData.position;
            eventData.pointerPressRaycast = eventData.pointerCurrentRaycast;
            base.DeselectIfSelectionChanged(gameObject, eventData);
            if (eventData.pointerEnter != gameObject)
            {
                base.HandlePointerExitAndEnter(eventData, gameObject);
                eventData.pointerEnter = gameObject;
            }
            GameObject eventHandler = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject, eventData, ExecuteEvents.pointerDownHandler);
            if (eventHandler == null)
            {
                eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
            }
            float unscaledTime = Time.unscaledTime;
            if (eventHandler != eventData.lastPress)
            {
                eventData.clickCount = 1;
            }
            else
            {
                eventData.clickCount = ((unscaledTime - eventData.clickTime) >= 0.3f) ? 1 : (eventData.clickCount + 1);
                eventData.clickTime = unscaledTime;
            }
            eventData.pointerPress = eventHandler;
            eventData.rawPointerPress = gameObject;
            eventData.clickTime = unscaledTime;
            eventData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
            if (eventData.pointerDrag != null)
            {
                ExecuteEvents.Execute<IInitializePotentialDragHandler>(eventData.pointerDrag, eventData, ExecuteEvents.initializePotentialDrag);
            }
        }
        if (released)
        {
            ExecuteEvents.Execute<IPointerUpHandler>(eventData.pointerPress, eventData, ExecuteEvents.pointerUpHandler);
            GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
            if ((eventData.pointerPress == eventHandler) && eventData.eligibleForClick)
            {
                ExecuteEvents.Execute<IPointerClickHandler>(eventData.pointerPress, eventData, ExecuteEvents.pointerClickHandler);
            }
            else if ((eventData.pointerDrag != null) && eventData.dragging)
            {
                ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject, eventData, ExecuteEvents.dropHandler);
            }
            eventData.eligibleForClick = false;
            eventData.pointerPress = null;
            eventData.rawPointerPress = null;
            if ((eventData.pointerDrag != null) && eventData.dragging)
            {
                ExecuteEvents.Execute<IEndDragHandler>(eventData.pointerDrag, eventData, ExecuteEvents.endDragHandler);
            }
            eventData.dragging = false;
            eventData.pointerDrag = null;
            ExecuteEvents.ExecuteHierarchy<IPointerExitHandler>(eventData.pointerEnter, eventData, ExecuteEvents.pointerExitHandler);
            eventData.pointerEnter = null;
        }
    }

    protected virtual void ProcessGaze()
    {
        bool flag = base.SendUpdateEventToSelectedObject();
        if (base.eventSystem.sendNavigationEvents && !(flag | base.SendMoveEventToSelectedObject()))
        {
            base.SendSubmitEventToSelectedObject();
        }
        base.ProcessMouseEvent();
    }

    protected virtual void ProcessOculusTouchController()
    {
    }

    protected virtual void ProcessViveControllers()
    {
    }

    protected override void Start()
    {
        if (Application.isPlaying)
        {
            base.Start();
        }
    }

    public static CurvedUIInputModule Instance
    {
        get
        {
            if (instance == null)
            {
                instance = EnableInputModule<CurvedUIInputModule>();
            }
            return instance;
        }
        private set => 
            instance = value;
    }

    public static Ray CustomControllerRay
    {
        get => 
            Instance.customControllerRay;
        set => 
            Instance.customControllerRay = value;
    }

    [Obsolete("Misspelled. Use CustomControllerButtonDown instead.")]
    public static bool CustromControllerButtonDown
    {
        get => 
            Instance.pressedDown;
        set => 
            Instance.pressedDown = value;
    }

    public static bool CustomControllerButtonDown
    {
        get => 
            Instance.pressedDown;
        set => 
            Instance.pressedDown = value;
    }

    public Vector2 WorldSpaceMouseInCanvasSpace
    {
        get => 
            this.worldSpaceMouseInCanvasSpace;
        set
        {
            this.worldSpaceMouseInCanvasSpace = value;
            this.lastWorldSpaceMouseOnCanvas = value;
        }
    }

    public Vector2 WorldSpaceMouseInCanvasSpaceDelta =>
        this.worldSpaceMouseInCanvasSpace - this.lastWorldSpaceMouseOnCanvas;

    public float WorldSpaceMouseSensitivity
    {
        get => 
            this.worldSpaceMouseSensitivity;
        set => 
            this.worldSpaceMouseSensitivity = value;
    }

    public static CUIControlMethod ControlMethod
    {
        get => 
            Instance.controlMethod;
        set
        {
            if (Instance.controlMethod != value)
            {
                Instance.controlMethod = value;
            }
        }
    }

    public GameObject CurrentPointedAt =>
        this.currentPointedAt;

    public Hand UsedHand
    {
        get => 
            this.usedHand;
        set => 
            this.usedHand = value;
    }

    public bool GazeUseTimedClick
    {
        get => 
            this.gazeUseTimedClick;
        set => 
            this.gazeUseTimedClick = value;
    }

    public float GazeClickTimer
    {
        get => 
            this.gazeClickTimer;
        set => 
            this.gazeClickTimer = Mathf.Max(value, 0f);
    }

    public float GazeClickTimerDelay
    {
        get => 
            this.gazeClickTimerDelay;
        set => 
            this.gazeClickTimerDelay = Mathf.Max(value, 0f);
    }

    public float GazeTimerProgress =>
        this.gazeTimerProgress;

    public Image GazeTimedClickProgressImage
    {
        get => 
            this.gazeTimedClickProgressImage;
        set => 
            this.gazeTimedClickProgressImage = value;
    }

    public enum CUIControlMethod
    {
        MOUSE = 0,
        GAZE = 1,
        WORLD_MOUSE = 2,
        CUSTOM_RAY = 3,
        VIVE = 4,
        OCULUS_TOUCH = 5,
        GOOGLEVR = 7
    }

    public enum Hand
    {
        Both,
        Right,
        Left
    }
}

