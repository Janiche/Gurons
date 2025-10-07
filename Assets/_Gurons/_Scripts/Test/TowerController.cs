using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{

    private enum TowerStates
    {
        stopped,
        moving
    }


    [SerializeField] private float _currentSpeed = 0;
    //[SerializeField] private int _currentGurons = 0;

    [Space]
    [SerializeField] private float _transitionSpeed = 0.1f;
    [SerializeField] private TowerStates _mainState = TowerStates.stopped;


    [Space]
    [Header("Limites")]
    [SerializeField] private float _maxSpeed = 2;

    //[SerializeField] private float _maxGurons = 3;

    [SerializeField] private float _nextSpeed = 0;
    [SerializeField] private bool _side = true;

    Coroutine _speedRoutine;

    /*
        INICIALMENTE LA TORRE PARTE DETENIDA CUANDO EL MOVIMIENTO COMIENZA, SERÁ LATERAL,
        A MEDIDA AUMENTA LA CANTIDAD DE GURONS, LA VELOCIDAD  Y AMPLITUD IRÁ EN AUMENTO.
        CUANDO LA TORRE ALCANCE SU MAYOR AMPLITUD LATERAL, COMENZARÁ A MEZCLARSE ROTACIÓN
        Y MOVIMIENTO HASTA ALCANZAR VELOCIDAD MÁXIMA, DONDE CAMBIARÁ SOLO A ROTACIÓN
    */

    private void Start()
    {
        float _r = Random.Range(-1.1f, 1.1f) + 0.1f;

        _side = (_r > 0);

        StartCoroutine(_MoveTower());
    }

    public void _ChangeDir(bool dir)
    {
        _side = dir;
    }

    public void _NextState()
    {
        //GlobalVars.blox++;

        //if (GlobalVars.blox % 3 == 0)
        //{

        //}
        //_currentGurons++;

        switch (_mainState)
        {
            case TowerStates.stopped:

                //if (GlobalVars.blox > 4)
                //if (_currentGurons > 4)
                //{
                if (_currentSpeed < _maxSpeed / 2)
                {
                    //_nextSpeed += _transitionSpeed;
                    _currentSpeed += _transitionSpeed;

                }
                else
                {
                    _mainState = TowerStates.moving;
                }
                //}

                break;

            case TowerStates.moving:


                //_nextSpeed += _transitionSpeed;
                _currentSpeed += _transitionSpeed;

                break;
        }

        //_nextSpeed = Mathf.Clamp(_nextSpeed, 0, _maxSpeed);

        //_speedRoutine = StartCoroutine(_ChangeSpeed(_nextSpeed));

        _currentSpeed = Mathf.Clamp(_currentSpeed, 0, _maxSpeed);
    }

    IEnumerator _ChangeSpeed(float _end)
    {
        if (_currentSpeed > _end)
        {
            while (_currentSpeed > _end)
            {
                //_currentSpeed = Mathf.Lerp(_end, _currentSpeed, _transitionSpeed);

                _currentSpeed -= 0.01f;
                yield return new WaitForSecondsRealtime(0.01f);
            }
        }
        else
        {
            while (_currentSpeed < _end)
            {
                //_currentSpeed = Mathf.Lerp(_currentSpeed, _end, _transitionSpeed);

                _currentSpeed += 0.01f;
                yield return new WaitForSecondsRealtime(0.01f);
            }
        }
    }

    IEnumerator _MoveTower()
    {
        Vector3 _pos = transform.localPosition;
        yield return new WaitForSecondsRealtime(0.1f);
        while (true)
        {
            int _sign = (_side) ? -1 : 1;

            if (Time.timeScale > 0)
            {
                transform.Translate(Vector3.right * _sign * _currentSpeed);
            }

            //transform.localPosition = new Vector3(transform.localPosition.x + _sign * _currentSpeed * _movementSpeed * Time.fixedDeltaTime, _pos.y, _pos.z);

            if (_mainState != TowerStates.stopped)
            {

            }
            yield return new WaitForSecondsRealtime(0.01f);

        }
    }
}
