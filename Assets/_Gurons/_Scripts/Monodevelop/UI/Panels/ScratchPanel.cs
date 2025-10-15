using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

using PixelCrushers;
using Bermost;

public class ScratchPanel : UIPanelAnimator
{

    #region VARIABLES 

    [SerializeField] private Camera _cam;
    [SerializeField] private TextTable _textTable;

    [SerializeField] private AlertPanel _alertPanel;
    [SerializeField] private Color _inactiveColor;

    [Space]
    [Header("Prize")]
    [SerializeField] private Image _prizeImage;
    [SerializeField] private TextMeshProUGUI _prizeText;

    [Space]
    [Header("Scratch")]
    [SerializeField] private Image _blockedScratchableZone;
    [SerializeField] private Image _clearScratchableZone;
    [SerializeField] private float _scratchSpeed = 60;
    [SerializeField] private float _scratchedPercent = 0;
    [SerializeField] private Transform _instruction;
    [SerializeField] private float _maxSize = 2;
    [SerializeField] private GameObject _hand;
    [SerializeField] private GameObject _particle;

    int _width;
    int _height;

    [Space]
    [Header("Cost")]
    [SerializeField] private TextMeshProUGUI _scratchPrice;
    [SerializeField] private Button _buyScratch;
    [SerializeField] private Button _close;
    [SerializeField] private Image _scratch;

    [SerializeField] private int[] _priceCost = new int[] { 0, 150, 300, 600, 100 };
    [SerializeField] private int _currentCost = 0;

    [SerializeField] private int _currentPrize = -1;
    [SerializeField] private List<ScratchPrize> _prizesList = new List<ScratchPrize>();
    int count = 0;

    private float _total = -10;

    #endregion

    private void OnEnable()
    {
        _ResetScratch();

        if (_total < 0)
        {
            _total = 0;
            count = _prizesList.Count;

            for (int i = 0; i < count; i++)
            {
                float _tempTotal = _total;
                _total += _prizesList[i]._percent;
                _prizesList[i]._percent += _tempTotal;

            }
        }
    }

    void _ResetScratch()
    {

        _width = (int)_blockedScratchableZone.rectTransform.rect.width;
        _height = (int)_blockedScratchableZone.rectTransform.rect.height;

        _currentCost = GlobalVars.saveData.playerData.scratchIndex;

        _currentCost = Mathf.Clamp(_currentCost, 0, _priceCost.Length - 1);

        _blockedScratchableZone.raycastTarget = false;
        _blockedScratchableZone.gameObject.SetActive(true);
        _hand.SetActive(false);
        _instruction.gameObject.SetActive(false);
        _buyScratch.interactable = true;
        _close.interactable = true;
        _clearScratchableZone.gameObject.SetActive(false);

        _blockedScratchableZone.color = _inactiveColor;
        _scratch.color = _inactiveColor;

        _instruction.localScale = Vector3.one;

        _scratchedPercent = 0;

        _scratchPrice.text = (_priceCost[_currentCost] > 0) ? _priceCost[_currentCost].ToString() : _textTable.GetFieldText("Store.Free");
    }

    public void _BuyTicket()
    {


        if (Store.instance.GetBalance(Constants._Currencies.coin_currency) >= _priceCost[_currentCost])
        {
            Store.instance.TakeProduct(Constants._Currencies.coin_currency, _priceCost[_currentCost]);

            _buyScratch.interactable = false;
            _close.interactable = false;

            _blockedScratchableZone.raycastTarget = true;
            _clearScratchableZone.gameObject.SetActive(true);
            _instruction.gameObject.SetActive(true);
            _hand.SetActive(true);

            _blockedScratchableZone.color = Color.white;
            _scratch.color = Color.white;

            _SortPrize();

            if (_currentCost < _priceCost.Length - 1)
            {
                _currentCost++;
            }

            _currentCost = Mathf.Clamp(_currentCost, 0, _priceCost.Length - 1);

        }
        else
        {
            _alertPanel.ShowFailedAlert(FailedReason.InsufficientCoins);
        }
    }


    private void _SortPrize()
    {
        float rand = Random.Range(0, _total);

        Debug.Log(rand);

        for (int i = 0; i < count; i++)
        {
            if (rand <= _prizesList[i]._percent)
            {
                //_currentPrize = _prizesList[i];
                _currentPrize = i;
                break;
            }
        }


        string _id = string.Empty;

        Constants._Currencies _currency = _prizesList[_currentPrize]._product;

        switch (_currency)
        {
            case Constants._Currencies.double_score:
                _id = "Scratch.DoubleScore";
                break;

            case Constants._Currencies.extra_chance:
                _id = "Scratch.ExtraChance";
                break;
            case Constants._Currencies.coin_currency:
                _id = "Scratch.Coins";
                break;

        }

        _prizeImage.sprite = _prizesList[_currentPrize]._prizeIcon;
        _prizeText.text = $"{_textTable.GetFieldText(_id)} x{_prizesList[_currentPrize]._amount}";
        //_prizeText.text = _textTable.GetFieldText(_id) + " x" + _prizesList[_currentPrize]._amount.ToString();

    }


    /*public void _Scratch(Gesture _gesture)
    {
        Vector2 _realPos = _gesture.ActivePointers[0].Position;

        Vector2 _relativePos = new Vector2();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(_blockedScratchableZone.rectTransform, _realPos, _cam, out _relativePos);

        if (Mathf.Abs(_relativePos.x) < _width / 2 && Mathf.Abs(_relativePos.y) < _height / 2)
        {
            if (_scratchedPercent < 100)
            {
                _scratchedPercent += (Time.deltaTime * _scratchSpeed);

                _instruction.localScale = Vector3.one * (1 + (_scratchedPercent / 100));
            }
            else
            {
                _EndScratch();
            }

        }

    }*/

    private void _EndScratch()
    {
        GlobalVars.saveData.playerData.scratchIndex = _currentCost;
        _particle.SetActive(true);
        _instruction.gameObject.SetActive(false);

        _blockedScratchableZone.raycastTarget = false;

        _blockedScratchableZone.gameObject.SetActive(false);
        _clearScratchableZone.gameObject.SetActive(true);
        _hand.SetActive(false);

        _GivePrize();

        SaveSystem.SaveData();

        StartCoroutine(_WaitParticle());
    }


    private void _GivePrize()
    {
        if (_currentPrize > 0)
        {
            Store.instance.GiveProduct(_prizesList[_currentPrize]._product, _prizesList[_currentPrize]._amount);
        }
    }

    IEnumerator _WaitParticle()
    {
        yield return new WaitForSecondsRealtime(4);
        _ResetScratch();
    }
}