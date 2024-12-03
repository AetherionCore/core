using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using System.Numerics;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using GameServerCore.Enums;

namespace Spells
{
    public class RemoveScurvy : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            owner.StopMovement();
            float ap = owner.Stats.AbilityPower.Total;
            float newHealth = target.Stats.CurrentHealth + 80 + ap;
            target.Stats.CurrentHealth = Math.Min(newHealth, target.Stats.HealthPoints.Total);
            owner.SetStatus(StatusFlags.CanAttack, true);
            owner.SetStatus(StatusFlags.CanCast, true);
            owner.SetStatus(StatusFlags.CanMove, true);
            owner.SetStatus(StatusFlags.Stunned, false);
            owner.SetStatus(StatusFlags.Rooted, false);
            owner.SetStatus(StatusFlags.Disarmed, false);
            owner.SetStatus(StatusFlags.Rooted, false);
            owner.SetStatus(StatusFlags.Suppressed, false);

            owner.Stats.SetActionState(ActionState.CAN_ATTACK, true);
            owner.Stats.SetActionState(ActionState.CAN_CAST, true);
            owner.Stats.SetActionState(ActionState.CAN_MOVE, true);

            owner.SetStatus(StatusFlags.CanMove, true);
            owner.SetStatus(StatusFlags.Rooted, false);
        }
    }
}
