using UnityEngine;

public class NPCSounds : MonoBehaviour
{
    public AudioClip[] sounds;
    public AudioSource audioSource;

    private void Start()
    {
        if (!audioSource) audioSource = GetComponent<AudioSource>();
        InvokeRepeating("PlayNPCSound", Random.Range(0,10), Random.Range(5,10));
    }
    public void PlayNPCSound()
    {
        audioSource.PlayOneShot(sounds[Random.Range(0,sounds.Length)]);
    }
}
