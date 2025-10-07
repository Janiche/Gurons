using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerClock : MonoBehaviour
{
    public delegate void EmptyMethod();

    public enum TimeMode
    {
        DeltaTime, // es afectado por la pausa de time.timeScale = 0
        UnscaleTimeScale, // no es afectado
    }

    //#region Testing

    //public Command _command = Command.None;
    //public float _Testtime;

    //private void FixedUpdate()
    //{
    //    _Testtime = _time;
    //    _SetCommand(_command);
    //    _command = Command.None;
    //}

    //#endregion
    [Header("Timer Afected by TimeScale")]
    [Tooltip("Esta constante nos indica si el timer es afectado por las pausas del time.timeScale = 0")]
    public TimeMode _timeMode;
    [Header("Solo Para ver, No tocar")]
    [SerializeField] private State _state;
    [SerializeField] private float _time;
    [SerializeField] private float _cutTime;
    [SerializeField] private bool _isEventActive;
    EmptyMethod _EventMethod;

    public enum State
    {
        None,

        Running,
        Stoped,
        Paused,

        All,
    }

    public enum Command
    {
        None,

        Play,
        Stop,
        Pause,

        All,
    }

    public TimerClock()
    {
        _time = 0;
    }
    public void _SetCommand(Command command)
    {
        switch (command)
        {
            case Command.Play: _Play(); break;
            case Command.Stop: _Stop(); break;
            case Command.Pause: _Pause(); break;
            default: break;
        }
    }
    /// <summary>
    /// Metodo que genera un evento cuando el timer sobrepasa el valor
    /// </summary>
    /// <param name="eventMethod"></param>
    /// <param name="cutTime"></param>
    public void _SetEventTime(EmptyMethod eventMethod, float cutTime)
    {
        _EventMethod = eventMethod;
        _cutTime = cutTime;
        _isEventActive = true;
    }
    public float _GetTime()
    {
        return _time;
    }
    public State _GetState()
    {
        return _state;
    }
    public bool _IsRunnig()
    {
        return (_state == State.Running);
    }
    public void _DisableEventTime()
    {
        _isEventActive = false;
        _EventMethod = null;
        _cutTime = 0;
    }

    private void Update()
    {
        //Debug.Log("Time: " + Time.time);
        //Debug.Log("Delta Time: " + Time.deltaTime);
        //Debug.Log("UnscaledTime: " + (int)Time.unscaledTime);
        //Debug.Log("UnscaledDeltaTime: " + Time.unscaledDeltaTime);
        //Debug.Log("FixedUnscaledTime: " + Time.fixedUnscaledTime);
        //Debug.Log("FixedUnscaledTime: " + Time.fixedTime);

        if (_state == State.Running)
        {
            CalculateTime();

            if (_isEventActive)
            {
                if (_time > _cutTime)
                {
                    _isEventActive = false;
                    _EventMethod();
                    if (!_isEventActive)
                    {
                        _DisableEventTime();
                    }
                }
            }
        }
        //Debug.Log("time: " + _time);
    }
    private void CalculateTime()
    {
        if (_timeMode == TimeMode.UnscaleTimeScale)
        {
            _time += Time.unscaledTime * 0.001f;
        }
        else
        {
            _time += Time.deltaTime;
        }
    }
    private void _Play()
    {
        if (_state != State.Running)
        {
            _state = State.Running;
        }
    }
    private void _Pause()
    {
        if (_state == State.Running)
        {
            _state = State.Paused;
        }
    }
    private void _Stop()
    {
        if (_state != State.Stoped)
        {
            _time = 0;
            _state = State.Stoped;
        }
    }



}
