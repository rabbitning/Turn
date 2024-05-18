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
    List<Vector3Int> groundAllTile = new List<Vector3Int>();

    [Space(10)]
    [Header("Wall")]

    [SerializeField] Tilemap wall = null;
    [SerializeField] RuleTile sideViewWall = null;
    [SerializeField] RuleTile topViewWall = null;
    List<Vector3Int> wallAllTile = new List<Vector3Int>();
    List<Vector3Int> wallBelowTile = new List<Vector3Int>();

    void Start()
    {
        GameManager.gameManager.OnViewChanged.AddListener(ViewChanged);

        foreach (Vector3Int pos in ground.cellBounds.allPositionsWithin)
            if (ground.GetTile(pos))
                groundAllTile.Add(pos);

        foreach (Vector3Int pos in wall.cellBounds.allPositionsWithin)
            if (wall.GetTile(pos))
                wallAllTile.Add(pos);

        foreach (Vector3Int pos in wall.cellBounds.allPositionsWithin)
            if (wall.GetTile(pos) && !wall.GetTile(pos + Vector3Int.down))
                wallBelowTile.Add(pos + Vector3Int.down);

        ViewChanged(GameManager.gameManager.GetView());
    }

    // void Update()
    // {

    // }

    void ViewChanged(bool currentView)
    {
        if (currentView)
        {
            foreach (Vector3Int pos in groundAllTile)
                ground.SetTile(pos, sideViewGround);

            foreach (Vector3Int pos in wallAllTile)
                wall.SetTile(pos, sideViewWall);

            foreach (Vector3Int pos in wallBelowTile)
                wall.SetTile(pos, null);
        }
        else
        {
            foreach (Vector3Int pos in groundAllTile)
                ground.SetTile(pos, topViewGround);

            foreach (Vector3Int pos in wallAllTile)
                wall.SetTile(pos, topViewWall);

            foreach (Vector3Int pos in wallBelowTile)
                wall.SetTile(pos, topViewWall);
        }
    }
}
