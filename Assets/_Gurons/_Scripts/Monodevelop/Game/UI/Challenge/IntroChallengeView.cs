using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using Doozy.Engine.UI;

public class IntroChallengeView : MonoBehaviour
{
    [SerializeField] private UIView _view;

    [SerializeField] private TextMeshProUGUI _message;


    public void _Init(string _message)
    {
        this._message.text = _message;

        _ActiveView();
    }

    public void _ActiveView()
    {
        _view.Show();
    }

    public void _ClosePanel()
    {
        _view.Hide();
    }
}
