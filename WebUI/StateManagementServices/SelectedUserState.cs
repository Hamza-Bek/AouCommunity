namespace WebUI.StateManagementServices;

public class SelectedUserState
{
    private string _receiverId;

    public string ReceiverId
    {
        get => _receiverId;
        set
        {
            _receiverId = value;
            NotifyStateChanged();
        }
    }
    
    public event Action OnChange;
    private void NotifyStateChanged() => OnChange?.Invoke();

}