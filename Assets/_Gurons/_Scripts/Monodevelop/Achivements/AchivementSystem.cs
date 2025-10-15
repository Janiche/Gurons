using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

public class AchivementSystem : MonoBehaviour
{

    #region SINGLETON
    private static AchivementSystem instance;
    public static AchivementSystem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<AchivementSystem>();

                if (instance == null)
                {
                    Debug.LogError("SIN INSTANCIA Achivement Controller");
                }
            }
            return instance;
        }
    }
    #endregion


    #region VARIABLE

    public bool _menuCheck = false;

    [SerializeField] private GameObject _canvas;
    [SerializeField] private string popupName = "AchievementPopup";
    //[SerializeField] private TextTable _textTable;
    //[SerializeField] private UIPopup _popup; //DOOZY

    [Space]
    [SerializeField] private List<AchievementCondition> _achivementsConditions = new List<AchievementCondition>();
    private AchievementCondition _ach;


    private List<GuronAchivement> _gurons = new List<GuronAchivement>();
    private List<PlanetAchivement> _planets = new List<PlanetAchivement>();
    private List<LosesAchivement> _loses = new List<LosesAchivement>();
    private List<GameAchivement> _game = new List<GameAchivement>();


    private int _totalCount = 0;
    private int _guronCount = 0;
    private int _planetCount = 0;
    private int _losesCount = 0;
    private int _gameCount = 0;

    #endregion



    #region PROPERTIES

    public int GetAchivementCount { get { return _achivementsConditions.Count; } }

    public List<Achivement> GetAchivements
    {
        get
        {

            List<Achivement> _tempList = new List<Achivement>();

            int c = _achivementsConditions.Count;

            for (int i = 0; i < c; i++)
            {
                if (GlobalVars.saveData.achivementData.achivements.Exists(a => a.id == _achivementsConditions[i].id))
                {
                    Achivement _tempAchivement = GlobalVars.saveData.achivementData.achivements.Find(a => a.id == _achivementsConditions[i].id);
                    _tempList.Add(_tempAchivement);
                }

                else
                {
                    Achivement _tempAchivement = new Achivement
                        (
                            _achivementsConditions[i].id,
                            _achivementsConditions[i]._name,
                            false, false
                        );

                    _tempList.Add(_tempAchivement);
                }
            }


            _tempList.Sort((x, y) => x.id.CompareTo(y.id));

            return _tempList;
        }
    }

    public int GetUnclaimedAchievement
    {
        get
        {
            int tot = GlobalVars.saveData.achivementData.achivements.Count;
            int unclaimed = 0;

            if (tot > 0)
            {
                for (int i = 0; i < tot; i++)
                {
                    if (!GlobalVars.saveData.achivementData.achivements[i].claimed)
                    {
                        unclaimed++;
                    }
                }
            }

            return unclaimed;
        }
    }


    #endregion



    private void OnEnable()
    {
        _Init();
    }

    private void _Init()
    {
        DontDestroyOnLoad(_canvas);

        GameEvents.OnCheckGuronAchievement -= OnCheckGuronAchievement;
        GameEvents.OnCheckPlanetAchievement -= OnCheckPlanetAchievement;
        GameEvents.OnCheckLosesAchievement -= OnCheckLosesAchievement;
        GameEvents.OnCheckGameAchievement -= OnCheckGameAchievement;

        GameEvents.OnCheckGuronAchievement += OnCheckGuronAchievement;
        GameEvents.OnCheckPlanetAchievement += OnCheckPlanetAchievement;
        GameEvents.OnCheckLosesAchievement += OnCheckLosesAchievement;
        GameEvents.OnCheckGameAchievement += OnCheckGameAchievement;


        _totalCount = _achivementsConditions.Count;

        for (int i = 0; i < _totalCount; i++)
        {
            if (_achivementsConditions[i].GetType() == typeof(GuronAchivement))
            {
                _gurons.Add(_achivementsConditions[i] as GuronAchivement);
            }

            else if (_achivementsConditions[i].GetType() == typeof(PlanetAchivement))
            {
                _planets.Add(_achivementsConditions[i] as PlanetAchivement);
            }

            else if (_achivementsConditions[i].GetType() == typeof(LosesAchivement))
            {
                _loses.Add(_achivementsConditions[i] as LosesAchivement);
            }

            else if (_achivementsConditions[i].GetType() == typeof(GameAchivement))
            {
                _game.Add(_achivementsConditions[i] as GameAchivement);
            }
        }



        _guronCount = _gurons.Count;
        _planetCount = _planets.Count;
        _losesCount = _loses.Count;
        _gameCount = _game.Count;
    }

    private void _MostrarPanel()
    {

        string description = string.Empty;

        string descKey = $"Achievements.{_ach._name}.Name";

        /*if (_textTable != null)
        {
            description = _textTable.GetFieldText(descKey);
        }

        else
        {
            description = descKey;
        }*/

        //DOOZY
        /*_popup.Data.SetImagesSprites(_ach._icon);
        _popup.Data.SetLabelsTexts(description);*/
    }

    public void _UnlockAchivement(int id)
    {
        _ach = _achivementsConditions.Find(a => a.id == id);

        if (_ach != null)
        {
            Achivement _achivement = new Achivement(id, _ach.name, true);

            GlobalVars.saveData.achivementData.achivements.Add(_achivement);

            SaveSystem.SaveData();
            _MostrarPanel();
        }
    }


    public bool GetAchievementStatus(int id)
    {
        if (GlobalVars.saveData.achivementData.achivements.Exists(a => a.id == id))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public AchievementCondition GetCondition(int id)
    {
        return _achivementsConditions.Find(a => a.id == id);
    }






    private void OnCheckGuronAchievement()
    {
        for (int i = 0; i < _guronCount; i++)
        {


            if (!GetAchievementStatus(_gurons[i].id))
            {
                if (_gurons[i]._EvalCondition())
                {
                    _UnlockAchivement(_gurons[i].id);
                }
            }
        }
    }

    private void OnCheckPlanetAchievement()
    {
        for (int i = 0; i < _planetCount; i++)
        {
            if (!GetAchievementStatus(_planets[i].id))
            {
                if (_planets[i]._EvalCondition())
                {
                    _UnlockAchivement(_planets[i].id);
                }
            }
        }
    }

    private void OnCheckLosesAchievement()
    {
        for (int i = 0; i < _losesCount; i++)
        {
            if (!GetAchievementStatus(_loses[i].id))
            {
                if (_loses[i]._EvalCondition())
                {
                    _UnlockAchivement(_loses[i].id);
                }
            }
        }
    }

    private void OnCheckGameAchievement()
    {
        for (int i = 0; i < _gameCount; i++)
        {
            if (!GetAchievementStatus(_game[i].id))
            {
                if (_game[i]._EvalCondition())
                {
                    _UnlockAchivement(_game[i].id);
                }
            }
        }
    }

}
