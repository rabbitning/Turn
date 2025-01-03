using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : EffectByViewChange
{
    protected Action _move = null;
    protected Action _updateAnimationState = null;

    public List<StatData> DefaultStatsData = new() { new(StatName.MaxHealth, 100), new(StatName.Health, 100) };
    public Dictionary<StatName, float> CurrentStatsData { get; protected set; } = new();

    public void SetCurrentStatsData(StatName statName, float value)
    {
        if (!CurrentStatsData.ContainsKey(statName)) return;

        value = Mathf.Max(value, 0);

        CurrentStatsData[statName] = statName switch
        {
            StatName.Health => Mathf.Min(value, CurrentStatsData[StatName.MaxHealth]),
            StatName.Shield => Mathf.Min(value, CurrentStatsData[StatName.MaxShield]),
            StatName.Invincible => value == 0 ? 0 : 1,
            _ => value
        };
    }

    protected override void Start()
    {
        base.Start();
        foreach (StatData stat in DefaultStatsData) CurrentStatsData.TryAdd(stat.Name, stat.Value);
    }

    protected override void ViewChanged(bool isSS)
    {
        base.ViewChanged(isSS);
        if (isSS)
        {
            _move = MoveInSS;
            _updateAnimationState = UpdateAnimationStateSS;
        }
        else
        {
            _move = MoveInTD;
            _updateAnimationState = UpdateAnimationStateTD;
        }
    }

    protected abstract void MoveInSS();
    protected abstract void MoveInTD();

    protected abstract void UpdateAnimationStateSS();
    protected abstract void UpdateAnimationStateTD();

}