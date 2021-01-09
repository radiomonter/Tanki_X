using Steamworks;
using System;
using System.Text;
using UnityEngine;

[DisallowMultipleComponent]
public class SteamManager : MonoBehaviour
{
    private static SteamManager s_instance;
    private static bool s_EverInialized;
    private bool m_bInitialized;
    private SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;

    private void Awake()
    {
        if (s_instance != null)
        {
            Destroy(base.gameObject);
        }
        else
        {
            s_instance = this;
            if (s_EverInialized)
            {
                throw new Exception("Tried to Initialize the SteamAPI twice in one session!");
            }
            DontDestroyOnLoad(base.gameObject);
            if (!Packsize.Test())
            {
                Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
            }
            if (!DllCheck.Test())
            {
                Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
            }
            try
            {
                if (SteamAPI.RestartAppIfNecessary(AppId_t.Invalid))
                {
                    Application.Quit();
                    return;
                }
            }
            catch (DllNotFoundException exception)
            {
                Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + exception, this);
                Application.Quit();
                return;
            }
            this.m_bInitialized = SteamAPI.Init();
            if (!this.m_bInitialized)
            {
                Debug.Log("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);
            }
            else
            {
                s_EverInialized = true;
            }
        }
    }

    private void OnDestroy()
    {
        if (s_instance == this)
        {
            s_instance = null;
            if (this.m_bInitialized)
            {
                SteamAPI.Shutdown();
            }
        }
    }

    private void OnEnable()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
        if (this.m_bInitialized && (this.m_SteamAPIWarningMessageHook == null))
        {
            this.m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.SteamAPIDebugTextHook);
            SteamClient.SetWarningMessageHook(this.m_SteamAPIWarningMessageHook);
        }
    }

    private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
    {
        Debug.LogWarning(pchDebugText);
    }

    private void Update()
    {
        if (this.m_bInitialized)
        {
            SteamAPI.RunCallbacks();
        }
    }

    private static SteamManager Instance =>
        (s_instance != null) ? s_instance : new GameObject("SteamManager").AddComponent<SteamManager>();

    public static bool Initialized =>
        Instance.m_bInitialized;
}

