using UnityEngine;

using Doozy.Engine.UI;

public class MessageView : MonoBehaviour
{
    [SerializeField] private UIView _panel;

    public void _ActiveView()
    {
        _panel.Show();
    }

    public void _ClosePanel()
    {
        _panel.Hide();
    }
}
