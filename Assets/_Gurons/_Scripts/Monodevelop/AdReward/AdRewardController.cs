using System.Collections.Generic;
using UnityEngine;

using TMPro;
using PixelCrushers;

namespace Bermost
{


    public class AdRewardController : UIPanelAnimator
    {
        [SerializeField] private GameObject _remainingAdBubble;
        [SerializeField] private TextMeshProUGUI _remainingAdText;
        [SerializeField] private int _remainingAd = 4;
        [SerializeField] private int _maxAd = 4;

        [Space]
        [SerializeField] private TextTable textTable = null;

        [Space]
        [SerializeField] private List<Day> _days;
        [SerializeField] private List<AdReward> _adRewards;
        int _currentAd = 0;

        string _key;

        //SynchronizationContext ctx;

        void OnEnable()
        {
            //AdsManager.OnFinishedAd += FinishAd;
            //Advertisement.AddListener(this);
        }

        void OnDisable()
        {
            //AdsManager.OnFinishedAd -= FinishAd;
            //Advertisement.RemoveListener(this);
        }


        public void _Init()
        {
            //ctx = SynchronizationContext.Current;

            _remainingAd = _maxAd;
            _key = textTable.GetFieldText("Reward.Prize");

            _FillDay();
        }

        /// <summary>
        /// Fill day ads if is setted
        /// </summary>
        void _FillDay()
        {
            int _currentDay = GlobalVars.saveData.rewardData._currentDay;
            _currentAd = GlobalVars.saveData.rewardData._currentAd;

            for (int i = 0; i < _adRewards.Count; i++)
            {
                string _name = string.Format(_key, (i + 1));

                _adRewards[i]._Init(_days[_currentDay].ads[i].reward, _name);

                if (i < _currentAd)
                {
                    _remainingAd--;
                    _adRewards[i]._Receive();
                }
                else if (i == _currentAd)
                {
                    _adRewards[i]._Unlock();
                    //break;
                }
            }

            _remainingAd = Mathf.Clamp(_remainingAd, 0, _maxAd);

            _remainingAdText.text = _remainingAd.ToString();

            _remainingAdBubble.SetActive((_remainingAd > 0));
        }

        //public void OnUnityAdsReady(string placementId)
        //{
        //    if (placementId == Constants.Ads_Constants.Ads_Placement_Rewarded)
        //    {
        //        //_get.GetComponent<Button>().interactable = true;
        //    }
        //}

        //public void OnUnityAdsDidError(string message)
        //{
        //    //throw new System.NotImplementedException();
        //}

        //public void OnUnityAdsDidStart(string placementId)
        //{
        //    //throw new System.NotImplementedException();
        //}

        //public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        //{
        //    Debug.Log(placementId);

        //    if (placementId == Constants.Ads_Constants.Ads_Placement_Rewarded)
        //    {
        //        switch (showResult)
        //        {
        //            case ShowResult.Finished:
        //                //FinishAd();
        //                ctx.Post((callbackData) => FinishAd(), null);

        //                break;

        //            case ShowResult.Skipped:
        //                break;

        //            case ShowResult.Failed:
        //                Debug.LogWarning("The ad did not finish due to an error.");
        //                break;
        //        }
        //    }

        //}

        private void FinishAd(string placementId)
        {

            //ENTREGA REWARD
            _adRewards[_currentAd]._Receive();
            _adRewards[_currentAd]._FinishAd();


            //ACTUALIZA BURBUJA DE ADS
            _currentAd++;
            _remainingAd--;

            _remainingAd = Mathf.Clamp(_remainingAd, 0, _maxAd);
            _remainingAdBubble.SetActive((_remainingAd > 0));
            _remainingAdText.text = _remainingAd.ToString();

            GlobalVars.saveData.rewardData._currentAd = _currentAd;

            if (_currentAd < _adRewards.Count)
            {
                _adRewards[_currentAd]._Unlock();
            }

            SaveSystem.SaveData();

        }
    }
}