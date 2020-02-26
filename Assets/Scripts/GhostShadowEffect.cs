using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostShadowEffect : MonoBehaviour 
{
	SpriteRenderer sprite;
	float timer = 0.2f;



	void Start () 
	{
		sprite = GetComponent<SpriteRenderer>();

		transform.position = MoonWalker.instance.transform.position;
		transform.localScale = MoonWalker.instance.transform.localScale;

		sprite.sprite = MoonWalker.instance.GetComponent<SpriteRenderer> ().sprite;
		sprite.color = new Vector4 (50,50,50,0.2f);
	}
	

	
	void Update () 
	{
		timer -= Time.deltaTime;

		if(timer <= 0)
		{
			Destroy (gameObject);
		}
	}
}
