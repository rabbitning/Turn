using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Controller : MonoBehaviour, IDamageable
{
    [SerializeField] float maxHp = 0;
    public float MaxHp { get => maxHp; }
    float hp = 0;
    public float Hp
    {
        get => hp;
        set
        {
            hp = Mathf.Clamp(value, 0, MaxHp);
            if (hp == 0) { Destroy(gameObject); }
        }
    }

    Action move = null;
    Rigidbody2D rb = null;

    void Start()
    {
        GameManager.gameManager.OnViewChanged.AddListener(ViewChanged);

        rb = GetComponent<Rigidbody2D>();

        hp = MaxHp;

        ViewChanged(GameManager.gameManager.GetView());
    }

    // void Update()
    // {

    // }

    void FixedUpdate()
    {
        
    }

    void ViewChanged(bool currentView)
    {
        rb.velocity = Vector2.zero;

        if (currentView)
        {
            move = MoveInSideView;
        }
        else
        {
            move = MoveInTopView;
        }
    }

    void MoveInSideView()
    {

    }

    void MoveInTopView()
    {

    }

    public void Damage(float value)
    {
        Hp -= value;
    }
}
