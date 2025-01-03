using System;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public struct ColliderData
{
    public bool ShowGizmos;
    public Vector2 Size;
    public Vector2 Offset;
}

[Serializable]
public struct MovementData
{
    // [Min(0)] public float MaxMoveSpeed;
    // [Min(0)] public float GroundAcceleration;
    // [Min(0)] public float AirAcceleration;
    [HideInInspector] public Vector2 CurrentVelocity;
    // [Min(0)] public float JumpForceMultiplier;
    [Min(0)] public float CoyoteTime;
    [HideInInspector] public float CoyoteTimer;
    [Min(0)] public float JumpBufferTime;
    [HideInInspector] public float JumpBufferTimer;
    [Min(0)] public float FallForceMultiplier;
}

[Serializable]
public struct GroundCheckData
{
    public bool ShowGizmos;
    [HideInInspector] public bool IsGrounded;
    public LayerMask GroundLayer;
    public Vector2 GroundCheckOffset;
    public Vector2 GroundCheckSize;
    [HideInInspector] public Vector2 LastGroundedPosition;
}

[Serializable]
public struct WallCheckData
{
    public bool ShowGizmos;
    [HideInInspector] public bool2 IsBlockedByWall;
    public LayerMask WallLayer;
    public Vector2 WallCheckOffsetX;
    public Vector2 WallCheckSizeX;
    public Vector2 WallCheckOffsetY;
    public Vector2 WallCheckSizeY;
}

[Serializable]
public struct MeleeAttackData
{
    public bool ShowGizmos;
    public float Damage;
    public float AttackCooldown;
    [HideInInspector] public float AttackCooldownTimer;
}

[Serializable]
public struct RangedAttackData
{
    public bool ShowGizmos;
    public GameObject Projectile;
    public Vector2 ProjectileOffset;
    public float ProjectileSpeed;
    public float Damage;
    public float AttackCooldown;
    [HideInInspector] public float AttackCooldownTimer;
}

public enum StatSetType { 無, 加算, 乗算, 設定 }

public enum StatName
{
    MaxHealth,
    Health,
    MaxShield,
    Shield,
    MaxMoveSpeed,
    AttackCooldown,
    RangedAttackDamage,
    MeleeAttackDamage,
    Invincible,
    GroundAcceleration,
    AirAcceleration,
    JumpForceMultiplier,
    FallForceMultiplier,
}

[Serializable]
public struct StatData
{
    public StatData(StatName name, float value)
    {
        Name = name;
        Value = value;
    }
    public StatName Name;
    [Min(0)] public float Value;
}

public struct InputData
{
    public bool CanInput;
    public Vector2 MoveInput;
    public bool2 JumpInput;
    public bool2 AttackInput;
    public Vector2 MousePosInput;
    public bool3 SkillInput;
}
