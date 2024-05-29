using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class PlayerInputs : MonoBehaviour
{
    [Header("Input Properties")]
    public float InputHorizontal = 0f;
    public bool InputJump = false;
    public bool InputJumpHeld = false;
    private bool ReadyToClear = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CleanInputs();

        ProcessInputs();

        InputHorizontal = Mathf.Clamp(InputHorizontal, -1, 1);//将给定值限制在给定的最小浮点值和最大浮点值之间
    }

    private void FixedUpdate()//循环更新
    {
        ReadyToClear = true;
    }

    private void ProcessInputs()
    {
        InputHorizontal += Input.GetAxis("Horizontal");
        InputJump = InputJump || Input.GetKeyDown(KeyCode.Space);
        InputJumpHeld = InputJumpHeld || Input.GetKey(KeyCode.Space);
    }

    private void CleanInputs()
    {
        if (ReadyToClear == false)
            return;//结束函数并返回

        InputHorizontal = 0f;
        InputJump = false;
        InputJumpHeld = false;

        ReadyToClear = false;
    }
}
