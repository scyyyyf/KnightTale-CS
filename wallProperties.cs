using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallProperties : MonoBehaviour
{
    public GameObject objectToActivate;  

    public void ActivateObject()
    {
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);  
        }
    }
}
