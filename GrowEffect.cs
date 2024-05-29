using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowEffect : MonoBehaviour
{
    public Material glowMaterial;
    public Color glowColor = Color.white;
    public float intensity = 2.0f;

    void Start()
    {
        // 设置发光颜色和强度
        glowMaterial.SetColor("_EmissionColor", glowColor * intensity);
    }

    // 根据需要更新发光效果
    void UpdateGlow(Color newColor, float newIntensity)
    {
        glowMaterial.SetColor("_EmissionColor", newColor * newIntensity);
    }
}
