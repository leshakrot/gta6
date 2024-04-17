using TMPro;
using UnityEngine;

public class PlayerBankUI : MonoBehaviour
{
    public static PlayerBankUI instance;

    [SerializeField] TextMeshProUGUI _moneyText;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateMoneyUI()
    {
        _moneyText.text = PlayerBank.instance.GetMoneyAmount().ToString();
    }
}
