using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardSprites", menuName = "ScriptableObjects/CardSprites", order = 1)]
public class CardSprites : ScriptableObject
{
    [SerializeField] private Sprite[] _cardSpritesDiamonds;
    [SerializeField] private Sprite[] _cardSpritesHearts;
    [SerializeField] private Sprite[] _cardSpritesSpades;
    [SerializeField] private Sprite[] _cardSpritesClubs;
    [SerializeField] private Sprite _lightJoker;
    [SerializeField] private Sprite _darkJoker;

    private Sprite[,] _myCardSprites;

    private void OnEnable()
    {
        _myCardSprites = CombineArrays(_cardSpritesDiamonds, _cardSpritesHearts, _cardSpritesSpades, _cardSpritesClubs);
    }

    private Sprite[,] CombineArrays(params Sprite[][] arrays)
    {
        var rows = arrays.Length;
        var cols = arrays[0].Length;

        Sprite[,] result = new Sprite[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                result[i, j] = arrays[i][j];
            }
        }

        return result;
    }
    public Sprite LightJoker { get => _lightJoker; }
    public Sprite DarkJoker { get => _darkJoker; }
    public Sprite[,] MyCardSprites { get => _myCardSprites; }
}
