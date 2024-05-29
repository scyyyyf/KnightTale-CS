using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platform; 

    public void ActivatePlatform()
    {
        platform.SetActive(true);
    }
    public void ChangeMusic()
    {
        AudioManager.Instance.ChangeMusic("BGM-secondstage");
    }
}
