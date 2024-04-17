using Invector.vCharacterController;
using UnityEngine;

public class DeliveryStart : MonoBehaviour
{
    [SerializeField] private DeliveryWorkUI _deliveryWorkUI;
    [SerializeField] private DeliveryMan _deliveryMan;

    [SerializeField] private BCG_EnterExitVehicle _bus;
    [SerializeField] private Transform _parkingLot;
    [SerializeField] private BCG_EnterExitPlayer _player;
    [SerializeField] private BCG_EnterExitManager _enterExitManager;

    private bool _isWorking;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out vThirdPersonController player))
        {
            if (!_isWorking) _deliveryWorkUI.ShowStartWorkPopUp();
            else _deliveryWorkUI.ShowEndWorkPopUp();
        }
    }

    public void StartWork()
    {
        _bus.transform.position = _parkingLot.transform.position;
        _bus.gameObject.SetActive(true);
        _player.targetVehicle = _bus;
        _enterExitManager.Interact();
        _deliveryMan.StartWork();
        _isWorking = true;
    }

    public void StopWork()
    {
        _bus.gameObject.SetActive(false);
        _bus.transform.position = _parkingLot.transform.position;
        _deliveryMan.StopWork();
        _isWorking = false;
    }
}
