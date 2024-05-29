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
        // ���÷�����ɫ��ǿ��
        glowMaterial.SetColor("_EmissionColor", glowColor * intensity);
    }

    // ������Ҫ���·���Ч��
    void UpdateGlow(Color newColor, float newIntensity)
    {
        glowMaterial.SetColor("_EmissionColor", newColor * newIntensity);
    }
}
