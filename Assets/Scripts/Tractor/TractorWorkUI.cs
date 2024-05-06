using UnityEngine;

public class TractorWorkUI : MonoBehaviour
{
    public static TractorWorkUI instance;

    [SerializeField] private GameObject _startWorkPopUp;
    [SerializeField] private GameObject _noLevelPopUp;
    [SerializeField] private GameObject _endWorkPopUp;
    [SerializeField] private GameObject _notification;

    [SerializeField] private TractorWorkStart _workStart;

    private void Awake()
    {
        instance = this;
    }

    private void ShowNoLevelPopUp()
    {
        _noLevelPopUp.SetActive(true);
    }

    public void ShowStartWorkPopUp()
    {
        if (PlayerLevel.instance.GetCurrentLevel() < 2)
        {
            ShowNoLevelPopUp();
            return;
        }
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

    public void ShowNotification()
    {
        _notification.SetActive(true);
    }

    public void HideNotification()
    {
        _notification.SetActive(false);
    }

    public void HideNoLevelPopUp()
    {
        _noLevelPopUp.SetActive(false);
    }
}
