using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsDelayDestory : MonoBehaviour
{
    public float time = 0.15f;

    void Start()
    {
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
