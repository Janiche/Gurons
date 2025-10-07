using UnityEngine;

using UnityEngine.UI;

public class TimerView : MonoBehaviour
{
    [SerializeField] private GameObject _view;

    [Header("Timer Limits")]
    private float _dangerLimit = 0;
    private float _criticalLimit = 0;

    [Header("Timer Elements")]
    [SerializeField] private Slider _timerSlider = null;
    [SerializeField] private Image _fill = null;

    //[Header("Timer Colors")]
    //[SerializeField] private Color full;
    //[SerializeField] private Color danger;
    //[SerializeField] private Color critic;


    //private bool _countDown = false;
    //private float _remainTime = 0;
    //private float _elapsedTime = 0;

    public void _Init(float _time)
    {
        _timerSlider.maxValue = _time;
        _timerSlider.value = _time;
        //_SetLimits();

    }

    public void _ActiveView(bool _active)
    {
        _view.SetActive(_active);
    }

    //public void _SetLimits(float _danger, float _critical)
    //{
    //    _dangerLimit = _danger;
    //    _criticalLimit = _critical;
    //}

    public void _SetValue(float _time)
    {
        _timerSlider.value = _time;

    }

    public void _SetColor(Color _color)
    {
        _fill.color = _color;
    }

}
