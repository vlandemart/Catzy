using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vlandemart.Localization;

public class PlayerHUD : MonoBehaviour
{
	public static PlayerHUD Instance;

	[SerializeField]
	private Image hungerBar;
	[SerializeField]
	private TMP_Text hungerText;
	[SerializeField]
	private Image sleepBar;
	[SerializeField]
	private TMP_Text sleepText;
	[SerializeField]
	private Image stressBar;
	[SerializeField]
	private TMP_Text stressText;
	[SerializeField]
	private TMP_Text moneyText;
	//
	[SerializeField]
	private TMP_Text daysText;
	[SerializeField]
	private Image daysTimer;
	//
	public GameObject LossFull;
	public GameObject LossKinda;
	public GameObject WinFull;
	public GameObject WinKinda;

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
		hungerText.text = LocalizationManager.Instance.Localize("hunger");
		sleepText.text = LocalizationManager.Instance.Localize("sleep");
		stressText.text = LocalizationManager.Instance.Localize("stress");
	}

	private void Update()
	{
		hungerBar.fillAmount = Player.Instance.Hunger / 100;
		sleepBar.fillAmount = Player.Instance.Sleep / 100;
		stressBar.fillAmount = Player.Instance.Stress / 100;
		moneyText.text = LocalizationManager.Instance.Localize("money") + " - " + Player.Instance.Money + "$";
		//
		int day = (int)Player.Instance.DaysCounter / 24;
		daysText.text = LocalizationManager.Instance.Localize("day") + ": " + day + "/15";
		daysTimer.fillAmount = (Player.Instance.DaysCounter % 24) / 24;
	}
}
