using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlatformController : MonoBehaviour
{
    PlatformEffector2D _platformEffector = null;
    float _sideViewSurfaceArc = 0;
    // Tile tile;

    void Start()
    {
        GameManager.Instance.OnViewChanged.AddListener(ViewChanged);

        _platformEffector = GetComponent<PlatformEffector2D>();

        _sideViewSurfaceArc = _platformEffector.surfaceArc;

        ViewChanged(GameManager.Instance.GetView());

        // tile = gameObject.GetComponent<Tile>();
    }

    void ViewChanged(bool isSS)
    {
        _platformEffector.surfaceArc = isSS ? _sideViewSurfaceArc : 360f;
    }
}
