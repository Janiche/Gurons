using PixelCrushers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// THIS SCRIPT CONTROLS ANY MICRO UI CONTROLLER
/// LIKE VIEWCONTROLLER. AND INTERACT WITH EXTERNAL
/// UI FUNCTION
/// </summary>
public class UIController : MonoBehaviour
{
    //ADD BLACK PANEL ON OPEN ANY MESSAGE PANEL

    [SerializeField] private GameObject _blackPanel;

    [SerializeField] private Button _pause;

    #region Controllers

    [Header("Common Game Mode Panel")]
    [SerializeField] private ScoreController _scoreController;
    [SerializeField] private MessageController _noLifeController;
    [SerializeField] private MessageController _pauseController;
    [SerializeField] private ChancesController _chancesController;
    [SerializeField] private MessageController _unlockedController;
    [SerializeField] private GameOverController _gameOverController;
    [SerializeField] private EndTutorialController _endTutorialController;
    [SerializeField] private UIPanelAnimator _noAdsPanel;
    [SerializeField] private WindDirectionController _windDirectionController;
    [SerializeField] private TextTable localizationTable = null;

    [Space]
    [Header("Store")]
    [SerializeField] private StoreProduct _oneLife;
    [SerializeField] AlertPanel alertsMessages;
    public UnityEvent onPurchaseComplete;
    public UnityEvent onPurchaseFailed;

    #endregion

    #region Internal

    /// <summary>
    /// General Initialization. [FOR GAME ONLY]
    /// </summary>
    /// <param name="_chances">Chances.</param>
    public void _Init(int _chances, int _continueCost, float _intensity)
    {
        _gameOverController._Init(_continueCost);

        //_noLifeController._Init();
        //_pauseController._Init();
        //_unlockedController._Init();

        _pause.interactable = true;

        _scoreController._Init();
        _chancesController._Init(_chances);

        _windDirectionController._Init(_intensity);

        string _name = localizationTable.GetFieldText(_oneLife.productId + ".name");
        string _desc = localizationTable.GetFieldText(_oneLife.productId + ".description");

        _oneLife._Init(_name, _desc);

        Store.OnPurchaseFail += OnPurchaseFail;
        Store.OnPurchaseSuccess += OnPurchaseSuccess;

    }

    /// <summary>
    /// General Initialization. [FOR TUTORIAL ONLY]
    /// </summary>
    /// <param name="_chances">Chances.</param>
    public void _Init(int _chances, float _intensity)
    {
        _pause.interactable = false;
        _scoreController._Init();
        _chancesController._Init(_chances);
        //_endTutorialController._Init();

        _windDirectionController._Init(_intensity);
    }


    private void OnDisable()
    {
        Store.OnPurchaseFail -= OnPurchaseFail;
        Store.OnPurchaseSuccess -= OnPurchaseSuccess;
    }

    #endregion



    #region On Need to enable any View


    public void _OnPressPause()
    {
        _pauseController._ActiveView(true);
    }

    public void _OnPause()
    {
        GameEvents.Pause();
        _blackPanel.SetActive(true);
    }

    public void _OnUnpause()
    {
        GameEvents.Unpause();
        _blackPanel.SetActive(false);
    }

    #endregion

    #region On Press Scene Button

    public void _OnPressStore()
    {
        //ADD SCENEMANAGER TO STORE
    }

    public void _OnPressMenu()
    {
        //ADD SCENEMANAGER TO MAIN MENU
    }

    public void _OnPressRetry()
    {
        //ADD SCENEMANAGER TO RELOAD GAME
    }

    //public void _OnResume()
    //{

    //}

    public void _OnPressNewChallenge()
    {
        //ADD SCENEMANAGER TO GAME CREATING NEW CHALLENGE
    }

    public void _OnDiamondContinue()
    {
        _blackPanel.SetActive(false);
        _gameOverController._ActiveView(false);
    }

    public void _OnAdContinue()
    {
        _blackPanel.SetActive(false);
        _gameOverController._ActiveView(false);
        //_blackPanel.SetActive(false);
    }

    public void _OnPressRestart()
    {
        //ADD SCENEMANAGER TO GAME SCENE.
    }

    public void _OnPressNextPlanet()
    {
        //ADD SCENEMANAGER TO GAME SCENE WITH NEW PLANET SELECTED
    }

    public void _OnNoLifeContinue()
    {
        //OPENS NOLIFECONTROLLER
        _noLifeController._ActiveView(true);
        _blackPanel.SetActive(true);
    }

    public void _OnKeepPlaying()
    {
        _blackPanel.SetActive(false);
    }

    #endregion

    #region On Trigger Any Game Event

    public void _OnGameOver(int _score, int _best, Constants._GameContinue _continue)
    {
        _gameOverController._OnGameOver(_score, _best, _continue);
        _blackPanel.SetActive(true);
    }

    public void _OnChangeScore(int _score)
    {
        _scoreController._UpdateScore(_score);
    }

    public void _OnUnlockPlanet()
    {
        _unlockedController._ActiveView(true);
        _blackPanel.SetActive(true);
    }


    public void _OnTutorialSuccess()
    {
        _endTutorialController._OnSuccess();
    }

    public void _OnTutorialFailed()
    {
        _endTutorialController._OnFailed();
    }

    public void _AddChance()
    {
        _chancesController._AddChance();
    }

    public void _RemoveChance()
    {
        _chancesController._RemoveChance();
    }

    public void _NoAds()
    {
        _noAdsPanel.Show();
        _blackPanel.SetActive(true);
    }

    public void _ChangeWind(float _intensity)
    {
        _windDirectionController._ChangeWind(_intensity);
    }
    #endregion


    public void OnPurchaseSuccess(BuyStatus _status, FailedReason _reason, string productId)
    {
        string _name = localizationTable.GetFieldText(productId + ".name");
        Debug.LogError("PURCHSED");
        alertsMessages.ShowSuccessAlert(_name);
        _gameOverController._UpdateStatus();
        _noLifeController._ActiveView(false);
        _blackPanel.SetActive(true);
        onPurchaseComplete.Invoke();
    }

    public void OnPurchaseFail(BuyStatus _status, FailedReason _reason, string _productId)
    {
        alertsMessages.ShowFailedAlert(_reason);
        _blackPanel.SetActive(true);
        onPurchaseFailed.Invoke();
    }
}
