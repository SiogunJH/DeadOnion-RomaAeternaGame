using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridTileController : MonoBehaviour
{
    public GridTileData Data;

    private MeshRenderer _tileMeshRenderer;
    private HighlighMode _highlightMode;

    #region MonoBehaviour

    private void Awake()
    {
        _tileMeshRenderer = GetComponentInChildren<MeshRenderer>();
        Debug.Assert(_tileMeshRenderer != null);
    }

    private void OnMouseEnter()
    {
        if (_highlightMode == HighlighMode.Valid)
        {
            SetHighlightMode(HighlighMode.Hovered);
        }
    }

    public void OnMouseExit()
    {
        if (_highlightMode == HighlighMode.Hovered)
        {
            SetHighlightMode(HighlighMode.Valid);
        }
    }

    public void OnMouseDown()
    {
        // Validate tile target
        if (_highlightMode != HighlighMode.Hovered)
        {
            Debug.Log($"Click Denied!\nTile is not Highlighted, and cannot be targeted!");
        }

        // Gather variables
        var ability = CombatManager.Instance.CurrentAbility;
        var target = Data.Coordinates;
        var map = GridManager.Instance.Grid;
        var caster = CombatManager.Instance.CurrentCombatant;

        // Check if the ability can be used
        if (!ability.CanUseAbility(caster))
        {
            Debug.Log("Click Denied!\nAbility cannot be used!");
            return;
        }

        CombatAbilityExecutor.ExecuteAbility(target, ability, map, caster);

        CombatManager.Instance.CurrentAbility = null;
        CombatManager.Instance.ClearTileHighlight();
    }

    #endregion

    public void SetHighlightMode(HighlighMode highlighMode)
    {
        _highlightMode = highlighMode;

        bool highlight = highlighMode != HighlighMode.None;
        bool usePartialHighlight = highlighMode == HighlighMode.Valid || highlighMode == HighlighMode.Invalid;
        bool animatePartialHighlight = highlighMode == HighlighMode.Valid;
        bool validHighlight = highlighMode != HighlighMode.Invalid;

        UpdateHighlightMode(highlight, usePartialHighlight, animatePartialHighlight, validHighlight);
    }

    private void UpdateHighlightMode(bool isHighlighted, bool usePartialHighlight, bool animatePartialHighlight, bool validHighlight)
    {
        MaterialPropertyBlock propertyBlock = new();

        _tileMeshRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetFloat("_IsHighlighted", isHighlighted ? 1 : 0);
        propertyBlock.SetFloat("_UsePartialHighlight", usePartialHighlight ? 1 : 0);
        propertyBlock.SetFloat("_AnimatePartialHighlight", animatePartialHighlight ? 1 : 0);
        propertyBlock.SetFloat("_ValidHighlight", validHighlight ? 1 : 0);
        _tileMeshRenderer.SetPropertyBlock(propertyBlock);
    }

    public enum HighlighMode
    {
        None = 0,
        Hovered = 1 << 0,
        Valid = 1 << 1,
        Invalid = 1 << 2,
    }
}
