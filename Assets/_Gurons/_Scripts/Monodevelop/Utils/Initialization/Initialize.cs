//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Audio;

using PixelCrushers;
using PixelCrushers.DialogueSystem;

using CodeStage.AntiCheat.ObscuredTypes;
using Bermost;

/*
 * ****  ORDEN INICIALIZACIÓN  ****
 * 
 * 0.- Inicia Inicializacion
 * 1.- Analiza Piratería
 * 2.- Inicializa Idioma
 * 3.- Inicializa Audio
 * 4.- Inicializa Tienda
 * 5.- Inicializa Facebook
 * 6.- Termina Inicialización
 * 7.- Carga Escena
 * 
 */

public class Initialize : MonoBehaviour
{
    #region Variables

    [SerializeField] protected int cryptoKey = 0;

    //[SerializeField] private AdsManager _adsManager;
    [SerializeField] private AnalyticsManager _analyticsManager;
    [SerializeField] private NotificationManager _notificationManager;
    //[SerializeField] private InetManager _inetManager;

    [Header("Configuración Audio")]
    [Tooltip("AudioMixer")] [SerializeField] private AudioMixer mixer = null;

    //TODO: PERFECCIONAR VALIDACIÓN
    [Header("Estado Piratería")]
    private ObscuredBool piracy = false; //ESTADO DE VERIFICACIÓN DE PIRATERÍA

    [Header("Localizacion")]
    //[Tooltip("Objeto de actualizador de localización")] [SerializeField] private UpdateTextMesh updateLocalized = null;
    //[SerializeField] private UIView _languagePanel;
    private bool _firstRun = false;
    //UILocalizationManager
    //[Tooltip("Objeto de actualizador de localización")] [SerializeField] private UILocalizationManager updateLocalized = null;
    [Tooltip("Tabla de localización de UI")] [SerializeField] private TextTable localizedTextTable = null;



    [Header("Otros")]
    [SerializeField] private ObscuredBool udp = false;
    [SerializeField] private ObscuredBool amazon = false;
    [SerializeField] private ObscuredBool noData = true;
    [SerializeField] private ObscuredInt _maxDays = 0;
    [Tooltip("Objeto contenedor de Loading Screen")] [SerializeField] private GameObject loadingCanvas = null;

    #endregion

    #region Funciones Base

    void GenerateKey()
    {
        cryptoKey = 0;
        //CREA CRYPTOKEY ALEATORIA
        //GENERADA POR 4 RANDOM DIFERENTES

        int[] k = new int[4];
        int[] ik = new int[4];

        //GENERA UN RANDOM POR LUGAR
        int kLength = k.Length;

        for (int i = 0; i < kLength; i++)
        {
            k[i] = Random.Range(0, 10);
        }

        //INVIERTE ORDEN
        for (int j = 0; j < kLength; j++)
        {
            ik[j] = k[3 - j];
        }

        //MULTIPLICA POR PRIMOS Y SUMA PARA DIGITO FINAL
        int[] primos = new int[] { 2, 3, 5, 7, 11, 13 };
        int suma = 0;
        for (int x = 0; x < kLength; x++)
        {
            ik[x] *= primos[x];
            suma += ik[x];
        }

        //OBTIENE RESTO DEL FINAL
        int resto = suma % 8;

        //CREA CRYPTOKEY
        for (int z = 0; z < kLength; z++)
        {
            cryptoKey += (k[z] * ((int)Mathf.Pow(10, z)));
        }
        cryptoKey += resto;

        //INICIA ENCRIPTACIÓN DE OBSCURED TYPES
        ObscuredInt.SetNewCryptoKey(cryptoKey);
        ObscuredBool.SetNewCryptoKey((byte)cryptoKey);
        ObscuredString.SetNewCryptoKey(cryptoKey.ToString());
        ObscuredFloat.SetNewCryptoKey(cryptoKey);
        ObscuredDouble.SetNewCryptoKey((long)cryptoKey);

        ////APLICA NUEVA ENCRIPTACIÓN EN INSTANCIAS ACTUALES
        //testMode.ApplyNewCryptoKey();
        //androidAppKey.ApplyNewCryptoKey();
        //iosAppKey.ApplyNewCryptoKey();

        piracy.ApplyNewCryptoKey();
        noData.ApplyNewCryptoKey();
    }

