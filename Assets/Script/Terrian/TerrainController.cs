using System;
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
    }

    [SerializeField] RuleTileData[] _ruleTileData = null;

    protected override void OnSS()
    {
        foreach (var ruleTileData in _ruleTileData)
        {
            if (ruleTileData.Tilemap == null || ruleTileData.RuleSS == null || ruleTileData.RuleTD == null) continue;
            ruleTileData.Tilemap.SwapTile(ruleTileData.RuleTD, ruleTileData.RuleSS);
        }
    }

    protected override void OnTD()
    {
        foreach (var ruleTileData in _ruleTileData)
        {
            if (ruleTileData.Tilemap == null || ruleTileData.RuleSS == null || ruleTileData.RuleTD == null) continue;
            ruleTileData.Tilemap.SwapTile(ruleTileData.RuleSS, ruleTileData.RuleTD);
        }
    }
}