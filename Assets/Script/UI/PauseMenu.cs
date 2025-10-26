using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseScreen;
    public float animationSpeed = 10f;

    private bool isPaused = false;
    private Vector3 targetScale;

    private AudioSource themeSong;

    [Header("Loading Screen")]
    public GameObject loadingScreen;
    public float loadingDelay = 2f;

    void Start()
    {
        pauseScreen.transform.localScale = Vector3.zero;
        targetScale = Vector3.zero;
        Time.timeScale = 1f;

        GameObject themeObj = GameObject.Find("Theme Song");
        if (themeObj != null)
        {
            themeSong = themeObj.GetComponent<AudioSource>();
            if (themeSong != null)
            {
                themeSong.loop = true;
                themeSong.Play();
            }
        }
        else
        {
            Debug.LogWarning("Theme Song object not found in scene!");
        }
    }

    void Update()
    {
        pauseScreen.transform.localScale = Vector3.Lerp(
            pauseScreen.transform.localScale,
            targetScale,
            Time.unscaledDeltaTime * animationSpeed
        );
    }

    public void PauseGame()
    {
        if (!isPaused)
        {
            Time.timeScale = 0f;
            targetScale = Vector3.one;
            isPaused = true;

            if (themeSong != null)
                themeSong.Pause();

            GameObject openPauseObj = GameObject.Find("Open Pause Menu");
            if (openPauseObj != null)
            {
                AudioSource sfx = openPauseObj.GetComponent<AudioSource>();
                if (sfx != null)
                    sfx.Play();
            }
            else
            {
                Debug.LogWarning("Open Pause Menu object not found!");
            }
        }
        else
        {
            Time.timeScale = 1f;
            targetScale = Vector3.zero;
            isPaused = false;

            if (themeSong != null)
                themeSong.UnPause();
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        targetScale = Vector3.zero;
        isPaused = false;

        if (themeSong != null)
            themeSong.UnPause();
    }

    public void BackToMainMenu()
    {
        StartCoroutine(LoadGameAsync("MainMenu"));
    }

    private IEnumerator LoadGameAsync(string sceneName)
    {
        if (loadingScreen != null)
            loadingScreen.SetActive(true);

        Time.timeScale = 1f;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        yield return new WaitForSecondsRealtime(loadingDelay);
        operation.allowSceneActivation = true;
    }
}
