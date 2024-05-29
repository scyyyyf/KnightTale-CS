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

        InputHorizontal = Mathf.Clamp(InputHorizontal, -1, 1);//������ֵ�����ڸ�������С����ֵ����󸡵�ֵ֮��
    }

    private void FixedUpdate()//ѭ������
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
            return;//��������������

        InputHorizontal = 0f;
        InputJump = false;
        InputJumpHeld = false;

        ReadyToClear = false;
    }
}
