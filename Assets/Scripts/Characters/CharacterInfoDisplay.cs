using System.Text;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(TMP_Text))]
public class CharacterInfoDisplay : MonoBehaviour
{
    public Character ReferencedCharacter;

    public string Name => ReferencedCharacter.CharacterProfile.Name;
    public int Health => ReferencedCharacter.CurrentHealth;
    public int MaxHealth => ReferencedCharacter.CharacterProfile.TotalVitality;

    public int ActionPoints => ReferencedCharacter.CurrentActionPoints;
    public int MovePoints => ReferencedCharacter.CurrentMovePoints;
    public int AmmoPoints => ReferencedCharacter.CurrentAmmo;
    public int MaxAmmoPoints => ReferencedCharacter.CharacterProfile.TotalAmmo;

    public float Accuracy => ReferencedCharacter.CharacterProfile.TotalAccuracy;
    public float WeakspotDamage => ReferencedCharacter.CharacterProfile.TotalWeakspotDamage;
    public float CriticalChance => ReferencedCharacter.CharacterProfile.TotalCritChance;
    public float CriticalDamage => ReferencedCharacter.CharacterProfile.TotalCritDamage;

    public int Armor => ReferencedCharacter.CharacterProfile.TotalArmor;
    public float Evasion => ReferencedCharacter.CharacterProfile.TotalEvasion;

    [SerializeField] private TMP_Text _infoField;


    public void UpdateReferenceCharacter(Character character)
    {
        ReferencedCharacter = character;
    }

    public void UpdateInfo()
    {
        if (_infoField == null)
        {
            Debug.LogError("CharacterInfoDisplay: _infoField is null");
            return;
        }

        if (ReferencedCharacter == null)
        {
            _infoField.text = "No character selected";
            return;
        }

        StringBuilder info = new();

        info.AppendLine($"Name: {Name}");
        info.AppendLine($"Health: {Health}/{MaxHealth}");
        info.AppendLine(string.Empty);
        info.AppendLine($"Ammo Points: {AmmoPoints}/{MaxAmmoPoints}");
        info.AppendLine($"Action Points: {ActionPoints}");
        info.AppendLine($"Move Points: {MovePoints}");
        info.AppendLine(string.Empty);
        info.AppendLine($"Accuracy: {Accuracy * 100}%");
        info.AppendLine($"Weakspot Damage: {WeakspotDamage * 100}%");
        info.AppendLine($"Critical Chance: {CriticalChance * 100}%");
        info.AppendLine($"Critical Damage: {CriticalDamage * 100}%");
        info.AppendLine(string.Empty);
        info.AppendLine($"Armor: {Armor}");
        info.AppendLine($"Evasion: {Evasion * 100}%");

        _infoField.text = info.ToString();
    }
}