using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PlayerController playerController = (PlayerController)target;
        
        if (GUILayout.Button("Add Missing Stats"))
        {
            AddMissingStats(playerController);
        }
        DrawDefaultInspector();
    }

    private void AddMissingStats(PlayerController playerController)
    {
        StatName[] allStats = (StatName[])System.Enum.GetValues(typeof(StatName));
        List<StatData> currentStats = playerController.DefaultStatsData;
        
        foreach (StatName stat in allStats)
        {
            bool statExists = false;
            foreach (StatData currentStat in currentStats)
            {
                if (currentStat.Name == stat)
                {
                    statExists = true;
                    break;
                }
            }
            if (!statExists)
            {
                playerController.DefaultStatsData.Add(new StatData { Name = stat, Value = 100 });
            }
        }
        EditorUtility.SetDirty(playerController);
    }
}