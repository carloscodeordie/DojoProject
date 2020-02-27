using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePointLights : MonoBehaviour {

	public float rotateSpeed = 2f;
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Rotate(0f,rotateSpeed*Time.deltaTime,0f,Space.Self);
	}
}
