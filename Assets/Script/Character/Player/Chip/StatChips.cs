using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewChip", menuName = "Chips/數值晶片", order = -1)]
public class StatChips : Chips
{
    [Serializable]
    public struct StatModifierData
    {
        public StatData StatData;
        public StatSetType SetType;
    }
    public List<StatModifierData> StatModifiers = new();

    public override void Active(PlayerController player) { }
}