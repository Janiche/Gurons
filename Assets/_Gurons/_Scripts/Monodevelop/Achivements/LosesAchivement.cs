using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bermost/Achievement/Loses Achievement")]
public class LosesAchivement : AchievementCondition
{
    [SerializeField] private int _loses = 0;

    public override bool _EvalCondition()
    {
        return GlobalVars.saveData.gameData.totalLoses >= _loses;
    }
}
