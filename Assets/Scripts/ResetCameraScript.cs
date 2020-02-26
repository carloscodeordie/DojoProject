using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetCameraScript : MonoBehaviour {

	public void Active()
  	{
		GetComponent<Animator> ().SetTrigger ("Go");
	}

	void ResetCamera()
	{
		FindObjectOfType<CameraFollow>().maxXAndY.x = 120f;
	}
}
