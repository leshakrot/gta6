using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusWorker : MonoBehaviour
{
    public static Action onWorkStarted;
    public static Action onCheckpointPassed;
    public static Action onStopAtBusStop;
    public static Action onBusStopPassed;
    public static Action onWorkStop;

    [SerializeField] private List<Passenger> _passengers = new List<Passenger>();

    [SerializeField] GameObject _directionArrow;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _checkpointSfx;
    [SerializeField] Transform _getOutPos;

    private int _passengersCount = 0;

    private void OnEnable()
    {
        onWorkStarted += ShowDirectionArrow;
        onCheckpointPassed += PassCheckpoint;
        onStopAtBusStop += StopAtBusStop;
        onStopAtBusStop += RemovePassenger;
        onBusStopPassed += PassBusStopCheckpoint;
        onWorkStop += HideDirectionArrow;
    }

    private void OnDisable()
    {
        onWorkStarted -= ShowDirectionArrow;
        onCheckpointPassed -= PassCheckpoint;
        onStopAtBusStop -= StopAtBusStop;
        onStopAtBusStop -= RemovePassenger;
        onBusStopPassed -= PassBusStopCheckpoint;
        onWorkStop -= HideDirectionArrow;
    }

    public void PassCheckpoint()
    {
        PlayCheckpointPassSound();
    }

    public void StopAtBusStop()
    {
        StartCoroutine(WaitForPassengers());
    }

    private IEnumerator WaitForPassengers()
    {
        yield return new WaitForSeconds(10f);
        onBusStopPassed?.Invoke();
    }

    public void PassBusStopCheckpoint()
    {
        Debug.Log("Bus Checkpoint Passed");
    }

    public void StartWork()
    {
        onWorkStarted?.Invoke();
    }

    public void StopWork()
    {
        _passengersCount = 0;
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

    public void AddPassenger()
    {
        _passengersCount++;
    }

    public void RemovePassenger()
    {
        if (_passengersCount < 1) return;
        StartCoroutine(WaitToRemovePassenger());
    }

    IEnumerator WaitToRemovePassenger()
    {
        yield return new WaitForSeconds(1f);
        var passenger = _passengers[UnityEngine.Random.Range(0, _passengers.Count)];
        passenger.transform.position = _getOutPos.position;
        passenger.gameObject.SetActive(true);
        _passengersCount--;
    }
}
