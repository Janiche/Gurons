using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RateAppPanel : MonoBehaviour
{
    [Header("Rate Panel Variables")]
    [SerializeField] private string gplayRate = "market://details?id=";
    [SerializeField] private string iosRate = "";

    /// <summary>
    /// Redirecciona a pag de rate segun Plataforma (IOS/Android/Xiaomi)
    /// </summary>
    public void RateNow()
    {
        string url = "";
#if UNITY_ANDROID
        url = gplayRate + Application.identifier;
        Application.OpenURL(url);
#elif UNITY_IOS
        url = string.Format("itms - apps://itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?id={0}&onlyLatestVersion=true&pageNumber=0&sortOrdering=1&type=Purple+Software", iosRate);
#endif
        GlobalVars.showRate = false;
        RateNever();
    }

    /// <summary>
    /// Posterga Aparicion de Rate para proxima Fecha (+2 dias)
    /// </summary>
    public void RateLater()
    {
        GlobalVars.showRate = false;
        GlobalVars.saveData.playerData.nextRate = System.DateTime.Now.Date.AddDays(GlobalVars.timeToRate);
        SaveSystem.SaveData();
    }

    /// <summary>
    /// No vuelve a activar Rate
    /// </summary>
    public void RateNever()
    {
        GlobalVars.showRate = false;
        GlobalVars.saveData.playerData.rated = true;
        SaveSystem.SaveData();
    }
}
