using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{

	public int lives;
	public int characterIndex;
	public static GameManager gameManager;

	void Awake () 
	{
		if(gameManager == null)
		{
			gameManager = this;
		}
		else
		{
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
	}
	

	void Update () 
	{
		
	}
}
