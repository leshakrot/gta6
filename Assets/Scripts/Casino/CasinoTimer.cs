using System;
using TMPro;
using UnityEngine;

public class CasinoTimer : MonoBehaviour
{
    public static CasinoTimer instance;

    public static Action onTimeUp;

    public int currentSecond;

    [SerializeField] private float _time;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _timerTextInfo;

    private float _timeLeft = 0f;
    
    private bool _timerOn = false;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        Casino.onRoll += StartTimer;
    }

    private void OnDisable()
    {
        Casino.onRoll += StartTimer;
    }

    private void Update()
    {
        if (_timerOn)
        {
            if (_timeLeft > 0)
            {
                _timeLeft -= Time.deltaTime;
                UpdateTimeText();
            }
            else
            {
                StopTimer();
            }
        }
    }

    private void UpdateTimeText()
    {
        if (_timeLeft < 0)
            _timeLeft = 0;

        //float minutes = Mathf.FloorToInt(_timeLeft / 60);
        float seconds = Mathf.FloorToInt(_timeLeft % 60);
        currentSecond = Mathf.FloorToInt(_timeLeft % 60);
        _timerText.text = seconds.ToString();
    }

    private void StartTimer()
    {
        _timeLeft = _time;
        _timerTextInfo.gameObject.SetActive(true);
        _timerOn = true;
    }

    private void StopTimer()
    {
        _timeLeft = _time;
        _timerOn = false;
        _timerText.text = "";
        _timerTextInfo.gameObject.SetActive(false);
        onTimeUp?.Invoke();
    }
}
