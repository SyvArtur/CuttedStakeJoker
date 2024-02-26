using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdatingShop : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshProCountOfMoney;
    //[SerializeField] private PlayerShopping PlayerShopping;

    private void OnEnable()
    {
        _textMeshProCountOfMoney.SetText(DataPersistenceManager.Instance._ShoppingData.CountMoney.ToString());
    }
}

