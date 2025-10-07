using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private ScoreView _scoreView;


    public void _Init()
    {
        _scoreView._Init(GlobalVars._scoreAdded);
    }

    public void _UpdateScore(int _score)
    {
        if (_scoreView._GetScore < _score)
        {
            _scoreView._UpdateScore(_score);
        }
    }

    public void _ActiveView(bool _active)
    {
        _scoreView._ActiveView(_active);
    }

}
