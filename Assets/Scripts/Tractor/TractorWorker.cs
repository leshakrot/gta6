using System;
using UnityEngine;

public class TractorWorker : MonoBehaviour
{
    public static Action onWorkStarted;
    public static Action onCheckpointPassed;
    public static Action onWorkStop;

    [SerializeField] GameObject _directionArrow;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _checkpointSfx;
    [SerializeField] Transform _getOutPos;

    private void OnEnable()
    {
        onWorkStarted += ShowDirectionArrow;
        onCheckpointPassed += PassCheckpoint;
        onWorkStop += HideDirectionArrow;
    }

    private void OnDisable()
    {
        onWorkStarted -= ShowDirectionArrow;
        onCheckpointPassed -= PassCheckpoint;
        onWorkStop -= HideDirectionArrow;
    }

    public void PassCheckpoint()
    {
        PlayerBank.instance.AddMoney(15);
        PlayerLevel.instance.AddExp(10);
        PlayCheckpointPassSound();
    }

    public void StartWork()
    {
        onWorkStarted?.Invoke();
    }

    public void StopWork()
    {
        onWorkStop?.Invoke();
    }

    private void ShowDirectionArrow()
    {
        _directionArrow.SetActive(true);
    }

    private void HideDirectionArrow()
    {
        _directionArrow.SetActive(false);
    }

    private void PlayCheckpointPassSound()
    {
        _audioSource.PlayOneShot(_checkpointSfx);
    }
}
