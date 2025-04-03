using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;


#if UNITY_EDITOR
using VInspector;
#endif

public class Character : GridEntity
{
    public CharacterProfile CharacterProfile => _profile;

#if UNITY_EDITOR
    [Tab("Character")]
#endif

    [SerializeField] private CharacterProfile _profile;

#if UNITY_EDITOR
    [EndTab]
#endif

    private int _currentArmor;

    public int ActionPointsLeft { get; private set; }

    private int _currentMovePoints;

    #region MonoBehaviour

    private void Awake()
    {
        _currentHealth = CharacterProfile.TotalVitality;
        _currentArmor = CharacterProfile.TotalArmor;
        UserFriendlyName = CharacterProfile.Name;
    }

    #endregion

    #region Constructors

    public Character()
    {
        LoadAttributes();
    }

    #endregion

    #region >>> Effect <<<

    private List<CombatAbilityEffect> _activeEffects = new();

    public void AddEffect(CombatAbilityEffect effect)
    {
        CombatAbilityEffect newEffect = CombatAbilityEffect.CloneDeep(effect);
        _activeEffects.Add(newEffect);
    }
    private void ExecuteActiveEffects()
    {
        _activeEffects = _activeEffects.Where(e => e.ForAdditionalTurns >= 0).ToList();
        foreach (var effect in _activeEffects)
        {
            CombatAbilityExecutor.ExecuteEffectOnCharacter(effect, GridManager.Instance.Grid, this);
            Debug.Log("Replace null with gridmap reference");
            if (effect.ForAdditionalTurns <= 0) _activeEffects.Remove(effect);
        }
    }

    public void SkipTurn()
    {
        _hadTurn = true;
    }
    public void Heal(int amount)
    {
        if (amount < 0) return;
        _currentHealth += amount;
        _currentHealth = Mathf.Min(_currentHealth, 1);
    }
    public void TakeTrueDamage(int damage)
    {
        if (damage < 0) return;

        _currentHealth -= damage;

        // uint maxDamageBlocked = (uint)Mathf.RoundToInt(damage * ArmorClassToDamageReduction(CharacterProfile.ArmorClass));
        // uint damageToHealth = (uint)damage - maxDamageBlocked;
        // _currentHealth -= (int)damageToHealth;
        // _currentArmor -= (int)maxDamageBlocked;
        // if (_currentArmor < 0)
        // {
        //    _currentHealth += _currentArmor;
        //    _currentArmor = 0;
        // }

        // Die();
    }
    public void TakeElementalDamage(int amount, CombatAbilityEffect.EffectType damageType)
    {
        TakeTrueDamage(amount);
        Debug.LogWarning("Elemental damage not yet implemented");
        TryToDie();
        return;

#pragma warning disable CS0162 // Unreachable code detected
        if (amount < 0) return;
        switch (damageType)
        {
            case CombatAbilityEffect.EffectType.DamageAcid:
            case CombatAbilityEffect.EffectType.DamageKinetic:
            case CombatAbilityEffect.EffectType.DamageEnergy:
            case CombatAbilityEffect.EffectType.DamagePlasma:
            case CombatAbilityEffect.EffectType.DamageFire:
                break;

            default:
                Debug.Log("Given EffectType is not a type of damage");
                return;
        }

        Debug.Log("Do damage here"); //don't forget about armor
                                     //die
#pragma warning restore CS0162 // Unreachable code detected
    }
    public void GainArmor(int amount)
    {
        //if (amount < 0) return;
        //_currentArmor += amount;
        //_currentHealth = Mathf.Min(_currentArmor, CharacterProfile.MaxArmor);
    }
    public void Reload()
    {
        Debug.Log("Character reloaded");
    }
    public void Move()
    {
        Debug.Log("Character Moved");
    }
    public void Interract()
    {
        Debug.Log("Character Interracted");
    }

    #endregion


    #region >>> Attributes <<<

    public enum Attribute //Enum to have a neat attribute name dropdown in tool and avoid using reflection or writing a 100 methods
    {
        None = 0,
        Strength = 1,
        Dex = 2,
        Evasion = 3
    }
    private Dictionary<Attribute, Property<int>> _attributes = new();

    public void ChangeAttribute(Attribute attribute, int value) //Called by handlers
    {
        if (attribute == Attribute.None) return;

        if (_attributes.TryGetValue(attribute, out var property))
        {
            property.Value = value;
        }
        else Debug.LogError($"{attribute} Not found on {_profile.Name}");
    }
    public void ChangeAttribute(Attribute attribute, float value) { throw new NotImplementedException(); } //Implement if attributes other than int needed

    private void LoadAttributes() //Load all attributes (after they get designed)
    {
        _attributes[Attribute.Dex] = new Property<int>(() => _vitalityModification, value => _vitalityModification = value);
    }
    private void ResetAttributeChanges()
    {
        foreach (var attribute in _attributes.Values)
        {
            attribute.Value = 0;
        }
    }

    #region > Attributes <

    private int _vitalityModification { get; set; }
    public int MaxVitality => Mathf.Max(0, _profile.TotalVitality + _vitalityModification);
    private int _currentHealth = 1;
    public int CurrentHealth => _currentHealth;

    private int _powerModification { get; set; }
    public int Power => Mathf.Max(0, _profile.TotalPower + _powerModification);

