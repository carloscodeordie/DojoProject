using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour 
{
	public static LevelLoader levelLoader;

	public GameObject loadingScreen;
	public Slider slider;
	public Text progressText;


	void Awake()
	{
		levelLoader = this;
	}

    public void LoadCredits()
    {
        StartCoroutine(LoadAsynchronously(0, false));
    }

    public void LoadStart()
    {
        StartCoroutine(LoadAsynchronously(3, false));
    }

    public void LoadLevel(int sceneIndex)
    {
        bool isLoader = false;

        string currentSceneName = SceneManager.GetActiveScene().name;
        switch (currentSceneName)
        {
            case "History":
                isLoader = true;
                break;
        }
        if (isLoader) {
            loadingScreen.SetActive(true);
        }

        StartCoroutine (LoadAsynchronously(sceneIndex, isLoader));
	}

	IEnumerator LoadAsynchronously(int sceneIndex, bool isLoaderDisplayed)
	{

		AsyncOperation operation = SceneManager.LoadSceneAsync (sceneIndex);

        if (isLoaderDisplayed)
        {
            while (operation.isDone == false)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);

                slider.value = progress;
                progressText.text = progress * 100f + "%";
                yield return null;
            }
        }
        yield return null;
    }
}
