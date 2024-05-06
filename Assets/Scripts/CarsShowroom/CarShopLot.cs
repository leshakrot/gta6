using Invector.vCharacterController;
using UnityEngine;

public enum CarType
{
    Ram1500,
    McLaren2004,
    McLarenGTS,
    Lancer,
    G63,
    Mustang,
    TraverseRed,
    TraverseBlack,
    Van,
    Suv,
    Sedan,
    Pickup,
    Compact
}

public class CarShopLot : MonoBehaviour
{
    public bool isSold;

    [SerializeField] private CarType _carType;
    [SerializeField] private int _price;
    [SerializeField] private string _name;
    [SerializeField] private CarShopLotUI _carShopLotUI;
    [SerializeField] private CarShop _carShop;

    private void OnEnable()
    {
        CarShop.onCarBought += TryHideChechpoint;
    }

    private void OnDisable()
    {
        CarShop.onCarBought -= TryHideChechpoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out vThirdPersonController player))
        {
            _carShop.currentCar = _carType;
            _carShop.currentCarPrice = _price;
            MakeProposal();
        }
    }

    public void MakeProposal()
    {
        _carShopLotUI.SetupProposalText(_name, _price.ToString());

        _carShopLotUI.ShowCarProposalPopUp();
    }

    private void TryHideChechpoint()
    {
        if(_carType == _carShop.currentCar)
        {
            gameObject.SetActive(false);
            isSold = true;
        } 
    }
}
