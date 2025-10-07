using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Bermost;

//[CreateAssetMenu(menuName = "Bermost/Achivement")]
public abstract class AchievementCondition : ScriptableObject
{
    public int id;
    public Sprite _icon;
    public string _name = string.Empty;
    public Reward _reward;

    public abstract bool _EvalCondition();
}

