using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatingAchievements : MonoBehaviour
{
    [SerializeField] private GameObject _prefabAchievement;
    [SerializeField] private GameObject _parentAchievement;
    private List<FillDataOfAchievement> fillData = new List<FillDataOfAchievement>();
    private bool _achieveCreated = false;

    private void OnEnable()
    {
        if (!_achieveCreated)
        {
            _achieveCreated = true;

            for (int i = 0; i < DataPersistenceManager.Instance._AchievementsData._achievements.Count; i++) 
            {
                GameObject instantiatedObject = Instantiate(_prefabAchievement);
                fillData.Add(instantiatedObject.GetComponent<FillDataOfAchievement>());
                fillData[i].FillText(DataPersistenceManager.Instance._AchievementsData._achievements[i].NameAchievement, DataPersistenceManager.Instance._AchievementsData._achievements[i].DescriptionAchievement);
                fillData[i].SetCheckmark(DataPersistenceManager.Instance._AchievementsData._achievements[i]._completeAchievement);
                
                instantiatedObject.transform.SetParent(_parentAchievement.transform, false);
            }
            
        } else
        {
            for (int i = 0; i < fillData.Count; i++)
            {
                fillData[i].GetComponent<FillDataOfAchievement>().SetCheckmark(DataPersistenceManager.Instance._AchievementsData._achievements[i]._completeAchievement);
            }
        }
    }
}
