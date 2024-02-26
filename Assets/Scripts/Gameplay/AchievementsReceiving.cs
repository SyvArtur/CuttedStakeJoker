using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsReceiving : MonoBehaviour
{
    [SerializeField] private GameObject _achieveImage;

    private int _caughtCardsInRow;
    private int _countOfJumpsInGame;

    private void Start()
    {
        StartCheckingForPlayOneHourInGame();
    }

    private static AchievementsReceiving _instance;

    public static AchievementsReceiving Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AchievementsReceiving>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("MyAchievementsReceiving");
                    _instance = singletonObject.AddComponent<AchievementsReceiving>();
                }
            }

            return _instance;
        }

    }

    public void CheckForCatchManyCardsInRow(bool _cardIsCatched)
    {
        if (!DataPersistenceManager.Instance._AchievementsData._achievements[0]._completeAchievement)
        {
            if (_cardIsCatched)
            {
                _caughtCardsInRow += 1;
            } else
            {
                _caughtCardsInRow = 0;
            }
            if (_caughtCardsInRow == 40)
            {
                DataPersistenceManager.Instance._AchievementsData._achievements[0]._completeAchievement = true;
                
                
                StartCoroutine(ShowAchieveImageForFewSeconds());
            }
        }
    }

    private IEnumerator ShowAchieveImageForFewSeconds()
    {
        DataPersistenceManager.Instance._ShoppingData.CountMoney += 1000;
        _achieveImage.SetActive(true);
        yield return new WaitForSeconds(4);
        _achieveImage.SetActive(false);
        
    }

    private void StartCheckingForPlayOneHourInGame()
    {
        if (!DataPersistenceManager.Instance._AchievementsData._achievements[1]._completeAchievement)
        {
            StartCoroutine(WaitOneHourForCompleteAchieve());
        }
    }

    private IEnumerator WaitOneHourForCompleteAchieve()
    {
        yield return new WaitForSeconds(3600);
        DataPersistenceManager.Instance._AchievementsData._achievements[1]._completeAchievement = true;
        //SaveChanges();
        StartCoroutine(ShowAchieveImageForFewSeconds());
    }

    public void CheckForPlayingGameAfterEleventh()
    {
        if (!DataPersistenceManager.Instance._AchievementsData._achievements[2]._completeAchievement)
        {
            DateTime now = DateTime.Now;
            TimeSpan lateNightStart = new TimeSpan(23, 0, 0); 
            TimeSpan earlyMorningEnd = new TimeSpan(4, 0, 0); 

            if (now.TimeOfDay >= lateNightStart || now.TimeOfDay < earlyMorningEnd)
            {
                DataPersistenceManager.Instance._AchievementsData._achievements[2]._completeAchievement = true;
                //SaveChanges();
                StartCoroutine(ShowAchieveImageForFewSeconds());
            }
        }
    }

    public void CheckForManyJumpsInOneGame()
    {
        if (!DataPersistenceManager.Instance._AchievementsData._achievements[3]._completeAchievement)
        {
            _countOfJumpsInGame += 1;
            if (_countOfJumpsInGame == 50)
            {
                DataPersistenceManager.Instance._AchievementsData._achievements[3]._completeAchievement = true;
                //SaveChanges();
                StartCoroutine(ShowAchieveImageForFewSeconds());
            }
        }
    }

    public void GameStarted()
    {
        _caughtCardsInRow = 0;
        _countOfJumpsInGame = 0;
        CheckForPlayingGameAfterEleventh();
    }
}
