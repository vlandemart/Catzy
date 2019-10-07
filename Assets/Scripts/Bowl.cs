using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowl : CatSpot, IInteractable
{
	public bool HasFood;
	[SerializeField]
	private int price;
	[SerializeField]
	private Sprite spriteEmpty;
	[SerializeField]
	private Sprite spriteFull;
	private SpriteRenderer sr;
	[SerializeField]
	private int priority = 3;

	private void Start()
	{
		sr = GetComponent<SpriteRenderer>();
	}

	public override bool OnSpotReach(Cat cat)
	{
		if (!HasFood)
			return false;
		cat.Eat();
		HasFood = false;
		sr.sprite = spriteEmpty;
		return true;
	}

	public void FillBowl(Player player)
	{
		player.Money -= price;
		HasFood = true;
		sr.sprite = spriteFull;
	}

	public float Interact(Player player)
	{
		FillBowl(player);
		return (0.2f);
	}

	public void OnInteractionComplete(Player player)
	{
		return;
	}

	public string InteractionName()
	{
		return ("bowl");
	}

	public bool Interactable(Player player)
	{
		return (player.Money >= price && !HasFood);
	}

	public int InteractionPriority()
	{
		return (priority);
	}
}
