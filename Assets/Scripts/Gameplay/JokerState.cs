using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

public class JokerState : MonoBehaviour
{
    private static JokerState _instance;
    public static JokerState Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<JokerState>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("MyJoker");
                    _instance = singletonObject.AddComponent<JokerState>();
                }

                //DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    [SerializeField] private GameObject _joker;
    [SerializeField] public Positions _positions;

    private int _indexOfSectorPosition;

    private bool _lowerBankOfRiver;

    private int _score;

    private bool _inJump;

    [SerializeField] private SkeletonAnimation _jokerSkeletonAnimation;

    private void OnEnable()
    {
        IndexOfSectorPosition = 1;
        LowerBankOfRiver = true;
        Score = 0;
        JokerObject.transform.position = _positions.GetPositionForJoker(LowerBankOfRiver, IndexOfSectorPosition);
        JokerObject.transform.localScale = new Vector3(0.30f, 0.30f, 1);
        _jokerSkeletonAnimation.AnimationState.SetAnimation(0, "stay", true);
        InJump = false;
    }

    public int IndexOfSectorPosition { get => _indexOfSectorPosition; set => _indexOfSectorPosition = value; }
    public bool LowerBankOfRiver { get => _lowerBankOfRiver; set => _lowerBankOfRiver = value; }
    public GameObject JokerObject { get => _joker; }
    public int Score { get => _score; set => _score = value; }
    public bool InJump { get => _inJump; set => _inJump = value; }

    public SkeletonAnimation JokerSkeletonAnimation
    {
        get => _jokerSkeletonAnimation;
        set => _jokerSkeletonAnimation = value;
    }
}
