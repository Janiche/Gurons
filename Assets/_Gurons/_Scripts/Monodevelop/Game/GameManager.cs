using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Audio;
using CodeStage.AntiCheat.ObscuredTypes;


public class GameManager : MonoBehaviour
{

    private enum WindType
    {
        none,
        changeFull,
        changeMid,
        full,
        mid
    }

    #region VARIABLES

    private static GameManager Instance = null;
    public static GameManager _instance
    {
        get
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<GameManager>();
            }

            return Instance;
        }
    }


    [Header("Controllers")]
    [SerializeField] private UIController _uiController;
    //[SerializeField] private Buttons _buttons = null;
    [SerializeField] private ObjectPooling _objectPooling = null;
    [SerializeField] private FeedbackController _feedbackController;
    [SerializeField] private Rope _rope;
    [SerializeField] WindType _windType;

    [Space]
    [Header("Planets")]
    [SerializeField] private Transform _planetParent = null;
    [SerializeField] private GameObject[] _planets = null;
    [SerializeField] private AreaEffector2D _windEffector = null;
    [SerializeField] private WindZone _windZone = null;
    [SerializeField] private float _windIntensity = 0;

    [Space]
    [Header("Game Mode")]
    [SerializeField] public ObscuredBool _game = false;
    [SerializeField] private GameMode _normalMode;
    [SerializeField] private GameMode _tutorialMode;
    [SerializeField] private ObscuredInt _chances = 3;
    [SerializeField] private ObscuredInt _extraChances = 0;
    GameMode _gameMode = null;

    [Space]
    [Header("Gurons")]
    [SerializeField] private Animator _ropeAnimator;
    [SerializeField] private ParticleSystem _coinParticle;
    [SerializeField] private ParticleSystem _smokeParticle;
    [SerializeField] private ParticleSystem _thunderParticle;
    [SerializeField] private AudioSource _coinSound;
    [SerializeField] private AudioSource _ropeSource;


    [SerializeField] private Transform _guronRopePoint;
    [SerializeField] private float _riseAmount = 1.14f;
    [SerializeField] private Transform _cameraPoi;
    [SerializeField] private float _guronSpeed = 1;
    [SerializeField] private TowerController _towerController;
    [SerializeField] private ObscuredFloat _lastGuronPos;
    [SerializeField] private GuronController _currentGuron;

    [Space]
    [Header("Acoustic Feedback")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private ObscuredInt _centerSoundIndex = 0;
    [SerializeField] private AudioClip[] _centerSounds;
    [SerializeField] ObscuredFloat _currentRopeFrame = 0;

    [Space]
    [Header("Pause")]
    [SerializeField] private AudioMixerSnapshot _pauseSnapshot;
    [SerializeField] private AudioMixerSnapshot _unpauseSnapshot;

    [Space]
    [Header("Continue")]
    [SerializeField] private ObscuredInt _continueDiamondCost = 100;
    [SerializeField] private ObscuredBool _continueUsed = false;

    Coroutine _windRoutine;

    #endregion

    private void OnEnable()
    {
        _Initialize();
    }

    #region INITIALIZATION

    void _Initialize()
    {
        _OnUnpause();

        DG.Tweening.DOTween.defaultTimeScaleIndependent = true;

        if (MusicInstance.instance != null)
        {
            MusicInstance.instance._Pause();
        }

        _SetInstance();

        //SET BG AUDIO CLIP
        _SetAudio();

        //SET GLOBAL VARIABLES
        _SetGlobalVar();

        //INITIALIZE PLANET
        _InitializePlanet();


        //INITIALIZE GAME MODE
        _InitializeGameMode();


        //SUBSCRIBE TO GAMEMODE DELEGATES
        _Subscription();

        //SET OFF ADS

        //_GenerateGuron();

        _windRoutine = StartCoroutine(_ChangeWindDirection());
    }

    void _SetInstance()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void _SetAudio()
    {

    }

    void _SetGlobalVar()
    {
        GlobalVars.blox = 0;
        GlobalVars.score = 0;
        GlobalVars.scoreMultiplier = 1;
        Time.timeScale = 1f;
    }

    void _EndGlobalVar()
    {

        GlobalVars._chanceAdded = false;
        GlobalVars._scoreAdded = false;
        GlobalVars.powerUpMultiplier = 1;
        GlobalVars.extraChance = 0;
    }

    void _InitializeGameMode()
    {

        //TUTORIAL COMPLETED
        if (GlobalVars.saveData.playerData.tutorial)
        {
            _gameMode = _normalMode;

            _extraChances = GlobalVars.extraChance;
            _chances = _gameMode.maxChances + _extraChances;

            //INITIALIZE  UICONTROLLER
            _uiController._Init(_gameMode.maxChances, _continueDiamondCost, _windIntensity);

        }

        //NEEDS TO PLAY THE TUTORIAL
        else
        {
            _gameMode = _tutorialMode;

            //INITIALIZE  UICONTROLLER
            _uiController._Init(_gameMode.maxChances, _windIntensity);

            _GenerateGuron();
        }

        _gameMode._SetTransform(transform);
        _gameMode._Init();


    }

    /// <summary>
    /// Initialize planet variables, like gravity, wind and map
    /// </summary>
    void _InitializePlanet()
    {
        _OnUnpause();
        //Activate selected planet
        //_planets[GlobalVars.currentPlanet].SetActive(true);
        GameObject _planet = Instantiate(_planets[GlobalVars.currentPlanet]);
        _planet.transform.SetParent(_planetParent);
        _planet.transform.localPosition = Vector3.zero;
        _planet.transform.localRotation = Quaternion.identity;
        _planet.transform.localScale = Vector3.one;
        _planet.SetActive(true);

        _guronSpeed = Mathf.Abs(GlobalVars.gravity) / 9.81f;

        _guronSpeed = (float)System.Math.Round(_guronSpeed, 1);



        //Debug.Log("ROPE: " + _guronSpeed);

        _ropeAnimator.SetFloat("Speed", _guronSpeed);


        //Wind force direction

        if (GlobalVars.wind != 0)
        {
            float sign = Mathf.Sign(Random.Range(-1.1f, 1.1f) + 0.01f);
            _windEffector.forceMagnitude = sign * GlobalVars.wind;
            //_windEffector.forceVariation = (GlobalVars.wind > 0) ? 0.2f : 0;
            _windEffector.gameObject.SetActive(true);


            _windZone.gameObject.transform.localEulerAngles = new Vector3(45, sign * 90, 0);
            _windZone.windMain = (GlobalVars.wind) / 10;


            _windIntensity = (_windEffector.forceMagnitude / GlobalVars.wind);
            _windIntensity = Mathf.Clamp(_windIntensity, -1, 1);
        }
    }

    void _Subscription()
    {
        GameEvents.OnChangeScore += _OnChangeScore;
        GameEvents.OnPause += _OnPause;
        GameEvents.OnUnpause += _OnUnpause;

        GameEvents.OnUICommand += _OnUiCommand;

        GameEvents.OnFailGuron += _OnFailGuron;
        GameEvents.OnSideGuron += _OnSideGuron;
        GameEvents.OnCenterGuron += _OnCenterGuron;
        GameEvents.OnEndGuron += _OnEndGuron;
        GameEvents.OnGameOver += _OnGameOver;
        GameEvents.OnUnlockPlanet += _OnUnlockPlanet;
        GameEvents.OnSuccessTutorial += _OnTutorialSuccess;
        GameEvents.OnFailedTutorial += _OnTutorialFailed;


        GameEvents.OnPlacePool += _OnPlacePool;

        GameEvents.OnChangeScene += _UnSubscription;

        //AdsManager.OnNoAd += NoAds;
        //AdsManager.OnFinishedAd += FinishAd;

    }

    void _UnSubscription()
    {
        _OnUnpause();

        GameEvents.OnChangeScore -= _OnChangeScore;
        GameEvents.OnChangeGuron -= _OnChangeGuron;
        GameEvents.OnPause -= _OnPause;
        GameEvents.OnUnpause -= _OnUnpause;

        GameEvents.OnUICommand -= _OnUiCommand;

        GameEvents.OnFailGuron -= _OnFailGuron;
        GameEvents.OnSideGuron -= _OnSideGuron;
        GameEvents.OnCenterGuron -= _OnCenterGuron;
        GameEvents.OnEndGuron -= _OnEndGuron;
        GameEvents.OnGameOver -= _OnGameOver;
        GameEvents.OnUnlockPlanet -= _OnUnlockPlanet;
        GameEvents.OnSuccessTutorial -= _OnTutorialSuccess;
        GameEvents.OnFailedTutorial -= _OnTutorialFailed;


        GameEvents.OnPlacePool -= _OnPlacePool;

        GameEvents.OnChangeScene -= _UnSubscription;

        //AdsManager.OnNoAd -= NoAds;
        //AdsManager.OnFinishedAd -= FinishAd;

        StopCoroutine(_windRoutine);
        _windRoutine = null;

        SaveSystem.SaveData();

    }

    #endregion

    #region POOL

    private void _OnPlacePool(Constants._PoolType _poolType, GameObject _poolObject)
    {
        _objectPooling._PlacePool(_poolType, _poolObject);
    }

    #endregion

    #region Trigger UI or Game Events

    private void _OnChangeScore()
    {

        _uiController._OnChangeScore(GlobalVars.score);
    }

    private void _OnChangeGuron()
    {
        _uiController._OnChangeScore(GlobalVars.blox);
    }

    /// <summary>
    /// Set TimeScale to 0.0 and SoundSnap
    /// </summary>
    private void _OnPause()
    {
        _pauseSnapshot.TransitionTo(0.01f);
        Time.timeScale = 0.0f;
    }

    /// <summary>
    /// Restore TimeScale to 1.0 and SoundSnap
    /// </summary>
    private void _OnUnpause()
    {
        _unpauseSnapshot.TransitionTo(0.01f);
        Time.timeScale = GlobalVars.currentTimeScale;
    }

    private void _OnGameOver()
    {
        //Debug.LogError("GAMEOVER");
        Constants._GameContinue _continue = Constants._GameContinue._none;

        _currentGuron = null;

        if (!_continueUsed)
        {

            //GlobalVars.saveData.gameData.totalLoses++;
            if (GlobalVars._loses < 3)
            {
                GlobalVars._loses++;
            }

            //DIAMOND CURRENCY AT LEAST 2
            if (Store.instance.GetBalance(Constants._Currencies.diamond_currency) >= _continueDiamondCost)
            {
                _continue = Constants._GameContinue._diamondAndAds;

            }

            else
            {
                _continue = Constants._GameContinue._onlyAds;
            }
        }
        else
        {
            _continue = Constants._GameContinue._noContinue;
        }

        _uiController._OnGameOver(GlobalVars.score, GlobalVars.saveData.gameData.planetData[GlobalVars.currentPlanet].bestRecord, _continue);
    }

    private void _OnUnlockPlanet()
    {
        _game = false;
        _uiController._OnUnlockPlanet();

    }

    /// <summary>
    /// Runs a function according command
    /// </summary>
    /// <param name="_command"></param>
    private void _OnUiCommand(Constants._UICommand _command)
    {
        switch (_command)
        {
            case Constants._UICommand._toMenu:
                _OnMenu();
                break;

            case Constants._UICommand._toStore:
                _OnStore();
                break;

            case Constants._UICommand._retry:
                _OnRetry();
                break;

            case Constants._UICommand._adContinue:
                _OnAdContinue();
                break;

            case Constants._UICommand._diamondContinue:
                _OnDiamondContinue();
                break;

            case Constants._UICommand._noLifeContinue:
                break;

            case Constants._UICommand._resume:
                _OnResume();
                break;

            case Constants._UICommand._restart:
                _OnRestart();
                break;

            case Constants._UICommand._keepPlaying:
                _KeepPlaying();
                break;

            case Constants._UICommand._nextPlanet:
                _OnNextPlanet();
                break;

            case Constants._UICommand._retryTutorial:
                _OnRetryTutorial();
                break;

            case Constants._UICommand._skipTutorial:
                _OnSkipTutorial();
                break;
        }
    }

    #endregion

    #region GAME SPECIFIC FUNCTION FOR ONUICOMMAND

    private void _OnResume()
    {
        _game = true;
        //_OnUnpause();
    }

    /// <summary>
    /// Reinicia el nivel [SOLO SI DISPONE DE VIDAS]
    /// </summary>
    private void _OnRetry()
    {

        GlobalVars.saveData.gameData.totalLoses++;

        GameEvents.CheckLosesAchievement();


        if (GlobalVars._loses > 2)
        {
            GlobalVars._loses = 0;
            /*
            if (AdsManager.instance != null)
            {
                if (AdsManager.instance._IsInsterticialReady())
                {
                    AdsManager.instance._ShowInsterticial();
                }
            }
            */
        }

        _EndGlobalVar();

        //OBTIENE BALANCE DE VIDAS, SI ES SOBRE 0 QUITA UNA Y RECARGA ESCENA DE JUEGO
        if (Store.instance.GetBalance(Constants._Currencies.one_life) > 0)
        {
            _UnSubscription();

            Store.instance.TakeProduct(Constants._Currencies.one_life, 1);
            _game = false;
            //_buttons.LoadSceneButton(Constants._SceneName.Game.ToString());
        }

        //DE LO CONTRARIO ACTIVA PANEL DE NO VIDAS
        else
        {
            _uiController._OnNoLifeContinue();


            //_noLife.SetActive(true);
            //_gameOver.SetActive(false);
        }


        //SaveSystem.SaveData();
    }

    /// <summary>
    /// Redirige al menú principal
    /// </summary>
    public void _OnMenu()
    {
        //SI TIENE MÁS DE UNA VIDA LA QUITA Y DIRIGE AL MENU

        GlobalVars.saveData.gameData.totalLoses++;
        _game = false;
        _OnUnpause();

        _EndGlobalVar();

        LifeSystemController.Instance._Pause(false);
        //_buttons.LoadSceneButton(Constants._SceneName.MainMenu.ToString());
        //_buttons.LoadSceneButton("MainMenu");

    }

    public void _OnRestart()
    {
        _EndGlobalVar();

        //SI TIENE MÁS DE UNA VIDA LA QUITA Y DIRIGE AL JUEGO
        _game = false;
        Instance = null;

        if (Store.instance.GetBalance(Constants._Currencies.one_life) > 0)
        {
            _OnUnpause();
            Store.instance.TakeProduct(Constants._Currencies.one_life, 1);
            //_buttons.LoadSceneButton(Constants._SceneName.Game.ToString());
        }
        //DE LO CONTRARIO ACTIVA PANEL DE NO VIDAS
        else
        {
            _uiController._OnNoLifeContinue();
            //_noLife.SetActive(true);
            //_gameOver.SetActive(false);
        }
        //SaveSystem.SaveData();

    }

    public void _OnStore()
    {

        _game = false;
        _EndGlobalVar();
        //_buttons.LoadSceneButton(Constants._SceneName.Store.ToString());
    }

    /// <summary>
    /// Descuenta un diamante en caso de tener y procede a otorgar nueva estrella (oportunidad)
    /// </summary>
    public void _OnDiamondContinue()
    {
        _game = false;
        //AL MOMENTO DE CONTINUAR, SI TIENE MÁS DE 1 DIAMANTE, LO QUITA Y AGREGA OPORTUNIDAD
        if (Store.instance.GetBalance(Constants._Currencies.diamond_currency) >= _continueDiamondCost)
        {

            Store.instance.TakeProduct(Constants._Currencies.diamond_currency, _continueDiamondCost);
            _continueUsed = true;
            _uiController._OnDiamondContinue();
            _Continue();
        }
    }

    /// <summary>
    /// Muestra publicidad y procede a otorgar nueva estrella (oportunidad)
    /// </summary>
    public void _OnAdContinue()
    {
        _game = false;
        _continueUsed = true;
        _uiController._OnAdContinue();
        //AdsManager.instance._ShowRewarded();
    }

    public void _OnNextPlanet()
    {
        _EndGlobalVar();

        LifeSystemController.Instance._Pause(false);

        _game = false;
        //_buttons.LoadSceneButton(Constants._SceneName.Planets.ToString());

        //if (GlobalVars.currentPlanet < GlobalVars.maxPlanets)
        //{
        //    GlobalVars.currentPlanet++;
        //    _buttons.LoadSceneButton("Game");
        //}
        //else
        //{
        //    _buttons.LoadSceneButton("Planets");
        //}
    }

    /// <summary>
    /// Reintenta desde pausa
    /// </summary>
    public void _OnPausedRetry()
    {
        _EndGlobalVar();

        _game = false;
        Instance = null;
        //OBTIENE BALANCE DE VIDAS, SI ES SOBRE 0 QUITA UNA Y RECARGA ESCENA DE JUEGO
        if (Store.instance.GetBalance(Constants._Currencies.one_life) > 0)
        {
            Store.instance.TakeProduct(Constants._Currencies.one_life, 1);
            _OnUnpause();
            //_buttons.LoadSceneButton(Constants._SceneName.Game.ToString());
        }

        //SaveSystem.SaveData();
    }

    /// <summary>
    /// Sale del juego guarda y despausa
    /// </summary>
    public void _OnExit()
    {
        _game = false;
        _OnUnpause();

        //SaveSystem.SaveData();
        _EndGlobalVar();
        //_buttons.LoadSceneButton(Constants._SceneName.MainMenu.ToString());
    }

    /// <summary>
    /// Restablece la opcion de seguir jugando tras desbloquear un nuevo planeta
    /// </summary>
    public void _KeepPlaying()
    {
        _game = true;
        _uiController._OnKeepPlaying();
    }

    /// <summary>
    /// Continua el juego, agrega oportunidad, permite jugar, 
    /// activa generación de powerUps
    /// </summary>
    void _Continue()
    {
        //Activa y da nueva oportunidad
        //GameController.Instance.chances[0].SetActive(true);

        _chances = 1;
        _uiController._AddChance();
        _game = true;


        _GenerateGuron();

        //if (GameController.Instance.gameMode.allowPowerUp)
        //{
        //    GameController.Instance.genPowerUp = GameController.Instance.GenPowerUp();
        //    StartCoroutine(GameController.Instance.genPowerUp);
        //}

    }

    void _OnRetryTutorial()
    {
        //_buttons.LoadSceneButton(Constants._SceneName.Game.ToString());
    }

    void _OnSkipTutorial()
    {
        GlobalVars.saveData.playerData.tutorial = true;
        GlobalVars.saveData.gameData.planetToUnlock = 0;

        _game = false;
        //_buttons.LoadSceneButton(Constants._SceneName.Planets.ToString());

        _EndGlobalVar();
        //SaveSystem.SaveData();
    }

    #endregion

    #region GAME MODE FUNCIONS

    public void _LauchGuron()
    {
        if (_game)
        {
            if (_currentGuron != null)
            {
                _rope._ChangeRope(false);
                //_ropeSource.Play();
                _currentGuron._LaunchGuron();
            }
        }
    }

    /// <summary>
    /// Evento llamado en caso de requerirse un nuevo guron, eleva cámara y mueve torre
    /// </summary>
    public void _CameraNextStep()
    {
        //SI HA COLOCADO MÁS DE UN GURON ACTUALIZA PUNTO DE INTERES DE CAM Y MUEVE
        if (GlobalVars.blox > 0)
        {
            _cameraPoi.transform.position = new Vector2(_cameraPoi.transform.position.x, _lastGuronPos - _riseAmount);

            //AUMENTA VELOCIDAD DE TORRE POR CADA 3 GURONS PUESTOS
            if (GlobalVars.blox % 3 == 0)
            {
                //_towerStep++;
                //_towerSpeed = (float)_towerStep / (float)10;

                //_towerSpeed = Mathf.Clamp(_towerSpeed, 0f, 2f);
                //_tower.SetFloat("Speed", _towerSpeed);

                _towerController._NextState();
            }
        }
    }

    //TODO AA: viento más "real" varianza de direccion e intensidad
    IEnumerator _ChangeWindDirection()
    {
        float _time = Random.Range(20, 30);

        yield return new WaitForSeconds(_time);


        float _newDir = (-1) * _windEffector.forceMagnitude;
        float _minWind = -Mathf.Abs(_newDir);
        float _maxWind = Mathf.Abs(_newDir);
        float _sign = Mathf.Sign(_newDir);
        float _discount = 0.1f;
        float _currentWind = _minWind;

        _windZone.gameObject.transform.localEulerAngles = new Vector3(45, _sign * 90, 0);

        while (_currentWind < _maxWind)
        {
            yield return new WaitForSeconds(0.001f);
            _windEffector.forceMagnitude += (_sign * _discount);

            _windEffector.forceMagnitude = Mathf.Clamp(_windEffector.forceMagnitude, _minWind, _maxWind);

            _windIntensity = (_windEffector.forceMagnitude / GlobalVars.wind);

            _windIntensity = Mathf.Clamp(_windIntensity, -1, 1);
            _uiController._ChangeWind(_windIntensity);

            _currentWind += _discount;


            //Debug.LogError($"_intensity {_windIntensity} ^ Force {_windEffector.forceMagnitude} ^ Varianza {_windEffector.forceVariation}");

        }




        _windRoutine = StartCoroutine(_ChangeWindDirection());
    }

    /// <summary>
    /// En caso de fin de juego, resta una vida e invoca al evento de fin de
    /// juego segun GameMode
    /// </summary>
    public void _EndGame()
    {

        _game = false;
        _gameMode._EndGame();

        //OnEndGame.Invoke();
    }

    /// <summary>
    /// Al colocar Guron en centro, reproduce feedback progresivo
    /// al llegar al máximo entrega premio aleatorio
    /// </summary>
    public void _CenterPrize()
    {

        _feedbackController._ShowFeedback(_centerSoundIndex);

        if (_centerSoundIndex < _centerSounds.Length - 1)
        {
            _centerSoundIndex++;

            //Debug.LogError("CENTER");

        }
        else
        {
            _centerSoundIndex = 0;
            //Debug.LogError("RESET");
            //TODO: RULETA DE PREMIO CENTRAL CON OPCION DE DOBLAR POR PUBLICIDAD
            //PrizeRoulette.instance.SelectPrize();
        }

        _audioSource.PlayOneShot(_centerSounds[_centerSoundIndex]);
    }

    /// <summary>
    /// Fija indice de centro
    /// </summary>
    /// <param name="index">Index.</param>
    public void _SetCenterIndex(int index)
    {
        _centerSoundIndex = index;
    }

    private void _OnSideGuron()
    {
        GlobalVars.saveData.gameData.totalSide++;
        GlobalVars.saveData.gameData.totalBlox++;


        _lastGuronPos = _currentGuron.transform.position.y;
        _gameMode._SideGuron();

        _smokeParticle.transform.position = _currentGuron.transform.position;
        _smokeParticle.Play();

        _currentGuron = null;
    }

    private void _OnCenterGuron()
    {
        GlobalVars.saveData.gameData.totalCenter++;
        GlobalVars.saveData.gameData.totalBlox++;

        _lastGuronPos = _currentGuron.transform.position.y;
        _gameMode._CenterGuron();

        _smokeParticle.transform.position = _currentGuron.transform.position;
        _smokeParticle.Play();

        _thunderParticle.transform.position = _currentGuron.transform.position;
        _thunderParticle.Play();

        _GiveCoins(1, _currentGuron.transform.position);

        _currentGuron = null;
    }

    private void _OnFailGuron()
    {
        _currentGuron = null;

        if (_game)
        {
            GlobalVars.saveData.gameData.totalFails++;

            _chances -= 1;
            _uiController._RemoveChance();
            _gameMode._FailGuron();

            Handheld.Vibrate();
            if (_chances <= 0)
            {
                _EndGame();
            }
        }

    }

    private void _OnEndGuron()
    {
        _currentGuron = null;

        if (_game)
        {
            GameEvents.CheckGuronAchievement();
            _CameraNextStep();
            _gameMode._EndGuron();
        }
    }

    #endregion

    #region CLOSE & INACTIVE FUNCTIONS

    public void _CloseGame()
    {
        //GameEvents.OnChangeScore -= _OnChangeScore;
        //GameEvents.OnChangeGuron -= _OnChangeGuron;
        //GameEvents.OnChangeTime -= _OnChangeTime;
        //GameEvents.OnPause -= _OnPause;
        //GameEvents.OnUnpause -= _OnUnpause;

        //GameEvents.OnUICommand -= _OnUiCommand;

        //GameEvents.OnFailGuron -= _OnFailGuron;
        //GameEvents.OnSideGuron -= _OnSideGuron;
        //GameEvents.OnCenterGuron -= _OnCenterGuron;
        //GameEvents.OnEndGuron -= _OnEndGuron;
        //GameEvents.OnGameOver -= _OnGameOver;
        //GameEvents.OnUnlockPlanet -= _OnUnlockPlanet;

        //GameEvents.OnPlacePool -= _OnPlacePool;
    }

    private void OnDisable()
    {
        _game = false;
    }

    void OnApplicationQuit()
    {
        _game = false;
    }

    #endregion

    private void _GiveCoins(int _coins, Vector2 _position)
    {
        var _main = _coinParticle.main;
        _main.maxParticles = _coins * 10;

        _coinParticle.transform.position = _position;
        _coinParticle.Play();

        if (_coinSound != null)
        {
            _coinSound.Play();
        }
        Store.instance.GiveProduct(Constants._Currencies.coin_currency, _coins);

    }

    public GameObject _OnGenSpace()
    {
        //Debug.LogError("gen");
        return _objectPooling._TakePoolFirst(Constants._PoolType._space);

    }

    public void _OnPlaceSpace(GameObject _space)
    {
        _objectPooling._PlacePool(Constants._PoolType._space, _space);
    }

    public void _GenerateGuron()
    {
        //if (_game)
        //{
        if (_currentGuron == null)
        {
            _rope._ChangeRope(true);
            //_currentRopeFrame = _ropeLenght * (_ropeAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1) * _ropeFrameRate;
            //_currentRopeFrame = Mathf.RoundToInt(_currentRopeFrame);

            _currentRopeFrame = (_ropeAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1);
            GameObject _go = _objectPooling._TakePoolRandom(Constants._PoolType._guron);
            _currentGuron = _go.GetComponent<GuronController>();

            _currentGuron._Appear(_guronSpeed, _currentRopeFrame, _guronSpeed);

            _go.transform.SetParent(_guronRopePoint);
            _go.transform.position = _guronRopePoint.position;
            _go.transform.localScale = Vector3.one * 0.8f;
        }

        //}
    }


    void OnConversationEnd(Transform actor)
    {
        _gameMode.OnConversationEnd();
    }

    void _OnTutorialSuccess()
    {
        _uiController._OnTutorialSuccess();
    }

    void _OnTutorialFailed()
    {
        _uiController._OnTutorialFailed();
    }

    private void FinishAd(string placementId)
    {
        _Continue();
    }

    private void NoAds(string placementId)
    {
        _uiController._NoAds();
    }
}
