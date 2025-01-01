using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using GL = UnityEngine.GUILayout;
using EGL = UnityEditor.EditorGUILayout;
using System.Reflection;
using System;
using System.Text;

public class CharacterProfileCreator : EditorWindow
{
    private const int FIELD_WIDTH = 240;
    private const int BUTTON_WIDTH = 160;
    private const int BUTTON_HEIGHT = 20;
    private const int WINDOW_PADDING = 8;
    private const int SLIDER_MAX_WIDTH = 240;
    private const int LABEL_SHORT_WIDTH = 120;
    private const int LABEL_MEDIUM_WIDTH = 240;
    private const int LABEL_LONG_WIDTH = 360;


    private float _windowWidth;
    private Vector2 scrollPosition;
    private bool _settingsAreOpen = false;

    private CharacterProfile _characterProfile;


    [MenuItem("Tools/Character Profile Editor")]
    public static void ShowWindow()
    {
        GetWindow<CharacterProfileCreator>("Character Profile Editor");
    }

    private void OnGUI()
    {
        OperateWindow();

        scrollPosition = EGL.BeginScrollView(scrollPosition, false, false);
        GL.BeginHorizontal();     //
        GL.Space(WINDOW_PADDING); //Pad the entire window;
        GL.BeginVertical();       //
        GL.Space(10);

        CharacterProfile prevCharacter = _characterProfile;
        _characterProfile = (CharacterProfile)EGL.ObjectField("Current character:", _characterProfile, typeof(CharacterProfile), false, GL.MaxWidth(FIELD_WIDTH + 100));
        if(_characterProfile != prevCharacter)
        {
            LoadCharacterProfileSettings();
            EditorUtility.SetDirty(_characterProfile);
            Repaint();
        }
        if (_characterProfile == null) DrawCharacterEmptyGUI();
        else DrawCharacterPresentGUI();

        GL.EndVertical();
        GL.EndHorizontal();
        EGL.EndScrollView();
    }
    private void OperateWindow()
    {
        _windowWidth = EditorGUIUtility.currentViewWidth;
    }
    private void LoadCharacterProfileSettings()
    {
        if (_characterProfile == null) return;
        var set = Resources.LoadAll<CharacterProfileSettings>("Settings");
        if (set.Length > 1) Debug.LogError("More than one CharacterProfileSettings asset found");
        if(set.Length <= 0)
        {
            Debug.LogError("No CharacterProfileSettings asset found");
            return;
        }
        _characterProfile.SetCharacterProfileSettings(set.FirstOrDefault());
    }
    private void DrawCharacterEmptyGUI()
    {
        if(GL.Button("Create new character", GL.MaxWidth(BUTTON_WIDTH), GL.MaxHeight(BUTTON_HEIGHT)))
        {
            _characterProfile = CreateInstance<CharacterProfile>();
            LoadCharacterProfileSettings();
        }
    }
    private void DrawCharacterPresentGUI()
    {
        if(GL.Button("Save", GL.MaxWidth(BUTTON_WIDTH), GL.MaxHeight(BUTTON_HEIGHT)))
        {
            if (!AssetDatabase.Contains(_characterProfile))
            {
                string path = System.IO.Path.Combine(Application.dataPath, "Resources/CharacterProfiles");
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                path = EditorUtility.SaveFilePanelInProject("Save Character Profile", "NewCharacterProfile", "asset", "Save Character Profile", path);
                if (path != "" && path != null)
                {
                    AssetDatabase.CreateAsset(_characterProfile, path);
                    AssetDatabase.SaveAssets();
                }
            }
            else Debug.LogWarning("Asset database already has this asset saved. If you want to create a copy, just duplicate the scriptable object in unity.");
        }
        GL.Space(10);



        DrawDivider();
        string prevName = _characterProfile.Name;
        GL.BeginHorizontal();
        GL.Label("Name" ,GL.MaxWidth(LABEL_MEDIUM_WIDTH));
        _characterProfile.Name = GL.TextField(prevName, GL.MaxWidth(FIELD_WIDTH));
        GL.EndHorizontal();
        if(_characterProfile.Name != prevName) EditorUtility.SetDirty(_characterProfile);

        DrawStats();
        DrawAbilities();
        DrawSettings();
        GL.Space(160);
    }


