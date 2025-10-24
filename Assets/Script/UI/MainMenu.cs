using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [Header("Loading Screen")]
    public GameObject loadingScreen; // Assign your animated loading screen here

    public void StartGame()
    {
        StartCoroutine(LoadGameAsync("Game"));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator LoadGameAsync(string sceneName)
    {
        // Show your animated loading screen
        if (loadingScreen != null)
            loadingScreen.SetActive(true);

        // Begin loading the scene asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        // Wait until the scene is ready (progress reaches 0.9)
        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        // Optional: wait a short time to let animation play if needed
        yield return new WaitForSeconds(2f);

        // Activate the new scene
        operation.allowSceneActivation = true;
    }
}