    void Start()
    {

        GenerateKey();

        ////Inicia delegadas
        //OnStartInit += onStartInit;
        //OnSetLanguage += onSetLanguage;
        //OnSetAudio += onSetAudio;
        //OnPiracyChecked += onPiracyChecked;
        ////OnStoreReady += onStoreReady;
        //OnEndInit += onEndInit;

        //StartInit();

        Init();
    }

    /// <summary>
    /// Inicializa secuencia de inicialización
    /// </summary>
    private void Init()
    {
        //PlayerPrefs.DeleteKey("Decoded");

        bool saveData = SaveSystem.CheckData();

        //Store.instance._Init();

        if (saveData)
        {
            noData = false;
            SaveSystem.Load();
            SaveSystem.LoadPref();

            _firstRun = false;
        }

        //No existen Datos e inicia por primera vez
        else
        {
            FirstRun();
            //NotificationManager._instance._SendNotification("Test Notification", "This is a test Notification", System.DateTime.Now.AddMinutes(2));
            _firstRun = true;
        }


        //appKey.ApplyNewCryptoKey();
        //Debug.LogError(appKey);
        //DESACTIVA PEDIDO DE UBICACIÓN

        //SORTEA EL DÍA DE ADS

        System.TimeSpan _tsElapsed = GlobalVars.saveData.playerData.lastLogin - GlobalVars.saveData.playerData.lastGiven;
        System.TimeSpan _tsRate = GlobalVars.saveData.playerData.lastLogin - GlobalVars.saveData.playerData.nextRate;

        //Debug.LogError("DAYS: " + (int)_tsElapsed.TotalDays + " RATE: " + (int)_tsRate.TotalDays);

        if ((int)_tsElapsed.TotalDays > 0)
        {
            GlobalVars.saveData.rewardData._assigned = false;
            GlobalVars.saveData.playerData.scratchIndex = 0;
        }
        //System.TimeSpan _tsElapsed = System.DateTime.Now - GlobalVars.saveData.playerData.lastGiven;


        if (!GlobalVars.saveData.rewardData._assigned)
        {
            if (_maxDays > 0)
            {
                GlobalVars.saveData.playerData.lastGiven = System.DateTime.Now;

                GlobalVars.saveData.rewardData._currentDay = Random.Range(0, _maxDays);
                GlobalVars.saveData.rewardData._currentAd = 0;
                //Debug.LogError("R:" + GlobalVars.saveData.rewardData._currentDay);
                GlobalVars.saveData.rewardData._assigned = true;
            }
        }

        if (!GlobalVars.saveData.playerData.rated)
        {

            //PATCH 1.1.1=>1.2.0: AGREGAR CANTIDAD DE GURONS COLOCADOS : if ((int)_tsRate.TotalDays >= GlobalVars.timeToRate)

            if ((int)_tsRate.TotalDays >= GlobalVars.timeToRate || GlobalVars.saveData.gameData.totalBlox >= 50)
            {
                GlobalVars.showRate = true;
            }
            else
            {
                GlobalVars.showRate = false;
            }
            //Debug.LogError("Show Rate: " + GlobalVars.showRate);
        }
        else
        {
            GlobalVars.showRate = false;
        }


        //INICIA VERIFICACIÓN DE PIRATERÍA
        PiracyCheck();
    }

    /// <summary>
    /// En caso de no existir dato de juego, configura todo de manera inicial
    /// </summary>
    private void FirstRun()
    {
        noData = true;

        //Crea Archivo de Guardado
        GlobalVars.saveData = new SaveData();
        GlobalVars.prefData = new PrefData();


        GlobalVars.saveData.playerData.firstLogin = System.DateTime.Now.Date;

        //ACTIVA VOLUMEN EN SFX Y MÚSICA
        GlobalVars.prefData.musicVol = true;
        GlobalVars.prefData.soundVol = true;

        //SOLICITA IDIOMA DEL EQUIPO, EN CASO DE NO ESTAR DISPONIBLE
        //COLOCA INGLES COMO IDIOMA DEFECTO
        string languageCode = Localization.GetLanguage(Application.systemLanguage);

        if (!localizedTextTable.HasLanguage((languageCode)))
        {
            languageCode = "en";
        }

        //FIJA IDIOMA E INDICE
        GlobalVars.prefData.languageCode = languageCode;
        GlobalVars.prefData.systemLanguageCode = languageCode;

        //updateLocalized.UpdateText(languageCode);
        UpdateText(languageCode);
    }

