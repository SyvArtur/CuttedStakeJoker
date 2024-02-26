using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusicManager : MonoBehaviour
{
    private void OnEnable()
    {
        AudioManager.Instance.StartOverGameMusic();
    }
}
