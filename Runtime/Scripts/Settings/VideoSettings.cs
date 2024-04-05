using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VideoSettings : MonoBehaviour
{
    [SerializeField] private bool loadOnAwake;
    [SerializeField] private bool applyOnAwake;

    [Header("Defaults")]
    [SerializeField] private int defaultQualitySettings = 2;
    [SerializeField] private int defaultFullScreenMode = 1;
    [SerializeField] private int defaultTargetFrameRate = 60;
    [SerializeField] private int defaultVSync = 1;

    [Space]

    [SerializeField] private bool showDebug;


    private readonly string savePrefix = "VideoSettings";
    private readonly string debugPrefix = "[VideoSettings]";

    private FullScreenMode cachedFullScreenMode;
    private Resolution cachedScreenResolution;
    private int screenResolutionIndex;
    private int cachedTargetFrameRate;
    private int cachedVSync;
    private int cachedQualitySettings;

    public UnityEvent<int> onGetQualitySettings;
    public UnityEvent<int> onGetFullScreenMode;
    public UnityEvent<string[]> onGetScreenResolutions = new UnityEvent<string[]>();
    public UnityEvent<int> onGetScreenResolution;
    public UnityEvent<int> onGetFPS;
    public UnityEvent<int> onGetVSync;

    private void Awake()
    {
        if(loadOnAwake) LoadSettings();
        if(applyOnAwake) ApplySettings();
    }

    public void LoadSettings()
    {
        // Screen Mode
        cachedFullScreenMode = (FullScreenMode)PlayerPrefs.GetInt(GetPlayerPrefKey("fullScreenMode"), defaultFullScreenMode);
        onGetFullScreenMode.Invoke((int)cachedFullScreenMode);

        // Screen resolution
        string[] screenR = new string[Screen.resolutions.Length];
        for(int i = 0; i < Screen.resolutions.Length; i++)
        {
            screenR[i] = Screen.resolutions[i].ToString();
        }
        if(screenR.Length > 0) onGetScreenResolutions?.Invoke(screenR);
        screenResolutionIndex = PlayerPrefs.GetInt(GetPlayerPrefKey("screenResolution"), -1);
        if(screenResolutionIndex == -1) screenResolutionIndex = Screen.resolutions.Length - 1;
        if(screenResolutionIndex < 0) screenResolutionIndex = 0;
        cachedScreenResolution = Screen.resolutions[screenResolutionIndex];
        onGetScreenResolution.Invoke(screenResolutionIndex);

        // Frames per second
        cachedTargetFrameRate = PlayerPrefs.GetInt(GetPlayerPrefKey("targetFrameRate"), defaultTargetFrameRate);
        onGetFPS.Invoke(cachedTargetFrameRate);

        // Vsync
        cachedVSync = PlayerPrefs.GetInt(GetPlayerPrefKey("vSync"), defaultVSync);
        onGetVSync.Invoke(cachedVSync);

        // Quality Settings
        cachedQualitySettings = PlayerPrefs.GetInt(GetPlayerPrefKey("qualitySettings"), defaultQualitySettings);
        onGetQualitySettings.Invoke(cachedQualitySettings);

        if(showDebug) Debug.Log($"{debugPrefix} Loaded Video Settings");
    }

    public void ApplySettings()
    {
        // Screen Mode
        ApplyScreenMode();

        // Screen resolution
        ApplyScreenResolution();

        // Frames per second
        ApplyTargetFrameRate();

        // Vsync
        ApplyVsync();

        // Quality Settings
        ApplyQualitySettings();

        if(showDebug) Debug.Log($"{debugPrefix} Applied Video Settings");
    }

    public void ResetSettings()
    {
        SetCachedScreenMode(defaultFullScreenMode);
        SetCachedResolution(-1);
        SetCachedTargetFrameRate(defaultTargetFrameRate);
        SetCachedVSync(defaultVSync);
        SetCachedQualitySettings(defaultQualitySettings);

        ApplySettings();
        LoadSettings();
    }

    #region Set

    /// <summary>
    /// ExclusiveFullScreen = 0, FullScreenWindow = 1, MaximizedWindow = 2, Windowed = 3
    /// </summary>
    /// <param name="value"></param>
    public void SetCachedScreenMode(int value)
    {
        cachedFullScreenMode = (FullScreenMode)value;
    }

    public void SetCachedResolution(int index)
    {
        screenResolutionIndex = index;
        if(index >= 0 && index < Screen.resolutions.Length) cachedScreenResolution = Screen.resolutions[index];
    }

    public void SetCachedTargetFrameRate(int value)
    {
        cachedTargetFrameRate = value;
    }

    /// <summary>
    /// Set vSync. 0 = default, Unity doesnt wait for vertical sync. Otherwise, value must be 1, 2, 3 or 4
    /// </summary>
    /// <param name="value"></param>
    public void SetCachedVSync(int value)
    {
        cachedVSync = value;
    }

    public void SetCachedQualitySettings(int value)
    {
        cachedQualitySettings = value;
    }

    private string GetPlayerPrefKey(string key)
    {
        return $"{Application.productName}_{savePrefix}_{key}";
    }

    #endregion Set

    #region Apply

    public void ApplyScreenMode()
    {
        Screen.fullScreenMode = cachedFullScreenMode;
        PlayerPrefs.SetInt(GetPlayerPrefKey("fullScreenMode"), (int)cachedFullScreenMode);

        if(showDebug) Debug.Log($"{debugPrefix} Apply fullscreen mode {cachedFullScreenMode}");
    }

    public void ApplyScreenResolution()
    {
        Screen.SetResolution(cachedScreenResolution.width, cachedScreenResolution.height, cachedFullScreenMode);
        PlayerPrefs.SetInt(GetPlayerPrefKey("screenResolution"), screenResolutionIndex);

        if(showDebug) Debug.Log($"{debugPrefix} Apply Screen Resolution: {cachedScreenResolution} with index: {screenResolutionIndex}");
    }

    public void ApplyTargetFrameRate()
    {
        Application.targetFrameRate = cachedTargetFrameRate;
        PlayerPrefs.SetInt(GetPlayerPrefKey("targetFrameRate"), cachedTargetFrameRate);

        if(showDebug) Debug.Log($"{debugPrefix} Apply Target Frame Rate: {cachedTargetFrameRate}");
    }

    public void ApplyVsync()
    {
        QualitySettings.vSyncCount = cachedVSync;
        PlayerPrefs.SetInt(GetPlayerPrefKey("vSync"), cachedVSync);

        if(showDebug) Debug.Log($"{debugPrefix} Apply vSync: {cachedVSync}");
    }

    public void ApplyQualitySettings()
    {
        QualitySettings.SetQualityLevel(cachedQualitySettings, true);
        PlayerPrefs.SetInt(GetPlayerPrefKey("qualitySettings"), cachedQualitySettings);

        if(showDebug) Debug.Log($"{debugPrefix} Apply QualitySettings: {cachedQualitySettings}");
    }

    #endregion
}
