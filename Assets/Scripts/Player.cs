using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
	public static Player Instance;

	[SerializeField]
	private Transform spriteTransform;
	[SerializeField]
	private float maxSpeed;
	private Rigidbody2D myRB;
	private IInteractable interactable;
	private Vector2 inputVector;
	public bool Busy;
	public bool HasInput;
	private float jobTime;
	//
	public int Money;
	public float Stress;
	public float Hunger;
	public float Sleep;
	private float speed;
	private float stressedPhraseCooldown = 5f;
	//
	public bool GotCrazy = false;
	[SerializeField]
	private SpriteRenderer mySR;
	[SerializeField]
	private Sprite playerNormal;
	[SerializeField]
	private Sprite playerCrazy;
	public float DaysCounter;
	public int WineCount;
	[SerializeField]
	private ParticleSystem starsParticle;

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(this.gameObject);
			return;
		}
		Instance = this;
	}

	public void WhineAtStart()
	{
		PlayerUI.Instance.StartThought("divorce");
		HasInput = true;
		DaysCounter = 0;
	}

	public void DrinkWine()
	{
		WineCount++;
		starsParticle.Play();
	}

	private void Start()
	{
		myRB = GetComponent<Rigidbody2D>();
		speed = maxSpeed;
		HasInput = true;
		//HasInput = false;
		mySR.sprite = playerNormal;
	}

	public void GetCrazy()
	{
		mySR.sprite = playerCrazy;
		GotCrazy = true;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		DaysCounter += Time.deltaTime;
		JobUpdate();
		HandleNeeds();
		if (Busy)
			return;
		GetInput();
		FinishGame();
	}

	private void FixedUpdate()
	{
		if (Busy)
			return;
		Movement();
	}

	private void HandleNeeds()
	{
		float dTime = Time.deltaTime;

		Sleep += dTime;
		Hunger += dTime;
		Stress += dTime;
		stressedPhraseCooldown -= dTime;
		Sleep = Mathf.Clamp(Sleep, 0f, 100f);
		Hunger = Mathf.Clamp(Hunger, 0f, 100f);
		Stress = Mathf.Clamp(Stress, 0f, 100f);
		speed = (Sleep > 90f || Hunger > 90f || Stress > 90f) ? maxSpeed / 2 : maxSpeed;
		if (Stress >= 100f)
		{
			Sleep += dTime;
			if (stressedPhraseCooldown <= 0f)
			{
				PlayerUI.Instance.StartThought("stress_" + Random.Range(0, 10));
				stressedPhraseCooldown = 15f;
			}
		}
	}

	private void JobUpdate()
	{
		if (!Busy)
			return;
		jobTime -= Time.deltaTime;
		if (jobTime > 0)
			return;
		Busy = false;
		interactable?.OnInteractionComplete(this);
	}

	public void SetJob(float time)
	{
		if (Busy)
			return;
		Busy = true;
		jobTime = time;
		PlayerUI.Instance.StartWaitTimer(jobTime);
	}

	private void GetInput()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && Computer.Instance.gameObject.activeInHierarchy)
			Computer.Instance.Close();
		if (!HasInput)
			return;
		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
		inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		if (Input.GetKeyDown(KeyCode.E))
			Interact();
	}

	private void Movement()
	{
		if (inputVector == Vector2.zero)
			return;
		myRB.position += inputVector.normalized * (speed * Time.deltaTime);
		spriteTransform.position = new Vector3(myRB.position.x, myRB.position.y, myRB.position.y);
	}

	private void Interact()
	{
		PlayerUI.Instance.HideTooltip();
		if (Busy || interactable == null || !interactable.Interactable(this))
			return;
		SetJob(interactable.Interact(this));
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		IInteractable _interactable = other.GetComponent<IInteractable>();
		if (_interactable != null && _interactable.Interactable(this))
		{
			if (interactable != null && _interactable.InteractionPriority() < interactable.InteractionPriority())
				return;
			interactable = _interactable;
			PlayerUI.Instance.ShowTooltip(interactable.InteractionName());
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		IInteractable _interactable = other.GetComponent<IInteractable>();
		if (_interactable != null && _interactable == interactable)
		{
			if (other.GetComponent<ComputerTrigger>() != null)
				Computer.Instance.Close();
			CleanInteractable();
		}
	}

	public void CleanInteractable()
	{
		interactable = null;
		PlayerUI.Instance.HideTooltip();
	}

	public void FinishGame()
	{
		if (Money >= 100 && !GotCrazy)
		{
			PlayerHUD.Instance.WinFull.SetActive(true);
		}
		else if (WineCount >= 10)
		{
			PlayerHUD.Instance.LossFull.SetActive(true);
		}
		else if (GotCrazy && (DaysCounter / 24) > 15)
		{
			PlayerHUD.Instance.LossKinda.SetActive(true);
		}
		else if (CatManager.Instance.Cats.Count >= 20)
		{
			PlayerHUD.Instance.LossKinda.SetActive(true);
		}
		else if ((DaysCounter / 24) > 15)
		{
			PlayerHUD.Instance.WinKinda.SetActive(true);
		}
		//We didn't finish the game
	}
}
