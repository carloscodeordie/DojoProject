using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableWeapon : MonoBehaviour {

	public WeaponItem weapon;

	private SpriteRenderer sprite;

	// Use this for initialization
	void Start () 
	{
		sprite = GetComponent<SpriteRenderer>();
		sprite.sprite = weapon.sprite;
		sprite.color = weapon.color;
	}
	

}
