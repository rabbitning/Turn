using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainController : EffectByViewChange
{
    [Header("Ground")]
    [SerializeField] Tilemap _groundTilemap = null;
    [SerializeField] RuleTile _groundRuleTileCurrent = null;
    [SerializeField] RuleTile _groundRuleTileSS = null;
    [SerializeField] RuleTile _groundRuleTileTD = null;

    [Space(10)]
    [Header("Wall")]

    [SerializeField] Tilemap _wallTilemap = null;
    [SerializeField] RuleTile _wallRuleTileCurrent = null;
    [SerializeField] RuleTile _wallRuleTileSS = null;
    [SerializeField] RuleTile _wallRuleTileTD = null;

    // [Space(10)]
    // [Header("Switch Ground")]

    // [SerializeField] Tilemap _switchGroundTilemap = null;
    // [SerializeField] RuleTile _switchGroundRuleTileCurrent = null;
    // [SerializeField] RuleTile _switchGroundRuleTileSS = null;
    // [SerializeField] RuleTile _switchGroundRuleTileTD = null;

    // [Space(10)]
    // [Header("Switch Wall")]
    // [SerializeField] Tilemap _switchWallTilemap = null;
    // [SerializeField] RuleTile _switchWallRuleTileCurrent = null;
    // [SerializeField] RuleTile _switchWallRuleTileSS = null;
    // [SerializeField] RuleTile _switchWallRuleTileTD = null;

    protected override void ViewChanged(bool isSS)
    {
        base.ViewChanged(isSS);
        if (isSS)
        {
            _groundRuleTileCurrent.m_DefaultSprite = _groundRuleTileSS.m_DefaultSprite;
            _groundRuleTileCurrent.m_TilingRules = _groundRuleTileSS.m_TilingRules;

            _wallRuleTileCurrent.m_DefaultSprite = _wallRuleTileSS.m_DefaultSprite;
            _wallRuleTileCurrent.m_TilingRules = _wallRuleTileSS.m_TilingRules;

            // _switchGroundRuleTileCurrent.m_DefaultSprite = _switchGroundRuleTileSS.m_DefaultSprite;
            // _switchGroundRuleTileCurrent.m_TilingRules = _switchGroundRuleTileSS.m_TilingRules;

            // _switchWallRuleTileCurrent.m_DefaultSprite = _switchWallRuleTileSS.m_DefaultSprite;
            // _switchWallRuleTileCurrent.m_TilingRules = _switchWallRuleTileSS.m_TilingRules;
        }
        else
        {
            _groundRuleTileCurrent.m_DefaultSprite = _groundRuleTileTD.m_DefaultSprite;
            _groundRuleTileCurrent.m_TilingRules = _groundRuleTileTD.m_TilingRules;

            _wallRuleTileCurrent.m_DefaultSprite = _wallRuleTileTD.m_DefaultSprite;
            _wallRuleTileCurrent.m_TilingRules = _wallRuleTileTD.m_TilingRules;

            // _switchGroundRuleTileCurrent.m_DefaultSprite = _switchGroundRuleTileTD.m_DefaultSprite;
            // _switchGroundRuleTileCurrent.m_TilingRules = _switchGroundRuleTileTD.m_TilingRules;

            // _switchWallRuleTileCurrent.m_DefaultSprite = _switchWallRuleTileTD.m_DefaultSprite;
            // _switchWallRuleTileCurrent.m_TilingRules = _switchWallRuleTileTD.m_TilingRules;
        }

        _groundTilemap.RefreshAllTiles();
        _wallTilemap.RefreshAllTiles();
        // _switchGroundTilemap.RefreshAllTiles();
        // _switchWallTilemap.RefreshAllTiles();
    }
}