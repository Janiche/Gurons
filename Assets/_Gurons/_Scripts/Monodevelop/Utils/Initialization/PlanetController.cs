//using System.Collections;
//using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


using PixelCrushers;


public class PlanetController : MonoBehaviour
{
    [Tooltip("Panel de selector de modo de juego")] [SerializeField] private UIPanelAnimator gameModeSelect = null;
    [Tooltip("Panel de selector de powerUp")] [SerializeField] private UIPanelAnimator powerupSelect = null;

    [Header("Planets")]
    [Tooltip("Arreglo con planetas disponibles")] [SerializeField] private Planet[] _planets;

    [Space]
    [Tooltip("Valor PerStep del slider de selector de planetas")] [SerializeField] private float perStep = 0.3f;

    [Tooltip("Indice del planeta en el centro")] [SerializeField] private int planetIndex = 0;
    [Tooltip("ScrollBar del selector de planeta")] [SerializeField] private Scrollbar planetScroll = null;

    [Tooltip("Boton siguiente planeta")] [SerializeField] private Button nextBtn;
    [Tooltip("Boton anterior planeta")] [SerializeField] private Button prevBtn;

    [Header("Auto Unlock")]
    [SerializeField] private AlertPanel alertsMessages;
    [SerializeField] private ParticleSystem _unlockParticle;

    [Header("PowerUp")]

    [SerializeField] private StoreProduct _chance;
    [SerializeField] private StoreProduct _score;

    [SerializeField] private PlanetPowerUpController _planetPowerUpController;
    [SerializeField] private TextTable localizationTable;
    int _toUnlock = -1;

    private Coroutine _unlockCoroutine;


    private void OnEnable()
    {
        _Init();
    }

    private void OnDisable()
    {
        Store.OnPurchaseFail -= OnPurchaseFail;
        Store.OnPurchaseSuccess -= OnPurchaseSuccess;
    }

    void _Init()
    {
/*
        if (AdsManager.instance != null)
        {
            AdsManager.instance._ShowBanner(true);
        }
*/
        if (!MusicInstance.instance._IsPlaying)
        {
            MusicInstance.instance._Play();
        }

        //SI EL PLANETA NO ESTÁ DESBLOQUEADO, DESACTIVA BOTON Y MUESTRA PARA "COMPRAR"

        GlobalVars.maxPlanets = _planets.Length;

        //for (int i = 0; i < planets.Length; i++)
        for (int i = 0; i < GlobalVars.maxPlanets; i++)
        {
            _planets[i]._unlocked = GlobalVars.saveData.gameData.planetData[i].unlocked;
            _planets[i].Init();
        }

        _toUnlock = GlobalVars.saveData.gameData.planetToUnlock;
        if (_toUnlock >= 0)
        {
            _unlockCoroutine = StartCoroutine(_UnlockRoutine());
        }


        string _name = localizationTable.GetFieldText(_chance.productId + ".name");
        string _desc = localizationTable.GetFieldText(_chance.productId + ".description");

        _chance._Init(_name, _desc);

        _name = localizationTable.GetFieldText(_score.productId + ".name");
        _desc = localizationTable.GetFieldText(_score.productId + ".description");

        _score._Init(_name, _desc);

        Store.OnPurchaseFail += OnPurchaseFail;
        Store.OnPurchaseSuccess += OnPurchaseSuccess;



    }

    /// <summary>
    /// Cuando selecciona un planeta, activa outline y desactiva los demás planetas
    /// </summary>
    public void OnPlanet()
    {
        //for(int i=0; i<_planets.Length;i++)
        for (int i = 0; i < GlobalVars.maxPlanets; i++)
        {
            if (i != GlobalVars.currentPlanet)
            {
                //_planets[i].enabled = false;
                _planets[i]._ActiveOutline(false);
            }
            else
            {
                gameModeSelect.Show();
                powerupSelect.Show();
            }
        }

        if (!_planets[GlobalVars.currentPlanet]._GetOutlineActive())
        {
            gameModeSelect.Hide();
            powerupSelect.Hide();
        }
        else
        {
            gameModeSelect.Show();
            powerupSelect.Show();
        }
    }

