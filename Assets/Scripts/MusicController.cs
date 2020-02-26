using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour 
{
    /**
     * Setters and Getters
     * */
    public static MusicController instance{
        get;
        set;
    }

    /**
     * Music clips
     * */
    [SerializeField] public AudioClip levelSong;
    [SerializeField] public AudioClip bossSong;
    [SerializeField] public AudioClip levelClearSong;

    /**
     * State variables
     * */
    private AudioSource audioSource;

    /**
     * Methods declaration
     * */
    void Awake()
	{
		instance = this;
	}

	void Start () 
	{
        audioSource = GetComponent<AudioSource> ();
		PlaySong (levelSong);
	}

	public void PlaySong(AudioClip clip)
	{
        audioSource.clip = clip;
        audioSource.Play ();
	}
}
