using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [Header("Loading Screen")] 
    public GameObject loadingAnim;
    public GameObject loadingScreen;
    public float loadingDelay = 2f;

    [Header("SFX")]
    public AudioSource clickBTN;
    public AudioSource backBTN;
    public AudioSource themeSong;

    void Start()
    {
        if (loadingAnim.activeInHierarchy == false)loadingAnim.SetActive(true);
    }

    public void StartGame()
    {
        if(clickBTN != null) clickBTN.Play();
        
        StartCoroutine(LoadGameAsync("Game"));
        
        if(clickBTN != null) themeSong.Stop();
    }

    public void QuitGame()
    {
        if(backBTN != null) backBTN.Play();
        StartCoroutine(QuitGameWithLoading());
        if(clickBTN != null) themeSong.Stop();
    }

    private IEnumerator LoadGameAsync(string sceneName)
    {
        if(loadingAnim!=null) 
            loadingAnim.GetComponent<Animator>().SetTrigger("LoadingOut");
        
        yield return new WaitForSeconds(1f);
        
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
        if(loadingAnim!=null) 
            loadingAnim.GetComponent<Animator>().SetTrigger("LoadingOut");
        
        yield return new WaitForSeconds(1f);

        Application.Quit();
    }
}

