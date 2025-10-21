using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class CatnipManager : MonoBehaviour
{
    public static CatnipManager Instance;

    [Header("Currency Settings")]
    public int startingCatnip = 99999; // FOR TESTING
    private int currentCatnip;

    public TextMeshProUGUI catnipText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
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
}
