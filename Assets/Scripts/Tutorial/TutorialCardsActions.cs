using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class TutorialCardsActions : MonoBehaviour
{
    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private Positions _positions;
    [SerializeField] private CardSprites _cardSprites;
    [SerializeField] private GameObject _cardExplosion;
    //private GameObject _currectCard;

    void Start()
    {

    }

    public void CreateFirstCard(int cardIndexPos)
    {
        var my2DPos = _positions.GetStartPositionForRiver(cardIndexPos);
        Vector3 cardPosition = new Vector3(my2DPos.x, my2DPos.y, -0.01f);

        GameObject card = Instantiate(_cardPrefab, cardPosition, Quaternion.identity);
        var cardProperty = card.GetComponent<CardProperty>();

        System.Random rand = new System.Random();

        int indexRandomTypeOfCard = rand.Next(_cardSprites.MyCardSprites.GetLength(1));
        int indexRandomSuitOfCard = rand.Next(_cardSprites.MyCardSprites.GetLength(0));
        cardProperty._cardFace.sprite = _cardSprites.MyCardSprites[indexRandomSuitOfCard, indexRandomTypeOfCard];

        CurrentCardState.Instance.CurrentCard = card;
        CurrentCardState.Instance.IndexOfSectorPosition = cardIndexPos;
        CurrentCardState.Instance.CardType = CurrentCardState.PropertyOfCard.Usual;
    }

   

    public void CreateSpecialCard(bool goodJoker, int cardIndexPos)
    {
        var my2DPos = _positions.GetStartPositionForRiver(cardIndexPos);
        Vector3 cardPosition = new Vector3(my2DPos.x, my2DPos.y, -0.01f);

        GameObject card = Instantiate(_cardPrefab, cardPosition, Quaternion.identity);
        var cardProperty = card.GetComponent<CardProperty>();

        CurrentCardState.Instance.CurrentCard = card;
        CurrentCardState.Instance.IndexOfSectorPosition = cardIndexPos;
        
        if (goodJoker)
        {
            cardProperty._cardFace.sprite = _cardSprites.LightJoker;
            CurrentCardState.Instance.CardType = CurrentCardState.PropertyOfCard.LightJoker;
        } else
        {
            cardProperty._cardFace.sprite = _cardSprites.DarkJoker;
            CurrentCardState.Instance.CardType = CurrentCardState.PropertyOfCard.DarkJoker;
        }
    }

    public IEnumerator DestroyCardWithDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        Vector3 posForExplosion = CurrentCardState.Instance.CurrentCard.transform.position + Vector3.up;

        Destroy(CurrentCardState.Instance.CurrentCard);

        _cardExplosion.transform.position = posForExplosion;
        var animation = _cardExplosion.GetComponent<SkeletonAnimation>();
        animation.AnimationState.SetAnimation(0, "animation", false);

        if (CurrentCardState.Instance.CardType.Equals(CurrentCardState.PropertyOfCard.LightJoker))
        {
            AudioManager.Instance.CatchGoodJokerSound();
        }
        if (CurrentCardState.Instance.CardType.Equals(CurrentCardState.PropertyOfCard.DarkJoker))
        {
            AudioManager.Instance.CatchDarkJokerSound();
        }
        else
        {
            AudioManager.Instance.CatchCardSound();
        }

    }
}
