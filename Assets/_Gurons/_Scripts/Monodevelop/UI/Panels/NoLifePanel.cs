using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NoLifePanel : MonoBehaviour
{

    public void _EndOpen()
    {
        //AdsManager.OnFinishedAd += FinishAd;
    }

    public void _EndClose()
    {
        //AdsManager.OnFinishedAd -= FinishAd;
    }

    public void _LoadAd()
    {
        //AdsManager.instance._ShowRewarded();
    }

    private void FinishAd(string placementId)
    {
        Store.instance.GiveProduct(Constants._Currencies.one_life, 1);
    }
}
