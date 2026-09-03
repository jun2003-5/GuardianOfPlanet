using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Unity.Services.RemoteConfig;
using Unity.Services.Authentication;
using Unity.Services.Core;

/// <summary>
/// Check if the app is updated or not
/// </summary>
public class UpdateChecker : MonoBehaviour
{

    #region Editor

    [Header("Options")]

    /// <summary>
    /// Checks when starting the app
    /// </summary>
    [Tooltip("Checks when starting the app")]
    [SerializeField]
    protected bool _checkOnStart = true;

    [Header("Android")]

    /// <summary>
    /// Android version key in Remote Settings
    /// </summary>
    [Tooltip("Android version key in Remote Settings")]
    [SerializeField]
    protected string _keyAndroid = "versionAndroid";
    /// <summary>
    /// Play Store app ID
    /// </summary>
    [Tooltip("Play Store app ID")]
    [SerializeField]
    protected string _playstoreID = "games.versionzero.app";

    [Header("iOS")]

    /// <summary>
    /// iOS version key in Remote Settings
    /// </summary>
    [Tooltip("iOS version key in Remote Settings")]
    [SerializeField]
    protected string _keyIOS = "versioniOS";
    /// <summary>
    /// ITunes app ID
    /// </summary>
    [Tooltip("ITunes app ID")]
    [SerializeField]
    protected string _itunesID = "games.versionzero.app";

    [Header("Events")]

    /// <summary>
    /// Event fired if the app is out of date
    /// </summary>
    [Tooltip("Event fired if the app is out of date")]
    [SerializeField]
    public GameObject _onOutdatedTab;

    #endregion
    public struct userAttributes { }
    public struct appAttributes { }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before any of the Update methods are called the first time
    /// </summary>

    public void Start()
    {
        InvokeRepeating("checkingProcess", 0f, 10f);
    }

    async Task checkingProcess()
    {
        if(_checkOnStart) {
            RemoteConfigService.Instance.FetchCompleted += Check;
            await RemoteConfigService.Instance.FetchConfigsAsync(new userAttributes(), new appAttributes());
        }
    }

    /// <summary>
    /// Check the version of the app
    /// </summary>
    public void Check(ConfigResponse configResponse)
    {
        Version currentVersion = new Version(Application.version);
        Version checkVersion = null;

#if UNITY_ANDROID
            if(RemoteConfigService.Instance.appConfig.HasKey(_keyAndroid))
                checkVersion = new Version(RemoteConfigService.Instance.appConfig.GetString(_keyAndroid));
#elif UNITY_IOS
        if(RemoteConfigService.Instance.appConfig.HasKey(_keyIOS))
            checkVersion = new Version(RemoteConfigService.Instance.appConfig.GetString(_keyIOS));
#endif

        if(checkVersion != null) {
            if(currentVersion < checkVersion) {
                GameManager.instance.PauseGame();
                _onOutdatedTab.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Redirects to update the app
    /// </summary>
    public void UpdateApp()
    {
#if UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.aragames.guardianofplanet");
#elif UNITY_IPHONE
        Application.OpenURL("https://apps.apple.com/us/app/%EA%B0%80%EB%94%94%EC%96%B8-%EC%98%A4%EB%B8%8C-%ED%94%8C%EB%9E%98%EB%8B%9B-%EB%B0%A9%EC%B9%98%ED%98%95-rpg/id6483863042");
#endif
    }

}