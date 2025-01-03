using UnityEngine;

[CreateAssetMenu(fileName = "DashChip", menuName = "Chips/主動晶片/Dash")]
public class DashChip : ActiveChips
{
    public override void UseActiveSkill(PlayerController player)
    {
        Debug.Log("Dash");
    }
}