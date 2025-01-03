using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "SlowHeallingChip", menuName = "Chips/被動晶片/SlowHealling")]
public class SlowHeallingChip : PassiveChips
{
    [SerializeField] float _healAmount;
    [SerializeField, Range(0f, 100f)] float _healInterval = 1f;
    WaitForSeconds _healIntervalWait;

    protected override void OnEnable()
    {
        base.OnEnable();
        _healIntervalWait = new WaitForSeconds(_healInterval);
    }

    public override IEnumerator CPassiveSkill(PlayerController player)
    {
        while (player != null)
        {
            if (player.CurrentStatsData[StatName.Health] <= 0) yield break;
            player.SetCurrentStatsData(StatName.Health, player.CurrentStatsData[StatName.Health] + _healAmount);
            yield return _healIntervalWait;
        }
    }
}