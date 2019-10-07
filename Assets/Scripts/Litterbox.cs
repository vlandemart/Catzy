using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Litterbox : CatSpot, IInteractable
{
	public bool Empty;
	[SerializeField]
	private int priority = 2;
	private SpriteRenderer sr;
	[SerializeField]
	private Sprite spriteEmpty;
	[SerializeField]
	private Sprite spriteFilled;

	private void Start()
	{
		sr = GetComponent<SpriteRenderer>();
	}

	public override bool OnSpotReach(Cat cat)
	{
		if (!Empty)
			return false;
		if (cat.Poo(true) == false)
			return true;
		Empty = false;
		sr.sprite = spriteFilled;
		return true;
	}

	public float Interact(Player player)
	{
		return (0.5f);
	}

	public void OnInteractionComplete(Player player)
	{
		Empty = true;
		sr.sprite = spriteEmpty;
	}

	public string InteractionName()
	{
		return ("clean");
	}

	public bool Interactable(Player player)
	{
		return (!Empty);
	}

	public int InteractionPriority()
	{
		return (priority);
	}
}
