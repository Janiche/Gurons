using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGuron : MonoBehaviour
{
    public enum Action
    {
        _none,
        _init,
        _launch,
        _reset
    }

    public bool testing;
    public Action _action = Action._none;
    public GuronController _controller;
    public Constants._GuronState _guronState = Constants._GuronState.none;


    //private void OnEnable()
    //{
    //    _Init();
    //}

    void _Init()
    {
        _controller._Init();
    }

    private void Update()
    {
        if (testing && _controller != null)
        {
            switch (_action)
            {
                case Action._init:
                    if (_guronState != Constants._GuronState.none)
                    {
                        _controller._Init(_guronState);
                    }
                    else
                    {
                        _controller._Init();
                    }
                    break;

                case Action._launch:
                    _controller._LaunchGuron();
                    break;

                case Action._reset:
                    _controller._Reset();
                    break;

            }
            _action = Action._none;

            //switch (_guronState)
            //{
            //    case Constants._GuronState.balance:
            //        break;

            //    case Constants._GuronState.fall:
            //        _controller._LaunchGuron();

            //        break;

            //    case Constants._GuronState.hit:
            //        break;
            //}
            //_guronState = Constants._GuronState.none;
        }
    }
}