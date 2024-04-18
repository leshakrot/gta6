using System;
using UnityEngine;

public class DeliveryMan : MonoBehaviour
{
    public static DeliveryMan instance;

    public static Action onWorkStarted;
    public static Action onDeliveryStarted;
    public static Action onDelivered;
    public static Action onWorkStop;

    [SerializeField] GameObject _directionArrow;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        onWorkStarted += ShowDirectionArrow;
        onDeliveryStarted += StartDelivery;
        onDelivered += HandOver;
        onWorkStop += HideDirectionArrow;
    }

    private void OnDisable()
    {
        onWorkStarted -= ShowDirectionArrow;
        onDeliveryStarted -= StartDelivery;
        onDelivered -= HandOver;
        onWorkStop -= HideDirectionArrow;
    }

    public void StartDelivery()
    {
        
    }

    public void StopDelivery()
    {    
        
    }

    public void HandOver()
    {
        PlayerBank.instance.AddMoney(UnityEngine.Random.Range(100, 211));
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

    private void ShowDirectionArrow()
    {
        _directionArrow.SetActive(true);
    }

    private void HideDirectionArrow()
    {
        _directionArrow.SetActive(false);
    }
}
