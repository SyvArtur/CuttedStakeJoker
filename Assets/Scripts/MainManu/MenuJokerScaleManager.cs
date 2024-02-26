using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuJokerScaleManager : MonoBehaviour
{
    [SerializeField] private GameObject _menuJoker;

    void Start()
    {
        //Debug.Log(Screen.height/(float)Screen.width);
        float scale = Screen.height / (float)Screen.width / 2f;
        _menuJoker.transform.localScale = new Vector3(scale, scale, 1);
    }

    void Update()
    {
        
    }
}
