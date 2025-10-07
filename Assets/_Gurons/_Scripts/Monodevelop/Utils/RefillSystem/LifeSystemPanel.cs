using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

using UnityEngine.Events;
using DG.Tweening;
using System;

public class LifeSystemPanel : MonoBehaviour
{

    #region VARIABLES
    [Header("Datos")]
    [Tooltip("Campo de cantidad de vidas actuales")] [SerializeField] private TextMeshProUGUI _lifeCount = null;
    [Tooltip("Campo de cantidad de diamantes actuales")] [SerializeField] private TextMeshProUGUI _diamondCount = null;
    [Tooltip("Campo de cantidad de diamantes actuales")] [SerializeField] private TextMeshProUGUI _coinCount = null;
    [Tooltip("Campo temporizador de proxima carga de vida")] [SerializeField] private TextMeshProUGUI _timer = null;
    [Tooltip("Campo de cantidad de vidas pasado temporizador")] [SerializeField] private TextMeshProUGUI _nextLife = null;

    [Header("Panel y Posiciones")]
    [Tooltip("Panel de Cronometro")] [SerializeField] private GameObject _refillCounter = null;

    [SerializeField] private bool _timerActive = false;

    [Space]
    [SerializeField] private float _scaleMultiplier = 0.1f;
    [SerializeField] private float _time = 0.5f;
    #endregion


    [Space]
    [SerializeField] private UnityEvent StartRefill;
    [SerializeField] private UnityEvent ProgressRefill;
    [SerializeField] private UnityEvent ResetRefill;
    [SerializeField] private UnityEvent EndRefill;
    [SerializeField] private UnityEvent UpdateRefill;

    private int _lifes = 0;

    private void OnEnable()
    {
        Store.OnChangeValue += OnChangeValue;

        _lifes = Store.instance.GetBalance(Constants._Currencies.one_life);

        _lifeCount.text = _lifes.ToString("00");
        _nextLife.text = (_lifes + 1).ToString("00");
        _diamondCount.text = Store.instance.GetBalance(Constants._Currencies.diamond_currency).ToString("00");
        _coinCount.text = Store.instance.GetBalance(Constants._Currencies.coin_currency).ToString("00");

        LifeSystemController.OnStartRefill += OnStartRefill;
        LifeSystemController.OnProgressRefill += OnProgressRefill;
        LifeSystemController.OnResetRefill += OnResetRefill;
        LifeSystemController.OnEndRefill += OnEndRefill;
        LifeSystemController.OnUpdateRefill += OnUpdateRefill;
    }

    private void OnDisable()
    {
        Store.OnChangeValue -= OnChangeValue;

        LifeSystemController.OnStartRefill -= OnStartRefill;
        LifeSystemController.OnProgressRefill -= OnProgressRefill;
        LifeSystemController.OnResetRefill -= OnResetRefill;
        LifeSystemController.OnEndRefill -= OnEndRefill;
        LifeSystemController.OnUpdateRefill -= OnUpdateRefill;
    }




    void OnChangeValue(Constants._Currencies _type, double _amount)
    {
        switch (_type)
        {
            case Constants._Currencies.one_life:
                _lifes = Store.instance.GetBalance(Constants._Currencies.one_life);

                _lifeCount.text = _lifes.ToString("00");
                _nextLife.text = (_lifes + 1).ToString();

                _SizeUp(_lifeCount.transform);

                break;
            case Constants._Currencies.diamond_currency:
                _diamondCount.text = Store.instance.GetBalance(Constants._Currencies.diamond_currency).ToString("00");

                _SizeUp(_diamondCount.transform);


                break;
            case Constants._Currencies.coin_currency:
                _coinCount.text = Store.instance.GetBalance(Constants._Currencies.coin_currency).ToString("00");

                _SizeUp(_coinCount.transform);

                break;
        }
    }

    private void _SizeUp(Transform _text)
    {
        Vector3 _originalScale = _text.localScale;
        Vector3 _midScale = _originalScale + (Vector3.one * _scaleMultiplier);

        _text.DOScale(_midScale, _time).OnComplete(() =>
        _text.DOScale(_originalScale, _time)
        );
    }





    private void OnStartRefill(object _data)
    {
        _refillCounter.SetActive((bool)_data);

        StartRefill.Invoke();
    }

    private void OnProgressRefill(object _data)
    {
        TimeSpan t = (TimeSpan)_data;

        _timer.text = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
        _timerActive = true;

        _refillCounter.SetActive(_timerActive);
        _nextLife.text = (_lifes + 1).ToString("0");

        ProgressRefill.Invoke();
    }

    private void OnResetRefill(object _data)
    {
        _lifeCount.text = ((int)_data).ToString();
        _nextLife.text = ((int)_data + 1).ToString();

        ResetRefill.Invoke();
    }

    private void OnEndRefill(object _data)
    {

        _timerActive = false;
        _refillCounter.SetActive(false);

        _refillCounter.SetActive(_timerActive);

        EndRefill.Invoke();
    }

    private void OnUpdateRefill(object _data)
    {
        UpdateRefill.Invoke();
        _lifeCount.text = ((int)_data).ToString();
        _nextLife.text = ((int)_data + 1).ToString();
    }
}
