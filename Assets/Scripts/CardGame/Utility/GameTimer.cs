using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private Text _text;
    [Space]
    [Header("Количество времени в секундах")]
    [SerializeField] private int _initialTime;

    public UnityEvent OnTimeRanOut;

    private float _timeRemaining;
    private int _minutes, _seconds;

    private void Awake()
    {
        _timeRemaining = _initialTime;

        UpdateText();
    }

    private void Update()
    {
        _timeRemaining -= Time.deltaTime;
        if(_timeRemaining > 0)
        {
            UpdateText();
        }
        else
        {
            OnTimeRanOut.Invoke();
            enabled = false;
        }
    }

    private void UpdateText()
    {
        int prevSeconds = _seconds;

        int roundedTime = Mathf.FloorToInt(_timeRemaining);
        _minutes = Mathf.FloorToInt(roundedTime / 60f);
        _seconds = roundedTime % 60;

        string minutesText = _minutes < 10 ? $"0{_minutes}" : $"{_minutes}";
        string secondsText = _seconds < 10 ? $"0{_seconds}" : $"{_seconds}";

        if (prevSeconds != _seconds)
            _text.text = minutesText + ":" + secondsText;
    }
}
