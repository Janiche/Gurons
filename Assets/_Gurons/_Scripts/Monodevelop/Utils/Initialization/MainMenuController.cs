using System.Collections.Generic;
using UnityEngine;

using TMPro;
using Bermost;
using UnityEngine.UI;



public class MainMenuController : MonoBehaviour
{
    [SerializeField] private AnimatedPanelController _animatedPanelController;

    [SerializeField] private bool _tutorial = false;

    #region INITIALIZATION VARIABLES


    [Header("Version")]
    [Tooltip("Objeto con texto de versión")] [SerializeField] private TextMeshProUGUI version = null;

    [Header("Daily Reward")]
    [SerializeField] private AdRewardController _adRewardController;
    [SerializeField] private int _elapsedDays;
    [SerializeField] private int _rateDays;

    [Header("EXTRAS")]

    //[SerializeField] private GameObject _lifeSystemPrefab;
    //[SerializeField] private TextTable _localizedTextTable;
    [SerializeField] private GameObject _forbiddenScratch;
    [SerializeField] private Button _scratch;

    [Space]
    [Header("Logros")]
    [SerializeField] private GameObject _burbuja;

    [SerializeField] private TextMeshProUGUI _noReclamados;
    //Notification _refillNotification;

    #endregion

    private void OnEnable()
    {
        _Init();
    }

    #region INITIALIZATION

    void _Init()
    {

        GlobalVars._transactionContext = Constants.TransactionContext.ON_MENU;

        if (!AchivementSystem.Instance._menuCheck)
        {
            GameEvents.CheckGameAchievement();
            GameEvents.CheckGuronAchievement();
            GameEvents.CheckLosesAchievement();
            GameEvents.CheckPlanetAchievement();

            AchivementSystem.Instance._menuCheck = true;
        }

        int _unclaimed = AchivementSystem.Instance.GetUnclaimedAchievement;

        if (MusicInstance.instance != null)
        {
            if (!MusicInstance.instance._IsPlaying)
            {
                MusicInstance.instance._Play();
            }
        }

    /*
        if (AdsManager.instance != null)
        {
            AdsManager.instance._ShowBanner(true);

            AdsManager.OnNoAd += NoAds;
        }
        */

        _adRewardController._Init();

        if (LifeSystemController.Instance != null)
        {
            LifeSystemController.Instance._Init();
        }

        _tutorial = GlobalVars.saveData.playerData.tutorial;



        if (_unclaimed > 0)
        {
            _burbuja.SetActive(true);
            _noReclamados.text = $"{_unclaimed}";
        }
        else
        {
            _burbuja.SetActive(false);
            _noReclamados.text = "0";
        }

        _EvalRate();

    }

    private void OnDisable()
    {
        //AdsManager.OnNoAd -= NoAds;
    }

    /// <summary>
    /// Analiza si es tiempo para mostrar mensaje de calificación
    /// </summary>
    public void _EvalRate()
    {
        ////ABRE RATE APP EN CASO DE CUMPLIRSE EL TIEMPO
        //Debug.LogError("Show Rate: " + GlobalVars.showRate);
        if (GlobalVars.showRate)
        {
            //ABRE PANEL DE RATE
            _animatedPanelController._ShowRateApp();
            //ratePanel.SetActive(true);
        }
        else
        {
            _animatedPanelController._HideBlack();
        }
    }

    #endregion

    #region BUTTONS

    /// <summary>
    /// Dirige a pantalla de planetas solo si posee vidas
    /// </summary>
    public void _NewGame()
    {
        if (_tutorial)
        {
            //CARGA PANTALLA DE PLANETAS
            if (Store.instance.GetBalance(Constants._Currencies.one_life) > 0)
            {
                SceneLoader._instance.LoadScene(Constants._SceneName.Planets.ToString());

                //_sceneLoader.LoadSceneButton(Constants._SceneName.Planets.ToString());
            }

            //ACTIVA PANEL DE NO VIDAS
            else
            {
                _animatedPanelController._ShowNoLife();
            }
        }

        else
        {
            //_sceneLoader.LoadSceneButton(Constants._SceneName.Game.ToString());
            SceneLoader._instance.LoadScene(Constants._SceneName.Game.ToString());
        }
    }

    private void OnReceiveNotification(string intent)
    {
    }

    #endregion

    private void NoAds(string placementId)
    {
        //_animatedPanelController._ShowNoAds();
    }



    //    private void _CheckStore()
    //    {
    //#if UNITY_EDITOR
    //        AnalyticsManager.Instance._SetStore("Standalone");

    //#elif UNITY_IOS
    //            AnalyticsManager.Instance._SetStore("IOS");

    //#elif UNITY_ANDROID
    //            string path = Application.persistentDataPath + "/Unity" + Application.cloudProjectId + "/udp/udp.json";

    //            //APLICACIÓN DE GOOGLE PLAY
    //            if (!System.IO.File.Exists(path))
    //            {
    //                AnalyticsManager.Instance._SetStore("Google PLAY");
    //            }

    //            //APLICACIÓN DE UDP
    //            else
    //            {
    //                string json = System.IO.File.ReadAllText(path);
    //                UDPData udp = JsonUtility.FromJson<UDPData>(json);

    //                Debug.LogError(udp.StoreName);

    //                AnalyticsManager.Instance._SetStore(udp.StoreName);
    //            }
    //#endif
    //    }

    //    private class UDPData
    //    {
    //        public string key = string.Empty;
    //        public string StoreName = string.Empty;
    //        public string udpClientId = string.Empty;
    //        public string CloudProjectId = string.Empty;

    //    }
}


