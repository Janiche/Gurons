using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChancesController : MonoBehaviour
{
    [SerializeField] private ChancesView _chancesView;


    /// <summary>
    /// Initialize chances panel, actives needed chances
    /// according parameter or gameMode.
    /// </summary>
    /// <param name="_maxChances">Max chances.</param>
    public void _Init(int _maxChances = 3)
    {
        _chancesView._Init(GlobalVars._chanceAdded, _maxChances, GlobalVars.extraChance);
    }

    public void _AddChance()
    {
        _chancesView._AddChance();
    }

    public void _RemoveChance()
    {
        _chancesView._RemoveChance();
    }

}
