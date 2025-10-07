using UnityEngine;

using UnityEngine.UI;
using TMPro;



namespace Bermost
{
    public class AdReward : MonoBehaviour
    {

        [SerializeField] TextMeshProUGUI _title;
        [SerializeField] private Image _baseImage = null;
        [SerializeField] TextMeshProUGUI _amount;
        [SerializeField] private Image _icon = null;
        [SerializeField] private GameObject _ticket = null;

        [Space]
        [Header("Colours")]
        [SerializeField] private Color _lockedColor;
        [SerializeField] private Color _unlockedColor;

        [Space]
        [Header("Buttons")]
        [SerializeField] private GameObject _get;
        [SerializeField] private GameObject _unlock;
        [SerializeField] private GameObject _received;

        public Button _GetButton
        {
            get { return _get.GetComponent<Button>(); }
        }

        [Header("Reward")]
        [SerializeField] private Reward _reward;

        int _productAmount = 0;

        public void _Init(Reward reward, string title = "")
        {
            _title.text = title;
            _icon.sprite = reward.icon;
            _productAmount = reward.amount;

            _reward = reward;
            _amount.text = _productAmount.ToString();
        }

        public void _WatchAds()
        {
            //AdsManager.instance._ShowRewarded();
        }

        private void _Lock()
        {
            _baseImage.color = _lockedColor;
            _get.SetActive(false);
            _unlock.SetActive(true);
            _received.SetActive(false);

        }

        public void _Unlock()
        {
            _baseImage.color = _unlockedColor;
            _get.SetActive(true);
            _unlock.SetActive(false);
            _received.SetActive(false);
        }

        public void _Receive()
        {
            //Debug.LogError("RECEIVED");
            _get.SetActive(false);
            _unlock.SetActive(false);
            _received.SetActive(true);
            _ticket.SetActive(true);
        }

        public void _FinishAd()
        {
            //Debug.LogError("FINISHED");
            Store.instance.GiveProduct(_reward.productId, _reward.amount);
        }
    }

}