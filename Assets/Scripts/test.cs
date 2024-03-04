using UnityEngine;

public class test : MonoBehaviour
{
    public AudioClip playerDeathSound;
    public AudioSource audioSource;

    private void Start()
    {
        if (!audioSource) audioSource = GetComponent<AudioSource>();
    }
    public void PlayPlayerDeathSound()
    {
        audioSource.PlayOneShot(playerDeathSound);
    }
}
