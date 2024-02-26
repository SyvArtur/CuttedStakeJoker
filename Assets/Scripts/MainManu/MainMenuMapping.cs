using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMapping : MonoBehaviour
{
    [SerializeField] private GameObject[] _mainMenuControls;
    [SerializeField] private GameObject[] _achievementsControls;
    [SerializeField] private GameObject[] _optionsControls;
    [SerializeField] private GameObject[] _recordsControls;
    [SerializeField] private GameObject[] _shopControls;

    public void SetActiveForMainMenuControls(bool active)
    {
        foreach (GameObject mainMenuControl in _mainMenuControls)
        {
            mainMenuControl.SetActive(active);
        }
    }

    public void SetActiveForAchievementsControls(bool active)
    {
        foreach (GameObject achievementsControl in _achievementsControls)
        {
            achievementsControl.SetActive(active);
        }
    }

    public void SetActiveForOptionsControls(bool active)
    {
        foreach (GameObject optionControl in _optionsControls)
        {
            optionControl.SetActive(active);
        }
    }

    public void SetActiveForRecordsControls(bool active)
    {
        foreach (GameObject recordControl in _recordsControls)
        {
            recordControl.SetActive(active);
        }
    }

    public void SetActiveForShopControls(bool active)
    {
        foreach (GameObject shopControl in _shopControls)
        {
            shopControl.SetActive(active);
        }
    }
}
