using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using Bermost;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class AchivementItem : MonoBehaviour
{
    [SerializeField] private Color _unlockedColor;
    [SerializeField] private Color _lockedColor;
    [SerializeField] private Color _claimedColor;
    [SerializeField] private Image _bg;


    [Space]
    [SerializeField] Reward _reward;

    [Space]
    [SerializeField] private Image _achivementIcon;
    [SerializeField] private TextMeshProUGUI _achivementName;
    [SerializeField] private TextMeshProUGUI _achivementDescription;
    [SerializeField] private Button _reclaim;


    [Space]
    [SerializeField] private Image _rewardIcon;
    [SerializeField] private TextMeshProUGUI _rewardAmount;

    [SerializeField] protected ObscuredInt _id = -1;
    protected Constants._Currencies _rewardCurrency = Constants._Currencies.none;
    protected AchievementCondition _achivement;
    protected ObscuredBool _unlocked = false;
    protected ObscuredBool _claimed = false;
    protected ObscuredInt _amount = 0;

    public void _Init(string _name, string _desc, AchievementCondition _condition, bool _unlock, bool claimed)
    {
        _id = _condition.id;
        _achivementIcon.sprite = _condition._icon;
        _achivementIcon.preserveAspect = true;

        _achivementName.text = _name;
        _achivementDescription.text = _desc;

        _unlocked = _unlock;
        _claimed = claimed;

        if (_unlock)
        {
            _achivementIcon.color = Color.white;

            if (!claimed)
            {
                _bg.color = _unlockedColor;
                _reward = _condition._reward;
                _reclaim.gameObject.SetActive(true);

                if (_reward != null)
                {
                    _rewardIcon.sprite = _reward.icon;
                    _rewardIcon.preserveAspect = true;

                    _rewardCurrency = _reward.productId;
                    _amount = _reward.amount;
                    _rewardAmount.text = _reward.amount.ToString();
                }

            }

            else
            {
                _reclaim.gameObject.SetActive(false);
                _bg.color = _claimedColor;
            }
        }

        else
        {
            _bg.color = _lockedColor;
            _achivementIcon.color = Color.black;
            _reclaim.gameObject.SetActive(false);
        }
    }

    public void _Reclaim()
    {

        if (_reward != null)
        {
            Store.instance.GiveProduct(_rewardCurrency, _amount);
        }
        _bg.color = _claimedColor;
        _reclaim.gameObject.SetActive(false);

        AchivementsController.ReclaimAchievement(_id);
    }

}
