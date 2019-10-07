using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Vlandemart.Localization;

public class Computer : MonoBehaviour
{
	public static Computer Instance;

	//Cat shelter
	[SerializeField]
	private GameObject shelterObj;
	[SerializeField]
	private int adoptionPrice = 15;
	[SerializeField]
	private TMP_Text shelterTitleText;
	[SerializeField]
	private TMP_Text shelterDescText;
	[SerializeField]
	private TMP_InputField shelterInputField;
	//Job
	[SerializeField]
	private GameObject jobObj;
	[SerializeField]
	private TMP_Text jobTitleText;
	[SerializeField]
	private TMP_Text jobDescText;
	[SerializeField]
	private float jobTime = 3f;
	[SerializeField]
	private int jobMoney = 6;
	[SerializeField]
	private float jobSleep = 10f;
	[SerializeField]
	private float jobStress = 10f;

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(this.gameObject);
			return;
		}
		Instance = this;
	}

	private void Start()
	{
		//Cat shelter
		shelterTitleText.text = LocalizationManager.Instance.Localize("shelter_title");
		shelterDescText.text = LocalizationManager.Instance.Localize("shelter_desc");

		this.gameObject.SetActive(false);
	}

	public void OpenPage(GameObject obj)
	{
		shelterObj.SetActive(false);
		jobObj.SetActive(false);
		obj.SetActive(true);
		Player.Instance.HasInput = false;
	}

	public void AdoptCat()
	{
		if (Player.Instance.Money < adoptionPrice || shelterInputField.text.Length < 1)
			return;
		CatManager.Instance.CreateCat(shelterInputField.text);
		Player.Instance.Money -= adoptionPrice;
		Close();
	}

	public void Work()
	{
		if (Player.Instance.Busy || Player.Instance.Sleep >= 100f)
			return;
		Player.Instance.Money += jobMoney;
		Player.Instance.Stress += jobStress;
		Player.Instance.Sleep += jobSleep;
		Player.Instance.SetJob(jobTime);
	}

	public void Close()
	{
		Player.Instance.HasInput = true;
		gameObject.SetActive(false);
	}
}
