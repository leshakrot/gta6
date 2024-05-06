using UnityEngine;

public class PlayerBank : MonoBehaviour
{
    public static PlayerBank instance;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            AddMoney(100);
        }
    }

    private int money;

    public void AddMoney(int count)
    {
        money += count;
        PlayerBankUI.instance.UpdateMoneyUI();
    }

    public void RemoveMoney(int count)
    {
        money -= count;
        PlayerBankUI.instance.UpdateMoneyUI();
    }

    public int GetMoneyAmount()
    {
        return money;
    }
}
