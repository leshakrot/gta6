using TMPro;
using UnityEngine;

public class PlayerGarageLotUI : MonoBehaviour
{
    public static PlayerGarageLotUI instance;

    [SerializeField] private GameObject _carProposalPopUp;
    [SerializeField] private TextMeshProUGUI _proposal;


    private void Awake()
    {
        instance = this;
    }

    public void SetupProposalText(string nameText)
    {
        _proposal.text = "Взять " + nameText + "?";
    }

    public void ShowCarProposalPopUp()
    {
        _carProposalPopUp.SetActive(true);
    }

    public void HideCarProposalPopUp()
    {
        _carProposalPopUp.SetActive(false);
    }
}
