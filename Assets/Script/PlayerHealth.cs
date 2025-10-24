using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    public float defaultHealth = 10f;
    
    void Start()
    {
        maxHealth = defaultHealth;
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (currentHealth != null)
        {
            if (currentHealth <= 5)
            {
                // play sfx indication of half health
            }
            else if (currentHealth == 10)
            {
                
            }
        }
    }
}
