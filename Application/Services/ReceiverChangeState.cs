namespace WebUI;

public class ReceiverChangeState
{
    private string? receiverId;
    public event Action OnReceiverIdChanged;

    public string? ReceiverId
    {
        get => receiverId;
        set
        {
            if (receiverId != value)
            {
                receiverId = value;
                OnReceiverIdChanged?.Invoke();
            }
        }
    }
}