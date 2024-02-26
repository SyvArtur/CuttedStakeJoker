using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingMainScene : MonoBehaviour
{
    [SerializeField] private Slider slider;

    void Start()
    {
        StartCoroutine(LoadSceneAsync("MainScene"));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
/*        for (int i = 0; i <= 90; i++)
        {
            yield return new WaitForSeconds(0.04f);
            float progressValue = i / 100f;
            slider.value = progressValue;
        }*/

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f); 
            slider.value = progressValue;
            yield return null; 
        }
    }
}
