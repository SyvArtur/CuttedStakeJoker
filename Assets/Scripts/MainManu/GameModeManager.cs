using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    [SerializeField] private GameObject _menuCanvas;
    [SerializeField] private GameObject _game;
    [SerializeField] private GameObject _gameplay;
    [SerializeField] private GameObject _tutorial;

    private static GameModeManager _instance;

    public static GameModeManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameModeManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("GameModeManager");
                    _instance = singletonObject.AddComponent<GameModeManager>();
                }

                //DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    public void ShowMainMenu()
    {
        _menuCanvas.SetActive(true);
    }

    public void HideMainMenu()
    {
        _menuCanvas.SetActive(false);
    }

    public void ShowGame()
    {
        _game.SetActive(true);
        _gameplay.SetActive(true);
    }

    public void HideGame()
    {
        _game.SetActive(false);
        _gameplay.SetActive(false);
    }

    public void ShowTutorial()
    {
        _game.SetActive(true);
        _tutorial.SetActive(true);
    }

    public void HideTutorial()
    {
        _game.SetActive(false);
        _tutorial.SetActive(false);
    }

    public bool GetGameplayActive()
    {
        return _gameplay.activeSelf;
    }
}
