using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class TutorialJokerActions : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation _jokerSkeletonAnimation;
    [SerializeField] private Positions _positions;
    [SerializeField] private float _jumpingTime = 1f;
    private Vector2 _startTouchPos;
    private Vector2 _endTouchPos;
    private bool _blockTouch;
    private int _jokerIndexPos;
    private bool _jokerInJump;
    private bool _jokerInLowerBankOfRiver;

    void OnEnable()
    {
        _jokerInJump = false;
        _jokerIndexPos = 1;
        _jokerInLowerBankOfRiver = true;
        gameObject.transform.position = _positions.GetPositionForJoker(_jokerInLowerBankOfRiver, _jokerIndexPos);
        //gameObject.transform.localScale = new Vector3(1, 1, 1);
        _jokerSkeletonAnimation.AnimationState.SetAnimation(0, "stay", true);
        MakeJokerBright(false);
    }

    public void MakeJokerBright(bool bright)
    {
        if (bright)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -0.01f);
        } else
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);
        }
    }

    public void CheckForCorrectJump(int cardIndexPos, Action actionForCorrectJump, Action actionForCorrectJumpToOtherSide)
    {
        if (!_blockTouch && !_jokerInJump)
        {
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
                    _blockTouch = true;
                    StartCoroutine(SwipeActive(touch, _jokerIndexPos, cardIndexPos, actionForCorrectJump, actionForCorrectJumpToOtherSide));
                }
            }
        }
    }

    private IEnumerator SwipeActive(Touch touch, int jokerIndexPos, int cardIndexPos, Action actionForCorrectJump, Action actionForCorrectJumpToOtherSide)
    {
        yield return new WaitForSeconds(0.22f);
        _endTouchPos = touch.position;

        Vector2 swipeDelta = _endTouchPos - _startTouchPos;

        float angle = Mathf.Atan2(swipeDelta.y, swipeDelta.x) * Mathf.Rad2Deg;

        if (!_jokerInLowerBankOfRiver)
        {
            angle = -angle;
        }

        float distance = Mathf.Abs(_positions.GetEstablishedPositionForRiver(0).y - _positions.GetPositionForJoker(_jokerInLowerBankOfRiver, jokerIndexPos).y) - (_jokerInLowerBankOfRiver ? 0.4f : 0.1f);
        //Vector2.Distance(_positions.GetPositionForJoker(JokerState.Instance.LowerBankOfRiver, JokerState.Instance.IndexOfSectorPosition), currentCardPos);

        float minAngleForCurrentCard = GetMinRangeAngle(distance, cardIndexPos, jokerIndexPos);
        float maxAngleForCurrentCard = GetMaxRangeAngle(distance, cardIndexPos, jokerIndexPos);

        if (angle > minAngleForCurrentCard && angle < maxAngleForCurrentCard)
        {
            StartCoroutine(MoveJokerToAnotherBank(cardIndexPos, _jokerInLowerBankOfRiver, jokerIndexPos, actionForCorrectJumpToOtherSide));
            actionForCorrectJump?.Invoke();
            _jokerSkeletonAnimation.AnimationState.SetAnimation(0, "Jump", false);
            _jokerIndexPos = cardIndexPos;
            _jokerInJump = true;
            //stop
        }
        _blockTouch = false;
    }


    private IEnumerator MoveJokerToAnotherBank(int sectorOfFinishedPos, bool fromLowerBankOfRiver, int sectorOfStartedPos, Action actionForCorrectJumpToOtherSide, float t = 0)
    {
        yield return null;
        if (t <= 1.0f)
        {
            Vector2 position = CalculateBezierPoint(t, _positions.GetPositionForJoker(fromLowerBankOfRiver, sectorOfStartedPos), _positions.GetEstablishedPositionForRiver(sectorOfFinishedPos) + ((fromLowerBankOfRiver ? -1.7f : 1) * new Vector2(0, 4)), _positions.GetPositionForJoker(!fromLowerBankOfRiver, sectorOfFinishedPos));
            gameObject.transform.position = new Vector3 (position.x, position.y, gameObject.transform.position.z);

            t += Time.deltaTime / _jumpingTime;

            //var distance = Vector2.Distance(_positions.GetPositionForJoker(JokerState.Instance.LowerBankOfRiver, JokerState.Instance.IndexOfSectorPosition), _positions.GetPositionForJoker(!JokerState.Instance.LowerBankOfRiver, sectorOfPos));
            float startedYDistance = Math.Abs(_positions.GetPositionForJoker(!fromLowerBankOfRiver, sectorOfFinishedPos).y - _positions.GetPositionForJoker(fromLowerBankOfRiver, sectorOfStartedPos).y);
            float currentDistance = Math.Abs(_positions.GetPositionForJoker(!fromLowerBankOfRiver, sectorOfFinishedPos).y - position.y);
            float fractionOfJourney = currentDistance / startedYDistance;

            float newScale = GetScaleByJokerPos(0.3f, 0.2f, fractionOfJourney, fromLowerBankOfRiver);

            gameObject.transform.localScale = new Vector3(newScale, newScale, gameObject.transform.localScale.z);

            StartCoroutine(MoveJokerToAnotherBank(sectorOfFinishedPos, fromLowerBankOfRiver, sectorOfStartedPos, actionForCorrectJumpToOtherSide, t));
        }
        else
        {
            _jokerSkeletonAnimation.AnimationState.SetAnimation(0, "stay", true);
            actionForCorrectJumpToOtherSide?.Invoke();
            _jokerInLowerBankOfRiver = !_jokerInLowerBankOfRiver;
            _jokerInJump = false;
        }
    }

    private float GetScaleByJokerPos(float scaleTo, float scaleFrom, float fractionOfJourney, bool fromLowerBankOfRiver)
    {
        if (fromLowerBankOfRiver)
        {
            return Mathf.Lerp(scaleFrom, scaleTo, fractionOfJourney);
        }
        else
        {
            return Mathf.Lerp(scaleTo, scaleFrom, fractionOfJourney);
        }
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
