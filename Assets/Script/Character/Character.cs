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

        switch (statName)
        {
            // case StatName.MaxHealth:
            //     CurrentStatsData[StatName.MaxHealth] = value;
            //     SetCurrentStatsData(StatName.Health, CurrentStatsData[StatName.Health]);
            //     break;
            case StatName.Health:
                CurrentStatsData[StatName.Health] = Mathf.Min(value, CurrentStatsData[StatName.MaxHealth]);
                break;
            // case StatName.MaxShield:
            //     CurrentStatsData[StatName.MaxShield] = value;
            //     SetCurrentStatsData(StatName.Shield, CurrentStatsData[StatName.Shield]);
            //     break;
            // case StatName.Shield:
            //     CurrentStatsData[StatName.Shield] = Mathf.Min(value, CurrentStatsData[StatName.MaxShield]);
            //     break;
            case StatName.Invincible:
                CurrentStatsData[StatName.Invincible] = value == 0 ? 0 : 1;
                break;
            default:
                CurrentStatsData[statName] = value;
                break;
        }
    }

    protected virtual void Awake()
    {
        foreach (StatData stat in DefaultStatsData) CurrentStatsData.TryAdd(stat.Name, stat.Value);
    }

    protected virtual void FixedUpdate()
    {
        _move?.Invoke();
    }

    protected virtual void LateUpdate()
    {
        _updateAnimationState?.Invoke();
    }

    protected override void OnSS()
    {
        base.OnSS();
        _move = MoveInSS;
        _updateAnimationState = UpdateAnimationStateSS;
    }

    protected override void OnTD()
    {
        base.OnTD();
        _move = MoveInTD;
        _updateAnimationState = UpdateAnimationStateTD;
    }

    protected abstract void MoveInSS();
    protected abstract void MoveInTD();

    protected abstract void UpdateAnimationStateSS();
    protected abstract void UpdateAnimationStateTD();

}