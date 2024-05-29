using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D cursorTexture;  // 将你的纹理拖拽到这个公共字段
    public Vector2 hotSpot = Vector2.zero;  // 光标的热点位置，通常是光标的中心或点击点

    void Start()
    {
        SetCustomCursor();
    }

    void SetCustomCursor()
    {
        // 设置光标
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.ForceSoftware);
    }

    void OnDestroy()
    {
        // 当物体销毁时，重置光标为默认
        Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
    }
}
