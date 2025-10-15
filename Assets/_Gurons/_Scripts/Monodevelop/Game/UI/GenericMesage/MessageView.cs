using UnityEngine;


public class MessageView : UIPanelAnimator
{
    //[SerializeField] private UIView _panel;

    public void _ActiveView()
    {
        Show();
    }

    public void _ClosePanel()
    {
        Hide();
    }
}
