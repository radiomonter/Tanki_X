using System;
using UnityEngine;

public class HUDFPS : MonoBehaviour
{
    public KeyCode toggleKey = KeyCode.F8;
    public bool show = true;
    public Rect startRect = new Rect(10f, 10f, 75f, 50f);
    public bool updateColor = true;
    public bool allowDrag = true;
    public float frequency = 0.5f;
    public int nbDecimal = 1;
    public bool limitFrameRate;
    public int frameRate = 60;
    private float accum;
    private int frames;
    private Color color = Color.white;
    private string sFPS = string.Empty;
    private GUIStyle style;
    private float updateTimer;

    private void CalcCurrentFPS()
    {
        float num = this.accum / ((float) this.frames);
        this.sFPS = num.ToString("f" + Mathf.Clamp(this.nbDecimal, 0, 10)) + " FPS";
        this.color = (num < 30f) ? ((num <= 10f) ? Color.yellow : Color.red) : Color.green;
        this.accum = 0f;
        this.frames = 0;
    }

    private void DoMyWindow(int windowID)
    {
        GUI.Label(new Rect(0f, 0f, this.startRect.width, this.startRect.height), this.sFPS, this.style);
        if (this.allowDrag)
        {
            GUI.DragWindow(new Rect(0f, 0f, (float) Screen.width, (float) Screen.height));
        }
    }

    private void OnGUI()
    {
        if (this.show)
        {
            if (this.style == null)
            {
                this.style = new GUIStyle(GUI.skin.label);
                this.style.normal.textColor = Color.white;
                this.style.alignment = TextAnchor.MiddleCenter;
            }
            GUI.color = !this.updateColor ? Color.white : this.color;
            this.startRect = GUI.Window(0, this.startRect, new GUI.WindowFunction(this.DoMyWindow), string.Empty);
        }
    }

    private void Start()
    {
        if (this.limitFrameRate)
        {
            Application.targetFrameRate = this.frameRate;
        }
        this.updateTimer = this.frequency;
    }

    private void Update()
    {
        if (Input.GetKeyUp(this.toggleKey))
        {
            this.show = !this.show;
            this.accum = 0f;
            this.frames = 0;
            this.updateTimer = this.frequency;
        }
        if (this.show)
        {
            this.accum += Time.timeScale / Time.deltaTime;
            this.frames++;
            this.updateTimer -= Time.deltaTime;
            if (this.updateTimer <= 0f)
            {
                this.CalcCurrentFPS();
                this.updateTimer = this.frequency;
            }
        }
    }
}

