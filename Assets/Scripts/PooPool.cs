using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooPool : MonoBehaviour
{
	public static PooPool Instance;

	private List<Poo> pooListEnabled = new List<Poo>();
	private List<Poo> pooListDisabled = new List<Poo>();
	[SerializeField]
	private Poo pooPrefab;

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(this);
			return;
		}
		Instance = this;
	}

	public void SpawnPoo(Vector3 pos)
	{
		Poo poo;
		if (pooListDisabled.Count <= 0)
		{
			poo = Instantiate(pooPrefab, pos, Quaternion.identity);
			poo.gameObject.SetActive(false);
			pooListDisabled.Add(poo);
		}
		poo = pooListDisabled[0];
		poo.Spawn(pos);
		pooListEnabled.Add(poo);
		pooListDisabled.Remove(poo);
	}

	public void ReturnToPool(Poo poo)
	{
		pooListDisabled.Add(poo);
		pooListEnabled.Remove(poo);
		poo.gameObject.SetActive(false);
	}
}
