using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private GameOverView _gameOverView;
    private bool _setted = false;


    public void _Init(int _continueCost)
    {
        _gameOverView._Init(_continueCost);
    }

    /// <summary>
    /// Sets the game over score and continue button.
    /// </summary>
    /// <param name="_score">Score.</param>
    /// <param name="_continue">0[Diamond] 1[Chance] 2[Ads].</param>
    public void _OnGameOver(int _score, int _best, Constants._GameContinue _continue)
    {
        _gameOverView._ActiveButton(_continue);
        _gameOverView._SetScore(_score, _best, Store.instance.GetBalance(Constants._Currencies.one_life), Store.instance.GetBalance(Constants._Currencies.diamond_currency), Store.instance.GetBalance(Constants._Currencies.coin_currency));
    }

    public void _UpdateStatus()
    {
        _gameOverView._UpdateStatus(Store.instance.GetBalance(Constants._Currencies.one_life), Store.instance.GetBalance(Constants._Currencies.diamond_currency), Store.instance.GetBalance(Constants._Currencies.coin_currency));
        //Store.instance.GetBalance(Constants._Currencies.one_life), Store.instance.GetBalance(Constants._Currencies.diamond_currency), Store.instance.GetBalance(Constants._Currencies.coin_currency)
    }

    public void _ActiveView(bool _active)
    {
        if (_active)
        {
            _gameOverView._ActiveView();
            //if (_setted)
            //{
            //}
        }
        else
        {
            _gameOverView._ClosePanel();
        }
    }

    public void _OnPressMenu()
    {
        GameEvents.UICommand(Constants._UICommand._toMenu);
    }

    public void _OnRetry()
    {
        GameEvents.UICommand(Constants._UICommand._retry);
    }

    public void _OnAdContinue()
    {
        GameEvents.UICommand(Constants._UICommand._adContinue);
    }

    public void _OnDiamondContinue()
    {
        GameEvents.UICommand(Constants._UICommand._diamondContinue);
    }
}
