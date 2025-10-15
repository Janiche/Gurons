using UnityEngine;

public class PlanetPowerUpController : MonoBehaviour
{
    [SerializeField] private UIPanelAnimator _noPowerUpPanel;
    [SerializeField] private GameObject _blackPanel;

    [SerializeField] private Constants._PowerUps _selectedPowerUp = Constants._PowerUps.none;

    [Header("Double Score")]
    [SerializeField] private GameObject _addScoreIcon;
    [SerializeField] private GameObject _activeScore;
    [SerializeField] private GameObject _removeScoreIcon;
    [SerializeField] private GameObject _inactiveScore;
    [SerializeField] private bool _scoreAdded = false;
    [SerializeField] private GameObject _noDoubleScorePanel;


    [Space]
    [Header("Extra Chance")]
    [SerializeField] private GameObject _addChanceIcon;
    [SerializeField] private GameObject _activeChance;
    [SerializeField] private GameObject _removeChanceIcon;
    [SerializeField] private GameObject _inactiveChance;
    [SerializeField] private bool _chanceAdded = false;
    [SerializeField] private GameObject _noExtraChancePanel;


    public void _DoubleScore()
    {
        if (_scoreAdded)
        {
            _RemoveScore();
        }

        else
        {
            if (Store.instance.GetBalance(Constants._Currencies.double_score) > 0)
            {
                _AddScore();
            }

            else
            {
                _selectedPowerUp = Constants._PowerUps.ScoreMultiplier;

                _noPowerUpPanel.Show();
                _blackPanel.SetActive(true);
                _noDoubleScorePanel.SetActive(true);
                _noExtraChancePanel.SetActive(false);
            }
        }
    }

    public void _ExtraChance()
    {
        if (_chanceAdded)
        {
            _RemoveChance();
        }
        else
        {
            if (Store.instance.GetBalance(Constants._Currencies.extra_chance) > 0)
            {
                _AddChance();
            }

            else
            {
                _selectedPowerUp = Constants._PowerUps.NewChance;

                //_noPowerUpPanel.Show();
                _blackPanel.SetActive(true);
                _noExtraChancePanel.SetActive(true);
                _noDoubleScorePanel.SetActive(false);
            }

        }
    }

    private void _AddChance()
    {
        _selectedPowerUp = Constants._PowerUps.none;
        _addChanceIcon.SetActive(false);
        _activeChance.SetActive(true);
        _inactiveChance.SetActive(false);
        _removeChanceIcon.SetActive(true);
        _chanceAdded = true;
        GlobalVars._chanceAdded = true;
    }

    private void _RemoveChance()
    {
        _selectedPowerUp = Constants._PowerUps.none;
        _addChanceIcon.SetActive(true);
        _activeChance.SetActive(false);
        _inactiveChance.SetActive(true);
        _removeChanceIcon.SetActive(false);
        _chanceAdded = false;
        GlobalVars._chanceAdded = false;
    }

    private void _AddScore()
    {
        _selectedPowerUp = Constants._PowerUps.none;
        _addScoreIcon.SetActive(false);
        _activeScore.SetActive(true);
        _inactiveScore.SetActive(false);
        _removeScoreIcon.SetActive(true);
        _scoreAdded = true;
        GlobalVars._scoreAdded = true;
    }

    private void _RemoveScore()
    {
        _selectedPowerUp = Constants._PowerUps.none;
        _addScoreIcon.SetActive(true);
        _activeScore.SetActive(false);
        _inactiveScore.SetActive(true);
        _removeScoreIcon.SetActive(false);
        _scoreAdded = false;
        GlobalVars._scoreAdded = false;
    }

    public void _OcultarNoPowerUp(bool _black = true)
    {
        _noPowerUpPanel.Hide();
        _noDoubleScorePanel.SetActive(false);
        _noExtraChancePanel.SetActive(false);

        if (_black)
            _blackPanel.SetActive(false);
    }

    //PATCH 1.1.1 => 1.2.0: NO EXISTE
    public void _AsignarPostCompra()
    {
        switch (_selectedPowerUp)
        {
            case Constants._PowerUps.ScoreMultiplier:
                _AddScore();
                break;

            case Constants._PowerUps.NewChance:
                _AddChance();
                break;
        }

        _selectedPowerUp = Constants._PowerUps.none;
    }

}