    private void DrawStats()
    {
        DrawPrimaryStats();
        DrawSecondaryStats();
        DrawTertiaryStats();
    }
    private void DrawPrimaryStats()
    {
        DrawDivider();
        GL.Label("Primary Stats");
        GL.Space(8);
        DrawFieldCharacterProfilePropertyWithName("Vitality");
        DrawFieldCharacterProfilePropertyWithName("Strength");
        DrawFieldCharacterProfilePropertyWithName("Power");
        DrawFieldCharacterProfilePropertyWithName("Agility");
        DrawFieldCharacterProfilePropertyWithName("Focus");
        DrawFieldCharacterProfilePropertyWithName("Reflex");
    }
    private void DrawSecondaryStats()
    {
        DrawDivider();
        GL.Label("Secondary Stats");
        GL.Space(8);

        //Base
        DrawCharacterProfilePropertiesWithName("Base");
        GL.Space(20);

        //Additional
        GL.Label("Additional");
        DrawPropertyUnderline();
        foreach(var property in _characterProfile.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!property.Name.Contains("Additional")) continue;

            if(property.PropertyType == typeof(int))
            {
                int value = (int)property.GetValue(_characterProfile);
                GL.BeginHorizontal();
                GL.Label(AddSpaceBeforeUppercase(RemoveWordFromStart(property.Name, "Additional")), GL.MaxWidth(LABEL_MEDIUM_WIDTH));
                int newValue = EGL.IntField(value, GL.MaxWidth(60));
                GL.Label("int", GL.MaxWidth(60));
                GL.EndHorizontal();
                if(newValue != value)
                {
                    if(newValue < 0) newValue = 0;
                    property.SetValue(_characterProfile, newValue);
                    EditorUtility.SetDirty(_characterProfile);
                }
            }
            if(property.PropertyType == typeof(float))
            {
                float value = (float)property.GetValue(_characterProfile);
                GL.BeginHorizontal();
                GL.Label(AddSpaceBeforeUppercase(RemoveWordFromStart(property.Name, "Additional")), GL.MaxWidth(LABEL_MEDIUM_WIDTH));
                float newValue = EGL.FloatField(value, GL.MaxWidth(60));
                GL.Label("float", GL.MaxWidth(60));
                GL.EndHorizontal();
                if(newValue != value)
                {
                    if(newValue < 0) newValue = 0;
                    property.SetValue(_characterProfile, newValue);
                    EditorUtility.SetDirty(_characterProfile);
                }
            }
            DrawPropertyUnderline();
        }
        GL.Space(20);

