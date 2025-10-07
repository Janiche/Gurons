//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;

public class Planet : MonoBehaviour
{
    public PlanetBP _planet = null;

    [Header("Datos Visuales")]
    [SerializeField] private TextMeshProUGUI _name = null;
    [SerializeField] private Image _sprite = null;
    [SerializeField] private TextMeshProUGUI _unlockCost = null;
    [SerializeField] private TextMeshProUGUI _gravity = null;
    [SerializeField] private TextMeshProUGUI _wind = null;
    [SerializeField] private TextMeshProUGUI _progress = null;
    [SerializeField] private TextMeshProUGUI _record = null;

    [SerializeField] private ParticleSystem _unlockParticle;
    public ParticleSystem UnlockParticle
    {
        get { return _unlockParticle; }
    }

    public Image _PlanetImage
    {
        get { return _sprite; }
    }

    [Header("Datos Funcionales")]
    public bool _unlocked = false;
    public Outline _outline;
    public GameObject _data = null;
    public GameObject _unlock = null;

    [Header("Alertas")]
    [SerializeField] AlertPanel alertsMessages;
    //[SerializeField] AlertMessage alertsMessages;

    public void Init()
    {
        _sprite.sprite = _planet._sprite as Sprite;
        _sprite.preserveAspect = true;
        _name.text = _planet._name;

        if (_unlocked)
        {
            _data.SetActive(true);
            //_unlock.SetActive(false);
            GetComponent<Button>().interactable = true;

            _sprite.color = Color.white;
        }
        else
        {
            //_unlock.SetActive(true);
            _data.SetActive(false);
            GetComponent<Button>().interactable = false;
            _sprite.color = Color.black;
        }

        //_unlockCost.text = _planet._unlockCost.ToString("####");


        _outline = _sprite.gameObject.GetComponent<Outline>();

        _gravity.text = _planet._gravity.ToString("#.00");
        _wind.text = (_planet._wind > 0) ? _planet._wind.ToString("#.##") : "----";

        _outline.enabled = false;

        if (GlobalVars.saveData == null)
            return;

        //Debug.Log("LOADED");
        _progress.text = string.Format("{0}/{1}", GlobalVars.saveData.gameData.planetData[_planet.id].maxBlox, _planet._toComplete);
        _record.text = GlobalVars.saveData.gameData.planetData[_planet.id].bestRecord.ToString("###0");
    }

    /// <summary>
    /// Selecciona o deselecciona el planeta
    /// </summary>
    public void OnPlanet()
    {
        GlobalVars.currentPlanet = _planet.id;
        GlobalVars.currentPlanetName = _planet._name;
        GlobalVars.nextPlanet = _planet.id + 1;
        _outline.enabled = !_outline.isActiveAndEnabled;

        GlobalVars.gravity = _planet._gravity;
        GlobalVars.wind = _planet._wind;
        GlobalVars.bloxToComplete = _planet._toComplete;
    }

    public void Unlock()
    {
        //DIAMANTES SUFICIENTES PARA DESBLOQUEAR
        if ((Store.instance.GetBalance(Constants._Currencies.diamond_currency) - _planet._unlockCost) > 0)
        {
            _unlocked = true;
            _data.SetActive(true);
            _unlock.SetActive(false);
            Store.instance.TakeProduct(Constants._Currencies.diamond_currency, _planet._unlockCost);
            GlobalVars.saveData.gameData.planetData[_planet.id].unlocked = true;
            SaveSystem.SaveData();
        }

        //DIAMANTES INSUFICIENTES, ERROR
        else
        {
            //alertsMessages.ShowAlert(BuyStatus.failed, FailedReason.InsufficientFounds, Constants._Currencies.diamond_currency);
            alertsMessages.ShowFailedAlert(FailedReason.InsufficientFounds);
        }
    }

    public void _ActiveOutline(bool _active)
    {
        _outline.enabled = _active;
    }

    public bool _GetOutlineActive()
    {
        return _outline.enabled;
    }
}
