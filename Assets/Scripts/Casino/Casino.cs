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
    [SerializeField] private Button _applyBetButton;
    [SerializeField] private Button _enterGameButton;
    [SerializeField] private Button _throwDiceButton;
    [SerializeField] private TextMeshProUGUI _applyBetText;
    [SerializeField] private List<TextMeshProUGUI> _botBetsText = new List<TextMeshProUGUI>();
    [SerializeField] private TextMeshProUGUI _playerBetText;
    [SerializeField] private TextMeshProUGUI _winnerInfoText;
    
    private int _betAmount;

    private int _maxScore;
    private int _playerScore;
    private int _currentPlayersAmount;

    private void Start()
    {

    }

    private void OnEnable()
    {
        CasinoTimer.onTimeUp += TryGiveWinningToPlayer;
        CasinoTimer.onTimeUp += ResetGameCore;
    }

    private void OnDisable()
    {
        CasinoTimer.onTimeUp -= TryGiveWinningToPlayer;
        CasinoTimer.onTimeUp -= ResetGameCore;
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
        _gameCoreUI.SetActive(false);
    }

    public void SetBet(int amount)
    {
        _betAmount += amount;
        _applyBetButton.gameObject.SetActive(true);
    }

    public void ApplyBet()
    {
        onBetPlaced?.Invoke();
        _applyBetText.gameObject.SetActive(true);
        _applyBetButton.gameObject.SetActive(false);
        ShowEnterGameButton();
    }

    public void ResponseToRoll()
    {
        Roll(UnityEngine.Random.Range(1, 5));
        _throwDiceButton.gameObject.SetActive(true);
        _enterGameButton.gameObject.SetActive(false);
    }

    private void Roll(int playersCount)
    {
        _maxScore = 0;
        _playerScore = 0;
        onRoll?.Invoke();
        foreach(var bet in _botBetsText)
        {
            bet.text = "...";
        }

        var random = new System.Random();
        var listNew = _botBetsText.OrderBy(s => random.Next()).Take(playersCount).ToList();
        _currentPlayersAmount = listNew.Count + 1;
        foreach (var bet in listNew)
        {        
            StartCoroutine(WaitTillBotMakeBet(bet));
        }
    }

    public void ThrowDice()
    {
        //_playerScore = UnityEngine.Random.Range(2, 13);
        _playerScore = 12;
        _playerBetText.text = _playerScore.ToString();
        HideThrowDiceButton();
    }

    public void TryGiveWinningToPlayer()
    {
        if (_playerScore > _maxScore) 
        {
            Debug.Log("WIN");
            _winnerInfoText.text = "вы победили! выигрыш: " + _betAmount * _currentPlayersAmount;
        } 
    }

    private IEnumerator WaitTillBotMakeBet(TextMeshProUGUI bet)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(1, 10));
        bet.text = UnityEngine.Random.Range(2, 13).ToString();
        var betScore = Convert.ToInt32(bet.text);
        if (betScore > _maxScore)
        {
            _maxScore = betScore;
        }
        //UpdateBetText();
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

    public void HideThrowDiceButton()
    {
        _throwDiceButton.gameObject.SetActive(false);
    }

    public void UpdateGameCoreUI()
    {

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
        ResetGameCoreUI();
        _currentPlayersAmount = 0;
    }

    public void ResetGameCoreUI()
    {
        _applyBetText.gameObject.SetActive(false);
        _applyBetButton.gameObject.SetActive(false);
        _throwDiceButton.gameObject.SetActive(false);
        _enterGameButton.gameObject.SetActive(true);
        _enterGameButton.gameObject.SetActive(false);

        foreach (var bet in _botBetsText)
        {
            bet.text = "...";
        }

        _playerBetText.text = "...";
    }
}