        //Total
        DrawCharacterProfilePropertiesWithName("Total");
    }
    private void DrawTertiaryStats()
    {
        DrawDivider();
        GL.Label("Tertiary Stats");
        GL.Space(8);
        DrawFieldCharacterProfilePropertyWithName("ActionPoints");
        DrawFieldCharacterProfilePropertyWithName("MaxArmor");
        DrawFieldCharacterProfilePropertyWithName("MaxHealth");
        DrawFieldCharacterProfilePropertyWithName("MaxShield");
        GL.Space(4);

        CharacterProfile.CharacterArmorClass prevArmorClass = _characterProfile.ArmorClass;
        GL.BeginHorizontal();
        GL.Label("Armor Class" ,GL.MaxWidth(LABEL_MEDIUM_WIDTH));
        _characterProfile.ArmorClass = (CharacterProfile.CharacterArmorClass)EGL.EnumPopup(_characterProfile.ArmorClass, GL.MaxWidth(BUTTON_WIDTH));
        GL.EndHorizontal();
        if (_characterProfile.ArmorClass != prevArmorClass)
        {
            EditorUtility.SetDirty(_characterProfile);
            GUI.FocusControl(null);
        }
    }
    private void DrawCharacterProfilePropertiesWithName(string name)
    {
        GL.Label(name);
        DrawPropertyUnderline();
        foreach(var property in _characterProfile.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!property.Name.Contains(name)) continue;

            GL.BeginHorizontal();

            GL.Label(AddSpaceBeforeUppercase(RemoveWordFromStart(property.Name, name)), GL.MaxWidth(LABEL_MEDIUM_WIDTH));
            GL.Space(16);
            GL.Label(property.GetValue(_characterProfile).ToString(), GL.MaxWidth(LABEL_MEDIUM_WIDTH));

            GL.EndHorizontal();
            DrawPropertyUnderline();
        }
    }
    private void DrawFieldCharacterProfilePropertyWithName(string name)
    {
        PropertyInfo property = _characterProfile.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.Name.Equals(name)).FirstOrDefault();

        if(property.PropertyType == typeof(int))
        {
            int value = (int)property.GetValue(_characterProfile);
            GL.BeginHorizontal();
            GL.Label(AddSpaceBeforeUppercase(property.Name), GL.MaxWidth(LABEL_MEDIUM_WIDTH));
            int newValue = EGL.IntField(value, GL.MaxWidth(60));
            GL.Label("int", GL.MaxWidth(60));
            GL.EndHorizontal();
            if(newValue != value)
            {
                if(newValue < 0) newValue = 0;
                property.SetValue(_characterProfile, newValue);
                EditorUtility.SetDirty(_characterProfile);
            }
        }
        if(property.PropertyType == typeof(float))
        {
            float value = (float)property.GetValue(_characterProfile);
            GL.BeginHorizontal();
            GL.Label(AddSpaceBeforeUppercase(property.Name), GL.MaxWidth(LABEL_MEDIUM_WIDTH));
            float newValue = EGL.FloatField(value, GL.MaxWidth(60));
            GL.Label("float", GL.MaxWidth(60));
            GL.EndHorizontal();
            if(newValue != value)
            {
                if(newValue < 0) newValue = 0;
                property.SetValue(_characterProfile, newValue);
                EditorUtility.SetDirty(_characterProfile);
            }
        }
        DrawPropertyUnderline();
    }

    private void DrawAbilities()
    {
        DrawDivider();
        GL.Label("Combat Abilities");
        GL.Space(8);

        GL.BeginHorizontal();
        if (GL.Button("+", GL.MaxWidth(22), GL.MaxHeight(22), GL.MinHeight(22), GL.MinWidth(22)))
        {
            _characterProfile.CombatAbilities.Add(null);
            GUI.FocusControl(null);
        }
        if (GL.Button("-", GL.MaxWidth(22), GL.MaxHeight(22), GL.MinHeight(22), GL.MinWidth(22)))
        {
            _characterProfile.CombatAbilities.Remove(_characterProfile.CombatAbilities.Last());
            GUI.FocusControl(null);
        }
        GL.EndHorizontal();

        for (int i = 0; i < _characterProfile.CombatAbilities.Count; i++)
        {
            _characterProfile.CombatAbilities[i] = (CombatAbility)EGL.ObjectField("Current Ability", _characterProfile.CombatAbilities[i], typeof(CombatAbility), false, GL.Width(FIELD_WIDTH + 100));
        }
    }

    private void DrawSettings()
    {
        DrawDivider();
        
        GL.Label("Character profile setting");
        

        if (_settingsAreOpen)
        {
            if (GL.Button("Hide settings", GL.MaxHeight(BUTTON_HEIGHT), GL.MaxWidth(BUTTON_WIDTH)))
            {
                _settingsAreOpen = false;
                GUI.FocusControl(null);
            }
        }
        else 
        {
            if (GL.Button("Show settings", GL.MaxHeight(BUTTON_HEIGHT), GL.MaxWidth(BUTTON_WIDTH)))
            {
                _settingsAreOpen = true;
                GUI.FocusControl(null);
            }
        }
        if (_settingsAreOpen == false) return;

        Color prev = GUI.color;
        GUI.color = Color.red;
        GL.Label("Changing these changes them for all character profiles.");
        GUI.color = prev;
        GL.Space(8);

        foreach (var property in _characterProfile.Settings.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if(property.PropertyType == typeof(int))
            {
                int value = (int)property.GetValue(_characterProfile.Settings);
                GL.BeginHorizontal();
                GL.Label(AddSpaceBeforeUppercase(property.Name), GL.MaxWidth(FIELD_WIDTH));
                int newValue = EGL.IntField(value, GL.MaxWidth(60));
                GL.Label("int", GL.MaxWidth(60));
                GL.EndHorizontal();
                if(newValue != value)
                {
                    if(newValue < 0) newValue = 0;
                    property.SetValue(_characterProfile.Settings, newValue);
                    EditorUtility.SetDirty(_characterProfile.Settings);
                }
            }

            if(property.PropertyType == typeof(float))
            {
                float value = (float)property.GetValue(_characterProfile.Settings);
                GL.BeginHorizontal();
                GL.Label(AddSpaceBeforeUppercase(property.Name), GL.MaxWidth(FIELD_WIDTH));
                float newValue = EGL.FloatField(value, GL.MaxWidth(60));
                GL.Label("float", GL.MaxWidth(60));
                GL.EndHorizontal();
                if(newValue != value)
                {
                    if(newValue < 0) newValue = 0;
                    property.SetValue(_characterProfile.Settings, newValue);
                    EditorUtility.SetDirty(_characterProfile.Settings);
                }
            }

            if (property.Name.Contains("Minimal")) GL.Space(6);
        }
    }



    private void DrawDivider()
    {
        GL.Space(16);
        Rect rec = GUILayoutUtility.GetRect(_windowWidth - 40, 2, GL.ExpandWidth(false));
        EditorGUI.DrawRect(rec, new Color(0.14f, 0.14f, 0.14f, 1));
        GL.Space(2);
    }
    private void DrawPropertyUnderline()
    {
        Rect rec = GUILayoutUtility.GetRect(300, 1, GL.ExpandWidth(false));
        EditorGUI.DrawRect(rec, new Color(0.14f, 0.14f, 0.14f, 1));
    }


    private void OnDestroy()
    {
        if (_characterProfile == null) return;
        RemoveEmptyCombatAbilities();
    }
    private void RemoveEmptyCombatAbilities()
    {
        _characterProfile.CombatAbilities = _characterProfile.CombatAbilities.Where(a => a != null).ToList();
        EditorUtility.SetDirty(_characterProfile);
    }


    private StringBuilder _stringBuilder = new StringBuilder();
    private string RemoveWordFromStart(string input, string wordToRemove)
    {
        if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(wordToRemove))
            return input;
        _stringBuilder.Clear();
        _stringBuilder.Append(input);

        if (_stringBuilder.ToString().StartsWith(wordToRemove))
        {
            _stringBuilder.Remove(0, wordToRemove.Length);
        }
        return _stringBuilder.ToString();
    }
    public string AddSpaceBeforeUppercase(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        _stringBuilder.Clear();

        foreach (char c in input)
        {
            if (char.IsUpper(c))
            {
                _stringBuilder.Append(' ');
            }
            _stringBuilder.Append(c);
        }

        return _stringBuilder.ToString().TrimStart();
    }
}
