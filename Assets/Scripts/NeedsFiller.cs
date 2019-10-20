using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedsFiller : MonoBehaviour, IInteractable
{
	[SerializeField]
	private float interactTime = 1f;
	[SerializeField]
	private int moneyGain = 5;
	[SerializeField]
	private float stressGain = 5;
	[SerializeField]
	private float sleepGain = 5;
	[SerializeField]
	private float hungerGain = 0;
	[SerializeField]
	private string interactionName;
	[SerializeField]
	private int priority = 2;
	[SerializeField]
	private bool addsWine = false;

	public bool Interactable(Player player)
	{
		return (true);
	}

	public float Interact(Player player)
	{
		if (moneyGain < 0 && player.Money + moneyGain < 0)
		{
			PlayerUI.Instance.StartThought("need_money");
			return (0f);
		}
		return (interactTime);
	}

	public void OnInteractionComplete(Player player)
	{
		if (moneyGain < 0 && player.Money + moneyGain < 0)
			return;
		player.Money += moneyGain;
		player.Stress += stressGain;
		player.Hunger += hungerGain;
		player.Sleep += sleepGain;
		if (addsWine)
			player.DrinkWine();
	}

	public string InteractionName()
	{
		return (interactionName);
	}

	public int InteractionPriority()
	{
		return (priority);
	}
}
