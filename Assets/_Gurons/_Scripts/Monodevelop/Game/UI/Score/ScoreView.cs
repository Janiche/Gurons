using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private TextMeshProUGUI _scoreLabel;
    [SerializeField] private TextMeshProUGUI _scoreValue;
    [SerializeField] private GameObject _doubleScore;

    private int _currentScore = 0;
    public int _GetScore
    {
        get { return _currentScore; }
    }

    public void _ActiveView(bool _active)
    {
        _view.SetActive(_active);
    }

    public void _Init(bool _powerUp)
    {
        _ActiveView(true);
        _doubleScore.SetActive(_powerUp);

    }

    /// <summary>
    /// Updates the score according replacing onf value by new one.
    /// </summary>
    /// <param name="_score">New Score value.</param>
    public void _UpdateScore(int _score)
    {
        _scoreValue.text = _score.ToString("00");
    }

}
