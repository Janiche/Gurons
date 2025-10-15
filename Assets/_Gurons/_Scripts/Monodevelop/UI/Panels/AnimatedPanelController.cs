using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimatedPanelController : MonoBehaviour
{
    [SerializeField] private GameObject _blackPanel = null;

    [Space]
    [Header("Panels")]
    [SerializeField] UIPanelAnimator _adsPanel = null;
    [SerializeField] UIPanelAnimator _noAdsPanel = null;
    [SerializeField] UIPanelAnimator _scratchPanel = null;
    [SerializeField] UIPanelAnimator _rateAppPanel = null;
    [SerializeField] UIPanelAnimator _noLifePanel = null;
    [SerializeField] UIPanelAnimator _creditPanel = null;

    public void _ShowAds()
    {
        _adsPanel.Show();//DOOZY
        _blackPanel.SetActive(true);
    }

    public void _ShowRateApp()
    {
        _rateAppPanel.Show();//DOOZY
        _blackPanel.SetActive(true);
    }

    public void _ShowNoLife()
    {
        _noLifePanel.Show();
        _blackPanel.SetActive(true);
    }

    public void _ShowScratch()
    {
        _scratchPanel.Show();
        _blackPanel.SetActive(true);
    }

    public void _ShowCredits()
    {
        _creditPanel.Show();
        _blackPanel.SetActive(true);
    }

    public void _ShowNoAds()
    {
        _noAdsPanel.Show();
        _blackPanel.SetActive(true);
    }

    public void _HideBlack()
    {
        _blackPanel.SetActive(false);
    }

}
