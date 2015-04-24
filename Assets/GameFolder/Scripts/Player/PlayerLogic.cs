using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerLogic : MonoBehaviour 
{
	public GameLogic game;
	public bool isDefensivePlayer;
	public AudioClip grunt;
	private AudioSource audioSource;
	public GameObject defensivePlayerSpawnPosition;
	public GameObject offensivePlayerSpawnPosition;

	public SliderDemo healthSlider;
	public SliderDemo manaSlider;

	private int health;
	private int energy;

	// PLAYER CURRENCY
	public Text currencyText;
	private int currentPlayerCurrency;
	public int startingPlayerCurrency;

	// PLAYER HEALTH TEXTURE
	public GameObject healthTexture;

	// Use this for initialization
	void Start () 
	{
		// TODO: Change this weird logic
		if (isDefensivePlayer)
		{
			isDefensivePlayer = false;
			switchOffensiveDefensive ();
		}
					
		health = 100;
		energy = 100;
		energyCounter = 0;
		currentPlayerCurrency = startingPlayerCurrency;
		currencyText.text = "" + startingPlayerCurrency;
		audioSource = gameObject.GetComponent<AudioSource> ();
	}

	private int energyRefreshCount = 100;
	private int energyCounter;
	// Update is called once per frame
	void Update ()
	{
		// Update current currency gui value
		//currencyText.text = "" + currentPlayerCurrency;
		
		healthSlider.SetWidgetValue (health / 100.0f);
		manaSlider.SetWidgetValue (energy / 100.0f);
		energyCounter++;
		if (energyCounter > energyRefreshCount)
		{
			if (energy < 100)
				energy += 10;
			energyCounter = 0;
		}
	}

	public void respawn()
	{
		game.killPlayerEndGame ();
		transform.position = offensivePlayerSpawnPosition.transform.position;
		transform.rotation = offensivePlayerSpawnPosition.transform.rotation;
	}

	public void dealDamage(int damageToDeal)
	{
		health -= damageToDeal;
		// Player is dead
		if (health < 0)
		{
			respawn();
		}
		// Player just takes damage
		else
		{
			audioSource.PlayOneShot(grunt);
//			Material healthMaterial = healthTexture.GetComponent<Material>();
//			healthMaterial.SetColor();

			Color color = healthTexture.GetComponent<Renderer>().material.color;
			float newAlpha = (float)health / 100.0f;
			color.a = 1.0f - newAlpha;
			healthTexture.GetComponent<Renderer>().material.color = color;
		}
	}

	public void useEnergy(int energyCost)
	{
		energy -= energyCost;
	}

	public int getHealth()
	{
		return health;
	}

	public int getEnergy()
	{
		return energy;
	}

	public void switchOffensiveDefensive()
	{
		if (!isDefensivePlayer)
		{
			isDefensivePlayer = true;
			transform.position = defensivePlayerSpawnPosition.transform.position;
			transform.rotation = defensivePlayerSpawnPosition.transform.rotation;
			HandController hand = (HandController) transform.GetChild (1).GetChild (1).
				GetChild (0).gameObject.GetComponent(typeof(HandController));
			hand.enabled = false;
			
			// Make the player not use gravity if they are defensive
			OVRPlayerController ovrController = (OVRPlayerController) GetComponent(typeof(OVRPlayerController));
			//			ovrController.changeGravityUse(false);
		}
		else
		{
			respawn();
			isDefensivePlayer = false;
			HandController hand = (HandController) transform.GetChild (1).GetChild (1).
				GetChild (0).gameObject.GetComponent(typeof(HandController));
			hand.enabled = true;
		}
	}

	// CURRENCY FUNCTIONALITY
	public int getCurrencyValue()
	{
		return currentPlayerCurrency;
	}
	
	public void changeCurrency(int currencyChange)
	{
		currentPlayerCurrency += currencyChange;
		currencyText.text = "" + currentPlayerCurrency;
	}
}
