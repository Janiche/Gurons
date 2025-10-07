using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic class for only info panel, with no information on variables
/// </summary>
public class MessageController : MonoBehaviour
{
    [SerializeField] private MessageView _messageView = null;

    public void _ActiveView(bool _active)
    {
        if (_active)
        {
            _messageView._ActiveView();
        }
        else
        {
            _messageView._ClosePanel();
        }
    }

    public void _OnClose()
    {
        _ActiveView(false);
        GameEvents.UICommand(Constants._UICommand._noLifeContinue);
    }

    public void _OnStore()
    {
        GameEvents.UICommand(Constants._UICommand._toStore);
    }

    public void _OnResume()
    {
        _ActiveView(false);
        GameEvents.UICommand(Constants._UICommand._resume);
    }

    public void _OnRestart()
    {
        GameEvents.UICommand(Constants._UICommand._restart);
    }

    public void _OnMenu()
    {
        GameEvents.UICommand(Constants._UICommand._toMenu);
    }

    public void _OnKeepPlaying()
    {
        GameEvents.UICommand(Constants._UICommand._keepPlaying);
    }

    public void _OnNextPlanet()
    {
        GameEvents.UICommand(Constants._UICommand._nextPlanet);
    }
}
