using System;
using System.Collections.Generic;
using UnityEngine;

public class CarShop : MonoBehaviour
{
    public static CarShop instance;

    public static Action onCarBought;

    public CarType currentCar;
    public int currentCarPrice;

    [SerializeField] private List<BCG_EnterExitVehicle> _cars = new List<BCG_EnterExitVehicle>();
    [SerializeField] private BCG_EnterExitPlayer _player;
    [SerializeField] private BCG_EnterExitManager _enterExitManager;

    [SerializeField] private Transform _parkingLot;

    [SerializeField] private List<CarShopLot> _lots = new List<CarShopLot>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InitLots();
    }

    private void InitLots()
    {
        foreach (var lot in _lots)
        {
            if (!lot.isSold) lot.gameObject.SetActive(true);
        }
    }

    private void SpawnCar(int index)
    {
        _cars[index].transform.position = _parkingLot.transform.position;
        _cars[index].gameObject.SetActive(true);
        _player.targetVehicle = _cars[index];
        _enterExitManager.Interact();
    }

    public void SellCar()
    {
        switch (currentCar)
        {
            case CarType.Ram1500:
                {
                    SpawnCar(0);
                    break;
                }
            case CarType.McLaren2004:
                {
                    SpawnCar(1);
                    break;
                }
            case CarType.McLarenGTS:
                {
                    SpawnCar(2);
                    break;
                }
            case CarType.Lancer:
                {
                    SpawnCar(3);
                    break;
                }
            case CarType.G63:
                {
                    SpawnCar(4);
                    break;
                }
            case CarType.Mustang:
                {
                    SpawnCar(5);
                    break;
                }
            case CarType.TraverseBlack:
                {
                    SpawnCar(6);
                    break;
                }
            case CarType.TraverseRed:
                {
                    SpawnCar(7);
                    break;
                }
            case CarType.Van:
                {
                    SpawnCar(8);
                    break;
                }
            case CarType.Suv:
                {
                    SpawnCar(9);
                    break;
                }
            case CarType.Sedan:
                {
                    SpawnCar(10);
                    break;
                }
            case CarType.Pickup:
                {
                    SpawnCar(11);
                    break;
                }
            case CarType.Compact:
                {
                    SpawnCar(12);
                    break;
                }
        }
        onCarBought?.Invoke();
        PlayerBank.instance.RemoveMoney(currentCarPrice);
    }
}
