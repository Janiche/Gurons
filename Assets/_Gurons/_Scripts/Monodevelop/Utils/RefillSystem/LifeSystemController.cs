using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;
using PixelCrushers;

public class LifeSystemController : MonoBehaviour
{
    #region DELEGATES
    public delegate void LifeSystemEvents(object _data);

    public static LifeSystemEvents OnStartRefill;
    public static LifeSystemEvents OnProgressRefill;
    public static LifeSystemEvents OnResetRefill;
    public static LifeSystemEvents OnEndRefill;
    public static LifeSystemEvents OnUpdateRefill;

    private void StartRefill(object _data)
    {
        OnStartRefill?.Invoke(_data);
    }
    private void ProgressLife(object _data)
    {
        OnProgressRefill?.Invoke(_data);
    }
    private void ResetRefill(object _data)
    {
        OnResetRefill?.Invoke(_data);
    }
    private void EndRefill(object _data)
    {
        OnEndRefill?.Invoke(_data);
    }
    private void UpdateLife(object _data)
    {
        OnUpdateRefill?.Invoke(_data);
    }
    #endregion

    private static LifeSystemController _instance;
    public static LifeSystemController Instance
    {
        get { return _instance; }
    }



    [SerializeField] private int lifes = 0;
    [SerializeField] private int _maxEnergy = 5;
    [SerializeField] private int _restoreDuration = 0;
    [SerializeField] private DateTime _nextEnergyTime;
    [SerializeField] private DateTime _lastAddedTime;
    [SerializeField] private bool _restoring = false;
    [SerializeField] private bool _paused = false;

    [SerializeField] private bool _initialized = false;

    [SerializeField] private TextTable _textTable;
    [SerializeField] private Notification lifeNotification;

    Coroutine _restore;

