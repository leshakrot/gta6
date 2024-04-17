using TMPro;
using UnityEngine;

public class FarmWorkUI : MonoBehaviour
{
    public static FarmWorkUI instance;

    [SerializeField] private GameObject _startWorkPopUp;
    [SerializeField] private GameObject _endWorkPopUp;
    [SerializeField] private TextMeshProUGUI _notification;

    [SerializeField] private FarmStart _farm;

    private void OnEnable()
    {
        Farmer.onWorkStarted += ShowNotificationFarm;
        Farmer.onHarvestDelivered += ShowNotificationFarm;
        Farmer.onHarvestEnded += ShowNotificationBarn;
        Farmer.onWorkStop += HideNotification;
    }

    private void OnDisable()
    {
        Farmer.onWorkStarted -= ShowNotificationFarm;
        Farmer.onHarvestDelivered -= ShowNotificationFarm;
        Farmer.onHarvestEnded -= ShowNotificationBarn;
        Farmer.onWorkStop -= HideNotification;
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
        _notification.text = "Соберите урожай";
        _notification.gameObject.SetActive(true);
    }

    public void ShowNotificationBarn()
    {
        _notification.text = "Отнесите урожай в амбар";
        _notification.gameObject.SetActive(true);
    }

    public void HideNotification()
    {
        _notification.gameObject.SetActive(false);
    }
}
