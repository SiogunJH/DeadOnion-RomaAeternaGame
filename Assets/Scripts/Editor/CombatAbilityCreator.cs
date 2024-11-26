using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using static UnityEditor.AddressableAssets.Build.Layout.BuildLayout;

public class CombatAbilityCreator : EditorWindow
{
    private const int BUTTON_WIDTH = 160;
    private const int BUTTON_HEIGHT = 26;

    [MenuItem("Tools/Combat Ability Editor")]
    public static void ShowWindow()
    {
        GetWindow<CombatAbilityCreator>("Combat Ability Editor");
    }


    private CombatAbility _combatAbility;
    private void OnGUI()
    {
        GUILayout.Label("Ability");
        _combatAbility = (CombatAbility)EditorGUILayout.ObjectField("Current ability", _combatAbility, typeof(CombatAbility), false);

        if (_combatAbility == null) DrawAbilityEmptyGUI();
        else DrawAbilityPresentGUI();
    }
    private void DrawAbilityEmptyGUI()
    {
        if(GUILayout.Button("Create new ability", GUILayout.MaxWidth(BUTTON_WIDTH), GUILayout.MaxHeight(BUTTON_HEIGHT)))
        {
            _combatAbility = CreateInstance<CombatAbility>();
            _combatAbility.Width = 11;
            _combatAbility.Height = 11;
        }
    }
    private void DrawAbilityPresentGUI()
    {
        if(GUILayout.Button("Save", GUILayout.MaxWidth(BUTTON_WIDTH), GUILayout.MaxHeight(BUTTON_HEIGHT)))
        {
            string path = System.IO.Path.Combine(Application.dataPath, "Resources/CombatAbilities");
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            path = EditorUtility.SaveFilePanelInProject("Save Combat Ability", "NewCombatAbility", "asset", "Save Combat Ability", path);
            if(path != "" && path != null)
            {
                AssetDatabase.CreateAsset(_combatAbility, path);
                AssetDatabase.SaveAssets();
            }
        }


        int padding = 10;
        int divider = 4;
        Color c1 = new Color(1f, 1f, 1f, 0.20f);
        Color c2 = new Color(1f, 1f, 1f, 0.50f);
        int w = 11;
        int h = 11;
        float winW = EditorGUIUtility.currentViewWidth;
        int side = (int)(((winW - (2 * padding)) - (divider * w)) / w * 1f);

        GUILayout.Space(padding);
        for(int i = 0; i < h; i++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(padding);
            for(int j = 0; j < w; j++)
            {
                Rect rec = GUILayoutUtility.GetRect(side, side, GUILayout.ExpandWidth(false));
                if (j == (w-1)/2 && i == (h-1)/2) EditorGUI.DrawRect(rec, c2);
                else EditorGUI.DrawRect(rec, c1);
                GUILayout.Space(divider);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(divider);
        }
        GUILayout.Space(padding - divider);
    }
}
