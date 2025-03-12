using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHandler : CombatAbilityEffectHandler
{
    public override CombatAbilityEffect.EffectType EffectType => CombatAbilityEffect.EffectType.Move;

    protected override void DoEffect(CombatAbilityEffect effect, Character caster, Character targetCharacter, GridTileController targetTile)
    {
        Debug.Log("Trying to move");
        // affectedCharacter.MoveTo();
    }
}
