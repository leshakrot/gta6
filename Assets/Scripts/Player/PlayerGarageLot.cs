using Invector.vCharacterController;
using UnityEngine;

public class PlayerGarageLot : MonoBehaviour
{
    public bool isPurchased;

    [SerializeField] private CarType _carType;
    [SerializeField] private string _name;
    [SerializeField] private PlayerGarageLotUI _playerGarageLotUI;
    [SerializeField] private PlayerGarage _playerGarage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out vThirdPersonController player))
        {
            _playerGarage.currentCar = _carType;
            MakeProposal();
        }
    }

    public void MakeProposal()
    {
        _playerGarageLotUI.SetupProposalText(_name);
        _playerGarageLotUI.ShowCarProposalPopUp();
    }
}
