using Invector.vCharacterController;
using System;
using UnityEngine;

public class BusWorkDepot : MonoBehaviour
{   
    [SerializeField] private BusWorkUI _busWorkUI;
    [SerializeField] private BCG_EnterExitVehicle _bus;
    [SerializeField] private BusWorker _busWorker;
    [SerializeField] private Transform _parkingLot;

    [SerializeField] private BCG_EnterExitPlayer _player;
    [SerializeField] private BCG_EnterExitManager _enterExitManager;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out vThirdPersonController player))
        {
            _busWorkUI.ShowStartWorkPopUp();
        }
    }

    public void StartWork()
    {
        _bus.transform.position = _parkingLot.transform.position;
        _bus.gameObject.SetActive(true);
        _player.targetVehicle = _bus;
        _enterExitManager.Interact();
        _busWorker.StartWork();
    }

    public void StopWork()
    {
        _bus.gameObject.SetActive(false);
        _bus.transform.position = _parkingLot.transform.position;       
        _busWorker.StopWork();
    }
}
