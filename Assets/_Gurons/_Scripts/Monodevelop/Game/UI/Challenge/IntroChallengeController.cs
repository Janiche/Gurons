using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroChallengeController : MonoBehaviour
{
    [SerializeField] private IntroChallengeView _introChallengeView = null;

    public void _Init(string _message)
    {
        _introChallengeView._Init(_message);
    }

    public void _ActiveView(bool _active)
    {
        if (_active)
        {
            _introChallengeView._ActiveView();
        }
        else
        {
            _introChallengeView._ClosePanel();
        }
    }
}
