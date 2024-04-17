using System.Collections;
using UnityEngine;

public class Passenger : MonoBehaviour
{
    private void OnEnable()
    {
        BusWorker.onStopAtBusStop += GoToBus;
    }

    private void OnDisable()
    {
        BusWorker.onStopAtBusStop -= GoToBus;
    }

    private void GoToBus()
    {
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(Random.Range(0,6));
        BusWorker.onPassengerEnter?.Invoke();
        gameObject.SetActive(false);
    }
}
