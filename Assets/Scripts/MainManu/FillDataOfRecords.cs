using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FillDataOfRecords : MonoBehaviour
{
    [SerializeField] private RecordObject[] _records;

    private void OnEnable()
    {
        for (int i = 0; i < _records.Length; i++)
        {
            if (DataPersistenceManager.Instance._RecordsData.Records[i].ScoreRecords > 0)
            {
                _records[i]._recordObject.SetActive(true);
                _records[i]._textScore.SetText("Score: " + DataPersistenceManager.Instance._RecordsData.Records[i].ScoreRecords);
                _records[i]._textTime.SetText("Time: " + DataPersistenceManager.Instance._RecordsData.Records[i].TimeOfRecord);
            } else
            {
                _records[i]._recordObject.SetActive(false);
            }
        }
    }

    [Serializable]
    public class RecordObject
    {
        public GameObject _recordObject;
        public TextMeshProUGUI _textScore;
        public TextMeshProUGUI _textTime;
    }
}
