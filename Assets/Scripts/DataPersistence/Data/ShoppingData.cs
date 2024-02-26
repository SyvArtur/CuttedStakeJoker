using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShoppingData
{
    [SerializeField] private bool[] jokerSkins;
    [SerializeField] private int[] _skinsCost;
    [SerializeField] private int indexUsedSkin;

    [SerializeField] private bool scoreDoubledAbiliryForJokerCard;
    [SerializeField] private int costScoreDoubledAbiliry;

    [SerializeField] private bool stopTimerAbiliryForJokerCard;
    [SerializeField] private int costStopTimerAbiliry;

    [SerializeField] private int countMoney;

    public int[] SkinsCost { get => _skinsCost; }
    public bool[] JokerSkins { get => jokerSkins; set => jokerSkins = value; }
    public int IndexUsedSkin { get => indexUsedSkin; set => indexUsedSkin = value; }
    public bool ScoreDoubledAbiliryForJokerCard { get => scoreDoubledAbiliryForJokerCard; set => scoreDoubledAbiliryForJokerCard = value; }
    public int CostScoreDoubledAbiliry { get => costScoreDoubledAbiliry; set => costScoreDoubledAbiliry = value; }
    public bool StopTimerAbiliryForJokerCard { get => stopTimerAbiliryForJokerCard; set => stopTimerAbiliryForJokerCard = value; }
    public int CostStopTimerAbiliry { get => costStopTimerAbiliry; set => costStopTimerAbiliry = value; }
    public int CountMoney { get => countMoney; set => countMoney = value; }

    public ShoppingData()
    {
        int countOfSkins = 1;
        JokerSkins = new bool[countOfSkins];
        JokerSkins[0] = true;

        _skinsCost = new int[countOfSkins];
        _skinsCost[0] = 0;
/*        _skinsCost[1] = 10000;
        _skinsCost[2] = 20000;*/

        IndexUsedSkin = 0;

        ScoreDoubledAbiliryForJokerCard = false;
        StopTimerAbiliryForJokerCard = false;

        CostScoreDoubledAbiliry = 15000;
        CostStopTimerAbiliry = 15000;
        CountMoney = 0;
    }
}