    /// <summary>
    /// Analiza si el juego fue modificado
    /// </summary>
    private void PiracyCheck()
    {
        //REVISA DI APP FUE MODIFICADA DESDE COMPILADO

        bool _genuine = Application.genuine;
        bool _checkAvailable = Application.genuineCheckAvailable;



        if (_genuine && _checkAvailable)
        {
            GlobalVars.pirate = false;
            //GlobalVars.piracy = true;

        }
        else
        {
            GlobalVars.pirate = true;
        }

        //Debug.LogError($"{_genuine} AND {_checkAvailable} => {GlobalVars.pirate}");


        InitializeLanguage();
    }

    /// <summary>
    /// Configura idioma defecto el idioma de sistema y fija idioma
    /// </summary>
    private void InitializeLanguage()
    {
        //ACTUALIZA IDIOMA POR IDIOMA EN PREFERENCIAS
        string languageCode = GlobalVars.prefData.languageCode;

        UpdateText(languageCode);
        //updateLocalized.UpdateText(languageCode);

        InitializeAudio();

    }

    /// <summary>
    /// Configura los volumenes del mixer
    /// </summary>
    private void InitializeAudio()
    {
        if (mixer != null)
        {
            mixer.SetFloat("Music", (GlobalVars.prefData.musicVol) ? 0 : -80);
            mixer.SetFloat("SFX", (GlobalVars.prefData.soundVol) ? 0 : -80);
        }

        //StoreReady();
        EndInit();
    }

    #endregion

    #region Funciones Delegadas

    private void EndInit()
    {

        SaveSystem.SaveData();
        SaveSystem.SavePref();
        Store.instance.Save();

    /*
        IInitListener listener = new InitListener();
        StoreService.Initialize(listener);
        */

    /*
        if (_adsManager != null)
        {
            _adsManager._Init();
        }
        */

        //NOTIFICACIONES
        if (_notificationManager != null)
        {
            _notificationManager._Init();
        }

        if (_analyticsManager != null)
        {


            _analyticsManager._Init();
        }

        Notification _replayNotification = new Notification()
        {
            _title = localizedTextTable.GetFieldText("Notification.PlayReminder.title"),
            _content = localizedTextTable.GetFieldText("Notification.PlayReminder.content"),
            _fireTime = System.DateTime.Now.AddDays(3),
            _repeatInterval = new System.TimeSpan(6, 0, 0, 0)
        };

        NotificationManager._instance._SendNotification(_replayNotification);


        //_inetManager._StartCheck();
        FinishInit();

    }

    public void FinishInit()
    {

        _CheckStore();

        if (!_firstRun)
        {
            loadingCanvas.SetActive(true);
            SceneLoader.LoadAsyncWithLoader(Constants._SceneName.MainMenu.ToString());
        }
        else
        {
            //_languagePanel.Show();
        }
    }



    public void _LoadMainMenu()
    {
        SceneLoader.LoadAsyncWithLoader(Constants._SceneName.MainMenu.ToString());
    }
    #endregion

    /// <summary>
    /// Actualiza todo TextMesh en escena
    /// </summary>
    public void UpdateText(string languageCode)
    {
        GlobalVars.prefData.languageCode = languageCode;

        TextTable.currentLanguageID = localizedTextTable.GetLanguageID(languageCode);

        if (DialogueManager.instance != null)
        {
            DialogueManager.SetLanguage(languageCode);
        }
    }

    private void _CheckStore()
    {
        string store = string.Empty;
        //#if UNITY_EDITOR
        //        AnalyticsManager.Instance._SetStore("Standalone");



#if UNITY_ANDROID


        //UDP
        if (!udp)
        {
            //AMAZON
            if (amazon)
            {
                store = Constants.Stores.AMAZON_APPSTORE;
            }

            //GOOGLE PLAY
            else
            {
                store = Constants.Stores.GOOGLE_PLAY;
            }
        }

        //else
        //{
        //    string udpFilePath = Path.Combine(Application.persistentDataPath, "Unity", Application.cloudProjectId, "udp", "udp.json");
        //    string udpFileContents = File.ReadAllText(udpFilePath);
        //    AppAttributes appAttr = JsonUtility.FromJson<AppAttributes>(udpFileContents);

        //    store = appAttr.udpStore;
        //}


#elif UNITY_IOS
            store = Constants.Stores.IOS;
#endif

        GlobalVars.store = store;
    }

    public struct AppAttributes { public string udpStore; }

}
