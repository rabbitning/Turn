using UnityEngine;

[CreateAssetMenu(fileName = "HurtSelfChip", menuName = "Chips/主動晶片/HurtSelf")]
public class HurtSelfChip : ActiveChips
{
    [SerializeField, Min(0)] float _damage = 0;

    public override void UseActiveSkill(PlayerController player)
    {
        player.Damage(_damage);
    }
}