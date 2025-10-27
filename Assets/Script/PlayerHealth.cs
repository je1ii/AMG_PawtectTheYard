using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.VisualScripting;


public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;
    
    public float currentHealth;

    public float defaultHealth = 10f;

    public bool isFullHealth;
    public bool isHalfHealth;
    public bool canDie;
    
    
    public AudioSource scream1;
    public AudioSource scream2;
    public AudioSource hit;

    private CameraDamageEffect cde;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        cde = this.gameObject.GetComponent<CameraDamageEffect>();
        currentHealth = defaultHealth;
        
        isFullHealth = true;
        isHalfHealth = false;
        canDie = false;
    }

    public void DamagePlayer(float damage)
    {
        cde.OnHit();
        hit.Play();
        currentHealth -= damage;

        if (currentHealth <= 0 && canDie)
        {
            StartCoroutine(PlayerDied());
        }
        else if (currentHealth <= 2 && isHalfHealth)
        {
            scream2.Play();
            Debug.Log("Player reached low health: " + currentHealth);
            isHalfHealth = false;
            canDie = true;
        }
        else if (currentHealth <= 6 && isFullHealth)
        {
            scream2.Play();
            Debug.Log("Player reached half health: " + currentHealth);
            isFullHealth = false;
            isHalfHealth = true;
        }
    }
    
    private IEnumerator PlayerDied()
    {
        // show game over menu
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Game Over");
    }
}
