using UnityEngine;

using UnityEngine.UI;
using TMPro;

using Doozy.Engine.UI;

public class GameOverView : MonoBehaviour
{
    [SerializeField] private UIView _view;

    [Header("Text field for Score")]
    [SerializeField] private TextMeshProUGUI _score;
    [SerializeField] private TextMeshProUGUI _best;

    [Header("Status")]
    [SerializeField] private TextMeshProUGUI _lifeCount;
    [SerializeField] private TextMeshProUGUI _diamondCount;
    [SerializeField] private TextMeshProUGUI _coinsCount;

    [Header("Different Continue Button")]
    [SerializeField] private GameObject _diamondContinue;
    [SerializeField] private GameObject _adContinue;
    [SerializeField] private TextMeshProUGUI _continueCost;



    public void _Init(int _cost)
    {
        _continueCost.text = _cost.ToString();
    }

    public void _SetScore(int _score, int _best, int lifes, int diamonds, int coins)
    {
        this._score.text = _score.ToString("00");
        this._best.text = _best.ToString("00");

        _lifeCount.text = lifes.ToString();
        _diamondCount.text = diamonds.ToString();
        _coinsCount.text = coins.ToString();

        _ActiveView();
    }

    public void _UpdateStatus(int lifes, int diamonds, int coins)
    {
        _lifeCount.text = lifes.ToString();
        _diamondCount.text = diamonds.ToString();
        _coinsCount.text = coins.ToString();
    }


    /// <summary>
    /// Actives a continue button
    /// </summary>
    /// <param name="_button">0[Diamond & Ads] 1[Chance & Ads] 2[ONLY Ads] 3[No Continue]</param>
    public void _ActiveButton(Constants._GameContinue _gameContinue)
    {
        _diamondContinue.SetActive(false);
        _adContinue.SetActive(false);

        switch (_gameContinue)
        {
            //DIAMOND CONTINUE
            case Constants._GameContinue._diamondAndAds:
                _diamondContinue.SetActive(true);
                _adContinue.SetActive(true);

                break;

            //ADS CONTINUE
            case Constants._GameContinue._onlyAds:
                _diamondContinue.SetActive(true);
                _diamondContinue.GetComponent<Button>().interactable = false;
                _adContinue.SetActive(true);
                _adContinue.GetComponent<Button>().interactable = true;
                break;

            //NO CONTINUE
            case Constants._GameContinue._noContinue:


                //PATCH 1.1.1 => 1.2.0 : _diamondContinue.SetActive(true);
                _diamondContinue.SetActive(false);

                _diamondContinue.GetComponent<Button>().interactable = false;
                _adContinue.SetActive(false);
                _adContinue.GetComponent<Button>().interactable = false;

                break;
        }
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
