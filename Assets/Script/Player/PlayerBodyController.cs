using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyController : MonoBehaviour
{
    [HideInInspector] public BoxCollider2D col = null;
    [SerializeField] Vector2 sideViewColSize = Vector2.zero;
    [SerializeField] Vector2 sideViewColOffset = Vector2.zero;

    [Space(10)]

    [SerializeField] Vector2 topViewColSize = Vector2.zero;
    [SerializeField] Vector2 topViewColOffset = Vector2.zero;
    void Start()
    {
        GameManager.gameManager.OnViewChanged.AddListener(ViewChanged);

        col = GetComponent<BoxCollider2D>();

        ViewChanged(GameManager.gameManager.GetView());
    }

    void ViewChanged(bool currentView)
    {
        if (currentView)
        {
            col.size = sideViewColSize;
            col.offset = sideViewColOffset;
        }
        else
        {
            col.size = topViewColSize;
            col.offset = topViewColOffset;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (GameManager.gameManager && col)
        {
            if (GameManager.gameManager.GetView())
            {
                Gizmos.DrawWireCube(transform.position + (Vector3)sideViewColOffset, new Vector3(sideViewColSize.x, sideViewColSize.y, 0));
            }
            else
            {
                Gizmos.DrawWireCube(transform.position + (Vector3)topViewColOffset, new Vector3(topViewColSize.x, topViewColSize.y, 0));
            }
        }
    }
}
