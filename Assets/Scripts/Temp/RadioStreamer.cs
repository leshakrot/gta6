using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class RadioStreamer : MonoBehaviour
{
    public string radioUrl = "https://stream.overdrivelive.net:8000//alternative_128.mp3";
    private AudioSource audioSource;
    private AudioClip audioClip;
    private bool isPlaying = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayRadio());
    }

    IEnumerator PlayRadio()
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(radioUrl, AudioType.UNKNOWN))
        {
            www.SendWebRequest();

            while (!www.isDone)
            {
                yield return null;
            }

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error loading radio stream: " + www.error);
            }
            else
            {
                audioClip = DownloadHandlerAudioClip.GetContent(www);

                if (audioClip != null)
                {
                    audioSource.clip = audioClip;
                    audioSource.Play();
                    isPlaying = true;
                    Debug.Log("Radio stream playing successfully.");
                }
                else
                {
                    Debug.LogError("Failed to load AudioClip from radio stream.");
                }
            }
        }
    }

    void Update()
    {
        if (isPlaying && !audioSource.isPlaying)
        {
            StartCoroutine(PlayRadio());
        }
    }
}
