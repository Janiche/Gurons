using UnityEngine;

using Doozy.Engine.UI;

public class EndTutorialView : MonoBehaviour
{
    [SerializeField] private UIView _success;
    [SerializeField] private UIView _failed;

    public void _OnSuccess()
    {
        _success.Show();
    }

    public void _OnFailed()
    {
        _failed.Show();
    }
}
