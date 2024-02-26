using System;
using UnityEngine;

public class Positions : MonoBehaviour
{
    [SerializeField] private Transform[] _positionsForJokerLowerBank;
    [SerializeField] private Transform[] _positionsForJokerUpperBank;
    [SerializeField] private Transform[] _positionsForRiver;

/*    [SerializeField] private Transform[] _positionsForRiverLowerBank;
    [SerializeField] private Transform[] _positionsForRiverUpperBank;
    public Transform[] PositionsForRiverLowerBank { get => _positionsForRiverLowerBank; set => _positionsForRiverLowerBank = value; }
    public Transform[] PositionsForRiverUpperBank { get => _positionsForRiverUpperBank; set => _positionsForRiverUpperBank = value; }*/
    public Transform[] PositionsForJokerLowerBank { get => _positionsForJokerLowerBank; set => _positionsForJokerLowerBank = value; }
    public Transform[] PositionsForJokerUpperBank { get => _positionsForJokerUpperBank; set => _positionsForJokerUpperBank = value; }
    public Transform[] PositionsForRiver { get => _positionsForRiver; set => _positionsForRiver = value; }

    /*public Vector2[] PositionsForRiverLowerBank { get; private set; }
    public Vector2[] PositionsForRiverUpperBank { get; private set; }
    public Vector2[] PositionsForJokerLowerBank { get; private set; }
    public Vector2[] PositionsForJokerUpperBank { get; private set; }

        private void OnEnable()
        {
            PositionsForRiverLowerBank = GetPosition(_positionsForRiverLowerBank);

            PositionsForRiverUpperBank = GetPosition(_positionsForRiverUpperBank);

            PositionsForJokerLowerBank = GetPosition(_positionsForJokerLowerBank);
            PositionsForJokerUpperBank = GetPosition(_positionsForJokerUpperBank);
            Debug.Log(PositionsForJokerLowerBank[0]);
        }

        private Vector2[] GetPosition(Transform[] positionsFor)
        {
            Vector2[] xPositionFor = new Vector2[positionsFor.Length];
            for (int i = 0; i < positionsFor.Length; i++)
            {

                xPositionFor[i] = positionsFor[i].position;

            }
            return xPositionFor;
        }
        public Vector2 GetPositionForRiver(bool lowerBank, int indexPos)
        {
        Transform[] positions = PositionsForRiverLowerBank;
        if (!lowerBank)
        {
            positions = PositionsForRiverUpperBank;
        }

        return positions[indexPos].position;
    }*/

    public Vector2 GetStartPositionForRiver(int indexPos)
    {
        return PositionsForRiver[indexPos].position;
    }

    public Vector2 GetEstablishedPositionForRiver(int indexPos)
    {
        return PositionsForRiver[indexPos].position + new Vector3(0, 1f, 0);
    }

    public int GetRandomIndexPosForRiver()
    {
        int randomIndex = new System.Random().Next(PositionsForRiver.Length);

        return randomIndex;
    }

    public Vector2 GetPositionForJoker(bool lowerBank, int indexPos)
    {
        Transform[] positions = PositionsForJokerLowerBank;
        if (!lowerBank)
        {
            positions = PositionsForJokerUpperBank;
        }
        
        return positions[indexPos].position;
    }

/*    public int GetRandomIndexPosForJoker()
    {
        int randomIndex = new System.Random().Next(PositionsForJokerLowerBank.Length);

        return randomIndex;
    }*/
}