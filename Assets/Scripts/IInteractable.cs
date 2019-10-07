public interface IInteractable
{
	float Interact(Player player);
	void OnInteractionComplete(Player player);
	string InteractionName();
	bool Interactable(Player player);
	int InteractionPriority();
}
