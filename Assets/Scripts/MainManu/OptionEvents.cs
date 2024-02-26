using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class OptionEvents : MonoBehaviour
{
    [SerializeField] private Slider _sliderForSoundEffects;
    [SerializeField] private Slider _sliderForMusic;


    private void Start()
    {
        CheckSound();
    }


    void CheckSound()
    {
        if (DataPersistenceManager.Instance._SettingsData._muteSoundEffects)
        {
            AudioManager.Instance.SetVolumeToSoundEffectsSource(0);
            _sliderForSoundEffects.value = 0;
        }
        else
        {
            AudioManager.Instance.SetVolumeToSoundEffectsSource(AudioManager.Instance.DefaultVolumeValueForSoundEffects);
            _sliderForSoundEffects.value = 1;
        }

        if (DataPersistenceManager.Instance._SettingsData._muteMusic)
        {
            AudioManager.Instance.SetVolumeToMusicAudioSource(0);
            _sliderForMusic.value = 0;
        }
        else
        {
            AudioManager.Instance.SetVolumeToMusicAudioSource(AudioManager.Instance.DefaultVolumeValueForMusic);
            _sliderForMusic.value = 1;
        }
/*        EditorUtility.SetDirty(_playerInf);
        AssetDatabase.SaveAssets();*/
    }

    public void SetSoundEffects()
    {
        DataPersistenceManager.Instance._SettingsData._muteSoundEffects = !DataPersistenceManager.Instance._SettingsData._muteSoundEffects;
        _sliderForSoundEffects.value = 1 - _sliderForMusic.value;

        CheckSound();
    }


    public void SetMusic()
    {
        DataPersistenceManager.Instance._SettingsData._muteMusic = !DataPersistenceManager.Instance._SettingsData._muteMusic;
        _sliderForMusic.value = 1 - _sliderForMusic.value;

        CheckSound();
    }
}
