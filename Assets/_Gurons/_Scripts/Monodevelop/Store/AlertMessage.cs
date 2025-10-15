//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

using TMPro;

//public enum BuyStatus
//{
//    failed,
//    completed
//}

//public enum FailedReason
//{
//    ProductUnavailable,
//    UserCancelled,
//    PaymentDeclined,
//    InsufficientFounds,
//    NoUnlockDiamonds
//}

//public class AlertMessage : MonoBehaviour 
//{
//    [Header("Mensajes")]
//    [SerializeField] private TextMeshProUGUI title;
//    [SerializeField] private TextMeshProUGUI message;
//    [SerializeField] private TextTable _localizedTable;

//    public void ShowAlert(BuyStatus _buyStatus, string productName)
//    {
//        title.text = _localizedTable.GetFieldText("Store.Completed");
//        message.text = string.Format(_localizedTable.GetFieldText("Store.Completed.Message"), productName);

//        //gameObject.SetActive(true);
//    }

//    //TODO: CORREGIR MENSAJE DESBLOQUEO
//    public void ShowAlert(BuyStatus _buyStatus, FailedReason _reason, string productName)
//    {
//        title.text = _localizedTable.GetFieldText("Store.Failed");

//        switch(_reason)
//        {
//            case FailedReason.ProductUnavailable:
//                message.text = _localizedTable.GetFieldText("Store.Failed.ProductUnavailable");
//                break;

//            case FailedReason.UserCancelled:
//                message.text = _localizedTable.GetFieldText("Store.Failed.UserCancelled");
//                break;

//            case FailedReason.PaymentDeclined:
//                message.text = _localizedTable.GetFieldText("Store.Failed.PaymentDeclined");
//                break;

//            case FailedReason.InsufficientFounds:
//                message.text = _localizedTable.GetFieldText("Store.Failed.InsufficientFounds");
//                break;
//        }

//        gameObject.SetActive(true);
//    }
//}
