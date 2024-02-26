using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;


    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AudioManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("MyAudioManager");
                    _instance = singletonObject.AddComponent<AudioManager>();
                }

                //DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }

    }


    [SerializeField] private AudioSource _soundEffectsSource;

    [SerializeField] private AudioSource _musicAudioSource;

    [SerializeField] private AudioClip _gameMusic;
    [SerializeField] private AudioClip _menuButtonClick;
    [SerializeField] private AudioClip _lose;
    [SerializeField] private AudioClip _lvlUp;
    [SerializeField] private AudioClip _catchCard;
    [SerializeField] private AudioClip _goodJoker;

    private float _defaultVolumeValueForMusic;
    private float _defaultVolumeValueForSoundEffects;

    public float DefaultVolumeValueForMusic { get => _defaultVolumeValueForMusic; }
    public float DefaultVolumeValueForSoundEffects { get => _defaultVolumeValueForSoundEffects; }

    public void MenuButtonSound()
    {
        if (_menuButtonClick != null)
        {
            _soundEffectsSource.PlayOneShot(_menuButtonClick);
        }
    }

    public void CatchDarkJokerSound()
    {
        if (_lose != null)
        {
            _soundEffectsSource.PlayOneShot(_lose);
        }
    }

    public void LvlUpSound()
    {
        if (_lvlUp != null)
        {
            _soundEffectsSource.PlayOneShot(_lvlUp);
        }
    }

    public void CatchCardSound()
    {
        if (_catchCard != null)
        {
            _soundEffectsSource.PlayOneShot(_catchCard);
        }
    }

    public void CatchGoodJokerSound()
    {
        if (_goodJoker != null)
        {
            _soundEffectsSource.PlayOneShot(_goodJoker);
        }
    }

    public void StartOverGameMusic()
    {
        if (_gameMusic != null)
        {
            if (_musicAudioSource.isPlaying)
            {
                _musicAudioSource.Stop();
            }
            _musicAudioSource.Play();
        }
    }

    public void StopGameMusic()
    {
        if (_gameMusic != null)
        {
            _musicAudioSource.Stop();
        }
    }

    public void SetVolumeToSoundEffectsSource(float volume)
    {
        _soundEffectsSource.volume = volume;
    }

    public void SetVolumeToMusicAudioSource(float volume)
    {
        _musicAudioSource.volume = volume;
    }

    public float GetVolumeFromSoundEffectsSource()
    {
        return _soundEffectsSource.volume;
    }

    public float GetVolumeFromMusicAudioSource()
    {
        return _musicAudioSource.volume;
    }

    private void Start()
    {
        _musicAudioSource.clip = _gameMusic;
        _defaultVolumeValueForMusic = GetVolumeFromMusicAudioSource();
        _defaultVolumeValueForSoundEffects = GetVolumeFromSoundEffectsSource();
    }
}
