using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target = null;
    [SerializeField] float followSpeed = 0;
    [SerializeField] Vector3 sideViewCameraOffset = Vector3.zero;
    [SerializeField] Vector3 topViewCameraOffset = Vector3.zero;
    Vector3 posOffset = Vector3.zero;

    Rigidbody2D rb = null;

    void Start()
    {
        GameManager.gameManager.OnViewChanged.AddListener(ViewChanged);

        rb = GetComponent<Rigidbody2D>();

        ViewChanged(GameManager.gameManager.GetView());

        // transform.position = target.position + posOffset;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.timeScale != 0)
        {
            rb.AddForce(new Vector2(100, 0), ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        // Vector2 v = (target.position + posOffset - transform.position) * followSpeed;
        // if (v.magnitude < 1f) v = Vector2.zero;
        // rb.velocity = v;
    }

    void ViewChanged(bool currentView)
    {
        posOffset = currentView ? sideViewCameraOffset : topViewCameraOffset;
    }
}
