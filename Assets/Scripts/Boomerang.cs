using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour 
{
	public int direction = 1;

	private Rigidbody rb;


	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
		StartCoroutine (MoveBoomerang());//不是Invoke 因为要用轶代重复执行
	}
	

	void FixedUpdate () 
	{
		rb.velocity = new Vector3 (6 * direction, 0, 0 * direction);
	}

	IEnumerator MoveBoomerang()
	{
		yield return new WaitForSeconds (2f);
		direction *= -1;
	}
}
