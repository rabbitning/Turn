using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainController : MonoBehaviour
{
    [Header("Ground")]
    [SerializeField] Tilemap ground = null;
    [SerializeField] RuleTile sideViewGround = null;
    // [SerializeField] RuleTile topViewGround = null;
    // List<Vector3Int> groundAllTile = new List<Vector3Int>();
    List<Vector3Int> groundBottonTile = new List<Vector3Int>();
    [SerializeField] Sprite[] ssGroundBotton = new Sprite[3];
    [SerializeField] Sprite[] tdGroundBotton = new Sprite[1];

    [Space(10)]
    [Header("Wall")]

    [SerializeField] Tilemap wall = null;
    [SerializeField] RuleTile sideViewWall = null;
    // [SerializeField] RuleTile topViewWall = null;
    // List<Vector3Int> wallAllTile = new List<Vector3Int>();
    List<Vector3Int> wallBelowTile = new List<Vector3Int>();
    [SerializeField] Sprite[] ssWallBotton = new Sprite[3];
    [SerializeField] Sprite[] tdWallBotton = new Sprite[1];

    void Start()
    {
        GameManager.gameManager.OnViewChanged.AddListener(ViewChanged);

        // foreach (Vector3Int pos in ground.cellBounds.allPositionsWithin)
        //     if (ground.GetTile(pos))
        //         groundAllTile.Add(pos);

        // foreach (Vector3Int pos in ground.cellBounds.allPositionsWithin)
        //     if (ground.GetTile(pos) && !ground.GetTile(pos + Vector3Int.down))
        //         groundBottonTile.Add(pos);

        // foreach (Vector3Int pos in wall.cellBounds.allPositionsWithin)
        //     if (wall.GetTile(pos))
        //         wallAllTile.Add(pos);

        // foreach (Vector3Int pos in wall.cellBounds.allPositionsWithin)
        //     if (wall.GetTile(pos) && !wall.GetTile(pos + Vector3Int.down))
        //         wallBelowTile.Add(pos + Vector3Int.down);

        ViewChanged(GameManager.gameManager.GetView());
    }

    // void Update()
    // {

    // }

    void ViewChanged(bool currentView)
    {
        if (currentView)
        {
            for (int i = 1; i <= tdGroundBotton.Length; i++)
                sideViewGround.m_TilingRules[sideViewGround.m_TilingRules.Count - i].m_Sprites[0] = ssGroundBotton[ssGroundBotton.Length - i];

            // for (int i = 1; i <= tdWallBotton.Length; i++)
            // sideViewWall.m_TilingRules[sideViewWall.m_TilingRules.Count - i].m_Sprites[0] = ssWallBotton[ssWallBotton.Length - i];
            sideViewWall.m_TilingRules[sideViewWall.m_TilingRules.Count - 1].m_Sprites[0] = ssWallBotton[0];

            // foreach (Vector3Int pos in groundAllTile)
            //     ground.SetTile(pos, sideViewGround);

            // foreach (Vector3Int pos in wallAllTile)
            //     wall.SetTile(pos, sideViewWall);

            // foreach (Vector3Int pos in wallBelowTile)
            //     wall.SetTile(pos, null);
        }
        else
        {
            for (int i = 1; i <= tdGroundBotton.Length; i++)
                sideViewGround.m_TilingRules[sideViewGround.m_TilingRules.Count - i].m_Sprites[0] = tdGroundBotton[tdGroundBotton.Length - i];

            // for (int i = 1; i <= tdWallBotton.Length; i++)
            //     sideViewWall.m_TilingRules[sideViewWall.m_TilingRules.Count - i].m_Sprites[0] = tdWallBotton[tdWallBotton.Length - i];
            sideViewWall.m_TilingRules[sideViewWall.m_TilingRules.Count - 1].m_Sprites[0] = tdWallBotton[0];


            // foreach (Vector3Int pos in groundAllTile)
            //     ground.SetTile(pos, topViewGround);

            // foreach (Vector3Int pos in wallAllTile)
            //     wall.SetTile(pos, topViewWall);

            // foreach (Vector3Int pos in wallBelowTile)
            //     wall.SetTile(pos, topViewWall);
        }
        ground.RefreshAllTiles();
        wall.RefreshAllTiles();
    }
}
