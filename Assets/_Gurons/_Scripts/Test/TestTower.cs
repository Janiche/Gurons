using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTower : MonoBehaviour
{
    [SerializeField] private TowerController _controller;

    [SerializeField] private float _time = 0.1f;
    [SerializeField] private bool _autoPlace = true;


    [Range(0, 100)] [SerializeField] private float _centerPercent = 30;
    public void _CenterGuron()
    {
        _controller._NextState();
    }

    public void _SideGuron()
    {
        _controller._NextState();
    }

    private void Start()
    {
        StartCoroutine(_AutoPlace());
    }

    IEnumerator _AutoPlace()
    {
        int _r = 0;


        while (_autoPlace)
        {
            _r = Random.Range(0, 100);

            //CENTRO
            if (_r < _centerPercent)
            {
                _controller._NextState();
            }

            //LATERAL
            else
            {
                _controller._NextState();
            }

            ////NADA
            //else
            //{

            //}

            yield return new WaitForSecondsRealtime(_time);
        }
    }
}
