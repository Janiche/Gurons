using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChancesView : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private GameObject[] _chances;
    [SerializeField] private GameObject _extraChanceGo;
    //[SerializeField] private ParticleSystem[] _chancesParticle;

    [SerializeField] private int _totalChance = 0;
    [SerializeField] private int _currentChance = 0;
    [SerializeField] private int _extraChance = 0;

    public void _Init(bool _powerUp, int _maxChances = 3, int _extra = 0)
    {

        _extraChanceGo.SetActive(_powerUp);

        _totalChance = _chances.Length;
        _currentChance = _maxChances;
        _extraChance = _extra;

        for (int i = 0; i < _totalChance; i++)
        {
            if (i < _maxChances)
            {
                _chances[i].SetActive(true);
            }
            else
            {
                _chances[i].SetActive(false);
            }
        }
    }

    public void _AddChance()
    {
        if (_extraChance <= 0)
        {
            _currentChance += 1;
            //TODO AA: CHANGE ACTIVATION FOR SIMPLE SIZE ANIMATION
            if (_currentChance < _totalChance)
            {
                _chances[_currentChance - 1].SetActive(true);
            }
        }
    }

    public void _RemoveChance()
    {
        if (_extraChance > 0)
        {
            _extraChance--;
            _extraChanceGo.SetActive(false);
        }
        else
        {
            //TODO AA: CHANGE DEACTIVATION FOR SIMPLE SIZE ANIMATION
            _currentChance -= 1;

            if (_currentChance >= 0)
            {
                //_chancesParticle[_currentChance].Play();
                _chances[_currentChance].SetActive(false);
            }
        }

    }

}
