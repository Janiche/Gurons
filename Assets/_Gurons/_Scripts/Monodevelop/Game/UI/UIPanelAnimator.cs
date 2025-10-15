using System.Collections;
using UnityEngine;

using NaughtyAttributes;

[RequireComponent(typeof(CanvasGroup))]
public class UIPanelAnimator : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private Coroutine _currentAnimation;
    private Vector3 _hiddenScale = Vector3.zero;
    private Vector3 _targetScale = Vector3.one;



    [Header("Config")]
    [Tooltip("Duración completa de la animación")]
    [Range(0, 2)][SerializeField] private float _duration = 0.5f;


    #region METODOS FUNCIONAMIENTO ANIMACIÓN
    void Awake()
    {
        if (_canvasGroup == null)
            _canvasGroup = GetComponent<CanvasGroup>();

        transform.localScale = _hiddenScale;
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
    }

    [Button("Show")]
    public void Show()
    {
        if (_currentAnimation != null)
            StopCoroutine(_currentAnimation);
        _currentAnimation = StartCoroutine(AnimatePopUp(true));
    }

    [Button("Force Show")]
    public void _ForceShow()
    {
        if (_canvasGroup == null)
            _canvasGroup = GetComponent<CanvasGroup>();

        transform.localScale = Vector3.one;
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = false;
    }


    [Button("Hide")]
    public void Hide()
    {
        if (_currentAnimation != null)
            StopCoroutine(_currentAnimation);
        _currentAnimation = StartCoroutine(AnimatePopUp(false));

    }

    [Button("Force Hide")]
    public void _ForceHide()
    {
        if (_canvasGroup == null)
            _canvasGroup = GetComponent<CanvasGroup>();
        transform.localScale = _hiddenScale;
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = true;
    }
    private IEnumerator AnimatePopUp(bool _show)
    {
        float _startTime = Time.time;
        float _endTime = Time.time + _duration;

        Vector3 _startScale = transform.localScale;
        Vector3 _endScale = _show ? _targetScale : _hiddenScale;

        float _startAlpha = _canvasGroup.alpha;
        float _endAlpha = _show ? 1 : 0;

        _canvasGroup.blocksRaycasts = _show;

        while (Time.time < _endTime)
        {
            float t = (Time.time - _startTime) / _duration;
            float easeT = EaseInOut(Mathf.Clamp01(t));
            //float easeT = _show ? EaseOut(t) : EaseIn(t);
            //float easeT =_show ? EaseOut(t) : EaseIn(1f-t);

            transform.localScale = Vector3.LerpUnclamped(_startScale, _endScale, easeT);
            _canvasGroup.alpha = Mathf.Lerp(_startAlpha, _endAlpha, t);

            yield return null;
        }

        transform.localScale = _endScale;
        _canvasGroup.alpha = _endAlpha;

        _canvasGroup.blocksRaycasts = _show;
        _currentAnimation = null;

    }

    private float EaseInOut(float t)
    {
        return t < 0.5f ? 2f * t * t : -1f + (4f - 2f * t) * t;
    }

    #endregion

    #region METODOS DE POPUP

    public void _ClosePopUp()
    {
        Hide();
    }


    #endregion

}
