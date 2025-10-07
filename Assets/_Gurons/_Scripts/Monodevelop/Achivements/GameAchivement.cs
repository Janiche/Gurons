using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bermost/Achievement/Game Achievement")]
public class GameAchivement : AchievementCondition
{
    [SerializeField] private Constants._GameCondition _condition = Constants._GameCondition.firstTime;

    public override bool _EvalCondition()
    {
        bool result = false;
        switch (_condition)
        {
            case Constants._GameCondition.firstTime:
                {
                    if (GlobalVars.saveData.playerData.firstLogin != null)
                        result = true;
                }

                break;

            case Constants._GameCondition.allPlanets:
                {

                    int plan = GlobalVars.maxPlanets;
                    int count = 0;

                    for (int i = 0; i < plan; i++)
                    {
                        if (GlobalVars.saveData.gameData.planetData[i].unlocked)
                        {
                            count++;
                        }
                    }

                    if (count == plan)
                        result = true;
                }

                break;
        }

        return result;
    }
}
