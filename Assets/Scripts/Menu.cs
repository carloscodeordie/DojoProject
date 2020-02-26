using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {


	
	// Update is called once per frame
	void Update () 
	{
		if(Input.anyKeyDown) 
		{
			LoadScene();
		}
	}

	void LoadScene()
	{
        //SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
        LevelLoader.levelLoader.LoadLevel (SceneManager.GetActiveScene().buildIndex + 1);
	}
}
