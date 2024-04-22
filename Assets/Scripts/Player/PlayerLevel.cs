using System;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    public static PlayerLevel instance;

    public static Action onLevelReached;
    public static Action onExpAdded;

    [SerializeField] private PlayerLevelUI _playerLevelUI;

    private int _level;
    private int _currentLevelExp;
    private int _expAmountToNextlvl = 50;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _playerLevelUI.UpdateExpText();
        _playerLevelUI.UpdateLvlSlider();
        _playerLevelUI.UpdateLvlText();
    }

    public int GetCurrentLevel()
    {
        return _level;
    }

    public int GetCurrentExp()
    {
        return _currentLevelExp;
    }

    public int GetExpAmountToNextLvl()
    {
        return _expAmountToNextlvl;
    }

    public void AddExp(int exp)
    {
        _currentLevelExp += exp;
        onExpAdded?.Invoke();
        if(_currentLevelExp >= _expAmountToNextlvl)
        {
            IncreaseLevel();
        }
        _playerLevelUI.UpdateLvlSlider();
        _playerLevelUI.UpdateExpText();
    }

    public void IncreaseLevel()
    {       
        _currentLevelExp = 0;
        _level++;
        _expAmountToNextlvl *= 2;
        _playerLevelUI.UpdateLvlText();
        onLevelReached?.Invoke();
    }

}
