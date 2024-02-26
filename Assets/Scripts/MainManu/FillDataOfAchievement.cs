
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FillDataOfAchievement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textName;
    [SerializeField] private TextMeshProUGUI _textDescription;
    [SerializeField] private Image _checkmarkComponent;

    public void FillText(string nameOfAchievement, string descriptionOfAchievement)
    {
        _textName.SetText(nameOfAchievement);
        _textDescription.SetText(descriptionOfAchievement);

        StartCoroutine(SetObjectSizeThroughLines());
    }

    private IEnumerator SetObjectSizeThroughLines()
    {
        int currentLineCount = 0;
        while (currentLineCount == 0)
        {
            currentLineCount = _textName.textInfo.lineCount;
            float newHeight = 120 + ((currentLineCount - 1) * 100)/* + (2 * padding)*/;

            var RectTransform = _textName.gameObject.GetComponent<RectTransform>();
            RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, newHeight);
            yield return new WaitForFixedUpdate();
        }
    }

    public void SetCheckmark(bool achievementCompleted)
    {
        Color currentColor = _checkmarkComponent.color;

        if (!achievementCompleted) {
            currentColor.a = 0;
        } else
        {
            currentColor.a = 1;
        }

        _checkmarkComponent.color = currentColor;
    }

}
