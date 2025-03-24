using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipTurnHandler : CombatAbilityEffectHandler
{
    public override CombatAbilityEffect.EffectType EffectType => CombatAbilityEffect.EffectType.SkipTurn;

    protected override void DoEffect(CombatAbilityEffect effect, Character caster, Character targetCharacter, GridTileController targetTile)
    {
        targetCharacter.SkipTurn();
    }
}
