using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public static UIManager instance{get; set;}
	public Slider healthUI,healthBufferUI;
	public Image playerImage;
	public Text playerName;
	public Text livesText;
	public Text displayMessage;

	public GameObject enemyUI;
	public Slider enemySlider,enemyBufferSlider;
	public Text enemyName;
	public Image enemyImage;

	public float enemyUITime=4f;

	public float enemyTimer;
	private Player player;


	void Awake()
	{
		instance = this;
	}

	void Start () 
	{
		player = FindObjectOfType<Player> ();
		healthUI.maxValue = player.maxHealth;
		healthUI.value = healthUI.maxValue;
		healthBufferUI.maxValue = player.maxHealth;
		healthBufferUI.value = healthUI.maxValue;
		playerName.text = player.playerName;
		playerImage.sprite = player.playerImage;
		UpdateLives ();
	}
	

	void Update () 
	{
		enemyTimer += Time.deltaTime;
		if(enemyTimer > enemyUITime )
		{
			enemyTimer = 0;
			enemyUI.SetActive(false);
		}

		UpdateHealthBuffer();
	}

	public void UpdateHealth(int amount)
	{
		healthUI.value = amount;
	}

	void UpdateHealthBuffer()
	{
		healthBufferUI.value = Mathf.Lerp(healthBufferUI.value,healthUI.value,Time.deltaTime * 1f);

		if(enemyBufferSlider.value >= enemySlider.value)
		{
			enemyBufferSlider.value = Mathf.Lerp(enemyBufferSlider.value,enemySlider.value,Time.deltaTime * 1f);
		}
		else if(enemyBufferSlider.value < enemySlider.value)
		{
			enemyBufferSlider.value = enemySlider.value;
		}
	}

	public void UpdateEnemyUI(int maxHealth, int currentHealth,string name,Sprite image)
	{
		enemySlider.maxValue = maxHealth;
		enemySlider.value = currentHealth;
		enemyBufferSlider.maxValue = maxHealth;

		enemyName.text = name;
		enemyImage.sprite = image;
		enemyTimer = 0;
		enemyUI.SetActive (true);
	}

	public void UpdateLives()
	{
		livesText.text = "x " + FindObjectOfType<GameManager> ().lives.ToString ();
	}

	public void UpdateDisplayMessage(string message)
	{
		displayMessage.text = message;
	}
}
