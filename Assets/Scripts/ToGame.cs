using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToGame : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		Invoke("ToMainGame",4f);	
	}
	
	// Update is called once per frame
	void ToMainGame () 
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex+1);
	}
}
