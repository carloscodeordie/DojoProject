using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
	public int damage;
	public float punchForce;
	public GameObject hitEffect;
	public GameObject hitEffectPos;

	public Animator anim;

	void Start () 
	{
        
    }
	

	void Update () 
	{
		
	}

	private void OnTriggerEnter(Collider other)
	{
		Enemy enemy = other.GetComponent<Enemy> ();
		Player player = other.GetComponent<Player> ();
		CarTookDamage car = other.GetComponent<CarTookDamage>();
		if (enemy != null) 
		{
			GameObject.FindObjectOfType<HitStopContrl>().Stop(0.1f);

			if (this.name != "Weapon") 
			{
				// anim.SetBool ("Combo", true);
			}
		
			enemy.TookDamage(damage);
			Instantiate(hitEffect,hitEffectPos.transform.position,Quaternion.identity);
		}

		if (player != null) 
		{
			player.TookDamage(damage);
			if (transform.position.x - player.transform.position.x > 0) 
			{
				if (!player.facingRight) {
					player.Flip ();
				}
			} 
			else 
			{
				if (player.facingRight) {
					player.Flip ();
				}
			}
		}

		if (car != null) 
		{
			anim.SetBool ("Combo", true);
			car.TookDamage(damage);
			Instantiate(hitEffect,hitEffectPos.transform.position,Quaternion.identity);
		}
	}	
}
