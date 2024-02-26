using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    //[SerializeField] private PlayerRecords _playerRecords;
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private GameObject[] _playAgainControls;
    [SerializeField] private GameObject[] _menuUI;
    [SerializeField] private TextMeshProUGUI _textScoreForGameOver;
    [SerializeField] private TextMeshProUGUI _textScoreForGameplay;
    private Coroutine TimerCoroutine;
    private int _time;

    private void OnEnable()
    {
        //_gameOverUI.SetActive(false);
        JokerState.Instance.JokerObject.SetActive(true);
        _time = 0;
        TimerCoroutine = StartCoroutine(Timer());
    }


    private IEnumerator Timer()
    {
        _time += 1;
        UpdateScoreAndTimeInGameplay();
        yield return new WaitForSeconds(1);
        TimerCoroutine = StartCoroutine(Timer());
    } 
    
    public void UpdateScoreAndTimeInGameplay()
    {
        string textScore = "";
        if (CurrentCardState.Instance.ScoreDoubled)
        {
            textScore = "x2";
        }
        _textScoreForGameplay.SetText("Score: " + JokerState.Instance.Score.ToString("N0").Replace(",", " ") + " " + textScore + "\nTime: " + _time + " s");
    }

    public void CheckForGameOver()
    {
        if (CurrentCardState.Instance.CurrentCard != null || (CurrentCardState.Instance.CardType.Equals(CurrentCardState.PropertyOfCard.DarkJoker) && CurrentCardState.Instance.CurrentCard == null))
        {
            Destroy(CurrentCardState.Instance.CurrentCard);
            DataPersistenceManager.Instance._RecordsData.CheckForNewRecord(new RecordsData.Record(JokerState.Instance.Score, _time));
            StopCoroutine(TimerCoroutine);
            _textScoreForGameOver.SetText("Your Score:\n<size=130> <color=#FFB423>" + JokerState.Instance.Score.ToString("N0").Replace(",", " ") + "</color></size>\n<voffset=-40>Time: <color=#FFB423>" + _time + " s</color>");
            PlayAgainControls(false);
            _gameOverUI.SetActive(true);
            AudioManager.Instance.StopGameMusic();
            JokerState.Instance.JokerObject.SetActive(false);
        }
    }

    public void PlayAgainControls(bool active)
    {
        for (int i = 0; i < _playAgainControls.Length; i++)
        {
            _playAgainControls[i].SetActive(active);
        }
        _gameOverUI.SetActive(!active);
        if (active)
        {
            OnEnable();
        }
    }


}
