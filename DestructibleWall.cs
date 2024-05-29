using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestructibleWall : MonoBehaviour
{
    public Tilemap destructibleTilemap;
    public GameObject explodeEffectPrefab;

    void Start()
    {
        if (destructibleTilemap == null)
        {
            destructibleTilemap = GetComponent<Tilemap>();
        }
    }

    public void DestroyTile(Vector3 worldPosition)
    {
        Vector3Int cellPosition = destructibleTilemap.WorldToCell(worldPosition);
        if (destructibleTilemap.HasTile(cellPosition))
        {
            destructibleTilemap.SetTile(cellPosition, null);
            Instantiate(explodeEffectPrefab, destructibleTilemap.GetCellCenterWorld(cellPosition), Quaternion.identity);
            RefreshTilemapCollider();
        }
    }
    void RefreshTilemapCollider()
    {
        TilemapCollider2D tilemapCollider = destructibleTilemap.gameObject.GetComponent<TilemapCollider2D>();
        if (tilemapCollider != null)
        {
            tilemapCollider.enabled = false;
            tilemapCollider.enabled = true;
        }
    }
}
