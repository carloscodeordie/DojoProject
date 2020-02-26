using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectMenu : MonoBehaviour
{
    [SerializeField] Image asukaImage, sakuraImage, misakiImage;
    [SerializeField] Image[] characterImages = new Image[3];

    private Color defaultColor;
    private int characterIndex;
    private AudioSource audioSource;

    void Start()
    {
        characterIndex = 0;
        defaultColor = characterImages[0].color;
        audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            characterIndex--;
            if (characterIndex < 0)
            {
                characterIndex = characterImages.Length - 1;
            }
            PlaySound();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            characterIndex++;
            if (characterIndex >= characterImages.Length)
            {
                characterIndex = 0;
            }
            PlaySound();
        }

        for (int index = 0; index < characterImages.Length; index++) {
            if (characterIndex == index)
            {
                characterImages[index].color = Color.yellow;
            }
            else
            {
                characterImages[index].color = defaultColor;
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            FindObjectOfType<GameManager>().characterIndex = characterIndex;
            Confirm();
        }
    }

    void PlaySound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void SelectCharacter(int index)
    {
        characterIndex = index;
        PlaySound();
    }

    public void Confirm()
	{
		FindObjectOfType<GameManager>().characterIndex = characterIndex;
		LevelLoader.levelLoader.LoadLevel (SceneManager.GetActiveScene().buildIndex + 1);
	}
}