    private void OnEnable()
    {
        if (_instance == null)
        {
            _instance = this;

            //Store.OnChangeValue += OnChangeValue;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }



    [Button("Init")]
    public void _Init()
    {
        if (!_initialized)
        {
            //Store.OnChangeValue += OnChangeValue;
            _initialized = true;
        }

        _Load();

        _restore = StartCoroutine(_RestoreRoutine());

        UpdateLife(lifes);
    }

    private void OnDisable()
    {
        //    Store.OnChangeValue -= OnChangeValue;
        _initialized = false;
    }


    [Button("Use")]
    public void _Use()
    {
        lifes = Store.instance.GetBalance(Constants._Currencies.one_life);

        if (lifes == 0)
            return;

        Store.instance.TakeProduct(Constants._Currencies.one_life, 1);

        lifes--;
        _UpdateTimer();

        if (!_restoring)
        {

            if (lifes + 1 == _maxEnergy)
            {
                _nextEnergyTime = _AddDuration(DateTime.Now, _restoreDuration);
            }

            _restore = StartCoroutine(_RestoreRoutine());
        }
    }

    [Button("Add")]
    private void _Add()
    {
        Store.instance.GiveProduct(Constants._Currencies.one_life, 1);
        lifes = Store.instance.GetBalance(Constants._Currencies.one_life);

        //UpdateLife(lifes);

    }

    //[Button("Pause")]
    public void _Pause(bool p)
    {
        _paused = p;


        //if (lifes < _maxEnergy)
        //{
        //DETIENE TEMPORIZADOR
        if (_paused)
        {

            if (_restore != null)
            {
                StopCoroutine(_restore);
            }

            _lastAddedTime = DateTime.Now;
            _nextEnergyTime = DateTime.Now;
            _Save();

        }
        //ACTIVA TEMPORIZADOR
        else
        {
            _nextEnergyTime = _AddDuration(DateTime.Now, _restoreDuration);
            _Save();
            _restore = StartCoroutine(_RestoreRoutine());

        }
        //}

    }

    private void _Save()
    {
        //Debug.LogError("S + " + _nextEnergyTime.ToString());

        PlayerPrefs.SetString("nextEnergyTime", _nextEnergyTime.ToString());
        PlayerPrefs.SetString("lastAddedTime", _lastAddedTime.ToString());
    }

    private void _Load()
    {
        //lifes = PlayerPrefs.GetInt("totalEnergy");

        lifes = Store.instance.GetBalance(Constants._Currencies.one_life);
        //Debug.LogError("L: " + lifes);

        _nextEnergyTime = _StringToDate(PlayerPrefs.GetString("nextEnergyTime"));
        _lastAddedTime = _StringToDate(PlayerPrefs.GetString("lastAddedTime"));
    }

    private IEnumerator _RestoreRoutine()
    {
        _UpdateTimer();
        _UpdateLife();
        _restoring = true;

        //Debug.LogError($"restoring {_restoring}  start {lifes < _maxEnergy}");

        StartRefill(lifes < _maxEnergy);

        while (lifes < _maxEnergy)
        {
            DateTime _currentTime = DateTime.Now;
            DateTime _counter = _nextEnergyTime;
            bool _isAdding = false;

            while (_currentTime > _counter)
            {
                if (lifes < _maxEnergy)
                {
                    _isAdding = true;
                    lifes++;
                    Store.instance.GiveProduct(Constants._Currencies.one_life, 1);
                    DateTime _timeToAdd = (_lastAddedTime > _counter) ? _lastAddedTime : _counter;
                    _counter = _AddDuration(_timeToAdd, _restoreDuration);
                    //UpdateLife(lifes);

                }

                else
                    break;
            }

            if (_isAdding)
            {
                _lastAddedTime = DateTime.Now;
                _nextEnergyTime = _counter;

                ResetRefill(lifes);
            }

            _UpdateTimer();
            _UpdateLife();
            _Save();
            yield return null;
        }
        _restoring = false;
    }

    private void _UpdateTimer()
    {

        if (lifes >= _maxEnergy)
        {
            EndRefill("FULL");
            return;
        }

        TimeSpan t = _nextEnergyTime - DateTime.Now;
        //string _value = $"{t.TotalMinutes:n0}:{t.TotalSeconds:n0}";
        //var _value = string.Format("{0}:{1}", t.TotalMinutes, t.TotalSeconds);

        ProgressLife(t);
    }

    private void _UpdateLife()
    {
        //UpdateLife(lifes);
    }

    private DateTime _AddDuration(DateTime time, int duration)
    {
        //return time.AddSeconds(duration);
        return time.AddMinutes(duration);

    }

    private DateTime _StringToDate(string date)
    {
        if (string.IsNullOrEmpty(date))
            return DateTime.Now;

        return DateTime.Parse(date);
    }



    void OnApplicationQuit()
    {
        lifes = Store.instance.GetBalance(Constants._Currencies.one_life);

        if (lifes < _maxEnergy)
        {

            if (_paused)
            {
                _nextEnergyTime = _AddDuration(DateTime.Now, _restoreDuration);
                _Save();
            }

            int _remainingLife = _maxEnergy - lifes;
            int _time = _remainingLife * _restoreDuration;

            lifeNotification = new Notification()
            {
                _title = _textTable.GetFieldText("Notification.Refill.title"),
                _content = _textTable.GetFieldText("Notification.Refill.content"),
                _fireTime = _AddDuration(DateTime.Now, _time),
                _repeatInterval = new TimeSpan(0, 2, 0)
            };

            NotificationManager._instance._SendNotification(lifeNotification);
        }

        else
        {
            if (NotificationManager._instance != null)
            {
                if (lifeNotification != null)
                    NotificationManager._instance._CancelNotification(lifeNotification);
            }
        }

    }

    void OnApplicationPause(bool pause)
    {

#if !UNITY_EDITOR

        lifes = Store.instance.GetBalance(Constants._Currencies.one_life);

        if (pause)
        {
            if (lifes < _maxEnergy)
            {

                if (_paused)
                {
                    _nextEnergyTime = _AddDuration(DateTime.Now, _restoreDuration);
                    _Save();
                }

                int _remainingLife = _maxEnergy - lifes;
                int _time = _remainingLife * _restoreDuration;

                lifeNotification = new Notification()
                {
                    _title = _textTable.GetFieldText("Notification.Refill.title"),
                    _content = _textTable.GetFieldText("Notification.Refill.content"),
                    _fireTime = _AddDuration(DateTime.Now, _time),
                    _repeatInterval = new TimeSpan(0, 2, 0)
                };

                NotificationManager._instance._SendNotification(lifeNotification);
            }
        }
        else
        {
            if (NotificationManager._instance != null)
            {
                if (lifeNotification != null)
                    NotificationManager._instance._CancelNotification(lifeNotification);
            }
        }
#endif
    }
}