    private void RemovePlanet()
    {
        for (int i = 0; i < GlobalVars.maxPlanets; i++)
        {
            _planets[i]._ActiveOutline(false);

            if (!_planets[GlobalVars.currentPlanet]._GetOutlineActive())
            {
                gameModeSelect.Hide();
                powerupSelect.Hide();
            }
        }
    }


    /// <summary>
    /// Al seleccionar modo de juego normal
    /// </summary>
    public void OnNormal()
    {
        // GlobalVars.gameMode = "Normal";

        if (Store.instance.GetBalance(Constants._Currencies.one_life) > 0)
        {
            Store.instance.TakeProduct(Constants._Currencies.one_life, 1);
        }

        if (GlobalVars._scoreAdded)
        {
            Store.instance.TakeProduct(Constants._Currencies.double_score, 1);
            GlobalVars.powerUpMultiplier = 2;
        }

        if (GlobalVars._chanceAdded)
        {
            Store.instance.TakeProduct(Constants._Currencies.extra_chance, 1);
            GlobalVars.extraChance = 1;
        }

        //GetComponent<Buttons>().LoadSceneButton(Constants._SceneName.Game.ToString());

        LifeSystemController.Instance._Pause(true);

        //Debug.LogError(GlobalVars._remainNotification);
    }

    /// <summary>
    /// Avanza al siguiente planeta en selector
    /// </summary>
    public void NextPlanet()
    {
        planetIndex++;
        prevBtn.interactable = true;
        planetScroll.value = (planetIndex * perStep);

        RemovePlanet();

        if (planetScroll.value >= 1)
        {
            nextBtn.interactable = false;
            return;
        }

    }

    /// <summary>
    /// Retrocede al planeta Anterior en selector
    /// </summary>
    public void PrevPlanet()
    {
        planetIndex--;
        nextBtn.interactable = true;
        planetScroll.value = (planetIndex * perStep);

        RemovePlanet();

        if (planetScroll.value <= 0)
        {
            prevBtn.interactable = false;
            return;
        }

    }

    IEnumerator _UnlockRoutine()
    {
        nextBtn.interactable = false;
        prevBtn.interactable = false;

        planetIndex = _toUnlock;

        float _sliderPos = _toUnlock * perStep;
        _unlockParticle = _planets[_toUnlock].UnlockParticle;
        Image _img = _planets[_toUnlock]._PlanetImage;

        //MOVE SCROLL TO PLANET POSITION
        planetScroll.value = _sliderPos;
        yield return new WaitForSecondsRealtime(0.1f);

        _unlockParticle.Play();

        yield return new WaitForSecondsRealtime(1f);
        _unlockParticle.Stop();

        _UnlockPlanet(_toUnlock);
    }

    private void _UnlockPlanet(int _planetId)
    {
        GlobalVars.saveData.gameData.planetData[_planetId].unlocked = true;
        _planets[_planetId]._unlocked = true;
        //_planets[_planetId]._UnlockCircle.gameObject.SetActive(false);
        _planets[_planetId].Init();

        alertsMessages.ShowPlanetUnlockAlert(_planets[_planetId]._planet._name);

        if (_planetId > 0 && _planetId < GlobalVars.maxPlanets - 1)
        {
            nextBtn.interactable = true;
            prevBtn.interactable = true;
        }

        //Last Planet
        else if (_planetId == GlobalVars.maxPlanets - 1)
        {
            prevBtn.interactable = true;
        }

        //First Planet
        else
        {
            nextBtn.interactable = true;
        }
        GameEvents.CheckPlanetAchievement();
        GlobalVars.saveData.gameData.planetToUnlock = -1;
        SaveSystem.SaveData();
    }

    public void OnPurchaseSuccess(BuyStatus _status, FailedReason _reason, string productId)
    {
        string _name = localizationTable.GetFieldText(productId + ".name");
        alertsMessages.ShowSuccessAlert(_name);
        _planetPowerUpController._OcultarNoPowerUp(false);
        _planetPowerUpController._AsignarPostCompra();
    }

    public void OnPurchaseFail(BuyStatus _status, FailedReason _reason, string _productId)
    {
        alertsMessages.ShowFailedAlert(_reason);
        _planetPowerUpController._OcultarNoPowerUp(false);
    }
}
