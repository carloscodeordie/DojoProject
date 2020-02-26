using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour 
{
    /**
     * Players 
     * */
    [SerializeField] GameObject[] player;

	// Initialization Awake
	void Awake () 
	{
        // Change this line when game is created
        //int index = FindObjectOfType<GameManager> ().characterIndex - 1;
        int index = 0;
        Instantiate (player[index], transform.position, transform.rotation);
	}
}
