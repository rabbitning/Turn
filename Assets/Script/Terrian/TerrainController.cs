using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainController : MonoBehaviour
{
    [Header("Ground")]
    [SerializeField] Tilemap ground = null;
    [SerializeField] RuleTile sideViewGround = null;
    [SerializeField] RuleTile topViewGround = null;

    [Space(10)]
    [Header("Wall")]

    [SerializeField] Tilemap wall = null;
    [SerializeField] RuleTile sideViewWall = null;
    [SerializeField] RuleTile topViewWall = null;
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
            foreach (var pos in ground.cellBounds.allPositionsWithin)
            {
                if (ground.GetTile(pos))
                    ground.SetTile(pos, sideViewGround);
            }
            foreach (var pos in wall.cellBounds.allPositionsWithin)
            {
                if (wall.GetTile(pos))
                    wall.SetTile(pos, sideViewWall);
            }
        }
        else
        {
            foreach (var pos in ground.cellBounds.allPositionsWithin)
            {
                if (ground.GetTile(pos))
                    ground.SetTile(pos, topViewGround);
            }
            foreach (var pos in wall.cellBounds.allPositionsWithin)
            {
                if (wall.GetTile(pos))
                    wall.SetTile(pos, topViewWall);
            }
        }
    }
}