    private int _enduranceModification { get; set; }
    public int Endurance => Mathf.Max(0, _profile.TotalEndurance + _enduranceModification);

    private int _ammoModification { get; set; }
    public int MaxAmmo => Mathf.Max(0, _profile.TotalAmmo + _ammoModification);
    private int _currentAmmo = 1;
    public int CurrentAmmo => _currentAmmo;

    private int _initiativeModification { get; set; }
    public int Initiative => Mathf.Max(0, _profile.TotalInitiative + _initiativeModification);

    private int _movementSpeedModification { get; set; }
    public int MovementSpeed => Mathf.Max(0, _profile.TotalMovementSpeed + _movementSpeedModification);

    private float _accuracyModification { get; set; }
    public float Accuracy => Mathf.Max(0, _profile.TotalAccuracy + _accuracyModification);

    private float _evasionModification { get; set; }
    public float Evasion => Mathf.Max(0, _profile.TotalEvasion + _evasionModification);

    private float _critChanceModification { get; set; }
    public float CritChance => Mathf.Max(0, _profile.TotalCritChance + _critChanceModification);

    private float _critDamageModification { get; set; }
    public float CritDamage => Mathf.Max(0, _profile.TotalCritDamage + _critDamageModification);




    private float _stunResistModification { get; set; }
    public float StunResist => Mathf.Max(0, _profile.TotalStunResist + _stunResistModification);

    private float _energyResistModification { get; set; }
    public float EnergyResist => Mathf.Max(0, _profile.TotalEnergyResist + _energyResistModification);

    private float _poisonResistModification { get; set; }
    public float PoisonResist => Mathf.Max(0, _profile.TotalPoisonResist + _poisonResistModification);

    private float _burnResistModification { get; set; }
    public float BurnResist => Mathf.Max(0, _profile.TotalBurnResist + _burnResistModification);

    private float _bleedResistModification { get; set; }
    public float BleedResist => Mathf.Max(0, _profile.TotalBleedResist + _bleedResistModification);



    public int BonusDamageModifier => _profile.BonusDamageModifier;
    public float BonusDamageMultiplier => _profile.BonusDamageMultiplier;
    public float BonusStunBuildup => _profile.BonusStunBuildup;
    public float BonusPoisionBuildup => _profile.BonusPoisionBuildup;
    public float BonusBurnBuildup => _profile.BonusBurnBuildup;
    public float BonusBleedBuildup => _profile.BonusBleedBuildup;
    public float BonusEnergyBuildup => _profile.BonusEnergyBuildup;
    public int BonusArmorDamageModifier => _profile.BonusArmorDamageModifier;
    public int BonusArmorPierce => _profile.BonusArmorPierce;

    #endregion

    #endregion


    #region >>> Turn <<<

    [SerializeField, HideInInspector] private bool _hadTurn = false;
    public bool HadTurn => _hadTurn;

    public void RemoveActionPoints(int amount)
    {
        // Validate
        Debug.Assert(ActionPointsLeft >= amount, "Cannot remove more action points than there is available"); // AP availability should be verified before performing an action

        // Remove
        ActionPointsLeft = Mathf.Clamp(ActionPointsLeft - amount, 0, int.MaxValue);
    }

    public void BeginTurn()
    {
        ResetAttributeChanges();
        ExecuteActiveEffects();

        Debug.Log($"Character '{CharacterProfile.Name} [{ID}]' has started their turn!");
        ActionPointsLeft = 2; // TODO: Assign action points from Character

        StartCoroutine(PerformTurn());
    }

    private IEnumerator PerformTurn()
    {
        Debug.Log($"Character '{CharacterProfile.Name} [{ID}]' is now performing!");

        yield return new WaitForSeconds(0.5f);

        CombatManager.Instance.UI.DisplayAbilities(CharacterProfile.CombatAbilities);
    }

    public bool TryToEndTurn()
    {
        // Validate
        if (ActionPointsLeft > 0) return false;

        EndTurn();
        return true;
    }

    private void EndTurn()
    {
        _hadTurn = true;

        Debug.Log($"Character '{CharacterProfile.Name} [{ID}]' has finished their turn!");

        EventSystem.current.SetSelectedGameObject(null);
        CombatManager.Instance.UI.HideAbilities();
        CombatManager.Instance.NextTurn();
    }

    public void ResetTurn()
    {
        _hadTurn = false;
        _currentMovePoints = CharacterProfile.TotalMovementSpeed;
        _currentMovePoints = 1;
    }

    #endregion

    private bool TryToDie()
    {
        if (CurrentHealth <= 0)
        {
            Die();
            return true;
        }

        return false;
    }

    private void Die()
    {
        Debug.Log($"Character '{CharacterProfile.Name} [{ID}]' has died");

        CombatManager.Instance.RemoveCharacter(this); // Remove from turn order
        Location.RemoveOccupant(this); // Remove from tile

        // Remove visually
        Destroy(gameObject); // TODO: In the future development, this should be replaced by a call to animation controller for death animation
        //Animation controller is beeing done but i will still not do it. LOL
    }

    #region >>> Animation <<<

    [SerializeField]
    private GameObject _billboard;
    public GameObject Billboard => _billboard;

    [SerializeField]
    private Sprite _hurtSprite = null;
    public Sprite HurtSprite => _hurtSprite;

    public void OnAnimationFinish() //add this to event in animation player
    {

    }

    #endregion
}
