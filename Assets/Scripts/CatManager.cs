using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatManager : MonoBehaviour
{
	public static CatManager Instance;

	public List<AudioClip> Meows = new List<AudioClip>();
	public List<AudioClip> Purrs = new List<AudioClip>();
	public List<CatSpot> InterestingSpots = new List<CatSpot>();
	public List<CatSpot> InterestingSpotsEmpty = new List<CatSpot>();
	public List<Bowl> Bowls = new List<Bowl>();
	public List<Litterbox> Litterboxes = new List<Litterbox>();
	public List<Cat> Cats = new List<Cat>();
	public List<CatIdentity> CatIdentities = new List<CatIdentity>();
	[SerializeField]
	private List<Sprite> catFaces = new List<Sprite>();
	[SerializeField]
	private Cat catPrefab;

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(this);
			return;
		}
		Instance = this;
	}

	private void Start()
	{
		InterestingSpotsEmpty = new List<CatSpot>(InterestingSpots);
	}

	public void CreateCat(string _name)
	{
		Cat cat = Instantiate(catPrefab, transform.position, Quaternion.identity);
		CatIdentity identity = FindCatIdentity(_name);
		if (identity == null)
			identity = CatIdentities[Random.Range(0, CatIdentities.Count)];
		identity._name = _name;
		if (identity._spriteFace == null)
			identity._spriteFace = catFaces[Random.Range(0, catFaces.Count)];
		cat.SetUp(identity);
		Cats.Add(cat);
		if (Cats.Count >= 10 && !Player.Instance.GotCrazy)
			Player.Instance.GetCrazy();
	}

	private CatIdentity FindCatIdentity(string _name)
	{
		string str = _name.ToLower();

		foreach (CatIdentity identity in CatIdentities)
		{
			foreach (string alias in identity._knownAliases)
			{
				if (str.Contains(alias))
					return (identity);
			}
		}
		return (null);
	}

	public CatSpot FindInterestingPlace()
	{
		if (InterestingSpotsEmpty.Count <= 0)
			return (null);
		CatSpot spot = InterestingSpotsEmpty[Random.Range(0, InterestingSpotsEmpty.Count)];
		InterestingSpotsEmpty.Remove(spot);
		spot.Occupied = true;
		return (spot);
	}

	public CatSpot FindFoodbowl()
	{
		foreach (Bowl bowl in Bowls)
		{
			if (bowl.HasFood && !bowl.Occupied)
				return (bowl);
		}
		return (null);
	}

	public CatSpot FindLitterbox()
	{
		foreach (Litterbox box in Litterboxes)
		{
			if (box.Empty && !box.Occupied)
				return (box);
		}
		return (null);
	}

	public AudioClip GetMeow()
	{
		return (Meows[Random.Range(0, Meows.Count)]);
	}

	public AudioClip GetPurr()
	{
		return (Purrs[Random.Range(0, Purrs.Count)]);
	}
}

[System.Serializable]
public class CatIdentity
{
	public List<string> _knownAliases;
	public string _name;
	public Sprite _spriteFace;
	public Sprite _spriteLying;
	public Sprite _spriteStaying;
}
