using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseScreen;
    public float animationSpeed = 10f;

    private bool isPaused = false;
    private Vector3 targetScale;

    void Start()
    {
        pauseScreen.transform.localScale = Vector3.zero;
        targetScale = Vector3.zero;
        Time.timeScale = 1f;
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
        }
        else
        {
            Time.timeScale = 1f;
            targetScale = Vector3.zero;
            isPaused = false;
        }
        
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        targetScale = Vector3.zero;
        isPaused = false;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
