using TMPro;
using UnityEngine;

public class CarShopLotUI : MonoBehaviour
{
    public static CarShopLotUI instance;
    
    [SerializeField] private GameObject _carProposalPopUp;
    [SerializeField] private GameObject _noMoneyPopUp;
    [SerializeField] private TextMeshProUGUI _proposal;


    private void Awake()
    {
        instance = this;
    }

    public void SetupProposalText(string nameText, string priceText)
    {
        _proposal.text = "Купить " + nameText + "?\n" + "Стоимость: " + priceText;
    }

    public void ShowCarProposalPopUp()
    {
        _carProposalPopUp.SetActive(true);
    }

    public void HideCarProposalPopUp()
    {
        _carProposalPopUp.SetActive(false);
    }

    public void ShowNoMoneyPopUp()
    {
        _noMoneyPopUp.SetActive(true);
    }

    public void HideNoMoneyPopUp()
    {
        _noMoneyPopUp.SetActive(false);
    }
}
