using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridTileController : MonoBehaviour
{
    public GridTileData Data;

    private MeshRenderer _tileMeshRenderer;
    private bool _isHighlighted;
    private bool _usePartialHighlight;

    #region MonoBehaviour

    private void Awake()
    {
        _tileMeshRenderer = GetComponentInChildren<MeshRenderer>();
        Debug.Assert(_tileMeshRenderer != null);
    }

    private void OnMouseEnter()
    {
        if (_isHighlighted)
        {
            SetHighlight(true, false);
        }
    }

    public void OnMouseExit()
    {
        if (_isHighlighted)
        {
            SetHighlight(true, true);
        }
    }

    public void OnMouseDown()
    {
        if (_isHighlighted)
        {
            var ability = CombatManager.Instance.CurrentAbility;
            var target = Data.Coordinates;
            var map = GridManager.Instance.Grid;
            var caster = CombatManager.Instance.CurrentCombatant;

            CombatAbilityExecutor.ExecuteAbility(target, ability, map, caster);
        }
        else
        {
            Debug.Log($"Click denied");
        }
    }

    #endregion

    public void SetHighlight(bool setHighlightActive, bool usePartialHighlight = true)
    {
        _isHighlighted = setHighlightActive;
        _usePartialHighlight = usePartialHighlight;

        UpdateHighlightMode();
    }

    private void UpdateHighlightMode()
    {
        MaterialPropertyBlock propertyBlock = new();

        _tileMeshRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetFloat("_IsHighlighted", _isHighlighted ? 1 : 0);
        propertyBlock.SetFloat("_UsePartialHighlight", _usePartialHighlight ? 1 : 0);
        _tileMeshRenderer.SetPropertyBlock(propertyBlock);
    }
}
