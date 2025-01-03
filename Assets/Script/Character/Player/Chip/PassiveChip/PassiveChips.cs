using System.Collections;
using UnityEngine;

public abstract class PassiveChips : Chips
{
    public Coroutine PassiveSkillCoroutine = null;
    public virtual IEnumerator CPassiveSkill(PlayerController player) { yield return null; }
}