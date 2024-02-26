using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class RecordsData
{
    public Record[] Records = new Record[3];

    public void CheckForNewRecord(Record record)
    {
        for (int i = 0; i < Records.Length; i++)
        {
            if (record.ScoreRecords >= Records[i].ScoreRecords)
            {
                int tempScore = Records[i].ScoreRecords;
                float tempTime = Records[i].TimeOfRecord;
                if (i + 1 < Records.Length)
                {
                    for (int j = i + 1; j < Records.Length; j++)
                    {
                        int myTempScore = Records[j].ScoreRecords;
                        float myTempTime = Records[j].TimeOfRecord;

                        Records[j].ScoreRecords = tempScore;
                        Records[j].TimeOfRecord = tempTime;

                        tempScore = myTempScore;
                        tempTime = myTempTime;
                    }
                }
                Records[i].ScoreRecords = record.ScoreRecords;
                Records[i].TimeOfRecord = record.TimeOfRecord;
                break;
            }
        }
    }

    public RecordsData()
    {
        for (int i = 0; i < Records.Length; i++)
        {
            Records[i] = new Record(0, 0);
        }
    }


    [System.Serializable]
    public class Record
    {
        public int _scoreRecords;
        public float _timeOfRecord;

        public Record(int scoreRecords, float timeOfRecord)
        {
            ScoreRecords = scoreRecords;
            TimeOfRecord = timeOfRecord;
        }

        public int ScoreRecords { get => _scoreRecords; set => _scoreRecords = value; }
        public float TimeOfRecord { get => _timeOfRecord; set => _timeOfRecord = value; }
    }
}
