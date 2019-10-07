using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSpot : MonoBehaviour
{
	public bool Occupied = false;

	public virtual bool OnSpotReach(Cat cat)
	{
		return false;
	}

	public void ClearSpot()
	{
		CatManager.Instance.InterestingSpotsEmpty.Add(this);
		Occupied = false;
	}
}
