using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

using Unity.Services.LevelPlay;
using System;


public class AdsManager : MonoBehaviour
{
    #region VARIABLES

    [Header("Debug")]
    [SerializeField] private bool _initialized = false;
    [SerializeField] private bool _bannerLoaded = false;


    [Space]
    [Header("Configuración Ads")]
    [SerializeField] private ObscuredString androidAppKey = "";
    [SerializeField] private ObscuredString iosAppKey = "";
    private ObscuredString appKey;

    LevelPlayBannerAd _bannerAd;
    LevelPlayInterstitialAd _interstitialAd; // = new LevelPlayInterstitialAd(ADS_PLACEMENT.Ads_Placement_Insterticial);
    LevelPlayRewardedAd rewardedAd; // = new LevelPlayRewardedAd(ADS_PLACEMENT.Ads_Placement_Rewarded);

    #endregion

    #region EVENTOS
    public delegate void AdEvent(string _value);

    public static AdEvent OnInitializationDone;
    public static AdEvent OnInitializationFailed;

    public static AdEvent OnReadyRewardedVideoAd;
    public static AdEvent OnErrorRewardedVideoAd;
    public static AdEvent OnStartRewardedVideoAd;
    public static AdEvent OnFinishedRewardedVideoAd;
    public static AdEvent OnSkippedRewardedVideoAd;
    public static AdEvent OnFailedRewardedVideoAd;
    public static AdEvent OnNoRewardedVideoAd;

    public static AdEvent OnReadyInterstitialVideoAd;
    public static AdEvent OnErrorInterstitialVideoAd;
    public static AdEvent OnStartInterstitialVideoAd;
    public static AdEvent OnFinishedInterstitialVideoAd;
    public static AdEvent OnSkippedInterstitialVideoAd;
    public static AdEvent OnClosedInterstitialVideoAd;
    public static AdEvent OnFailedInterstitialVideoAd;
    public static AdEvent OnNoInterstitialVideoAd;


    //INICIALIZACIÓN
    public static void InitializationDone(string _value)
    {
        OnInitializationDone?.Invoke(_value);
    }

    public static void InitializationFailed(string _value)
    {
        OnInitializationFailed?.Invoke(_value);
    }

    //REWARDED VIDEO
    public static void ReadyRewardedVideoAd(string _value)
    {
        OnReadyRewardedVideoAd?.Invoke(_value);
    }
    public static void ErrorRewardedVideoAd(string _value)
    {
        OnErrorRewardedVideoAd?.Invoke(_value);
    }
    public static void StartRewardedVideoAd(string _value)
    {
        OnStartRewardedVideoAd?.Invoke(_value);
    }
    public static void FinishedRewardedVideoAd(string _value)
    {
        OnFinishedRewardedVideoAd?.Invoke(_value);
    }
    public static void SkippedRewardedVideoAd(string _value)
    {
        OnSkippedRewardedVideoAd?.Invoke(_value);
    }
    public static void FailedRewardedVideoAd(string _value)
    {
        OnFailedRewardedVideoAd?.Invoke(_value);
    }
    public static void NoRewardedVideoAd(string _value)
    {
        OnNoRewardedVideoAd?.Invoke(_value);
    }

    //INTERSTICIAL
    public static void ReadyInterstitialVideoAd(string _value)
    {
        OnReadyInterstitialVideoAd?.Invoke(_value);
    }
    public static void ErrorInterstitialVideoAd(string _value)
    {
        OnErrorInterstitialVideoAd?.Invoke(_value);
    }
    public static void StartInterstitialVideoAd(string _value)
    {
        OnStartInterstitialVideoAd?.Invoke(_value);
    }
    public static void FinishedInterstitialVideoAd(string _value)
    {
        OnFinishedInterstitialVideoAd?.Invoke(_value);
    }
    public static void SkippedInterstitialVideoAd(string _value)
    {
        OnSkippedInterstitialVideoAd?.Invoke(_value);
    }
    public static void ClosedInterstitialVideoAd(string _value)
    {
        OnClosedInterstitialVideoAd?.Invoke(_value);
    }

