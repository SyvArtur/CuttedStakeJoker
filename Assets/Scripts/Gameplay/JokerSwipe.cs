using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class JokerSwipe : MonoBehaviour
{
    [SerializeField] private CardsFromRiver _cardsFromRiver;
    [SerializeField] private Positions _positions;
    [SerializeField] private float _jumpingTime = 1f;

    [SerializeField] private GameObject[] _lines;

    private Vector2 _startTouchPos;
    private Vector2 _endTouchPos;

    private bool _blockTouch;

    private void OnEnable()
    {
        _blockTouch = false;
        _startTouchPos = new Vector2();
        _endTouchPos = new Vector2();

        //JokerState.Instance.JokerObject.transform.position = _positions.GetPositionForJoker(JokerState.Instance.LowerBankOfRiver, JokerState.Instance.IndexOfSectorPosition);
    }


    void Update()
    {
        if (!_blockTouch && !JokerState.Instance.InJump) {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    _startTouchPos = touch.position;
                }
                float swipeThreshold = 120f;
                if ((touch.position - _startTouchPos).magnitude > swipeThreshold)
                {
                    StartCoroutine(SwipeActive(touch));
                    _blockTouch = true;
                }
            }
        }
    }

    private IEnumerator SwipeActive(Touch touch)
    {
        yield return new WaitForSeconds(0.22f);
        _endTouchPos = touch.position;

        Vector2 swipeDelta = _endTouchPos - _startTouchPos;

        float angle = Mathf.Atan2(swipeDelta.y, swipeDelta.x) * Mathf.Rad2Deg;

        if (!JokerState.Instance.LowerBankOfRiver)
        {
            angle = -angle;
        }

        SkeletonAnimation animation = JokerState.Instance.JokerSkeletonAnimation;

        int jokerIndexPos = JokerState.Instance.IndexOfSectorPosition;
        int cardIndexPos = CurrentCardState.Instance.IndexOfSectorPosition;
        float distance = Mathf.Abs(_positions.GetEstablishedPositionForRiver(0).y - _positions.GetPositionForJoker(JokerState.Instance.LowerBankOfRiver, jokerIndexPos).y) - (JokerState.Instance.LowerBankOfRiver ? 0.4f : 0.1f);
        //Vector2.Distance(_positions.GetPositionForJoker(JokerState.Instance.LowerBankOfRiver, JokerState.Instance.IndexOfSectorPosition), currentCardPos);

        float minAngleForCurrentCard = GetMinRangeAngle(distance, cardIndexPos, jokerIndexPos);
        float maxAngleForCurrentCard = GetMaxRangeAngle(distance, cardIndexPos, jokerIndexPos);


        if (angle > minAngleForCurrentCard && angle < maxAngleForCurrentCard)
        {
            StartCoroutine(MoveJokerToAnotherBank(cardIndexPos, jokerIndexPos));
            JokerState.Instance.IndexOfSectorPosition = cardIndexPos;
            _cardsFromRiver.GetScoreAndDestroyCard();
            animation.AnimationState.SetAnimation(0, "Jump", false);
            AchievementsReceiving.Instance.CheckForCatchManyCardsInRow(true);
            AchievementsReceiving.Instance.CheckForManyJumpsInOneGame();
            JokerState.Instance.InJump = true;
        }

        else
        {
            for (int i = 0; i < _positions.PositionsForRiver.Length; i++)
            {
                if (i != cardIndexPos)
                {
                    float minAngle = GetMinRangeAngle(distance, i, jokerIndexPos);
                    float maxAngle = GetMaxRangeAngle(distance, i, jokerIndexPos);

                    if (angle > minAngle && angle < maxAngle)
                    {
                        StartCoroutine(MoveJokerToAnotherBank(i, JokerState.Instance.IndexOfSectorPosition));
                        JokerState.Instance.IndexOfSectorPosition = i;
                        animation.AnimationState.SetAnimation(0, "Jump", false);
                        JokerState.Instance.InJump = true;
                        AchievementsReceiving.Instance.CheckForCatchManyCardsInRow(false);
                        AchievementsReceiving.Instance.CheckForManyJumpsInOneGame();
                        break;
                    }
                }
            }
        }


        if (!JokerState.Instance.LowerBankOfRiver)
        {
            _lines[0].transform.rotation = Quaternion.Euler(0, 0, -minAngleForCurrentCard - 90);
            _lines[1].transform.rotation = Quaternion.Euler(0, 0, -maxAngleForCurrentCard - 90);
        }
        else
        {
            _lines[0].transform.rotation = Quaternion.Euler(0, 0, minAngleForCurrentCard - 90);
            _lines[1].transform.rotation = Quaternion.Euler(0, 0, maxAngleForCurrentCard - 90);
        }
        _blockTouch = false;
    }

    private float GetMinRangeAngle(float distance, float cardIndexPos, float jokerIndexPos)
    {
        float minAngle = Mathf.Atan(distance / ((cardIndexPos + 1 - (jokerIndexPos + 1) + 0.5f) * 1.0823f)) - (8f / 180f * Mathf.Abs(cardIndexPos - jokerIndexPos));
        if (minAngle < 0)
        {
            minAngle += Mathf.PI;
        }
        minAngle = minAngle * Mathf.Rad2Deg;
        return minAngle;
    }

    private float GetMaxRangeAngle(float distance, float cardIndexPos, float jokerIndexPos)
    {
        float maxAngle = Mathf.Atan(distance / ((cardIndexPos + 1 - (jokerIndexPos + 1) - 0.5f) * 1.0823f)) + (8f / 180f * Mathf.Abs(cardIndexPos - jokerIndexPos));
        if (maxAngle < 0)
        {
            maxAngle += Mathf.PI;
        }
        maxAngle = maxAngle * Mathf.Rad2Deg;
        return maxAngle;
    }


    private IEnumerator MoveJokerToAnotherBank(int sectorOfFinishedPos, int sectorOfStartedPos, float t = 0)
    {
        yield return new WaitForFixedUpdate();
        if (t <= 1.0f)
        {
            Vector2 position = CalculateBezierPoint(t, _positions.GetPositionForJoker(JokerState.Instance.LowerBankOfRiver, sectorOfStartedPos), _positions.GetEstablishedPositionForRiver(sectorOfFinishedPos) + ((JokerState.Instance.LowerBankOfRiver ? -1.7f : 1) * new Vector2(0, 4)), _positions.GetPositionForJoker(!JokerState.Instance.LowerBankOfRiver, sectorOfFinishedPos));
            JokerState.Instance.JokerObject.transform.position = new Vector3(position.x, position.y, gameObject.transform.position.z); ;

            t += Time.deltaTime / _jumpingTime;
            

            //var distance = Vector2.Distance(_positions.GetPositionForJoker(JokerState.Instance.LowerBankOfRiver, JokerState.Instance.IndexOfSectorPosition), _positions.GetPositionForJoker(!JokerState.Instance.LowerBankOfRiver, sectorOfPos));
            float startedYDistance = Math.Abs(_positions.GetPositionForJoker(!JokerState.Instance.LowerBankOfRiver, sectorOfFinishedPos).y - _positions.GetPositionForJoker(JokerState.Instance.LowerBankOfRiver, sectorOfStartedPos).y);
            float currentDistance = Math.Abs(_positions.GetPositionForJoker(!JokerState.Instance.LowerBankOfRiver, sectorOfFinishedPos).y - position.y);
            float fractionOfJourney = currentDistance / startedYDistance;

            float newScale = GetScaleByJokerPos(0.30f, 0.20f, fractionOfJourney);

            JokerState.Instance.JokerObject.transform.localScale = new Vector3(newScale, newScale, JokerState.Instance.JokerObject.transform.localScale.z);

            StartCoroutine(MoveJokerToAnotherBank(sectorOfFinishedPos, sectorOfStartedPos, t));
        } else
        {
            SkeletonAnimation animation = JokerState.Instance.JokerSkeletonAnimation;
            animation.AnimationState.SetAnimation(0, "stay", true);
            JokerState.Instance.LowerBankOfRiver = !JokerState.Instance.LowerBankOfRiver;
            JokerState.Instance.InJump = false;
        }
    }

    private float GetScaleByJokerPos(float scaleTo, float scaleFrom, float fractionOfJourney)
    {
        if (JokerState.Instance.LowerBankOfRiver)
        {
            return Mathf.Lerp(scaleFrom, scaleTo, fractionOfJourney);
        }
        else
        {
            return Mathf.Lerp(scaleTo, scaleFrom, fractionOfJourney);
        }
    }


    private Vector2 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector2 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p2;

        return p;
    }

}
