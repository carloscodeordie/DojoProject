using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundFollow : MonoBehaviour 
{
    /**
     * Camera Controls 
     * */
	[SerializeField] Transform camera;
    [SerializeField] Vector3 offset;
    [SerializeField] bool parallax;
    [SerializeField] bool on_Y;
    [SerializeField] float speed;
    [SerializeField] List<SpriteRenderer> backGroundRenderersList;

    /**
     * State variables
     * */
    private Vector3 defaultPosition;
    private float xParadase;
    private float yParadase;
    private float camVelocityX;
    private float camVelocityY;
    private float previousPosX;
    private float previousPosY;
    private float curentPosX;
    private float currentPosY;
    private bool canFollow;
    private SpriteRenderer[] backGroundRenderersArray;

    /*
     * Initialization method
     * */
	void Start()
	{
        canFollow = false;

        // Initialize background images list
        backGroundRenderersList = new List<SpriteRenderer>();

        // Fetch all child images
        backGroundRenderersArray = GetComponentsInChildren<SpriteRenderer> ();
		foreach(SpriteRenderer backgroundRenderer in backGroundRenderersArray)
		{
            // Add umages into background images list
            backGroundRenderersList.Add(backgroundRenderer);
		}
	}

    /**
     * Enable Camera
     * */
	void OnEnable() 
	{
		defaultPosition = transform.position;

		StartCoroutine (CaculateCameraSpeedX(0f));
		StartCoroutine (CaculateCameraSpeedY(0f));
	}

    /**
     * Disable Camera
     * */
    void OnDisable()
	{
		StopCoroutine (CaculateCameraSpeedX(0f));
		StopCoroutine (CaculateCameraSpeedY (0f));
	}

    /**
     * Update on each frame the camera
     * */
	void Update () 
	{

		camVelocityX = Mathf.Clamp (camVelocityX, -0.1f, 0.1f);
		camVelocityY = Mathf.Clamp (camVelocityY, -0.1f, 0.1f);
		yParadase = Mathf.Clamp (yParadase, -1f, 1f);

		if (Time.timeScale > 0.001f ) 
		{
			xParadase -= speed * camVelocityX ;
			yParadase -= speed * camVelocityY ;
			Invoke ("CanFollow",0.05f);

			if(parallax)
			{
				if(on_Y)
				{
					if(Mathf.Abs (camVelocityY)<0.19f && Mathf.Abs (camVelocityX)<0.19f)
					{
						transform.position = new Vector3 (camera.position.x + xParadase , camera.position.y + yParadase, transform.position.z) + offset ;
					}
				}
				else
				{
					if(Mathf.Abs (camVelocityX)<0.19f)
					{
						transform.position = new Vector3 (camera.position.x + xParadase, transform.position.y, transform.position.z) + offset ;
					}
				}
			}
			else
			{
				if(on_Y)
				{
					transform.position = new Vector3 (transform.position.x, camera.position.y, transform.position.z) + offset ;
				}
				else
				{
					transform.position = new Vector3 (camera.position.x, transform.position.y, transform.position.z) + offset ;
				}
			}

			foreach(SpriteRenderer backgrounds in backGroundRenderersList)
			{
				backgrounds.enabled = true;
			}
		}
	}

	IEnumerator CaculateCameraSpeedX(float waitTime)
	{
		while (true) 
		{
			previousPosX = camera.transform.position.x;
			yield return new WaitForSeconds (0.01f);
			curentPosX = camera.transform.position.x;

			if(canFollow)
			{
				camVelocityX = curentPosX - previousPosX;
			}

		}
	}

	IEnumerator CaculateCameraSpeedY(float waitTime)
	{
		while (true) 
		{
			previousPosY = camera.transform.position.y;
			yield return new WaitForSeconds (0.0000000001f);
			currentPosY = camera.transform.position.y;
			
			if(canFollow)
			{
				camVelocityY = currentPosY - previousPosY;
			}
			
		}
	}

	void CanFollow()
	{
		canFollow = true;
	}

}
