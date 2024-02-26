using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;


public class Shopping : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation _jokerSkeleton;

    [SerializeField] private GameObject[] _jokerSkinsButtons;

    [SerializeField] private TextMeshProUGUI[] _textMeshProCostJokerSkins;

    [SerializeField] private SkeletonDataAsset[] _skeletonsJokerSkins;

    [SerializeField] private GameObject _buttonScoreDoubledAbiliryForJokerCard;

    [SerializeField] private TextMeshProUGUI _textMeshProCostScoreDoubledAbility;

    [SerializeField] private GameObject _buttonStopTimerAbiliryForJokerCard;
    [SerializeField] private TextMeshProUGUI _textMeshProCostStopTimerAbility;

    [SerializeField] private TextMeshProUGUI _textMeshProCountOfMoney;

    private void OnEnable()
    {
        _textMeshProCountOfMoney.SetText(DataPersistenceManager.Instance._ShoppingData.CountMoney.ToString());
        CheckForOpportunityToBuy();
    }

    public void CheckForJokerSkin()
    {
        for (int i = 0; i < _jokerSkinsButtons.Length; i++)
        {
            if (DataPersistenceManager.Instance._ShoppingData.JokerSkins[i])
            {
                Button button = _jokerSkinsButtons[i].GetComponent<Button>();
                if (DataPersistenceManager.Instance._ShoppingData.IndexUsedSkin == i)
                {

                    _jokerSkeleton.skeletonDataAsset = _skeletonsJokerSkins[i];
                    _jokerSkeleton.Initialize(true);

                    SetButtonInteractable(button, false);

                    _jokerSkinsButtons[i].GetComponentInChildren<TextMeshProUGUI>().SetText("Used");
                }
                else
                {
                    SetButtonInteractable(button, true);
                    _jokerSkinsButtons[i].GetComponentInChildren<TextMeshProUGUI>().SetText("Use");
                }
            }
        }
    }

    public void SetShoppingPrice()
    {
        for (int i = 0; i < _textMeshProCostJokerSkins.Length; i++)
        {
            _textMeshProCostJokerSkins[i].SetText(DataPersistenceManager.Instance._ShoppingData.SkinsCost[i].ToString());
        }
        _textMeshProCostScoreDoubledAbility.SetText(DataPersistenceManager.Instance._ShoppingData.CostScoreDoubledAbiliry.ToString());
        _textMeshProCostStopTimerAbility.SetText(DataPersistenceManager.Instance._ShoppingData.CostStopTimerAbiliry.ToString());
    }


    public void SetScoreDoubledAbiliryForJokerCard (bool scoreDoubledAbiliry)
    {
        DataPersistenceManager.Instance._ShoppingData.ScoreDoubledAbiliryForJokerCard = scoreDoubledAbiliry;
        DataPersistenceManager.Instance._ShoppingData.CountMoney = DataPersistenceManager.Instance._ShoppingData.CountMoney - DataPersistenceManager.Instance._ShoppingData.CostScoreDoubledAbiliry;

        Button button = _buttonScoreDoubledAbiliryForJokerCard.GetComponent<Button>();

        SetButtonInteractable(button, false);

        CheckForOpportunityToBuy();
    }

    public void SetStopTimerAbiliryForJokerCard(bool stopTimerAbiliry)
    {
        DataPersistenceManager.Instance._ShoppingData.StopTimerAbiliryForJokerCard = stopTimerAbiliry;
        DataPersistenceManager.Instance._ShoppingData.CountMoney = DataPersistenceManager.Instance._ShoppingData.CountMoney - DataPersistenceManager.Instance._ShoppingData.CostStopTimerAbiliry;

        Button button = _buttonStopTimerAbiliryForJokerCard.GetComponent<Button>();
        SetButtonInteractable(button, false);

        CheckForOpportunityToBuy();
    }

    public void SetJokerSkinByIndex(int indexSkin)
    {
        if (!DataPersistenceManager.Instance._ShoppingData.JokerSkins[indexSkin])
        {
            DataPersistenceManager.Instance._ShoppingData.CountMoney = DataPersistenceManager.Instance._ShoppingData.CountMoney - DataPersistenceManager.Instance._ShoppingData.SkinsCost[indexSkin];
            DataPersistenceManager.Instance._ShoppingData.JokerSkins[indexSkin] = true;
        }
        DataPersistenceManager.Instance._ShoppingData.IndexUsedSkin = indexSkin;

        CheckForOpportunityToBuy();
        CheckForJokerSkin();
    }

    private void CheckForOpportunityToBuy()
    {
        for (int i = 0; i < DataPersistenceManager.Instance._ShoppingData.JokerSkins.Length; i++)
        {
            if (_jokerSkinsButtons != null && _jokerSkinsButtons.Length != 0)
            {
                Button jokerSkinButton = _jokerSkinsButtons[i].GetComponent<Button>();
                if (!DataPersistenceManager.Instance._ShoppingData.JokerSkins[i])
                {
                    if (DataPersistenceManager.Instance._ShoppingData.CountMoney < DataPersistenceManager.Instance._ShoppingData.SkinsCost[i])
                    {
                        SetButtonInteractable(jokerSkinButton, false);
                    }
                    else
                    {
                        SetButtonInteractable(jokerSkinButton, true);
                    }
                }
            }
        }

        Button buttonStopTimerAbiliry = _buttonStopTimerAbiliryForJokerCard.GetComponent<Button>();
        if (!DataPersistenceManager.Instance._ShoppingData.StopTimerAbiliryForJokerCard)
        {
            if (DataPersistenceManager.Instance._ShoppingData.CountMoney < DataPersistenceManager.Instance._ShoppingData.CostStopTimerAbiliry)
            {
                SetButtonInteractable(buttonStopTimerAbiliry, false);
            }
            else
            {
                SetButtonInteractable(buttonStopTimerAbiliry, true);
            }
        }
        else
        {
            SetButtonInteractable(buttonStopTimerAbiliry, false);
            buttonStopTimerAbiliry.GetComponentInChildren<TextMeshProUGUI>().SetText("Used");
        }

        Button buttonScoreDoubledAbiliry = _buttonScoreDoubledAbiliryForJokerCard.GetComponent<Button>();
        if (!DataPersistenceManager.Instance._ShoppingData.ScoreDoubledAbiliryForJokerCard)
        {
            if (DataPersistenceManager.Instance._ShoppingData.CountMoney < DataPersistenceManager.Instance._ShoppingData.CostScoreDoubledAbiliry)
            {
                SetButtonInteractable(buttonScoreDoubledAbiliry, false);
            }
            else
            {
                SetButtonInteractable(buttonScoreDoubledAbiliry, true);
            }
        }
        else
        {
            SetButtonInteractable(buttonScoreDoubledAbiliry, false);
            buttonScoreDoubledAbiliry.GetComponentInChildren<TextMeshProUGUI>().SetText("Used");
        }

        _textMeshProCountOfMoney.SetText(DataPersistenceManager.Instance._ShoppingData.CountMoney.ToString());
    }



    private void SetButtonInteractable(Button button, bool interactable)
    {
        button.interactable = interactable;
        /*ColorBlock colors = button.colors;
/*        if (interactable)
        {
            colors.normalColor = Color.white;
        }
        else
        {
            colors.normalColor = Color.gray;
        }
        button.colors = colors;*/
    }


    
}
