using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AchievementsData
{
    public List<Achievement> _achievements;

    [System.Serializable]
    public class Achievement
    {
        [SerializeField] private string _nameAchievement;
        [SerializeField] private string _descriptionAchievement;
        public string NameAchievement { get => _nameAchievement; }
        public string DescriptionAchievement { get => _descriptionAchievement; }

        public Achievement(string nameAchievement, string descriptionAchievement)
        {
            _nameAchievement = nameAchievement;
            _descriptionAchievement = descriptionAchievement;
            _completeAchievement = false;
        }

        public bool _completeAchievement;
    }

    public AchievementsData()
    {
        _achievements = new List<Achievement>();
        Achievement achievementNumberOne = new Achievement("Errorless Agility", "Catch a card 40 times in a row without any misses");
        Achievement achievementNumberTwo = new Achievement("Cardventure Time", "Without closing the game, spend one hour in it");
        Achievement achievementNumberThree = new Achievement("Insomniac Card Guard", "Play the game after 11 pm");
        Achievement achievementNumberFour = new Achievement("Seventh Heaven Leaps", "Complete 100 jumps in one match");
        _achievements.Add(achievementNumberOne);
        _achievements.Add(achievementNumberTwo);
        _achievements.Add(achievementNumberThree);
        _achievements.Add(achievementNumberFour);
    }
}
