//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

using TMPro;
using System;

using UnityEngine.Events;

public class RefillSystem : MonoBehaviour
{
    [Serializable]
    private class NotificationEvent : UnityEvent<float>
    { }


    //#region REFILL DELEGATES

    //public delegate void RefillEvent(float _remainingTime);
    //public static RefillEvent OnStartRefill;
    //public static RefillEvent OnResetRefill;
    //public static RefillEvent OnEndRefill;

    //private void StartRefill(float _remainingTime)
    //{
    //    OnStartRefill?.Invoke(_remainingTime);
    //}
    //private void ResetRefill(float _remainingTime)
    //{
    //    OnResetRefill?.Invoke(_remainingTime);
    //}
    //private void EndRefill(float _remainingTime)
    //{
    //    OnEndRefill?.Invoke(_remainingTime);
    //}

    //#endregion

    #region VARIABLES
    [Header("Datos")]
    [Tooltip("Campo de cantidad de vidas actuales")] [SerializeField] private TextMeshProUGUI _lifeCount = null;
    [Tooltip("Campo de cantidad de diamantes actuales")] [SerializeField] private TextMeshProUGUI _diamondCount = null;
    [Tooltip("Campo de cantidad de diamantes actuales")] [SerializeField] private TextMeshProUGUI _coinCount = null;
    [Tooltip("Campo temporizador de proxima carga de vida")] [SerializeField] private TextMeshProUGUI _timer = null;
    [Tooltip("Campo de cantidad de vidas pasado temporizador")] [SerializeField] private TextMeshProUGUI _nextLife = null;

    [Header("Panel y Posiciones")]
    [Tooltip("Panel de Cronometro")] [SerializeField] private GameObject _refillCounter = null;

    #endregion

    #region DATOS INTERNOS

    [Tooltip("Cantidad de minutos restantes para la proxima vida")] [SerializeField] private float refillMinutes;
    [Tooltip("Cantidad máxima de vidas")] [SerializeField] private int maxLifes;
    [Tooltip("Cantidad actual de vidas")] [SerializeField] private int lifes;
    [Tooltip("Tiempo de la ultima vida obtenida")] [SerializeField] private DateTime lastLifeTime = DateTime.Now;
    [Tooltip("Tiempo para la próxima vida")] [SerializeField] private DateTime nextLifeTime = DateTime.Now;
    [Tooltip("Tiempo actual")] [SerializeField] private DateTime currentTime = DateTime.Now;

    [SerializeField] private float _remainingTime = 0;

    [Space]
    [SerializeField] private NotificationEvent OnStartTimer;
    [SerializeField] private NotificationEvent OnResetTimer;
    [SerializeField] private NotificationEvent OnEndTimer;

    #endregion

    void OnDisable()
    {
        Store.OnChangeValue -= OnChangeValue;
    }

    public void _Init()
    {
        #region INIT VALUES
        Store.OnChangeValue += OnChangeValue;

        _lifeCount.text = Store.instance.GetBalance(Constants._Currencies.one_life).ToString("00");
        _diamondCount.text = Store.instance.GetBalance(Constants._Currencies.diamond_currency).ToString("00");
        _coinCount.text = Store.instance.GetBalance(Constants._Currencies.coin_currency).ToString("00");

        //OBTIENE CANTIDAD DE VIDAS ACTUALES
        lifes = Store.instance.GetBalance(Constants._Currencies.one_life);

        #endregion

        _StartLifeCounDown();

    }

    //FUNCIONAMIENTO REFILL SYSTEM ESTÁ AQUÍ
    void Update()
    {
        _LifeCountDown();
    }

