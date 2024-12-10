using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GL = UnityEngine.GUILayout;

public class CombatAbilityCreator : EditorWindow
{
    private const int FIELD_WIDTH = 240;
    private const int BUTTON_WIDTH = 160;
    private const int BUTTON_HEIGHT = 20;
    private const int WINDOW_PADDING = 8;
    private const int SLIDER_MAX_WIDTH = 240;

    private const int GRID_PADDING = 10;
    private const int GRID_DIVIDER_WIDTH = 4;
    private const int GRID_SQUARE_MAX_WIDTH = 26;
    private const int GRID_SQUARE_MIN_WIDTH = 10;
    private Color _gridSquareEmptyColor = new Color(0.3f, 0.3f, 0.3f);
    private Color _gridSquareCenterPositionColor = new Color(0.5f, 0.5f, 0.5f);
    private Color _gridSquareRangeColor = new Color(0.2f, 0.2f, 0);


    private const int ABILITY_MAX_SIZE = 15; //keep this odd


    private float _windowWidth;
    private Vector2 scrollPosition;
    private bool _leftMouseIsDown = false;
    private bool _rightMouseIsDown = false;
    private bool _isPainting = false;
    private bool _isErasing = false;

    private CombatAbility _combatAbility;
    private CombatAbilityEffect _selectedEffect = null;



    [MenuItem("Tools/Combat Ability Editor")]
    public static void ShowWindow()
    {
        GetWindow<CombatAbilityCreator>("Combat Ability Editor");
    }

    private void OnGUI()
    {
        OperateWindow();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, false, false);
        GL.BeginHorizontal();     //
        GL.Space(WINDOW_PADDING); //Pad the entire window;
        GL.BeginVertical();       //

        GL.Space(10);
        CombatAbility prevAbility = _combatAbility;
        _combatAbility = (CombatAbility)EditorGUILayout.ObjectField("Current Ability", _combatAbility, typeof(CombatAbility), false, GL.Width(FIELD_WIDTH + 100));
        if(_combatAbility != prevAbility)
        {
            EditorUtility.SetDirty(_combatAbility);
            _selectedEffect = null;
            Repaint();
        }

        if (_combatAbility == null) DrawAbilityEmptyGUI();
        else DrawAbilityPresentGUI();

