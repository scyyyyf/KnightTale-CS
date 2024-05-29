using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D cursorTexture;  // �����������ק����������ֶ�
    public Vector2 hotSpot = Vector2.zero;  // �����ȵ�λ�ã�ͨ���ǹ������Ļ�����

    void Start()
    {
        SetCustomCursor();
    }

    void SetCustomCursor()
    {
        // ���ù��
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.ForceSoftware);
    }

    void OnDestroy()
    {
        // ����������ʱ�����ù��ΪĬ��
        Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
    }
}
