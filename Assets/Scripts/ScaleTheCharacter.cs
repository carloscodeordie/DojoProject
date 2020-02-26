using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTheCharacter : MonoBehaviour
{	
	public float distanceToCam;
	float scaleX,scaleY;
	Player player;
	Enemy enemy;
	Attack attack;

	void Start()
	{
		scaleX = transform.localScale.x;
		scaleY = transform.localScale.y;
		player = GetComponent<Player>();
		enemy = GetComponent<Enemy>();
		attack = GetComponent<Attack>();
	}
	void Update () 
	{
		distanceToCam = Mathf.Abs(this.transform.position.z-Camera.main.transform.position.z);
		if(player != null)
		{
			if(player.facingRight)
			{
				transform.localScale =  new Vector3(Mathf.Clamp(scaleX * Mathf.Sign(transform.localScale.x) / (distanceToCam/10),scaleX-0.5f,scaleX+1.5f), 
					Mathf.Clamp(scaleY /  (distanceToCam/10),scaleY-0.5f,scaleY+1.5f),
					transform.localScale.z);
			}
			else
			{
				transform.localScale =  new Vector3(Mathf.Clamp(scaleX * Mathf.Sign(transform.localScale.x) / (distanceToCam/10),-scaleX-1.5f,0.5f-scaleX), 
					Mathf.Clamp(scaleY /  (distanceToCam/10),scaleY-0.5f,scaleY+1.5f),
					transform.localScale.z);
			}
		}

		if(enemy != null)
		{
			transform.localScale =  new Vector3(Mathf.Clamp(scaleX * Mathf.Sign(transform.localScale.x) / (distanceToCam/10),scaleX-0.5f,scaleX+1.5f), 
				Mathf.Clamp(scaleY /  (distanceToCam/10),scaleY-0.5f,scaleY+1.5f),
			transform.localScale.z);
		}

		if(attack != null)
		{
			transform.localScale =  new Vector3(Mathf.Clamp(scaleX * Mathf.Sign(transform.localScale.x) / (distanceToCam/10),scaleX-0.5f,scaleX+1.5f), 
				Mathf.Clamp(scaleY /  (distanceToCam/10),scaleY-0.5f,scaleY+1.5f),
				transform.localScale.z);
		}
		//Debug.Log(distanceToCam/13);
	}
}