        GL.EndVertical();
        GL.EndHorizontal();
        EditorGUILayout.EndScrollView();
    }
    private void OperateWindow()
    {
        _windowWidth = EditorGUIUtility.currentViewWidth;
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0) _leftMouseIsDown = true;
        if (Event.current.type == EventType.MouseUp && Event.current.button == 0) _leftMouseIsDown = false;

        if (Event.current.type == EventType.MouseDown && Event.current.button == 1) _rightMouseIsDown = true;
        if (Event.current.type == EventType.MouseUp && Event.current.button == 1) _rightMouseIsDown = false;


        if (_leftMouseIsDown && _combatAbility != null) _isPainting = true;
        else _isPainting = false;

        if (_rightMouseIsDown && _combatAbility != null) _isErasing = true;
        else _isErasing = false;

        if((_isPainting && _isErasing) || GUIUtility.hotControl != 0)
        {
            _isPainting = false;
            _isErasing = false;
        }
    }
    private void DrawAbilityEmptyGUI()
    {
        if(GL.Button("Create new ability", GL.MaxWidth(BUTTON_WIDTH), GL.MaxHeight(BUTTON_HEIGHT)))
        {
            _combatAbility = CreateInstance<CombatAbility>();
            _combatAbility.Width = ABILITY_MAX_SIZE;
            _combatAbility.Height = ABILITY_MAX_SIZE;
        }
    }
    private void DrawAbilityPresentGUI()
    {
        if(GL.Button("Save", GL.MaxWidth(BUTTON_WIDTH), GL.MaxHeight(BUTTON_HEIGHT)))
        {
            if (!AssetDatabase.Contains(_combatAbility))
            {
                string path = System.IO.Path.Combine(Application.dataPath, "Resources/CombatAbilities");
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                path = EditorUtility.SaveFilePanelInProject("Save Combat Ability", "NewCombatAbility", "asset", "Save Combat Ability", path);
                if (path != "" && path != null)
                {
                    AssetDatabase.CreateAsset(_combatAbility, path);
                    AssetDatabase.SaveAssets();
                }
            }
            else Debug.LogWarning("Asset database already has this asset saved. If you want to create a copy, just duplicate the scriptable object in unity.");
        }


        DrawGrid();
        DrawCombatAbility();
        DrawCombatAbilityEffects();
        GL.Space(200);
    }


    #region <<< Grid >>>

    private void DrawGrid()
    {
        if(_combatAbility == null) return;
        DrawDivider();
        GL.BeginHorizontal();
        GL.Label("Now painting:", GL.MaxWidth(90));
        if(_selectedEffect == null) GL.Label("Range", GL.MaxWidth(90));
        else GL.Label("Affected tiles", GL.MaxWidth(90));
        GL.EndHorizontal();

        int side = (int)(((_windowWidth - (2 * GRID_PADDING) - WINDOW_PADDING) - (GRID_DIVIDER_WIDTH * _combatAbility.Width)) / _combatAbility.Width * 1f);
        if(side > GRID_SQUARE_MAX_WIDTH) side = GRID_SQUARE_MAX_WIDTH;
        if(side < GRID_SQUARE_MIN_WIDTH) side = GRID_SQUARE_MIN_WIDTH;

        List<Vector2Int> affectedTiles = _selectedEffect == null ? new List<Vector2Int>(_combatAbility.Range) : new List<Vector2Int>(_selectedEffect.RelativeAffectedPositions);

        GL.Space(GRID_PADDING);
        for(int y = 0; y < _combatAbility.Height; y++)
        {
            GL.BeginHorizontal();
            GL.Space(GRID_PADDING);
            for(int x = 0; x < _combatAbility.Width; x++)
            {
                Rect rec = GUILayoutUtility.GetRect(side, side, GL.ExpandWidth(false));
                EditorGUI.DrawRect(rec, ChooseColor(x,y, ref affectedTiles));
                PaintTile(x, y, rec, ref affectedTiles);
                GL.Space(GRID_DIVIDER_WIDTH);
            }
            GL.EndHorizontal();
            GL.Space(GRID_DIVIDER_WIDTH);
        }
        GL.Space(Mathf.Max(0,GRID_PADDING - GRID_DIVIDER_WIDTH));
    }
    private Vector2Int ChangeAbsoluteCoordinateToRelative(int x, int y, int width, int height)
    {
        Vector2Int coordinate = new Vector2Int((width - 1) / 2, (height - 1) / 2);
        coordinate = new Vector2Int(x - coordinate.x, y - coordinate.y);
        return coordinate;
    }
    private Vector2Int ChangeRelativeCoordinateToAbsolute(int x, int y, int width, int height)
    {
        Vector2Int coordinate = new Vector2Int((width - 1) / 2, (height - 1) / 2);
        coordinate = new Vector2Int(x + coordinate.x, y + coordinate.y);
        return coordinate;
    }
    private Color ChooseColor(int x, int y, ref List<Vector2Int> affectedTiles)
    {
        bool isCenter = (x == (_combatAbility.Width - 1) / 2 && y == (_combatAbility.Height - 1) / 2);
        bool isAffected = affectedTiles.Contains(ChangeAbsoluteCoordinateToRelative(x, y, _combatAbility.Width, _combatAbility.Height));

        Color color = _gridSquareEmptyColor;
        if (isCenter) color = _gridSquareCenterPositionColor;

        if (isAffected && _selectedEffect != null) color += CombatAbilityEffect.EffectTypeToColor(_selectedEffect.Type);
        if (isAffected && _selectedEffect == null) color += _gridSquareRangeColor;

        return color;
    }
    private void PaintTile(int x, int y, Rect rec, ref List<Vector2Int> affectedTiles)
    {
        if(_isPainting)
        {
            Vector2Int relativePosition = ChangeAbsoluteCoordinateToRelative(x,y, _combatAbility.Width, _combatAbility.Height);

            if (rec.Contains(Event.current.mousePosition) && !affectedTiles.Contains(relativePosition))
            {
                if (_selectedEffect == null) _combatAbility.Range.Add(relativePosition); 
                else _selectedEffect.RelativeAffectedPositions.Add(relativePosition);
                EditorUtility.SetDirty(_combatAbility);
                Repaint();
            }
        }
        else if (_isErasing)
        {
            Vector2Int relativePosition = ChangeAbsoluteCoordinateToRelative(x,y, _combatAbility.Width, _combatAbility.Height);

            if (rec.Contains(Event.current.mousePosition) && affectedTiles.Contains(relativePosition))
            {
                if (_selectedEffect == null) _combatAbility.Range.Remove(relativePosition);
                else _selectedEffect.RelativeAffectedPositions.Remove(relativePosition);
                EditorUtility.SetDirty(_combatAbility);
                Repaint();
            }
        }
    }

    #endregion


    #region <<< CombatAbility >>>

    private void DrawCombatAbility()
    {
        if(_combatAbility == null) return;

        int prevWidth = _combatAbility.Width;
        int prevHeight = _combatAbility.Height;
        string prevName = _combatAbility.Name;

        int widthSliderValue = (_combatAbility.Width - 1) / 2;
        int heightSliderValue = (_combatAbility.Height - 1) / 2;

        GL.BeginHorizontal();
        _combatAbility.Name = EditorGUILayout.TextField(_combatAbility.Name, GL.MaxWidth(FIELD_WIDTH), GL.MinWidth(10));
        GL.Space(4);
        GL.Label("Combat ability name");
        GL.EndHorizontal();
        GL.Space(10);

        GL.BeginHorizontal();
        float rawWidth = GL.HorizontalSlider(widthSliderValue, 0, (ABILITY_MAX_SIZE - 1) / 2, GL.MaxWidth(SLIDER_MAX_WIDTH), GL.MinWidth(10));
        GL.Space(4);
        GL.Label("Width");
        GL.EndHorizontal();
        GL.Label($"{widthSliderValue * 2 + 1}");

        GL.Space(8);

        GL.BeginHorizontal();
        float rawHeight = GL.HorizontalSlider(heightSliderValue, 0, (ABILITY_MAX_SIZE - 1) / 2, GL.MaxWidth(SLIDER_MAX_WIDTH), GL.MinWidth(10));
        GL.Space(4);
        GL.Label("Height");
        GL.EndHorizontal();
        GL.Label($"{heightSliderValue * 2 + 1}");


        _combatAbility.Width = (Mathf.RoundToInt(rawWidth) * 2) + 1;
        _combatAbility.Height = (Mathf.RoundToInt(rawHeight) * 2) + 1;
        if (_combatAbility.Width != prevWidth || _combatAbility.Height != prevHeight || _combatAbility.Name != prevName) EditorUtility.SetDirty(_combatAbility);
    }

    #endregion


    #region <<< CombatAbilityEffects >>>

    private void DrawCombatAbilityEffects()
    {
        if (_combatAbility == null) return;

        DrawDivider();
        DrawAddRemoveEffectButtons();
        GL.Space(8);
        DrawEffectList();
    }
    private void DrawAddRemoveEffectButtons()
    {
        int effectsLength = _combatAbility.AbilityEffects.Length;
        GL.Label($"Ability effects: {effectsLength}");

        GL.BeginHorizontal();
        if(GL.Button("+", GL.MaxWidth(22), GL.MaxHeight(22)))
        {
            int length = _combatAbility.AbilityEffects.Length;
            CombatAbilityEffect[] copyList = GetCopyOfAbilityEffects(_combatAbility.AbilityEffects);
            _combatAbility.AbilityEffects = new CombatAbilityEffect[length + 1];
            _combatAbility.AbilityEffects = CopyAbilityEffectsToNewArray(copyList, ref _combatAbility.AbilityEffects);
            _combatAbility.AbilityEffects[length] = new CombatAbilityEffect();

            _selectedEffect = null;
            GUI.FocusControl(null);
            EditorUtility.SetDirty(_combatAbility);
        }
        if(GL.Button("-", GL.MaxWidth(22), GL.MaxHeight(22)))
        {
            int length = _combatAbility.AbilityEffects.Length;
            CombatAbilityEffect[] copyList = GetCopyOfAbilityEffects(_combatAbility.AbilityEffects);
            _combatAbility.AbilityEffects = new CombatAbilityEffect[length - 1];
            _combatAbility.AbilityEffects = CopyAbilityEffectsToNewArray(copyList, ref _combatAbility.AbilityEffects);

            _selectedEffect = null;
            GUI.FocusControl(null);
            EditorUtility.SetDirty(_combatAbility);
        }
        GL.EndHorizontal();
    }
    private CombatAbilityEffect[] GetCopyOfAbilityEffects(CombatAbilityEffect[] original)
    {
        CombatAbilityEffect[] copyList = new CombatAbilityEffect[original.Length];
        for(int i = 0; i < original.Length; i++)
        {
            copyList[i] = original[i];
        }

        return copyList;
    }
    private ref CombatAbilityEffect[] CopyAbilityEffectsToNewArray(CombatAbilityEffect[] original, ref CombatAbilityEffect[] copy)
    {
        for(int i = 0; i < original.Length; i++)
        {
            if (i >= copy.Length)
                break;
            copy[i] = original[i];
        }
        return ref copy;
    }
    private void DrawEffectList()
    {
        for (int i = 0; i < _combatAbility.AbilityEffects.Length; i++)
        {
            CombatAbilityEffect.EffectType prevType = _combatAbility.AbilityEffects[i].Type;
            GL.BeginHorizontal();

            _combatAbility.AbilityEffects[i].Type = (CombatAbilityEffect.EffectType)EditorGUILayout.EnumPopup(_combatAbility.AbilityEffects[i].Type, GL.MaxWidth(BUTTON_WIDTH));
            GL.Space(6);
            if (GL.Button("Select this", GL.MaxWidth(BUTTON_WIDTH), GL.MinHeight(BUTTON_HEIGHT)))
            {
                if (_selectedEffect == null || _selectedEffect != _combatAbility.AbilityEffects[i]) _selectedEffect = _combatAbility.AbilityEffects[i];
                else _selectedEffect = null;
                GUI.FocusControl(null);
            }
            GL.Space(6);
            if (_combatAbility.AbilityEffects[i] == _selectedEffect)
            {
                GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
                boxStyle.normal.background = EditorGUIUtility.whiteTexture;
                Color prevColor = GUI.color;
                GUI.color = Color.green;
                GL.Box(GUIContent.none, boxStyle, GL.Width(BUTTON_HEIGHT-10), GL.Height(BUTTON_HEIGHT-10));
                GUI.color = prevColor;
                GL.Space(16);


                GL.Label("Amount:", GL.MaxWidth(50));
                int prevAmount = _selectedEffect.Amount;
                _selectedEffect.Amount = EditorGUILayout.IntField(_selectedEffect.Amount, GL.MaxWidth(BUTTON_WIDTH));
                if(_selectedEffect.Amount != prevAmount) EditorUtility.SetDirty(_combatAbility);
            }

            GL.EndHorizontal();
            if (_combatAbility.AbilityEffects[i].Type != prevType)
            {
                EditorUtility.SetDirty(_combatAbility);
                GUI.FocusControl(null);
            }
        }
    }

    #endregion


    private void DrawDivider()
    {
        GL.Space(16);
        Rect rec = GUILayoutUtility.GetRect(_windowWidth - 40, 2, GL.ExpandWidth(false));
        EditorGUI.DrawRect(rec, new Color(0.14f, 0.14f, 0.14f, 1));
        GL.Space(2);
    }
}



//TODO: Save button allowing saving when assed is already saved in database.
//TODO: Make it so that the tool doesn't overwrite the edited object, instead work on a copy and save using save button when finished