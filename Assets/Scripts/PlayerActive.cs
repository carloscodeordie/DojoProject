using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActive : MonoBehaviour 
{
	public GameObject Player1,Player2;
	
	// Use this for initialization
	void Awake ()
	{
		if (FindObjectOfType<GameManager> ().characterIndex == 1) 
		{
			Player1.SetActive (true);
			Player2.SetActive (false);
		}
		else if(FindObjectOfType<GameManager> ().characterIndex == 2) 
		{
			Player2.SetActive (true);
			Player1.SetActive (false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
