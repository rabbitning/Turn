using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainController : EffectByViewChange
{
    [Serializable]
    struct RuleTileData
    {
        public Tilemap Tilemap;
        public RuleTile RuleSS;
        public AdvancedRuleOverrideTile RuleTD;
        public readonly bool IsNull => Tilemap == null || RuleSS == null || RuleTD == null;
    }

    [SerializeField][Min(1)] private float maxRadius = 5f;
    [SerializeField] RuleTileData[] _ruleTileData = null;

    protected override void ViewChanged(bool isSS)
    {
        base.ViewChanged(isSS);
        foreach (RuleTileData ruleTileData in _ruleTileData)
        {
            if (ruleTileData.IsNull) continue;
            StartCoroutine(ChangeTiles(ruleTileData));
        }
    }

    IEnumerator ChangeTiles(RuleTileData ruleTileData)
    {
        Vector3Int playerPosition = Vector3Int.FloorToInt(PlayerController.Instance.transform.position);
        float elapsedTime = 0f;
        float effectDuration = GameManager.Instance.ViewChangeCooldown;

        float radiusStep = maxRadius / effectDuration;

        while (elapsedTime <= effectDuration)
        {
            float radius = elapsedTime * radiusStep;
            SwitchTilesInRadius(ruleTileData, playerPosition, radius);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        BoundsInt tilemapBounds = ruleTileData.Tilemap.cellBounds;
        TileBase[] allTilesInBounds = ruleTileData.Tilemap.GetTilesBlock(tilemapBounds);

        List<Vector3Int> tilePositions = new();
        List<TileBase> tilesToReplace = new();

        for (int x = 0; x < tilemapBounds.size.x; x++)
        {
            for (int y = 0; y < tilemapBounds.size.y; y++)
            {
                Vector3Int currentPosition = new(tilemapBounds.xMin + x, tilemapBounds.yMin + y, 0);
                TileBase currentTile = allTilesInBounds[x + y * tilemapBounds.size.x];
                if (currentTile == null || IsSS && currentTile == ruleTileData.RuleSS || !IsSS && currentTile == ruleTileData.RuleTD) continue;

                tilePositions.Add(currentPosition);
                tilesToReplace.Add(IsSS ? ruleTileData.RuleSS : ruleTileData.RuleTD);

                if (tilePositions.Count >= 600)
                {
                    ruleTileData.Tilemap.SetTiles(tilePositions.ToArray(), tilesToReplace.ToArray());
                    tilePositions.Clear();
                    tilesToReplace.Clear();
                    yield return null;
                }
            }
        }

        if (tilePositions.Count > 0) ruleTileData.Tilemap.SetTiles(tilePositions.ToArray(), tilesToReplace.ToArray());
        ruleTileData.Tilemap.CompressBounds();
    }

    void SwitchTilesInRadius(RuleTileData ruleTileData, Vector3Int center, float radius)
    {
        int ceilRadius = Mathf.CeilToInt(radius);
        for (int x = -ceilRadius; x <= ceilRadius; x++)
        {
            for (int y = -ceilRadius; y <= ceilRadius; y++)
            {
                Vector3Int tilePosition = new(center.x + x, center.y + y, center.z);
                if ((tilePosition - center).sqrMagnitude <= radius * radius)
                {
                    TileBase tile = ruleTileData.Tilemap.GetTile(tilePosition);
                    if (tile == null || IsSS && tile == ruleTileData.RuleSS || !IsSS && tile == ruleTileData.RuleTD) continue;

                    ruleTileData.Tilemap.SetTile(tilePosition, IsSS ? ruleTileData.RuleSS : ruleTileData.RuleTD);
                }
            }
        }
    }
}