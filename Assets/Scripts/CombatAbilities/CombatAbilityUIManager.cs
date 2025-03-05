using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CombatAbilityUIManager : MonoBehaviour
{
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
        // Get CombatAbilityUIObject to use
        CombatAbilityUIObject abilityUI;
        if (_combatAbilityObjects.Any(a => !a.Button.gameObject.activeSelf))
        {
            abilityUI = _combatAbilityObjects.First(a => !a.Button.gameObject.activeSelf);
        }
        else
        {
            abilityUI = CreateNewCombatAbilityUIObject();
        }

        // Set data
        abilityUI.Button.interactable = true;
        abilityUI.Image.color = new(Mathf.Clamp(Random.value, 0.2f, 0.8f), Mathf.Clamp(Random.value, 0.2f, 0.8f), Mathf.Clamp(Random.value, 0.2f, 0.8f)); // TEMP
        abilityUI.Name.text = ability.Name;

        // Add OnSelect listener
        EventTrigger.Entry selectEntry = new()
        {
            eventID = EventTriggerType.Select
        };
        void OnSelect(BaseEventData _) => CombatManager.Instance.HighlightTilesInRange(CombatManager.Instance.CurrentCombatant, ability, true);
        selectEntry.callback.AddListener(OnSelect);
        abilityUI.EventTrigger.triggers.Add(selectEntry);

        // Add OnDeselect listener
        EventTrigger.Entry deselectEntry = new()
        {
            eventID = EventTriggerType.Deselect
        };
        void OnDeselect(BaseEventData _) => CombatManager.Instance.HighlightTilesInRange(CombatManager.Instance.CurrentCombatant, ability, true);
        deselectEntry.callback.AddListener(OnDeselect);
        abilityUI.EventTrigger.triggers.Add(deselectEntry);

        // Display
        abilityUI.Button.gameObject.name = $"Combat Ability [{ability.Name}]";
        abilityUI.Button.gameObject.SetActive(true);
    }

    private void RemoveAbilityFromView(CombatAbilityUIObject abilityUI)
    {
        abilityUI.Button.gameObject.SetActive(false);
    }

    private CombatAbilityUIObject CreateNewCombatAbilityUIObject()
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
}
