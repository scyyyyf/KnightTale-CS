using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SkillType
{
    AttackPower,
    AttackSpeed,
    AttackRange
}

public class SkillBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public SkillType skillType;
    public void UpdateSkillLevel(SkillType skillType, int level)
    {
        if (this.skillType == skillType)
        {
            int previousLevel = (int)slider.value; 
            slider.value = level;
            fill.color = gradient.Evaluate(slider.normalizedValue);
            if (level > previousLevel)
            {
                StartCoroutine(BlinkEffect(0.2f, 3)); 
            }
        }
    }
    private IEnumerator BlinkEffect(float duration, int count)
    {
        Color originalColor = fill.color; 
        Color blinkColor = Color.white; 

        for (int i = 0; i < count; i++)
        {
            fill.color = blinkColor;
            yield return new WaitForSeconds(duration / 2);
            fill.color = originalColor;
            yield return new WaitForSeconds(duration / 2);
        }
    }
}
