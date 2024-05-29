using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlinkingTitle : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public float transitionTime = 2.0f;   
    private void Start()
    {
        if (textMesh == null)
        {
            textMesh = GetComponent<TextMeshProUGUI>();
        }
        StartCoroutine(Breathe());
    }

    private IEnumerator Breathe()
    {
        Color startColor = Color.yellow;  
        Color endColor = Color.white;    
        float timer = 0;
        while (true)
        {
            while (timer < transitionTime)
            {
                textMesh.color = Color.Lerp(startColor, endColor, timer / transitionTime);
                timer += Time.deltaTime;
                yield return null;  
            }
            timer = 0;
            (startColor, endColor) = (endColor, startColor);
        }
    }
}