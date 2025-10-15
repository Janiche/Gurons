using UnityEngine;


public class EndTutorialView : MonoBehaviour
{
    [SerializeField] private UIPanelAnimator _success;
    [SerializeField] private UIPanelAnimator _failed;

    public void _OnSuccess()
    {
        _success.Show();
    }

    public void _OnFailed()
    {
        _failed.Show();
    }
}
