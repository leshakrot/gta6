using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Casino : MonoBehaviour
{
    public static Action onRoll;
    public static Action onBetPlaced;
    public static Action onPlayerWon;
    public static Action onPlayerLost;
    public static Action onGameEnded;

    [SerializeField] private GameObject _startGamePopup;
    [SerializeField] private GameObject _endGamePopup;
    [SerializeField] private GameObject _gameCoreUI;
    [SerializeField] private TextMeshProUGUI _betText;
    [SerializeField] private TextMeshProUGUI _betInfoText;
    [SerializeField] private Button _applyBetButton;
    [SerializeField] private Button _enterGameButton;
    [SerializeField] private Button _exitGameButton;
    [SerializeField] private Button _throwDiceButton;
    [SerializeField] private TextMeshProUGUI _applyBetText;
    [SerializeField] private List<TextMeshProUGUI> _botBetsText = new List<TextMeshProUGUI>();
    [SerializeField] private List<Button> _betButtons = new List<Button>();
    [SerializeField] private TextMeshProUGUI _playerBetText;
    [SerializeField] private TextMeshProUGUI _winnerInfoText;
    
    private int _betAmount;

    private int _maxScore;
    private int _playerScore;
    private int _currentPlayersAmount;

    private List<TextMeshProUGUI> _currentWinnersBetsText = new List<TextMeshProUGUI>();

    private void OnEnable()
    {
        CasinoTimer.onTimeUp += TryGiveWinningToPlayer;
        CasinoTimer.onTimeUp += ResetGameCore;
        CasinoTimer.onTimeUp += Reroll;
    }

    private void OnDisable()
    {
        CasinoTimer.onTimeUp -= TryGiveWinningToPlayer;
        CasinoTimer.onTimeUp -= ResetGameCore;
        CasinoTimer.onTimeUp -= Reroll;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out CasinoPlayer player))
        {
            ShowStartGamePopup();
        }
    }
    public void StartGame()
    {
        _gameCoreUI.SetActive(true);
        _betAmount = 0;
        UpdateBetText();
    }

    public void QuitGame()
    {
        _playerBetText.text = "...";
        _winnerInfoText.text = "";
        _gameCoreUI.SetActive(false);
        _applyBetButton.gameObject.SetActive(false);
        _applyBetText.gameObject.SetActive(false);
    }

    public void SetBet(int amount)
    {
        _betAmount += amount;
        _applyBetButton.gameObject.SetActive(true);
    }

    public void ApplyBet()
    {
        onBetPlaced?.Invoke();
        HideBetButtons();
        _applyBetText.gameObject.SetActive(true);
        _applyBetButton.gameObject.SetActive(false);
        ShowEnterGameButton();    
    }

    public void ResponseToRoll()
    {
        Roll(UnityEngine.Random.Range(1, 5));
        ShowThrowDiceButton();
        HideEnterGameButton();
        _exitGameButton.gameObject.SetActive(false);
    }

    private void Roll(int playersCount)
    {
        _winnerInfoText.text = "";
        _maxScore = 0;
        _playerScore = 0;
        onRoll?.Invoke();
        foreach(var bet in _botBetsText)
        {
            bet.text = "...";
        }

        var random = new System.Random();
        var newBetsList = _botBetsText.OrderBy(s => random.Next()).Take(playersCount).ToList();
        
        _currentPlayersAmount = newBetsList.Count + 1;
        foreach (var bet in newBetsList)
        {        
            StartCoroutine(WaitTillBotMakeBet(bet));
        }
    }

    private void Reroll()
    {
        Debug.Log("COUNT " + _currentWinnersBetsText.Count);
        if (_currentWinnersBetsText.Count < 2) return;
        _maxScore = 0;
        _playerScore = 0;
        _playerBetText.text = "...";
        onRoll?.Invoke();
        HideThrowDiceButton();
        if (_currentWinnersBetsText.Contains(_playerBetText)) ShowThrowDiceButton();
        foreach (var bet in _botBetsText)
        {
            bet.text = "...";
        }
        foreach (var bet in _currentWinnersBetsText)
        {
            if (bet == _playerBetText)
            {
                continue;
            }
            StartCoroutine(WaitTillBotMakeBet(bet));
        }
    }

    public void ThrowDice()
    {
        if(CasinoTimer.instance.currentSecond <= 1) _playerScore = UnityEngine.Random.Range(8, 13);
        else _playerScore = UnityEngine.Random.Range(2, 13);
        //_playerScore = 12;
        _playerBetText.text = _playerScore.ToString();
        if (_playerScore == _maxScore)
        {
            _currentWinnersBetsText.Add(_playerBetText);
        }
        else if (_playerScore > _maxScore)
        {
            _maxScore = _playerScore;
            _currentWinnersBetsText.Clear();
            _currentWinnersBetsText.Add(_playerBetText);
        }
        HideThrowDiceButton();
    }

    public void TryGiveWinningToPlayer()
    {
        if (_currentWinnersBetsText.Count > 1) return;
        if (_playerScore == _maxScore)
        {
            Debug.Log("WIN");
            _winnerInfoText.text = "вы победили! выигрыш: " + _betAmount * _currentPlayersAmount;
        }
    }

    private IEnumerator WaitTillBotMakeBet(TextMeshProUGUI bet)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(1, 30));
        bet.text = UnityEngine.Random.Range(2, 13).ToString();
        var betScore = Convert.ToInt32(bet.text);
        if (betScore == _maxScore)
        {
            _currentWinnersBetsText.Add(bet);
        }
        else if (betScore > _maxScore)
        {
            _maxScore = betScore;
            _currentWinnersBetsText.Clear();
            _currentWinnersBetsText.Add(bet);
        }
    }

    public void ShowStartGamePopup()
    {
        _startGamePopup.SetActive(true);
    }

    public void CloseStartGamePopup()
    {
        _startGamePopup.SetActive(false);
    }

    public void ShowEndGamePopup()
    {
        _endGamePopup.SetActive(true);
    }

    public void CloseEndGamePopup()
    {
        _endGamePopup.SetActive(false);
    }

    public void ShowGameCoreUI()
    {
        _gameCoreUI.SetActive(true);
    }

    public void HideGameCoreUI()
    {
        _gameCoreUI.SetActive(false);
    }

    public void ShowEnterGameButton()
    {
        _enterGameButton.gameObject.SetActive(true);
    }

    public void HideEnterGameButton()
    {
        _enterGameButton.gameObject.SetActive(false);
    }

    public void HideThrowDiceButton()
    {
        _throwDiceButton.gameObject.SetActive(false);
    }

    public void ShowThrowDiceButton()
    {
        _throwDiceButton.gameObject.SetActive(true);
    }

    public void ShowBetButtons()
    {
        foreach(var button in _betButtons)
        {
            button.gameObject.SetActive(true);
        }
        _betInfoText.gameObject.SetActive(true);
    }

    public void HideBetButtons()
    {
        foreach (var button in _betButtons)
        {
            button.gameObject.SetActive(false);
            _betInfoText.gameObject.SetActive(false);
        }
    }

    public void UpdateBetText()
    {
        _betText.text = "Ваша ставка:" + _betAmount.ToString();
        foreach(var betText in _botBetsText)
        {
            betText.text = betText.text;
        }
    }

    public void ResetGameCore()
    {
        if (_currentWinnersBetsText.Count > 1) return;
        ResetGameCoreUI();
        _currentPlayersAmount = 0;
        _betAmount = 0;
        UpdateBetText();
        ShowBetButtons();
    }

    public void ResetGameCoreUI()
    {       
        _applyBetText.gameObject.SetActive(false);
        _applyBetButton.gameObject.SetActive(false);
        HideThrowDiceButton();
        _enterGameButton.gameObject.SetActive(false);
        _exitGameButton.gameObject.SetActive(true);
        
        foreach (var bet in _botBetsText)
        {
            bet.text = "...";
        }

        _playerBetText.text = "...";
    }
}
