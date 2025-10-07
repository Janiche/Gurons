using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bermost/Achievement/Guron Achievement")]
public class GuronAchivement : AchievementCondition
{
    [SerializeField] private int _gurons = 0;
    [SerializeField] private Constants._GuronsCondition _condition = Constants._GuronsCondition.none;

    public override bool _EvalCondition()
    {
        bool result = false;
        switch (_condition)
        {
            case Constants._GuronsCondition.place:
                result = (GlobalVars.saveData.gameData.totalBlox >= _gurons);
                break;

            case Constants._GuronsCondition.center:
                result = (GlobalVars.saveData.gameData.totalCenter >= _gurons);
                break;

            case Constants._GuronsCondition.side:
                result = (GlobalVars.saveData.gameData.totalSide >= _gurons);
                break;

            case Constants._GuronsCondition.fail:
                result = (GlobalVars.saveData.gameData.totalFails >= _gurons);
                break;
        }

        return result;
    }
}
