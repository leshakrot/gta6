using UnityEngine;
using System;

public class Farmer : MonoBehaviour
{
    public static Farmer instance;

    public static Action onWorkStarted;
    public static Action onHarvestStarted;
    public static Action onHarvestEnded;
    public static Action onHarvestDelivered;
    public static Action onWorkStop;

    [SerializeField] private GameObject _harvestObject;


    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        onHarvestStarted += StartHarvest;
        onHarvestEnded += StopHarvest;
        onHarvestDelivered += HandOverHarvest;
    }

    private void OnDisable()
    {
        onHarvestStarted -= StartHarvest;
        onHarvestEnded -= StopHarvest;
        onHarvestDelivered -= HandOverHarvest;
    }

    public void StartHarvest()
    {
        onHarvestEnded?.Invoke();
    }

    public void StopHarvest()
    {
        if(_harvestObject) _harvestObject.SetActive(true);
        Debug.Log("Collected!");
    }

    public void HandOverHarvest()
    {
        if (_harvestObject) _harvestObject.SetActive(false);
        Debug.Log("Money earned!");
    }

    public void StartWork()
    {
        onWorkStarted?.Invoke();
    }

    public void StopWork()
    {
        onWorkStop?.Invoke();
    }
}
