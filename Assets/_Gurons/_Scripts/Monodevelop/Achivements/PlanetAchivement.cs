using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bermost/Achievement/Planet Achievement")]
public class PlanetAchivement : AchievementCondition
{
    [SerializeField] private Constants._Planets _planet;

    public override bool _EvalCondition()
    {
        return GlobalVars.saveData.gameData.planetData[(int)_planet].unlocked; ;
    }
}
