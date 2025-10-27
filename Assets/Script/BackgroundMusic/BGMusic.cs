using UnityEngine;

public class BGMusic : MonoBehaviour
{
    void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No AudioSource found on " + gameObject.name);
        }
    }
}
