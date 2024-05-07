using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlatformController : MonoBehaviour
{
    PlatformEffector2D platformEffector = null;
    float sideViewSurfaceArc = 0;
    // Tile tile;

    void Start()
    {
        GameManager.gameManager.OnViewChanged.AddListener(ViewChanged);

        platformEffector = GetComponent<PlatformEffector2D>();

        sideViewSurfaceArc = platformEffector.surfaceArc;

        ViewChanged(GameManager.gameManager.GetView());

        // tile = gameObject.GetComponent<Tile>();
    }

    void ViewChanged(bool currentView)
    {
        platformEffector.surfaceArc = currentView ? sideViewSurfaceArc : 360f;
    }
}
