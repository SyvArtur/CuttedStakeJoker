using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartedPriceAndSkin : MonoBehaviour
{
    [SerializeField] private Shopping PlayerShopping;


    void Awake()
    {
        PlayerShopping.CheckForJokerSkin();
        PlayerShopping.SetShoppingPrice();
    }

}
