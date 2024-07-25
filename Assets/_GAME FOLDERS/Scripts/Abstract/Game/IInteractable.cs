public interface IInteractable
{
    event System.Action OnCollected;
    void Interact(PlayerHealth health = null);
}
