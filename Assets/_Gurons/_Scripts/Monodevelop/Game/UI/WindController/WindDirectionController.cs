using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class WindDirectionController : MonoBehaviour
{

    [SerializeField] private Image _leftWind;
    [SerializeField] private Image _rightWind;

    public void _Init(float _windIntensity)
    {

        _windIntensity = Mathf.Clamp(_windIntensity, -1, 1);

        if (_windIntensity == 0)
        {
            _leftWind.fillAmount = 0;
            _rightWind.fillAmount = 0;
        }
        else
        {

            if (_windIntensity > 0)
            {
                _rightWind.fillAmount = _windIntensity;
                _leftWind.fillAmount = 0;
            }

            else
            {
                _leftWind.fillAmount = -_windIntensity;
                _rightWind.fillAmount = 0;
            }
        }

    }

    public void _ChangeWind(float _windIntensity)
    {
        _windIntensity = Mathf.Clamp(_windIntensity, -1, 1);


        if (_windIntensity == 0)
        {
            _leftWind.fillAmount = 0;
            _rightWind.fillAmount = 0;
        }
        else
        {
            if (_windIntensity > 0)
            {
                _rightWind.fillAmount = _windIntensity;
                _leftWind.fillAmount = 0;
            }

            else
            {
                _leftWind.fillAmount = -_windIntensity;
                _rightWind.fillAmount = 0;
            }
        }
    }
}
