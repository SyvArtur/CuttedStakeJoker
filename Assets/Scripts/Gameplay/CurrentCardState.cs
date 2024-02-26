using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentCardState : MonoBehaviour
{
    private static CurrentCardState _instance;


    public static CurrentCardState Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CurrentCardState>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("MyCard");
                    _instance = singletonObject.AddComponent<CurrentCardState>();
                }

                //DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    private GameObject _currentCard;
    private int _indexOfSectorPosition;
    private int _cardScore;
    private PropertyOfCard _cardType;
    private bool _scoreDoubled;

    private bool _cardsDoesNotDisappear;
    private Coroutine ScoreDoubling;

    private float _startAbilityTime;

    private void OnEnable()
    {
        CurrentCard = null;
        IndexOfSectorPosition = new int();
        CardScore = 0;
        _startAbilityTime = Time.time;
        CardType = PropertyOfCard.Usual;
        ScoreDoubled = false;
        CardsDoesNotDisappear = false;
        StopAllCoroutines();
    }

    public void StartDoublingScoreAndCardsDoesNotDisappear()
    {
        if (ScoreDoubling != null)
        {
            StopCoroutine(ScoreDoubling);
        }
        _startAbilityTime = Time.time;
        ScoreDoubling = StartCoroutine(AllScoreReceivedAreDoubledFor(20, DataPersistenceManager.Instance._ShoppingData.ScoreDoubledAbiliryForJokerCard, DataPersistenceManager.Instance._ShoppingData.StopTimerAbiliryForJokerCard));
    }

    private IEnumerator AllScoreReceivedAreDoubledFor(float seconds, bool scoreDoubled, bool cardsDoesNotDissapear)
    {
        ScoreDoubled = scoreDoubled;
        CardsDoesNotDisappear = cardsDoesNotDissapear;
        yield return new WaitForSeconds(seconds);
        ScoreDoubled = false;
        CardsDoesNotDisappear = false;
    }

    public int IndexOfSectorPosition { get => _indexOfSectorPosition; set => _indexOfSectorPosition = value; }

    public GameObject CurrentCard { get => _currentCard; set => _currentCard = value; }

    public int CardScore { get => _cardScore; 
        set
        {
            if (ScoreDoubled)
            {
                value = value * 2;
            }
            _cardScore = value;
        }
    }

    public PropertyOfCard CardType { get => _cardType; set => _cardType = value; }

    public bool ScoreDoubled { get => _scoreDoubled; private set => _scoreDoubled = value; }

    public bool CardsDoesNotDisappear { get => _cardsDoesNotDisappear; set => _cardsDoesNotDisappear = value; }

    public float RemainingTimeUntilSbilityRnds { get {
            float time = 20 - (Time.time - _startAbilityTime);
            if (time > 0)
            {
                return time;
            }
            else return 0f;
                } }

    public void NewCardState(GameObject currentCard, int indexOfSectorPosition, int cardScore, PropertyOfCard cardType)
    {
        CurrentCard = currentCard;
        IndexOfSectorPosition = indexOfSectorPosition;
        CardScore = cardScore;
        CardType = cardType;
    }

    public enum PropertyOfCard
    {
        Usual, LightJoker, DarkJoker
    }

}
