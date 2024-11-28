using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GL = UnityEngine.GUILayout;

public class CombatAbilityCreator : EditorWindow
{
    private const int FIELD_WIDTH = 300;
    private const int BUTTON_WIDTH = 160;
    private const int BUTTON_HEIGHT = 26;
    private const int WINDOW_PADDING = 8;

    private const int GRID_PADDING = 10;
    private const int GRID_DIVIDER_WIDTH = 4;
    private const int GRID_SQUARE_MAX_WIDTH = 36;
    private const int GRID_SQUARE_MIN_WIDTH = 16;
    private Color _gridSquareEmptyColor = new Color(1f, 1f, 1f, 0.20f);
    private Color _gridSquareUserPositionColor = new Color(1f, 1f, 1f, 0.50f);

    private const int SLIDER_MAX_WIDTH = 300;


    private float _windowWidth;

    [MenuItem("Tools/Combat Ability Editor")]
    public static void ShowWindow()
    {
        GetWindow<CombatAbilityCreator>("Combat Ability Editor");
    }


    private CombatAbility _combatAbility;
    
    private void OnGUI()
    {
        _windowWidth = EditorGUIUtility.currentViewWidth;

        GL.BeginHorizontal();     //
        GL.Space(WINDOW_PADDING); //Left padding;
        GL.BeginVertical();       //

        GL.Label("Ability");
        _combatAbility = (CombatAbility)EditorGUILayout.ObjectField("Current ability", _combatAbility, typeof(CombatAbility), false, GL.Width(FIELD_WIDTH + 100));

        if (_combatAbility == null) DrawAbilityEmptyGUI();
        else DrawAbilityPresentGUI();

        GL.EndVertical();
        GL.EndHorizontal();
    }
    private void DrawAbilityEmptyGUI()
    {
        if(GL.Button("Create new ability", GL.MaxWidth(BUTTON_WIDTH), GL.MaxHeight(BUTTON_HEIGHT)))
        {
            _combatAbility = CreateInstance<CombatAbility>();
            _combatAbility.Width = 11;
            _combatAbility.Height = 11;
        }
    }
    private Vector2 scrollPosition;
    private void DrawAbilityPresentGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, true, true);
        if(GL.Button("Save", GL.MaxWidth(BUTTON_WIDTH), GL.MaxHeight(BUTTON_HEIGHT)))
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


        DrawGrid();
        DrawWidthHeightSliders();
        EditorGUILayout.EndScrollView();
    }
    private void DrawGrid()
    {
        int w = 11;
        int h = 11;
        w = _widthSliderValue * 2 + 1;
        h = _heightSliderValue * 2 + 1;
        int side = (int)(((_windowWidth - (2 * GRID_PADDING) - WINDOW_PADDING) - (GRID_DIVIDER_WIDTH * w)) / w * 1f);
        if(side > GRID_SQUARE_MAX_WIDTH) side = GRID_SQUARE_MAX_WIDTH;
        if(side < GRID_SQUARE_MIN_WIDTH) side = GRID_SQUARE_MIN_WIDTH;

        GL.Space(GRID_PADDING);
        for(int i = 0; i < h; i++)
        {
            GL.BeginHorizontal();
            GL.Space(GRID_PADDING);
            for(int j = 0; j < w; j++)
            {
                Rect rec = GUILayoutUtility.GetRect(side, side, GL.ExpandWidth(false));
                if (j == (w-1)/2 && i == (h-1)/2) EditorGUI.DrawRect(rec, _gridSquareUserPositionColor);
                else EditorGUI.DrawRect(rec, _gridSquareEmptyColor);
                GL.Space(GRID_DIVIDER_WIDTH);
            }
            GL.EndHorizontal();
            GL.Space(GRID_DIVIDER_WIDTH);
        }
        GL.Space(Mathf.Max(0,GRID_PADDING - GRID_DIVIDER_WIDTH));
    }
    private int _widthSliderValue = 5;
    private int _heightSliderValue = 5;
    private string _name = "";
    private void DrawWidthHeightSliders()
    {
        GL.BeginHorizontal();
        _name = EditorGUILayout.TextField(_name, GL.MaxWidth(FIELD_WIDTH));
        GL.Space(4);
        GL.Label("Combat ability name");
        GL.EndHorizontal();
        GL.Space(16);

        GL.BeginHorizontal();
        float rawWidth = GL.HorizontalSlider(_widthSliderValue, 0, 5, GL.MaxWidth(SLIDER_MAX_WIDTH),GL.MinWidth(10));
        GL.Space(4);
        GL.Label("Width");
        GL.EndHorizontal();
        GL.Label($"{_widthSliderValue * 2 + 1}");

        GL.Space(16);

        GL.BeginHorizontal();
        float rawHeight = GL.HorizontalSlider(_heightSliderValue, 0, 5, GL.MaxWidth(SLIDER_MAX_WIDTH),GL.MinWidth(10));
        GL.Space(4);
        GL.Label("Height");
        GL.EndHorizontal();
        GL.Label($"{_heightSliderValue * 2 + 1}");

        _widthSliderValue = Mathf.RoundToInt(rawWidth);
        _heightSliderValue = Mathf.RoundToInt(rawHeight);
    }
}
