using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Doozy.Engine.UI;

public class AnimatedPanelController : MonoBehaviour
{
    [SerializeField] private GameObject _blackPanel = null;

    [Space]
    [Header("Panels")]
    [SerializeField] UIView _adsPanel = null;
    [SerializeField] UIView _noAdsPanel = null;
    [SerializeField] UIView _scratchPanel = null;
    [SerializeField] UIView _rateAppPanel = null;
    [SerializeField] UIView _noLifePanel = null;
    [SerializeField] UIView _creditPanel = null;

    public void _ShowAds()
    {
        _adsPanel.Show();
        _blackPanel.SetActive(true);
    }

    public void _ShowRateApp()
    {
        _rateAppPanel.Show();
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
