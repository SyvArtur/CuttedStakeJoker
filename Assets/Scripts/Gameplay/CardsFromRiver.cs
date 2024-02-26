using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CardsFromRiver : MonoBehaviour//, IShoppingDataPersistence
{
    private Coroutine _coroutineForCreatingCard;
    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private Positions _positions;
    public float _startedCardLifeTime = 4f;
    [SerializeField] private float _cardLifeMin = 2.5f;
    [SerializeField] public float _timeBeforeCreateCard = 0.4f;
    [SerializeField] private UnityEvent CheckForGameOver;

    [SerializeField] private CardSprites _cardSprites;

    [SerializeField] private GameObject _cardExplosion;
    [SerializeField] private GameObject _lvlUpImage;
    //private ShoppingData _shoppingData;
    //[SerializeField] private PlayerShopping _playerShopping;

    private int _remainingNumberCardsToCreateJoker;
    private float _currentCardLifeTime;
    private int _remainingNumberCardsToLevelUp;
    private int _lvl;

    void OnEnable()
    {
        _remainingNumberCardsToCreateJoker = GetNumberCardsToCreateoker();
        _coroutineForCreatingCard = StartCoroutine(CardsCreating());
        AchievementsReceiving.Instance.GameStarted();
        _currentCardLifeTime = _startedCardLifeTime;
        _remainingNumberCardsToLevelUp = 20;
        _lvl = 0;
        _lvlUpImage.SetActive(false);
    }

    void OnDisable()
    {
        if (_coroutineForCreatingCard != null)
        {
            StopCoroutine(_coroutineForCreatingCard);
        }
/*        UnityEditor.EditorUtility.SetDirty(_playerShopping);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();*/
    }

    public void GetScoreAndDestroyCard()
    {
        StartCoroutine(DestroyCardWithDelay(0.6f));
    }

    private IEnumerator DestroyCardWithDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        StopCoroutine(_coroutineForCreatingCard);
        JokerState.Instance.Score += CurrentCardState.Instance.CardScore;
        DataPersistenceManager.Instance._ShoppingData.CountMoney += 10;
        Vector3 posForExplosion = CurrentCardState.Instance.CurrentCard.transform.position + Vector3.up;

        Destroy(CurrentCardState.Instance.CurrentCard);
        CurrentCardState.Instance.CurrentCard = null;
        CheckForGameOver?.Invoke();
        //StopCoroutine(_coroutineForCreatingCard);
        _cardExplosion.transform.position = posForExplosion;
        var animation = _cardExplosion.GetComponent<SkeletonAnimation>();
        animation.AnimationState.SetAnimation(0, "animation", false);

        if (CurrentCardState.Instance.CardType.Equals(CurrentCardState.PropertyOfCard.LightJoker))
        {
            GoodJokerDestroyed();
        } else if (CurrentCardState.Instance.CardType.Equals(CurrentCardState.PropertyOfCard.DarkJoker))
        {
            AudioManager.Instance.CatchDarkJokerSound();
        } else
        {
            AudioManager.Instance.CatchCardSound();
        }
        _coroutineForCreatingCard = StartCoroutine(CardsCreating());
    }

    private void GoodJokerDestroyed()
    {
        CurrentCardState.Instance.StartDoublingScoreAndCardsDoesNotDisappear();
        AudioManager.Instance.CatchGoodJokerSound();
    }

    private void CheckForNewLevel()
    {
        if (_remainingNumberCardsToLevelUp <= 0)
        {
            _lvl += 1;
            _currentCardLifeTime -= 0.3f;
            JokerState.Instance.Score += 1000;
            DataPersistenceManager.Instance._ShoppingData.CountMoney += 150;
            _remainingNumberCardsToLevelUp = 20;
            AudioManager.Instance.LvlUpSound();
            StartCoroutine(ShowLvlUpImageFor(4));
        } else
        {
            _remainingNumberCardsToLevelUp = _remainingNumberCardsToLevelUp - 1;
        }
    }

    private IEnumerator ShowLvlUpImageFor(float seconds)
    {
        _lvlUpImage.SetActive(true);
        _lvlUpImage.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText(_lvl.ToString());
        yield return new WaitForSeconds(seconds);
        _lvlUpImage.SetActive(false);
    }

    private int GetNumberCardsToCreateoker()
    {
        return new System.Random().Next(10, 20);
    }

    private IEnumerator CardsCreating()
    {
        yield return new WaitForSeconds(_timeBeforeCreateCard);
        
        _remainingNumberCardsToCreateJoker -= 1;
        int indexPos =  _positions.GetRandomIndexPosForRiver();
        var my2DPos = _positions.GetStartPositionForRiver(indexPos);
        Vector3 cardPosition = new Vector3(my2DPos.x, my2DPos.y, _cardPrefab.transform.position.z);

        GameObject card = Instantiate(_cardPrefab, cardPosition, Quaternion.identity);

        var cardProperty = card.GetComponent<CardProperty>();
        Animation animation = cardProperty._card.GetComponent<Animation>();
        var cardAppear = animation["Appear"];
        if (cardAppear != null)
        {
            cardAppear.speed = _startedCardLifeTime / _currentCardLifeTime;
            animation.Play(cardAppear.name);
        }

        System.Random rand = new System.Random();

        CurrentCardState.PropertyOfCard propertyOfCard = CurrentCardState.PropertyOfCard.Usual;
        if (_remainingNumberCardsToCreateJoker > 0)
        {
            int indexRandomTypeOfCard = rand.Next(_cardSprites.MyCardSprites.GetLength(1));
            int indexRandomSuitOfCard = rand.Next(_cardSprites.MyCardSprites.GetLength(0));
            cardProperty._cardFace.sprite = _cardSprites.MyCardSprites[indexRandomSuitOfCard, indexRandomTypeOfCard];
            CurrentCardState.Instance.NewCardState(card, indexPos, TypeOfCardIndexToCardScore(indexRandomTypeOfCard), propertyOfCard);
            //Debug.Log("Create usual card");
        } else
        {
            propertyOfCard = (CurrentCardState.PropertyOfCard)rand.Next(1, 3);
            if (propertyOfCard.Equals(CurrentCardState.PropertyOfCard.LightJoker))
            {
                cardProperty._cardFace.sprite = _cardSprites.LightJoker;
            }
            if (propertyOfCard.Equals(CurrentCardState.PropertyOfCard.DarkJoker))
            {
                cardProperty._cardFace.sprite = _cardSprites.DarkJoker;
            }
            //Debug.Log("Create Joker card");
            CurrentCardState.Instance.NewCardState(card, indexPos, 200, propertyOfCard);
            _remainingNumberCardsToCreateJoker = GetNumberCardsToCreateoker();
        }

        CheckForNewLevel();

        yield return new WaitForSeconds(_currentCardLifeTime);
        if (CurrentCardState.PropertyOfCard.Usual.Equals(propertyOfCard) && !CurrentCardState.Instance.CardsDoesNotDisappear)
        {
            CheckForGameOver?.Invoke();
        } else
        {
            if (!CurrentCardState.Instance.CardsDoesNotDisappear)
            {
                Destroy(card);
                _coroutineForCreatingCard = StartCoroutine(CardsCreating());
            } else
            {
                yield return new WaitForSeconds(CurrentCardState.Instance.RemainingTimeUntilSbilityRnds);
                CheckForGameOver?.Invoke();
            }
        }
    }

    private int TypeOfCardIndexToCardScore(int indexRandomTypeOfCard)
    {
        return (indexRandomTypeOfCard + 2) * 10 + 100;
    }

/*    public void LoadData(ShoppingData playerShopping)
    {
        _shoppingData = playerShopping;
        Debug.Log("Ahoj" + _shoppingData._countMoney);
    }

    public void SaveData(ref ShoppingData playerShopping)
    {
        //playerShopping = _shoppingData;
    }*/
}
