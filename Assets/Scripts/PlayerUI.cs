using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vlandemart.Localization;

public class PlayerUI : MonoBehaviour
{
	public static PlayerUI Instance;

	[SerializeField]
	private GameObject tooltipObj;
	[SerializeField]
	private TMP_Text tooltipText;

	[SerializeField]
	private GameObject dialogueObj;
	[SerializeField]
	private TMP_Text dialogueText;
	private string dialogueFullText;
	private int dialogueCurrentLetter;
	private float dialogueWaitTimeLetter = 0.05f;
	private float dialogueWaitTimePeriod = 0.3f;
	private float dialoguefadeTime = 1f;

	[SerializeField]
	private Image waitTimerImage;
	private float waitTimerMax;
	private float waitTimerCurrent;

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(this);
			return;
		}
		Instance = this;
		dialogueObj.SetActive(false);
		tooltipObj.SetActive(false);
		waitTimerImage.gameObject.SetActive(false);
	}

	private void Update()
	{
		HandleTimer();
	}

	public void ShowTooltip(string tip)
	{
		tooltipObj.SetActive(true);
		tooltipText.text = LocalizationManager.Instance.Localize(tip);
	}

	public void HideTooltip()
	{
		tooltipObj.SetActive(false);
	}

	public void StartThought(string thoughtText)
	{
		dialogueFullText = LocalizationManager.Instance.Localize(thoughtText);;
		dialogueObj.SetActive(true);
		dialogueText.text = "";
		dialogueCurrentLetter = 0;
		StartCoroutine(Dialogue());
	}

	private IEnumerator Dialogue()
	{
		if (dialogueCurrentLetter == -1)
			yield return null;

		while (dialogueCurrentLetter < dialogueFullText.Length)
		{
			dialogueText.text += dialogueFullText[dialogueCurrentLetter];
			if (dialogueFullText[dialogueCurrentLetter] == '.')
				yield return new WaitForSeconds(dialogueWaitTimePeriod);
			else
				yield return new WaitForSeconds(dialogueWaitTimeLetter);
			dialogueCurrentLetter++;
		}
		yield return new WaitForSeconds(dialoguefadeTime);
		dialogueCurrentLetter = -1;
		dialogueObj.SetActive(false);
	}

	//Timer
	public void StartWaitTimer(float time)
	{
		waitTimerMax = time;
		waitTimerCurrent = 0;
		waitTimerImage.gameObject.SetActive(true);
		waitTimerImage.fillAmount = 0f;
	}

	private void HandleTimer()
	{
		if (waitTimerMax <= 0)
			return;

		waitTimerCurrent += Time.deltaTime;
		waitTimerImage.fillAmount = waitTimerCurrent / waitTimerMax;

		if (waitTimerCurrent < waitTimerMax)
			return;

		waitTimerMax = 0;
		waitTimerCurrent = 0;
		waitTimerImage.gameObject.SetActive(false);
	}
}
