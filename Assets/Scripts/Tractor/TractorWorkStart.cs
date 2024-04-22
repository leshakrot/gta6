using Invector.vCharacterController;
using UnityEngine;

public class TractorWorkStart : MonoBehaviour
{
    [SerializeField] private TractorWorkUI _tractorWorkUI;
    [SerializeField] private BCG_EnterExitVehicle _tractor;
    [SerializeField] private TractorWorker _tractorWorker;
    [SerializeField] private Transform _parkingLot;

    [SerializeField] private BCG_EnterExitPlayer _player;
    [SerializeField] private BCG_EnterExitManager _enterExitManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out vThirdPersonController player))
        {
            _tractorWorkUI.ShowStartWorkPopUp();
        }
    }

    public void StartWork()
    {
        _tractor.transform.position = _parkingLot.transform.position;
        _tractor.gameObject.SetActive(true);
        _player.targetVehicle = _tractor;
        _enterExitManager.Interact();
        _tractorWorker.StartWork();
    }

    public void StopWork()
    {
        _player.GetOut();
        _tractorWorker.StopWork();
        _tractor.gameObject.SetActive(false);
        _tractor.transform.position = _parkingLot.transform.position;
    }
}
