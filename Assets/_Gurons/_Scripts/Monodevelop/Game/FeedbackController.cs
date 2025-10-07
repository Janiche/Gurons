using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class FeedbackController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _feedback;
    [SerializeField] private Sprite[] _totalFeedback;

    [SerializeField] private Vector2 _startPos;


    int _currentFeedback = 0;
    bool _active = false;
    Coroutine _hideRoutine;


    public void _ShowFeedback(int _index)
    {
        if (!_active)
        {
            if (_index < _totalFeedback.Length)
            {
                _startPos = _feedback.transform.localPosition;
                _currentFeedback = _index;
                _active = true;

                _Appear();
            }
        }
    }

    private void _Appear()
    {
        _feedback.gameObject.SetActive(true);
        _feedback.sprite = _totalFeedback[_currentFeedback];

        //OPACITY
        _feedback.DOColor(Color.white, 0.4f);

        //POSITION
        _feedback.transform.DOLocalMoveY(1.4f, 0.4f);

        //SCALE
        _feedback.transform.DOScale(1, 0.1f).OnComplete(() => _hideRoutine = StartCoroutine(_HideFeedback()));
    }

    private void _Disappear()
    {
        //_feedback.sprite = _totalFeedback[_currentFeedback];

        //OPACITY
        _feedback.DOColor(new Color(1, 1, 1, 0), 0.3f).OnComplete(() => _Reset());

        //SCALE
        //_feedback.transform.DOScale(0.3f, 0.2f)
        _UnlockSlot();

        //POSITION
        //_feedback.transform.DOMoveY(-2, 0.6f);
    }

    private void _UnlockSlot()
    {
        _active = false;
    }


    IEnumerator _HideFeedback()
    {
        yield return new WaitForSeconds(0.5f);
        StopCoroutine(_hideRoutine);
        _Disappear();

    }

    private void _Reset()
    {
        _feedback.gameObject.SetActive(false);

        _feedback.transform.localPosition = _startPos;
        _feedback.transform.localScale = Vector3.one * 0.3f;
    }
}
