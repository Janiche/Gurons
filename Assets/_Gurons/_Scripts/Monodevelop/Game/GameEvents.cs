using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents
{

    public delegate void GameModeEvents();
    public delegate void GuronEvents();
    public delegate void UIEvents(Constants._UICommand _uiCommand);
    public delegate void TakePoolEvents(Constants._PoolType _poolType, Constants._SelectionType _selectionType);
    public delegate void PlacePoolEvent(Constants._PoolType _poolType, GameObject _poolObject);


    public delegate void AchievementEvents();


    //public delegate void AdEvent();

    #region GameModeEvents
    public static GameModeEvents OnChangeScore;
    public static GameModeEvents OnChangeGuron;
    public static GameModeEvents OnPause;
    public static GameModeEvents OnUnpause;
    public static GameModeEvents OnGameOver;
    public static GameModeEvents OnUnlockPlanet;

    public static GameModeEvents OnChangeScene;

    public static GameModeEvents OnSuccessTutorial;
    public static GameModeEvents OnFailedTutorial;

    /// <summary>
    /// Funcion delegada al cambiar el score
    /// </summary>
    public static void ChangeScore()
    {
        OnChangeScore?.Invoke();
    }

    public static void ChangeGuron()
    {
        OnChangeGuron?.Invoke();
    }

    public static void Pause()
    {
        OnPause?.Invoke();
    }

    public static void Unpause()
    {
        OnUnpause?.Invoke();
    }

    public static void GameOver()
    {
        OnGameOver?.Invoke();
    }

    public static void UnlockPlanet()
    {
        OnUnlockPlanet?.Invoke();
    }

    public static void ChangeScene()
    {
        OnChangeScene?.Invoke();
    }

    public static void SuccessTutorial()
    {
        OnSuccessTutorial?.Invoke();
    }

    public static void FailedTutorial()
    {
        OnFailedTutorial?.Invoke();
    }

    #endregion

    #region GuronEvents
    public static GuronEvents OnFailGuron;
    public static GuronEvents OnSideGuron;
    public static GuronEvents OnCenterGuron;
    public static GuronEvents OnEndGuron;

    public static void FailGuron()
    {
        OnFailGuron?.Invoke();
    }

    public static void SideGuron()
    {
        OnSideGuron?.Invoke();
    }

    public static void CenterGuron()
    {
        OnCenterGuron?.Invoke();
    }

    public static void EndGuron()
    {
        OnEndGuron?.Invoke();
    }

    #endregion

    #region UIEvents

    public static UIEvents OnUICommand;

    public static void UICommand(Constants._UICommand _uiCommand)
    {
        OnUICommand?.Invoke(_uiCommand);
    }

    #endregion

    #region PoolEvents
    public static TakePoolEvents OnTakePool;
    public static PlacePoolEvent OnPlacePool;


    public static void TakePool(Constants._PoolType _poolType, Constants._SelectionType _selectionType)
    {
        OnTakePool?.Invoke(_poolType, _selectionType);
    }

    public static void PlacePool(Constants._PoolType _poolType, GameObject _poolObject)
    {
        OnPlacePool?.Invoke(_poolType, _poolObject);
    }

    #endregion

    #region Notification Events

    //public static AdEvent OnFinishedAd;



    //public static void FinishedAd()
    //{
    //    OnFinishedAd?.Invoke();
    //}
    #endregion



    #region ACHIEVEMENTS

    public static AchievementEvents OnCheckGuronAchievement;
    public static AchievementEvents OnCheckPlanetAchievement;
    public static AchievementEvents OnCheckLosesAchievement;
    public static AchievementEvents OnCheckGameAchievement;

    public static void CheckGuronAchievement()
    {
        OnCheckGuronAchievement?.Invoke();
    }

    public static void CheckPlanetAchievement()
    {
        OnCheckPlanetAchievement?.Invoke();
    }

    public static void CheckLosesAchievement()
    {
        OnCheckLosesAchievement?.Invoke();
    }

    public static void CheckGameAchievement()
    {
        OnCheckGameAchievement?.Invoke();
    }

    #endregion

}
