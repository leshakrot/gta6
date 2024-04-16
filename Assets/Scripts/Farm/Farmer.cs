using UnityEngine;
using System;

public class Farmer : MonoBehaviour
{
    public static Farmer instance;

    public static Action onHarvestStarted;
    public static Action onHarvestEnded;
    public static Action onHarvestDelivered;

    [SerializeField] private GameObject _harvestObject;


    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        CheckIfPlayerIsOnGround();
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

    private void CheckIfPlayerIsOnGround()
    {
        if (transform.position.y < -1) transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
    }
}
