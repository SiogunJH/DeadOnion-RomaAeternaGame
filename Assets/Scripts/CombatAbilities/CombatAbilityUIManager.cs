using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CombatAbilityUIManager : MonoBehaviour
{
    private static EventTrigger.Entry SelectEntryTrigger => new() { eventID = EventTriggerType.Select };
    private static EventTrigger.Entry DeselectEntryTrigger => new() { eventID = EventTriggerType.Deselect };

    [SerializeField] private List<CombatAbilityUIObject> _combatAbilityObjects;
    [SerializeField] private GameObject _combatAbilityUIObjectPrefab;

    [System.Serializable]
    private struct CombatAbilityUIObject
    {
        public Button Button;
        public Image Image;
        public TMP_Text Name;
        public EventTrigger EventTrigger;
    }

    public void DisplayAbilities(IEnumerable<CombatAbility> abilities)
    {
        HideAbilities();
        foreach (var ability in abilities)
        {
            AddAbilityToView(ability);
        }
    }

    public void HideAbilities()
    {
        var abilitiesToHide = _combatAbilityObjects.Where(ability => ability.Button.gameObject.activeSelf);
        foreach (var ability in abilitiesToHide)
        {
            RemoveAbilityFromView(ability);
        }
    }

    private void AddAbilityToView(CombatAbility ability)
    {
        var abilityUI = GetAbilityUIObject();

        // Prepare Visually
        abilityUI.Image.color = new(Mathf.Clamp(Random.value, 0.2f, 0.8f), Mathf.Clamp(Random.value, 0.2f, 0.8f), Mathf.Clamp(Random.value, 0.2f, 0.8f)); // TEMP
        abilityUI.Name.text = ability.Name;
        abilityUI.Button.gameObject.name = $"Combat Ability [{ability.Name.ToUpper()}]";
        abilityUI.Button.gameObject.SetActive(true);

        // Can the Ability be used
        abilityUI.Button.interactable = true;

        // Add OnSelect listener
        EventTrigger.Entry selectEntry = SelectEntryTrigger;
        void OnSelect(BaseEventData _)
        {
            // Debug.Log("Select event called");
            CombatManager.Instance.CurrentAbility = ability;
            CombatManager.Instance.HighlightTilesInRange(true);
        }
        selectEntry.callback.AddListener(OnSelect);
        abilityUI.EventTrigger.triggers.Add(selectEntry);

        // Add OnDeselect listener
        EventTrigger.Entry deselectEntry = DeselectEntryTrigger;
        void OnDeselect(BaseEventData _)
        {
            // Debug.Log("Deselect event called");
            CombatManager.Instance.HighlightTilesInRange(false);
            CombatManager.Instance.CurrentAbility = null;
        }
        deselectEntry.callback.AddListener(OnDeselect);
        abilityUI.EventTrigger.triggers.Add(deselectEntry);
    }

    private void RemoveAbilityFromView(CombatAbilityUIObject abilityUI)
    {
        // Clear previous listeners
        abilityUI.EventTrigger.triggers.RemoveAll(listener => true);

        //
        abilityUI.Button.gameObject.SetActive(false);
    }

    #region UI Components Retrieval

    private CombatAbilityUIObject GetAbilityUIObject()
    {
        // Get CombatAbilityUIObject to use
        CombatAbilityUIObject abilityUI;
        if (_combatAbilityObjects.Any(a => !a.Button.gameObject.activeSelf))
        {
            abilityUI = _combatAbilityObjects.First(a => !a.Button.gameObject.activeSelf);
        }
        else
        {
            abilityUI = CreateNewAbilityUIObject();
        }
        return abilityUI;
    }

    private CombatAbilityUIObject CreateNewAbilityUIObject()
    {
        GameObject combatAbilityObject = Instantiate(_combatAbilityUIObjectPrefab, transform);
        CombatAbilityUIObject newUIObject = new()
        {
            Button = combatAbilityObject.GetComponent<Button>(),
            Image = combatAbilityObject.GetComponent<Image>(),
            EventTrigger = combatAbilityObject.GetComponent<EventTrigger>(),
            Name = combatAbilityObject.GetComponentInChildren<TMP_Text>(),
        };

        Debug.Assert(newUIObject.Button != null, "[Button] component of newly created Combat Ability UI is [null]!");
        Debug.Assert(newUIObject.Image != null, "[Image] component of newly created Combat Ability UI is [null]!");
        Debug.Assert(newUIObject.EventTrigger != null, "[EventTrigger] component of newly created Combat Ability UI is [null]!");
        Debug.Assert(newUIObject.Name != null, "[Name] component of newly created Combat Ability UI is [null]!");

        _combatAbilityObjects.Add(newUIObject);

        return newUIObject;
    }

    #endregion

}
