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
            else lot.gameObject.SetActive(false);
        }
    }

    private void SpawnCar(int index)
    {
        foreach(var car in _cars)
        {
            car.gameObject.SetActive(false);
        }
        _cars[index].transform.position = _parkingLot.transform.position;
        _cars[index].gameObject.SetActive(true);
        _player.targetVehicle = _cars[index];
        _enterExitManager.Interact();
    }

    public void SellCar()
    {
        if(PlayerBank.instance.GetMoneyAmount() < currentCarPrice)
        {
            CarShopLotUI.instance.ShowNoMoneyPopUp();
            return;
        }
        var index = 0;
        switch (currentCar)
        {
            case CarType.Ram1500:
                {
                    index = 0;
                    SpawnCar(index);
                    break;
                }
            case CarType.McLaren2004:
                {
                    index = 1;
                    SpawnCar(index);
                    break;
                }
            case CarType.McLarenGTS:
                {
                    index = 2;
                    SpawnCar(index);
                    break;
                }
            case CarType.Lancer:
                {
                    index = 3;
                    SpawnCar(index);
                    break;
                }
            case CarType.G63:
                {
                    index = 4;
                    SpawnCar(index);
                    break;
                }
            case CarType.Mustang:
                {
                    index = 5;
                    SpawnCar(index);
                    break;
                }
            case CarType.TraverseBlack:
                {
                    index = 6;
                    SpawnCar(index);
                    break;
                }
            case CarType.TraverseRed:
                {
                    index = 7;
                    SpawnCar(index);
                    break;
                }
            case CarType.Van:
                {
                    index = 8;
                    SpawnCar(index);
                    break;
                }
            case CarType.Suv:
                {
                    index = 9;
                    SpawnCar(index);
                    break;
                }
            case CarType.Sedan:
                {
                    index = 10;
                    SpawnCar(index);
                    break;
                }
            case CarType.Pickup:
                {
                    index = 11;
                    SpawnCar(index);
                    break;
                }
            case CarType.Compact:
                {
                    index = 12;
                    SpawnCar(index);
                    break;
                }
        }
        PlayerGarage.instance.lots[index].isPurchased = true;
        PlayerBank.instance.RemoveMoney(currentCarPrice);
        onCarBought?.Invoke();    
    }
}
