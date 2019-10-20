using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cat : MonoBehaviour, IInteractable
{
	private bool pickedUp;
	[SerializeField]
	private int priority = 0;
	[SerializeField]
	private AudioSource myAs;
	[SerializeField]
	private TMP_Text nameText;

	[SerializeField]
	private SpriteRenderer srBody;
	[SerializeField]
	private SpriteRenderer srFace;
	[SerializeField]
	private Transform faceLyingTransform;
	[SerializeField]
	private Transform faceStayingTransform;
	[SerializeField]
	private Sprite spriteLying;
	[SerializeField]
	private Sprite spriteStaying;
	[SerializeField]
	private Sprite spriteFace;

	private CatSpot selectedSpot;
	private bool petted;

	//Stats
	[SerializeField]
	private float speed = 1.5f;
	[SerializeField]
	private float maxHunger = 20f;
	[SerializeField]
	private float hunger = 5f;
	[SerializeField]
	private float hungerRate = 1f;
	[SerializeField]
	private float maxEnergy = 20f;
	[SerializeField]
	private float energy = 5f;
	[SerializeField]
	private float energyRate = 1f;
	[SerializeField]
	private float sleepTime = 5f;
	[SerializeField]
	private float poo = 0f;
	[SerializeField]
	private float maxPoo = 20f;
	[SerializeField]
	private ParticleSystem sleepParticle;
	[SerializeField]
	private ParticleSystem noiseParticle;
	[SerializeField]
	private ParticleSystem heartParticle;

	public void SetUp(CatIdentity identity)
	{
		name = identity._name;
		spriteFace = identity._spriteFace;
		spriteLying = identity._spriteLying;
		spriteStaying = identity._spriteStaying;

		nameText.text = name;
		srBody.sprite = spriteStaying;
		srFace.sprite = spriteFace;
		srFace.transform.position = faceStayingTransform.position;
		StartCoroutine(Behaviour());
	}

	private void Start()
	{
		maxHunger += Random.Range(-5f, 5f);
		hunger = maxHunger;
		maxEnergy += Random.Range(-5f, 5f);
		energy = maxEnergy;
		maxPoo += Random.Range(-5f, 5f);
		speed += Random.Range(-0.5f, 0.5f);
	}

	private void Update()
	{
		energy -= Time.deltaTime * energyRate;
		energy = Mathf.Clamp(energy, 0, maxEnergy);
		hunger -= Time.deltaTime * hungerRate;
		hunger = Mathf.Clamp(hunger, 0, maxHunger);
		if (hunger > 10f)
		{
			poo += Time.deltaTime;
			poo = Mathf.Clamp(poo, 0, maxPoo);
			if (poo >= maxPoo)
				Poo(false);
		}
	}

	private IEnumerator Behaviour()
	{
		//Cat is born
		//Play some nice animation - jumping out of box or something like that
		yield return new WaitForSeconds(0.5f);
		while (true)
		{
			while(pickedUp)
				yield return null;
			if (petted)
			{
				yield return new WaitForSeconds(1.5f);
				petted = false;
			}
			if (selectedSpot == null)
			{
				if (hunger <= 5f)
				{
					PlaySound(CatManager.Instance.GetMeow());
					noiseParticle.Play();
					selectedSpot = CatManager.Instance.FindFoodbowl();
					if (selectedSpot == null)
						yield return new WaitForSeconds(3f);
				}
				else if (poo >= maxPoo / 2)
				{
					selectedSpot = CatManager.Instance.FindLitterbox();
				}
				if (selectedSpot == null)
					selectedSpot = CatManager.Instance.FindInterestingPlace();
			}

			if (Move())
			{
				selectedSpot?.OnSpotReach(this);
				yield return new WaitForSeconds(0.5f);
				if (energy <= 0)
				{
					yield return Dance(3);
					PlaySound(CatManager.Instance.GetPurr());
					srBody.sprite = spriteLying;
					srFace.transform.position = faceLyingTransform.position;
					sleepParticle.Play();
					energy = maxEnergy;
					yield return new WaitForSeconds(sleepTime);
					sleepParticle.Stop();
				}
				srBody.sprite = spriteStaying;
				srFace.transform.position = faceStayingTransform.position;
				ClearSelectedSpot();
				if (Random.value > 0.8f)
					yield return Dance(6);
			}
			yield return null;
		}
	}

	private IEnumerator Dance(int danceMoves)
	{
		int n = 0;
		while (n < danceMoves)
		{
			Vector3 scale = srBody.transform.localScale;
			scale.x = -scale.x;
			srBody.transform.localScale = scale;
			yield return new WaitForSeconds(0.3f);
			n++;
		}
	}

	private void PlaySound(AudioClip clip)
	{
		myAs.clip = clip;
		myAs.pitch = Random.Range(0.8f, 1.2f);
		myAs.Play();
	}

	public void Eat()
	{
		hunger = maxHunger;
		energy += maxEnergy / 5f;
		heartParticle.Play();
	}

	public bool Poo(bool inLitterbox)
	{
		float tmp = poo;
		poo = 0;
		energy += maxEnergy / 5f;
		hunger += 1f;
		if (inLitterbox)
			return true;
		PooPool.Instance.SpawnPoo(transform.position);
		return (tmp < 4f);
	}

	private void ClearSelectedSpot()
	{
		if (selectedSpot == null)
			return;
		selectedSpot.ClearSpot();
		selectedSpot = null;
	}

	private bool Move()
	{
		Vector3 movePos;
		Vector3 currPos = transform.position;
		if (selectedSpot == null)
			return (true);
		movePos = selectedSpot.transform.position;
		Vector3 dir = (movePos - currPos).normalized;
		if (dir.x > 0)
		{
			if (movePos.x > 7.5f && currPos.x < 6.5f)
				movePos = new Vector3(7f, 0.5f);
			if (movePos.x > 9f && (currPos.x >= 6.5f && currPos.x < 9f))
				movePos = new Vector3(9.5f, 0.5f);
		}
		else
		{
			if (movePos.x < 9f && currPos.x > 9.5f)
				movePos = new Vector3(9f, 0.5f);
			if (movePos.x < 6.5f && (currPos.x > 7f && currPos.x < 9.5f))
				movePos = new Vector3(6.5f, 0.5f);
		}
		dir = (movePos - currPos).normalized;
		Vector3 scale = srBody.transform.localScale;
		scale.x = (dir.x > 0) ? -1f : 1f;
		srBody.transform.localScale = scale;

		currPos += dir * (speed * Time.deltaTime);
		transform.position = new Vector3(currPos.x, currPos.y, currPos.y);
		return (Vector2.Distance(selectedSpot.transform.position, currPos) < 0.2f);
	}

	private void PickUp()
	{
		pickedUp = true;
	}

	public void PutDown()
	{
		pickedUp = false;
	}

	void Pet(Player player)
	{
		petted = true;
		if (hunger <= 0)
			return;
		heartParticle.Play();
		PlaySound(CatManager.Instance.GetPurr());
		player.Stress -= 10;
	}

	public bool Interactable(Player player)
	{
		return (hunger > 0);
	}

	public float Interact(Player player)
	{
		Pet(player);
		ClearSelectedSpot();
		return (0.2f);
	}

	public void OnInteractionComplete(Player player)
	{
	}

	public string InteractionName()
	{
		return ("pet_cat");
	}

	public int InteractionPriority()
	{
		return (priority);
	}
}
