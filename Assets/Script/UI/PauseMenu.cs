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

    private AudioSource resumeBTN;
    private AudioSource quitBTN;
    private AudioSource ambienceSFX;

    [Header("Loading Screen")] 
    public GameObject loadingAnim;
    public GameObject loadingScreen;
    public float loadingDelay = 2f;

    void Start()
    {
        pauseScreen.transform.localScale = Vector3.zero;
        targetScale = Vector3.zero;
        Time.timeScale = 1f;
        
        resumeBTN = GameObject.Find("Click Button").GetComponent<AudioSource>();
        quitBTN = GameObject.Find("Back Button").GetComponent<AudioSource>();
        ambienceSFX = GameObject.Find("SFX Nature Ambience").GetComponent<AudioSource>();
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
        if(resumeBTN!=null) resumeBTN.Play();
        if (!isPaused)
        {
            Time.timeScale = 0f;
            targetScale = Vector3.one;
            isPaused = true;

            if (ambienceSFX != null)
                ambienceSFX.Pause();

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

            if (ambienceSFX != null)
                ambienceSFX.UnPause();
        }
    }

    public void ResumeGame()
    {
        if(resumeBTN!=null) resumeBTN.Play();
        Time.timeScale = 1f;
        targetScale = Vector3.zero;
        isPaused = false;

        if (ambienceSFX != null)
            ambienceSFX.UnPause();
    }

    public void BackToMainMenu()
    {
        if(resumeBTN!=null) quitBTN.Play();
        
        Time.timeScale = 1f;
        if (loadingAnim != null)
            loadingAnim.GetComponent<Animator>().SetTrigger("LoadingOut");
        
        StartCoroutine(LoadGameAsync("MainMenu"));
    }

    private IEnumerator LoadGameAsync(string sceneName)
    {
        yield return new WaitForSecondsRealtime(1f);
        
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
