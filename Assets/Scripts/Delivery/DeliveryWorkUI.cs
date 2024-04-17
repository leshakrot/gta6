using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeliveryWorkUI : MonoBehaviour
{
    public static DeliveryWorkUI instance;

    [SerializeField] private GameObject _startWorkPopUp;
    [SerializeField] private GameObject _endWorkPopUp;
    [SerializeField] private TextMeshProUGUI _notification;

    [SerializeField] private DeliveryStart _stock;

    private void OnEnable()
    {
        DeliveryMan.onWorkStarted += ShowNotificationFarm;
        DeliveryMan.onDelivered += ShowNotificationFarm;
        DeliveryMan.onDeliveryEnded += ShowNotificationBarn;
        DeliveryMan.onWorkStop += HideNotification;
    }

    private void OnDisable()
    {
        DeliveryMan.onWorkStarted -= ShowNotificationFarm;
        DeliveryMan.onDelivered -= ShowNotificationFarm;
        DeliveryMan.onDeliveryEnded -= ShowNotificationBarn;
        DeliveryMan.onWorkStop -= HideNotification;
    }

    private void Awake()
    {
        instance = this;
    }

    public void ShowStartWorkPopUp()
    {
        _startWorkPopUp.SetActive(true);
    }

    public void CloseStartWorkPopUp()
    {
        _startWorkPopUp.SetActive(false);
    }

    public void ShowEndWorkPopUp()
    {
        _endWorkPopUp.SetActive(true);
    }

    public void CloseEndWorkPopUp()
    {
        _endWorkPopUp.SetActive(false);
    }

    public void ShowNotificationFarm()
    {
        _notification.text = "Везите товар к точке выгрузки";
        _notification.gameObject.SetActive(true);
    }

    public void ShowNotificationBarn()
    {
        _notification.text = "Езжайте на склад, есть заказ";
        _notification.gameObject.SetActive(true);
    }

    public void HideNotification()
    {
        _notification.gameObject.SetActive(false);
    }
}