    private void _StartLifeCounDown()
    {
        if (lifes < maxLifes)
        {
            _remainingTime = (maxLifes - lifes) * refillMinutes;
            //Debug.LogError("START " + _remainingTime);
            OnStartTimer.Invoke(_remainingTime);

            if (PlayerPrefs.HasKey("_lastLife"))
            {
                //lee ultima entrega de preferencias
                //    //CALCULA EL TIEMPO TRANSCURRIDO
                long temp = Convert.ToInt64(PlayerPrefs.GetString("_lastLife"));
                lastLifeTime = DateTime.FromBinary(temp);
            }
            else
            {
                _refillCounter.SetActive(false);
                lastLifeTime = DateTime.Now;
                PlayerPrefs.SetString("_lastLife", DateTime.Now.ToBinary().ToString());
            }
            //asigna proxima entrega
            nextLifeTime = lastLifeTime.AddMinutes(refillMinutes);
        }

        //VIDAS AL MAXIMO
        else
        {

            _refillCounter.SetActive(false);

            if (PlayerPrefs.HasKey("_lastLife"))
            {
                PlayerPrefs.DeleteKey("_lastLife");
            }

            lastLifeTime = DateTime.Now;
        }

    }

    private void _LifeCountDown()
    {
        if (lifes < maxLifes)
        {
            TimeSpan remaining = nextLifeTime - DateTime.Now;

            //Tiempo menor al necesario para una nueva vida
            if (DateTime.Now < nextLifeTime)
            {

                var countdownTimer = string.Format("{0:D2}:{1:D2}", remaining.Minutes, remaining.Seconds);

                _refillCounter.SetActive(true);
                _nextLife.text = (lifes + 1).ToString();
                _timer.text = countdownTimer;
            }

            //Tiempo mayor al necesario para una nueva vida
            else
            {
                float amount = ((Mathf.Abs(remaining.Minutes) + refillMinutes) / refillMinutes);
                amount = Mathf.FloorToInt(amount);

                //Debug.LogError("Remaining: " + remaining.Minutes + " refill: " + refillMinutes + " amount: " + amount);

                //No quedan vidas a repartir
                if (amount + lifes >= maxLifes)
                {
                    if (_refillCounter.activeSelf)
                    {
                        _remainingTime = (maxLifes - lifes) * refillMinutes;
                        //Debug.LogError("END " + _remainingTime);
                        OnEndTimer.Invoke(_remainingTime);

                    }

                    _refillCounter.SetActive(false);
                    amount = maxLifes - lifes;
                    //Debug.LogError("B: " + amount);

                    if (PlayerPrefs.HasKey("_lastLife"))
                    {
                        PlayerPrefs.DeleteKey("_lastLife");
                    }

                }

                //Aun quedan vidas
                else
                {
                    amount = (amount > 0) ? amount : 1;

                    //Debug.LogError("A: " + amount);
                    nextLifeTime = nextLifeTime.AddMinutes(refillMinutes);
                    PlayerPrefs.SetString("_lastLife", DateTime.Now.ToBinary().ToString());

                    _remainingTime = (maxLifes - lifes) * refillMinutes;
                    //Debug.LogError("RESET " + _remainingTime);
                    OnResetTimer.Invoke(_remainingTime);

                }

                Store.instance.GiveProduct(Constants._Currencies.one_life, (int)amount);
            }

        }

        else
        {
            _refillCounter.SetActive(false);
            if (PlayerPrefs.HasKey("_lastLife"))
            {
                PlayerPrefs.DeleteKey("_lastLife");
            }
        }
    }

    //AL CAMBIAR VALOR ACTUALIZA GUI
    void OnChangeValue(Constants._Currencies _type, double _amount)
    {
        switch (_type)
        {
            case Constants._Currencies.one_life:
                lifes = Store.instance.GetBalance(_type);
                _lifeCount.text = lifes.ToString("00");
                //_lifeCount.text = Store.instance.GetBalance(_type).ToString("00");

                break;
            case Constants._Currencies.diamond_currency:
                _diamondCount.text = Store.instance.GetBalance(_type).ToString("00");

                break;

            case Constants._Currencies.coin_currency:
                _coinCount.text = Store.instance.GetBalance(_type).ToString("00");

                break;
        }
    }
}
