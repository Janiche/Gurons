
using System.Collections.Generic;
using UnityEngine;

using PixelCrushers;
using UnityEngine.Events;

public class StoreController : MonoBehaviour
{
    [Header("Alertas")]
    [SerializeField] AlertPanel alertsMessages;
    [SerializeField] private GameObject _blackPanel;

    [Header("Eventos")]
    public UnityEvent onPurchaseComplete;
    public UnityEvent onPurchaseFailed;

    [Space]
    [Header("Localización")]
    [SerializeField] private TextTable localizationTable = null;

    [Space]
    [SerializeField] private List<StoreProduct> productList;

    void OnEnable()
    {
        if (MusicInstance.instance != null)
        {
            MusicInstance.instance._Pause();
        }

        _Init();
    }

    private void OnDisable()
    {
        Store.OnPurchaseFail -= OnPurchaseFail;
        Store.OnPurchaseSuccess -= OnPurchaseSuccess;
    }

    private void _Init()
    {
        GlobalVars._transactionContext = Constants.TransactionContext.ON_STORE;
/*
        if (AdsManager.instance != null)
        {
            AdsManager.instance._ShowBanner(true);
        }
*/
        UpdateStore();

        Store.OnPurchaseFail += OnPurchaseFail;
        Store.OnPurchaseSuccess += OnPurchaseSuccess;

    }

    public void UpdateStore()
    {
        foreach (StoreProduct p in productList)
        {
            string _name = localizationTable.GetFieldText(p.productId + ".name");
            string _desc = localizationTable.GetFieldText(p.productId + ".description");

            p._Init(_name, _desc);
        }
    }

    public void OnPurchaseSuccess(BuyStatus _status, FailedReason _reason, string _productId)
    {
        string _name = localizationTable.GetFieldText(_productId + ".name");

        alertsMessages.ShowSuccessAlert(_name);
        _blackPanel.SetActive(true);
        onPurchaseComplete.Invoke();
    }

    public void OnPurchaseFail(BuyStatus _status, FailedReason _reason, string _productId)
    {
        //string _name = localizationTable.GetFieldText(_productId + ".name");
        alertsMessages.ShowFailedAlert(_reason);
        _blackPanel.SetActive(true);
        onPurchaseFailed.Invoke();
    }
}
