using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class RuleTileBatchImporter : EditorWindow
{
    private RuleTile ruleTile;
    private Texture2D spriteSheet;

    [MenuItem("Tools/RuleTile 填入工具")]
    public static void ShowWindow()
    {
        GetWindow<RuleTileBatchImporter>("RuleTile Batch Importer");
    }

    private void OnGUI()
    {
        GUILayout.Label("RuleTile Batch Importer", EditorStyles.boldLabel);

        ruleTile = (RuleTile)EditorGUILayout.ObjectField("RuleTile", ruleTile, typeof(RuleTile), false);

        GUILayout.Label("Select Sprite Sheet", EditorStyles.label);
        spriteSheet = (Texture2D)EditorGUILayout.ObjectField("Sprite Sheet", spriteSheet, typeof(Texture2D), false);

        if (ruleTile != null && spriteSheet != null)
        {
            if (GUILayout.Button("Replace Sprites in RuleTile"))
            {
                ReplaceSpritesInRuleTile();
            }
        }
    }

    private void ReplaceSpritesInRuleTile()
    {
        string path = AssetDatabase.GetAssetPath(spriteSheet);
        Object[] allSprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(path);

        List<Sprite> sprites = new();
        foreach (Object obj in allSprites)
        {
            if (obj is Sprite sprite)
            {
                sprites.Add(sprite);
            }
        }

        Undo.RecordObject(ruleTile, "Replace Sprites in RuleTile");

        while (ruleTile.m_TilingRules.Count < sprites.Count)
        {
            ruleTile.m_TilingRules.Add(new RuleTile.TilingRule());
        }

        ruleTile.m_DefaultSprite = sprites[1];

        for (int i = 0; i < sprites.Count; i++)
        {
            ruleTile.m_TilingRules[i].m_Sprites[0] = sprites[i];
        }

        EditorUtility.SetDirty(ruleTile);
        AssetDatabase.SaveAssets();
    }
}