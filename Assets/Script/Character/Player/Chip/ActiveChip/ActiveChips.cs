using UnityEngine;

public abstract class ActiveChips : Chips
{
    public abstract float _cooldownTime { get; set; }
    float _lastUsedTime = 0f;

    public abstract void UseActiveSkill(PlayerController player);

    public override void Active(PlayerController player)
    {
        if (Time.time - _lastUsedTime >= _cooldownTime)
        {
            UseActiveSkill(player);
            _lastUsedTime = Time.time;
        }
    }
}