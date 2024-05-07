using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainController : MonoBehaviour
{
    [Header("Ground")]
    [SerializeField] Tilemap ground = null;
    [SerializeField] Tile sideViewGroundBotton = null;
    [SerializeField] Tile sideViewGroundBottonCorner = null;
    [SerializeField] Tile topViewGroundBotton = null;
    [SerializeField] Tile topViewGroundBottonCorner = null;

    [Space(10)]
    [Header("Wall")]

    [SerializeField] Tilemap wall = null;
    // [SerializeField] Tile sideViewWallBotton = null;
    // [SerializeField] Tile sideViewWallBottonCorner = null;
    // [SerializeField] Tile topViewWallBotton = null;
    // [SerializeField] Tile topViewWallBottonCorner = null;

    // [SerializeField] Color notFocusColor = Color.black;

    void Start()
    {
        GameManager.gameManager.OnViewChanged.AddListener(ViewChanged);

        ViewChanged(GameManager.gameManager.GetView());
    }

    // void Update()
    // {

    // }

    void ViewChanged(bool currentView)
    {
        if (currentView)
        {
            // ground.color = Color.white;
            // wall.color = notFocusColor;
            foreach (var pos in ground.cellBounds.allPositionsWithin)
            {
                if (ground.GetTile(pos) && !ground.GetTile(pos + Vector3Int.down))
                {
                    if (!ground.GetTile(pos + Vector3Int.left) || !ground.GetTile(pos + Vector3Int.right))
                        ground.SetTile(pos, sideViewGroundBottonCorner);
                    else
                        ground.SetTile(pos, sideViewGroundBotton);
                }
            }
            // foreach (var pos in wall.cellBounds.allPositionsWithin)
            // {
            //     if (wall.GetTile(pos) && !wall.GetTile(pos + Vector3Int.down))
            //     {
            //         if (!wall.GetTile(pos + Vector3Int.left) || !wall.GetTile(pos + Vector3Int.right))
            //             wall.SetTile(pos, sideViewWallBottonCorner);
            //         else
            //             wall.SetTile(pos, sideViewWallBotton);
            //     }
            // }
        }
        else
        {
            // ground.color = notFocusColor;
            // wall.color = Color.white;
            foreach (var pos in ground.cellBounds.allPositionsWithin)
            {
                if (ground.GetTile(pos) && !ground.GetTile(pos + Vector3Int.down))
                {
                    if (!ground.GetTile(pos + Vector3Int.left) || !ground.GetTile(pos + Vector3Int.right))
                        ground.SetTile(pos, topViewGroundBottonCorner);
                    else
                        ground.SetTile(pos, topViewGroundBotton);
                }
            }
            // foreach (var pos in wall.cellBounds.allPositionsWithin)
            // {
            //     if (wall.GetTile(pos) && !wall.GetTile(pos + Vector3Int.down))
            //     {
            //         if (!wall.GetTile(pos + Vector3Int.left) || !wall.GetTile(pos + Vector3Int.right))
            //             wall.SetTile(pos, topViewWallBottonCorner);
            //         else
            //             wall.SetTile(pos, topViewWallBotton);
            //     }
            // }
        }
    }
}
