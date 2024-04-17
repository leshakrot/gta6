using Invector.vCharacterController;
using UnityEngine;

public class FarmStart : MonoBehaviour
{
    [SerializeField] private FarmWorkUI _farmWorkUI;
    [SerializeField] private Farmer _farmer;

    private bool _isWorking;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out vThirdPersonController player))
        {
            if (!_isWorking) _farmWorkUI.ShowStartWorkPopUp();
            else _farmWorkUI.ShowEndWorkPopUp();
        }
    }

    public void StartWork()
    {
        _farmer.StartWork();
        _isWorking = true;
    }

    public void StopWork()
    {
        _farmer.StopWork();
        _isWorking = false;
    }
}
