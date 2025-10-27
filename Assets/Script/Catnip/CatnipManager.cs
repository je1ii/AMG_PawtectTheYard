using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class CatnipManager : MonoBehaviour
{
    public static CatnipManager Instance;

    [Header("Currency Settings")]
    public int startingCatnip = 99999; // FOR TESTING
    public float textShakeAmount = 5f;
    private int currentCatnip;
    private Vector2 textOrigPos;
    private bool isShaking = false;

    public TextMeshProUGUI catnipText;
    
    private AudioSource deniedSFX;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        deniedSFX = GameObject.Find("Denied")?.GetComponent<AudioSource>();
        if (deniedSFX != null) deniedSFX.volume = Mathf.Min(deniedSFX.volume * 10, 1f);
        
        textOrigPos = catnipText.transform.position;
        
        currentCatnip = startingCatnip;
        UpdateCatnipUI();
    }

    public void AddCatnip(int amount)
    {
        currentCatnip += amount;
        UpdateCatnipUI();
    }

    public bool SpendCatnip(int amount)
    {
        if (currentCatnip >= amount)
        {
            currentCatnip -= amount;
            UpdateCatnipUI();
            return true;
        }
        else
        {
            Debug.Log("Not enough catnip!");
            if (!isShaking) StartCoroutine(ShakeText());
            if (deniedSFX != null) deniedSFX.Play();
            return false;
        }
    }

    private void UpdateCatnipUI()
    {
        if (catnipText != null)
            catnipText.text = currentCatnip.ToString();
    }

    public int GetGold()
    {
        return currentCatnip;
    }

    private IEnumerator ShakeText()
    {
        isShaking = true;
        
        var elapsed = 0f;
        while (elapsed < 0.5f)
        {
            float offsetX = Random.Range(-1f, 1f) * textShakeAmount * 0.1f;
            float offsetY = Random.Range(-1f, 1f) * textShakeAmount * 0.1f;
            catnipText.transform.position = textOrigPos + new Vector2(offsetX, offsetY);
            
            elapsed += Time.deltaTime;
            yield return null;
        }

        catnipText.transform.position = textOrigPos;
        isShaking = false;
    }
}
