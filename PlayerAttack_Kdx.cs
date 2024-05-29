using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack_Kdx : MonoBehaviour
{
    [SerializeField] private bool ZHeld;
    public float ChargingTime = 0f;
    public float MinChargeTime = 1f;
    public float MaxChargeTime = 2f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Checking Player input
        ZHeld = false;
        ZHeld = ZHeld || Input.GetKey(KeyCode.Z);

        if (ZHeld)
        {
            ChargingTime += Time.deltaTime;
            Debug.Log("isCharging");
        }
        else if (!ZHeld && (ChargingTime > 0 && ChargingTime < MinChargeTime))
        {
            Debug.Log("normal Attack");
            ChargingTime = 0f;
        }
        else if (!ZHeld && ChargingTime > MaxChargeTime)
        {
            Debug.Log("Charged Attack");
            ChargingTime = 0f;
        }
        else if (!ZHeld && ChargingTime < MaxChargeTime && ChargingTime > MinChargeTime)
        {
            Debug.Log("normal Attack");
            ChargingTime = 0f;
        }
    }
}
