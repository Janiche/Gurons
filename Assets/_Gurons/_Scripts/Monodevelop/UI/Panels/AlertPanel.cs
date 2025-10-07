using UnityEngine;

using TMPro;
using PixelCrushers;
using Doozy.Engine.UI;

public enum BuyStatus
{
    failed,
    completed
}

public enum FailedReason
{
    None,
    DuplicateTransaction,
    ExistingPurchasePending,
    PaymentDeclined,
    ProductUnavailable,
    PurchasingUnavailable,
    SignatureInvalid,
    UserCancelled,
    Unknown,

    InsufficientFounds,
    InsufficientCoins,
}


public class AlertPanel : MonoBehaviour
{
    [Header("Alert Panel Variables")]
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI message;
    [SerializeField] private TextTable _localizedTable;

    [SerializeField] private UIView _view;

    public void ShowSuccessAlert(string productId)
    {
        title.text = _localizedTable.GetFieldText("Store.Completed");
        message.text = string.Format(_localizedTable.GetFieldText("Store.Completed.Message"), productId);

        _view.Show();
    }

    public void ShowFailedAlert(FailedReason _reason)
    {
        title.text = _localizedTable.GetFieldText("Store.Failed");

        message.text = _localizedTable.GetFieldText("Store.Failed.Unknown");

        gameObject.SetActive(true);
    }

    public void ShowPlanetUnlockAlert(string _planetName)
    {
        title.text = _localizedTable.GetFieldText("Planet.Unlocked.Title");
        message.text = string.Format(_localizedTable.GetFieldText("Planet.Unlocked.Message"), _planetName);

        _view.Show();
    }

}
