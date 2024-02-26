using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideGameOnStart : MonoBehaviour
{
    [SerializeField] private GameObject _general;
    [SerializeField] private GameObject _game;
    [SerializeField] private GameObject _tutor;
    [SerializeField] private Button _buttonStart;
    [SerializeField] private Button _button;

    void Start()
    {
        //GameModeManager.Instance.HideMainMenu();
        GameModeManager.Instance.ShowGame();
        GameModeManager.Instance.HideGame();
        AudioManager.Instance.StopGameMusic();
        //GameModeManager.Instance.ShowMainMenu();
        //_buttonStart?.onClick.Invoke();
        //_button?.onClick.Invoke();
        //StartCoroutine(HideGameWithDelay(0.5f));
        //_button?.onClick.Invoke();
    }

    private IEnumerator HideGameWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameModeManager.Instance.HideGame();
        AudioManager.Instance.StopGameMusic();
    }

}
