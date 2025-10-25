using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class TextController : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI continueText;

    [Header("Loading Screen")]
    public GameObject loadingScreen;
    public float loadingDelay = 2f;

    [Header("Settings")]
    public float typingSpeed = 0.02f;

    [TextArea(3, 10)]
    public string[] sentences;

    private int index = 0;
    private bool isTyping = false;

    void Start()
    {
        dialogueText.text = "";
        continueText.text = "";
        StartCoroutine(PlayScene());
    }

    void Update()
    {
        // Left mouse click to continue
        if (Input.GetMouseButtonDown(0) && !isTyping)
        {
            NextSentence();
        }
    }

    private IEnumerator PlayScene()
    {
        isTyping = true;
        yield return new WaitForSeconds(2f);
        StartCoroutine(TypeSentence());
    }

    IEnumerator TypeSentence()
    {
        isTyping = true;
        continueText.text = ""; // hide during typing
        dialogueText.text = "";

        foreach (char letter in sentences[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        continueText.text = "click to continue..."; // show after done typing
    }

    void NextSentence()
    {
        if (index < sentences.Length - 1)
        {
            index++;
            StartCoroutine(TypeSentence());
        }
        else
        {
            dialogueText.text = "";
            continueText.text = "";
            Debug.Log("Dialogue ended!");
            
            //loading scene before going back to menu
            StartCoroutine(LoadMainMenuWithScreen());
        }
    }

    private IEnumerator LoadMainMenuWithScreen()
    {
        if (loadingScreen != null)
            loadingScreen.SetActive(true);

        yield return new WaitForSeconds(loadingDelay);

        SceneManager.LoadScene("MainMenu");
    }
}
