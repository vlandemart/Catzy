using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerTrigger : MonoBehaviour, IInteractable
{
	[SerializeField]
	private int priority = 2;

	public float Interact(Player player)
	{
		Computer.Instance.gameObject.SetActive(true);
		Player.Instance.HasInput = false;
		return (0.1f);
	}

	public void OnInteractionComplete(Player player)
	{
		return;
	}

	public string InteractionName()
	{
		return ("computer");
	}

	public bool Interactable(Player player)
	{
		return (!player.Busy);
	}

	public int InteractionPriority()
	{
		return (priority);
	}
}
