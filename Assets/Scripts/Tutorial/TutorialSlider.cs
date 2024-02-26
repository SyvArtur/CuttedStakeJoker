using Spine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class TutorialSlider : MonoBehaviour
{
    [SerializeField] private GameObject[] _sliderTexts;
    //[SerializeField] private GameObject _slider;
    private int _indexOfSlide;
    [FormerlySerializedAs("JokerActions")]
    [SerializeField] private TutorialJokerActions _jokerActions;
    [SerializeField] private int _slideIndexToCheckForCorrectJumpToCard;
    [SerializeField] private int _slideIndexToCheckForCorrectJumpToSpecialCard;
    [SerializeField] private int _slideIndexToCreateDarkJoker;
    [SerializeField] private GameObject _handler;
    //[SerializeField] private GameObject _joker;
    [SerializeField] private TutorialCardsActions _cardsActions;
    [SerializeField] private GameModeManager _gameModeManager;

    private bool _permissionTo—heckForCorrectJump = false;
    private Action _actionForCurrectJump;
    private Action _actionForCurrectJumpToOtherSide;
    private Coroutine _startCheckingForCorrectJump;

    void OnEnable()
    {
        _indexOfSlide = 0;
        _sliderTexts[_indexOfSlide].gameObject.SetActive(true);
        _handler.SetActive(false);
        _permissionTo—heckForCorrectJump = false;

        _actionForCurrectJump = () => {
            StopCoroutine(_startCheckingForCorrectJump);
            StartCoroutine(_cardsActions.DestroyCardWithDelay(0.6f));
        };

        _actionForCurrectJumpToOtherSide = () => {
            _permissionTo—heckForCorrectJump = false;
            _handler.SetActive(false);
            _jokerActions.MakeJokerBright(false);
            CLickOnScreen();
        };
        
    }

    void Update()
    { 
        if (Input.touchCount > 0 && !_permissionTo—heckForCorrectJump)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                CLickOnScreen();
            }
        }
        
    }

    private void CLickOnScreen()
    {
        if (_sliderTexts.Length - 1 == _indexOfSlide + 1)
        {
            StartCoroutine(BackToMenuWithDelay(2));
        }

        if (_sliderTexts.Length - 1 < _indexOfSlide + 1)
        {
            return;
        }

        _sliderTexts[_indexOfSlide].gameObject.SetActive(false);
        _indexOfSlide++;
        _sliderTexts[_indexOfSlide].gameObject.SetActive(true);

        if (_slideIndexToCheckForCorrectJumpToCard == _indexOfSlide)
        {
            _cardsActions.CreateFirstCard(2);
            PrepareForJump(1);

        } else if (_slideIndexToCheckForCorrectJumpToSpecialCard == _indexOfSlide)
        {
            _cardsActions.CreateSpecialCard(true, 0);
            PrepareForJump(2);
        } else if (_slideIndexToCreateDarkJoker == _indexOfSlide)
        {
            _cardsActions.CreateSpecialCard(false, 0);
        } else if (_slideIndexToCreateDarkJoker + 3 == _indexOfSlide)
        {
            Destroy(CurrentCardState.Instance.CurrentCard);
        }
    }

    private IEnumerator StartCheckingForCorrectJump()
    {
        yield return new WaitForSeconds(1);
        while (true)
        {
            _jokerActions.CheckForCorrectJump(CurrentCardState.Instance.IndexOfSectorPosition, _actionForCurrectJump, _actionForCurrectJumpToOtherSide);
            yield return null;
        }
    }

    private IEnumerator BackToMenuWithDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _sliderTexts[_indexOfSlide].gameObject.SetActive(false);

        _gameModeManager.ShowMainMenu();
        _gameModeManager.HideTutorial();
        _gameModeManager.HideGame();
    }

    private void PrepareForJump(int animationNumberForHandler)
    {
        _handler.transform.position = new Vector3(_jokerActions.gameObject.transform.position.x + (animationNumberForHandler%2 * 0.05f) + 0.15f, _jokerActions.gameObject.transform.position.y - (((animationNumberForHandler % 2 == 0) ? 1 : 0) * 0.2f), _handler.transform.position.z);
        _handler.SetActive(true);
        //_handler.GetComponentInChildren<Animator>().SetInteger("animationNumber", animationNumberForHandler);
        StartCoroutine(MoveObjectToCurrentCard(_handler, (animationNumberForHandler * 0.1f) + 0.4f, _handler.transform.position));

        _jokerActions.MakeJokerBright(true);
        _permissionTo—heckForCorrectJump = true;
        _startCheckingForCorrectJump = StartCoroutine(StartCheckingForCorrectJump());
    }

    private IEnumerator MoveObjectToCurrentCard(GameObject myObject, float movingTime, Vector2 startedPosition, float t = 0)
    {
        yield return null;
        if (CurrentCardState.Instance.CurrentCard != null && myObject.active)
        {
            if (t <= 1.0f)
            {
                t += (Time.deltaTime / movingTime);

                //Debug.Log(myObject.transform.position + "  " + CurrentCardState.Instance.CurrentCard.transform.position);
                Vector2 finalPos = CurrentCardState.Instance.CurrentCard.transform.position;
                float positionDeviationY = 1.2f;
                float positionDeviationX = 0.2f;
                if (startedPosition.y < finalPos.y)
                {
                    positionDeviationY = 0.3f;
                }
                Vector2 newPosition = Vector2.Lerp(startedPosition, new Vector2(finalPos.x + positionDeviationX, finalPos.y + positionDeviationY), t);

                myObject.transform.position = new Vector3(newPosition.x, newPosition.y, myObject.transform.position.z);

                StartCoroutine(MoveObjectToCurrentCard(myObject, movingTime, startedPosition, t));
            }
            else
            {
                yield return new WaitForSeconds(0.4f);
                StartCoroutine(MoveObjectToCurrentCard(myObject, movingTime, startedPosition));
                
            }
        }
    }

}
