using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour 
{
	private SpriteRenderer spriteRenderer;
	
	private Color color;
	private int durability;

	// Use this for initialization
	void Start () 
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	public void ActivateWeapon(Sprite sprite, Color color, int durabilityValue, int damage)
	{
		spriteRenderer.sprite = sprite;
		spriteRenderer.color = color;
		durability = durabilityValue;
		GetComponent<Attack> ().damage = damage;
	}

	private void OnTriggerEnter(Collider other)
	{
		Enemy enemy = other.GetComponent<Enemy> ();
		if(enemy != null)
		{
			other.gameObject.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(5f,1,0),ForceMode.Impulse);
			durability--;
			if(durability <= 0)
			{
				spriteRenderer.sprite = null;
				GetComponentInParent<Player>().SetHoldingWeaponToFalse();
			}
		}
	}
}
