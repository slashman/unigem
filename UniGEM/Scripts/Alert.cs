using UnityEngine;

[RequireComponent(typeof(Window))]
public class Alert : MonoBehaviour
{
    public Label messageLabel;
    public void Show(string message)
    {
        messageLabel.SetText(message);
        GameUI.i.blockingMask.Pop(GetComponent<Window>());
    }

    public void Hide()
    {
        GameUI.i.blockingMask.Hide();
    }
}
