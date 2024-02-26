using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopGameMusicInMenu : MonoBehaviour
{
    private void OnEnable()
    {
        AudioManager.Instance.StopGameMusic();
    }

}
