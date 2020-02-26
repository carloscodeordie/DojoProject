using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTookDamage : MonoBehaviour
{
	public int MaxHealth;
	public Animator carBodyAnim;
	public Animator carBottomAnim;
	public AudioClip collisionSound;
	public GameObject[] carPart;
	public Transform explosionPos;
	public Transform explosionPos1;
	public GameObject explosionEffect;
	int explosionCount;
	
	private int currentHealth;
	private bool isDead;
	private AudioSource AudioS;
	private BoxCollider playerAttack;

	void Start()
	{
		currentHealth = MaxHealth;
		AudioS = GetComponent<AudioSource> ();
		playerAttack = GameObject.FindGameObjectWithTag ("Player").transform.Find ("Attack").GetComponent<BoxCollider> ();
	}

	void Update()
	{
		carBodyAnim.SetInteger ("Health", currentHealth);
		carBottomAnim.SetInteger ("Health", currentHealth);
	}
	public void TookDamage(int damage)
	{
		if (!isDead)
		{
			currentHealth -= damage;
			carBodyAnim.SetTrigger("Damage");
			carBottomAnim.SetTrigger("Damage");
			PlaySong(collisionSound);
		}
		if (currentHealth <= 23) 
		{
			carPart[0].SetActive(true);
			carPart[0].GetComponent<Rigidbody>().AddExplosionForce(500f,explosionPos.transform.position+new Vector3(0,1,0),55f);
		}
		if (currentHealth <= 18)
		{
			carPart[1].SetActive(true);
			carPart[1].GetComponent<Rigidbody>().AddExplosionForce(500f,explosionPos.transform.position+new Vector3(0,1,0),55f);
		}
		if (currentHealth <= 14)
		{
			carPart[2].SetActive(true);
			carPart[2].GetComponent<Rigidbody>().AddExplosionForce(500f,explosionPos.transform.position+new Vector3(0,1,0),55f);
		}
		if (currentHealth <= 8)
		{
			carPart[3].SetActive(true);
			carPart[3].GetComponent<Rigidbody>().AddExplosionForce(500f,explosionPos.transform.position+new Vector3(0,1,0),55f);
		}
		if (currentHealth <= 4)
		{
			carPart[4].SetActive(true);
			carPart[4].GetComponent<Rigidbody>().AddExplosionForce(500f,explosionPos.transform.position+new Vector3(0,1,0),55f);
		}
		if (currentHealth <= 0) 
		{
			carPart[4].SetActive(true);
			carPart[4].GetComponent<Rigidbody>().AddExplosionForce(500f,explosionPos.transform.position+new Vector3(0,1,0),55f);
			isDead = true;
			Physics.IgnoreCollision(playerAttack,this.GetComponent<BoxCollider>());
			if(explosionCount>=0)
			{
				GameObject exFX = Instantiate(explosionEffect,explosionPos1.position,explosionPos1.rotation);
				explosionCount --;
			}
			Destroy(this.gameObject,0.01f);
			GameManager.gameManager.lives += 1;
		}
	}

	private void PlaySong(AudioClip collisionSound)
	{
		AudioS.clip = collisionSound; 
		AudioS.Play ();
	}
}
