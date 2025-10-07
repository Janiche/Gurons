using UnityEngine;

public class EndTutorialController : MonoBehaviour
{
    [SerializeField] private EndTutorialView _view;

    public void _OnSuccess()
    {
        _view._OnSuccess();
    }

    public void _OnFailed()
    {
        _view._OnFailed();
    }

    public void _OnPlanet()
    {
        GameEvents.UICommand(Constants._UICommand._nextPlanet);
    }

    public void _OnRetry()
    {
        GameEvents.UICommand(Constants._UICommand._retryTutorial);
    }

    public void _OnSkip()
    {
        GameEvents.UICommand(Constants._UICommand._skipTutorial);
    }
}
