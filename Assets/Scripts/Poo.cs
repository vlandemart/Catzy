using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poo : MonoBehaviour, IInteractable
{
	[SerializeField]
	private int priority = 3;

	public void Spawn(Vector3 pos)
	{
		this.gameObject.SetActive(true);
		pos.z = pos.y;
		transform.position = pos;
	}

	public float Interact(Player player)
	{
		return (0.5f);
	}

	public void OnInteractionComplete(Player player)
	{
		player.CleanInteractable();
		PooPool.Instance.ReturnToPool(this);
	}

	public string InteractionName()
	{
		return ("clean");
	}

	public bool Interactable(Player player)
	{
		return (gameObject.activeInHierarchy);
	}

	public int InteractionPriority()
	{
		return (priority);
	}
}
