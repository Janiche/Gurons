using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TutorialStateMachine
{
    public enum TutorialState
    {
        none,
        welcome,
        waitingFirstPlace,
        firstFailed,
        firstSuccess,
        missionStart,
        missionDone,
        missionFailed,
    }

    private static TutorialState _state = TutorialState.none;

    public static TutorialState _GetState
    {
        get { return _state; }
    }

    public static void _ChangeState(TutorialState _nextState)
    {
        _state = _nextState;
        Debug.Log(_state);
    }
}
