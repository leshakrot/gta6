using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelUI : MonoBehaviour
{
    [SerializeField] private Slider _lvlSlider;
    [SerializeField] private TextMeshProUGUI _lvlText;
    [SerializeField] private TextMeshProUGUI _expText;

    [SerializeField] private PlayerLevel _playerLevel;

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        
    }

    private void Awake()
    {
        _lvlSlider.maxValue = _playerLevel.GetExpAmountToNextLvl();
    }

    public void UpdateLvlSlider()
    {
        if (_playerLevel.GetExpAmountToNextLvl() >= _lvlSlider.maxValue)
        {
            _lvlSlider.maxValue = _playerLevel.GetExpAmountToNextLvl();
        }
        _lvlSlider.value = _playerLevel.GetCurrentExp();
    }

    public void UpdateLvlText()
    {
        _lvlText.text = "Уровень " + _playerLevel.GetCurrentLevel().ToString();
    }

    public void UpdateExpText()
    {
        _expText.text = _playerLevel.GetCurrentExp().ToString() + " / " + _playerLevel.GetExpAmountToNextLvl().ToString();
    }
}
