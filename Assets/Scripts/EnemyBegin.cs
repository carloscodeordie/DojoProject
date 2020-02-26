using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBegin : MonoBehaviour {

	Transform target;
	public GameObject enemy;
	public float dis = 4f;
	// Use this for initialization
	void Start () 
	{
		target = FindObjectOfType<Player> ().transform; 
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Vector3.Distance(target.position,transform.position)<=dis)
		{
			this.gameObject.SetActive(false);
			enemy.SetActive(true);
		}
	}
}
