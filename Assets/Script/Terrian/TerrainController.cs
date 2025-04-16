using System;
using System.Collections;
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
            StartCoroutine(ShiftTiles(ruleTileData));
        }
    }

    IEnumerator ShiftTiles(RuleTileData ruleTileData)
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

        if (IsSS) ruleTileData.Tilemap.SwapTile(ruleTileData.RuleTD, ruleTileData.RuleSS);
        else ruleTileData.Tilemap.SwapTile(ruleTileData.RuleSS, ruleTileData.RuleTD);
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

                    if (IsSS) ruleTileData.Tilemap.SetTile(tilePosition, ruleTileData.RuleSS);
                    else ruleTileData.Tilemap.SetTile(tilePosition, ruleTileData.RuleTD);
                }
            }
        }
    }
}