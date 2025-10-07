//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using CodeStage.AntiCheat.ObscuredTypes;

public class TimerController : MonoBehaviour
{
    #region NEW CONTROLLER

    [SerializeField] private TimerView _timerView;

    #endregion

    //TODO AA: ADD VIEW AND REMOVE REFERENCE TO GAMEMANAGER
    //[Tooltip("Componente Slider a mostrar progreso del temporizador")] [SerializeField] private Slider timerSlider = null;
    //[Tooltip("Imagen a modificar por tiempo")] [SerializeField] private Image fill = null;

    [Header("Colores de tiempo")]
    [Tooltip("Color al estar full tiempo")] [SerializeField] private Color full;
    [Tooltip("Color al estar 50% tiempo")] [SerializeField] private Color danger;
    [Tooltip("Color al estar en tiempo crítico")] [SerializeField] private Color critical;

    [Header("Timer Limits")]
    private float _dangerLimit = 0;
    private float _criticalLimit = 0;

    private ObscuredBool _countDown = false;

    [Header("Timer Times")]
    private ObscuredFloat _totalTime = 0;
    private ObscuredFloat _elapsedTime = 0;
    private ObscuredFloat _remainingTime = 0;


    //TODO AA: REMOVE INSTANCE
    //private static TimerController _instance;
    //public static TimerController Instance
    //{
    //    get { return _instance; }
    //}

    public void _Init(float _time)
    {
        _timerView._Init(_time);


    }

    public void _ActiveView(bool _active)
    {
        _timerView._ActiveView(_active);
    }

    #region NEW FUNCTION

    #endregion

    ///// <summary>
    ///// Inicializa temporizador
    ///// </summary>
    ///// <param name="time">Time.</param>
    //public void SetTimer(float time)
    //{
    //    //_instance = this;
    //    timerSlider.maxValue = time;
    //    timerSlider.value = time;
    //    remainTime = time;
    //    elapsedTime = 0;

    //    CalculateLimit();
    //}



    /// <summary>
    /// Inicia la cuenta regresiva
    /// </summary>
    public void StartTimer()
    {
        _countDown = true;
    }

    //Pausa la cuenta regresiva
    public void PauseTimer()
    {
        _countDown = false;
    }

    //Detiene la cuenta regresiva 
    public void StopTimer()
    {
        _countDown = false;
    }

    /// <summary>
    /// Al recibir cualquier modificación de tiempo
    /// </summary>
    /// <param name="time">Tiempo a agregar o quitar.</param>
    public void EditTimer(float time)
    {
        _timerView._SetValue(time);

        //timerSlider.maxValue += time;
        //timerSlider.value += time;
        _totalTime += time;

        CalculateLimit();
    }

    /// <summary>
    /// Calcula limites de colores
    /// </summary>
    private void CalculateLimit()
    {

        float _dangerLimit = _totalTime * 0.5f;
        float _criticLimit = _totalTime * 0.2f;

        //_timerView._SetLimits(_dangerLimit, _criticLimit);
        _timerView._SetValue(_totalTime);
    }

    private void Update()
    {
        if (_countDown)
        {
            if (_elapsedTime <= _totalTime)
            {
                _elapsedTime += Time.deltaTime;
                _remainingTime = _totalTime - _elapsedTime;

                _timerView._SetValue(_remainingTime);
                //timerSlider.value = (remainTime - elapsedTime);
            }
            else
            {
                //GameManager.Instance.gameMode.FailChallenge();

                //TODO AA: MAYBE REPLACE FOR GAMEEVENTS??
                //_EndTimer?.Invoke();
            }

            //Cambio de color

            //Cambia color a full
            if (_remainingTime > _dangerLimit)
            {
                _timerView._SetColor(full);
                //fill.color = full;
            }
            //Cambia color a Danger
            else if (_remainingTime <= _dangerLimit && _remainingTime > _criticalLimit)
            {
                _timerView._SetColor(danger);
                //fill.color = danger;
            }
            //Cambia color a Critic
            else if (_remainingTime <= _criticalLimit)
            {
                _timerView._SetColor(critical);
                //fill.color = critic;
            }
        }
    }

}
