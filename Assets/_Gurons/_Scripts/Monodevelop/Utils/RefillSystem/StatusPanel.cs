//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

using TMPro;
using DG.Tweening;

public class StatusPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _lifeCount = null;
    [SerializeField] private TextMeshProUGUI _diamondCount = null;
    [SerializeField] private TextMeshProUGUI _coinCount = null;

    [SerializeField] private float _scaleMultiplier = 0.1f;
    [SerializeField] private float _time = 0.5f;


    void OnEnable()
    {
        Store.OnChangeValue += OnChangeValue;

        _lifeCount.text = Store.instance.GetBalance(Constants._Currencies.one_life).ToString("00");
        _diamondCount.text = Store.instance.GetBalance(Constants._Currencies.diamond_currency).ToString("00");
        _coinCount.text = Store.instance.GetBalance(Constants._Currencies.coin_currency).ToString("00");

    }

    void OnDisable()
    {
        Store.OnChangeValue -= OnChangeValue;
    }

    void OnChangeValue(Constants._Currencies _type, double _amount)
    {
        switch (_type)
        {
            case Constants._Currencies.one_life:
                _lifeCount.text = Store.instance.GetBalance(Constants._Currencies.one_life).ToString("00");

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
        //Vector3 _midScale = new Vector3(_originalScale.x + _scaleMultiplier, _originalScale.y + _scaleMultiplier, _originalScale.z + _scaleMultiplier);
        Vector3 _midScale = _originalScale + (Vector3.one * _scaleMultiplier);

        _text.DOScale(_midScale, _time).OnComplete(() =>
        _text.DOScale(_originalScale, _time)
        );
    }
}
