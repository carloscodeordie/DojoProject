using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryManager : MonoBehaviour
{
	private BoxCollider2D managerBox;
	private Transform player;
	public GameObject boundary;
	public bool isOut;

	//public EnemyControllByBoundary[] enemies;

	void Start ()
	{
		managerBox = GetComponent<BoxCollider2D> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform> ();

		//if (enemies != null) 
		//{
			//foreach(EnemyControllByBoundary enemy in enemies)
			//{
				//enemy.boundaryManager = this;
			//}

		//}
	}
	

	void Update ()
	{
		ManageBoundary ();
	}

	void ManageBoundary()
	{
		if(managerBox.bounds.min.x < player.position.x && player.position.x < managerBox.bounds.max.x&&
			managerBox.bounds.min.y < player.position.y && player.position.y < managerBox.bounds.max.y)
		{
			boundary.SetActive (true);
			isOut = false;
		}
		else
		{
			boundary.SetActive (false);
			isOut = true;

			//foreach(EnemyControllByBoundary enemy in enemies)
			//{
				//enemy.gameObject.SetActive(true);
			//}
		}
	}
}
