using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [Header("Loading Screen")]
    public GameObject loadingScreen;
    public float loadingDelay = 2f;

    public void StartGame()
    {
        StartCoroutine(LoadGameAsync("Game"));
    }

    public void QuitGame()
    {
        QuitGameWithLoading();
    }

    private IEnumerator LoadGameAsync(string sceneName)
    {
        if (loadingScreen != null)
            loadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        operation.allowSceneActivation = true;
    }

    private IEnumerator QuitGameWithLoading()
    {
        if (loadingScreen != null)
            loadingScreen.SetActive(true);

        yield return new WaitForSeconds(loadingDelay);

        Application.Quit();
    }
}

