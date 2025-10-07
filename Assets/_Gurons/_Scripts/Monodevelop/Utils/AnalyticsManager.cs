using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Analytics;


public class AnalyticsManager : MonoBehaviour
{
    private static AnalyticsManager instance;
    public static AnalyticsManager _instance { get { return instance; } }

    public void _Init()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }




    public void _GameOpened(string version)
    {
        Debug.LogError($"game_opened V: {version}");

        Dictionary<string, object> _args = new Dictionary<string, object>
        {
            {"version", version }
        };

        AnalyticsEvent.Custom("game_opened", _args);
    }








    public void _StartGame()
    {
        Debug.LogError("start_game");

        AnalyticsEvent.GameStart();
    }
    public void _GameOver(int score, int level)
    {
        Debug.LogError($"game_over S: {score} L: {level}");

        Dictionary<string, object> _args = new Dictionary<string, object>
        {
            {"score", score },
            {"level", level}
        };


        AnalyticsEvent.GameOver(string.Empty, _args);
    }
    public void _ExitGame(int score, int level)
    {
        Debug.LogError($"game_quit S: {score} L: {level}");

        Dictionary<string, object> _args = new Dictionary<string, object>
        {
            {"score", score },
            {"level", level}
        };

        AnalyticsEvent.Custom("game_quit", _args);
    }
    public void _LevelStart(string level)
    {
        Debug.LogError($"level_start L: {level}");
        AnalyticsEvent.LevelStart(level);
    }
    public void _LevelFail(string level)
    {
        Debug.LogError($"level_fail L: {level}");
        AnalyticsEvent.LevelFail(level);
    }
    public void _LevelComplete(string level)
    {
        Debug.LogError($"level_complete L: {level}");
        AnalyticsEvent.LevelComplete(level);
    }





    public void _StartTutorial()
    {
        Debug.LogError("start_tutorial");

        AnalyticsEvent.TutorialStart();
    }
    public void _SkipTutorial()
    {
        Debug.LogError("skip_tutorial");
        AnalyticsEvent.TutorialSkip();
    }
    public void _EndTutorial()
    {
        Debug.LogError("end_tutorial");

        AnalyticsEvent.TutorialComplete();
    }







    public void _AchievementUnlock(string achievement)
    {
        Debug.LogError($"achievement_unlocked {achievement}");
        AnalyticsEvent.AchievementUnlocked(achievement);
    }






    public void _StoreOpened()
    {
        Debug.LogError("store_opened");
        AnalyticsEvent.StoreOpened(StoreType.Premium);
    }
    public void _Transaction(string productId, decimal amount, string currency)
    {
        Analytics.Transaction(productId, amount, currency);
    }
    public void _IapTransaction(string transactionContext, float price, string productId)
    {
        Debug.LogError($"iap_transaction {transactionContext} - {price} - {productId}");
        AnalyticsEvent.IAPTransaction(transactionContext, price, productId);
    }
    public void _ItemAcquired(AcquisitionType acquisitionType, string transactionContext, int amount, int balance, string productId)
    {
        Debug.LogError($"item_acquired {acquisitionType} - {transactionContext} - {amount} - {balance} - {productId}");
        AnalyticsEvent.ItemAcquired(acquisitionType, transactionContext, amount, productId, balance);
    }
    public void _ItemSpend(AcquisitionType acquisitionType, string transactionContext, int amount, int balance, string productId)
    {
        Debug.LogError($"item_spend {acquisitionType} - {transactionContext} - {amount} - {balance} - {productId}");
        AnalyticsEvent.ItemSpent(acquisitionType, transactionContext, amount, productId, balance);
    }
    public void _SetStore(string store)
    {
        Debug.LogError($"user_store {store}");
        Dictionary<string, object> _param = new Dictionary<string, object>()
        {
            { "store", store.ToString() }
        };

        Analytics.CustomEvent("user_store", _param);
    }






    public void _AdOffer()
    {
        Debug.LogError("ad_offer");
        //AnalyticsEvent.AdOffer(true, null, ADS_PLACEMENT.Ads_Placement_Rewarded);
    }
    public void _StartRewarded(int score, string planet)
    {
        Debug.LogError($"ad_start S: {score} L: {planet}");

        Dictionary<string, object> _args = new Dictionary<string, object>
        {
            {"score", score },
            {"planet", planet}
        };

        //AnalyticsEvent.AdStart(true, AdvertisingNetwork.Appodeal, ADS_PLACEMENT.Ads_Placement_Rewarded, _args);
    }
    public void _ClickRewarded()
    {
        Debug.LogError("rewarded_click");

        AnalyticsEvent.Custom("intertitial_click");
    }
    public void _ClickBanner(string scene)
    {
        Debug.LogError($"banner_click S: {scene}");

        Dictionary<string, object> _args = new Dictionary<string, object>
        {
            {"scene_name", scene }
        };
        AnalyticsEvent.Custom("banner_click", _args);
    }






    public void _ChangeLanguage(string langCode)
    {
        Debug.LogError($"language_changed L: {langCode}");

        Dictionary<string, object> _args = new Dictionary<string, object>
        {
            {"language_code", langCode}
        };
        AnalyticsEvent.Custom("language_changed", _args);
    }


}