    public static void FailedInterstitialVideoAd(string _value)
    {
        OnFailedInterstitialVideoAd?.Invoke(_value);
    }
    public static void NoInterstitialVideoAd(string _value)
    {
        OnNoInterstitialVideoAd?.Invoke(_value);
    }

    #endregion


    private static AdsManager Instance;
    public static AdsManager instance
    {
        get { return Instance; }
    }

    /*void Start()
    {
        _Init();
    }*/


    public void _Init()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

#if UNITY_ANDROID
        appKey = androidAppKey;
#elif UNITY_IOS
        appKey = iosId;
#elif UNITY_EDITOR
        appKey = "1111111";
#endif

        LevelPlay.OnInitSuccess += OnInitSuccess;
        LevelPlay.OnInitFailed += OnInitFailed;

        LevelPlay.Init(appKey);
    }

    void _ConfigBanner()
    {
        var configBuilder = new LevelPlayBannerAd.Config.Builder();

        configBuilder.SetSize(LevelPlayAdSize.LARGE);
        configBuilder.SetPosition(LevelPlayBannerPosition.BottomCenter);
        configBuilder.SetDisplayOnLoad(true);
        configBuilder.SetPlacementName(ADS_PLACEMENT.Ads_Banner_Id);
        var bannerConfig = configBuilder.Build();

        _bannerAd = new LevelPlayBannerAd(ADS_PLACEMENT.Ads_Banner_Id, bannerConfig);


    }



    public void _LoadBanner()
    {
        if (!_initialized)
        {
            Debug.LogError("Cant Show Banner, ADS no initialized");
            return;
        }

        _bannerLoaded = true;

        _bannerAd.LoadAd();

    }

    public void _ShowBanner()
    {
        if (!_bannerLoaded)
        {
            Debug.LogError("Cant Show Banner, Banner not loaded");
            _LoadBanner();
            return;
        }

        _bannerAd.ShowAd();
    }

    public void _HideBanner()
    {
        if (!_bannerLoaded)
        {
            Debug.LogError("Cant Hide Banner, Banner not loaded");
            _LoadBanner();
            return;
        }

        _bannerAd.HideAd();
    }

    public void _LoadInsterticial()
    {
        if (!_initialized)
        {
            Debug.LogError("Cant Show Insterticial, ADS no initialized");
            return;
        }

        if (_interstitialAd == null)
        {
            Debug.LogError("NULL Interstitial");
            _interstitialAd = new LevelPlayInterstitialAd(ADS_PLACEMENT.Ads_Intersticial_Id);
        }
        _interstitialAd.LoadAd();
    }

    public void _ShowIntersticial()
    {
        if (_interstitialAd.IsAdReady())
        {
            _interstitialAd.ShowAd();
        }
        else
        {
            Debug.LogWarning("Cant Show Insterticial, Not Loaded");
            _LoadInsterticial();
            return;
        }
    }

    public void _LoadRewarded()
    {
        if (!_initialized)
        {
            Debug.LogError("Cant Show Rewarded, ADS no initialized");
            return;
        }
    }

    void _ShowRewarded()
    {
        if (rewardedAd.IsAdReady())
        {
            rewardedAd.ShowAd();
        }
        else
        {
            Debug.LogWarning("Cant Show Rewarded, Not Loaded");
            _LoadRewarded();
            return;

        }
    }



    #region ADS CALLBACKS

    //INITIALIZATION
    void OnInitSuccess(LevelPlayConfiguration configuration)
    {
        Debug.LogError("Mediation Initialized");
        _initialized = true;

        _bannerAd = new LevelPlayBannerAd(ADS_PLACEMENT.Ads_Banner_Id);
        _bannerAd.OnAdLoaded += BannerOnAdLoaded;
        _bannerAd.OnAdLoadFailed += BannerOnAdLoadFailed;



        _interstitialAd = new LevelPlayInterstitialAd(ADS_PLACEMENT.Ads_Intersticial_Id);
        _interstitialAd.OnAdLoaded += InterstitialOnAdLoaded;
        _interstitialAd.OnAdLoadFailed += InterstitialOnAdLoadFailed;
        _interstitialAd.OnAdDisplayed += InterstitialOnAdDisplayed;
        _interstitialAd.OnAdDisplayFailed += InterstitialOnAdDisplayFailed;
        _interstitialAd.OnAdClosed += InterstitialOnAdClosed;

        InitializationDone(String.Empty);

    }

    void OnInitFailed(LevelPlayInitError error)
    {
        Debug.LogError("Init " + error.ErrorCode + " - " + error.ErrorMessage);
        _initialized = false;
        InitializationFailed("Init " + error.ErrorCode + " - " + error.ErrorMessage);
    }

    //BANNER

    /*
    BannerOnAdLoadedEvent
    BannerOnAdLoadFailedEvent
    BannerOnAdDisplayedEvent
    BannerOnAdDisplayFailedEvent
    BannerOnAdClickedEvent
    BannerOnAdCollapsedEvent
    BannerOnAdLeftApplicationEvent
    BannerOnAdExpandedEvent
    */


    void BannerOnAdLoaded(LevelPlayAdInfo info)
    {
        _bannerLoaded = true;

        _ShowBanner();
    }
    void BannerOnAdLoadFailed(LevelPlayAdError error)
    {
        Debug.LogError("Banner: " + error.ErrorCode + " - " + error.ErrorMessage);
    }


    //INTERTICIAL

    /*
    InterstitialOnAdLoadedEvent
    InterstitialOnAdLoadFailedEvent
    InterstitialOnAdDisplayedEvent
    InterstitialOnAdDisplayFailedEvent
    InterstitialOnAdClickedEvent
    InterstitialOnAdClosedEvent
    InterstitialOnAdInfoChangedEvent
    */

    void InterstitialOnAdLoaded(LevelPlayAdInfo info)
    {
        ReadyInterstitialVideoAd(info.AdUnitName);
        //_ShowInsterticial();
    }

    void InterstitialOnAdLoadFailed(LevelPlayAdError error)
    {
        ErrorInterstitialVideoAd("Interstitial " + error.ErrorCode + " - " + error.ErrorMessage);
        Debug.LogError("Interstitial " + error.ErrorCode + " - " + error.ErrorMessage);
    }

    void InterstitialOnAdDisplayed(LevelPlayAdInfo info)
    {
        FinishedInterstitialVideoAd(info.AdUnitName);
    }

    void InterstitialOnAdDisplayFailed(LevelPlayAdInfo info, LevelPlayAdError error)
    {
        FailedInterstitialVideoAd(info.AdUnitName + " " + error.ErrorCode + " - " + error.ErrorMessage);
    }

    void InterstitialOnAdClosed(LevelPlayAdInfo adInfo)
    {
        ClosedInterstitialVideoAd(adInfo.AdUnitName);
    }
    #endregion
}



public class ADS_PLACEMENT
{
#if UNITY_ANDROID
    public const string Ads_Banner_Id =         "0fyntlw1lxh4n3jv";
    public const string Ads_Rewarded_Id =       "h9lgru22ofx0o2qk";
    public const string Ads_Intersticial_Id =   "8oli2gj5py9xszh8";
    
#elif UNITY_IOS
    public const string Ads_Banner_Id =          "IOS_Banner";
    public const string Ads_Rewarded_Id =        "IOS_Rewarded";
    public const string Ads_Intersticial_Id =    "IOS_Interstitial";

#elif UNITY_EDITOR
    public const string Ads_Banner_Id =          null;
    public const string Ads_Rewarded_Id =        null;
    public const string Ads_Intersticial_Id =    null;
#endif
}