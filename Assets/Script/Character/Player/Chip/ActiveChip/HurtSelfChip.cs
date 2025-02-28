using UnityEngine;

[CreateAssetMenu(fileName = "HurtSelfChip", menuName = "Chips/主動晶片/HurtSelf")]
public class HurtSelfChip : ActiveChips
{
    public override float _cooldownTime { get; set; } = 1f;
    [SerializeField, Min(0)] float _damage = 0;

    public override void UseActiveSkill(PlayerController player)
    {
        player.Damage(_damage);
    }
}