//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

//using UnityEngine.Purchasing;
using TMPro;
using UnityEngine.Events;
//using PixelCrushers.DialogueSystem;
using UnityEngine.UI;
using PixelCrushers;

//using CodeStage.AntiCheat.ObscuredTypes;

public class StoreProduct : MonoBehaviour
{

    //[System.Serializable]
    //public class OnPurchaseCompletedEvent : UnityEvent<Product> { };

    //[System.Serializable]
    //public class OnPurchaseFailedEvent : UnityEvent<Product, PurchaseFailureReason> { };
    [SerializeField] private bool _storeButton = true;
    [Space]

    public string productId = "";
    [SerializeField] private string productNameString = "";
    [SerializeField] private GameObject infoPanel;
    private bool _infoActive = false;

    [Space]
    [Header("Precios")]
    [SerializeField] private int boughtAmount = 0;
    [SerializeField] private int diamondPrice = 0;
    [SerializeField] private int coinPrice = 0;

    [Header("Descuento")]
    [SerializeField] private bool _discount = false;
    [SerializeField] private int _discountValue = 10;
    [SerializeField] private TextMeshProUGUI _discountAmount;
    [SerializeField] private GameObject _discountIcon;

    [Space]
    [Header("Tipo de Compra")]
    [SerializeField] private bool _market = false;
    [SerializeField] private bool _diamond = false;
    [SerializeField] private bool _coin = false;

    [Space]
    [Header("Tipo de Producto")]
    [SerializeField] private bool _lifeProduct = false;
    [SerializeField] private bool _diamondProduct = false;
    [SerializeField] private bool _coinProduct = false;
    [SerializeField] private bool _chanceProduct = false;
    [SerializeField] private bool _scoreProduct = false;


    [Space]
    [Header("Botones")]
    [SerializeField] private GameObject marketButton;
    [SerializeField] private GameObject diamondButton;
    [SerializeField] private GameObject coinButton;

    [Space]
    [Header("Datos De Producto")]
    [SerializeField] private TextMeshProUGUI productName = null;
    [SerializeField] private TextMeshProUGUI productDescription = null;
    [SerializeField] private TextMeshProUGUI productMarket = null;
    [SerializeField] private TextMeshProUGUI productDiamond = null;
    [SerializeField] private TextMeshProUGUI productCoins = null;

    public void _Init(string _name, string _description)
    {

        if (!string.IsNullOrEmpty(productId))
        {
            productNameString = _name;

            if (_storeButton)
            {

                infoPanel.SetActive(false);
                _infoActive = false;

                productName.text = _name;

                productDescription.text = _description;

                //MUESTRA ETIQUETA DE DESCUENTO
                if (_discount)
                {
                    _discountAmount.text = $"-{_discountValue}%";
                    _discountIcon.SetActive(true);
                }
                else
                {
                    _discountIcon.SetActive(false);
                }

                //ACTIVA BOTON SEGUN LOS TIPOS DE COMPRA DISPONIBLE
                marketButton?.SetActive(_market);
                diamondButton?.SetActive(_diamond);
                coinButton?.SetActive(_coin);

                //ASIGNA VALOR EN DIAMANTES
                if (_diamond)
                {
                    productDiamond.text = diamondPrice.ToString("###");
                }

                //ASIGNA VALOR EN MONEDAS
                if (_coin)
                {
                    productCoins.text = coinPrice.ToString("###");
                }

                //ASIGNA VALOR EN DINERO
                if (_market)
                {
                    marketButton.GetComponent<Button>().interactable = true;
                    productMarket.text = Store.instance.GetLocalizedPrice(productId);
                    //if (GlobalVars.connected)
                    //{
                    //    //DEJA COMO INTERACTABLE EL BOTÓN
                    //    marketButton.GetComponent<Button>().interactable = true;
                    //    productMarket.text = Store.instance.GetLocalizedPrice(productId);
                    //}

                    ////NO ESTÁ CONECTADO A INET, NO HAY PRECIOS
                    //else
                    //{
                    //    marketButton.GetComponent<Button>().interactable = false;
                    //    productMarket.text = "----";
                    //}
                }
            }

            else
            {
                //ASIGNA VALOR EN DINERO
                if (_market)
                {
                    marketButton.SetActive(true);
                    marketButton.GetComponent<Button>().interactable = true;
                    productMarket.text = Store.instance.GetLocalizedPrice(productId);
                    //if (GlobalVars.connected)
                    //{
                    //    //DEJA COMO INTERACTABLE EL BOTÓN
                    //    marketButton.GetComponent<Button>().interactable = true;
                    //    productMarket.text = Store.instance.GetLocalizedPrice(productId);
                    //    //productMarket.text = p.metadata.localizedPriceString;
                    //}

                    ////NO ESTÁ CONECTADO A INET, NO HAY PRECIOS
                    //else
                    //{
                    //    marketButton.GetComponent<Button>().interactable = false;
                    //    productMarket.text = "----";
                    //}
                }
            }
        }
    }

    public void ActiveInfo()
    {
        _infoActive = !_infoActive;
        infoPanel.SetActive(_infoActive);
    }

    void OnDisable()
    {
        if (!string.IsNullOrEmpty(productId))
        {
        }
    }

    public void BuyProductWithMarket()
    {
        Store.instance.BuyProductWithMarket(productId);
    }

    public void BuyProductWithCurrency()
    {
        Constants._Currencies _boughtItem = Constants._Currencies.none;
        Constants._Currencies _payWith = Constants._Currencies.none;

        int price = 0;

        if (_lifeProduct)
        {
            _boughtItem = Constants._Currencies.one_life;
        }
        else if (_diamondProduct)
        {
            _boughtItem = Constants._Currencies.diamond_currency;
        }
        else if (_coinProduct)
        {
            _boughtItem = Constants._Currencies.coin_currency;
        }
        else if (_scoreProduct)
        {
            _boughtItem = Constants._Currencies.double_score;
        }
        else if (_chanceProduct)
        {
            _boughtItem = Constants._Currencies.extra_chance;
        }


        if (_coin)
        {
            _payWith = Constants._Currencies.coin_currency;
            price = coinPrice;
        }
        else if (_diamond)
        {
            _payWith = Constants._Currencies.diamond_currency;
            price = diamondPrice;
        }


        Debug.Log(productId + " Bought ");
        Store.instance.BuyProductWithCurrency(productId, price, _payWith, _boughtItem, boughtAmount);
    }

/*
    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        FailedReason _failedReason = FailedReason.None;

        switch (reason)
        {
            case PurchaseFailureReason.ProductUnavailable:
                _failedReason = FailedReason.ProductUnavailable;
                //alertsMessages.ShowAlert(BuyStatus.failed, FailedReason.ProductUnavailable, productNameString);
                break;

            case PurchaseFailureReason.PurchasingUnavailable:
                _failedReason = FailedReason.ProductUnavailable;

                //alertsMessages.ShowAlert(BuyStatus.failed, FailedReason.UserCancelled, productNameString);
                break;

            case PurchaseFailureReason.PaymentDeclined:
                _failedReason = FailedReason.PaymentDeclined;

                //alertsMessages.ShowAlert(BuyStatus.failed, FailedReason.PaymentDeclined, productNameString);
                break;

            case PurchaseFailureReason.Unknown:
                _failedReason = FailedReason.InsufficientFounds;

                //alertsMessages.ShowAlert(BuyStatus.failed, FailedReason.InsufficientFounds, productNameString);
                break;
        }
    }
    */


}
