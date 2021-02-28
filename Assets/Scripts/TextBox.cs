using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TextBox : MonoBehaviour
{
    public delegate void TextShownHandler();
    public event TextShownHandler TextShownEvent;

    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private RectTransform wipeScreen;
    [SerializeField] private float wipeScreenTime = 1f;
    [SerializeField] private float intervalBetweenCharacters = 0.05f;

    public void ShowText(string text)
    {
        ShowText(text, 0f);
    }

    public void ShowText(string text, float delay)
    {
        StartCoroutine(ClearAndShowText(text, delay));
    }

    public void ClearText()
    {
        StartCoroutine(WipeScreen());
    }

    private IEnumerator ClearAndShowText(string text, float delay)
    {
        yield return WipeScreen();

        yield return new WaitForSeconds(delay);

        yield return GraduallyShowText(text);
    }

    private IEnumerator WipeScreen()
    {
        if (!string.IsNullOrEmpty(textMesh.text))
        {
            float endTime = Time.time + wipeScreenTime;
            float progress = 0;
            while (progress < 1f)
            {
                wipeScreen.sizeDelta = new Vector2(progress * 800f, wipeScreen.sizeDelta.y);
                progress = 1 - ((endTime - Time.time) / wipeScreenTime);
                yield return null;
            }

            wipeScreen.sizeDelta = new Vector2(0f, wipeScreen.sizeDelta.y);
            textMesh.text = string.Empty;
        }
    }

    private IEnumerator GraduallyShowText(string text)
    {
        textMesh.text = text;
        textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 0);
        textMesh.ForceMeshUpdate();

        TMP_TextInfo textInfo = textMesh.textInfo;
        int totalCharacters = textMesh.textInfo.characterCount;
        int visibleCount = 0;
        Color32[] newVertexColors;

        while (visibleCount < totalCharacters)
        {
            int materialIndex = textInfo.characterInfo[visibleCount].materialReferenceIndex;
            newVertexColors = textInfo.meshInfo[materialIndex].colors32;
            int vertexIndex = textInfo.characterInfo[visibleCount].vertexIndex;

            newVertexColors[vertexIndex + 0].a = 255;
            newVertexColors[vertexIndex + 1].a = 255;
            newVertexColors[vertexIndex + 2].a = 255;
            newVertexColors[vertexIndex + 3].a = 255;

            visibleCount++;
            textMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            yield return new WaitForSeconds(intervalBetweenCharacters);
        }

        TextShownEvent?.Invoke();
    }
}
