using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; 

public class HighlightButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color highlightColor = new Color(1f, 1f, 1f, 0.5f); 
    private Color normalColor;
    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
        if (image != null)
        {
            normalColor = image.color; 
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (image != null)
        {
            image.color = highlightColor; 
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (image != null)
        {
            image.color = normalColor; 
        }
    }
}
