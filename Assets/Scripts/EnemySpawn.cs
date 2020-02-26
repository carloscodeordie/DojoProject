using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour 
{
    /**
     * Spawn configuration 
     * */
    [SerializeField] public float maxZ;
    [SerializeField] public float minZ;
    [SerializeField] public GameObject[] enemies;
    [SerializeField] public int numberOfEnemies;
    [SerializeField] public float spawnTime;

    /**
     * State variables 
     * */
    private int currentEnemies;

	/**
     * Update method
     * */
	void Update () 
	{
        // Validates if there are enemies on the spawn, if there are no enemies, the EnemySpawn is disabled and the camera is active
		if(currentEnemies >= numberOfEnemies)
		{
			int enemies = FindObjectsOfType<Enemy>().Length;
			if(enemies <= 0)
			{
				FindObjectOfType<ResetCameraScript>().Active();
				gameObject.SetActive(false);
			}
		}
	}

    /**
     * Method declaration
     * */
    void SpawnEnemy()
	{
		bool positionX = Random.Range (0, 2) == 0 ? true : false;
		Vector3 spawnPosition;
		spawnPosition.z = Random.Range (minZ,maxZ);
		if(positionX)
		{
			spawnPosition = new Vector3(transform.position.x+10,-8,spawnPosition.z);
		}
		else
		{
			spawnPosition = new Vector3(transform.position.x-10,-8,spawnPosition.z);
		}
		Instantiate (enemies[Random.Range(0, enemies.Length)],spawnPosition,Quaternion.identity);
		currentEnemies++;
		if(currentEnemies < numberOfEnemies)
		{
			Invoke ("SpawnEnemy",spawnTime);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			GetComponent<BoxCollider>().enabled = false;
			FindObjectOfType<CameraFollow>().maxXAndY.x = transform.position.x;
			SpawnEnemy();
		}
	}
}
