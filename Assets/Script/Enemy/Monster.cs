using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    protected Animator animator = null;
    protected Rigidbody2D rb = null;
    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {

    }
    protected void ViewChanged(bool currentView)
    {
        animator?.SetBool("isSS", currentView);
    }
}
